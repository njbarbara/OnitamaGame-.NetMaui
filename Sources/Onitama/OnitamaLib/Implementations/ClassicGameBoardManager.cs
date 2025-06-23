using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnitamaLib.Managers;
using OnitamaLib.Models;

namespace OnitamaLib.Implementations
{
    public class ClassicGameBoardManager : IGameBoardManager
    {
        public Board GenerateBoard()
        {
            Board board = new();
            int tailleX = board.Width, tailleY = board.Height;
            for (int i = 0; i < tailleX; i++)
                if (i == tailleX / 2) board.PlacePawn(new Sensei(Color.WHITE), i, 0);
                else board.PlacePawn(new Pawn(Color.WHITE), i, 0);
            for (int j = 0; j < tailleY; j++)
                if (j == tailleX / 2) board.PlacePawn(new Sensei(Color.BLACK), j, tailleY - 1);
                else board.PlacePawn(new Pawn(Color.BLACK), j, tailleY - 1);
            return board;
        }

        public OnitamaMovement GetPawnsPositionsColor(Color color, Board board)
        {
            List<Position> positions = [];
            Pawn? pawn;

            for (int x = 0; x < board.Width; x++)
            {
                for (int y = 0; y < board.Height; y++)
                {
                    pawn = board.GetPawnAt(x, y);
                    if (pawn != null && pawn.Color == color)
                        positions.Add(new Position(x, y));
                }
            }
            return new OnitamaMovement(positions);
        }
        public bool ISenseiAlive(Color color, Board board)
        {
            Pawn? pawn;
            for (int x = 0; x < board.Width; x++)
            {
                for (int y = 0; y < board.Height; y++)
                {
                    pawn = board.GetPawnAt(x, y);
                    if (pawn != null && pawn.Color == color && pawn.IsSensei) return true;
                }
            }
            return false;
        }
    }
}
