using PokeDex.Models;
using System.Collections.Generic;

namespace PokeDex.ViewModels
{
    public class InfoPokemonPopupVm(PokemonRow p, List<Ability> a)
    {
        public string? name { get; set; } = p.name;
        public string? img_url { get; set; } = p.img_url;
        public int? id { get; set; } = p.id;
        public List<Ability> abilities { get; set; } = a;
    }
}
