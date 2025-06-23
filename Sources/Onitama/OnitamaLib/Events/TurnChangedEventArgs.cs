using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnitamaLib.Models;

namespace OnitamaLib.Events
{
    /// <summary>
    /// Arguments fournis lorsqu'un changement de tour a lieu.
    /// </summary>
    public class TurnChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Le joueur dont c'est le tour.
        /// </summary>
        public Player Player { get; }

        /// <summary>
        /// Le score du joueur au moment du changement de tour.
        /// </summary>
        public int Score { get; }

        /// <summary>
        /// Les cartes du joueur au moment du changement de tour.
        /// </summary>
        public IEnumerable<OnitamaCard> PlayerCard { get; }

        /// <summary>
        /// Initialise une nouvelle instance de <see cref="TurnChangedEventArgs"/>.
        /// </summary>
        /// <param name="player">Le joueur actif.</param>
        /// <param name="score">Le score du joueur.</param>
        /// <param name="decks">Les cartes possédées par le joueur.</param>
        public TurnChangedEventArgs(Player player, int score, IEnumerable<OnitamaCard> decks)
        {
            Player = player;
            Score = score;
            PlayerCard = decks;
        }
    }
}

