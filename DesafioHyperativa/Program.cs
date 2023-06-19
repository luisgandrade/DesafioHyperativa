using DesafioHyperativa.CamadaDados;
using DesafioHyperativa.CamadaDados.Repositorios;
using DesafioHyperativa.Configuracoes;
using DesafioHyperativa.Extensoes;
using DesafioHyperativa.Middlewares;
using DesafioHyperativa.Servicos;
using DesafioHyperativa.Servicos.Autenticacao;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

var parametrosDoApp = builder.Configuration.Get<ParametrosDoApp>();

builder.Services.InjetarServicos(parametrosDoApp);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = parametrosDoApp.ConfiguracoesAutenticacao.Emissor,
        ValidAudience = parametrosDoApp.ConfiguracoesAutenticacao.Audiencia,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(parametrosDoApp.ConfiguracoesAutenticacao.ChaveAssinaturaJwt ?? 
            throw new ArgumentNullException(nameof(parametrosDoApp.ConfiguracoesAutenticacao.ChaveAssinaturaJwt))))
    };
});


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigurarSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<LogadorDeAcesso>(parametrosDoApp.CaminhoArquivoLog);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
