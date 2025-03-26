using MauiPopup.Views;
using PokeDex.Models;
using PokeDex.ViewModels;

namespace PokeDex;

public partial class InfoPokemonPopup : BasePopupPage
{
    public InfoPokemonPopup(PokemonRow p, List<Ability> a)
    {
        InitializeComponent();
        BindingContext = new InfoPokemonPopupVM(p, a);
    }
}