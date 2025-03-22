using PokeDex.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeDex.Services
{
    public interface IBaseRequest
    {
        public Task<List<Pokemon>> getPokemonList<T>(int offset = 0, int limit = 20);

        public List<PokemonRow> buildCollectionViewRowPokemon(List<Pokemon> pokemons);
    }
}
