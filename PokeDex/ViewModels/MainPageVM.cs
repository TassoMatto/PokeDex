using CommunityToolkit.Mvvm.Input;
using MvvmHelpers;
using PokeDex.Models;
using PokeDex.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PokeDex.ViewModels
{
    public partial class MainPageVM : CommunityToolkit.Mvvm.ComponentModel.ObservableObject, INotifyPropertyChanged
    {
        private readonly IBaseRequest service;
        
        private int pageSize = 20;

        private int offset = 20;

        private int limit = 20;

        //List<Pokemon> pokeList;  
        public ObservableRangeCollection<Pokemon> pokemonORC { get; set; } = new ObservableRangeCollection<Pokemon>();

        public ICommand LoadMorePokemonsCommand { get; }

        public MainPageVM(IBaseRequest service)
        {
            this.service = service;
            LoadMorePokemonsCommand = new Command(async () => await getNextPokemonChunck());
            loadPokemon();
        }

        public async void loadPokemon()
        {
            try
            {
                var resultAPI = await service.getPokemonList<ResPokemonAPI<Pokemon>>();
                pokemonORC.AddRange(resultAPI.Take(pageSize)); //! Bisogna capire come fare
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        [RelayCommand]
        public async Task getNextPokemonChunck()
        {
            try
            {
                var resultAPI = await service.getPokemonList<ResPokemonAPI<Pokemon>>(this.offset, this.limit);
                pokemonORC.AddRange(resultAPI.Take(pageSize));
                this.offset += 20;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
