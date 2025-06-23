using System.Collections.Generic;
using Xunit;
using OnitamaLib.Models;
using OnitamaLib.Implementations;
using OnitamaLib.Managers;

namespace OnitamaTests.Impementations_Unit_Tests
{
    public class ClassicBoardMoveManagerTests
    {
        readonly OnitamaLib.Implementations.ClassicGameBoardManager gameboard;
        readonly ClassicBoardMoveManager moveManager;
        readonly Board board;

        public ClassicBoardMoveManagerTests()
        {
            moveManager = new ClassicBoardMoveManager();
            gameboard = new OnitamaLib.Implementations.ClassicGameBoardManager();
            board = gameboard.GenerateBoard();
        }

        public static IEnumerable<object[]> IsMoveValidData => new List<object[]>
        {
            new object[] { new Position(2, 0), new Position(2, 1), true },
            new object[] { new Position(2, 0), new Position(2, 4), true },
            new object[] { new Position(2, 0), new Position(3, 0), false },
            new object[] { new Position(0, 1), new Position(0, 2), false },
            new object[] { new Position(2, 0), new Position(2, -1), false },
        };

        [Theory]
        [MemberData(nameof(IsMoveValidData))]
        public void IsMoveValid_Give_Expected_Results(Position source, Position destination, bool expected)
        {
            Assert.Equal(expected, moveManager.IsMoveValid(board, source, destination));
        }

        public static IEnumerable<object[]> TryMoveDatas => new List<object[]>
        {
            new object[] { new Position(2,0), new Position(2,1), new OnitamaCard("Test", new List<Position> { new Position(0,1) }), true },
            new object[] { new Position(2,0), new Position(3,0), new OnitamaCard("Test", new List<Position> { new Position(0,1) }), false },
            new object[] { new Position(2,0), new Position(1,0), new OnitamaCard("Test", new List<Position> { new Position(-1,0) }), false },
            new object[] { new Position(3,3), new Position(1,0), new OnitamaCard("Test", new List<Position> { new Position(-1,0) }), false }

        };

        [Theory]
        [MemberData(nameof(TryMoveDatas))]
        public void TryMove_Move_Pawn_Like_Expected(Position source, Position destination, OnitamaCard card, bool expected)
        {
            Assert.Equal(expected, moveManager.TryMove(card, board, source, destination));
        }

        public static IEnumerable<object[]> GetAvailableMoveDatas => new List<object[]>
        {
            new object[] { new Position(2,0), new OnitamaCard("Test", new List<Position> { new Position(0,1), new Position(1,0) }), new List<Position> { new Position(2,1) } },
            new object[] { new Position(3,0), new OnitamaCard("Test", new List<Position> { new Position(0,1), new Position(-1,1) }), new List<Position> { new Position(3,1), new Position(2,1) } }
        };

        [Theory]
        [MemberData(nameof(GetAvailableMoveDatas))]
        public void GetAvailableMove_Give_Expected_Positions(Position source, OnitamaCard card, List<Position> expected)
        {
            OnitamaMovement available = moveManager.GetAvailableMove(card, board, source);
            Assert.Equal(expected.Count, available.Positions.Count());
            foreach (Position p in expected)
                Assert.Contains(p, available.Positions);
        }

        [Fact]
        public void GetForbiddenMoves_Should_Return_Empty_As_ClassicMoveMg() => Assert.Empty(moveManager.GetForbiddenPositions().Positions);

    }
}
