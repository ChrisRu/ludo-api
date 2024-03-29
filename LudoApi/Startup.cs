﻿using LudoApi.Hubs;
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

            services.AddCors(
                o => o.AddPolicy(
                    "DefaultPolicy",
                    builder =>
                    {
                        builder.AllowAnyMethod()
                               .AllowAnyHeader()
                               .WithOrigins(
                                   "http://localhost:8080",
                                   "https://localhost:8080",
                                   "https://better-ludo.netlify.com")
                               .AllowCredentials();
                    })
                );

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
            app.UseCors("DefaultPolicy");

            app.UseEndpoints(endpointRouteBuilder =>
            {
                endpointRouteBuilder.MapControllerRoute("default", "{controller=Error}/{action=Index}/{id?}");
                endpointRouteBuilder.MapHub<GameHub>("/game");
            });
        }
    }
}