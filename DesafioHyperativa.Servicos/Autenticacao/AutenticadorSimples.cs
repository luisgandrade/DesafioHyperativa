using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DesafioHyperativa.Servicos.Autenticacao
{
    public class AutenticadorSimples : IAutenticador
    {
        private readonly SymmetricSecurityKey _chaveAssinatura;

        private readonly ConfiguracoesAutenticadorSimples _configuracoes;

        public AutenticadorSimples(ConfiguracoesAutenticadorSimples configuracoes)
        {
            _chaveAssinatura = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuracoes.ChaveAssinaturaJwt ?? throw new ArgumentNullException(nameof(_configuracoes.ChaveAssinaturaJwt))));
            _configuracoes = configuracoes;
        }

        public string? Autenticar(string? usuario, string? senha)
        {
            if (usuario is null || senha is null || usuario != _configuracoes.UsuarioPadrao || senha != _configuracoes.SenhaPadrao)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
            new Claim(ClaimTypes.NameIdentifier, _configuracoes.UsuarioPadrao),
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = _configuracoes.Emissor ?? throw new ArgumentNullException(nameof(_configuracoes.Emissor)),
                Audience = _configuracoes.Audiencia ?? throw new ArgumentNullException(nameof(_configuracoes.Audiencia)),
                SigningCredentials = new SigningCredentials(_chaveAssinatura, SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
