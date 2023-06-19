using System.Globalization;

namespace DesafioHyperativa.Middlewares
{
    public class LogadorDeAcesso
    {
        private readonly string _caminhoArquivoLog;
        private readonly RequestDelegate _next;
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        public LogadorDeAcesso(RequestDelegate next, string caminhoArquivoLog)
        {
            _caminhoArquivoLog = caminhoArquivoLog;
            _next = next;
        }

        private async Task EscreverNoArquivoAsync(string registro)
        {
            await _semaphore.WaitAsync();
            try
            {
                File.AppendAllLines(_caminhoArquivoLog, new[] { registro });
            }
            finally 
            {
                _semaphore.Release();
            }
        }

        public async Task InvokeAsync(HttpContext context)
        {

            var corpoOriginalDaResposta = context.Response.Body;
            try
            {
                
                using(var streamDeResposta = new MemoryStream())
                {
                    context.Response.Body = streamDeResposta;

                    await _next(context);

                    string resposta = "";
                    context.Response.Body.Seek(0, SeekOrigin.Begin);
                    using (var streamReader = new StreamReader(streamDeResposta))
                    {
                        if (context.Request.Path.HasValue && !context.Request.Path.Value.Contains("api/login"))
                            resposta = await streamReader.ReadToEndAsync();
                        
                        context.Response.Body.Seek(0, SeekOrigin.Begin);
                        await context.Response.Body.CopyToAsync(corpoOriginalDaResposta);
                    }
                    

                    await EscreverNoArquivoAsync($"{DateTime.Now} - {context.Request.Method} {context.Request.Path} - Status Code: {context.Response.StatusCode} - Resposta: {resposta}");
                }
                
            }
            finally
            {
                context.Response.Body = corpoOriginalDaResposta;
            }
            
        }
    }
}
