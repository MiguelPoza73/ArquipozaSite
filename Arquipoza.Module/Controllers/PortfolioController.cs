using Arquipoza.Module.Indexes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using OrchardCore.ContentManagement;
using YesSql;

namespace Arquipoza.Module.Controllers
{
    public class PortfolioController : Controller
    {
        private readonly ISession _session;
        private readonly IContentManager _contentManager;
        private readonly IMemoryCache _cache;
        private readonly ILogger<PortfolioController> _logger;

        public PortfolioController(
            ISession session,
            IContentManager contentManager,
            IMemoryCache cache,
            ILogger<PortfolioController> logger)
        {
            _session = session;
            _contentManager = contentManager;
            _cache = cache;
            _logger = logger;
        }

        public async Task<IActionResult> Index(string type = "", string search = "", int page = 1)
        {
            _logger.LogInformation("Portfolio index requested. Type: {Type}, Search: {Search}, Page: {Page}", type, search, page);

            var query = _session.Query<ContentItem, ProjectPartIndex>();

            if (!string.IsNullOrEmpty(type))
                query = query.Where(x => x.ProjectType == type);

            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => x.Location.Contains(search) || x.ClientName.Contains(search));

            int pageSize = 6;
            int total = await query.CountAsync();
            var projects = await query.Skip((page - 1) * pageSize).Take(pageSize).ListAsync();

            ViewBag.CurrentType = type;
            ViewBag.CurrentSearch = search;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)total / pageSize);
            ViewBag.TotalProjects = total;

            return View(projects);
        }

        public async Task<IActionResult> Detail(string id)
        {
            _logger.LogInformation("Project detail requested. Id: {Id}", id);

            var project = await _contentManager.GetAsync(id);

            if (project == null)
            {
                _logger.LogWarning("Project not found. Id: {Id}", id);
                return NotFound();
            }

            return View(project);
        }
    }
}