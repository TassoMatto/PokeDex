using MauiPopup;
using PokeDex.Models;
using PokeDex.ViewModels;

namespace PokeDex
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainPageVM mpvm)
        {
            InitializeComponent();
            BindingContext = mpvm;

            // Notazione per ignorare il risultato del Task 
            _ = mpvm.loadPokemon();
        }

        public void OnClickPokemon(object sender, SelectionChangedEventArgs e)
        {
            // Ricavo il pokemon cliccato nella Collection
            var clickedPokemon = e.CurrentSelection.FirstOrDefault();
            if (clickedPokemon != null && clickedPokemon is PokemonRow)
            {
                PopupAction.DisplayPopup(new InfoPokemonPopup());
                // Se il pokemon cliccato c'è ed è come nel modello allora apro il popup con alcuni dettagli
                // Ricavo i dettagli dal service 
            }
        }
    }

}
