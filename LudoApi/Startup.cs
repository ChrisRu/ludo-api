using LudoApi.Hubs;
using LudoApi.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LudoApi
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddTransient<IGameService, GameService>()
                .AddSingleton<ILobbyService, LobbyService>();

            services
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            
            services.AddSignalR();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment environment)
        {
            if (environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            if (environment.IsProduction() || environment.IsStaging())
            {
                app.UseHsts();
                app.UseHttpsRedirection();
            }

            app.UseCors(corsPolicyBuilder =>
            {
                corsPolicyBuilder
                    .WithOrigins("*")
                    .AllowAnyHeader()
                    .WithMethods("GET", "POST")
                    .AllowCredentials();
            });

            app.UseEndpoints(endpointRouteBuilder =>
            {
                endpointRouteBuilder.MapHub<GameHub>("/");
            });
        }
    }
}