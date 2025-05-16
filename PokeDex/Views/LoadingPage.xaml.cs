using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeDex.Views;

public partial class LoadingPage : ContentPage
{
    public LoadingPage()
    {
        InitializeComponent();
        var timeLogged = Preferences.Get("timeLogged", DateTime.MinValue.Ticks);
        if (Math.Abs(timeLogged - DateTime.Now.Ticks) < TimeSpan.FromMinutes(2).Ticks)
        {
            Preferences.Set("LoggedTime", DateTime.Now.Ticks);
            AppShell.Current.Dispatcher.Dispatch(async () =>
            {
                await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
            });
        }
        else
        {
            Preferences.Remove("timeLogged");
            AppShell.Current.Dispatcher.Dispatch(async () =>
            {
                await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
            });
        }
    }
}