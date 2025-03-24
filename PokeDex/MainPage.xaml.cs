using MauiPopup;
using PokeDex.Models;
using PokeDex.Services;
using PokeDex.ViewModels;

namespace PokeDex
{
    public partial class MainPage : ContentPage
    {
        private IBaseRequest service;

        public MainPage(MainPageVM mpvm, IBaseRequest service)
        {
            InitializeComponent();
            BindingContext = mpvm;

            // Notazione per ignorare il risultato del Task 
            this.service = service;
            _ = mpvm.loadPokemon();
        }

        public async void OnClickPokemon(object sender, SelectionChangedEventArgs e)
        {
            var clickedPokemon = e.CurrentSelection.FirstOrDefault();
            if (clickedPokemon != null && clickedPokemon is PokemonRow pokemon)
            {
                var pokemonDetails = await service.GiveAbilitiesOfPokemon(pokemon.url);
                var list = pokemonDetails.ToList();

                // Passo i dettagli al popup
                PopupAction.DisplayPopup(new InfoPokemonPopup(pokemon, list));
            }
        }

    }

}
