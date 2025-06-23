using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using OnitamaLib.Managers;

namespace OnitamaLib.Models
{

    /// <summary>
    /// Représente un bot dans le jeu
    /// Il choisi aléatoirement ses coups parmis ce qu'on lui donne
    /// </summary>
    /// 
    [DataContract]
    public class OnitamaBot : Player
    {

        /// <summary>
        /// Initialise un bot nommé "Bob" avec la couleur noire
        /// </summary>
        public OnitamaBot() : base("Bob", Color.BLACK) {            
            _isBot = true;    
        }


        /// <summary>
        /// Choisit une position de destination pour déplacer un pion,
        /// en parcourant les cartes et les pions jusqu’à trouver un mouvement valide
        /// </summary>
        /// <param name="board">le pllateau de jeu </param>
        /// <param name="gameBoardManager">le  manager pour obtenir les positions des pions</param>
        /// <param name="moveManager">le moveManager pour obtenir les mouvements possibles</param>
        /// <param name="card">la carte choisi</param> 
        /// <returns>Une position  valide pour un mouvement</returns>
        /// <exception cref="InvalidOperationException">Si aucun mouvement n'est possible</exception>
        public Position?  ChooseMove(Position pawnPosition, Board board, IGameBoardManager gameBoardManager, IMoveManager moveManager, OnitamaCard card)
        {
            OnitamaMovement availableMove;

            availableMove = moveManager.GetAvailableMove(card, board, pawnPosition);
             if (availableMove.Positions.Any())
                 return availableMove.Positions.ElementAt(RandomNumberGenerator.GetInt32(availableMove.Positions.Count()));
            
            return null;
        }



        /// <summary>
        /// Choisit aléatoirement un pion parmi ceux disponibles pour le bot
        /// </summary>
        /// <param name="board">le plateau de jeu actuel</param>
        /// <param name="gameBoardManager"> le board manager pour obtenir les positions des pions</param>
        /// <returns>Une position représentant le pion choisi</returns>
        /// <exception cref="InvalidOperationException">Si aucun pion n'est disponible</exception>
        public Position ChoosePawn(Board board, IGameBoardManager gameBoardManager)
        {
            OnitamaMovement pawnsPositions = gameBoardManager.GetPawnsPositionsColor(Color, board);
            IEnumerable<Position> positions = pawnsPositions.Positions;

            if (!pawnsPositions.Positions.Any())
                throw new InvalidOperationException("Aucun pion disponible pour le bot.");

            return positions.ElementAt(RandomNumberGenerator.GetInt32(positions.Count()));
        }

        

    }
}
