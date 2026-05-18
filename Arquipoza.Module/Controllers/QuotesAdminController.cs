using Arquipoza.Module.Indexes;
using Arquipoza.Module.Models;
using Microsoft.AspNetCore.Mvc;
using OrchardCore.Admin;
using YesSql;

namespace Arquipoza.Module.Controllers
{
    [Admin]
    public class QuotesAdminController : Controller
    {
        private readonly ISession _session;

        public QuotesAdminController(ISession session)
        {
            _session = session;
        }

        public async Task<IActionResult> Index()
        {
            var quotes = await _session
                .Query<QuoteRequest, QuoteRequestIndex>()
                .OrderByDescending(x => x.CreatedAt)
                .ListAsync();

            return View(quotes);
        }
    }
}