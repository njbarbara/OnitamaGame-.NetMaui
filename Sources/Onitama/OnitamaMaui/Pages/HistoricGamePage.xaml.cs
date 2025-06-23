using System.Collections.ObjectModel;
using Onitama.Persistance.Stub;
using OnitamaLib.Models;
using OnitamaPersistanceJson;

namespace OnitamaMaui.Pages;

public partial class HistoricGamePage : ContentPage
{
    private readonly JsonLoadManager _loadManager;
    readonly string SaveJsonPath = Path.Combine(FileSystem.AppDataDirectory, "savesJsonOnitama");

    public ObservableCollection<Game> Games { get; private set; } = [];

    public HistoricGamePage()
	{
		InitializeComponent();
        _loadManager = new JsonLoadManager();
        Games = [.. _loadManager.LoadAllGames(SaveJsonPath).Where(x => x.IsOver == true)];
        BindingContext = this;
    }
}