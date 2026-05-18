namespace Arquipoza.Module.ViewModels
{
    public class ProjectPartViewModel
    {
        public string ClientName { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public int Year { get; set; }
        public string ProjectType { get; set; } = string.Empty;
        public string Surface { get; set; } = string.Empty;
        public bool IsFeatured { get; set; }

        public string ImageUrl { get; set; } = string.Empty;
    }
}