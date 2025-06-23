using System.Collections.Generic;
using Xunit;
using OnitamaLib.Models;
using OnitamaLib.Implementations;
using OnitamaLib.Managers;

namespace OnitamaTests.Implementations_Unit_Tests
{
    public class ApocalypseBoardMoveManagerTests
    {
        private readonly OnitamaLib.Implementations.ClassicGameBoardManager gameboard;
        private readonly ApocalypseBoardMoveManager moveManager;
        private readonly Board board;

        public ApocalypseBoardMoveManagerTests()
        {
            gameboard = new OnitamaLib.Implementations.ClassicGameBoardManager();
            board = gameboard.GenerateBoard();
            moveManager = new ApocalypseBoardMoveManager(5,5);
        }

        public static IEnumerable<object[]> IsMoveValidData => new List<object[]>
        {
            new object[]{ new Position(2, 0), new Position(0, 2), false },
            new object[]{ new Position(2, 0), new Position(2, 2), false },
            new object[]{ new Position(2, 0), new Position(4, 2), false },
            new object[]{ new Position(2, 0), new Position(2, 1), true  },
            new object[]{ new Position(2, 0), new Position(2, 5), false }
        };

        [Theory]
        [MemberData(nameof(IsMoveValidData))]
        public void IsMoveValid_Give_Expected_Results(Position source, Position destination, bool expected) =>
            Assert.Equal(expected, moveManager.IsMoveValid(board, source, destination));

        public static IEnumerable<object[]> TryMoveData => new List<object[]>
        {
            new object[]{ new Position(2, 0), new Position(2, 2), new OnitamaCard("Test", new List<Position>{ new Position(0, 2) }), false },
            new object[]{ new Position(2, 0), new Position(2, 1), new OnitamaCard("Test", new List<Position>{ new Position(0, 1) }), true  },
            new object[]{ new Position(2, 0), new Position(3, 0), new OnitamaCard("Test", new List<Position>{ new Position(1, 0) }), false }
        };

        [Theory]
        [MemberData(nameof(TryMoveData))]
        public void TryMove_Move_Pawn_Like_Expected(Position source, Position destination, OnitamaCard card, bool expected) =>
            Assert.Equal(expected, moveManager.TryMove(card, board, source, destination));

        public static IEnumerable<object[]> GetAvailableMoveData => new List<object[]>
        {
            new object[]{
                new Position(2, 0),
                new OnitamaCard("Test", new List<Position>{ new Position(0, 1), new Position(1, 0), new Position(-1, 0) }),
                new List<Position>{ new Position(2, 1) }
            }
        };

        [Theory]
        [MemberData(nameof(GetAvailableMoveData))]
        public void GetAvailableMove_Filters_Apocalypse(Position source, OnitamaCard card, List<Position> expected)
        {
            OnitamaMovement available = moveManager.GetAvailableMove(card, board, source);
            Assert.Equal(expected.Count, available.Positions.Count());
            foreach (Position p in expected)
                Assert.Contains(p, available.Positions);
        }

        private static IEnumerable<Position> GetPositions(OnitamaMovement available)
        {
            return available.Positions;
        }

        public static IEnumerable<object[]> ForbiddenMovementData => new List<object[]>
        {
            new object[]{ new Position(0, 2) },
            new object[]{ new Position(2, 2) },
            new object[]{ new Position(4, 2) }
        };

        [Theory]
        [MemberData(nameof(ForbiddenMovementData))]
        public void GetForbiddenMove_Contains_The_Correct_Positions(Position expected) => Assert.Contains(expected, moveManager.GetForbiddenPositions().Positions);
    }
}
