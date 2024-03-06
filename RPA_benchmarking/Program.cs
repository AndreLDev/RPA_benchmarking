using Newtonsoft.Json;
using RPA_benchmarking.EmailSender;
using RPA_benchmarking.Models;
using RPA_benchmarking.Scraper;
using RPA_benchmarking.Services;
using System.Net.Http.Headers;
using System.Text;

public class Program
{
    private static readonly string _apiUrl = "http://regymatrix-001-site1.ktempurl.com/api/v1/produto/getall";
    private static readonly string _username = "11164448";
    private static readonly string _password = "60-dayfreetrial";
    private static readonly List<Produto> _produtosVerificados = new List<Produto>();
    private static string _emailName;

    public static void Main(string[] args)
    {
        int intervalo = 6000;

        Console.WriteLine("Digite o Email de destino:");
        _emailName = Console.ReadLine();
        Console.WriteLine("");

        Timer timer = new Timer(async (state) => await VerificacaoProdutoService.VerificarNovoProdutoAsync(_apiUrl, _username, _password, _produtosVerificados, _emailName), null, 0, intervalo);

        Console.WriteLine("Pressione qualquer tecla para sair...");
        Console.ReadKey();
    }

}
