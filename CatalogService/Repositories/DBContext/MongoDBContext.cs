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

        public MongoDBContext(IConfiguration configuration, ILogger <MongoDBContext> logger, Secret<SecretData> secret)
        {
            _configuration = configuration;
            _client = new MongoClient(secret.Data.Data["ConnectionString"].ToString());
            _database = _client.GetDatabase(secret.Data.Data["DatabaseName"].ToString());
            _logger = logger;
        }

        public IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            _logger.LogInformation($"Getting collection: {collectionName}");
            return _database.GetCollection<T>(collectionName);
        }
    }
}