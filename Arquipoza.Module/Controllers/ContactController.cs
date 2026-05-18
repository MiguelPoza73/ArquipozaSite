using Arquipoza.Module.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using OrchardCore.DisplayManagement.Notify;
using YesSql;

namespace Arquipoza.Module.Controllers
{
    public class ContactController : Controller
    {
        private readonly INotifier _notifier;
        private readonly ILogger<ContactController> _logger;
        private readonly IStringLocalizer S;
        private readonly ISession _session;

        public ContactController(
            INotifier notifier,
            ILogger<ContactController> logger,
            IStringLocalizer<ContactController> localizer,
            ISession session)
        {
            _notifier = notifier;
            _logger = logger;
            S = localizer;
            _session = session;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Submit(string name, string email, string message)
        {
            _logger.LogInformation("Contact form submitted by {Name} ({Email})", name, email);

            var contactMessage = new ContactMessage
            {
                Name = name,
                Email = email,
                Message = message,
                CreatedAt = DateTime.UtcNow,
                IsRead = false
            };

            _session.Save(contactMessage);
            await _session.SaveChangesAsync();

            TempData["Success"] = S["Thank you for your message. We will contact you soon."].Value;

            return RedirectToAction("Index");
        }
    }
}