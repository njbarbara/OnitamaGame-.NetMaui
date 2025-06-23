using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnitamaLib.Models;

namespace OnitamaLib.Events
{
    /// <summary>
    /// Arguments fournis lorsque la carte dans la pile est modifiée.
    /// </summary>
    public class CardInStackChangedEventArgs : EventArgs
    {
        /// <summary>
        /// La carte actuellement dans la pile.
        /// </summary>
        public OnitamaCard CardInStack { get; }

        /// <summary>
        /// Initialise une nouvelle instance de <see cref="CardInStackChangedEventArgs"/>.
        /// </summary>
        /// <param name="cardInStack">La carte dans la pile.</param>
        public CardInStackChangedEventArgs(OnitamaCard cardInStack) => CardInStack = cardInStack;
    }
}
