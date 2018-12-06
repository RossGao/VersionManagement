using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using VersionManagement.BusinessLogics;
using VersionManagement.Dtos;
using VersionManagement.Repositories;
using VersionManagement.Utils;

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

            var policyUri = Configuration.GetSection("Uri:HaoTian-Policy").Value;
            services.AddHttpClient("policyService", c =>  // Named client.
            {
                c.BaseAddress = new Uri(policyUri);
                c.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            })
                .SetHandlerLifetime(TimeSpan.FromMinutes(5)) // 一个HttpClient对应一个HttpMessageHanlder实例，这里设置handler池子中每个实例的生命周期
                .AddPolicyHandler(request => HttpClientHelper.GetTimeoutPolicy(request))
                .AddPolicyHandler(HttpClientHelper.GetRetryPolicy())
                .AddPolicyHandler(HttpClientHelper.GetCircuitBreakPolicy());

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
