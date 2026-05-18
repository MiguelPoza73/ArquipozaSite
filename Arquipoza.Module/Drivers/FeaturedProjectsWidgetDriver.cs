using Arquipoza.Module.Models;
using Arquipoza.Module.ViewModels;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;

namespace Arquipoza.Module.Drivers
{
    public class FeaturedProjectsWidgetDriver : ContentPartDisplayDriver<FeaturedProjectsWidget>
    {
        public override IDisplayResult Display(FeaturedProjectsWidget part, BuildPartDisplayContext context)
        {
            return Initialize<FeaturedProjectsWidgetViewModel>(
                "FeaturedProjectsWidget", vm =>
                {
                    vm.Title = part.Title;
                    vm.MaxProjects = part.MaxProjects;
                });
        }

        public override IDisplayResult Edit(FeaturedProjectsWidget part, BuildPartEditorContext context)
        {
            return Initialize<FeaturedProjectsWidgetViewModel>(
                "FeaturedProjectsWidget_Edit", vm =>
                {
                    vm.Title = part.Title;
                    vm.MaxProjects = part.MaxProjects;
                });
        }

        public async override Task<IDisplayResult> UpdateAsync(
            FeaturedProjectsWidget part,
            UpdatePartEditorContext context)
        {
            var vm = new FeaturedProjectsWidgetViewModel();
            await context.Updater.TryUpdateModelAsync(vm, Prefix);

            part.Title = vm.Title;
            part.MaxProjects = vm.MaxProjects;

            return await EditAsync(part, context);
        }
    }
}