using Onitama.Persistance.Stub;
using OnitamaLib;
using OnitamaLib.Events;
using OnitamaLib.Implementations;
using OnitamaLib.Managers;
using OnitamaLib.Models;
using OnitamaLib.PersistanceManagers;
using OnitamaMaui.Layouts;
using System.Threading.Tasks;
using System.ComponentModel;
using Microsoft.Maui.Controls;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Color = OnitamaLib.Models.Color;

namespace OnitamaMaui.Pages;
using OnitamaMaui.Functionalities;

using System.IO.Enumeration;
using OnitamaLib.Models;
using OnitamaPersistanceJson;

[QueryProperty(nameof(NamePlayer1), "player1name")]
[QueryProperty(nameof(NamePlayer2), "player2name")]
[QueryProperty(nameof(GameModeBool), "gamemode")]
[QueryProperty(nameof(IsSinglePlayerBool), "issingleplayer")]
[QueryProperty(nameof(FileName), "filename")]
public partial class GamePage : ContentPage, INotifyPropertyChanged
{
    public Board Board { get; set; }
    public Game Game { get; set; }

    private string _fileName = string.Empty;

    //Propriété à binder
    public CardMaui CardInStack { get; set; }

    public string FileName
    {
        get => _fileName;
        set
        {
            _fileName = value;
            OnPropertyChanged();
        }
    }
    private string _player1name = string.Empty;

    public string NamePlayer1
    {
        get => _player1name;
        set
        {
            _player1name = value;
        }
    }

    private string _player2name = string.Empty;
    public string NamePlayer2
    {
        get => _player2name;
        set
        {
            _player2name = value;
        }
    }

    private bool _gameModeBool;
    public bool GameModeBool
    {
        get => _gameModeBool;
        set
        {
            _gameModeBool = value;
            _gameSettings.CurrentGameMod = value ? GameMod.Apocalypse : GameMod.Classique;
            OnPropertyChanged();
        }
    }

    private bool _isSinglePlayerBool;
    public bool IsSinglePlayerBool
    {
        get => _isSinglePlayerBool;
        set
        {
            _isSinglePlayerBool = value;
            OnPropertyChanged();
        }
    }

    public BoardMaui2d Matrix { get; set; }
    public DeckMaui2d DeckPlayer1 { get; set; }
    public DeckMaui2d DeckPlayer2 { get; set; }
    public CurrentPlayerMaui CurrentPlayerMaui { get; set; }
   
    
    private CardMaui? cardSelected;


    private bool _pawnSelected;

    private Position pawnPosition;

    private Position pawnDestination;
    private bool _pageSettingsOpen;

    //TODO : ajouter les sons pour les events 
    //Game.CardInStack += 
    private readonly GameSettings _gameSettings;
    private readonly ISaveManager _saveManager;
    private readonly IloadManager _loadManager;
    private readonly GameManager gm;

    private readonly ISoundManager _soundManager;


    public Game LoadedGame { get; private set; }

    public GamePage(GameSettings settings, IGameManager gameManager, IloadManager loadManager, ISaveManager saveManager)
    {
        var services = Application.Current?.Handler.MauiContext?.Services;
        _soundManager = services?.GetService<ISoundManager>()!;

        InitializeComponent();

        _gameSettings = settings ?? throw new ArgumentNullException(nameof(settings));
        gm = (GameManager)gameManager ?? throw new ArgumentNullException(nameof(gameManager));
        _loadManager = loadManager ?? throw new ArgumentNullException(nameof(loadManager));
        _saveManager = saveManager ?? throw new ArgumentNullException(nameof(saveManager));


    }

