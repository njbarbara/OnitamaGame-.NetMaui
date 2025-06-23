using System.Runtime.InteropServices;
using OnitamaLib.Models;

namespace OnitamaTests.Models_Unit_Tests;

public class PlayerTest
{
    [Fact]
    public void Player_Test_Creation_name_return_test1()
    {
        Player player1 = new("test1",Color.BLACK);
 
        Assert.Equal("test1", player1.Name);


    }
    [Fact]
    public void Player_Test_Creation_name_if_empty()
    {
        ArgumentException exception = Assert.Throws<ArgumentException>( () => 

        { Player player1 = new("  ",Color.WHITE); }

        );
        Assert.Equal("erreur nom vide.", exception.Message);       

    }

    [Fact]
    public void Player_Test_Creation_Bot()
    {
        Player bot = new OnitamaBot();

        Assert.Equal("Bob", bot.Name);

    }

    [Fact]
    public void Player_Test_Bot_Is_true()
    {
        OnitamaBot bot = new();
        Assert.True(bot.IsBot);
    }

    [Fact]
    public void Player_Test_Color_()
    {
        Player player = new("bob", Color.BLACK);
        Assert.Equal(Color.BLACK, player.Color);
    }

    [Fact]
    public void Player_Test_Bot_Has_Correct_Color()
    {
        Player bot = new OnitamaBot();
        Assert.Equal(Color.BLACK, bot.Color);
    }

    [Theory]
    [InlineData("Alice", Color.WHITE, "Alice", Color.WHITE, true)]
    [InlineData("Alice", Color.WHITE, "Bob", Color.WHITE, false)]
    [InlineData("Alice", Color.WHITE, "Alice", Color.BLACK, false)]
    [InlineData("Alice", Color.WHITE, "Bob", Color.BLACK, false)]
    public void Equals_VariousCases_ReturnsExpected(string name1, Color color1, string name2, Color color2, bool expected)
    {
        var player1 = new Player(name1, color1);
        var player2 = new Player(name2, color2);

        Assert.Equal(expected, player1.Equals(player2));
    }

    [Fact]
    public void Equals_Null_ReturnsFalse()
    {
        Player player = new("Alice", Color.WHITE);
        Assert.False(player.Equals(null));
    }

    [Fact]
    public void Equals_DifferentType_ReturnsFalse()
    {
        Player player = new("Alice", Color.WHITE);
        object notAPlayer = "Alice";
        Assert.False(player.Equals(notAPlayer));
    }



}



