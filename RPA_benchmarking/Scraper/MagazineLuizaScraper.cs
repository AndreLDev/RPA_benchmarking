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

                    RegistrarLog("0001", "andre", DateTime.Now, "WebScraping - Magazine Luiza", "Sucesso", idProduto);

                    return produto;
                }
                else
                {
                    Console.WriteLine("Preço não encontrado.");

                    RegistrarLog("0001", "andre", DateTime.Now, "WebScraping - Magazine Luiza", "Preço não encontrado", idProduto);

                    return null;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao acessar a página: {ex.Message}");

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