using System.Diagnostics;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Messaging;
using MauiPopup;
using PokeDex.Models;
using PokeDex.Services;
using PokeDex.ViewModels;

namespace PokeDex
{
    public partial class MainPage : ContentPage
    {

        public ICommand OpenPokemonDetailsCommand  { get; private set; }
        private readonly IPokemonService service;

        private readonly MainPageVM mpvm;

        private bool once = false;

        private bool isOpening = false;

        public MainPage(MainPageVM mpvm, IPokemonService service)
        {
            OpenPokemonDetailsCommand = new Command<PokemonRow>(async (pokemonClicked) => 
            {
                if(isOpening) return;
                DataLoadIndicator.IsVisible = true;
                DataLoadIndicatorBackground.IsVisible = true;
                isOpening = true;
                List<Ability> list;    
                if(this.mpvm != null && pokemonClicked != null) list = await this.mpvm.GetPokemonAbility(pokemonClicked);
                else list = new List<Ability>();

                if (pokemonClicked != null)
                {
                    await Navigation.PushModalAsync(new InfoPokemonPopup(pokemonClicked, list));
                }
                DataLoadIndicator.IsVisible = false;
                DataLoadIndicatorBackground.IsVisible = false;
                isOpening = false;
            });

            InitializeComponent();
            
            this.mpvm = mpvm;
            BindingContext = this.mpvm;
            this.service = service;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if(!once) 
            {
                Task.Run(async () => 
                {
                    await mpvm.LoadPokemon();
                    await mpvm.LoadPokemonTypes();
                    DataLoadIndicator.IsVisible = false;
                    DataLoadIndicatorBackground.IsVisible = false;
                });
                
                once = true;
            }
        }

    }

}
