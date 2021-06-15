using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using hefesto.base_hefesto.Services;
using hefesto.admin.Models;
using hefesto.admin.Services;
using Microsoft.EntityFrameworkCore;
using hefesto_dotnet_graphql.GraphQL;

namespace hefesto_dotnet_graphql
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var connection = Configuration.GetConnectionString("HefestoDatabase");
            services.AddDbContextPool<dbhefestoContext>(options => options.UseNpgsql(connection));

            /*
            services.AddScoped<IAdmMenuService, AdmMenuService>();
            services.AddScoped<IAdmUserProfileService, AdmUserProfileService>();
            services.AddScoped<IAdmUserService, AdmUserService>();
            services.AddScoped<IAdmPageProfileService, AdmPageProfileService>();
            services.AddScoped<IAdmPageService, AdmPageService>();
            services.AddScoped<IAdmParameterService, AdmParameterService>();
            services.AddScoped<IAdmParameterCategoryService, AdmParameterCategoryService>();
            services.AddScoped<IAdmProfileService, AdmProfileService>();
            services.AddScoped<IChangePasswordService, ChangePasswordService>();
            services.AddScoped<ISystemService, SystemService>();
            */

            services.AddGraphQLServer().AddQueryType<Query>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGraphQL();
            });
        }
    }
}
