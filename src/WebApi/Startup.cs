using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Interfaces;
using Infrastructure.Identity;
using Infrastructure.Services.AuthService;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using WebApi.Constants;

namespace WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public static IConfiguration Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppIdentityDbContext>(options =>
                options.UseNpgsql(connectionString: Configuration.GetConnectionString("IdentityConnection")
                ));
            services.AddTransient<Migrator>();
            services.AddTransient<JwtTokenHelper>(services => new JwtTokenHelper(
                issuer: Startup.Configuration[StartupKeys.JWT_ISSUER],
                audience: Startup.Configuration[StartupKeys.JWT_AUDIENCE],
                secureKey: Startup.Configuration[StartupKeys.JWT_SECURE_KEY],
                lifeTime: Startup.Configuration[StartupKeys.JWT_LIFE_TIME_KEY],
                userManager: services.GetRequiredService<UserManager<ApplicationUser>>()
            ));
            services.AddTransient<IAuthService, AuthService>();

            services.AddControllers();
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo {Title = "WebApi", Version = "v1"}); });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}