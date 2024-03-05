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
            using (IWebDriver driver = new ChromeDriver())
            {
                driver.Navigate().GoToUrl($"https://www.magazineluiza.com.br/busca/{descricaoProduto}");

                System.Threading.Thread.Sleep(5000);

                IWebElement priceElement = driver.FindElement(By.CssSelector("[data-testid='price-value']"));
                IWebElement titleElement = driver.FindElement(By.CssSelector("[data-testid='product-title']"));
                IWebElement urlElement = driver.FindElement(By.CssSelector("[data-testid='product-card-container']"));

                if (priceElement != null)
                {

                    ProdutoScraper produto = new ProdutoScraper();
                    produto.Title = titleElement.Text;
                    produto.Price = priceElement.Text;
                    produto.Url = urlElement.GetAttribute("href"); 

                    RegistrarLog("3416", "andreLuiz", DateTime.Now, "WebScraping - Magazine Luiza", "Sucesso", idProduto);

                    return produto;
                }
                else
                {
                    Console.WriteLine("Preço não encontrado.");

                    RegistrarLog("3416", "andreLuiz", DateTime.Now, "WebScraping - Magazine Luiza", "Preço não encontrado", idProduto);

                    return null;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao acessar a página: {ex.Message}");

            RegistrarLog("3416", "andreLuiz", DateTime.Now, "Web Scraping - Magazine Luiza", $"Erro: {ex.Message}", idProduto);

            return null;
        }
    }

    private static void RegistrarLog(string codRob, string usuRob, DateTime dateLog, string processo, string infLog, int idProd)
    {

        using (var context = new CrawlerContext())
        {
            var log = new Log
            {
                CodigoRobo = codRob,
                UsuarioRobo = usuRob,
                DateLog = dateLog,
                Etapa = processo,
                InformacaoLog = infLog,
                IdProdutoAPI = idProd
            };
            context.LOGROBO.Add(log);
            context.SaveChanges();
        }

    }
}