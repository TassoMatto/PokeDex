using System.Collections.Specialized;
using System.Net.Http.Json;

namespace PokeDex.Services 
{
    public class PokemonService : IPokemonService
    {
        private const string pokemonListUrl = "https://pokeapi.co/api/v2/pokemon";
        private const string pokemonTypesListUrl = "https://pokeapi.co/api/v2/type/";

        /// <summary>
        /// Send a Get Http Request to <paramref name="url"/>
        /// </summary>
        /// <param name="url">Request URL</param>
        /// <typeparam name="T">Response Model</typeparam>
        /// <returns>JSON Model</returns>
        private static async Task<T> HttpGetRequest<T>(string url) where T : new ()
        {
            try
            {
                using var client = new HttpClient();
                var result = await client.GetAsync(url);
                if(result.IsSuccessStatusCode) 
                {
                    var response = await result.Content.ReadFromJsonAsync<T>();
                    return response ?? new T ();
                }
                else
                {
                    return new T ();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw;
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
                var queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);
                queryString.Add("offset", offset.ToString());
                queryString.Add("limit", limit.ToString());
 
                var url = pokemonListUrl + '?' + queryString.ToString() ?? "";
                return await HttpGetRequest<T>(url);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Gets a Pokemon's abilities from <paramref name="urlAbility"/>
        /// </summary>
        /// <typeparam name="T">API Returned Model</typeparam>
        /// <param name="urlAbility">Pokemon Skill Url</param>
        /// <returns>API Response Data Model</returns>
        public async Task<T> GetPokemonAbility<T>(string urlAbility) where T : new()
        {
            // Args checking
            if(urlAbility.Equals("")) 
            {
                Console.WriteLine("url_ability empty");
                return new T ();
            }

            try
            {
                return await HttpGetRequest<T>(urlAbility);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
        }
    
        /// <summary>
        /// Get List of pokemon's types
        /// </summary>
        /// <typeparam name="T">Response Model</typeparam>
        /// <returns>List of all pokemon's types</returns>
        public async Task<T> GetPokemonTypes<T>() where T : new()
        {
            try
            {
                var response = await HttpGetRequest<T>(pokemonTypesListUrl);
                return response ?? new T();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// Return pokemon's list having that <paramref name="type"/>
        /// </summary>
        /// <param name="type">Pokemon's type</param>
        /// <typeparam name="T">Response JSON</typeparam>
        /// <returns>Pokemon's list by type</returns>
        public async Task<T> GetPokemonByTypes<T>(string type) where T : new()
        {
            try
            {
                if(type == "") return new T ();
                var response = await HttpGetRequest<T>(pokemonTypesListUrl + type);
                return response ?? new T ();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}