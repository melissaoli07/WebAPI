using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CrudApi.Entities;
using CrudApi.Data;
using CrudApi.Repositories;
using Xunit;
using Moq;
using MongoDB.Driver;
using Microsoft.Extensions.Configuration;

namespace CrudApi.Tests
{
    public class ProdutoRepositoryTest
    {
        private readonly Mock<IMongoCollection<Produto>> _mockCollection;
        private readonly ProdutoRepository _repository;

        public ProdutoRepositoryTest()
        {
            _mockCollection = new Mock<IMongoCollection<Produto>>();

            // Mock do IConfiguration
            var mockConfig = new Mock<IConfiguration>();
            mockConfig.Setup(c => c.GetConnectionString("MongoDb")).Returns("mongodb://localhost:27017/MeuBancoDeDados");
            mockConfig.Setup(c => c["DatabaseName"]).Returns("MeuBancoDeDados");

            // Criação do contexto com o mock do IConfiguration
            var context = new MongoDbContext(mockConfig.Object);
            _repository = new ProdutoRepository(context);
        }

        [Fact]
        public async Task DeveRetornarTodosOsProdutos()
        {
            // Arrange
            var produtos = new List<Produto> { new Produto { Nome = "Produto 1", Preco = 100M } };
            var mockCursor = CreateMockCursor(produtos);

            _mockCollection.Setup(c => c.FindAsync(It.IsAny<FilterDefinition<Produto>>(), null, default))
                .ReturnsAsync(mockCursor.Object);

            // Act
            var result = await _repository.GetProdutosAsync();

            // Assert
            Assert.Single(result);
            Assert.Equal("Produto 1", result.First().Nome);
        }

        [Fact]
        public async Task TesteGetProducts()
        {
            // Arrange
            var produtos = new List<Produto> { new Produto { Id = "1", Nome = "Produto 1" } };
            var mockCursor = CreateMockCursor(produtos);

            _mockCollection.Setup(_ => _.FindAsync(It.IsAny<FilterDefinition<Produto>>(),
                                                    null,
                                                    It.IsAny<CancellationToken>()))
                           .ReturnsAsync(mockCursor.Object);

            // Act
            var result = await _repository.GetProdutosAsync();

            // Assert
            Assert.Single(result);
            Assert.Equal("Produto 1", result.First().Nome);
        }

        private Mock<IAsyncCursor<Produto>> CreateMockCursor(List<Produto> produtos)
        {
            var mockCursor = new Mock<IAsyncCursor<Produto>>();
            mockCursor.SetupSequence(_ => _.MoveNext(It.IsAny<CancellationToken>()))
                      .Returns(true)
                      .Returns(false);
            mockCursor.SetupGet(_ => _.Current).Returns(produtos);
            return mockCursor;
        }
    }
}
