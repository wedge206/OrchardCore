using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using OrchardCore.Modules;
using OrchardCore.Settings;

namespace OrchardCore.Localization
{
    /// <summary>
    /// These services are registered on the tenant service collection
    /// </summary>
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddPortableObjectLocalization(options => options.ResourcesPath = "Localization");

            // Override the default localization file locations with Orchard specific ones
            services.Replace(ServiceDescriptor.Singleton<ILocalizationFileLocationProvider, ModularPoFileLocationProvider>());
        }

        public override void Configure(IApplicationBuilder app, IRouteBuilder routes, IServiceProvider serviceProvider)
        {
            var siteSettings = serviceProvider.GetService<ISiteService>().GetSiteSettingsAsync().GetAwaiter().GetResult();

            var options = new RequestLocalizationOptions()
                .SetDefaultCulture(siteSettings.Culture)
                .AddSupportedCultures(siteSettings.SupportedCultures)
                .AddSupportedUICultures(siteSettings.SupportedCultures)
                ;

            app.UseRequestLocalization(options);
        }
    }
}
