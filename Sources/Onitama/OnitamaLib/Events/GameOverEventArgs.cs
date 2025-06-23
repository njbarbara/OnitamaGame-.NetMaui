using OnitamaLib.Models;

namespace OnitamaLib.Events{ 
    /// <summary>
    /// Arguments fournis lorsque la partie est terminée.
    /// </summary>
    public class GameOverEventArgs : EventArgs
    {
        /// <summary>
        /// La partie terminée.
        /// </summary>
        public Game Game { get; }

        /// <summary>
        /// Initialise une nouvelle instance de <see cref="GameOverEventArgs"/>.
        /// </summary>
        /// <param name="game">L'objet représentant la partie terminée.</param>
        public GameOverEventArgs(Game game) => Game = game;
    }
}