using Arquipoza.Module.Drivers;
using Arquipoza.Module.Events;
using Arquipoza.Module.Handlers;
using Arquipoza.Module.Indexes;
using Arquipoza.Module.Migrations;
using Arquipoza.Module.Models;
using Arquipoza.Module.Navigation;
using Arquipoza.Module.Permissions;
using Arquipoza.Module.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.BackgroundTasks;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Handlers;
using OrchardCore.Data.Migration;
using OrchardCore.Modules;
using OrchardCore.Navigation;
using OrchardCore.Security.Permissions;
using OrchardCore.Users.Events;
using YesSql.Indexes;



namespace Arquipoza.Module
{
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            // ContentPart
            services.AddContentPart<ProjectPart>()
                .UseDisplayDriver<ProjectPartDriver>()
                .AddHandler<ProjectPartHandler>();

            // Migrations
            services.AddScoped<IDataMigration, ProjectMigrations>();

            // Index
            services.AddSingleton<IIndexProvider, ProjectPartIndexProvider>();

            // Navigation
            services.AddScoped<INavigationProvider, ProjectAdminMenu>();

            // Permissions
            services.AddScoped<IPermissionProvider, ProjectPermissions>();

            // Widget
            services.AddContentPart<FeaturedProjectsWidget>()
                .UseDisplayDriver<FeaturedProjectsWidgetDriver>();

            // Background Task
            services.AddSingleton<IBackgroundTask, ProjectCleanupTask>();

            // Login Events
            services.AddScoped<ILoginFormEvent, LoginEvents>();

            // Contact Message Index
            services.AddSingleton<IIndexProvider, ContactMessageIndexProvider>();

            // Quote Request Index
            services.AddSingleton<IIndexProvider, QuoteRequestIndexProvider>();
        }

        public override void Configure(IApplicationBuilder app, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
        {

            routes.MapControllerRoute(
                name: "Root",
                pattern: "",
                defaults: new { controller = "Home", action = "Index", area = "Arquipoza.Module" }
            );
            routes.MapControllerRoute(
                name: "Portfolio",
                pattern: "portfolio",
                defaults: new { controller = "Portfolio", action = "Index", area = "Arquipoza.Module" }


            );

            routes.MapControllerRoute(
                name: "Contact",
                pattern: "contact",
                defaults: new { controller = "Contact", action = "Index", area = "Arquipoza.Module" }
            );

            routes.MapControllerRoute(
                name: "PortfolioDetail",
                pattern: "portfolio/{id}",
                defaults: new { controller = "Portfolio", action = "Detail", area = "Arquipoza.Module" }
            );

            routes.MapControllerRoute(
                name: "Quote",
                pattern: "quote",
                defaults: new { controller = "Quote", action = "Index", area = "Arquipoza.Module" }
            );

            routes.MapControllerRoute(
                name: "Home",
                pattern: "home",
                defaults: new { controller = "Home", action = "Index", area = "Arquipoza.Module" }
            );

            routes.MapControllerRoute(
                name: "Sketchbook",
                pattern: "sketchbook",
                defaults: new { controller = "Sketchbook", action = "Index", area = "Arquipoza.Module" }
            );

            routes.MapControllerRoute(
                name: "MessagesAdmin",
                pattern: "admin/messages",
                defaults: new { controller = "MessagesAdmin", action = "Index", area = "Arquipoza.Module" }
            );

            routes.MapControllerRoute(
                name: "QuotesAdmin",
                pattern: "admin/quotes",
                defaults: new { controller = "QuotesAdmin", action = "Index", area = "Arquipoza.Module" }
            );

        }
    }
}