using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPA_benchmarking.Services
{
    public class BenchmarkingService
    {

        public int CompararValor(string precoMercado, string precoMagazine, int idProduto)
        {
            var priceMercado = precoMercado.Replace(".", "");
            var priceMagazine = precoMagazine.Trim(new char[] { ' ', 'R', '$' }).Replace(".", "");

            var numPrecoMercado = double.Parse(priceMercado);
            var numPrecoMagazine = double.Parse(priceMagazine);

            if (numPrecoMercado > numPrecoMagazine)
            {
                RegistrarLog("3416", "andreLuiz", DateTime.Now, "Benchmarking", "Sucesso", idProduto);
                return 0;
            }
            else if (numPrecoMagazine > numPrecoMercado)
            {
                RegistrarLog("3416", "andreLuiz", DateTime.Now, "Benchmarking", "Sucesso", idProduto);
                return 1;
            }
            else
            {
                RegistrarLog("3416", "andreLuiz", DateTime.Now, "Benchmarking", "Alerta", idProduto);
                return 2;
            }
        }

        private void RegistrarLog(string codRob, string usuRob, DateTime dateLog, string processo, string infLog, int idProd)
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
}
