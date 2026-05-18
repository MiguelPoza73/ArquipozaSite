using Arquipoza.Module.Models;
using OrchardCore.ContentManagement.Handlers;

namespace Arquipoza.Module.Handlers
{
    public class ProjectPartHandler : ContentPartHandler<ProjectPart>
    {
        public override Task InitializingAsync(InitializingContentContext context, ProjectPart part)
        {
            part.Year = DateTime.Now.Year;
            part.ProjectType = "Residential";
            return Task.CompletedTask;
        }

        public override Task PublishedAsync(PublishContentContext context, ProjectPart part)
        {
            return Task.CompletedTask;
        }
    }
}