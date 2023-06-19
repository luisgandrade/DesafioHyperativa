using DesafioHyperativa.DTOs;
using DesafioHyperativa.Servicos.Autenticacao;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DesafioHyperativa.Controllers
{
    [Route("api/login")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IAutenticador _autenticador;

        public LoginController(IAutenticador autenticador)
        {
            _autenticador = autenticador;
        }

        [HttpPost]
        public IActionResult Login(LoginInfoDTO loginInfo)
        {

            var tokenJwt = _autenticador.Autenticar(loginInfo.Usuario, loginInfo.Senha);
            if (tokenJwt is null)
                return Unauthorized();

            return Ok(tokenJwt);
        }
    }
}
