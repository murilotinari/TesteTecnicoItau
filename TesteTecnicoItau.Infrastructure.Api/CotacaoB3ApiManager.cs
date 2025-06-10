using System.Net.Http.Json;
using TesteTecnicoItau.Domain.Interfaces.Infraestructure;
using TesteTecnicoItau.Domain.Models;

namespace TesteTecnicoItau.Infrastructure.Api
{

    public class CotacaoB3ApiManager : ICotacaoB3ApiManager
    {
        private readonly HttpClient _httpClient;

        public CotacaoB3ApiManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://b3api.vercel.app/api/");
        }

        // Método para buscar um ativo específico
        public async Task<AssetB3Dto?> BuscarAtivoAsync(string ticker)
        {
            try
            {
                var response = await _httpClient.GetAsync($"Assets/{ticker.ToUpper()}");

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    return null;

                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<AssetB3Dto>();
            }
            catch
            {
                return null;
            }
        }

        // ✅ Novo método: Buscar todos os ativos
        public async Task<List<AssetB3Dto>> BuscarTodosAtivosAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("Assets/");
                response.EnsureSuccessStatusCode();

                var ativos = await response.Content.ReadFromJsonAsync<List<AssetB3Dto>>();
                return ativos ?? new List<AssetB3Dto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao buscar ativos: {ex.Message}");
                return new List<AssetB3Dto>();
            }
        }

        public async Task<decimal> ObterPrecoAtualAsync(string ticker)
        {
            var ativo = await BuscarAtivoAsync(ticker);
            return ativo?.Price ?? 0m;
        }

    }

}
