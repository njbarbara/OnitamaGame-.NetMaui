using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnitamaLib.Models;

namespace OnitamaTests.Models_Unit_Tests
{
    public class Pawn_UT
    {
        [Theory]
        [InlineData(Color.BLACK)]
        [InlineData(Color.WHITE)]
        public void Pawn_Created_With_Correct_Color(Color color)
        {
            Pawn pawn = new(color);
            Assert.Equal(color, pawn.Color);
            Assert.False(pawn.IsSensei);
        }

        [Fact]
        public void Pawn_Created_Correctly()
        {
            Pawn pawn = new();
            Assert.False(pawn.IsSensei);
        }

    }
}
