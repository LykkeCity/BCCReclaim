using Autofac;
using Autofac.Extensions.DependencyInjection;
using BCCReclaimApi.Binder;
using BCCReclaimApi.Filters;
using BCCReclaimApi.Middleware;
using Core.Repositories.Settings;
using Core.Settings;
using LkeServices.Triggers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using SqlliteRepositories;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IO;
using System.Reflection;

namespace BCCReclaimApi
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
               .SetBasePath(env.ContentRootPath)
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
               .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
               .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public IServiceProvider ConfigureServices(IServiceCollection services)
        //public void ConfigureServices(IServiceCollection services)
        {
            var settings = new BaseSettings();

            services.AddMvc(o =>
            {
                o.Filters.Add(new HandleAllExceptionsFilterFactory());
            });

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info { Title = "BCC Reclaim API", Version = "v1" });
                options.DescribeAllEnumsAsStrings();

                //Determine base path for the application.
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;

                //Set the comments path for the swagger json and ui.
                var xmlPath = Path.Combine(basePath, "BCCReclaimApi.xml");
                options.IncludeXmlComments(xmlPath);
            });

            var builder = new SqlliteBinder().Bind(settings);
            builder.Populate(services);
            //builder.RegisterControllers(typeof(MvcApplication).Assembly);

            settings = GeneralSettingsReader.ReadGeneralSettings<BaseSettings>(Configuration,
                "BCCReclaimSettings");
            builder.RegisterInstance(settings);
            builder.RegisterType<ConfigurationSettingsRepository>().As<ISettingsRepository>();
            

            var container = builder.Build();
            var triggerHost = new TriggerHost(new AutofacServiceProvider(container));
            triggerHost.ProvideAssembly(GetType().GetTypeInfo().Assembly);
            triggerHost.StartAndBlock();

            return new AutofacServiceProvider(container);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseMiddleware<GlobalErrorHandlerMiddleware>();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "BCCReclaim API V1");
            });
        }
    }
}
