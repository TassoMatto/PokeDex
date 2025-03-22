using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeDex.Models
{
    public class ResPokemonAPI<T>
    {

        public int? count { get; set; }                     // Contatore totale Pokemon recuperabili tramiti API
        public string? next { get; set; }                   // Puntatore URL alla prossima lista di pokemon (viene caricato a chunck di 20) di default
        public string? previous { get; set; }               // Puntatore alla precedente lista
        public List<T>? results { get; set; }                // Lista dei primi pokemon ritornati

    }
}
