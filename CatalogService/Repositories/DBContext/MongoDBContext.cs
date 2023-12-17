using MongoDB.Driver;
using VaultSharp.V1.Commons;

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
            _client = new MongoClient(Environment.GetEnvironmentVariable("ConnectionString"));
            _database = _client.GetDatabase(Environment.GetEnvironmentVariable("DatabaseName"));
            _logger = logger;
        }

        public IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            _logger.LogInformation($"Getting collection: {collectionName}");
            return _database.GetCollection<T>(collectionName);
        }
    }
}