using CatalogService.Repositories.DBContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using MongoDB.Driver;
using CatalogService.Models;
using CatalogService.Repositories.DBContext;


namespace CatalogService.Repositories
{
    public class CatalogRepository : ICatalogRepository
    {
        private readonly ILogger<CatalogRepository> _logger;
        private readonly IConfiguration _configuration;
        private readonly IMongoCollection<CatalogDTO> _db;

        public CatalogRepository(ILogger<CatalogRepository> logger, IConfiguration configuration, MongoDBContext db)
        {
            _logger = logger;
            _configuration = configuration;
            _db = db.GetCollection<CatalogDTO>("Catalogs"); //Fortæller at vores added-informationer(fx. nye Catalogs) kommer inde under Collection "Users" på Mongo

        }


        public IEnumerable<CatalogDTO> GetAllCatalogs()
        {
            return _db.Find(_ => true).ToList();
        }

        public CatalogDTO GetCatalog(int id)
        {
            // Use MongoDB's LINQ methods to query for a Catalog by ID
            return _db.Find(u => u.CatalogId == id).FirstOrDefault();
        }

        public void AddCatalog(CatalogDTO catalog)
        {
            // Insert a new Catalog document into the collection
            _db.InsertOne(catalog);
        }

        public void UpdateCatalog(CatalogDTO catalog)
        {
            // Update an existing Catalog document based on their ID
            var filter = Builders<CatalogDTO>.Filter.Eq(u => u.CatalogId, catalog.CatalogId);
            _db.ReplaceOne(filter, catalog);
        }

        public void DeleteCatalog(int id)
        {
            // Delete a Catalog document by ID
            var filter = Builders<CatalogDTO>.Filter.Eq(u => u.CatalogId, id);
            _db.DeleteOne(filter);
        }

    }
}
