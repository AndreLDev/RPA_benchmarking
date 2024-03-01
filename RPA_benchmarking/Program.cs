using Newtonsoft.Json;
using OpenQA.Selenium.Internal.Logging;
using RPA_benchmarking.Benchmarking;
using RPA_benchmarking.Class;
using System.Net.Http.Headers;
using System.Text;

class Program
{
    // Lista para armazenar produtos já verificados
    static List<Produto> produtosVerificados = new List<Produto>();

    static void Main(string[] args)
    {
        // Definir o intervalo de tempo para 5 minutos (300.000 milissegundos)
        int intervalo = 6000;

        // Criar um temporizador que dispara a cada 5 minutos
        Timer timer = new Timer(VerificarNovoProduto, null, 0, intervalo);

        // Manter a aplicação rodando
        Console.WriteLine("Pressione qualquer tecla para sair...");
        Console.ReadKey();
    }

    static async void VerificarNovoProduto(object state)
    {
        string username = "11164448";
        string senha = "60-dayfreetrial";
        string url = "http://regymatrix-001-site1.ktempurl.com/api/v1/produto/getall";

        try
        {
            // Criar um objeto HttpClient
            using (HttpClient client = new HttpClient())
            {
                // Adicionar as credenciais de autenticação básica
                var byteArray = Encoding.ASCII.GetBytes($"{username}:{senha}");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

                // Fazer a requisição GET à API
                HttpResponseMessage response = await client.GetAsync(url);

                // Verificar se a requisição foi bem-sucedida (código de status 200)
                if (response.IsSuccessStatusCode)
                {
                    // Ler o conteúdo da resposta como uma string
                    string responseData = await response.Content.ReadAsStringAsync();

                    // Processar os dados da resposta
                    List<Produto> novosProdutos = ObterNovosProdutos(responseData);
                    foreach (Produto produto in novosProdutos)
                    {
                        if (!produtosVerificados.Exists(p => p.Id == produto.Id))
                        {
                            // Se é um novo produto, faça algo com ele
                            Console.WriteLine($"Novo produto encontrado: ID {produto.Id}, Nome: {produto.Nome}");
                            // Adicionar o produto à lista de produtos verificados
                            produtosVerificados.Add(produto);

                            // Registra um log no banco de dados apenas se o produto for novo
                            if (!ProdutoJaRegistrado(produto.Id))
                            {
                                RegistrarLog("0001", "andre", DateTime.Now, "ConsultaAPI - Verificar Produto", "Sucesso", produto.Id);

                                MercadoLivreScraper mercadoLivreScraper = new MercadoLivreScraper();
                                var mercadoPrice = mercadoLivreScraper.ObterPreco(produto.Nome, produto.Id);

                                MagazineLuizaScraper magazineLuizaScraper = new MagazineLuizaScraper();
                                var magazinePrice = magazineLuizaScraper.ObterPreco(produto.Nome, produto.Id);

                                Benchmarking benchmarking = new Benchmarking();
                                var menor = benchmarking.CompararValor(mercadoPrice.Price, magazinePrice.Price, produto.Id);

                                string melhorCompraTexto = menor == 0 ? "MagazineLuiza - <a href=" + magazinePrice.Url + " >Clique aqui</a>"  : menor == 1 ? "Mercado Livre -  <a href=" + mercadoPrice.Url + " >Clique aqui</a>" : "O Preço é igual nos dois." ;

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
                                    <p>Produto: {mercadoPrice.Title}</p>
                                    <p>Preço: R$ {mercadoPrice.Price}</p>
                                    <h2>MagazineLuiza</h2>
                                    <p>Produto: {magazinePrice.Title}</p>
                                    <p>Preço: {magazinePrice.Price}</p>
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
                                         "wallace@docente.senai.br"

                                    },
                                    subject: "Resultado da Comparação de Preço",
                                    body: corpoEmailHtml);
                            }
                        }
                    }
                }
                else
                {
                    // Imprimir mensagem de erro caso a requisição falhe
                    Console.WriteLine($"Erro: {response.StatusCode}");
                }
            }
        }
        catch (Exception ex)
        {
            // Imprimir mensagem de erro caso ocorra uma exceção
            Console.WriteLine($"Erro ao fazer a requisição: {ex.Message}");
        }
    }

    // Método para processar os dados da resposta e obter produtos
    static List<Produto> ObterNovosProdutos(string responseData)
    {
        // Desserializar os dados da resposta para uma lista de produtos
        List<Produto> produtos = JsonConvert.DeserializeObject<List<Produto>>(responseData);
        return produtos;
    }

    // Método para verificar se o produto já foi registrado no banco de dados
    static bool ProdutoJaRegistrado(int idProduto)
    {
        using (var context = new CrawlerContext())
        {
            return context.Logs.Any(log => log.IdProd == idProduto);
        }
    }

    // Método para registrar um log no banco de dados
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
