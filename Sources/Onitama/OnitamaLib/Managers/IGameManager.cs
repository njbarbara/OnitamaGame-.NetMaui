using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Threading.Tasks;
using OnitamaLib.Models;

namespace OnitamaLib.Managers
{
    /// <summary>
    /// Interface définissant les opérations possible pour jouer 
    /// </summary>
    public interface IGameManager
    {
        /// <summary>
        /// Met fin à la partie en cours.
        /// </summary>
        void EndGame();
        /// <summary>
        /// Joue un tour pour le joueur 
        /// </summary>
        /// <param name="cardPlayed">La carte utilisée pour effectuer le mouvement</param>
        /// <param name="pawnPosition">La position actuelle du pion à déplacer</param>
        /// <param name="pawnDestination">La position de destination du pion</param>
        void PlayTurn(OnitamaCard cardPlayed, Position pawnPosition, Position pawnDestination);

        /// <summary>
        /// Joue le tour du bot
        /// </summary>
        void PlayTurn();

        /// <summary>
        /// Crée une nouvelle partie avec deux joueurs
        /// </summary>
        /// <param name="player1Name">Le nom du joueur 1</param>
        /// <param name="color1">La couleur du joueur 1</param>
        /// <param name="player2Name">Le nom du joueur 2</param>
        /// <param name="color2">La couleur du joueur 2</param>
        /// <param name="gameMod">Le mode de jeu sélectionné (Classique ou Apocalypse)</param>
        /// <param name="gameCards">la liste des cartes utilisées pour la partie</param>
        /// <returns>la partie crée <see cref="Game"/> </returns>
        Game CreateGame(string player1Name, Color color1, string player2Name, Color color2, GameMod gameMod, List<OnitamaCard> gameCards);
        /// <summary>
        /// Crée une nouvelle partie contre un bot 
        /// </summary>
        /// <param name="player1Name">Le nom du joueur .</param>
        /// <param name="color1">La couleur du joueur </param>
        /// <param name="gameMod">Le mode de jeu sélectionné (Classique ou Apocalypse)</param>
        /// <param name="gameCards">la lsite cartes utilisées pour la partie</param>
        /// <returns>la partie contre le bot crée <see cref="Game"/> </returns>
        Game CreateGame(string player1Name, Color color1, GameMod gameMod, List<OnitamaCard> gameCards);
        /// <summary>
        /// Vérifie si un joueur a gagné la partie selon les conditions de victoire (élimination du Sensei ou sur le temple).
        /// </summary>
        /// <returns>Le joueur gagnant ou null  </returns>
        Player? CheckVictoryConditions();
        /// <summary>
        /// Vérifie si une victoire a été obtenue par conquête du temple adverse.
        /// </summary>
        /// <returns>Le joueur gagnant ou null si personne n'est encore dans le temple adverse</returns>
        Player? GetCheckTempleVictory();
    }
}
