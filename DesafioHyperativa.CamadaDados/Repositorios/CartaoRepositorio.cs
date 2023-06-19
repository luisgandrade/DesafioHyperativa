using DesafioHyperativa.CamadaDados.Entidades;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesafioHyperativa.CamadaDados.Repositorios
{
    public class CartaoRepositorio : ICartaoRepositorio
    {
        private readonly CartoesContext _cartoesContext;

        public CartaoRepositorio(CartoesContext cartoesContext)
        {
            _cartoesContext = cartoesContext;
        }

        public Task<Cartao?> BuscarCartao(string numeroCartao)
        {
            return _cartoesContext.Cartoes.SingleOrDefaultAsync(nc => nc.NumeroCartao == numeroCartao);
        }

        public async Task InserirCartao(Cartao cartao)
        {
            await _cartoesContext.Cartoes.AddAsync(cartao);            
        }
    }
}
