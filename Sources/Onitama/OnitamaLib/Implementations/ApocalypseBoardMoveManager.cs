using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnitamaLib.Managers;
using OnitamaLib.Models;

namespace OnitamaLib.Implementations
{
    public class ApocalypseBoardMoveManager : ClassicBoardMoveManager
    {
        private readonly OnitamaMovement _apocalypseMovements;
        public override OnitamaMovement  GetForbiddenPositions() => _apocalypseMovements;
        public ApocalypseBoardMoveManager(int Width, int Height)
        {
            int halfHeight = Height / 2, halfWidth = Width / 2;
            List<Position> positions = [
                new Position(0, halfHeight),
                new Position(halfWidth, halfHeight),
                new Position(Width-1, halfHeight)
            ];

            _apocalypseMovements = new OnitamaMovement(positions);
        }
        public override bool IsMoveValid(Board board, Position pawnPosition, Position pawnDestination) => base.IsMoveValid(board, pawnPosition, pawnDestination) && !_apocalypseMovements.Positions.Contains(pawnDestination);
        
    }
}
