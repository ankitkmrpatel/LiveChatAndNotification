using Microsoft.EntityFrameworkCore;
using NotificationBackendService.Data;

namespace NotificationBackendService.Extentions.ServiceCollection;

public static class DatabaseExtentions
{
    public static void AddSqlLiteDatabase(this IServiceCollection services)
    {
        services.AddDbContext<AppDataContext>(optionsBuilder =>
        {
            optionsBuilder.UseSqlite("Filename=Data/Database/NotifSystem.db", options =>
            {
                options.MigrationsAssembly(System.Reflection.Assembly.GetExecutingAssembly().FullName);
            });
        });
    }

    public static void PrepDbPopulation(this IApplicationBuilder app)
    {
        PrepData.PrepPopulation(app);
    }
}
