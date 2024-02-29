using Microsoft.Extensions.Caching.Memory;
using static System.Net.Mime.MediaTypeNames;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Xml.Linq;

namespace NotificationBackendService.Middleware;

public class CssMinificationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<CssMinificationMiddleware> _logger;
    private readonly IWebHostEnvironment _env;
    private readonly IMemoryCache _memoryCache;

    private static readonly string[] suffixes = new string[] 
    {
        ".css",
    };

    public CssMinificationMiddleware(RequestDelegate next, IWebHostEnvironment env, ILogger<CssMinificationMiddleware> logger, IMemoryCache memoryCache)
    {
        _next = next;
        _env = env;
        _logger = logger;
        _memoryCache = memoryCache;
    }

    public async Task Invoke(HttpContext context)
    {
        var path = context.Request.Path;

        // hand to next middleware if we are not dealing with an image
        if (!IsCssPath(path) || string.IsNullOrEmpty(path.Value))
        {
            await _next.Invoke(context);
            return;
        }

        // if we got this far, resize it
        // get the image location on disk
        var cssPath = Path.Combine(_env.WebRootPath,
            path.Value.Replace('/', Path.DirectorySeparatorChar).TrimStart(Path.DirectorySeparatorChar));

        // check file lastwrite
        var lastWriteTimeUtc = File.GetLastWriteTimeUtc(cssPath);
        if (lastWriteTimeUtc.Year == 1601) // file doesn't exist, pass to next middleware
        {
            await _next.Invoke(context);
            return;
        }

        var cssData = GetMinifiedCssData(cssPath, lastWriteTimeUtc);

        // write to stream
        context.Response.ContentType = "text/css";
        context.Response.ContentLength = cssData.Length;
        await context.Response.Body.WriteAsync(cssData.ToArray().AsMemory(0, (int)cssData.Length));
    }

    private byte[] GetMinifiedCssData(string cssPath, DateTime lastWriteTimeUtc)
    {
        CSSMinify minify = new(cssPath);
        return minify.GetMinifiedCss();
    }

    private bool IsCssPath(PathString path)
    {
        if (path == null || !path.HasValue)
            return false;

        return suffixes.Any(x => x.EndsWith(x, StringComparison.OrdinalIgnoreCase));
    }
}

public static class CssMinificationMiddlewareExtensions
{
    public static IServiceCollection AddCssMinification(this IServiceCollection services)
    {
        return services.AddMemoryCache();
    }

    public static IApplicationBuilder UseCssMinification(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<CssMinificationMiddleware>();
    }
}

public class CSSMinify
{
    private readonly string fileName = "";              // file to process
    private readonly string originalData = "";           // data from original file
    private string modifiedData = "";          // processed data
    private const int EOF = -1;                 // for end of file
    private readonly BinaryReader mReader;

    /// <summary>
    /// Constructor - does all the processing
    /// </summary>
    /// <param name="filePath">file path</param>
    public CSSMinify(string filePath)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException(filePath);

        fileName = filePath;

        //read contents completely. This is only for test purposes. The actual processing is done by another stream
        using (StreamReader rdr = new(fileName))
            originalData = rdr.ReadToEnd();

        using (mReader = new(new FileStream(fileName, FileMode.Open)))
            doProcess();

        //write modified data
        string outFile = fileName.Replace(".css", ".min.css");
        using StreamWriter wrt = new(outFile);
        wrt.Write(modifiedData);
    }

    public byte[] GetMinifiedCss()
    {
        return Encoding.ASCII.GetBytes(modifiedData);
    }

    /// <summary>
    /// Main process
    /// </summary>
    private void doProcess()
    {
        int lastChar = 1;                   // current byte read
        int thisChar = -1;                  // previous byte read
        int nextChar = -1;                  // byte read in peek()
        bool endProcess = false;            // loop control
        bool ignore = false;                // if false then add byte to final output
        bool inComment = false;             // true when current bytes are part of a comment
        bool isDoubleSlashComment = false;  // '//' comment


        // main processing loop
        while (!endProcess)
        {
            endProcess = (mReader.PeekChar() == -1);    // check for EOF before reading
            if (endProcess)
                break;

            ignore = false;
            thisChar = mReader.ReadByte();

            if (thisChar == '\t')
                thisChar = ' ';
            else if (thisChar == '\t')
                thisChar = '\n';
            else if (thisChar == '\r')
                thisChar = '\n';

            if (thisChar == '\n')
                ignore = true;

            if (thisChar == ' ')
            {
                if ((lastChar == ' ') || isDelimiter(lastChar) == 1)
                    ignore = true;
                else
                {
                    endProcess = (mReader.PeekChar() == -1); // check for EOF
                    if (!endProcess)
                    {
                        nextChar = mReader.PeekChar();
                        if (isDelimiter(nextChar) == 1)
                            ignore = true;
                    }
                }
            }


            if (thisChar == '/')
            {
                nextChar = mReader.PeekChar();
                if (nextChar == '/' || nextChar == '*')
                {
                    ignore = true;
                    inComment = true;
                    if (nextChar == '/')
                        isDoubleSlashComment = true;
                    else
                        isDoubleSlashComment = false;
                }
                if (nextChar == '/')
                {
                    int x = 0;
                    x = x + 1;
                }

            }

            // ignore all characters till we reach end of comment
            if (inComment)
            {
                while (true)
                {
                    thisChar = mReader.ReadByte();
                    if (thisChar == '*')
                    {
                        nextChar = mReader.PeekChar();
                        if (nextChar == '/')
                        {
                            thisChar = mReader.ReadByte();
                            inComment = false;
                            break;
                        }
                    }
                    if (isDoubleSlashComment && thisChar == '\n')
                    {
                        inComment = false;
                        break;
                    }

                } // while (true)
                ignore = true;
            } // if (inComment) 


            if (!ignore)
                addToOutput(thisChar);

            lastChar = thisChar;
        } // while (!endProcess) 
    }

    /// <summary>
    /// Add character to modified data string
    /// </summary>
    /// <param name="c">char to add</param>
    private void addToOutput(int c)
    {
        modifiedData += (char)c;
    }


    /// <summary>
    /// Original data from file
    /// </summary>
    /// <returns></returns>
    public string getOriginalData()
    {
        return originalData;
    }

    /// <summary>
    /// Modified data after processing
    /// </summary>
    /// <returns></returns>
    public string getModifiedData()
    {
        return modifiedData;
    }

    /// <summary>
    /// Check if a byte is alphanumeric
    /// </summary>
    /// <param name="c">byte to check</param>
    /// <returns>retval - 1 if yes. else 0</returns>
    private int isAlphanumeric(int c)
    {
        int retval = 0;

        if ((c >= 'a' && c <= 'z') ||
            (c >= '0' && c <= '9') ||
            (c >= 'A' && c <= 'Z') ||
            c == '_' || c == '$' || c == '\\' || c > 126)
            retval = 1;

        return retval;

    }

    /// <summary>
    /// Check if a byte is a delimiter 
    /// </summary>
    /// <param name="c">byte to check</param>
    /// <returns>retval - 1 if yes. else 0</returns>
    private int isDelimiter(int c)
    {
        int retval = 0;

        if (c == '(' || c == ',' || c == '=' || c == ':' ||
            c == '[' || c == '!' || c == '&' || c == '|' ||
            c == '?' || c == '+' || c == '-' || c == '~' ||
            c == '*' || c == '/' || c == '{' || c == '\n' ||
            c == ';'
        )
        {
            retval = 1;
        }

        return retval;

    }
}
