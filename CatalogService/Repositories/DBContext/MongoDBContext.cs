using MongoDB.Driver;

namespace CatalogService.Repositories.DBContext
{
    public class MongoDBContext
    {
        private IMongoDatabase _database;
        private IMongoClient _client;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public MongoDBContext(IConfiguration configuration, ILogger <MongoDBContext> logger)
        {
            _configuration = configuration;
            _client = new MongoClient(_configuration["ConnectionString"]);
            _database = _client.GetDatabase(_configuration["DatabaseName"]);
            _logger = logger;
        }

        public IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            _logger.LogInformation($"Getting collection: {collectionName}");
            return _database.GetCollection<T>(collectionName);
        }
    }
}