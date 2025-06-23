using System.Drawing;
using OnitamaLib.Implementations;
using OnitamaLib.Managers;
using OnitamaLib.Models;
using Color = OnitamaLib.Models.Color;

namespace OnitamaTests.Models_Unit_Tests
{
    public class OnitamaBotTests
    {
        [Fact]
        public void Constructor_SetsNameAndColorCorrectly()
        {
            OnitamaBot bot = new OnitamaBot();

            Assert.Equal("Bob", bot.Name);
            Assert.Equal(Color.BLACK, bot.Color);
            Assert.True(bot.IsBot);
        }

        [Fact]
        public void ChooseMove_ReturnsValidPosition_WhenMoveAvailable()
        {
            OnitamaBot bot = new();
            Board board = new();
            IGameBoardManager boardManager = new ClassicGameBoardManager();
            IMoveManager moveManager = new ClassicBoardMoveManager();

            Pawn pawn = new(bot.Color);
            board.PlacePawn(pawn, new Position(2, 2));

            OnitamaCard card = new("TestCard", [new Position(-1, 0)]);
            Position? chosenMove = bot.ChooseMove(new Position(2, 2), board, boardManager, moveManager, card);

            Assert.NotNull(chosenMove);
            Assert.True(board.IsInBoard(chosenMove));
        }

        [Fact]
        public void ChooseMove_ReturnsNull_WhenNoMoveAvailable()
        {
            OnitamaBot onitamaBot = new();
            OnitamaBot bot = onitamaBot;
            Board board = new();
            IGameBoardManager boardManager = new ClassicGameBoardManager();
            IMoveManager moveManager = new ClassicBoardMoveManager();

            Pawn pawn = new(bot.Color);
            board.PlacePawn(pawn, new Position(0, 0));

            OnitamaCard card = new("TestCard", [new Position(-1, 0)]);
            Position? chosenMove = bot.ChooseMove(new Position(0, 0), board, boardManager, moveManager, card);

            Assert.Null(chosenMove);
        }

        [Fact]
        public void ChoosePawn_ReturnsValidPawnPosition_WhenPawnsAvailable()
        {
            OnitamaBot bot = new();
            Board board = new();
            IGameBoardManager boardManager = new ClassicGameBoardManager();

            Pawn pawn1 = new(bot.Color);
            Pawn pawn2 = new(bot.Color);
            board.PlacePawn(pawn1, new Position(1, 1));
            board.PlacePawn(pawn2, new Position(3, 3));

            Position chosenPawn = bot.ChoosePawn(board, boardManager);

            Assert.True(chosenPawn.Equals(new Position(1, 1)) || chosenPawn.Equals(new Position(3, 3)));
        }

        [Fact]
        public void ChoosePawn_ThrowsException_WhenNoPawnAvailable()
        {
            OnitamaBot bot = new();
            Board board = new();
            IGameBoardManager boardManager = new ClassicGameBoardManager();

            Assert.Throws<InvalidOperationException>(() => bot.ChoosePawn(board, boardManager));
        }
    }
}