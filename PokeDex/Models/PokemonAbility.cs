namespace PokeDex.Models
{
    /// <summary>
    /// Risposta API abilità pokemon
    /// </summary>
    /// <typeparam name="T">Formato JSON abilità pokemon</typeparam>
    public class PokemonAbility<T>
    {
        /// <summary>
        /// Lista abilità
        /// </summary>
        public required T ability { get; set; }
        
        public bool isHidden { get; set; }
        public int slot { get; set; }
    }

}
