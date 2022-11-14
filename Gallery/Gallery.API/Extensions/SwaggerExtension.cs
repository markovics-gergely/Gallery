﻿using System.Reflection;

using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace Gallery.API.Extensions
{
    /// <summary>
    /// Manage openapi settings of the application
    /// </summary>
    public static class SwaggerExtension
    {
        /// <summary>
        /// Add swagger conigurations to the application
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void AddSwaggerExtension(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = configuration.GetValue<string>("Api:Description"), Version = "v1" });

                options.AddSecurityDefinition(configuration.GetValue<string>("IdentityServer:SecurityScheme"), new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        Password = new OpenApiOAuthFlow()
                        {
                            TokenUrl = new Uri(configuration.GetValue<string>("IdentityServer:Authority") + "/connect/token"),
                            Scopes = new Dictionary<string, string> {
                                { configuration.GetValue<string>("Api:ApiResource:Name"),
                                    configuration.GetValue<string>("Api:ApiResource:Description") }
                            }
                        }
                    },
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Scheme = "Bearer"
                });
                options.OperationFilter<AuthorizeSwaggerOperationFilter>(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme()
                        {
                            Reference = new OpenApiReference()
                            {
                                Id = configuration.GetValue<string>("IdentityServer:SecurityScheme"),
                                Type = ReferenceType.SecurityScheme
                            }
                        },
                        new List<string>(){ configuration.GetValue<string>("Api:Name") }
                    }
                });
                options.OperationFilter<SwaggerFileOperationFilter>();

                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });
        }
    }

    /// <summary>
    /// Add authorization filter to swagger
    /// </summary>
    public class AuthorizeSwaggerOperationFilter : IOperationFilter
    {
        private readonly OpenApiSecurityRequirement requirement;

        /// <summary>
        /// Inject requirement to the filter
        /// </summary>
        /// <param name="requirement"></param>
        public AuthorizeSwaggerOperationFilter(OpenApiSecurityRequirement requirement)
        {
            this.requirement = requirement;
        }

        /// <summary>
        /// Apply authorization configurations
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="context"></param>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (context.MethodInfo.GetCustomAttribute<AuthorizeAttribute>() != null ||
                context.MethodInfo.DeclaringType?.GetCustomAttribute<AuthorizeAttribute>() != null)
            {
                operation.Security.Add(requirement);
                operation.Responses.TryAdd("401", new OpenApiResponse() { Description = "Unauthorized" });
                operation.Responses.TryAdd("403", new OpenApiResponse() { Description = "Forbidden" });
            }
        }
    }

    /// <summary>
    /// Add file upload filter to swagger
    /// </summary>
    public class SwaggerFileOperationFilter : IOperationFilter
    {
        /// <summary>
        /// Apply file configurations
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="context"></param>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var fileUploadMime = "multipart/form-data";
            if (operation.RequestBody == null || !operation.RequestBody.Content.Any(x => x.Key.Equals(fileUploadMime, StringComparison.InvariantCultureIgnoreCase)))
                return;

            var fileParams = context.MethodInfo.GetParameters().Where(p => p.ParameterType == typeof(IFormFile));
            operation.RequestBody.Content[fileUploadMime].Schema.Properties =
                fileParams.ToDictionary(k => k.Name, v => new OpenApiSchema()
                {
                    Type = "string",
                    Format = "binary"
                });
        }
    }
}