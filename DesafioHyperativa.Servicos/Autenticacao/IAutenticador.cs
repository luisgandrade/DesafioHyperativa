using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesafioHyperativa.Servicos.Autenticacao
{
    public interface IAutenticador
    {

        string? Autenticar(string? usuario, string? senha);
    }
}
