using System;
using System.Collections.Generic;
using OnitamaLib.Managers;
using OnitamaLib.Models;
using OnitamaLib.Implementations;
using Xunit;

namespace OnitamaTests.Impementations_Unit_Tests
{
    public class ClassicGameBoardManager_UT
    {
        OnitamaLib.Implementations.ClassicGameBoardManager gameboard;
        private readonly Board board;

        public ClassicGameBoardManager_UT()
        {
            gameboard = new OnitamaLib.Implementations.ClassicGameBoardManager();
            board = gameboard.GenerateBoard();
        }

        [Fact]
        public void Board_Created_Correctly()
        {
            Assert.NotNull(board);
            Assert.Equal(5, board.Width);
            Assert.Equal(5, board.Height);
        }

        [Theory]
        [InlineData(Color.BLACK)]
        [InlineData(Color.WHITE)]
        public void GetPawnsPositionsColor_Returns_Correct_Pawns(Color color)
        {
            OnitamaMovement movement = gameboard.GetPawnsPositionsColor(color, board);


            Assert.NotNull(movement);
            Assert.All(movement.Positions, pos =>
            {
                Pawn? pawn = board.GetPawnAt(pos.PositionX, pos.PositionY);
                Assert.NotNull(pawn);
                Assert.Equal(color, pawn.Color);
            });

            Assert.Equal(5, movement.Positions.Count());
        }

        [Theory]
        [InlineData(Color.BLACK)]
        [InlineData(Color.WHITE)]
        public void ISenseiAlive_Returns_True_At_Start(Color color)
        {
            bool alive = gameboard.ISenseiAlive(color, board);
            Assert.True(alive);
        }

        [Fact]
        public void ISenseiAlive_Returns_False_When_Sensei_Killed()
        {
            board.PlacePawn(new Pawn(Color.WHITE), 2,0);
            
            bool alive = gameboard.ISenseiAlive(Color.WHITE, board);

            Assert.False(alive);
        }
    }
}
