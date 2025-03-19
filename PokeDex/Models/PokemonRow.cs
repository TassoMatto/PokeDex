using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeDex.Models
{
    public class PokemonRow : Pokemon
    {
        public int? id { get; set; }

        public string? img_url {  get; set; }
    }
}
