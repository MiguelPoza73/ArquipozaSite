using OrchardCore.ContentManagement;

namespace Arquipoza.Module.Models
{
    public class FeaturedProjectsWidget : ContentPart
    {
        public string Title { get; set; } = "Featured Projects";
        public int MaxProjects { get; set; } = 3;
    }
}