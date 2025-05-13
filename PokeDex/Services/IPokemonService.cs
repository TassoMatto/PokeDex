using PokeDex.Models;

namespace PokeDex.Services
{
    public interface IPokemonService
    {
        public Task<T> GetPokemon<T>(uint offset = 0, uint limit = 20) where T : new();
        
        public Task<T> GetPokemonAbility<T>(string url_ability) where T : new(); 

        public Task<T> GetPokemonTypes<T>() where T : new();

        public Task<T> GetPokemonByTypes<T>(string type) where T : new();
    }
}