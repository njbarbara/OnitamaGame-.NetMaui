using System;
using System.Collections.Generic;
using OnitamaLib.Models;

namespace OnitamaTests.Models_Unit_Tests
{
    public class PositionTest
    {
        [Fact]
        public void Create_Position()
        {
            Position pos = new(0, 5);

            Assert.Equal(0, pos.PositionX);
            Assert.Equal(5, pos.PositionY);

        }
        [Fact]
        public void TestReturnTrueWhenPos1EqualPos2()
        {
            Position pos1 = new(5, 10);
            Position pos2 = new(5, 10);

            Assert.True(pos1.Equals(pos2));
            Assert.Equal(pos1.GetHashCode(), pos2.GetHashCode());
        }

        [Fact]
        public void Test_Return_False_When_Pos1_Not_Equal_Pos2()
        {
            Position pos1 = new(5, 10);
            Position pos2 = new(5, 11);

            Assert.False(pos1.Equals(pos2));
            Assert.NotEqual(pos1.GetHashCode(), pos2.GetHashCode());
        }

        [Fact]
        public void TestReturnFalseWhenComparedWithNull()
        {
            Position pos = new(1, 1);
            Assert.False(pos.Equals(null));
        }

        [Fact]
        public void TestReturnFalseWhenComparedWithOtherType()
        {
            Position pos = new(1, 1);
            string notAPosition = "not a position";

            Assert.False(pos.Equals(notAPosition));
        }

        [Fact]
        public void Position_NotEquals_DifferentCoordinates_ReturnsTrue()
        {
            Position pos1 = new(1, 2);
            Position pos2 = new(2, 1);

            Assert.True(pos1 != pos2);
        }

    }
}
