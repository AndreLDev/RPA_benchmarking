using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Internal.Logging;
using RPA_benchmarking.Class;
using System;
using System.Diagnostics;

public class MagazineLuizaScraper
{
    public ProdutoScraper ObterPreco(string descricaoProduto, int idProduto)
    {
        try
        {
            // Inicializa o ChromeDriver
            using (IWebDriver driver = new ChromeDriver())
            {
                // Abre a página
                driver.Navigate().GoToUrl($"https://www.magazineluiza.com.br/busca/{descricaoProduto}");

                // Aguarda um tempo fixo para permitir que a página seja carregada (você pode ajustar conforme necessário)
                System.Threading.Thread.Sleep(5000);

                // Encontra o elemento que possui o atributo data-testid
                IWebElement priceElement = driver.FindElement(By.CssSelector("[data-testid='price-value']"));
                IWebElement titleElement = driver.FindElement(By.CssSelector("[data-testid='product-title']"));
                IWebElement urlElement = driver.FindElement(By.CssSelector("[data-testid='product-card-container']"));

                // Verifica se o elemento foi encontrado
                if (priceElement != null)
                {
                    // Obtém o preço do primeiro produto

                    ProdutoScraper produto = new ProdutoScraper();
                    produto.Title = titleElement.Text;
                    produto.Price = priceElement.Text;
                    produto.Url = urlElement.GetAttribute("href"); 

                    // Registra o log com o ID do produto
                    RegistrarLog("0001", "andre", DateTime.Now, "WebScraping - Magazine Luiza", "Sucesso", idProduto);

                    // Retorna o preço
                    return produto;
                }
                else
                {
                    Console.WriteLine("Preço não encontrado.");

                    // Registra o log com o ID do produto
                    RegistrarLog("0001", "andre", DateTime.Now, "WebScraping - Magazine Luiza", "Preço não encontrado", idProduto);

                    return null;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao acessar a página: {ex.Message}");

            // Registra o log com o ID do produto
            RegistrarLog("0001", "andre", DateTime.Now, "Web Scraping - Magazine Luiza", $"Erro: {ex.Message}", idProduto);

            return null;
        }
    }

    private static void RegistrarLog(string codRob, string usuRob, DateTime dateLog, string processo, string infLog, int idProd)
    {

        using (var context = new CrawlerContext())
        {
            var log = new Log
            {
                CodRob = codRob,
                UsuRob = usuRob,
                DateLog = dateLog,
                Processo = processo,
                InfLog = infLog,
                IdProd = idProd
            };
            context.Logs.Add(log);
            context.SaveChanges();
        }

    }
}