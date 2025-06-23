using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnitamaLib.Events;
using OnitamaLib.Models;

namespace OnitamaLib.Managers
{
    /// <summary>
    /// Interface qui donne les méthodes possible pour les déplacements
    /// </summary>
    public interface IMoveManager
    {
        /// <summary>
        /// Vérifie si le déplacement d'un pion vers une position est possible
        /// </summary>
        /// <param name="board">le plateau de jeu courant</param>
        /// <param name="pawnPosition">la position actuelle du pion</param>
        /// <param name="pawnDestination">La position voulu</param>
        /// <returns> true si le déplacement est valide ou false si pas valide</returns>
        public bool IsMoveValid(Board board, Position pawnPosition, Position pawnDestination);
        /// <summary>
        /// Tente d'effectuer un mouvement selon une carte et met à jour le plateau si le mouvement est valide
        /// </summary>
        /// <param name="card">la carte utilisée pour le déplacement</param>
        /// <param name="board">Le plateau de jeu </param>
        /// <param name="pawnPosition">La position de départ du pion</param>
        /// <param name="pawnDestination">La position d’arrivée (voulu)</param>
        /// <returns>true si le déplacement a été effectué false si pas valide</returns>
        public bool TryMove(OnitamaCard card, Board board, Position pawnPosition, Position pawnDestination);
        /// <summary>
        /// Obtient tous les déplacements possibles à partir d'une position 
        /// </summary>
        /// <param name="card">La carte </param>
        /// <param name="board">Le plateau de jeu </param>
        /// <param name="pawnPosition">La position du pion choisi</param>
        /// <returns>les movements  <see cref="OnitamaMovement"/> adisponible (possible de jouer)</returns>
        public OnitamaMovement GetAvailableMove(OnitamaCard card, Board board, Position pawnPosition);
        /// <summary>
        /// Renvoie les positions interdites pour le joueur courant (par exemple, hors limites ou occupées par un allié)
        /// </summary>
        /// <returns>les movements <see cref="OnitamaMovement"/> qui ont les postions non valide</returns>
        public OnitamaMovement GetForbiddenPositions();
    }

}
