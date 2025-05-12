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
                isOpening = true;
                List<Ability> list;    
                if(this.mpvm != null && pokemonClicked != null) list = await this.mpvm.GetPokemonAbility(pokemonClicked);
                else list = new List<Ability>();

                if (pokemonClicked != null)
                {
                    await Navigation.PushModalAsync(new InfoPokemonPopup(pokemonClicked, list));
                    //await Navigation.PushAsync(new InfoPokemonPopup(pokemonClicked, list));
                }
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
                _ = mpvm.LoadPokemon();
                _ = mpvm.LoadPokemonTypes();
                once = true;
            }
        }

        public void OnPokemonTypeSelectionIndexChange(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            Console.WriteLine(picker.SelectedItem);
        }

    }

}
