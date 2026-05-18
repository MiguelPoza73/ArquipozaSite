using Arquipoza.Module.Indexes;
using Arquipoza.Module.Models;
using Microsoft.AspNetCore.Mvc;
using OrchardCore.Admin;
using YesSql;

namespace Arquipoza.Module.Controllers
{
    [Admin]
    public class MessagesAdminController : Controller
    {
        private readonly ISession _session;

        public MessagesAdminController(ISession session)
        {
            _session = session;
        }

        public async Task<IActionResult> Index()
        {
            var messages = await _session
                .Query<ContactMessage, ContactMessageIndex>()
                .OrderByDescending(x => x.CreatedAt)
                .ListAsync();

            return View(messages);
        }
    }
}