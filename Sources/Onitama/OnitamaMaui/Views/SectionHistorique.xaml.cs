using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.Maui.Controls;
using OnitamaLib.Models;

namespace OnitamaMaui.Views;

public partial class SectionHistorique : ContentView, INotifyPropertyChanged
{
    private Game _game = default!; 

    public SectionHistorique()
    {
        InitializeComponent();
        BindingContext = this; 
    }

    public static readonly BindableProperty GameProperty =
        BindableProperty.Create(
            nameof(Game),
            typeof(Game),
            typeof(SectionHistorique),
            default(Game),
            propertyChanged: (bindable, oldValue, newValue) =>
            {
                var view = (SectionHistorique)bindable;
                var game = (Game)newValue;

                view._game = game;
                view.OnPropertyChanged(nameof(WinnerName));
                view.OnPropertyChanged(nameof(LoserName));
                view.OnPropertyChanged(nameof(ScoreP1));
                view.OnPropertyChanged(nameof(ScoreP2));
                view.OnPropertyChanged(nameof(Date));
            });

    public Game Game
    {
        get => _game;
        set => SetValue(GameProperty, value);
    }

    public string WinnerName => Game is null ? "" : (Game.ScoreP1 > Game.ScoreP2 ? Game.P1Name : Game.P2Name);
    public string LoserName => Game is null ? "" : (Game.ScoreP1 > Game.ScoreP2 ? Game.P2Name : Game.P1Name);
    public int ScoreP1 => Game?.ScoreP1 ?? 0;
    public int ScoreP2 => Game?.ScoreP2 ?? 0;
    public DateTime Date => Game?.Date ?? DateTime.MinValue;

    public new event PropertyChangedEventHandler? PropertyChanged;
    protected override void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}


