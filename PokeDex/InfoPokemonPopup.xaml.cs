using MauiPopup.Views;
using PokeDex.Models;

namespace PokeDex;

public partial class InfoPokemonPopup : BasePopupPage
{
	public string img_url {  get; set; }

	public string name { get; set; }

    public int id { get; set; }

    public List<Ability> abilities { get; set; }

	public InfoPokemonPopup(PokemonRow p, List<Ability> a)
	{
		this.name = p.name;
		this.img_url = p.img_url;
		this.id = (int) p.id;
		abilities = a;

		InitializeComponent();

        BindingContext = this;
    }
}