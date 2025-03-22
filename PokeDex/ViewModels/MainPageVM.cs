using MvvmHelpers;
using PokeDex.Models;
using PokeDex.Services;
using System.ComponentModel;
using System.Windows.Input;

namespace PokeDex.ViewModels
{
    public partial class MainPageVM : CommunityToolkit.Mvvm.ComponentModel.ObservableObject, INotifyPropertyChanged
    {
        private readonly IBaseRequest service;
        
        private int pageSize = 20;

        private int offset = 20;

        private int limit = 20;

        bool handling = false;

        public ObservableRangeCollection<Pokemon> pokemonORC { get; set; } = new ObservableRangeCollection<Pokemon>();

        public ICommand LoadMorePokemonsCommand { get; }

        public MainPageVM(IBaseRequest service)
        {
            this.service = service;
            LoadMorePokemonsCommand = new Command(async () => await getNextPokemonChunck());
            
        }

        public async Task InitializeAsync()
        {
            await loadPokemon();
        }

        public async Task loadPokemon()
        {
            try
            {
                var resultAPI = await service.getPokemonList<ResPokemonAPI<Pokemon>>();
                List<Pokemon> temp = resultAPI.ToList();
                var toAdd = service.buildCollectionViewRowPokemon(temp);

                Console.WriteLine(toAdd);
                pokemonORC.AddRange(toAdd.Take(pageSize));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task getNextPokemonChunck()
        {
            if (handling) return;

            handling = true;
            try
            {
                var resultAPI = await service.getPokemonList<ResPokemonAPI<Pokemon>>(this.offset, this.limit);
                List<Pokemon> temp = resultAPI.ToList();
                var toAdd = service.buildCollectionViewRowPokemon(temp);

                Console.WriteLine(toAdd);
                pokemonORC.AddRange(toAdd.Take(pageSize));
                this.offset += 20;

                handling = false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
