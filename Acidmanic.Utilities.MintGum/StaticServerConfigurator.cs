using System.Reflection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging.Abstractions;

namespace Acidmanic.Utilities.MintGum
{
    public class StaticServerConfigurator
    {

        public static readonly string DefaultServingDirectoryName = "front-end";
        public static readonly string DefaultDefaultPageFileName = "Index.html";
        
        
        public string ServingDirectoryName { get; private set; }
        public string ServingDirectoryPath { get; private set; } = string.Empty;

        public string DefaultPageFileName { get; private set; }

        public string DefaultPageFilePath { get; private set; } = string.Empty;
        

        private bool _serveForAngular = false;


        private ILogger _logger = NullLogger.Instance;

        public StaticServerConfigurator(string servingDirectoryName, string defaultPageFileName)
        {
            ServingDirectoryName = servingDirectoryName;
            DefaultPageFileName = defaultPageFileName;
            InitializePaths(new DirectoryInfo(".").FullName);
        }


        public StaticServerConfigurator(string servingDirectoryName) : this(servingDirectoryName, DefaultDefaultPageFileName)
        {
        }

        public StaticServerConfigurator() : this(DefaultServingDirectoryName)
        {
        }

        public StaticServerConfigurator UseLogger(ILogger logger)
        {
            _logger = logger;
            return this;
        }

        public StaticServerConfigurator ServeForAngular()
        {
            _serveForAngular = true;

            return this;
        }

        private void InitializePaths(string contentRootPath)
        {
            ServingDirectoryPath = Path.Combine(contentRootPath, ServingDirectoryName);

            DefaultPageFilePath = Path.Combine(ServingDirectoryPath, DefaultPageFileName);
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
        
        public static WebApplicationBuilder CreateApplicationBuilderOnBinariesSpot()
        {
            var builder = WebApplication.CreateBuilder(new WebApplicationOptions
            {
                ContentRootPath = ExecutableBinariesDirectory()
            });

            return builder;
        }
        
        public void ConfigurePreRouting(IApplicationBuilder app, IHostEnvironment env)
        {
            InitializePaths(env.ContentRootPath);

            if (!Directory.Exists(ServingDirectoryPath))
            {
                Directory.CreateDirectory(ServingDirectoryPath);


                File.WriteAllText(DefaultPageFilePath,
                    "<H1>Hello!, Apparently I'm being Updated! will be back soon! :D </H1>");
            }

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

            if (_serveForAngular)
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