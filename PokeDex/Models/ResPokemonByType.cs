using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeDex.Models
{
    public class ResPokemonByType
    {
        public List<ListPokemonByType> pokemon { get; set; }
    }

    public class ListPokemonByType 
    {
        public Pokemon pokemon { get; set; }

        public int slot { get; set; }
    }
}