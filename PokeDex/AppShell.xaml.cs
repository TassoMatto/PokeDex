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
                await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");   
            });
            InitializeComponent();
            
            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
        }
    }
}
