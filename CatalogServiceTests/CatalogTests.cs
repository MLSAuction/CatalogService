using NUnit.Framework;
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using CatalogService.Controllers;
using CatalogService.Models;
using CatalogService.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CatalogServiceTests
{
    [TestFixture]
    public class CatalogTests
    {
        private Mock<ICatalogRepository> _catalogRepositoryStub;
        private CatalogController _catalogController;

        [SetUp]
        public void Setup()
        {
            _catalogRepositoryStub = new Mock<ICatalogRepository>();
            var loggerMock = new Mock<ILogger<CatalogController>>();
            var configurationMock = new Mock<IConfiguration>();

            _catalogController = new CatalogController(loggerMock.Object, configurationMock.Object, _catalogRepositoryStub.Object);
        }

        [Test]
        public void GetAllCatalogsReturnsAllCatalogs()
        {
            // Arrange
            CatalogDTO catalog1 = new CatalogDTO { CatalogId = 1};
            CatalogDTO catalog2 = new CatalogDTO { CatalogId = 2};

            var catalogs = new List<CatalogDTO> { catalog1, catalog2 };

            _catalogRepositoryStub.Setup(repo => repo.GetAllCatalogs()).Returns(catalogs);

            // Act
            var result = _catalogController.GetAllCatalogs() as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(200, result.StatusCode);

            var resultCollection = result.Value as IEnumerable<CatalogDTO>;
            Assert.NotNull(resultCollection);

            CollectionAssert.AreEquivalent(catalogs, resultCollection);
        }

        [Test]
        [TestCase(1, true)]
        [TestCase(999, false)]
        public void GetCatalogReturnsResult(int catalogId, bool expectedResult)
        {
            // Arrange
            CatalogDTO catalog = new CatalogDTO { CatalogId = catalogId };

            _catalogRepositoryStub.Setup(repo => repo.GetCatalog(catalogId)).Returns(catalog);
            _catalogRepositoryStub.Setup(repo => repo.GetCatalog(999)).Returns((CatalogDTO)null); // Nonexistent catalogId

            // Act
            var result = _catalogController.GetCatalog(catalogId);

            // Assert
            if (expectedResult)
            {
                Assert.IsInstanceOf<OkObjectResult>(result);
                var okResult = (OkObjectResult)result;

                Assert.AreEqual(200, okResult.StatusCode);
                Assert.AreEqual(catalog, okResult.Value);
            }
            else
            {
                Assert.IsInstanceOf<NotFoundResult>(result);
                var notFoundResult = (NotFoundResult)result;

                Assert.AreEqual(404, notFoundResult.StatusCode);
            }
        }

        [Test]
        [TestCase(1, true)]
        [TestCase(999, false)]
        public void DeleteCatalogReturnsResult(int catalogId, bool expectedResult)
        {
            // Arrange
            CatalogDTO catalog = new CatalogDTO { CatalogId = catalogId };

            _catalogRepositoryStub.Setup(repo => repo.GetCatalog(catalogId)).Returns(catalog);
            _catalogRepositoryStub.Setup(repo => repo.GetCatalog(999)).Returns((CatalogDTO)null); // Nonexistent catalogId
            _catalogRepositoryStub.Setup(repo => repo.DeleteCatalog(catalogId));

            // Act
            var result = _catalogController.DeleteCatalog(catalogId);

            // Assert
            if (expectedResult)
            {
                Assert.IsInstanceOf<OkObjectResult>(result);
                var okResult = (OkObjectResult)result;

                Assert.AreEqual(200, okResult.StatusCode);
                Assert.AreEqual("Catalog deleted successfully", okResult.Value);
            }
            else
            {
                Assert.IsInstanceOf<NotFoundResult>(result);
                var notFoundResult = (NotFoundResult)result;

                Assert.AreEqual(404, notFoundResult.StatusCode);
            }
        }

        [Test]
        [TestCase(1, true)]
        [TestCase(999, false)]
        public void EditCatalogReturnsResult(int catalogId, bool expectedResult)
        {
            // Arrange
            CatalogDTO catalog = new CatalogDTO { CatalogId = catalogId };

            _catalogRepositoryStub.Setup(repo => repo.GetCatalog(catalogId)).Returns(catalog);
            _catalogRepositoryStub.Setup(repo => repo.GetCatalog(999)).Returns((CatalogDTO)null); // Nonexistent catalogId
            _catalogRepositoryStub.Setup(repo => repo.UpdateCatalog(It.IsAny<CatalogDTO>()));

            // Act
            var result = _catalogController.EditCatalog(catalog);

            // Assert
            if (expectedResult)
            {
                Assert.IsInstanceOf<OkObjectResult>(result);
                var okResult = (OkObjectResult)result;

                Assert.AreEqual(200, okResult.StatusCode);
                Assert.AreEqual(catalog, okResult.Value);
            }
            else
            {
                Assert.IsInstanceOf<BadRequestObjectResult>(result);
                var badRequestResult = (BadRequestObjectResult)result;

                Assert.AreEqual(400, badRequestResult.StatusCode);
                Assert.AreEqual("Catalog ID does not exist in the database", badRequestResult.Value);
            }
        }





    }
}
