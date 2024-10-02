using System.Collections.Generic;
using System.Threading.Tasks;
using CrudApi.Entities;
using CrudApi.Data;
using CrudApi.Repositories;
using Xunit;
using Moq;
using MongoDB.Driver;



public class ProdutoRepositoryTest
{
    private readonly Mock<IMongoCollection<Produto>> _mockCollection;
    private readonly ProdutoRepository _repository;

    public ProdutoRepositoryTest()
    {
        _mockCollection = new Mock<IMongoCollection<Produto>>();
        var context = new Mock<MongoDbContext>();
        context.Setup(c => c.GetCollection<Produto>("Produtos")).Returns(_mockCollection.Object);
        _repository = new ProdutoRepository(context.Object);
    }

    /*
    [Fact]
    public async Task DeveRetornarTodosOsProdutos()
    {
        _mockCollection.Setup(c => c.FindAsync(It.IsAny<FilterDefinition<Produto>>(), null, default))
            .ReturnsAsync(new List<Produto> { new Produto { Nome = "Produto 1", Preco = 100M } });

        var produtos = await _repository.GetProdutosAsync();

        Assert.Single(produtos);
        Assert.Equal("Produto 1", produtos.First().Nome);
    }
    */

    [Fact]
    public async Task TesteGetProducts()
    {
        // Arrange
        var mockCollection = new Mock<IMongoCollection<Produto>>();
        var mockCursor = new Mock<IAsyncCursor<Produto>>();

        // Configurando o mockCursor
        mockCursor.SetupSequence(_ => _.MoveNext(It.IsAny<CancellationToken>()))
                  .Returns(true)
                  .Returns(false);
        mockCursor.SetupGet(_ => _.Current).Returns(new List<Produto>
    {
        new Produto { Id = "1", Nome = "Produto 1" }
    });

        // Configurando o mockCollection
        mockCollection.Setup(_ => _.FindAsync(It.IsAny<FilterDefinition<Produto>>(),
                                               null,
                                               It.IsAny<CancellationToken>()))
                      .ReturnsAsync(mockCursor.Object);

        // Executando a operação que você está testando
        var result = await mockCollection.Object.FindAsync(FilterDefinition<Produto>.Empty);

        // Verificações
    }





}




