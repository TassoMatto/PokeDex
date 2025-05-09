using PokeDex.Models;

namespace PokeDex.Services
{
    public interface IPokemonService
    {
        public Task<T?> GetPokemon<T>(uint offset = 0, uint limit = 20);
        
        public Task<T?> GetPokemonAbility<T>(string url_ability);

        public Task<T> GetPokemonTypes<T>();

    }
}