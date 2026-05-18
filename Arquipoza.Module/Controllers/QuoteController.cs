using Arquipoza.Module.Models;
using Arquipoza.Module.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using OrchardCore.DisplayManagement.Notify;
using YesSql;

namespace Arquipoza.Module.Controllers
{
    public class QuoteController : Controller
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly INotifier _notifier;
        private readonly ILogger<QuoteController> _logger;
        private readonly IStringLocalizer S;
        private readonly ISession _session;

        public QuoteController(
            IAuthorizationService authorizationService,
            INotifier notifier,
            ILogger<QuoteController> logger,
            IStringLocalizer<QuoteController> localizer,
            ISession session)
        {
            _authorizationService = authorizationService;
            _notifier = notifier;
            _logger = logger;
            S = localizer;
            _session = session;
        }

        public IActionResult Index(string projectName = "")
        {
            ViewBag.ProjectName = projectName;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Submit(string name, string email, string phone,
            string projectType, string budget, string timeline, string description)
        {
            _logger.LogInformation("Quote request submitted by {Name} ({Email})", name, email);

            var quote = new QuoteRequest
            {
                Name = name,
                Email = email,
                Phone = phone,
                ProjectType = projectType,
                Budget = budget,
                Timeline = timeline,
                Description = description,
                CreatedAt = DateTime.UtcNow,
                IsRead = false
            };

            _session.Save(quote);
            await _session.SaveChangesAsync();

            TempData["Success"] = S["Thank you for your quote request. We will contact you within 48 hours."].Value;

            return RedirectToAction("Index");
        }
    }
}