using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NLog;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IO;
using System.Linq;
using System.Text;
using VersionManagement.BusinessLogics;
using VersionManagement.Dtos;
using VersionManagement.Repositories;

namespace VersionManagement
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IVersionRepository, VersionRepository>();
            services.AddScoped<IVersionLogic, VersionLogic>();
            services.AddDbContextPool<VersionContext>(options =>
            {
                options.UseLazyLoadingProxies();
                options.UseMySql(Configuration.GetConnectionString("Version"));
            }
            );

            services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ApiVersionReader = new HeaderApiVersionReader("api-version");
            });

            services.AddCors(options =>
            {
                options.AddPolicy(
                    "TestOrigin",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .AllowAnyMethod());
            });

            services.AddResponseCaching(options =>
            {
                options.UseCaseSensitivePaths = true;
                options.MaximumBodySize = 1024;
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSwaggerGen(c =>
            {

                c.SwaggerDoc("v1", new Info { Title = "Version Control Manager", Version = "v1" });
                //var xmlFile = $"{Assembly.GetEntryAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, ".xml");
                c.IncludeXmlComments(xmlPath);
                c.ResolveConflictingActions(apiDesc => apiDesc.Last());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("TestOrigin");

            app.UseExceptionHandler(errorApp => // Universe exception handling.
            {
                errorApp.Run(async context =>   // Add the middleware to handle exception.
                {
                    context.Response.StatusCode = 500;
                    context.Response.ContentType = "application/json";

                    var error = context.Features.Get<IExceptionHandlerFeature>();   // Use the feature to get exception info.
                    if (error != null)
                    {
                        var ex = error.Error;
                        var logger = LogManager.GetCurrentClassLogger();
                        logger.Error(ex, "Internal server error.");

                        await context.Response.WriteAsync((new ErrorDto()
                        {
                            Code = 500,
                            Message = $"Internal server error.{ex.GetBaseException().Message}"
                        }).ToString(), Encoding.UTF8);
                    }
                });
            });

            app.UseResponseCaching();
            app.Use(async (context, next) =>
            {
                context.Response.GetTypedHeaders().CacheControl =
                new Microsoft.Net.Http.Headers.CacheControlHeaderValue()
                {
                    Public = true,
                    MaxAge = TimeSpan.FromSeconds(30)
                };
                context.Response.Headers[Microsoft.Net.Http.Headers.HeaderNames.Vary] = new string[] { "Accept-Encoding" };

                await next();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Version Management server API");
            });

            app.UseMvc();
        }
    }
}
