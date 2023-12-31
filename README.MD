# Desafio Hyperativa

## Pr�-requisitos

- .NET 6 SDK
- Visual Studio 2022
- dotnet ef tools

## Antes de executar
Para realizar a configura��o inicial do banco de dados use os comandos abaixo:
```
cd /caminho/para/solucao
dotnet ef migrations add Inicial --project DesafioHyperativa.CamadaDados --startup-project DesafioHyperativa
dotnet ef database update --project DesafioHyperativa.CamadaDados --startup-project DesafioHyperativa
```
## Como rodar

Essa aplica��o pode ser executada diretamente no Visual Studio 2022.
Caso seja executado via VS, a aplica��o atender� requisi��es em https://localhost:7043.

## Como chamar a API
Acesse `https://localhost:[porta]/swagger/index.html` para visualizar todos os endpoints da API atrav�s da Swagger UI. 
Para autenticar, chame o endpoint `api/login` com o corpo:
```json
{
  "usuario": "usuario_teste",
  "senha": "senha_teste"
}
```
Por quest�es de simplicidade, para esse contexto espec�fico as credenciais, assim como as propriedades de configura�ao de autentica��o, 
est�o engessadas nos par�metros da aplica��o.

Os endpoints `api/cartoes/*` s�o acess�veis somente com um token adquirido em `api/login`. Esse token deve ser configurado no botao 
`Authorize` nessa mesma p�gina.

Caso opte por user alguma aplica�ao para fazer requisi�oes direto, deve sert adicionado o cabe�alho `Authorization` com valor igual a 
`Bearer tokenJwt`, onde `tokenJwt` � o token adquirido no endpoint de login.


