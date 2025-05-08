using System.Diagnostics;
using System.Windows.Input;
using MauiPopup;
using PokeDex.Models;
using PokeDex.Services;
using PokeDex.ViewModels;

namespace PokeDex
{
    public partial class MainPage : ContentPage
    {

        public ICommand OpenPokemonDetailsCommand  {get; private set;}
        private readonly IPokemonService service;

        private readonly MainPageVM mpvm;

        public MainPage(MainPageVM mpvm, IPokemonService service)
        {
            
            BindingContext = mpvm;
            this.mpvm = mpvm;
            OpenPokemonDetailsCommand = new Command<PokemonRow>(async (pokemonClicked) => 
            {
                List<Ability> list;
                
                if(this.mpvm != null && pokemonClicked != null) list = await this.mpvm.GetPokemonAbility(pokemonClicked);
                else list = new List<Ability>();

                if (pokemonClicked != null)
                {
                    _ = PopupAction.DisplayPopup(new InfoPokemonPopup(pokemonClicked, list));
                }
            });

            InitializeComponent();
            

            // Notazione per ignorare il risultato del Task 
            this.service = service;
            _ = mpvm.LoadPokemon();
        }

        // public ICommand OpenPokemonDetailsCommand 
        // { 
        //     get 
        //     {
        //         return new Command(
        //             async (sender) => 
        //             {
        //                 List<Ability> list;
        //                 PokemonRow? pokemonClicked = sender as PokemonRow;
        //                 MainPageVM? mpvm = BindingContext as MainPageVM;
        //                 if(mpvm != null && pokemonClicked != null) list = await mpvm.GetPokemonAbility(pokemonClicked);
        //                 else list = new List<Ability>();

        //                 if (pokemonClicked != null)
        //                 {
        //                     _ = PopupAction.DisplayPopup(new InfoPokemonPopup(pokemonClicked, list));
        //                 }
        //             });
        //     }
        // }

    }

}
