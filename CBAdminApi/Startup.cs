using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Entitys;
using Entitys.DB;
using Microsoft.EntityFrameworkCore;
using EntityRepository.Context;
using AuthService.Jwt;
using AvastInfrastructureRepository.SwaggerLogging.Middlewares.Extensions;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AvastInfrastructureRepository.SwaggerLogging.Enums;

namespace CBAdminApi
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
            services.AddControllers();
            services.AddMvc();
            services.AddEntityFrameworkOracle().AddDbContext<DataContext>(options =>
            {
                options.UseLazyLoadingProxies().UseOracle(dbConfig.ConnectionString);
            });
            services.AddSingleton<IDbContext>(provider =>
            {
                var aa = provider.GetService<DataContext>();
                return aa;
            });
            services.AddTransient<IDbContext, DataContext>();

            JwtAuthConfiguration.ConfigureJwtAuthService(services);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.XML";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            services.AddSwaggerHttpContextAccessor(xmlPath);

            var builder = new ContainerBuilder();

            AuthService.Start.Builder(builder);
            AdminService.Start.Builder(builder);
            MainsService.Start.Builder(builder);

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
            app.UseOpenApi();
            //SwaggerBuilderExtensions.UseSwagger(app);
            app.UseCors(builder => builder
                .WithOrigins("*")
                .AllowAnyMethod()
                .AllowAnyHeader());
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();

            //app.UseRouting();

            //app.UseAuthorization();

            app.UseSwaggerHttpContextAccessor();
            app.UseRequestResponseLogging(HttpMethodDirection.ALL);
        }
    }
}
