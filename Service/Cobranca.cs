using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Newtonsoft.Json;
using OAuth.Model;

namespace OAuth.Service
{
    public class Cobranca
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public Cobranca(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task CriarBoletoAsync()
        {
            // Obter o token de acesso (já obtido via OAuth2)
            var accessToken = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");

            using (var client = new HttpClient())
            {
                // Adicionar o cabeçalho de autorização com o token de acesso
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Exemplo de dados de boleto (ajuste conforme a especificação da API)
                var boletoData = new
                {
                    cedente = new
                    {
                        nome = "Nome do Cedente",
                        documento = "12345678000195", // CNPJ do Cedente
                    },
                    sacado = new
                    {
                        nome = "Nome do Sacado",
                        documento = "12345678901", // CPF ou CNPJ do Sacado
                        endereco = "Rua Exemplo, 123",
                    },
                    valor = 150.00,
                    vencimento = "2024-12-15", // Data de vencimento do boleto
                    descricao = "Exemplo de cobrança",
                    nossoNumero = "123456789", // Número de controle do boleto
                    instrucoes = "Instrução para pagamento"
                };

                // Convertendo os dados para JSON
                var jsonContent = new StringContent(JsonConvert.SerializeObject(boletoData), Encoding.UTF8, "application/json");

                // Enviar a requisição POST para criar o boleto
                var response = await client.PostAsync("https://api.bancodobrasil.com.br/cobrança/v2/boletos", jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    // Processar a resposta com o boleto gerado
                    var responseContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Boleto criado com sucesso: " + responseContent);
                }
                else
                {
                    // Erro na criação do boleto, log de erro
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Erro ao criar boleto: " + errorContent);
                }
            }
        }

public async Task ListarBoletos()
{
   var accessToken = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");

    using (var client = new HttpClient())
    {
        // Adicionando o cabeçalho de autorização com o token de acesso
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        // Endpoint para listar boletos - este é um exemplo, verifique a URL exata na documentação
        var url = "https://api.bancodobrasil.com.br/cobranca/v2/boletos";

        // Enviar a requisição GET
        var response = await client.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();

            // Desserializando o JSON de resposta em uma lista de boletos
            var boletos = JsonConvert.DeserializeObject<List<Boleto>>(responseContent);

            // Exibir os boletos ou processá-los conforme necessário
            foreach (var boleto in boletos)
            {
                Console.WriteLine($"ID: {boleto.Id}, Nosso Número: {boleto.NossoNumero}, Status: {boleto.Status}, Valor: {boleto.Valor}, Vencimento: {boleto.Vencimento.ToString("dd/MM/yyyy")}");
            }
        }
        else
        {
            // Se a requisição falhar, logue o erro
            var errorContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Erro ao listar boletos: " + errorContent);
        }
    }
}

    }
}