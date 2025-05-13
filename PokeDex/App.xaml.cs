using MvvmHelpers;
using PokeDex.Models;

namespace PokeDex
{
    public partial class App : Application
    {

        public ObservableRangeCollection<Pokemon> AllPokemon { get; set; } = [];

        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}