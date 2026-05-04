using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace InsightCore.WebApi.Modules.Swagger
{
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigureSwaggerOptions"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider used to resolve optional services.</param>
        public ConfigureSwaggerOptions(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

        /// <inheritdoc />
        public void Configure(SwaggerGenOptions options)
        {
            // Try to resolve the API version description provider; if it's not available, register a default v1 document
            var provider = _serviceProvider.GetService(typeof(IApiVersionDescriptionProvider)) as IApiVersionDescriptionProvider;
            if (provider == null)
            {
                // Fallback: register a default v1 swagger document when API versioning is not configured
                var fallbackInfo = new OpenApiInfo
                {
                    Version = "v1",
                    Title = "InsightCore.WebApi",
                    Description = "Web API",
                };
                options.SwaggerDoc("v1", fallbackInfo);
                return;
            }

            // add a swagger document for each discovered API version
            foreach (var description in provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
            }
        }

        static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
        {
            var info = new OpenApiInfo
            {
                Version = description.ApiVersion.ToString(),
                Title = "t",
                Description = " ",
                TermsOfService = new Uri(""),
                Contact = new OpenApiContact
                {
                    Name = "",
                    Email = "a",
                    Url = new Uri("")
                },
                License = new OpenApiLicense
                {
                    Name = "Use under LICX",
                    Url = new Uri("")
                }
            };

            if (description.IsDeprecated)
            {
                info.Description += "";
            }

            return info;
        }
    }
}
