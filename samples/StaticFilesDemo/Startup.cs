using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.FileProviders;
namespace StaticFilesDemo
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
        const int BufferSize = 64 * 1024;
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();


            //app.UseStaticFiles(new StaticFileOptions
            //{
            //    RequestPath = "/files",
            //    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "file"))
            //});

            //app.UseDirectoryBrowser();
            //app.UseDefaultFiles();
            //app.UseFileServer();



            app.MapWhen(context =>
            {
                return !context.Request.Path.Value.StartsWith("/api");
            }, appBuilder =>
            {
                var option = new RewriteOptions();
                option.AddRewrite(".*", "/index.html", true);
                appBuilder.UseRewriter(option);

                appBuilder.UseStaticFiles();
                //appBuilder.Run(async c =>
                //{
                //    var file = env.WebRootFileProvider.GetFileInfo("index.html");

                //    c.Response.ContentType = "text/html";
                //    using (var fileStream = new FileStream(file.PhysicalPath, FileMode.Open, FileAccess.Read))
                //    {
                //        await StreamCopyOperation.CopyToAsync(fileStream, c.Response.Body, null, BufferSize, c.RequestAborted);
                //    }
                //});
            });



            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
