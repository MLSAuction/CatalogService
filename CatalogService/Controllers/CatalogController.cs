using Microsoft.AspNetCore.Mvc;
using CatalogService.Repositories;
using CatalogService.Models;
using System.Linq;

namespace CatalogService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CatalogController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly ICatalogRepository _catalogService;

        public CatalogController(ILogger<CatalogController> logger, IConfiguration configuration, ICatalogRepository catalogRepository)
        {
            _logger = logger;
            _configuration = configuration;
            _catalogService = catalogRepository;
        }

        [HttpGet]
        public IActionResult GetAllCatalogs()
        {
            var catalogs = _catalogService.GetAllCatalogs();

            if (catalogs == null || !catalogs.Any())
            {
                return NotFound(); // Return 404 if no Catalogs are found
            }

            return Ok(catalogs);
        }

        [HttpGet("{id}")]
        public IActionResult GetCatalog(int id)
        {

            CatalogDTO catalog = _catalogService.GetCatalog(id);

            if (catalog == null)
            {
                return NotFound(); // Return 404 if Catalog is not found
            }

            _logger.LogInformation($"Catalog {catalog.CatalogId} - Retrived ");

            return Ok(catalog);
        }

        [HttpPost]
        public IActionResult AddCatalog([FromBody] CatalogDTO catalog)
        {
            if (catalog == null)
            {
                //If NO "Whole-data". Example: If no texting data in the JSON. 
                return BadRequest("Invalid catalog data");
            }

           

            if (catalog.CatalogId == null)
            {
                //Check if there is ID 
                catalog.CatalogId = GenerateUniqueId();
            }

            if (_catalogService.GetCatalog((int)catalog.CatalogId) != null)
            {
                // Handle the case where the ID already exists (e.g., generate a new ID, so it doesnt match the already exist)
                catalog.CatalogId = GenerateUniqueId();
            }

            _catalogService.AddCatalog(catalog);

            return CreatedAtAction(nameof(GetCatalog), new { id = catalog.CatalogId }, catalog);

        }

        [HttpPut("{id}")]
        public IActionResult EditCatalog(int id, [FromBody] CatalogDTO catalog)
        {
            if (catalog == null)
            {
                return BadRequest("Invalid catalog data");
            }

            if (id != catalog.CatalogId)
            {
                return BadRequest("Catalog ID in the request body does not match the route parameter");
            }

            _catalogService.UpdateCatalog(catalog);

            return Ok("Catalog updated successfully");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCatalog(int id)
        {
            var catalog = _catalogService.GetCatalog(id);

            if (catalog == null)
            {
                return NotFound(); // Return 404 if catalog is not found
            }

            _catalogService.DeleteCatalog(id);

            return Ok("Catalog deleted successfully");
        }



        private int GenerateUniqueId()
        {
            return Math.Abs(Guid.NewGuid().GetHashCode());
        }
    }
}
