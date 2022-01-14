using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Linq;
using TLPokemon.Application;
using TLPokemon.Application.DTOs.Health;
using TLPokemon.Application.Interfaces;
using TLPokemon.Application.Interfaces.Apis;
using TLPokemon.Infrastructure.Persistence;
using TLPokemon.Infrastructure.Persistence.HttpClients;
using TLPokemon.Infrastructure.Shared;
using TLPokemon.WebApi.Extensions;
namespace TLPokemon.WebApi
{
    public class Startup
    {
        public IConfiguration _config { get; }
        public Startup(IConfiguration configuration)
        {
            _config = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplicationLayer();
            services.AddPersistenceInfrastructure(_config);
            services.AddHttpClient<IPokemonApiClient, PokemonApiClient>();
            services.AddHttpClient<IShakespeareApiClient, ShakespeareApiClient>();
            services.AddHttpClient<IYodaApiClient, YodaApiClient>();
            services.AddMemoryCache();
            services.AddSharedInfrastructure(_config);
            services.AddSwaggerExtension();
            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddControllers();
            services.AddApiVersioningExtension();
            services.AddHealthChecks().AddUrlGroup(new Uri(_config["ApiSettings:PokemonApiUrl"]), name: "PokemonApiUrl");
            services.AddHealthChecks().AddUrlGroup(new Uri(_config["ApiSettings:ShakespeareApiUrl"]), name: "ShakespeareApiUrl");
            services.AddHealthChecks().AddUrlGroup(new Uri(_config["ApiSettings:YodaApiUrl"]), name: "YodaApiUrl");
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseSwaggerExtension();
            app.UseErrorHandlingMiddleware();
            app.UseHealthChecks("/health", new HealthCheckOptions
            {
                ResponseWriter = async (context, report) =>
                {
                    context.Response.ContentType = "application/json";
                    var response = new HealthCheckResponses
                    {
                        Status = report.Status.ToString(),
                        HealthChecks = report.Entries.Select(x => new HealthCheckResponse
                        {
                            Component = x.Key,
                            Status = x.Value.Status.ToString()
                        }),
                        HealthCheckDuration = report.TotalDuration
                    };
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
                }
            });
            app.UseEndpoints(endpoints =>
             {
                 endpoints.MapControllers();
             });
        }
    }
}
