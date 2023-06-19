using DesafioHyperativa.Servicos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DesafioHyperativa.Controllers
{
    [Route("api/cartoes")]
    [ApiController]
    [Authorize]
    public class CartoesController : ControllerBase
    {

        private readonly ICartoesCRUD _cartoesCrud;

        public CartoesController(ICartoesCRUD cartoesCrud)
        {
            _cartoesCrud = cartoesCrud;
        }

        [HttpGet]
        public async Task<IActionResult> Buscar([FromQuery][Required]string numeroCartao)
        {
            if(string.IsNullOrWhiteSpace(numeroCartao))
                return BadRequest("Número do cartão não informado");

            var idDoCartaoEncontrado = await _cartoesCrud.BuscarCartao(numeroCartao);
            if (!idDoCartaoEncontrado.HasValue)
                return NotFound();

            return Ok(idDoCartaoEncontrado.Value);
        }

        [HttpPost]
        public async Task<IActionResult> Inserir([FromBody] string numeroCartao)
        {
            if (string.IsNullOrWhiteSpace(numeroCartao))
                return BadRequest("Número do cartão não informado");

            await _cartoesCrud.InserirCartao(numeroCartao);

            return Ok();
        }

        [HttpPost("lote")]
        public async Task<IActionResult> InserirLote([Required] IFormFile arquivoLote)
        {
            if (arquivoLote is null)
                return BadRequest("Arquivo de lote não foi informado");

            await _cartoesCrud.InserirLoteDeCartoes(arquivoLote.OpenReadStream());

            return Ok();
        }


    }
}
