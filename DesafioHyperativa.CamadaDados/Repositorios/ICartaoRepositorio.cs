using DesafioHyperativa.CamadaDados.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesafioHyperativa.CamadaDados.Repositorios
{
    public interface ICartaoRepositorio
    {
        Task<Cartao?> BuscarCartao(string numeroCartao);
        Task InserirCartao(Cartao cartao);
    }
}
