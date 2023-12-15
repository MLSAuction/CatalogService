using System;
using CatalogService.Models;



namespace CatalogService.Repositories
{
	public interface ICatalogRepository
	{
		IEnumerable<CatalogDTO> GetAllCatalogs();
		CatalogDTO GetCatalog(Guid id);
		void AddCatalog(CatalogDTO catalog);
		void UpdateCatalog(CatalogDTO catalog);
		void DeleteCatalog(Guid id);


		
	}
}

