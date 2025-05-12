using CommunityToolkit.Mvvm.ComponentModel;
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
        private readonly int pageSize = 1302;
        private uint offset = 20;
        private readonly uint limit = 20;
        private bool isBusy = false;
        public ObservableRangeCollection<Pokemon> pokemonORC { get; private set; } = [];
        private ObservableRangeCollection<Pokemon> allPokemon { get; set; } = [];
        public ICommand LoadMorePokemonsCommand { get; }
        public ICommand SearchPokemons { get; }
        public ICommand SelectedChangeTypeCommand { get; }
        public ObservableRangeCollection<PokemonType> PokemonTypes { get; set; } = [];
        
        private PokemonType _selectedType;
        public PokemonType SelectedType 
        {
            get => _selectedType;
            set
            {
                if(_selectedType != value)
                {
                    _selectedType = value;
                    OnPropertyChanged();
                    SelectedChangeTypeCommand?.Execute(value);
                }
            }
        }
        
        private string _textChange;
        public string TextChange
        {
            get => _textChange;
            set
            {
                if(_textChange != value)
                {
                    _textChange = value;
                    OnPropertyChanged();
                    if(value == "") SearchPokemons?.Execute(value);
                }
            }
        }

#endregion ATTRIBUTE

#region PRIVATE
        
        /// <summary>
        /// Builds the list of pokemon with the image and other information
        /// </summary>
        /// <param name="pokemons"></param>
        /// <returns></returns>
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
        private async Task FilterPokemonsByTextTyped(string txt) 
        {
            if(txt == "") 
            {
                pokemonORC.Clear();
                pokemonORC.AddRange(allPokemon.Take(pageSize));
                //await FilterPokemonByTypePicked(SelectedType);
            };

            var filtered = this.pokemonORC.Where(p => p.name != null && p.name.Contains(txt));
            var list = filtered.ToList();
            if(list != null) 
            {
                pokemonORC.Clear();
                pokemonORC.AddRange(list.Take(pageSize));
            }
        }
        private async Task FilterPokemonByTypePicked(PokemonType type)
        {
            var response = await this.service.GetPokemonByTypes<ResPokemonByType>(type.Name);
            if(response.pokemon == null) return;
            List<ListPokemonByType> listByType = response.pokemon;
            List<string> pokemonChecked = new List<string>();
            foreach (var pokemon in listByType)
            {
                pokemonChecked.Add(pokemon.pokemon.name);
            }
            this.pokemonORC.Clear();
            pokemonORC.AddRange(allPokemon.Take(pageSize));
            //if(TextChange != null) await FilterPokemonsByTextTyped(TextChange);
            if(type.Name == "unknown") return;
            var indexToPop = new List<Pokemon>();
            foreach (var pokemon in pokemonORC)
            {
                if(!pokemonChecked.Contains(pokemon.name))
                {
                    indexToPop.Add(pokemon);
                }
            }
            foreach (var remove in indexToPop)
            {
                pokemonORC.Remove(remove);
            }
        }
        
#endregion PRIVATE

        public MainPageVM(IPokemonService service)
        {
            this.service = service;
            this.PokemonTypes = new ObservableRangeCollection<PokemonType>();
            LoadMorePokemonsCommand = new Command(async () => await getNextPokemonChunck());
            SelectedChangeTypeCommand = new Command<PokemonType>((type) => 
            {
                _ = Task.Run(async () => { await FilterPokemonByTypePicked(type); });
            }
            );
            SearchPokemons = new Command<string>((txt) => 
            {
                Task.Run(async () => { await FilterPokemonsByTextTyped(txt); });
            });
        }

        /// <summary>
        /// Get Pokemon list by <paramref name="limit"/> elements, start from <paramref name="offset"/>
        /// </summary>
        /// <param name="offset">Get Pokemon start from</param>
        /// <param name="limit">Max Numbers of Returned Pokemon</param>
        public async Task LoadPokemon(uint offset = 0, uint limit = 1302)
        {
            try
            {
                var response = await service.GetPokemon<ResPokemonAPI<Pokemon>>(offset, limit);
                if (response == null || response.results == null) return;
                List<Pokemon> pokemonList = response.results;
                var formatedPokemonList = this.buildCollectionViewRowPokemon(pokemonList);
                pokemonORC.AddRange(formatedPokemonList.Take(pageSize));
                allPokemon.AddRange(formatedPokemonList.Take(pageSize));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore: {ex.Message}");
            }
        }

        /// <summary>
        /// Returns the list of the <paramref name="pokemonClicked"/>'s abilities
        /// </summary>
        /// <param name="pokemonClicked">Clicked Pokemon by user</param>
        /// <returns>List contains Pokemon's abilities</returns>
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
    
        public async Task LoadPokemonTypes()
        {
            try
            {
                var response = await this.service.GetPokemonTypes<ResPokemonAPI<PokemonType>>();
                if(response?.results != null) 
                {
                    var types_list = response.results;
                    this.PokemonTypes.AddRange(types_list.Take(21));
                }   
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore: {ex.Message}");
                return;
            }
        }
    }
}
