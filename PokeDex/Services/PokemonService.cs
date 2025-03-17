using PokeDex.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Json;

namespace PokeDex.Services
{
    public class PokemonService : IBaseRequest
    {
        private readonly HttpClient _httpClient;

        public PokemonService(HttpClient hc) 
        {
            this._httpClient = hc;
        }

        public async Task<List<Pokemon>> getPokemonList<T>(int offset = 0, int limit = 20)
        {
            try
            {
                StringBuilder sb = new StringBuilder("https://pokeapi.co/api/v2/pokemon");

                if (offset > 0)
                {
                    sb.Append("?offset=").Append(offset);
                    if (limit > 0) sb.Append("&limit=").Append(limit);
                }
                else if(limit > 0) sb.Append("?limit=").Append(limit);


                string url = sb.ToString();
                var result = await _httpClient.GetAsync(url);
                
                // Richiesta HTTPS andata a buon fine
                // SUCCESS: ritorno la lista JSON dei primi pokemon
                // ERROR: restituisco la lista vuota
                if(result.IsSuccessStatusCode)
                {
                    var response = await result.Content.ReadFromJsonAsync<ResPokemonAPI<Pokemon>>();
                    return response?.results ?? new List<Pokemon>();
                }

                return new List<Pokemon>();
            }
            catch (Exception e)
            {
                // NOTIFICARE ERRORE

                throw;
            }
        }
    }
}
