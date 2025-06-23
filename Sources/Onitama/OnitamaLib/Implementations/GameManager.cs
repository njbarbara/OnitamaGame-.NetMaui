using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Transactions;
using OnitamaLib.Events;
using OnitamaLib.Managers;
using OnitamaLib.Models;
using OnitamaLib.PersistanceManagers;
using System.Collections.ObjectModel;

using Color = OnitamaLib.Models.Color;

namespace OnitamaLib.Implementations
{
    /// <summary>
    /// Gère la logique de jeu principale  incluant les déplacements, les cartes, les joueurs et l’état du jeu . Il sert d'ordonnanceur qui ordonne l'ordre des appels pour jouer un tour créer une parti il fait le lien entre la consoleaApp et le reste de la lib
    /// </summary>
    public class GameManager : IGameManager
    {
        /// <summary>
        /// Boardmanager pour le plateau
        /// </summary>
        public IGameBoardManager BoardManager { get; private set; }
        /// <summary>
        /// MoveManager s'occupe de la logique des déplacements des pions
        /// </summary>
        public IMoveManager MoveManager { get; private set; }
        /// <summary>
        /// DeckManager s'occupe de la logique des cartes 
        /// </summary>
        public IDeckManager DeckManager { get; private set; }
        /// <summary>
        /// LoadManager s'occupe du chargement des parties
        /// </summary>
        public IloadManager Loadmanager { get; private set; }
        /// <summary>
        /// Savemanager s'occupe du systeme de sauvegarde
        /// </summary>
        public ISaveManager SaveManager { get; private set; }



        private Game? _game;

        public Game? Game => _game;


        /// <summary>
        /// Événement déclenché lorsqu’un mouvement invalide est fait
        /// </summary>
        public event EventHandler<InvalidMoveEventArgs>? InvalidMove;

        /// <summary>
        /// Déclenche l'événement <see cref="InvalidMove"/>.
        /// </summary>
        /// <param name="args">arguments associés au mouvement invalide</param>
        protected virtual void OnInvalidMove(InvalidMoveEventArgs args) => InvalidMove?.Invoke(this, args);



        /// <summary>
        /// Constructeur principal du GameManager et instancie tout les manager 
        /// </summary>
        /// <param name="gameMod">le mode de jeu voulu pour appelé le bon MoveManager</param>
        /// <param name="loadmanager">le loadManager</param>
        /// <param name="saveManager">le saveMaanger</param>
        public GameManager(GameMod gameMod, IloadManager loadmanager, ISaveManager saveManager)
        {
            BoardManager = new ClassicGameBoardManager();
            MoveManager = (gameMod == GameMod.Classique) ? new ClassicBoardMoveManager() : new ApocalypseBoardMoveManager(5, 5);
            DeckManager = new DeckManager();
            Loadmanager = loadmanager;
            SaveManager = saveManager;

        }


        public GameManager(IGameBoardManager boardManager, IMoveManager moveManager, IDeckManager deckManager, IloadManager loadmanager, ISaveManager saveManager)
        {
            BoardManager = boardManager;
            MoveManager = moveManager;
            DeckManager = deckManager;
            Loadmanager = loadmanager;
            SaveManager = saveManager;

        }

        /// <summary>
        /// Crée une nouvelle partie avec deux joueurs (pas de bot)
        /// </summary>
        /// <param name="player1Name">le nom du joueur 1</param>
        /// <param name="color1">la couleur du joueur 1</param>
        /// <param name="player2Name">le nom du joueur 2</param>
        /// <param name="color2">la couleur du joueur 1</param>
        /// <param name="gameMod">le mode de jeu voulu </param>
        /// <param name="gameCards">la liste des cartes sélèctionné</param>
        public Game CreateGame(string player1Name, Color color1, string player2Name, Color color2, GameMod gameMod, List<OnitamaCard> gameCards)
        {
            if (color1 == color2)
            {
                throw new ArgumentException("Les deux joueurs ne peuvent pas avoir la même couleur.");
            }
            Player player1 = new(player1Name, color1);
            Player player2 = new(player2Name, color2);
            Board board = BoardManager.GenerateBoard();

            _game = new Game(player1, player2, board, gameMod, gameCards);

            BoardManager = new ClassicGameBoardManager();
            MoveManager = (_game.Gamemode == GameMod.Classique) ? new ClassicBoardMoveManager() : new ApocalypseBoardMoveManager(5, 5);
            return _game;
        }
        /// <summary>
        /// Crée une nouvelle partie avec 1 joueur et 1 bot 
        /// </summary>
        /// <param name="player1Name">le nom du joueur 1</param>
        /// <param name="color1">la couleur du joueur 1</param>
        /// <param name="gameMod">le mode de jeu voulu </param>
        /// <param name="gameCards">la liste des cartes sélèctionné</param>
        public Game CreateGame(string player1Name, Color color1, GameMod gameMod, List<OnitamaCard> gameCards)
        {
            Player player1 = new(player1Name, color1);
            Board board = BoardManager.GenerateBoard();

            _game = new Game(player1, board, gameMod, gameCards);

            BoardManager = new ClassicGameBoardManager();
            MoveManager = (_game.Gamemode == GameMod.Classique) ? new ClassicBoardMoveManager() : new ApocalypseBoardMoveManager(5, 5);

            return _game;

        }