    private async Task CreateGameAsync()
    {
        try
        {
            Color player1Color = Color.WHITE;
            Color player2Color = Color.BLACK;

            List<OnitamaCard> gameCards = _loadManager.InitializeGameCards();

            Game game;

            if (IsSinglePlayerBool)
            {
                if (string.IsNullOrWhiteSpace(NamePlayer1))
                {
                    await DisplayAlert("Erreur", "Le nom du joueur 1 est requis.", "OK");
                    return;
                }

                game = gm.CreateGame(player1Name: NamePlayer1, color1: player1Color, gameMod: _gameSettings.CurrentGameMod, gameCards: gameCards);
            }
            else
            {
                if (string.IsNullOrWhiteSpace(NamePlayer1) || string.IsNullOrWhiteSpace(NamePlayer2))
                {
                    await DisplayAlert("Erreur", "Les noms des deux joueurs sont requis pour le mode multijoueur.", "OK");
                    return;
                }

                game = gm.CreateGame( player1Name: NamePlayer1, color1: player1Color,player2Name: NamePlayer2, color2: player2Color,gameMod: _gameSettings.CurrentGameMod,gameCards: gameCards);
            }


            Game = game;
            LoadedGame = game;

            if (string.IsNullOrWhiteSpace(FileName))
            {
                FileName = $"{DateTime.Now:yyyyMMdd_HHmmss}.json";
                Game.FileName = FileName;
            }

            string modeTexte = _gameSettings.CurrentGameMod == GameMod.Classique ? "Classique" : "Apocalypse";
            string joueur2Affichage = IsSinglePlayerBool ? "(Bot) BOB" : NamePlayer2;

            await DisplayAlert("Partie créée !",$"Mode : {modeTexte}\n" + $"Joueur 1 : {NamePlayer1} ({player1Color})\n" + $"Joueur 2 : {joueur2Affichage} ({player2Color})\n" +$"Nombre de cartes : {gameCards.Count}", "OK");
        }
        catch (ArgumentException ex)
        {
            await DisplayAlert("Erreur", ex.Message, "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erreur", $"Échec de la création de la partie : {ex.Message}", "OK");
        }
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        if(_pageSettingsOpen)
        {
            _pageSettingsOpen = false;
            return;
        }
        string saveFolderPath = Path.Combine(FileSystem.AppDataDirectory, "savesJsonOnitama");

        if (!string.IsNullOrWhiteSpace(FileName))
        {
            try
            {
                Game = _loadManager.LoadGame(FileName, saveFolderPath);
                LoadedGame = Game;
                gm.LoadGame(Game);
                await DisplayAlert("Partie chargée", $"Nom fichier : {FileName}", "OK");

                
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erreur", $"Échec du chargement de la partie : {ex.Message}", "OK");
            }
        }
        else
        {
            await CreateGameAsync();
        }

        Game = gm.Game;
        Board = Game.Board;
        Matrix = new BoardMaui2d(Board, gm.MoveManager.GetForbiddenPositions());


        //Propriété à binder 
        DeckPlayer1 = new DeckMaui2d(Game.Player1);
        DeckPlayer2 = new DeckMaui2d(Game.Player2);


        CardInStack = new CardMaui(Game.CardInStack, 0);

        CurrentPlayerMaui = new CurrentPlayerMaui(Game, gm.BoardManager);

        _pawnSelected = false;

        pawnPosition = new Position();
        pawnDestination = new Position();


        NamePlayer1 = Game.GetPlayer(0).Name;
        NamePlayer2 = Game.GetPlayer(1).Name;


        CurrentPlayerMaui.ChangeNbPawnEaten(null, new BoardChangedEventArgs(Board));

        //Abonnemment des events
        gm.InvalidMove += DisplayInvalidMoveResults;
        Game.GameOver += GameOverEvents;
        Game.TurnChanged += TurnChange;


        BindingContext = this;

    }



    public async void TurnChange(object? sender, TurnChangedEventArgs e) {
        await _soundManager.PlaySound("transition_sound.mp3");
        if (!Game.IsOver)
        {
            if (cardSelected != null)
            {
                cardSelected.IsSelected = false;
                cardSelected = null;
                CardInStack.Card = Game.CardInStack;
                DeckPlayer1.UpdatePlayerDecks(Game.Player1);
                DeckPlayer2.UpdatePlayerDecks(Game.Player2);
            }
            if (Game.CurrentPlayer.IsBot) gm.PlayTurn();
        }
    }

    private async void DisplayInvalidMoveResults(object? sender, InvalidMoveEventArgs e)
    {
        if (e.ResMove)await DisplayAlert("Bien joué !", "Le mouvement a été fait avec succés ! ", "Ok");
        else await DisplayAlert("Attention !", "Le mouvement est invalide, retenté votre coup ! ", "Ok");
    }

