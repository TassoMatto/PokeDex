using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using PokeDex.ViewModels;

namespace PokeDex.Views;

public partial class LoginPage : ContentPage
{
    public ICommand UserLoginCommand { get; set; }
    private LoginPageVm _lpvm;
    
    public LoginPage(LoginPageVm vm)
    {
        this.UserLoginCommand = new Command(async () =>
        {
            var isLogged = await _lpvm.LoginUser();
            if (isLogged) await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
        });
        InitializeComponent();
        this.BindingContext = vm;
        this._lpvm = vm;
    }
}