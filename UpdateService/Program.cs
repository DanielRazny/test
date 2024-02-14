using UpdateService.Options;
using UpdateService.Services;
using UpdateService.Services.Cache;
using UpdateService.Services.Database;
using UpdateService.Services.FileSystem;
using UpdateService.Services.VersionChecker;

namespace UpdateService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            // ToDo put options into extension
            //builder.Services.AddOptions();
            builder.Services.Configure<CacheOptions>(builder.Configuration.GetSection("Cache"));
            builder.Services.Configure<DatabaseOptions>(builder.Configuration.GetSection("Database"));
            builder.Services.Configure<FileSystemOptions>(builder.Configuration.GetSection("FileSystem"));
            builder.Services.Configure<SwitchOptions>(builder.Configuration.GetSection("Switch"));

            builder.Services.AddSingleton<IVersionCompareHandler, VersionCompareHandler>();
            builder.Services.AddSingleton<IDatabaseHandler, DatabaseHandler>();
            builder.Services.AddSingleton<IFileSystemHandler, FileSystemHandler>();
            builder.Services.AddSingleton<IApplicationVersionHandler, ApplicationVersionHandler>();
            builder.Services.AddSingleton<IApplicationVersionCache, ApplicationVersionCache>();

            builder.Services.AddHostedService<HostedService>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
