using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using DependencyInjectionAutofacDemo.MyDI;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using DependencyInjectionAutofacDemo.Services;
using DependencyInjectionAutofacDemo.Controllers;
using Autofac.Extras.DynamicProxy;
using Autofac.Extensions.DependencyInjection;

namespace DependencyInjectionAutofacDemo
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

            services.AddControllers().AddControllersAsServices();
        }


        public void ConfigureContainer(ContainerBuilder builder)
        {
            //builder.RegisterType<MyService>().As<IMyService>();
            #region ÃüÃû×¢²á

            //builder.RegisterType<MyServiceV2>().Named<IMyService>("service2");
            #endregion

            #region ÊôÐÔ×¢²á
            //builder.RegisterType<MyNameService>();
            //builder.RegisterType<MyServiceV2>().As<IMyService>().PropertiesAutowired();
            #endregion

            #region AOP
            //builder.RegisterType<MyInterceptor>();
            //builder.RegisterType<MyNameService>();
            //builder.RegisterType<MyServiceV2>().As<IMyService>().PropertiesAutowired().InterceptedBy(typeof(MyInterceptor)).EnableInterfaceInterceptors();
            #endregion

            #region ×ÓÈÝÆ÷
            builder.RegisterType<MyNameService>().InstancePerMatchingLifetimeScope("myscope");
            #endregion

        }
        public ILifetimeScope AutofacContainer { get; private set; }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            this.AutofacContainer = app.ApplicationServices.GetAutofacRoot();

            //var servicenamed = this.AutofacContainer.Resolve<IMyService>();
            //servicenamed.ShowCode();


            //var service = this.AutofacContainer.ResolveNamed<IMyService>("service2");
            //service.ShowCode();

            #region ×ÓÈÝÆ÷

            using (var myscope = AutofacContainer.BeginLifetimeScope("myscope"))
            {
                var service0 = myscope.Resolve<MyNameService>();
                using (var scope = myscope.BeginLifetimeScope())
                {
                    var service1 = scope.Resolve<MyNameService>();
                    var service2 = scope.Resolve<MyNameService>();
                    Console.WriteLine($"service1=service2:{service1 == service2}");
                    Console.WriteLine($"service1=service0:{service1 == service0}");
                }
            }
            #endregion


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
