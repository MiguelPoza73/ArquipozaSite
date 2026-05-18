using Arquipoza.Module.Models;
using Arquipoza.Module.ViewModels;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;

namespace Arquipoza.Module.Drivers
{
    public class ProjectPartDriver : ContentPartDisplayDriver<ProjectPart>
    {
        public override IDisplayResult Display(ProjectPart part, BuildPartDisplayContext context)
        {
            return Initialize<ProjectPartViewModel>(
                "ProjectPart", vm =>
                {
                    vm.ClientName = part.ClientName;
                    vm.Location = part.Location;
                    vm.Year = part.Year;
                    vm.ProjectType = part.ProjectType;
                    vm.Surface = part.Surface;
                    vm.IsFeatured = part.IsFeatured;
                    vm.ImageUrl = part.ImageUrl;
                });
        }

        public override IDisplayResult Edit(ProjectPart part, BuildPartEditorContext context)
        {
            return Initialize<ProjectPartViewModel>(
                "ProjectPart_Edit", vm =>
                {
                    vm.ClientName = part.ClientName;
                    vm.Location = part.Location;
                    vm.Year = part.Year;
                    vm.ProjectType = part.ProjectType;
                    vm.Surface = part.Surface;
                    vm.IsFeatured = part.IsFeatured;
                    vm.ImageUrl = part.ImageUrl;
                });
        }

        public async override Task<IDisplayResult> UpdateAsync(
            ProjectPart part,
            UpdatePartEditorContext context)
        {
            var vm = new ProjectPartViewModel();
            await context.Updater.TryUpdateModelAsync(vm, Prefix);

            part.ClientName = vm.ClientName ?? string.Empty;
            part.Location = vm.Location ?? string.Empty;
            part.Year = vm.Year;
            part.ProjectType = vm.ProjectType ?? string.Empty;
            part.Surface = vm.Surface ?? string.Empty;
            part.IsFeatured = vm.IsFeatured;
            part.ImageUrl = vm.ImageUrl ?? string.Empty;

            return await EditAsync(part, context);
        }
    }
}