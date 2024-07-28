using System.Reflection;
using Acidmanic.Utilities.MintGum.Configuration;
using Microsoft.Extensions.FileProviders;

namespace Acidmanic.Utilities.MintGum
{
    internal class MintGum
    {
        public static readonly string DefaultDefaultFileContent =
            "<H1>Hello!, Apparently I'm being Updated! will be back soon! :D </H1>";

        public IMintGumConfiguration Configuration { get; }

        private readonly ILogger _logger;
        public string ServingDirectoryPath { get; private set; } = string.Empty;

        public string DefaultPageFilePath { get; private set; } = string.Empty;


        public MintGum(IMintGumConfiguration configuration, ILogger logger)
        {
            Configuration = configuration;
            _logger = logger;
        }


        private void InitializePaths(string contentRootPath)
        {
            ServingDirectoryPath = Path.Combine(contentRootPath, Configuration.ServingDirectoryName);

            DefaultPageFilePath = Path.Combine(ServingDirectoryPath, Configuration.DefaultPageFileName);
        }

        private static string ExecutableBinariesDirectory()
        {
            var assemblyLocation = Assembly.GetEntryAssembly()?.Location;

            if (!string.IsNullOrEmpty(assemblyLocation))
            {
                var directory = new FileInfo(assemblyLocation).Directory?.FullName;

                if (!string.IsNullOrEmpty(directory))
                {
                    return directory;
                }
            }

            return new DirectoryInfo(".").FullName;
        }


        public void CreateDefaultIndexFile(string? content = null)
        {
            if (!File.Exists(DefaultPageFilePath))
            {
                File.WriteAllText(DefaultPageFilePath,
                    content ?? DefaultDefaultFileContent);
            }
        }

        public void ConfigurePreRouting(IApplicationBuilder app, IHostEnvironment env)
        {
            InitializePaths(env.ContentRootPath);

            if (!Directory.Exists(ServingDirectoryPath))
            {
                Directory.CreateDirectory(ServingDirectoryPath);
            }

            CreateDefaultIndexFile();

            _logger.LogInformation("Static Pages Root: {Directory}", ServingDirectoryPath);

            var fileProvider = new PhysicalFileProvider(ServingDirectoryPath);

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = fileProvider,
                RequestPath = "",
                ServeUnknownFileTypes = true
            });
        }


        public void ConfigureMappings(IApplicationBuilder app, IHostEnvironment env)
        {
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", c => c.Response.WriteAsync(File.ReadAllText(DefaultPageFilePath)));
            });

            if (Configuration.ServesAngularSpa)
            {
                app.Use(async (context, next) =>
                {
                    await next.Invoke();

                    _logger.LogDebug("> request for {RequestUri} got response code {ResponseCode}",
                        context.Request.Path.ToString(), context.Response.StatusCode);

                    if (context.Response.StatusCode == 404)
                    {
                        context.Response.StatusCode = 200;

                        _logger.LogDebug("redirected to index file");

                        var content = await File.ReadAllTextAsync(DefaultPageFilePath);

                        await context.Response.WriteAsync(content);
                    }
                });
            }
        }
    }
}