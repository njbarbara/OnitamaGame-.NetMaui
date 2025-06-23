using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization;
using OnitamaLib.Exceptions;
using OnitamaLib.Events;
using OnitamaLib.Models;
using Xunit;
using Color = OnitamaLib.Models.Color;

namespace OnitamaTests.Models_Unit_Tests
{
    public class Board_UT
    {
        private readonly Board board;
        private readonly int width = 5;
        private readonly int height = 5;

        public Board_UT()
        {
            board = new Board();
        }

        [Fact]
        public void Board_Created_Correctly()
        {
            Assert.Equal(width, board.Width);
            Assert.Equal(height, board.Height);
        }

        [Theory]
        [InlineData(0, 0, true)]
        [InlineData(4, 4, true)]
        [InlineData(-1, 0, false)]
        [InlineData(0, -1, false)]
        [InlineData(5, 0, false)]
        [InlineData(0, 5, false)]
        public void IsInBoard_Check_If_In_Bounds_Correctly(int x, int y, bool expected)
        {
            Assert.Equal(expected, board.IsInBoard(new Position(x, y)));
        }

        [Theory]
        [InlineData(1, 1, Color.WHITE)]
        [InlineData(2, 3, Color.BLACK)]
        [InlineData(0, 4, Color.WHITE)]
        public void PlacePawn_Coordinates(int x, int y, Color color)
        {
            Pawn pawn = new(color);
            board.PlacePawn(pawn, x, y);

            Assert.True(board.DoIhavePawn(x, y));
            Assert.Equal(pawn, board.GetPawnAt(x, y));
        }

        [Theory]
        [InlineData(3, 2, Color.BLACK)]
        [InlineData(0, 0, Color.WHITE)]
        public void PlacePawn_By_Position(int x, int y, Color color)
        {
            Position pos = new(x, y);
            Pawn pawn = new(color);
            board.PlacePawn(pawn, pos);

            Assert.True(board.DoIhavePawn(pos));
            Assert.Equal(pawn, board.GetPawnAt(x, y));
        }

        [Theory]
        [InlineData(1, 2)]
        [InlineData(4, 0)]
        public void RemovePawn_Removes_Correctly(int x, int y)
        {
            Position pos = new(x, y);
            board.PlacePawn(new Pawn(Color.WHITE), pos);
            board.RemovePawn(pos);

            Assert.False(board.DoIhavePawn(pos));
            Assert.Null(board.GetPawnAt(x, y));
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(3, 3)]
        public void RemovePawn_Throws_If_Empty(int x, int y)
        {
            Position pos = new(x, y);
            Assert.False(board.DoIhavePawn(pos));
            Assert.Throws<NoPawnException>(() => board.RemovePawn(pos));
        }

        [Theory]
        [InlineData(Color.WHITE, 2, 0)]
        [InlineData(Color.BLACK, 2, 4)]
        public void GetPalace_ReturnsCorrectPosition(Color color, int expectedX, int expectedY)
        {
            Assert.Equal(new Position(expectedX, expectedY), board.GetPalace(color));
        }

        [Theory]
        [InlineData(-1, 0)]
        [InlineData(0, -1)]
        [InlineData(5, 0)]
        [InlineData(0, 5)]
        public void GetPawnAt_ReturnsNull_WhenOutOfBounds(int x, int y)
        {
            Assert.Null(board.GetPawnAt(x, y));
            Assert.Null(board.GetPawnAt(new Position(x, y)));
        }

        [Fact]
        public void GetPawnAt_ReturnsPawn_WhenPresent()
        {
            Position pos = new(2, 2);
            Pawn pawn = new(Color.BLACK);
            board.PlacePawn(pawn, pos);

            Assert.Equal(pawn, board.GetPawnAt(pos));
            Assert.Equal(pawn, board.GetPawnAt(2, 2));
        }

        [Fact]
        public void Pawns_ReturnsAllPositions()
        {
            board.PlacePawn(new Pawn(Color.WHITE), 1, 1);
            board.PlacePawn(new Pawn(Color.BLACK), 3, 3);

            ReadOnlyCollection<Pawn?> pawns = (ReadOnlyCollection<Pawn?>)board.Pawns;
            Assert.Equal(25, pawns.Count);
            Assert.NotNull(pawns[6]); // (1,1) = 1*5 + 1
            Assert.NotNull(pawns[18]); // (3,3) = 3*5 + 3
            Assert.Null(pawns[0]); // (0,0)
        }

        [Fact]
        public void BoardChanged_EventTriggered_OnPlacePawn()
        {
            bool eventTriggered = false;
            BoardChangedEventArgs? capturedArgs = null;
            board.BoardChanged += (sender, args) => { eventTriggered = true; capturedArgs = args; };

            board.PlacePawn(new Pawn(Color.WHITE), new Position(2, 2));

            Assert.True(eventTriggered);
            Assert.NotNull(capturedArgs);
            Assert.Equal(board, capturedArgs.Board);
        }

        [Fact]
        public void Serialization_Deserialization_PreservesPawns()
        {
            board.PlacePawn(new Pawn(Color.BLACK), 2, 2);

            DataContractSerializer serializer = new(typeof(Board));
            MemoryStream stream = new();
            serializer.WriteObject(stream, board);
            stream.Position = 0;
            Board deserializedBoard = (Board)serializer.ReadObject(stream)!;

            Assert.Equal(board.Width, deserializedBoard.Width);
            Assert.Equal(board.Height, deserializedBoard.Height);
            Assert.NotNull(deserializedBoard.GetPawnAt(2, 2));
            Assert.Null(deserializedBoard.GetPawnAt(0, 0));
        }
    }
}