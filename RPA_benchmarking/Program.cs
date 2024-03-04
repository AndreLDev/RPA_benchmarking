using Newtonsoft.Json;
using RestSharp;
using RPA_benchmarking.Benchmarking;
using RPA_benchmarking.Class;
using System.Net.Http.Headers;
using System.Text;

public class Program
{

    public static string EmailName { get; set; }

    static List<Produto> produtosVerificados = new List<Produto>();

    public static void Main(string[] args)
    {
        int intervalo = 6000;

        Console.WriteLine("");
        Console.WriteLine("Digite o Email de destino:");
        EmailName = Console.ReadLine();
        Console.WriteLine("");
        Console.WriteLine("");

        Timer timer = new Timer(VerificarNovoProduto, null, 0, intervalo);

        Console.WriteLine("Pressione qualquer tecla para sair...");
        Console.ReadKey();
    }

    public static async void VerificarNovoProduto(object state)
    {
        string username = "11164448";
        string senha = "60-dayfreetrial";
        string url = "http://regymatrix-001-site1.ktempurl.com/api/v1/produto/getall";

        try
        {
            using (HttpClient client = new HttpClient())
            {

                var byteArray = Encoding.ASCII.GetBytes($"{username}:{senha}");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));



                HttpResponseMessage response = await client.GetAsync(url);


                if (response.IsSuccessStatusCode)
                {

                    string responseData = await response.Content.ReadAsStringAsync();


                    List<Produto> novosProdutos = ObterNovosProdutos(responseData);


                    

                    foreach (Produto produto in novosProdutos)
                    {
                        if (!produtosVerificados.Exists(p => p.Id == produto.Id))
                        {

                            Console.WriteLine($"Novo produto encontrado: ID {produto.Id}, Nome: {produto.Nome}");
                            produtosVerificados.Add(produto);


                            if (!ProdutoJaRegistrado(produto.Id))
                            {

                                RegistrarLog("0001", "andre", DateTime.Now, "ConsultaAPI - Verificar Produto", "Sucesso", produto.Id);


                                MercadoLivreScraper mercadoLivreScraper = new MercadoLivreScraper();
                                var produtoMercado = mercadoLivreScraper.ObterPreco(produto.Nome, produto.Id);


                                MagazineLuizaScraper magazineLuizaScraper = new MagazineLuizaScraper();
                                var produtoMagazine = magazineLuizaScraper.ObterPreco(produto.Nome, produto.Id);


                                Benchmarking benchmarking = new Benchmarking();
                                var menor = benchmarking.CompararValor(produtoMercado.Price, produtoMagazine.Price, produto.Id);


                                string melhorCompraTexto = menor == 0 ? "MagazineLuiza - <a href=" + produtoMagazine.Url + " >Clique aqui</a>"  : menor == 1 ? "Mercado Livre -  <a href=" + produtoMercado.Url + " >Clique aqui</a>" : "O Preço é igual nos dois." ;


                                var email = new Email("smtp-mail.outlook.com", "testrpasenai@outlook.com", "#testrpa0011");


                                string corpoEmailHtml = $@"
                                <!DOCTYPE html>
                                <html lang='en'>
                                <head>
                                    <meta charset='UTF-8'>
                                    <meta http-equiv='X-UA-Compatible' content='IE=edge'>
                                    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                                </head>
                                <body>
                                    <h1>Produto Pesquisado</h1>
                                    <p> {produto.Nome} </p>
                                    <h2>Mercado Livre</h2>
                                    <p>Produto: {produtoMercado.Title}</p>
                                    <p>Preço: R$ {produtoMercado.Price}</p>
                                    <h2>MagazineLuiza</h2>
                                    <p>Produto: {produtoMagazine.Title}</p>
                                    <p>Preço: {produtoMagazine.Price}</p>
                                    <h2>Melhor Compra</h2>
                                    <p>{melhorCompraTexto}</p>
                                    <p>Robô 0001</p>
                                    <p>Usuário: andre</p>
                                </body>
                                </html>
                                ";


                                email.SendEmail(
                                    emailsTo: new List<string>
                                    {
                                        "andre.l.junior13@aluno.senai.br",
                                        EmailName
                                    },
                                    subject: "Resultado da Comparação de Preço",
                                    body: corpoEmailHtml);


                                RegistrarLog("0001", "andre", DateTime.Now, "EnvioEmail", "Sucesso", produto.Id);



                                var options = new RestClientOptions("https://app.whatsgw.com.br")
                                {
                                    MaxTimeout = -1,
                                };
                                var clientWsp = new RestClient(options);
                                var request = new RestRequest("/api/WhatsGw/Send", Method.Post);
                                request.AddHeader("Content-Type", "application/json");
                                var body = @"{
                                    " + "\n" +
                                    @"""apikey"" : ""B3CA76C2-07F3-47E6-A2F8-YOWAPIKEY"",
                                    " + "\n" +
                                    @"""phone_number"" : ""5511999999999"",
                                    " + "\n" +
                                    @"""contact_phone_number"" : ""5511988888888"",
                                    " + "\n" +
                                    @"""message_custom_id"" : ""yoursoftwareid"",
                                    " + "\n" +
                                    @"""message_type"" : ""text"",
                                    " + "\n" +
                                    @"""message_body"" : ""Teste de Msg\n_Italico_ \n*negrito*\n~tachado~\n```Monoespaçado```\n😜"",
                                    " + "\n" +
                                    @"""check_status"" : ""1""
                                    " + "\n" +
                                    @"}";
                                request.AddStringBody(body, DataFormat.Json);
                                RestResponse responseWsp = await clientWsp.ExecuteAsync(request);
                                Console.WriteLine(responseWsp.Content);
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"Erro: {response.StatusCode}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao fazer a requisição: {ex.Message}");
        }
    }

    static List<Produto> ObterNovosProdutos(string responseData)
    {
        List<Produto> produtos = JsonConvert.DeserializeObject<List<Produto>>(responseData);
        return produtos;
    }

    static bool ProdutoJaRegistrado(int idProduto)
    {
        using (var context = new CrawlerContext())
        {
            return context.Logs.Any(log => log.IdProd == idProduto);
        }
    }

    static void RegistrarLog(string codRob, string usuRob, DateTime dateLog, string processo, string infLog, int idProd)
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
