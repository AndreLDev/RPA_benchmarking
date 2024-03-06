using HtmlAgilityPack;
using OpenQA.Selenium.Internal.Logging;
using RPA_benchmarking.Scraper;
using RPA_benchmarking.Services;
using System;
using System.Diagnostics;
using System.Xml;

public class MercadoLivreScraper : ScraperBase
{
    public override ProdutoScraper ObterPreco(string descricaoProduto, int idProduto)
    {
        string url = $"https://lista.mercadolivre.com.br/{descricaoProduto}";

        try
        {
            HtmlWeb web = new HtmlWeb();
            HtmlDocument document = web.Load(url);

            HtmlNode firstProductPriceNode = document.DocumentNode.SelectSingleNode("//span[@class='andes-money-amount__fraction']");
            HtmlNode firstProductTitleNode = document.DocumentNode.SelectSingleNode("//h2[@class='ui-search-item__title']");
            HtmlNode firstProductUrlNode = document.DocumentNode.SelectSingleNode("//a[contains(@class, 'ui-search-link__title-card')]");

            if (firstProductPriceNode != null)
            {
                string firstProductPrice = firstProductPriceNode.InnerText.Trim();
                string firstProductTitle = firstProductTitleNode.InnerText.Trim();
                string firstProductUrl = firstProductUrlNode.GetAttributeValue("href", "");

                RegistrarLog("WebScraping - Mercado Livre", "Sucesso", idProduto);

                ProdutoScraper produto = new ProdutoScraper
                {
                    Title = firstProductTitle,
                    Price = firstProductPrice,
                    Url = firstProductUrl
                };

                return produto;
            }
            else
            {
                Console.WriteLine("Preço não encontrado.");
                RegistrarLog("WebScraping - Mercado Livre", "Preço não encontrado", idProduto);
                return null;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao acessar a página: {ex.Message}");
            RegistrarLog("Web Scraping - Mercado Livre", $"Erro: {ex.Message}", idProduto);
            return null;
        }
    }

}