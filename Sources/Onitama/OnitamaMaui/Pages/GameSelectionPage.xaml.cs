using OnitamaLib.Models;
using Onitama.Persistance.Stub;
using OnitamaPersistanceJson;
using Microsoft.Maui.Storage;
namespace OnitamaMaui.Pages;
using OnitamaMaui.Functionalities;

public partial class GameSelectionPage : ContentPage
{
    private readonly ISoundManager _soundManager;

    private readonly JsonLoadManager _loadManager;
    readonly string SaveJsonPath = Path.Combine(FileSystem.AppDataDirectory, "savesJsonOnitama");

    readonly string fileName = string.Empty;


    public List<Game> LoadedGames { get; private set; }

    public GameSelectionPage()
    {
        var services = Application.Current?.Handler.MauiContext?.Services;
        _soundManager = services?.GetService<ISoundManager>()!;
        InitializeComponent();
        _loadManager = new JsonLoadManager();

        LoadedGames = _loadManager.LoadAllGames(SaveJsonPath);

        LoadedGames = [.. LoadedGames.Where(x => x.IsOver == false)];
        BindingContext = this;
    }


    private async void OnCreateGameButtonClicked(object sender, EventArgs e)
    {
        await _soundManager.PlaySound("pawnMove.mp3");
        await Shell.Current.GoToAsync("creation");
    }


    private async void OnPlayGameButtonClicked(object sender, EventArgs e)
    {
        await _soundManager.PlaySound("pawnMove.mp3");
        if (GamesListView.SelectedItem is not Game selectedGame)
        {
            await DisplayAlert("Erreur", "Veuillez sélectionner une partie.", "OK");
            return;
        }

        if (string.IsNullOrEmpty(selectedGame.FileName))
        {
            await DisplayAlert("Erreur", "Impossible de retrouver le fichier associé à cette partie.", "OK");
            return;
        }

        string route = $"game?filename={Uri.EscapeDataString(selectedGame.FileName)}";
        await Shell.Current.GoToAsync(route);
    }


    private async void OnGameTapped(object sender, EventArgs e)
    {
        await _soundManager.PlaySound("pawnMove.mp3");

        if (sender is Grid grid && grid.BindingContext is Game game)
        {
            GamesListView.SelectedItem = game;
            await DisplayAlert("Game Selected", $"Selected game: {game.Players.First().Name} vs {game.Players.Last().Name}", "OK");
        }
    }
}