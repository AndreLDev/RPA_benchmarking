using RPA_benchmarking.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPA_benchmarking.Scraper
{
    public abstract class ScraperBase
    {
        public abstract ProdutoScraper ObterPreco(string descricaoProduto, int idProduto);
        protected void RegistrarLog(string processo, string informacaoLog, int idProduto)
        {
            LogService.RegistrarLogAsync("3416", "andreLuiz", DateTime.Now, processo, informacaoLog, idProduto);
        }
    }
}
