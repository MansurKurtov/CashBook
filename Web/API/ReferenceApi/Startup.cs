using System;
using System.IO;
using System.Reflection;
using AuthService.Jwt;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AvastInfrastructureRepository.SwaggerLogging.Enums;
using AvastInfrastructureRepository.SwaggerLogging.Middlewares.Extensions;
using EntityRepository.Context;
using Entitys.DB;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ReferenceApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public Autofac.IContainer Container { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddEntityFrameworkOracle().AddDbContext<DataContext>(options =>
            {
                options.UseLazyLoadingProxies().UseOracle(Configuration.GetConnectionString("DefaultConnection"));
                // options.UseOracle(dbConfig.ConnectionString);
            });
            services.AddTransient<IDbContext>(provider =>
            {
                var aa = provider.GetService<DataContext>();
                return aa;
            });

            JwtAuthConfiguration.ConfigureJwtAuthService(services);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.XML";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            services.AddSwaggerHttpContextAccessor(xmlPath);

            var builder = new ContainerBuilder();

            AuthService.Start.Builder(builder);
            AdminService.Start.Builder(builder);
            ReferenceService.Start.Builder(builder);

            builder.Populate(services);
            Container = builder.Build();
            // MappingConfig.RegisterMaps();


            return new AutofacServiceProvider(Container);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

           // app.UseMvc();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseRouting();

            //app.UseAuthorization();

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllers();
            //});

            app.UseSwaggerHttpContextAccessor();
            app.UseRequestResponseLogging(HttpMethodDirection.ALL);
        }
    }
}