    private async void GameOverEvents(object? sender, GameOverEventArgs e)
    {
        JsonSaveManager saveManager = new JsonSaveManager();
        string saveFolderPath = Path.Combine(FileSystem.AppDataDirectory, "savesJsonOnitama");


        if (string.IsNullOrWhiteSpace(Game.FileName))
        {
            Game.FileName = $"{DateTime.Now:yyyyMMdd_HHmmss}.json";
        }


        string fullFilePath = Path.Combine(saveFolderPath, Game.FileName);


        bool success = saveManager.SaveGame(Game, fullFilePath, Game.FileName);

        await Navigation.PushAsync(new VictoryPage(Game));

    }

    private async void SettingsButton(object sender, EventArgs args)
    {
        _pageSettingsOpen = true;
        var settingsPage = new SettingsPage();

        await Shell.Current.Navigation.PushModalAsync(settingsPage);
    }
    
    private async void OnPawnButton(object sender, EventArgs args)
    {
        await _soundManager.PlaySound("pawnMove.mp3");

        var grid =(ImageButton)sender;
        PawnMaui? pawn = (PawnMaui)grid.BindingContext;
        if (cardSelected == null)
        {
            await DisplayAlert("Attention ! ", "Sélectionné une carte", "Ok");
            return;
        }

        if (!_pawnSelected)
        {
            if (pawn.Pawn == null)
            {
                await DisplayAlert("Le pion est null ! ", "x = " + pawn.Position.PositionX + "\ny = " + pawn.Position.PositionY, "Ok");
                return;
            }
            else
            {
                if (gm.BoardManager.GetPawnsPositionsColor(Game.CurrentPlayer.Color, Board).Positions.Contains(pawn.Position))
                {
                    //await DisplayAlert("Coordonnées ! ", "x = " + pawn.Position.PositionX + "\ny = " + pawn.Position.PositionY, "Ok");
                    _pawnSelected = true;
                    pawnPosition = pawn.Position;

                    Matrix.PlaceAvailableMoves(gm.MoveManager.GetAvailableMove(cardSelected.Card, Board, pawnPosition));

                    return;
                }
                else
                {
                    await DisplayAlert("Erreur", "Vous ne possédez pas ce pion", "Ok");
                    return;
                }

            }
        }

        //await DisplayAlert("Le pion va etre déplacé à ! ", "x = " + pawn.Position.PositionX + "\ny = " + pawn.Position.PositionY, "Ok");
        pawnDestination = pawn.Position;
        Matrix.RemoveAvailableMoves();

        gm.PlayTurn(cardSelected.Card, pawnPosition, pawnDestination);

        _pawnSelected = false;

    }

    private async void OnCardButton(object sender, EventArgs args)
    {
        var grid = (ImageButton)sender;
        CardMaui ClickedCard = (CardMaui)grid.BindingContext;
        await _soundManager.PlaySound("pawnMove.mp3");


        if (Game.GetDeckCurrentPlayer().Contains(ClickedCard.Card))
        {
            if (cardSelected != null)
                cardSelected.IsSelected = false;
            if (_pawnSelected)
                Matrix.RemoveAvailableMoves();

            cardSelected = ClickedCard;
            cardSelected.IsSelected = true;

            //await DisplayAlert("Indice de la carte", "x = " + cardSelected.Indice + "\n", "Ok");
        }
        else await DisplayAlert("Erreur", "Vous ne possédez pas cette carte", "Ok");
    }

    private async void BackButton(object sender, EventArgs args)
    {
        await _soundManager.PlaySound("pawnMove.mp3");
        bool result = await DisplayAlert("Warning", " Save ? ", "Yes", "No");

        if (result)
        {
            try
            {
                JsonSaveManager saveManager = new JsonSaveManager();
                string saveFolderPath = Path.Combine(FileSystem.AppDataDirectory, "savesJsonOnitama");


                if (string.IsNullOrWhiteSpace(Game.FileName))
                {
                    Game.FileName = $"{DateTime.Now:yyyyMMdd_HHmmss}.json";
                }


                string fullFilePath = Path.Combine(saveFolderPath, Game.FileName);


                bool success = saveManager.SaveGame(Game, fullFilePath, Game.FileName);

                if (success)
                {
                    await DisplayAlert("Sauvegarde", "Partie sauvegardée avec succés.", "OK");
                }
                else
                {
                    await DisplayAlert("Erreur", "Echec de la sauvegarde de la partie.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erreur", $"Exception lors de la sauvegarde : {ex.Message}", "OK");
            }

            await Shell.Current.GoToAsync("home");
        }
    }
}

