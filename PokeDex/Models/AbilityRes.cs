using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeDex.Models
{
    /// <summary>
    /// Lista delle Abilità di un pokemon
    /// </summary>
    /// <typeparam name="T">Formato JSON delle abilità ritornate dalla API</typeparam>
    public class AbilityRes<T>
    {
        /// <summary>
        /// Lista abilità
        /// </summary>
        public List<T> abilities {  get; set; }
    }
}
