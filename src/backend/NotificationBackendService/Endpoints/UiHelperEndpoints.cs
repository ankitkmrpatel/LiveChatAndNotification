using Microsoft.AspNetCore.Authorization;
using NotificationBackendService.Data.Entities;
using NotificationBackendService.Services;
using System.Text;

namespace NotificationBackendService.Endpoints
{
    public static class UiHelperEndpoints
    {
        public static void MapUiHelperEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("api/uiHelper/{companyCode}", GetUiHelperFile)
                   .WithName("GetUiHelperFile").WithDisplayName("Get Ui Helper File")
                   .Produces<StringContent>(StatusCodes.Status200OK)
                   .Produces(StatusCodes.Status400BadRequest)
                   .Produces(StatusCodes.Status500InternalServerError);
        }

        [AllowAnonymous]
        private static IResult GetUiHelperFile(string companyCode)
        {
            if (string.IsNullOrEmpty(companyCode))
            {
                return Results.NotFound();
            }

            if (!companyCode.Equals("SFLHub.css"))
            {
                return Results.NotFound();
            }

            var cssContent = new StringContent(".hiddenView { display: none; }", Encoding.UTF8, "text/css");
            return Results.Ok(cssContent);
        }
    }
}
