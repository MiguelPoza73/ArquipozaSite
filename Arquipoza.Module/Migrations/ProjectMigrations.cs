using Arquipoza.Module.Indexes;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using YesSql.Sql;
using Arquipoza.Module.Indexes;

namespace Arquipoza.Module.Migrations
{
    public class ProjectMigrations : DataMigration
    {
        private readonly IContentDefinitionManager _contentDefinitionManager;

        public ProjectMigrations(IContentDefinitionManager contentDefinitionManager)
        {
            _contentDefinitionManager = contentDefinitionManager;
        }

        public async Task<int> CreateAsync()
        {
            SchemaBuilder.CreateMapIndexTable<ProjectPartIndex>(table => table
                .Column<string>("ClientName")
                .Column<string>("Location")
                .Column<int>("Year")
                .Column<string>("ProjectType")
                .Column<bool>("IsFeatured")
            );

            await _contentDefinitionManager.AlterPartDefinitionAsync("ProjectPart", part => part
                .WithDescription("Datos específicos de un proyecto arquitectónico")
            );

            await _contentDefinitionManager.AlterTypeDefinitionAsync("ArchitecturalProject", type => type
                .Creatable()
                .Listable()
                .WithPart("TitlePart")
                .WithPart("ProjectPart")
                .WithPart("HtmlBodyPart")
            );

            return 1;
        }

        public async Task<int> UpdateFrom1Async()
        {
            SchemaBuilder.CreateMapIndexTable<ProjectPartIndex>(table => table
                .Column<string>("ClientName")
                .Column<string>("Location")
                .Column<int>("Year")
                .Column<string>("ProjectType")
                .Column<bool>("IsFeatured")
            );

            return 2;
        }

        public async Task<int> UpdateFrom2Async()
        {
            await _contentDefinitionManager.AlterPartDefinitionAsync("FeaturedProjectsWidget", part => part
                .WithDescription("Displays featured architectural projects")
            );

            await _contentDefinitionManager.AlterTypeDefinitionAsync("FeaturedProjectsWidget", type => type
                .Stereotype("Widget")
                .WithPart("FeaturedProjectsWidget")
            );

            return 3;
        }
        public async Task<int> UpdateFrom3Async()
        {
            SchemaBuilder.CreateMapIndexTable<ContactMessageIndex>(table => table
                .Column<string>("Email")
                .Column<string>("Name")
                .Column<DateTime>("CreatedAt")
                .Column<bool>("IsRead")
            );

            return 4;
        }

        public async Task<int> UpdateFrom4Async()
        {
            SchemaBuilder.CreateMapIndexTable<QuoteRequestIndex>(table => table
                .Column<string>("Email")
                .Column<string>("Name")
                .Column<string>("ProjectType")
                .Column<DateTime>("CreatedAt")
                .Column<bool>("IsRead")
            );

            return 5;
        }

    }
}