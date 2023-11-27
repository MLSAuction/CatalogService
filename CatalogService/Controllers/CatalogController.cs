using Microsoft.AspNetCore.Mvc;
using CatalogService.Repositories;

namespace CatalogService.Controllers
{
    public class CatalogController : Controller
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly CatalogRepository _repository;

        public CatalogController (ILogger logger, IConfiguration configuration, CatalogRepository repository)
        {
            _logger = logger;
            _configuration = configuration;
            _repository = repository;
        }
    }
}
