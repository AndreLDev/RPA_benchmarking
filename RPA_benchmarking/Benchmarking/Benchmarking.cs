using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPA_benchmarking.Benchmarking
{
    public class Benchmarking
    {

        public int CompararValor(string precoMercado, string precoMagazine, int idProduto)
        {
            var priceMercado = precoMercado.Replace(".", "");
            var priceMagazine = precoMagazine.Trim(new Char[] { ' ', 'R', '$'}).Replace(".", "");

            var numPrecoMercado = Double.Parse(priceMercado);
            var numPrecoMagazine = Double.Parse(priceMagazine);

            if(numPrecoMercado > numPrecoMagazine)
            {
                RegistrarLog("0001", "andre", DateTime.Now, "Benchmarking", "Sucesso", idProduto);
                return 0;
            }
            else if(numPrecoMagazine > numPrecoMercado)
            {
                RegistrarLog("0001", "andre", DateTime.Now, "Benchmarking", "Sucesso", idProduto);
                return 1;
            }
            else 
            {
                RegistrarLog("0001", "andre", DateTime.Now, "Benchmarking", "Alerta", idProduto);
                return 2;
            }
        }

        private void RegistrarLog(string codRob, string usuRob, DateTime dateLog, string processo, string infLog, int idProd)
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
}
