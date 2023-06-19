using DesafioHyperativa.CamadaDados;
using DesafioHyperativa.CamadaDados.Entidades;
using DesafioHyperativa.CamadaDados.Repositorios;
using Moq;

namespace DesafioHyperativa.Servicos.Testes
{
    public class CartoesCRUDTestes
    {
        private readonly Mock<ICartaoRepositorio> _cartaoRepositorio;
        private readonly Mock<IUnidadeDeTrabalho> _unidadeDeTrabalho;
        private readonly CartoesCRUD _cartoesCrud;

        public CartoesCRUDTestes()
        {
            _cartaoRepositorio = new();
            _unidadeDeTrabalho = new();
            _cartoesCrud = new(_cartaoRepositorio.Object, _unidadeDeTrabalho.Object);
        }

        [Fact]
        public async Task BuscarCartaoDeveRetornarIdDoCartaoSeEncontrado()
        {
            var cartaoEncontrado = new Cartao 
            { 
                Id = 3, 
                NumeroCartao = "1111222233334444"
            };

            _cartaoRepositorio.Setup(cr => cr.BuscarCartao(cartaoEncontrado.NumeroCartao)).ReturnsAsync(cartaoEncontrado);

            var idDoCartaoEncontrado = await _cartoesCrud.BuscarCartao(cartaoEncontrado.NumeroCartao);

            Assert.Equal(cartaoEncontrado.Id, idDoCartaoEncontrado);
        }

        [Fact]
        public async Task BuscarCartaoDeveRetornarNuloSeCartaoNaoForEncontrado()
        {
            var cartaoEncontrado = new Cartao
            {
                Id = 3,
                NumeroCartao = "1111222233334444"
            };

            _cartaoRepositorio.Setup(cr => cr.BuscarCartao(cartaoEncontrado.NumeroCartao)).ReturnsAsync(() => null);

            var idDoCartaoEncontrado = await _cartoesCrud.BuscarCartao(cartaoEncontrado.NumeroCartao);

            Assert.Null(idDoCartaoEncontrado);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public async Task InserirCartaoDeveLancarExcecaoSeNenhumNumeroForInformado(string numeroCartao)
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _cartoesCrud.InserirCartao(numeroCartao));            
        }


        [Theory]
        [InlineData("123456875")]
        [InlineData("11112222333344a4")]
        [InlineData("11112222333344445555")]
        public async Task InserirCartaoDeveLancarExcecaoSeCartaoForInvalido(string numeroCartao)
        {
            await Assert.ThrowsAsync<ApplicationException>(async () => await _cartoesCrud.InserirCartao(numeroCartao));
        }

        [Fact]
        public async Task InserirCartaoLancaExcecaoSeCartaoJaFoiAdicionadoAoBanco()
        {
            var numeroCartao = "1111222233334444";
            _cartaoRepositorio.Setup(cr => cr.BuscarCartao(numeroCartao)).ReturnsAsync(new Cartao());
            await Assert.ThrowsAsync<ApplicationException>(async () => await _cartoesCrud.InserirCartao(numeroCartao));
        }

        [Fact]
        public async Task InserirCartaoDeveInserirNoBancoAtravesDoRepositorio()
        {
            var numeroCartao = "1111222233334444";
            _cartaoRepositorio.Setup(cr => cr.BuscarCartao(numeroCartao)).ReturnsAsync(() => null);

            await _cartoesCrud.InserirCartao(numeroCartao);

            _cartaoRepositorio.Verify(cr => cr.InserirCartao(It.Is<Cartao>(c => c.NumeroCartao == numeroCartao)), Times.Once());
            _unidadeDeTrabalho.Verify(ut => ut.CommitarTransacao(), Times.Once());
        }


        [Fact]
        public async Task InserirLoteDeveLancarExcecaoSeCabecalhoNaoEstiverPresente()
        {
            var arquivoEntrada = new MemoryStream();

            await Assert.ThrowsAsync<ApplicationException>(async () => await _cartoesCrud.InserirLoteDeCartoes(arquivoEntrada));
        }

        [Fact]
        public async Task InserirLoteDeveLancarExcecaoSeCabecalhoEstiverEmFormatoInesperado()
        {
            var arquivoEntrada = new MemoryStream();
            using (var streamWriter = new StreamWriter(arquivoEntrada, leaveOpen: true))
            {
                await streamWriter.WriteLineAsync("sdjiosdjihoijds");
            }
            arquivoEntrada.Seek(0, SeekOrigin.Begin);

            await Assert.ThrowsAsync<ApplicationException>(async () => await _cartoesCrud.InserirLoteDeCartoes(arquivoEntrada));
        }

