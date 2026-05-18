using Arquipoza.Module.Indexes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using OrchardCore.ContentManagement;
using YesSql;

namespace Arquipoza.Module.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISession _session;
        private readonly IMemoryCache _cache;
        private readonly ILogger<HomeController> _logger;

        public HomeController(
            ISession session,
            IMemoryCache cache,
            ILogger<HomeController> logger)
        {
            _session = session;
            _cache = cache;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Homepage requested, loading featured projects.");

            var cacheKey = "featured_projects";

            if (!_cache.TryGetValue(cacheKey, out IEnumerable<ContentItem> featured))
            {
                _logger.LogInformation("Cache miss — querying featured projects from database.");

                featured = await _session
                    .Query<ContentItem, ProjectPartIndex>()
                    .Where(x => x.IsFeatured == true)
                    .Take(3)
                    .ListAsync();

                _cache.Set(cacheKey, featured, TimeSpan.FromMinutes(10));
            }
            else
            {
                _logger.LogInformation("Cache hit — returning cached featured projects.");
            }

            return View(featured);
        }
    }
}