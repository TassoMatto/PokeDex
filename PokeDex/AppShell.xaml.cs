using System.Windows.Input;
using PokeDex.Views;

namespace PokeDex
{
    public partial class AppShell : Shell
    {
        public ICommand LogoutCommand { get; set; }
        
        public AppShell()
        {
            this.LogoutCommand = new Command(async () =>
            {
                Preferences.Remove("timeLogged");
                await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");   
            });
            InitializeComponent();
            
            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(LoadingPage), typeof(LoadingPage));
        }
    }
}
