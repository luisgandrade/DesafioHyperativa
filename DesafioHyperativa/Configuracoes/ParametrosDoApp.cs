using DesafioHyperativa.Servicos.Autenticacao;

namespace DesafioHyperativa.Configuracoes
{
    public class ParametrosDoApp
    {

        public string StringConexao { get; set; } = "";
        public ConfiguracoesAutenticadorSimples ConfiguracoesAutenticacao { get; set; } = new ConfiguracoesAutenticadorSimples();        
        public string CaminhoArquivoLog { get; set; } = "";
    }
}
