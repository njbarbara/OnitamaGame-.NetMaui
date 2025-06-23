using OnitamaLib.Implementations;
using OnitamaLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace OnitamaTests.Models_Unit_Tests
{
    public class GameTest
    {
        private readonly string _p1Name = "Player1";// a voir 
        private readonly Color _color1 = Color.WHITE;
        private readonly string _p2Name = "Player2";
        private readonly Color _color2 = Color.BLACK;
        private readonly List<OnitamaCard> _cards =
        [
            new OnitamaCard("carte1", [new Position(1, 0), new Position(-1, 0)]),
            new OnitamaCard("carte2", []),
            new OnitamaCard("carte3", []),
            new OnitamaCard("carte4", []),
            new OnitamaCard("carte5", [])
        ];


        [Fact]
        public void Constructor_TwoPlayerGame_InitializesCorrectly()
        {
            Player player1 = new(_p1Name, _color1);
            Player player2 = new(_p2Name, _color2);
            Board board = new();

            Game game = new(player1, player2, board, GameMod.Classique, _cards);

            Assert.Equal(player1, game.CurrentPlayer);
            Assert.Equal(board, game.Board);
            Assert.Equal(GameMod.Classique, game.Gamemode);
            Assert.Equal(_cards[0], game.CardInStack);
            Assert.Equal(2, game.Player1.Count());
            Assert.Equal(2, game.Player2.Count());
            Assert.False(game.IsOver);
            Assert.Equal(0, game.ScoreP1);
            Assert.Equal(0, game.ScoreP2);
        }

        [Fact]
        public void Constructor_TwoPlayers_InitializesCorrectly()
        {
            Player player1 = new(_p1Name, _color1);
            Player player2 = new(_p2Name, _color2);
            Board board = new();

            Game game = new(player1, player2, board, GameMod.Classique, _cards);

            Assert.NotNull(game.Board);
            Assert.IsType<Board>(game.Board);
            Assert.InRange(game.Date, DateTime.Now.AddSeconds(-1), DateTime.Now.AddSeconds(1));
            Assert.Equal(GameMod.Classique, game.Gamemode);
            Assert.Equal(0, game.ScoreP1);
            Assert.Equal(0, game.ScoreP2);
            Assert.Equal("carte1", game.CardInStack.Name); 
            Assert.Equal(2, game.CardInStack.Movement.Positions.Count());
            Assert.False(game.IsOver);
            Assert.Equal(_p1Name, game.CurrentPlayer.Name);
            Assert.Equal(2, game.Players.Count());
            Assert.Equal(_p1Name, game.GetPlayer(0).Name);
            Assert.Equal(_color1, game.GetPlayer(0).Color);
            Assert.False(game.GetPlayer(0).IsBot);
            Assert.Equal(_p2Name, game.GetPlayer(1).Name);
            Assert.Equal(_color2, game.GetPlayer(1).Color);
            Assert.False(game.GetPlayer(1).IsBot);
            Assert.Equal(2, game.Player1.Count());
            Assert.Equal(2, game.Player2.Count());
            Assert.Contains(game.Player1,c => c.Name == "carte2");
            Assert.Contains(game.Player2, c => c.Name == "carte4");
            Assert.NotNull(game.Board);
            Assert.Equal(5, game.Board.Width);
            Assert.Equal(5, game.Board.Height);
        }

        [Fact]
        public void Constructor_TwoPlayers_ApocalypseMode_InitializesCorrectly()
        {
            Player player1 = new(_p1Name, _color1);
            Player player2 = new(_p2Name, _color2);

            Board board = new();

            Game game = new(player1, player2, board, GameMod.Apocalypse, _cards);

            Assert.NotNull(game.Board);
            Assert.IsType<Board>(game.Board);
            Assert.InRange(game.Date, DateTime.Now.AddSeconds(-1), DateTime.Now.AddSeconds(1));
            Assert.Equal(GameMod.Apocalypse, game.Gamemode);
            Assert.Equal(0, game.ScoreP1);
            Assert.Equal(0, game.ScoreP2);
            Assert.Equal("carte1", game.CardInStack.Name);
            Assert.Equal(2, game.CardInStack.Movement.Positions.Count());
            Assert.False(game.IsOver);
            Assert.Equal(_p1Name, game.CurrentPlayer.Name);
            Assert.Equal(2, game.Players.Count());
            Assert.Equal(_p1Name, game.GetPlayer(0).Name);
            Assert.Equal(_color1, game.GetPlayer(0).Color);
            Assert.False(game.GetPlayer(0).IsBot);
            Assert.Equal(_p2Name, game.GetPlayer(1).Name);
            Assert.Equal(_color2, game.GetPlayer(1).Color);
            Assert.False(game.GetPlayer(1).IsBot);
            Assert.Equal(2, game.Player1.Count());
            Assert.Equal(2, game.Player2.Count());
            Assert.Contains(game.Player1, c => c.Name == "carte2");
            Assert.Contains(game.Player2, c => c.Name == "carte4");
            Assert.NotNull(game.Board);
            Assert.Equal(5, game.Board.Width);
            Assert.Equal(5, game.Board.Height);
        }

        [Fact]
        public void Constructor_OnePlayer_ApocalypseMode_InitializesCorrectly()
        {
            Player player1 = new(_p1Name, _color1);
        
            Board board = new();

            Game game = new(player1,  board, GameMod.Apocalypse, _cards);

            Assert.NotNull(game.Board);
            Assert.IsType<Board>(game.Board);
            Assert.InRange(game.Date, DateTime.Now.AddSeconds(-1), DateTime.Now.AddSeconds(1));
            Assert.Equal(GameMod.Apocalypse, game.Gamemode);
            Assert.Equal(0, game.ScoreP1);
            Assert.Equal(0, game.ScoreP2);
            Assert.Equal("carte1", game.CardInStack.Name);
            Assert.Equal(2, game.CardInStack.Movement.Positions.Count());
            Assert.False(game.IsOver);
            Assert.Equal(_p1Name, game.CurrentPlayer.Name);
            Assert.Equal(2, game.Players.Count());
            Assert.Equal(_p1Name, game.GetPlayer(0).Name);
            Assert.Equal(_color1, game.GetPlayer(0).Color);
            Assert.False(game.GetPlayer(0).IsBot);
            Assert.NotNull(game.GetPlayer(1));
            Assert.True(game.GetPlayer(1).IsBot);
            Assert.Equal(2, game.Player1.Count());
            Assert.Equal(2, game.Player2.Count());
            Assert.Contains(game.Player1, c => c.Name == "carte2");
            Assert.Contains(game.Player2, c => c.Name == "carte4");
            Assert.NotNull(game.Board);
            Assert.Equal(5, game.Board.Width);
            Assert.Equal(5, game.Board.Height);
        }


        [Fact]
        public void GetBoard_ReturnsCorrectBoard()
        {
            Player player1 = new(_p1Name, _color1);
            Player player2 = new(_p2Name, _color2);

            Board Creationboard = new();

            Game game = new(player1, player2, Creationboard, GameMod.Apocalypse, _cards);
            Board board = game.Board;

            Assert.NotNull(board);
            Assert.Equal(5, board.Width);
            Assert.Equal(5, board.Height);
        }

        [Fact]
        public void UpdateScore_IncrementsCorrectPlayer()
        {
            Player player1 = new(_p1Name, _color1);
            Player player2 = new(_p2Name, _color2);
            Player player3 = new(_p2Name, _color2);
            Board board = new();
            Game game = new(player1, player2, board, GameMod.Classique, _cards);

            game.UpdateScore(player1);
            Assert.Equal(1, game.ScoreP1);
            Assert.Equal(0, game.ScoreP2);

            game.UpdateScore(player2);
            Assert.Equal(1, game.ScoreP1);
            Assert.Equal(1, game.ScoreP2);

            var ex = Assert.Throws<ArgumentException>(() => game.UpdateScore(player3));
            Assert.Equal("Joueur inconnu.", ex.Message);
        }

        [Fact]
        public void SwitchPlayer_ChangesCurrentPlayer()
        {
            Player player1 = new(_p1Name, _color1);
            Player player2 = new(_p2Name, _color2);
            Board board = new();

            Game game = new(player1, player2, board, GameMod.Classique, _cards);
            Assert.Equal(player1, game.CurrentPlayer);

            game.SwitchPlayer();
            Assert.Equal(player2, game.CurrentPlayer);

            game.SwitchPlayer();
            Assert.Equal(player1, game.CurrentPlayer);
        }

        [Fact]
        public void ChangeStateOfGame_SetsIsOverToTrue()
        {
            Player player1 = new(_p1Name, _color1);
            Player player2 = new(_p2Name, _color2);
            Board board = new();
            Game game = new(player1, player2, board, GameMod.Classique, _cards);

            Assert.False(game.IsOver);
            game.ChangeStateOfGame();
            Assert.True(game.IsOver);
        }

        [Fact]
        public void AddAndRemoveCard_ModifiesDeckCorrectly()
        {
            Player player1 = new(_p1Name, _color1);
            Player player2 = new(_p2Name, _color2);
            Board board = new();
            Game game = new(player1, player2, board, GameMod.Classique, _cards);

            OnitamaCard newCard = new("BonusCard", []);
            game.AddCard2Player(player1, newCard);
            Assert.Contains(newCard, game.Player1);

            game.RemoveCardFromPlayer(player1, newCard);
            Assert.DoesNotContain(newCard, game.Player1);
        }



    }
}