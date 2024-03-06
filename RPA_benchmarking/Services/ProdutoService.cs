using Newtonsoft.Json;
using RPA_benchmarking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPA_benchmarking.Services
{
    public class ProdutoService
    {
        public static async Task ProcessarProdutoAsync(Produto produto, string emailName)
        {
            MercadoLivreScraper mercadoLivreScraper = new MercadoLivreScraper();
            var produtoMercado = mercadoLivreScraper.ObterPreco(produto.Nome, produto.Id);

            MagazineLuizaScraper magazineLuizaScraper = new MagazineLuizaScraper();
            var produtoMagazine = magazineLuizaScraper.ObterPreco(produto.Nome, produto.Id);

            BenchmarkingService benchmarking = new BenchmarkingService();
            var menor = benchmarking.CompararValor(produtoMercado.Price, produtoMagazine.Price, produto.Id);

            await EmailService.EnviarEmailAsync(produto, produtoMercado, produtoMagazine, menor, emailName);

            await LogService.RegistrarLogAsync("3416", "andreLuiz", DateTime.Now, "EnvioEmail", "Sucesso", produto.Id);
        }


        public static bool ProdutoJaRegistrado(int idProduto)
        {
            using (var context = new CrawlerContext())
            {
                return context.LOGROBO.Any(log => log.IdProdutoAPI == idProduto && log.CodigoRobo == "3416");
            }
        }

        public static List<Produto> ObterNovosProdutos(string responseData)
        {
            return JsonConvert.DeserializeObject<List<Produto>>(responseData);
        }
    }
}
