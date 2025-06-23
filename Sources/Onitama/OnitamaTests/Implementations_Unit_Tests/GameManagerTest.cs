using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using OnitamaLib.Implementations;
using OnitamaLib.Models;
using OnitamaLib.PersistanceManagers;
using Xunit;
using Color = OnitamaLib.Models.Color;
using Onitama.Persistance.Stub;

namespace OnitamaTests.Implementations_Unit_Tests
{
    public class GameManagerTests
    {
        private readonly List<OnitamaCard> cards;
        private readonly StubLoadManager loadStub;
        private readonly StubSaveManager saveStub;
        private readonly GameManager gameManager;
        private readonly Game loadedGame;

        public GameManagerTests()
        {
            loadStub = new();
            saveStub = new();

            List<OnitamaCard> allCards = loadStub.InitializeGameCards();
            cards = [.. allCards.Take(5)];
            gameManager = new(GameMod.Classique, loadStub, saveStub);
            loadedGame = loadStub.LoadGame("test", "chemin test");
        }

        public static IEnumerable<object[]> ValidPlayerData =>
        [
            ["Alban", Color.WHITE, "Najib", Color.BLACK],
            ["Esteban", Color.BLACK, "Alban", Color.WHITE]
        ];

        [Theory]
        [MemberData(nameof(ValidPlayerData))]
        public void CreateGame_TwoPlayers_CreatesGameCorrectly(string player1Name, Color color1, string player2Name, Color color2)
        {
            Game game = gameManager.CreateGame(player1Name, color1, player2Name, color2, GameMod.Classique, cards);

            Assert.NotNull(game);
            Assert.Equal(GameMod.Classique, game.Gamemode);
            Assert.Equal(player1Name, game.GetPlayer(0).Name);
            Assert.Equal(color1, game.GetPlayer(0).Color);
            Assert.Equal(player2Name, game.GetPlayer(1).Name);
            Assert.Equal(color2, game.GetPlayer(1).Color);
            Assert.False(game.IsOver);
            Assert.Equal(player1Name, game.CurrentPlayer.Name);
            Assert.NotEmpty(game.Player1);
            Assert.NotEmpty(game.Player2);
        }

        [Fact]
        public void CreateGame_SameColor_ThrowsArgumentException() =>
            Assert.Throws<ArgumentException>(() => gameManager.CreateGame("P1", Color.WHITE, "P2", Color.WHITE, GameMod.Classique, cards));

        [Fact]
        public void CreateGame_OnePlayer_CreatesGameWithStubbedSecondPlayer()
        {
            Game game = gameManager.CreateGame("SoloPlayer", Color.BLACK, GameMod.Classique, cards);

            Assert.NotNull(game);
            Assert.Equal(2, game.Players.Count());
            Assert.Equal("SoloPlayer", game.GetPlayer(0).Name);
            Assert.Equal(Color.BLACK, game.GetPlayer(0).Color);
            Assert.NotNull(game.GetPlayer(1));
            Assert.False(game.IsOver);
        }

        [Fact]
        public void EndGame_ShouldMarkGameAsOver()
        {
            Game game = gameManager.CreateGame("P1", Color.WHITE, "P2", Color.BLACK, GameMod.Classique, cards);
            gameManager.EndGame();
            Assert.True(game.IsOver);
        }

        [Fact]
        public void PlayTurn_WithoutGame_ThrowsInvalidOperationException()
        {
            GameManager manager = new(GameMod.Classique, loadStub, saveStub);
            Assert.Throws<InvalidOperationException>(() => manager.PlayTurn(cards[0], new(0, 0), new(1, 1)));
        }

        [Fact]
        public void LoadGame_ResetsManagersWithoutException()
        {
            Game game = gameManager.CreateGame("P1", Color.WHITE, "P2", Color.BLACK, GameMod.Classique, cards);
            GameManager newManager = new(GameMod.Apocalypse, loadStub, saveStub);
            newManager.LoadGame(game);
            Assert.NotNull(newManager.BoardManager);
            Assert.NotNull(newManager.MoveManager);
            Assert.NotNull(newManager.DeckManager);
        }

        [Fact]
        public void GetCheckTempleVictory_NoVictory_ReturnsNull()
        {
            gameManager.LoadGame(loadedGame);
            Player? winner = gameManager.GetCheckTempleVictory();
            Assert.Null(winner);
        }

