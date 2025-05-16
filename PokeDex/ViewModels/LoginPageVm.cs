using System.Windows.Input;
using PokeDex.Services;
using PokeDex.Views;

namespace PokeDex.ViewModels;

public class LoginPageVm : CommunityToolkit.Mvvm.ComponentModel.ObservableObject
{
    private readonly ILoginService _service;
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
    /// <summary>
    /// User login into Pokedex
    /// </summary>
    public async Task<bool> LoginUser()
    {
        if (Username == "" || Password == "") return false;

        return this._service.CheckAutentication(Username, Password);
    }

    public LoginPageVm(ILoginService service)
    {
        this._service = service;
    }

}