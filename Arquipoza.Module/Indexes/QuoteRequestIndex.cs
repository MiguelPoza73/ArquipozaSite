using Arquipoza.Module.Models;
using YesSql.Indexes;

namespace Arquipoza.Module.Indexes
{
    public class QuoteRequestIndex : MapIndex
    {
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string ProjectType { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public bool IsRead { get; set; }
    }

    public class QuoteRequestIndexProvider : IndexProvider<QuoteRequest>
    {
        public override void Describe(DescribeContext<QuoteRequest> context)
        {
            context.For<QuoteRequestIndex>()
                .Map(quote => new QuoteRequestIndex
                {
                    Email = quote.Email,
                    Name = quote.Name,
                    ProjectType = quote.ProjectType,
                    CreatedAt = quote.CreatedAt,
                    IsRead = quote.IsRead
                });
        }
    }
}