        [Fact]
        public void Constructor_WithCustomManagers_SetsPropertiesCorrectly()
        {
            ClassicGameBoardManager boardManager = new();
            ClassicBoardMoveManager moveManager = new();
            DeckManager deckManager = new();
            StubLoadManager loadManager = new();
            StubSaveManager saveManager = new();

            GameManager manager = new(boardManager, moveManager, deckManager, loadManager, saveManager);

            Assert.Equal(boardManager, manager.BoardManager);
            Assert.Equal(moveManager, manager.MoveManager);
            Assert.Equal(deckManager, manager.DeckManager);
            Assert.Equal(loadManager, manager.Loadmanager);
            Assert.Equal(saveManager, manager.SaveManager);
        }

        [Fact]
        public void PlayTurn_GameAlreadyOver_ThrowsInvalidOperationException()
        {
            Game game = gameManager.CreateGame("P1", Color.WHITE, "P2", Color.BLACK, GameMod.Classique, cards);
            gameManager.EndGame();

            Assert.Throws<InvalidOperationException>(() =>
                gameManager.PlayTurn(cards[0], new(0, 0), new(1, 1)));
        }

        [Fact]
        public void PlayTurn_InvalidMove_DoesNotSwitchPlayer()
        {
            Game game = gameManager.CreateGame("Alban", Color.WHITE, "Najib", Color.BLACK, GameMod.Classique, cards);
            gameManager.LoadGame(game);
            Player firstPlayer = game.CurrentPlayer;
            IReadOnlyList<OnitamaCard> playerDeck = [.. game.Player1];
            OnitamaCard cardToPlay = playerDeck[0];
            Position start = game.Board.GetPalace(firstPlayer.Color);
            Position destination = new(-1, -1);
            var moveManager = gameManager.MoveManager;

            bool moveResult = moveManager.TryMove(cardToPlay, game.Board, start, destination);
            gameManager.PlayTurn(cardToPlay, start, destination);

            Assert.False(moveResult);
            Assert.Equal(firstPlayer, game.CurrentPlayer);
        }

        [Fact]
        public void CheckVictoryConditions_SenseiWhiteEliminated_ReturnsBlackPlayer()
        {
            Board board = new();
            Sensei whiteSensei = new(Color.WHITE);
            Sensei blackSensei = new(Color.BLACK);
            Position whitePalace = new(0, 2);
            Position blackPalace = new(4, 2);
            board.PlacePawn(whiteSensei, whitePalace);
            board.PlacePawn(blackSensei, blackPalace);
            Game game = new(new("P1", Color.WHITE), new("P2", Color.BLACK), board, GameMod.Classique, cards);
            gameManager.LoadGame(game);
            board.RemovePawn(whitePalace);

            Player? winner = gameManager.CheckVictoryConditions();

            Assert.NotNull(winner);
            Assert.Equal(Color.BLACK, winner.Color);
        }

        [Fact]
        public void CheckVictoryConditions_SenseiBlackEliminated_ReturnsWhitePlayer()
        {
            Board board = new();
            Sensei whiteSensei = new(Color.WHITE);
            Sensei blackSensei = new(Color.BLACK);
            Position whitePalace = new(0, 2);
            Position blackPalace = new(4, 2);
            board.PlacePawn(whiteSensei, whitePalace);
            board.PlacePawn(blackSensei, blackPalace);
            Game game = new(new("P1", Color.WHITE), new("P2", Color.BLACK), board, GameMod.Classique, cards);
            gameManager.LoadGame(game);
            board.RemovePawn(blackPalace);

            Player? winner = gameManager.CheckVictoryConditions();

            Assert.NotNull(winner);
            Assert.Equal(Color.WHITE, winner.Color);
        }

        [Fact]
        public void GetCheckTempleVictory_WhiteSenseiInWhitePalace_ReturnsNull()
        {
            Game game = gameManager.CreateGame("P1", Color.WHITE, "P2", Color.BLACK, GameMod.Classique, cards);
            Board board = game.Board;
            Sensei sensei = new(Color.WHITE);
            Position whitePalace = new(0, 2);
            if (board.DoIhavePawn(whitePalace)) board.RemovePawn(whitePalace);
            board.PlacePawn(sensei, whitePalace);

            Player? winner = gameManager.GetCheckTempleVictory();

            Assert.Null(winner);
        }

