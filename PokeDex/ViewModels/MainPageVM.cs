using MvvmHelpers;
using PokeDex.Models;
using PokeDex.Services;
using System.ComponentModel;
using System.Windows.Input;

namespace PokeDex.ViewModels
{
    public partial class MainPageVM : CommunityToolkit.Mvvm.ComponentModel.ObservableObject, INotifyPropertyChanged
    {

#region ATTRIBUTE

        private readonly IPokemonService service;
        private int pageSize = 20;
        private uint offset = 20;
        private uint limit = 20;
        private bool isBusy = false;
        public ObservableRangeCollection<Pokemon> pokemonORC { get; set; } = new ObservableRangeCollection<Pokemon>();
        public ICommand LoadMorePokemonsCommand { get; }
        

#endregion ATTRIBUTE

#region PRIVATE
        
        private List<PokemonRow> buildCollectionViewRowPokemon(List<Pokemon> pokemons)
        {
            try
            {
                var toAdd = pokemons.Select(jsonRes =>
                {
                    if (jsonRes.url == null)
                    {
                        return null;
                    }
                    string[] parts = (new Uri(jsonRes.url)).Segments;
                    int id = parts.Count() != 0 ? Int32.Parse(parts[^1].Replace("/", "")) : -1;
                    return new PokemonRow
                    {
                        name = jsonRes.name,
                        url = jsonRes.url,
                        img_url = $"https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/{id}.png",
                        id = id,
                    };
                }).Where(p => p != null).Cast<PokemonRow>().ToList();

                return toAdd;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore: {ex.Message}");
            }

            return new List<PokemonRow>();
        }
        
        private async Task getNextPokemonChunck()
        {
            if (isBusy) return;

            isBusy = true;
            await this.LoadPokemon(this.offset, this.limit);
            this.offset += 20;
            isBusy = false;
        }

#endregion PRIVATE

        public MainPageVM(IPokemonService service)
        {
            this.service = service;
            LoadMorePokemonsCommand = new Command(async () => await getNextPokemonChunck());
        }

        public async Task LoadPokemon(uint offset = 0, uint limit = 20)
        {
            try
            {
                var response = await service.GetPokemon<ResPokemonAPI<Pokemon>>(offset, limit);
                if (response == null || response.results == null) return;
                List<Pokemon> pokemonList = response.results;
                var formatedPokemonList = this.buildCollectionViewRowPokemon(pokemonList);
                pokemonORC.AddRange(formatedPokemonList.Take(pageSize));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore: {ex.Message}");
            }
        }

        public async Task<List<Ability>> GetPokemonAbility(PokemonRow pokemonClicked) 
        {
            try
            {
                if(pokemonClicked.url == null) return [];
                var pokemonDetails = await service.GetPokemonAbility<AbilityRes<PokemonAbility<Ability>>>(pokemonClicked.url);
                var list = pokemonDetails?.abilities?.Select(pokemonAbility => pokemonAbility.ability).ToList();
                return list ?? [];
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore: {ex.Message}");
                return [];
            }
        }
    }
}
