namespace DesafioHyperativa.Servicos
{
    public interface ICartoesCRUD
    {
        Task InserirCartao(string cartao);
        Task InserirLoteDeCartoes(Stream lote);
        Task<int?> BuscarCartao(string cartao);

    }
}