        [Fact]
        public void CheckVictoryConditions_NullGame_ThrowsInvalidOperationException()
        {
            GameManager manager = new(GameMod.Classique, loadStub, saveStub);
            Assert.Throws<InvalidOperationException>(() => manager.CheckVictoryConditions());
        }

        [Fact]
        public void GetCheckTempleVictory_NullGame_ThrowsInvalidOperationException()
        {
            GameManager manager = new(GameMod.Classique, loadStub, saveStub);
            Assert.Throws<InvalidOperationException>(() => manager.GetCheckTempleVictory());
        }

        [Fact]
        public void EndGame_NullGame_ThrowsInvalidOperationException()
        {
            GameManager manager = new(GameMod.Classique, loadStub, saveStub);
            Assert.Throws<InvalidOperationException>(() => manager.EndGame());
        }

        [Fact]
        public void LoadGame_ApocalypseMode_SetsApocalypseMoveManager()
        {
            Board board = new();
            Game game = new(new("P1", Color.WHITE), new("P2", Color.BLACK), board, GameMod.Apocalypse, cards);

            gameManager.LoadGame(game);

            Assert.IsType<ApocalypseBoardMoveManager>(gameManager.MoveManager);
        }

        [Fact]
        public void PlayTurn_WithValidMove_ShouldMovePawnAndSwitchCardAndChangePlayer()
        {
            ClassicGameBoardManager boardManager = new();
            ClassicBoardMoveManager moveManager = new();
            DeckManager deckManager = new();
            StubLoadManager loadManager = new();
            StubSaveManager saveManager = new();

            GameManager gameManager = new(boardManager, moveManager, deckManager, loadManager, saveManager);

            OnitamaCard card = new("Card1", [new(0, 1)]);
            Player player1 = new("P1", Color.WHITE);
            Player player2 = new("P2", Color.BLACK);
            Board board = boardManager.GenerateBoard();

            List<OnitamaCard> cards =
            [
                new("StackCard", []),
                card,
                new("Card2", []),
                new("Card3", []),
                new("Card4", [])
            ];

            Game game = gameManager.CreateGame("P1", Color.WHITE, "P2", Color.BLACK, GameMod.Classique, cards);

            Position start = new(2, 2);
            Position destination = new(2, 3);
            game.Board.PlacePawn(new(Color.WHITE), start);

            gameManager.PlayTurn(card, start, destination);

            Assert.Null(game.Board.GetPawnAt(start));
            Assert.NotNull(game.Board.GetPawnAt(destination));
            Assert.Equal(Color.WHITE, game.Board.GetPawnAt(destination)?.Color);
            Assert.Contains(cards[0], game.Player1);
            Assert.Equal(card, game.CardInStack);
            Assert.Equal(Color.BLACK, game.CurrentPlayer.Color);
        }

        [Fact]
        public void PlayTurn_EndsGameAndUpdatesScore_WhenVictoryConditionIsNotNull()
        {
            ClassicGameBoardManager boardManager = new();
            ClassicBoardMoveManager moveManager = new();
            DeckManager deckManager = new();
            StubSaveManager saveManager = new();
            StubLoadManager loadManager = new();

            GameManager gm = new(boardManager, moveManager, deckManager, loadManager, saveManager);

            List<OnitamaCard> cards =
            [
                new("StackCard", [new(1, 0)]),
                new("Card1", [new(1, 0)]),
                new("Card2", [new(0, 1)]),
                new("Card3", [new(0, 1)]),
                new("Card4", [new(0, 1)]),
            ];

            Game game = gm.CreateGame("J1", Color.WHITE, "J2", Color.BLACK, GameMod.Classique, cards);

            Position blackSenseiPos = game.Board.GetPalace(Color.BLACK);
            game.Board.RemovePawn(blackSenseiPos);

            Pawn whitePawn = new(Color.WHITE);
            Sensei blackSensei = new(Color.BLACK);

            Position source = new(2, 2);
            Position destination = new(3, 2);

            game.Board.PlacePawn(whitePawn, source);
            game.Board.PlacePawn(blackSensei, destination);

            OnitamaCard cardToPlay = cards[1];

            gm.PlayTurn(cardToPlay, source, destination);

            Assert.False(boardManager.ISenseiAlive(Color.BLACK, game.Board));
            Assert.True(game.IsOver);
            Assert.Equal(1, game.ScoreP1);
            Assert.Equal(0, game.ScoreP2);
        }
    }
}