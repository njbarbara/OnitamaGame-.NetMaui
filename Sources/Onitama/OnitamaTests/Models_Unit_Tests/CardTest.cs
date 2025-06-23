using System;
using System.Collections.Generic;
using OnitamaLib.Models;
using Xunit;
namespace OnitamaTests.Models_Unit_Tests
{
    public class CardTest
    {

        [Fact]
        
        public void Card_test_Creation_Name_Empty_Return_Exception()
        {
            List<Position> positions = [new(0, 0), new(4, 4)];
            var exception = Assert.Throws<ArgumentException>(() => new OnitamaCard(" ", positions));
            Assert.Equal("erreur nom de la carte vide.", exception.Message);
        }

        [Fact]
        public void Card_test_Creation()
        {
            List<Position> positions = [new(0, 0), new(4, 4)];
            string Cardname = "Card1";
            OnitamaCard card = new(Cardname, positions);
            var movement = card.Movement;
            Assert.Equal(Cardname, card.Name);
            Assert.Equal(positions, movement.Positions);
        }

        public static IEnumerable<object[]> MovementTestData =>
        [
                [new Position(0, 0), new Position(1, 0), true],
                [new Position(2, 2), new Position(3, 1), true],
                [new Position(0, 0), new Position(0, 1), false],
                [new Position(3, 3), new Position(4, 4), false]
        ];
        [Theory]
        [MemberData(nameof(MovementTestData))]
        public void IsInCardMovements_Check(Position source, Position destination, bool expected)
        {
            List<Position> movementList = [new(1, 0), new(1, -1)];
            OnitamaCard card = new("TestCard", movementList);

            Assert.Equal(expected, card.IsInCardMovements(source, destination));
        }
    }
}
