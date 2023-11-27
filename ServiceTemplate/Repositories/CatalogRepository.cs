namespace CatalogService.Repositories
{
    public class CatalogRepository
    {
        private readonly ILogger<CatalogRepository> _logger;
        private readonly IConfiguration _configuration;

        public CatalogRepository (ILogger<CatalogRepository> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }
    }
}
