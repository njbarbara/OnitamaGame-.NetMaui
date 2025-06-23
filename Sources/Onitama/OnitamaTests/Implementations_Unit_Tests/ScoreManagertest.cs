using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnitamaLib.Implementations;
using OnitamaLib.Managers;
using OnitamaLib.Models;

namespace OnitamaTests.Impementations_Unit_Tests
{
    public class ScoreManagertest
    {
        private readonly List<OnitamaCard> _cards =
           [
               new("carte1", []),
                new("carte2", []),
                new("carte3", []),
                new("carte4", []),
                new("carte5", [])
           ];
        [Fact]
        public void CalcScore_WhenNbCoupsP1_IsHigherThanNbCoupsP2()
        {
            ClassicGameBoardManager boardManager = new();
            Player player1 = new("P1", Color.WHITE);
            Player player2 = new("P2", Color.BLACK);
            Board board = boardManager.GenerateBoard();
            Game game = new(player1,player2,board, GameMod.Classique, _cards);
 

            game.UpdateScore( player1);
            game.UpdateScore( player1);
            game.UpdateScore(player2);


            var (scoreP1, scoreP2) = ScoreManager.CalcScore(game);


            Assert.Equal(20, scoreP1);
            Assert.Equal(1, scoreP2);
        }

        [Fact]
        public void CalcScore_WhenNbCoupsP2_IsHigherThanNbCoupsP1()
        {
            ClassicGameBoardManager boardManager = new();
            Player player1 = new("P1", Color.WHITE);
            Player player2 = new("P2", Color.BLACK);
            Board board = boardManager.GenerateBoard();
            Game game = new(player1, player2, board, GameMod.Classique, _cards);


            game.UpdateScore(player1);
            game.UpdateScore(player2);
            game.UpdateScore(player2);


            var (scoreP1, scoreP2) = ScoreManager.CalcScore(game);


            Assert.Equal(1, scoreP1);
            Assert.Equal(20, scoreP2);
        }

        [Theory]
        
        [InlineData(4, 0, 40, 0)]   
        [InlineData(10, 0, 50, 0)]  
        [InlineData(20, 0, 40, 0)]  
        [InlineData(1, 5, 1, 25)]   
        [InlineData(0, 4, 0, 40)]   
        public void CalcScore_MultipleCases_ReturnsExpectedScores(int scoreP1, int scoreP2, int expectedP1, int expectedP2)
        {
          
            ClassicGameBoardManager boardManager = new();
            Player player1 = new("P1", Color.WHITE);
            Player player2 = new("P2", Color.BLACK);
            Board board = boardManager.GenerateBoard();
            Game game = new(player1, player2, board, GameMod.Classique, _cards);


            for (int i = 0; i < scoreP1; i++) game.UpdateScore(player1);
            for (int i = 0; i < scoreP2; i++) game.UpdateScore(player2);

          
            var (finalScoreP1, finalScoreP2) = ScoreManager.CalcScore(game);

           
            Assert.Equal(expectedP1, finalScoreP1);
            Assert.Equal(expectedP2, finalScoreP2);
        }
    }



}

