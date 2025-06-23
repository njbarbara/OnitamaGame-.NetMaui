using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnitamaLib.Models;

namespace OnitamaLib.Events
{
    /// <summary>
    /// Arguments fournis lors de la modification du plateau de jeu.
    /// </summary>
    public class BoardChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Référence au plateau de jeu ayant subi la modification.
        /// </summary>
        public Board Board { get; }

        /// <summary>
        /// Initialise une nouvelle instance de <see cref="BoardChangedEventArgs"/>.
        /// </summary>
        /// <param name="board">Le plateau de jeu modifié.</param>
        public BoardChangedEventArgs(Board board) => Board = board;
    }
}
