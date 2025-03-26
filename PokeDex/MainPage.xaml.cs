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

        /// <summary>
        /// Al click su un pokemon apre un popup di dettaglio
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void OnClickPokemon(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var clickedPokemon = e.CurrentSelection.FirstOrDefault();
                if (clickedPokemon != null && clickedPokemon is PokemonRow pokemon)
                {
                    var pokemonDetails = await service.GiveAbilitiesOfPokemon(pokemon.url);
                    var list = pokemonDetails.ToList();

                    // Passo i dettagli al popup
                    if (pokemon != null)
                    {
                        _ = PopupAction.DisplayPopup(new InfoPokemonPopup(pokemon, list));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore: {ex.Message}");
            }
            
        }

    }

}
