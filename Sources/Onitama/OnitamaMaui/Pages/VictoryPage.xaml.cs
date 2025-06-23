using OnitamaLib.Models;

namespace OnitamaMaui.Pages;

public partial class VictoryPage : ContentPage
{
	public Player Winner { get;}
    public Player Loser { get; }

	public Game Game { get; }

    public VictoryPage(Game game)
	{
		InitializeComponent();
		Winner = game.GetWinner();
		Loser = game.GetLoser();
		Game = game;
		BindingContext = this;
	}
    async void GoBackToMainPage(object sender, EventArgs args) =>  await Shell.Current.GoToAsync("home");
    
}