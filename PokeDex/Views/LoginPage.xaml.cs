using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokeDex.ViewModels;

namespace PokeDex.Views;

public partial class LoginPage : ContentPage
{
    public LoginPage(LoginPageVm vm)
    {
        InitializeComponent();
        this.BindingContext = vm;
    }
}