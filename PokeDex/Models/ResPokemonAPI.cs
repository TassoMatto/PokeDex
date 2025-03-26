using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeDex.Models
{

    /// <summary>
    /// Risposta API lista pokemon
    /// </summary>
    /// <typeparam name="T">Struttura JSON delle info di ogni pokemon</typeparam>
    public class ResPokemonAPI<T>
    {
        /// <summary>
        /// Contatore totale Pokemon recuperabili tramiti API
        /// </summary>
        public int? count { get; set; }
        /// <summary>
        /// Puntatore URL alla prossima lista di pokemon (viene caricato a chunck di 20) di default
        /// </summary>
        public string? next { get; set; }
        /// <summary>
        /// Puntatore alla precedente lista
        /// </summary>
        public string? previous { get; set; }
        /// <summary>
        /// Lista dei primi pokemon ritornati
        /// </summary>
        public List<T>? results { get; set; }

    }
}
