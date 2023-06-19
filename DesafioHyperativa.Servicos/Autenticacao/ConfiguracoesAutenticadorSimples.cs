using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesafioHyperativa.Servicos.Autenticacao
{
    public class ConfiguracoesAutenticadorSimples
    {
        public string? ChaveAssinaturaJwt { get; set; }
        public string? Emissor { get; set; }
        public string? Audiencia { get; set; }
        public string? UsuarioPadrao { get; set; }
        public string? SenhaPadrao { get; set; }
    }
}
