using System;
using CatalogService.Models;



namespace CatalogService.Repositories
{
	public interface ICatalogRepository
	{
		IEnumerable<CatalogDTO> GetAllCatalogs();
		CatalogDTO GetCatalog(int id);
		void AddCatalog(CatalogDTO catalog);
		void UpdateCatalog(CatalogDTO catalog);
		void DeleteCatalog(int id);


		
	}
}

