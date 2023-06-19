using DesafioHyperativa.CamadaDados;
using DesafioHyperativa.CamadaDados.Entidades;
using DesafioHyperativa.CamadaDados.Repositorios;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesafioHyperativa.Servicos
{
    public class CartoesCRUD : ICartoesCRUD
    {
        private readonly ICartaoRepositorio _cartaoRepositorio;
        private readonly IUnidadeDeTrabalho _unidadeDeTrabalho;

        public CartoesCRUD(ICartaoRepositorio cartaoRepositorio, IUnidadeDeTrabalho unidadeDeTrabalho)
        {
            _cartaoRepositorio = cartaoRepositorio;
            _unidadeDeTrabalho = unidadeDeTrabalho;
        }

        private bool CartaoValido(string numeroCartao)
        {
            var numeroCartaoSemPadding = numeroCartao.Trim();
            return numeroCartaoSemPadding.Length >= 16 && numeroCartaoSemPadding.Length <= 19 && numeroCartaoSemPadding.All(nc => char.IsDigit(nc));
        }

        public async Task<int?> BuscarCartao(string numeroCartao)
        {
            if (string.IsNullOrWhiteSpace(numeroCartao))
                throw new ArgumentNullException(nameof(numeroCartao));

            var cartaoEncontrado = await _cartaoRepositorio.BuscarCartao(numeroCartao);
            return cartaoEncontrado?.Id;
        }

        public async Task InserirCartao(string numeroCartao)
        {
            if (string.IsNullOrWhiteSpace(numeroCartao))
                throw new ArgumentNullException(nameof(numeroCartao));

            if (!CartaoValido(numeroCartao))
                throw new ApplicationException("Número de cartão inválido");
            var idDoCartaoNoBanco = await BuscarCartao(numeroCartao);
            if (idDoCartaoNoBanco.HasValue)
                throw new ApplicationException("Cartão já existe no banco");

            await _cartaoRepositorio.InserirCartao(new Cartao { NumeroCartao = numeroCartao });
            await _unidadeDeTrabalho.CommitarTransacao();
        }


        public async Task InserirLoteDeCartoes(Stream lote)
        {
            var cartoesParaInsercao = new HashSet<string>();
            using(var leitorDeStream = new StreamReader(lote))
            {
                var cabecalho = await leitorDeStream.ReadLineAsync();
                if (string.IsNullOrWhiteSpace(cabecalho))
                    throw new ApplicationException("Cabeçalho não informado");
                if (cabecalho.Length < 51 || !int.TryParse(cabecalho.Substring(45, 6), out var quantidadeCartoes))
                    throw new ApplicationException("Cabeçalho em formato inesperado");
                
                for (int i = 0; i < quantidadeCartoes; i++)
                {
                    var linha = await leitorDeStream.ReadLineAsync();
                    if (string.IsNullOrWhiteSpace(linha))
                        throw new ApplicationException($"Não foi informado registro na linha {i}");
                    var numeroCartao = linha.Substring(7);
                    if (!CartaoValido(numeroCartao))
                        throw new ApplicationException($"Cartão {numeroCartao} inválido");
                    cartoesParaInsercao.Add(numeroCartao);
                }
            }

            foreach (var numeroCartao in cartoesParaInsercao)
                await _cartaoRepositorio.InserirCartao(new Cartao { NumeroCartao = numeroCartao });
            await _unidadeDeTrabalho.CommitarTransacao();
        }
    }
}
