using System;
using System.Linq;
using AutoMapper;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NetCore.WebApi.Data;
using NetCore.WebApi.Services;
using Newtonsoft.Json.Serialization;

namespace NetCore.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            //第三方缓存库

            services.AddHttpCacheHeaders(expires =>
            {
                expires.MaxAge = 30;
                expires.CacheLocation = CacheLocation.Public;
            }, validation =>
            {
                validation.MustRevalidate = true;
            });
            services.AddResponseCaching();
            services.AddControllers(set=>
                {
                    set.ReturnHttpNotAcceptable = true;
                    set.CacheProfiles.Add("CacheProfile100", new CacheProfile()
                    {
                        Duration = 100
                    });
                    // set.OutputFormatters.Add(new XmlSerializerOutputFormatter());
                    //set.OutputFormatters.Insert(0,new XmlSerializerOutputFormatter());下标为0，即设为默认
                }).AddNewtonsoftJson(op =>
                {
                    op.SerializerSettings.ContractResolver=new CamelCasePropertyNamesContractResolver();
                })
                .AddXmlDataContractSerializerFormatters()
                //自定义错误报告
                .ConfigureApiBehaviorOptions(option =>
                {
                    option.InvalidModelStateResponseFactory = context =>
                    {
                        var problem=new ValidationProblemDetails(context.ModelState)
                        {
                            Type = "http://www.baidu.com",
                            Title = "error",
                            Status = StatusCodes.Status422UnprocessableEntity,
                            Detail = "具体信息",
                            Instance = context.HttpContext.Request.Path
                        };

                        problem.Extensions.Add("traceId",context.HttpContext.TraceIdentifier);

                        return new UnprocessableEntityObjectResult(problem)
                        {
                            ContentTypes = {"application/problem+json"}
                        };
                    };

                });
            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigins",
                    builder =>
                    {
                        builder.WithOrigins("http://www.2504511.xyz").AllowAnyHeader().AllowCredentials();
                    });
            });
            //由于会和NewtonsoftJson产生冲突，故通过配置进行修改
            services.Configure<MvcOptions>(op =>
            {
                var outPutForMatter = op.OutputFormatters
                    .OfType<NewtonsoftJsonOutputFormatter>()?.FirstOrDefault();

                outPutForMatter?.SupportedMediaTypes.Add("application/vnd.company.hateoas+json");

            });

            //查找autoMapper配置文件
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddDbContext<DemoContext>(option => option.UseMySQL(
                Configuration.GetConnectionString("Default")
            ));

            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddTransient<IPropertyMappingService, PropertyMappingService>();
            services.AddTransient<IPropertyCheckServices, PropertyCheckServices>();

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            #region 异常处理

            //生产环境下发生异常时处理
            //else
            //{
            //    app.UseExceptionHandler(appBuilder =>
            //    {
            //        appBuilder.Run(async context =>
            //        {
            //            context.Response.StatusCode = 500;
            //            await context.Response.WriteAsync("error 500");
            //        });
            //    });
            //}
            #endregion

            //app.UseResponseCaching();//需放在routing前

            app.UseHttpCacheHeaders();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("AllowSpecificOrigins");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
