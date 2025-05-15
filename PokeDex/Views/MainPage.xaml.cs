using System.Windows.Input;
using Google.Android.Material.Snackbar;
using PokeDex.Models;
using PokeDex.Services;
using PokeDex.ViewModels;

namespace PokeDex.Views
{
    public partial class MainPage : ContentPage
    {

        public ICommand OpenPokemonDetailsCommand  { get; private set; }
        private readonly IPokemonService _service;

        private readonly MainPageVM _mpvm;

        private bool _once = false;

        private bool _isOpening = false;

        public MainPage(MainPageVM mpvm, IPokemonService service)
        {
            OpenPokemonDetailsCommand = new Command<PokemonRow>(async void (pokemonClicked) =>
            {
                try
                {
                    if (_isOpening) return;
                    _mpvm.ShowLoadActivity();
                    _isOpening = true;

                    List<Ability> list;
                    if (this._mpvm != null && pokemonClicked != null)
                        list = await this._mpvm.GetPokemonAbility(pokemonClicked);
                    else list = [];
                    
                    if (pokemonClicked != null)
                        await Navigation.PushModalAsync(new InfoPokemonPopup(pokemonClicked, list));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                finally
                {
                    _mpvm.HideLoadActivity();
                    _isOpening = false;
                }
            });

            InitializeComponent();
            
            this._mpvm = mpvm;
            BindingContext = this._mpvm;
            this._service = service;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (_once) return;
            Task.Run(async () => 
            {
                try
                {
                    await _mpvm.LoadPokemon();
                    await _mpvm.LoadPokemonTypes();
                    _mpvm.HideLoadActivity();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            });
            
            _once = true;
        }

        public void OnButtonClicked(Object sender, EventArgs e)
        {
            PokemonCollectionView.ScrollTo(0, animate: false);
        }

    }

}
