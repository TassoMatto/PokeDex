using System.Collections.Specialized;
using System.Net.Http.Json;

namespace PokeDex.Services 
{
    public class PokemonService(HttpClient hc) : IPokemonService
    {

        private readonly HttpClient _httpClient = hc;

        private readonly string pokemonListUrl = "https://pokeapi.co/api/v2/pokemon";
        private readonly string pokemonTypesListUrl = "https://pokeapi.co/api/v2/type/";

        private async Task<T> HTTPGetRequest<T>(string url) where T : new ()
        {
            try
            {
                var result = await _httpClient.GetAsync(url);
                if(result.IsSuccessStatusCode) 
                {
                    var response = await result.Content.ReadFromJsonAsync<T>();
                    if(response == null) return new T ();
                    return response;
                }
                else
                {
                    return new T ();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore: {ex.Message}");
                return new T ();
            }
            
        }

        /// <summary>
        /// Get the list of pokemon from <paramref name="offset"/> for a maximum of <paramref name="limit"/> pokemon
        /// </summary>
        /// <typeparam name="T">API Returned Model</typeparam>
        /// <param name="offset">Data search offset</param>
        /// <param name="limit">Max Number of Pokemon</param>
        /// <returns>API Response Data Model</returns>
        public async Task<T> GetPokemon<T>(uint offset = 0, uint limit = 20) where T : new()
        {
            try
            {
                // HTTP GET Query Building
                NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);
                queryString.Add("offset", offset.ToString());
                queryString.Add("limit", limit.ToString());
 
                string url = this.pokemonListUrl + '?' + queryString.ToString() ?? "";
                return await HTTPGetRequest<T>(url);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore: {ex.Message}");
                return new T ();
            }
        }

        /// <summary>
        /// Gets a Pokemon's abilities from <paramref name="url_ability"/>
        /// </summary>
        /// <typeparam name="T">API Returned Model</typeparam>
        /// <param name="url_ability">Pokemon Skill Url</param>
        /// <returns>API Response Data Model</returns>
        public async Task<T> GetPokemonAbility<T>(string url_ability) where T : new()
        {
            // Args checking
            if(url_ability.Equals("")) 
            {
                Console.WriteLine("url_ability empty");
                return new T ();
            }

            try
            {
                return await HTTPGetRequest<T>(url_ability);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore: {ex.Message}");
                return new T ();
            }
        }
    
        public async Task<T> GetPokemonTypes<T>() where T : new()
        {
            var response = await HTTPGetRequest<T>(pokemonTypesListUrl);
            if(response == null) return new T ();
            else return response;
        }

        public async Task<T> GetPokemonByTypes<T>(string type) where T : new()
        {
            if(type == "") return new T ();
            var response = await HTTPGetRequest<T>(pokemonTypesListUrl + type);
            if(response == null) return new T ();
            else return response;
        }
    }
}