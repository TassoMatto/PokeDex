using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeDex.Models
{
    /// <summary>
    /// Formattazione degli elementi della CollectionView
    /// </summary>
    public class PokemonRow : Pokemon
    {
        /// <summary>
        /// Id Pokemon
        /// </summary>
        public int? id { get; set; }
        
        /// <summary>
        /// Url immagine pokemon
        /// </summary>
        public string? img_url {  get; set; }
    }
}
