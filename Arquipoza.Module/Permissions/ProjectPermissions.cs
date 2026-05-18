using OrchardCore.Security.Permissions;

namespace Arquipoza.Module.Permissions
{
    public class ProjectPermissions : IPermissionProvider
    {
        public static readonly Permission ManageProjects = new Permission(
            "ManageProjects",
            "Manage Architectural Projects"
        );

        public static readonly Permission ViewProjects = new Permission(
            "ViewProjects",
            "View Architectural Projects"
        );

        public Task<IEnumerable<Permission>> GetPermissionsAsync()
        {
            return Task.FromResult<IEnumerable<Permission>>(new[]
            {
                ManageProjects,
                ViewProjects
            });
        }

        public IEnumerable<PermissionStereotype> GetDefaultStereotypes()
        {
            return new[]
            {
                new PermissionStereotype
                {
                    Name = "Administrator",
                    Permissions = new[] { ManageProjects, ViewProjects }
                },
                new PermissionStereotype
                {
                    Name = "Editor",
                    Permissions = new[] { ViewProjects }
                }
            };
        }
    }
}