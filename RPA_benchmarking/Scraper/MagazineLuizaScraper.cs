using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Internal.Logging;
using RPA_benchmarking.Scraper;
using RPA_benchmarking.Services;
using System;
using System.Diagnostics;

public class MagazineLuizaScraper : ScraperBase
{
    public override ProdutoScraper ObterPreco(string descricaoProduto, int idProduto)
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
                    ProdutoScraper produto = new ProdutoScraper
                    {
                        Title = titleElement.Text,
                        Price = priceElement.Text,
                        Url = urlElement.GetAttribute("href")
                    };

                    RegistrarLog("WebScraping - Magazine Luiza", "Sucesso", idProduto);

                    return produto;
                }
                else
                {
                    Console.WriteLine("Preço não encontrado.");
                    RegistrarLog("WebScraping - Magazine Luiza", "Preço não encontrado", idProduto);
                    return null;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao acessar a página: {ex.Message}");
            RegistrarLog("Web Scraping - Magazine Luiza", $"Erro: {ex.Message}", idProduto);
            return null;
        }
    }
}