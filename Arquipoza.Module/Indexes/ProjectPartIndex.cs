using Arquipoza.Module.Models;
using OrchardCore.ContentManagement;
using YesSql.Indexes;

namespace Arquipoza.Module.Indexes
{
    public class ProjectPartIndex : MapIndex
    {
        public string ClientName { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public int Year { get; set; }
        public string ProjectType { get; set; } = string.Empty;
        public bool IsFeatured { get; set; }
    }

    public class ProjectPartIndexProvider : IndexProvider<ContentItem>
    {
        public override void Describe(DescribeContext<ContentItem> context)
        {
            context.For<ProjectPartIndex>()
                .When(contentItem => contentItem.Has<ProjectPart>())
                .Map(contentItem =>
                {
                    var part = contentItem.As<ProjectPart>();
                    return new ProjectPartIndex
                    {
                        ClientName = part.ClientName,
                        Location = part.Location,
                        Year = part.Year,
                        ProjectType = part.ProjectType,
                        IsFeatured = part.IsFeatured
                    };
                });
        }
    }
}