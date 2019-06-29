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
            else
            {
                app.UseExceptionHandler("/error");
                app.UseHsts();
            }

            app.UseRouting();
            app.UseCors(corsPolicyBuilder =>
            {
                corsPolicyBuilder
                    .AllowAnyHeader()
                    .WithMethods("GET", "POST")
                    .AllowCredentials();
            });

            app.UseEndpoints(endpointRouteBuilder =>
            {
                endpointRouteBuilder.MapControllerRoute("default", "{controller=Error}/{action=Index}/{id?}");
                endpointRouteBuilder.MapHub<GameHub>("/game");
            });
        }
    }
}