        /// <summary>
        /// Charge une partie existante 
        /// </summary>
        /// <param name="game">la partie à charger</param>
        public void LoadGame(Game game)
        {
            if (game == null)
                throw new ArgumentNullException(nameof(game), "La partie à charger ne peut pas être null.");

            _game = game;
            _game.SwitchPlayer(); 
            BoardManager = new ClassicGameBoardManager();
            MoveManager = (_game.Gamemode == GameMod.Classique) ? new ClassicBoardMoveManager() : new ApocalypseBoardMoveManager(5, 5);

            DeckManager = new DeckManager();
        }

        /// <summary>
        /// Joue un tour en tentant un déplacement et applique les conséquences si valide ou non valide
        /// </summary>
        /// <param name="cardPlayed">la carte jouée</param>
        /// <param name="pawnPosition">la position actuelle du pion.</param>
        /// <param name="pawnDestination">la destination choisi.</param>
        /// <exception cref="InvalidOperationException">si le jeu n'est pas chargé ou est terminé</exception>
        public void PlayTurn(OnitamaCard cardPlayed, Position pawnPosition, Position pawnDestination)
        {
            if (_game == null) throw new InvalidOperationException("Partie non chargé.");
            if (_game.IsOver) throw new InvalidOperationException("La partie est terminé");
            if (!_game.IsCardInCurrentPlayerHand(cardPlayed)) return;

            bool resMove;

            resMove = MoveManager.TryMove(cardPlayed, _game.Board, pawnPosition, pawnDestination);
            OnInvalidMove(new InvalidMoveEventArgs(resMove));

            if (resMove)
            {
                _game.UpdateScore(_game.CurrentPlayer);
                DeckManager.SwitchCard(cardPlayed, _game);
                _game.SwitchPlayer();
                var winner = CheckVictoryConditions();
                if (winner != null)
                {
                    ScoreManager.CalcScore(_game);
                    _game.ChangeStateOfGame();
                }

            }
        }

        /// <summary>
        /// Joue le tour du bot
        /// </summary>
        public void PlayTurn()
        {
            if (Game == null)
                throw new InvalidOperationException("Partie non chargée.");

            if (Game.CurrentPlayer.IsBot)
            {
                OnitamaBot onitamaBot = (OnitamaBot)Game.CurrentPlayer;
                OnitamaCard? chosenCard = null;
                Position? chosenPositionToMove = null;
                Position chosenPawnPosition = onitamaBot.ChoosePawn(Game.Board, BoardManager);


                while (chosenPositionToMove is null)
                {
                    foreach (OnitamaCard card in Game.GetDeckCurrentPlayer())
                    {
                        chosenPawnPosition = onitamaBot.ChoosePawn(Game.Board, BoardManager);
                        chosenPositionToMove = onitamaBot.ChooseMove(chosenPawnPosition, Game.Board, BoardManager, MoveManager, card);
                        chosenCard = card;

                    }
                }

                if (chosenCard is null) throw new InvalidOperationException("Le bot ne peut effectuer aucun mouvement.");
                PlayTurn(chosenCard, chosenPawnPosition, chosenPositionToMove);
            }
        }


        /// <summary>
        /// Vérifie si une condition de victoire est remplie (Sensei éliminé ou palais atteint).
        /// </summary>
        /// <returns>Le joueur gagnant ou null si la partie continue.</returns>
        public Player? CheckVictoryConditions()
        {
            if (_game == null)
                throw new InvalidOperationException("Le jeu n'a pas été créé ou chargé.");
            Board board = _game.Board;
            bool whiteSenseiAlive = BoardManager.ISenseiAlive(Color.WHITE, board);
            bool blackSenseiAlive = BoardManager.ISenseiAlive(Color.BLACK, board);

            if (!whiteSenseiAlive || !blackSenseiAlive)
                return _game.Players.First(p => p.Color == (!whiteSenseiAlive ? Color.BLACK : Color.WHITE));

            return GetCheckTempleVictory();
        }

        /// <summary>
        /// Vérifie si un Sensei adverse est dans le palais ennemi (victoire par position)
        /// </summary>
        /// <returns>Le joueur gagnant ou null si la condition n’est pas remplie.</returns>
        public Player? GetCheckTempleVictory()
        {
            if (_game == null)
                throw new InvalidOperationException("Le jeu n'a pas été créé ou chargé.");

            Board board = _game.Board;

            Position whitePalace = board.GetPalace(Color.WHITE);
            Position blackPalace = board.GetPalace(Color.BLACK);


            Pawn? whiteSensei = board.GetPawnAt(blackPalace.PositionX, blackPalace.PositionY);
            if (whiteSensei?.IsSensei == true && whiteSensei.Color == Color.WHITE)
                return _game.GetPlayerByColor(Color.WHITE);


            Pawn? blackSensei = board.GetPawnAt(whitePalace.PositionX, whitePalace.PositionY);
            if (blackSensei?.IsSensei == true && blackSensei.Color == Color.BLACK)
                return _game.GetPlayerByColor(Color.BLACK);

            return null;
        }

        /// <summary>
        /// Termine  la partie en changeant son état
        /// </summary>
        public void EndGame()
        {
            if (_game == null)
                throw new InvalidOperationException("Le jeu n'a pas été créé ou chargé.");
            _game.ChangeStateOfGame();
        }

    }
}
