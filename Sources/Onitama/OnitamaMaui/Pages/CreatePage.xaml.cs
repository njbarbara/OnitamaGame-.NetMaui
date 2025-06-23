using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using OnitamaLib;
using OnitamaLib.Implementations;
using OnitamaLib.Managers;
using OnitamaLib.Models;
using OnitamaLib.PersistanceManagers;
using OnitamaPersistanceJson;
using Color = OnitamaLib.Models.Color;
using Microsoft.Maui.Storage;

namespace OnitamaMaui.Pages;


public partial class CreatePage : ContentPage, INotifyPropertyChanged
{
    public string Player1Name { get; set; } = string.Empty;
    public string Player2Name { get; set; } = string.Empty;
    public bool GameMode { get; set; } = false;
    private bool _isSinglePlayer = true;
    public bool IsSinglePlayer
    {
        get => _isSinglePlayer;
        set
        {
            if (_isSinglePlayer != value)
            {
                _isSinglePlayer = value;
                OnPropertyChanged(nameof(IsSinglePlayer));
                OnPropertyChanged(nameof(IsMultiplayer));
            }
        }
    }
    public bool IsMultiplayer => !IsSinglePlayer;

    public CreatePage()
    {
        InitializeComponent();
        BindingContext = this;
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(Player1Name))
        {
            await DisplayAlert("Alerte", "Nom du joueur 1 vide", "OK");
            return;
        }

        if (IsMultiplayer && string.IsNullOrWhiteSpace(Player2Name))
        {
            await DisplayAlert("Alerte", "Nom du joueur 2 vide", "OK");
            return;
        }


        string route = $"game?player1name={Player1Name}&gamemode={GameMode}&issingleplayer={IsSinglePlayer}";

        if (IsMultiplayer)
        {
            route += $"&player2name={Player2Name}";
        }

        await Shell.Current.GoToAsync(route);

    }



    public new event PropertyChangedEventHandler? PropertyChanged;

    protected override void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }



}