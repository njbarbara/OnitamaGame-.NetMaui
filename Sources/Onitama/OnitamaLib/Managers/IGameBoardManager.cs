using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnitamaLib.Models;

namespace OnitamaLib.Managers
{
    /// <summary>
    /// Interface définissant les opérations liées à la gestion du plateau 
    /// </summary>
    public interface IGameBoardManager
    {
        /// <summary>
        /// Génère un plateau de jeu initialisé selon les règles du mode de jeu
        /// </summary>
        /// <returns>Une instance de <see cref="Board"/> </returns>
        public Board GenerateBoard();


        /// <summary>
        /// Vérifie si le Sensei d’une couleur donner en paramètre est encore en vie sur le plateau.
        /// </summary>
        /// <param name="color">La couleur du joueur (WHITE ou BLACK)</param>
        /// <param name="board">Le plateau de jeu .</param>
        /// <returns>true  si le Sensei est encore présent sur le plateau ou sinon  false</returns>
        public bool ISenseiAlive(Color color, Board board);


        /// <summary>
        /// Récupère les positions des pions appartenant à un joueur de la couleur du paramètre
        /// </summary>
        /// <param name="color">La couleur du joueur dont on souhaite obtenir les positions des pions</param>
        /// <param name="board">Le plateau de jeu </param>
        /// <returns>Un objet <see cref="OnitamaMovement"/> contenant les positions des pions</returns>
        public OnitamaMovement GetPawnsPositionsColor(Color color, Board board);

    }
}
