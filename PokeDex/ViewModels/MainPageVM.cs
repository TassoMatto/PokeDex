using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using MvvmHelpers;
using PokeDex.Models;
using PokeDex.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Extensions.Configuration;

namespace PokeDex.ViewModels
{
    public partial class MainPageVM : CommunityToolkit.Mvvm.ComponentModel.ObservableObject
    {

#region ATTRIBUTE
 
        private readonly IPokemonService _service;
        private readonly int _pageSize = 1302;
        private uint _offset = 20;
        private readonly uint _limit = 20;
        private bool _isBusy = false;
        public ObservableRangeCollection<Pokemon> PokemonOrc { get; private set; } = [];
        public ICommand LoadMorePokemonsCommand { get; }
        public ICommand SearchPokemons { get; }
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
                    SearchPokemons?.Execute("");
                }
            }
        }
        private bool _loadingData = true;
        public bool LoadingData
        {
            get => _loadingData;
            set
            {
                if (value == _loadingData) return;
                _loadingData = value;
                OnPropertyChanged();
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
                    if(value == "") SearchPokemons?.Execute("");
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
            if (_isBusy) return;

            _isBusy = true;
            await this.LoadPokemon(this._offset, this._limit);
            this._offset += 20;
            _isBusy = false;
        }
        private async Task FilterPokemon()
        {
            // Check if exists a filter by type
            if(_selectedType?.Name != "unknown" && _selectedType?.Name != null) 
            {
                var response = await this._service.GetPokemonByTypes<ResPokemonByType>(_selectedType.Name);
                if(response?.pokemon == null) return;
                List<ListPokemonByType> listByType = response.pokemon;
                List<string> pokemonChecked = [];
                foreach (var pokemon in listByType)
                {
                    var name = pokemon?.pokemon?.name ?? "";
                    if(name != "") pokemonChecked.Add(name);
                }

                var listFilteredByType = new ObservableRangeCollection<Pokemon>(((App)Application.Current).AllPokemon);
                var indexToPop = new List<Pokemon>();
                foreach (var pokemon in listFilteredByType)
                {
                    var name = pokemon?.name ?? "";
                    if(!pokemonChecked.Contains(name))
                    {
                        if(pokemon != null) indexToPop.Add(pokemon);
                    }
                }
                foreach (var remove in indexToPop)
                {
                    listFilteredByType.Remove(remove);
                }
                this.PokemonOrc.Clear();
                if(listFilteredByType.Count != 0) this.PokemonOrc.AddRange(listFilteredByType.Take(_pageSize));
            } 
            else
            {
                this.PokemonOrc.Clear();
                PokemonOrc.AddRange(((App)Application.Current).AllPokemon.Take(_pageSize));
            }
            
            // Check if exists a filter by text typed
            if(_textChange != null && _textChange != "")
            {
                var filteredByText = this.PokemonOrc.Where(p => p.name != null && p.name.Contains(_textChange));
                var list = filteredByText.ToList();
                if(list != null) 
                {
                    PokemonOrc.Clear();
                    PokemonOrc.AddRange(list.Take(_pageSize));
                }
#if ANDROID
                var imm = (Android.Views.InputMethods.InputMethodManager)MauiApplication.Current.GetSystemService(Android.Content.Context.InputMethodService);
                if (imm != null)
                {
                    var activity = Microsoft.Maui.ApplicationModel.Platform.CurrentActivity;
                    Android.OS.IBinder wToken = activity.CurrentFocus?.WindowToken;
                    imm.HideSoftInputFromWindow(wToken, 0);
                }
#endif
            }
            
        }

#endregion PRIVATE

        public MainPageVM(IPokemonService service, IConfiguration config)
        {
            this._service = service;
            this.PokemonTypes = new ObservableRangeCollection<PokemonType>();
            LoadMorePokemonsCommand = new Command(async () => await getNextPokemonChunck());
            SearchPokemons = new Command(() =>
            {
                Task.Run(async () =>
                {
                    LoadingData = true;
                    await FilterPokemon();
                    LoadingData = false;
                });
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
                var response = await _service.GetPokemon<ResPokemonAPI<Pokemon>>(offset, limit);
                if (response == null || response.results == null) return;
                List<Pokemon> pokemonList = response.results;
                var formatedPokemonList = this.buildCollectionViewRowPokemon(pokemonList);
                PokemonOrc.AddRange(formatedPokemonList.Take(_pageSize));
                ((App) Application.Current).AllPokemon.AddRange(formatedPokemonList.Take(_pageSize));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw;
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
                var pokemonDetails = await _service.GetPokemonAbility<AbilityRes<PokemonAbility<Ability>>>(pokemonClicked.url);
                var list = pokemonDetails?.abilities?.Select(pokemonAbility => pokemonAbility.ability).ToList();
                
                return list ?? [];
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore: {ex.Message}");
                return [];
            }
        }
    
        /// <summary>
        /// Set list of all pokemon's types
        /// </summary>
        public async Task LoadPokemonTypes()
        {
            try
            {
                var response = await this._service.GetPokemonTypes<ResPokemonAPI<PokemonType>>();
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
        public void HideLoadActivity()
        {
            this.LoadingData = false;
        }
        public void ShowLoadActivity()
        {
            this.LoadingData = true;
        }
    }
}
