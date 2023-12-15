using Microsoft.AspNetCore.Mvc;
using CatalogService.Repositories;
using CatalogService.Models;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

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

        /// <summary>
        /// Gets all catalog items
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("getAll")]
        public IActionResult GetAllCatalogs()
        {
            var catalogs = _catalogService.GetAllCatalogs();

            if (catalogs == null || !catalogs.Any())
            {
                return NotFound(); // Return 404 if no Catalogs are found
            }

            return Ok(catalogs);
        }

        /// <summary>
        /// Gets a catalog item by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("{id}")]
        public IActionResult GetCatalog(Guid id)
        {

            CatalogDTO catalog = _catalogService.GetCatalog(id);

            if (catalog == null)
            {
                return NotFound(); // Return 404 if Catalog is not found
            }

            _logger.LogInformation($"Catalog {catalog.CatalogId} - Retrived ");

            return Ok(catalog);
        }

        /// <summary>
        /// Adds a catalog item from object
        /// </summary>
        /// <param name="catalog"></param>
        /// <remarks>
        /// Always generates random id
        /// </remarks>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public IActionResult AddCatalog([FromBody] CatalogDTO catalog)
        {
            if (catalog == null)
            {
                //If NO "Whole-data". Example: If no texting data in the JSON. 
                return BadRequest("Invalid catalog data");
            }

            catalog.CatalogId = GenerateUniqueId();

            if (_catalogService.GetCatalog((Guid)catalog.CatalogId) != null)
            {
                // Handle the case where the ID already exists (e.g., generate a new ID, so it doesnt match the already exist)
                catalog.CatalogId = GenerateUniqueId();
            }

            _catalogService.AddCatalog(catalog);

            return CreatedAtAction(nameof(GetCatalog), new { id = catalog.CatalogId }, catalog);

        }

        /// <summary>
        /// Edits a catalog object using the id to find and replace
        /// </summary>
        /// <param name="catalog"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut]
        public IActionResult EditCatalog([FromBody] CatalogDTO catalog)
        {
            if (catalog == null)
            {
                return BadRequest("Invalid catalog data");
            }
            if (_catalogService.GetCatalog((Guid)catalog.CatalogId) == null)
            {
                return BadRequest("Catalog ID does not exist in the database");
            }

            _catalogService.UpdateCatalog(catalog);

            return Ok(catalog); //Return the updated catalog with 200 OK
        }

        /// <summary>
        /// Deletes a catalog item by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult DeleteCatalog(Guid id)
        {
            var catalog = _catalogService.GetCatalog(id);

            if (catalog == null)
            {
                return NotFound(); // Return 404 if catalog is not found
            }

            _catalogService.DeleteCatalog(id);

            return Ok("Catalog deleted successfully");
        }



        private Guid GenerateUniqueId()
        {
            return Guid.NewGuid();
        }
    }
}
