# Desafio Hyperativa

## Pré-requisitos

- .NET 6 SDK
- Visual Studio 2022
- dotnet ef tools

## Antes de executar
Para realizar a configuração inicial do banco de dados use os comandos abaixo:
```
cd /caminho/para/solucao
dotnet ef migrations add Inicial --project DesafioHyperativa.CamadaDados --startup-project DesafioHyperativa
dotnet ef database update --project DesafioHyperativa.CamadaDados --startup-project DesafioHyperativa
```
## Como rodar

Essa aplicação pode ser executada diretamente no Visual Studio 2022.
Caso seja executado via VS, a aplicação atenderá requisições em https://localhost:7043.

## Como chamar a API
Acesse `https://localhost:[porta]/swagger/index.html` para visualizar todos os endpoints da API através da Swagger UI. 
Para autenticar, chame o endpoint `api/login` com o corpo:
```json
{
  "usuario": "usuario_teste",
  "senha": "senha_teste"
}
```
Por questões de simplicidade, para esse contexto específico as credenciais, assim como as propriedades de configuraçao de autenticação, 
estão engessadas nos parâmetros da aplicação.

Os endpoints `api/cartoes/*` são acessíveis somente com um token adquirido em `api/login`. Esse token deve ser configurado no botao 
`Authorize` nessa mesma página.

Caso opte por user alguma aplicaçao para fazer requisiçoes direto, deve sert adicionado o cabeçalho `Authorization` com valor igual a 
`Bearer tokenJwt`, onde `tokenJwt` é o token adquirido no endpoint de login.


