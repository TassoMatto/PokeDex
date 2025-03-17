using PokeDex.ViewModels;

namespace PokeDex
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage(MainPageVM mpvm)
        {
            InitializeComponent();
            BindingContext = mpvm;
        }
    }

}
