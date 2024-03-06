using System;
using System.Threading.Tasks;
using RPA_benchmarking.Models;

namespace RPA_benchmarking.Services
{
    public class LogService
    {
        public static async Task RegistrarLogAsync(string codRob, string usuRob, DateTime dateLog, string processo, string infLog, int idProd)
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
                await context.SaveChangesAsync();
            }
        }
    }
}
