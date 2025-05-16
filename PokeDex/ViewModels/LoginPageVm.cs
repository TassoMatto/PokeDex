using System.Windows.Input;
using PokeDex.Services;
using PokeDex.Views;

namespace PokeDex.ViewModels;

public class LoginPageVm : CommunityToolkit.Mvvm.ComponentModel.ObservableObject
{
    private readonly ILoginService _service;
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
    public ICommand UserLoginCommand { get; set; }

#region PRIVATE

    /// <summary>
    /// User login into Pokedex
    /// </summary>
    private async Task LoginUser()
    {
        if (Username == "" || Password == "") return;
        
        var loginDone = this._service.CheckAutentication(Username, Password);
        if (loginDone)
        {
            await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
        }
        
    }

#endregion PRIVATE

    public LoginPageVm(ILoginService service)
    {
        this._service = service;
        this.UserLoginCommand = new Command(async () =>
        {
            await LoginUser();
        });
    }

}