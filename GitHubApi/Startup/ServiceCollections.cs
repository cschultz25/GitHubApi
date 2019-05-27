using GitHubApi.Configuration;
using GitHubApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Reflection;

namespace GitHubApi.Startup
{
    /// <summary>
    /// Service collections class to centralize all of the configurations and dependencies for the service
    /// </summary>
    public static class ServiceCollections
    {
        /// <summary>
        /// Method to configure all http clients used by the service
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection ConfigureHttpClients(this IServiceCollection services)
        {
            services.AddHttpClient<IGitHubService, GitHubService>(c =>
            {
                c.BaseAddress = new Uri(GitHubConstants.GitHubDomain);

                c.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                //User Agent header is a required header in the request to their api
                c.DefaultRequestHeaders.UserAgent.TryParseAdd("GitHubApi-SampleRequest");

            });                

            return services;
        }

        /// <summary>
        /// Swagger configuration
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info { Title = Assembly.GetExecutingAssembly().FullName, Version = "v1" });
            });

            return services;
        }

        /// <summary>
        /// MVC Configuration
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection ConfigureMvc(this IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            return services;
        }

        /// <summary>
        /// Logging configuration
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection ConfigureLogging(this IServiceCollection services)
        {
            services.AddLogging(logger =>
            {                
                logger.AddConsole();
                logger.AddDebug();
                logger.AddEventSourceLogger();
            });

            return services;
        }
    }
}
