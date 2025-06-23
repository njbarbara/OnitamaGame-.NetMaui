using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnitamaLib.Events;
using OnitamaLib.Managers;
using OnitamaLib.Models;

namespace OnitamaLib.Implementations
{
    public class ClassicBoardMoveManager : IMoveManager
    {
        public virtual OnitamaMovement GetForbiddenPositions() => new();

        public virtual bool IsMoveValid(Board board, Position pawnPosition, Position pawnDestination)
        {
            if (!board.IsInBoard(pawnDestination)) return false;

            Pawn? sourcePawn = board.GetPawnAt(pawnPosition);
            Pawn? destinationPawn = board.GetPawnAt(pawnDestination);

            return sourcePawn != null && (destinationPawn == null || destinationPawn.Color != sourcePawn.Color);
        }
        public bool TryMove(OnitamaCard card, Board board, Position pawnPosition, Position pawnDestination)
        {
            if (!card.IsInCardMovements(pawnPosition, pawnDestination))
                return false;
            if (!IsMoveValid(board, pawnPosition, pawnDestination))
                return false;           
            Pawn? sourcePawn = board.GetPawnAt(pawnPosition);
            if (sourcePawn == null)
                return false;

            board.RemovePawn(pawnPosition);
            board.PlacePawn(sourcePawn, pawnDestination);

            return true;
        }

        public OnitamaMovement GetAvailableMove(OnitamaCard card, Board board, Position pawnPosition)
        {
            List <Position> movement = [];
            OnitamaMovement cardMovement = card.Movement;
            foreach (Position cardPosition in cardMovement.Positions.Where(cardPosition => IsMoveValid(board, pawnPosition, pawnPosition + cardPosition)))
                movement.Add(pawnPosition + cardPosition);
            
            return new OnitamaMovement(movement);
        }
    }
}
