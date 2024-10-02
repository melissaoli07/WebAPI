using MongoDB.Driver;
using CrudApi.Entities;
using CrudApi.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CrudApi.Repositories
{
    public class ProdutoRepository
    {
        private readonly IMongoCollection<Produto> _produtos;

        public ProdutoRepository(MongoDbContext context)
        {
            _produtos = context.GetCollection<Produto>("Produtos");
        }

        public async Task<List<Produto>> GetProdutosAsync() => await _produtos.Find(_ => true).ToListAsync();

        public async Task<Produto> GetProdutoByIdAsync(string id) => await _produtos.Find(p => p.Id == id).FirstOrDefaultAsync();

        public async Task CriarProdutoAsync(Produto produto) => await _produtos.InsertOneAsync(produto);

        public async Task AtualizarProdutoAsync(string id, Produto produto) => await _produtos.ReplaceOneAsync(p => p.Id == id, produto);

        public async Task DeletarProdutoAsync(string id) => await _produtos.DeleteOneAsync(p => p.Id == id);
    }
}
