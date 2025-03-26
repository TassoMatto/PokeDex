using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeDex.Models
{
    /// <summary>
    /// Struttura base pokemon API
    /// </summary>
    public class Pokemon
    {
        /// <summary>
        /// Nome pokemon
        /// </summary>
        public string? name {  get; set; }
        /// <summary>
        /// Url abilità pokemon
        /// </summary>
        public string? url { get; set; }
    }
}
