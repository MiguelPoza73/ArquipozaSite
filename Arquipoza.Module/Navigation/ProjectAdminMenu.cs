using Arquipoza.Module.Permissions;
using Microsoft.Extensions.Localization;
using OrchardCore.Navigation;

namespace Arquipoza.Module.Navigation
{
    public class ProjectAdminMenu : INavigationProvider
    {
        private readonly IStringLocalizer S;

        public ProjectAdminMenu(IStringLocalizer<ProjectAdminMenu> localizer)
        {
            S = localizer;
        }

        public ValueTask BuildNavigationAsync(string name, NavigationBuilder builder)
        {
            if (name != "admin")
                return ValueTask.CompletedTask;

            builder.Add(S["Arquipoza"], menu => menu
                .AddClass("arquipoza")
                .Id("arquipoza")
                .Add(S["Proyectos"], S["Proyectos"], item => item
                    .Permission(ProjectPermissions.ManageProjects)
                    .Action("List", "Admin", new { area = "OrchardCore.Contents", contentTypeId = "ArchitecturalProject" })
                    .LocalNav()
                )
                .Add(S["Messages"], S["Messages"], item => item
                    .Permission(ProjectPermissions.ManageProjects)
                    .Action("Index", "MessagesAdmin", new { area = "Arquipoza.Module" })
                    .LocalNav()
                )
                .Add(S["Quote Requests"], S["Quote Requests"], item => item
                    .Permission(ProjectPermissions.ManageProjects)
                    .Action("Index", "QuotesAdmin", new { area = "Arquipoza.Module" })
                    .LocalNav()
                )
            );

            return ValueTask.CompletedTask;
        }
    }
}