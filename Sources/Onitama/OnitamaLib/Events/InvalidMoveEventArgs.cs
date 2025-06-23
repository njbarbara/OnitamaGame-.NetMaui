using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnitamaLib.Events
{

    /// <summary>
    /// Arguments fournis lorsqu'un déplacement invalide est effectué.
    /// </summary>
    public class InvalidMoveEventArgs : EventArgs
    {
        /// <summary>
        /// Indique si le déplacement a été refusé (faux) ou non (vrai).
        /// </summary>
        public bool ResMove { get; }

        /// <summary>
        /// Initialise une nouvelle instance de <see cref="InvalidMoveEventArgs"/>.
        /// </summary>
        /// <param name="resMove">Résultat du déplacement invalide.</param>
        public InvalidMoveEventArgs(bool resMove) => ResMove = resMove;
    }
} 