        [Fact]
        public async Task InserirLoteDeveLancarExcecaoSeRegistroEstiverEmBranco()
        {
            var arquivoEntrada = new MemoryStream();
            using (var streamWriter = new StreamWriter(arquivoEntrada, leaveOpen: true))
            {
                await streamWriter.WriteLineAsync("DESAFIO-HYPERATIVA           20180524LOTE0001000001");
            }
            arquivoEntrada.Seek(0, SeekOrigin.Begin);

            await Assert.ThrowsAsync<ApplicationException>(async () => await _cartoesCrud.InserirLoteDeCartoes(arquivoEntrada));
        }

        [Fact]
        public async Task InserirLoteDeveLancarExcecaoSeQuantidadeDeRegistrosEhDiferenteDaInformadaNoCabecalho()
        {
            var arquivoEntrada = new MemoryStream();
            using (var streamWriter = new StreamWriter(arquivoEntrada, leaveOpen: true))
            {
                await streamWriter.WriteLineAsync("DESAFIO-HYPERATIVA           20180524LOTE0001000002");
                await streamWriter.WriteLineAsync($"C1     4456897922969999");
            }
            arquivoEntrada.Seek(0, SeekOrigin.Begin);

            await Assert.ThrowsAsync<ApplicationException>(async () => await _cartoesCrud.InserirLoteDeCartoes(arquivoEntrada));
        }

        [Theory]
        [InlineData("123456875")]
        [InlineData("11112222333344a4")]
        [InlineData("11112222333344445555")]
        public async Task InserirLoteDeveLancarExcecaoSeNumeroDeCartaoLidoForInvalido(string numeroCartao)
        {
            var arquivoEntrada = new MemoryStream();
            using (var streamWriter = new StreamWriter(arquivoEntrada, leaveOpen: true))
            {
                await streamWriter.WriteLineAsync("DESAFIO-HYPERATIVA           20180524LOTE0001000001");
                await streamWriter.WriteLineAsync($"C1     {numeroCartao}");
            }
            arquivoEntrada.Seek(0, SeekOrigin.Begin);

            await Assert.ThrowsAsync<ApplicationException>(async () => await _cartoesCrud.InserirLoteDeCartoes(arquivoEntrada));
        }

        [Fact]
        public async Task InserirLoteDeveInserirTodosOsCartoesInformados()
        {
            var cartao1 = "4456897922969999";
            var cartao2 = "4456897999999999";
            var cartao3 = "4456897119999999";
            var arquivoEntrada = new MemoryStream();
            using (var streamWriter = new StreamWriter(arquivoEntrada, leaveOpen: true))
            {
                await streamWriter.WriteLineAsync("DESAFIO-HYPERATIVA           20180524LOTE0001000003");
                await streamWriter.WriteLineAsync($"C1     {cartao1}");
                await streamWriter.WriteLineAsync($"C2     {cartao2}");
                await streamWriter.WriteLineAsync($"C3     {cartao3}");
            }
            arquivoEntrada.Seek(0, SeekOrigin.Begin);

            await _cartoesCrud.InserirLoteDeCartoes(arquivoEntrada);

            _cartaoRepositorio.Verify(cr => cr.InserirCartao(It.Is<Cartao>(c => c.NumeroCartao == cartao1)), Times.Once());
            _cartaoRepositorio.Verify(cr => cr.InserirCartao(It.Is<Cartao>(c => c.NumeroCartao == cartao2)), Times.Once());
            _cartaoRepositorio.Verify(cr => cr.InserirCartao(It.Is<Cartao>(c => c.NumeroCartao == cartao3)), Times.Once());
            _unidadeDeTrabalho.Verify(ut => ut.CommitarTransacao(), Times.Once());
        }

        [Fact]
        public async Task InserirLoteDeveInserirApenasUmRegistroPorCartaoSeEstiverDuplicado()
        {
            var numeroCartao = "4456897922969999";
            var arquivoEntrada = new MemoryStream();
            using (var streamWriter = new StreamWriter(arquivoEntrada, leaveOpen: true))
            {
                await streamWriter.WriteLineAsync("DESAFIO-HYPERATIVA           20180524LOTE0001000002");
                await streamWriter.WriteLineAsync($"C1     {numeroCartao}");
                await streamWriter.WriteLineAsync($"C2     {numeroCartao}");
            }
            arquivoEntrada.Seek(0, SeekOrigin.Begin);

            await _cartoesCrud.InserirLoteDeCartoes(arquivoEntrada);

            _cartaoRepositorio.Verify(cr => cr.InserirCartao(It.Is<Cartao>(c => c.NumeroCartao == numeroCartao)), Times.Once());
            _unidadeDeTrabalho.Verify(ut => ut.CommitarTransacao(), Times.Once());
        }
    }
}