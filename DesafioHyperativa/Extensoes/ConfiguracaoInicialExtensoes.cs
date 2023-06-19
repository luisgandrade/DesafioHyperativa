using DesafioHyperativa.CamadaDados.Repositorios;
using DesafioHyperativa.CamadaDados;
using DesafioHyperativa.Configuracoes;
using DesafioHyperativa.Servicos.Autenticacao;
using DesafioHyperativa.Servicos;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace DesafioHyperativa.Extensoes
{
    public static class ConfiguracaoInicialExtensoes
    {

        public static IServiceCollection InjetarServicos(this IServiceCollection servicos, ParametrosDoApp parametrosDoApp)
        {
            servicos.AddDbContext<CartoesContext>(options =>
                options.UseSqlite(parametrosDoApp.StringConexao, b => b.MigrationsAssembly("DesafioHyperativa.CamadaDados")));
            servicos.AddScoped<ICartaoRepositorio, CartaoRepositorio>();
            servicos.AddScoped<IUnidadeDeTrabalho, UnidadeDeTrabalho>();
            servicos.AddScoped<ICartoesCRUD, CartoesCRUD>();
            servicos.AddScoped<IAutenticador>(_ => new AutenticadorSimples(parametrosDoApp.ConfiguracoesAutenticacao));

            return servicos;
        }

        public static IServiceCollection ConfigurarSwaggerGen(this IServiceCollection servicos)
        {
            servicos.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo { Title = "Cartões", Version = "v1" });
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Informe o token obtido no endpoint /api/login",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });

            return servicos;

        }
    }
}
