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
using System.Xml.Linq;

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
            loadPokemon();
        }

        public async void loadPokemon()
        {
            try
            {
                var resultAPI = await service.getPokemonList<ResPokemonAPI<Pokemon>>();
                List<Pokemon> temp = resultAPI.ToList();
                var toAdd = temp.Select(jsonRes => {
                    if(Nullable.Equals(jsonRes.url, null)) {
                        return null;
                    }
                    string[] parts = (new Uri(jsonRes.url)).Segments;
                    int id = parts.Count() != 0 ? Int32.Parse(parts[^1].Replace("/", ""))  : -1;
                    return new PokemonRow
                    {
                        name = jsonRes.name,
                        url = jsonRes.url,
                        img_url = $"https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/{id}.png",
                        id = id,
                    };
                }).ToList();

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
                var toAdd = temp.Select(jsonRes => {
                    if (Nullable.Equals(jsonRes.url, null))
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
                }).ToList();

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
