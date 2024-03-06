using RPA_benchmarking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPA_benchmarking.Services
{
    public class VerificacaoProdutoService
    {
        public static async Task VerificarNovoProdutoAsync(string apiUrl, string username, string password, List<Produto> produtosVerificados, string emailName)
        {
            try
            {
                HttpResponseMessage response = await ApiService.GetApiDataAsync(apiUrl, username, password);

                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();
                    List<Produto> novosProdutos = ProdutoService.ObterNovosProdutos(responseData);

                    foreach (Produto produto in novosProdutos)
                    {
                        if (!ProdutoService.ProdutoJaRegistrado(produto.Id))
                        {
                            Console.WriteLine($"Novo produto encontrado: ID {produto.Id}, Nome: {produto.Nome}");
                            produtosVerificados.Add(produto);
                            await LogService.RegistrarLogAsync("3416", "andreLuiz", DateTime.Now, "ConsultaAPI - Verificar Produto", "Sucesso", produto.Id);

                            await ProdutoService.ProcessarProdutoAsync(produto, emailName);
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"Erro: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao fazer a requisição: {ex.Message}");
            }
        }
    }
}
