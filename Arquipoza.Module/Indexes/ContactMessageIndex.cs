using Arquipoza.Module.Models;
using YesSql.Indexes;

namespace Arquipoza.Module.Indexes
{
    public class ContactMessageIndex : MapIndex
    {
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public bool IsRead { get; set; }
    }

    public class ContactMessageIndexProvider : IndexProvider<ContactMessage>
    {
        public override void Describe(DescribeContext<ContactMessage> context)
        {
            context.For<ContactMessageIndex>()
                .Map(message => new ContactMessageIndex
                {
                    Email = message.Email,
                    Name = message.Name,
                    CreatedAt = message.CreatedAt,
                    IsRead = message.IsRead
                });
        }
    }
}