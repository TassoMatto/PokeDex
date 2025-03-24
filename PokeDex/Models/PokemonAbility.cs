namespace PokeDex.Models
{
    public class PokemonAbility<T>
    {
        public T ability { get; set; }

        public bool isHidden { get; set; }
        public int slot { get; set; }
    }


}
