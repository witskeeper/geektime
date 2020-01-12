using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
namespace MiddlewareDemo
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
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Use(async (context, next) =>
            {
                //await context.Response.WriteAsync("Hello");
                await next();
                if (context.Response.HasStarted)
                {
                    //一旦已经开始输出，则不能再修改响应头的内容
                }
                await context.Response.WriteAsync("Hello2");
            });


            app.Map("/abc", abcBuilder =>
            {
                abcBuilder.Use(async (context, next) =>
                {
                    //await context.Response.WriteAsync("Hello");
                    await next();
                    await context.Response.WriteAsync("Hello2");
                });
            });


            app.MapWhen(context =>
            {
                return context.Request.Query.Keys.Contains("abc");
            }, builder =>
            {
                builder.Run(async context =>
                {
                    await context.Response.WriteAsync("new abc");
                });

            });


            app.UseMyMiddleware();


            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            #region
            //app.Use(async (context, next) =>
            //{
            //    await next();
            //    await context.Response.WriteAsync("Hi ");
            //});


            //app.UseMyMiddleware();

            //app.Run(async context =>
            //{
            //    await context.Response.WriteAsync("Hello");
            //});

            //app.Map("/abc", builder =>
            //{


            //});

            //app.MapWhen(context => context.Request.IsHttps, builder =>
            //{

            //    builder.Run(async context2 => {

            //        await context2.Response.WriteAsync("is https");

            //    });
            //});
            #endregion
        }
    }
}
