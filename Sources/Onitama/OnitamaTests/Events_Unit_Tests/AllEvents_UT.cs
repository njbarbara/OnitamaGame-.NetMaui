using Xunit;
using OnitamaLib.Events;
using OnitamaLib.Models;
using Onitama.Persistance.Stub;
using System.Collections.Generic;

namespace OnitamaTests.Events_Unit_Tests
{
    public class AllEvents_UT
    {
        private readonly List<OnitamaCard> cards;
        private readonly StubLoadManager loadStub;
        private readonly Game loadedGame;
        public AllEvents_UT() {
            loadStub = new StubLoadManager();
            loadedGame = loadStub.LoadGame("test", "chemin test");
            List<OnitamaCard> allCards = loadStub.InitializeGameCards();
            cards = [.. allCards.Take(5)];

        }
        [Fact]
        public void BoardChangedEventArgs_ShouldContainCorrectBoard()
        {
            Board board = new();
            BoardChangedEventArgs args = new(board);

            Assert.Equal(board, args.Board);
        }

        [Fact]
        public void CardInStackChangedEventArgs_ShouldContainCorrectCard()
        {
            CardInStackChangedEventArgs args = new(cards.First());

            Assert.Equal(cards.First(), args.CardInStack);
        }

        [Fact]
        public void GameOverEventArgs_ShouldContainCorrectGame()
        {
            StubLoadManager stub = new();
            Game game = stub.LoadGame("test","chemin test");
            GameOverEventArgs args = new(game);

            Assert.Equal(game, args.Game);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void InvalidMoveEventArgs_ShouldContainCorrectResult(bool expected)
        {
            InvalidMoveEventArgs args = new(expected);

            Assert.Equal(expected, args.ResMove);
        }

        [Fact]
        public void TurnChangedEventArgs_ShouldContainCorrectValues()
        {
            StubLoadManager stub = new();
            Game game = stub.LoadGame("test", "chemin test");

            int score = game.ScoreP1;      
            List<OnitamaCard> cards = [];

            TurnChangedEventArgs args = new(game.CurrentPlayer, score, cards);

            Assert.Equal(game.CurrentPlayer, args.Player);
            Assert.Equal(score, args.Score);
            Assert.Equal(cards, args.PlayerCard);
        }

        [Fact]
        public void StubSaveManager_SaveGame_ShouldReturnTrue()
        {
            
            var saveManager = new StubSaveManager();
            var dummyGame = new Game(); 
            string path = "fakePath";
            string filename = "save.json";

            
            var result = saveManager.SaveGame(dummyGame, path, filename);

            
            Assert.True(result);
        }

    }
}
