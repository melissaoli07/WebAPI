using Microsoft.AspNetCore.Mvc;
using CrudApi.Repositories;
using CrudApi.Entities;
using System.Threading.Tasks;

namespace CrudApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProdutosController : ControllerBase
    {
        private readonly ProdutoRepository _repository;

        public ProdutosController(ProdutoRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await _repository.GetProdutosAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var produto = await _repository.GetProdutoByIdAsync(id);
            if (produto == null) return NotFound();
            return Ok(produto);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Produto produto)
        {
            await _repository.CriarProdutoAsync(produto);
            return CreatedAtAction(nameof(Get), new { id = produto.Id }, produto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] Produto produto)
        {
            await _repository.AtualizarProdutoAsync(id, produto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _repository.DeletarProdutoAsync(id);
            return NoContent();
        }
    }
}
