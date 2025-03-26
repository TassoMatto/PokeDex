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

        /// <summary>
        /// Restituisce la lista di pokemon a blocchi di <paramref name="limit"/> a partire da <paramref name="offset"/>
        /// </summary>
        /// <typeparam name="T">Formato delle info dei pokemon da recuperare</typeparam>
        /// <param name="offset">Offset</param>
        /// <param name="limit">Numero max pokemon da restituire</param>
        /// <returns></returns>
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
                else if (limit > 0) sb.Append("?limit=").Append(limit);


                string url = sb.ToString();
                var result = await _httpClient.GetAsync(url);

                // Richiesta HTTPS andata a buon fine
                // SUCCESS: ritorno la lista JSON dei primi pokemon
                // ERROR: restituisco la lista vuota
                if (result.IsSuccessStatusCode)
                {
                    var response = await result.Content.ReadFromJsonAsync<ResPokemonAPI<Pokemon>>();
                    return response?.results ?? new List<Pokemon>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore: {ex.Message}");
            }
            
            return new List<Pokemon>();
        }

        /// <summary>
        /// Data una lista <paramref name="pokemons"/> costruisce la CollectionView per visualizzare i pokemon
        /// </summary>
        /// <param name="pokemons">Lista di pokemon</param>
        /// <returns></returns>
        public List<PokemonRow> buildCollectionViewRowPokemon(List<Pokemon> pokemons)
        {
            try
            {
                // Filtro pokemon nulli
                // Effettuo cast elementi da PokemonRow? a PokemonRow 
                var toAdd = pokemons.Select(jsonRes =>
                {
                    if (jsonRes.url == null)
                    {
                        return null;
                    }
                    string[] parts = (new Uri(jsonRes.url)).Segments;
                    int id = parts.Count() != 0 ? Int32.Parse(parts[^1].Replace("/", "")) : -1;
                    return new PokemonRow
                    {
                        name = jsonRes.name,
                        url = jsonRes.url,
                        img_url = $"https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/{id}.png",
                        id = id,
                    };
                }).Where(p => p != null).Cast<PokemonRow>().ToList();

                return toAdd;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore: {ex.Message}");
            }

            return new List<PokemonRow>();
        }

        /// <summary>
        /// Restitusci la lista delle abilità del pokemon dato <paramref name="url"/>
        /// </summary>
        /// <param name="url">Indirizzo delle abilità</param>
        /// <returns></returns>
        public async Task<List<Ability>> GiveAbilitiesOfPokemon(string? url)
        {

            if (url == null) return new List<Ability>();

            try
            {
                var result = await _httpClient.GetAsync(url);
                if (result.IsSuccessStatusCode)
                {
                    var response = await result.Content.ReadFromJsonAsync<AbilityRes<PokemonAbility<Ability>>>();
                    if (response == null) throw new NullReferenceException();
                    if (response.abilities == null) return new List<Ability>();
                    return response.abilities.Select(pokemonAbility => pokemonAbility.ability).ToList();
                }

                return new List<Ability>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore: {ex.Message}");
            }

            return new List<Ability>();
        }

    }
}
