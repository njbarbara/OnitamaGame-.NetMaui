using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnitamaLib.Models;

namespace OnitamaLib.Managers
{
    /// <summary>
    /// Interface définissant les opérations liées à la gestion des cartes 
    /// </summary>
    public interface IDeckManager
    {
        /// <summary>
        /// Permet d’échanger une carte jouée par le joueur courant avec la carte  en résèrve
        /// </summary>
        /// <param name="card">la carte jouée par le joueur</param>
        /// <param name="game">la partie en cours.</param>
        public void SwitchCard(OnitamaCard card, Game game);

        /// <summary>
        /// Génère les cartes utilisées pour une partie à partir d’un ensemble de cartes disponibles
        /// </summary>
        /// <param name="gamecards">la liste complète des cartes disponibles</param>
        /// <returns>Un sous-ensemble de cartes à utiliser pour la partie(5 cartes)</returns>
        public IEnumerable<OnitamaCard> GenerateGameCards(List<OnitamaCard> gamecards);

    }
}
