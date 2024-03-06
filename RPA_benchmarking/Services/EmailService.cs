using RPA_benchmarking.EmailSender;
using RPA_benchmarking.Models;
using RPA_benchmarking.Scraper;

namespace RPA_benchmarking.Services
{
    public class EmailService
    {
        public static async Task EnviarEmailAsync(Produto produto, ProdutoScraper produtoMercado, ProdutoScraper produtoMagazine, int menor, string emailName)
        {
            string melhorCompraTexto = menor == 0 ? "MagazineLuiza - <a href=" + produtoMagazine.Url + " >Clique aqui</a>" :
                                    menor == 1 ? "Mercado Livre -  <a href=" + produtoMercado.Url + " >Clique aqui</a>" :
                                    "O Preço é igual nos dois.";

            var email = new Email("smtp-mail.outlook.com", "testrpasenai@outlook.com", "#testrpa0011");

            string corpoEmailHtml = $@"
                <!DOCTYPE html>
                <html lang='en'>
                <head>
                    <meta charset='UTF-8'>
                    <meta http-equiv='X-UA-Compatible' content='IE=edge'>
                    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                </head>
                <body>
                    <h1>Produto Pesquisado</h1>
                    <p>{produto.Nome}</p>
                    <h2>Mercado Livre</h2>
                    <p>Produto: {produtoMercado.Title}</p>
                    <p>Preço: R$ {produtoMercado.Price}</p>
                    <h2>MagazineLuiza</h2>
                    <p>Produto: {produtoMagazine.Title}</p>
                    <p>Preço: {produtoMagazine.Price}</p>
                    <h2>Melhor Compra</h2>
                    <p>{melhorCompraTexto}</p>
                    <p>Robô 3416</p>
                    <p>Usuário: andreLuiz</p>
                </body>
                </html>
                ";

            await email.SendEmailAsync(
                new List<string>
                {
                    "andre.l.junior13@aluno.senai.br",
                    emailName
                },
                "Resultado da Comparação de Preço",
                corpoEmailHtml
            );
        }
    }
}
