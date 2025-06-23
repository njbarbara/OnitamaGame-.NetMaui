using OnitamaLib.Models;

namespace OnitamaLib.Implementations
{
    /// <summary>
    /// Manager qui calculer le score en fin de partie
    /// </summary>
    public static class ScoreManager
    {
        /// <summary>
        /// Calcule le score final des deux joueurs en fonction de leur nombre de coups (ScoreP1, ScoreP2).
        /// Applique un multiplicateur selon le nombre de coups du joueur gagnant(plus le nombre de coups et bas plus le multiplicateur est haut)
        /// </summary>
        /// <param name="game">Instance du jeu contenant les scores actuels des joueurs.</param>
        /// <returns>
        /// Un tuple contenant les scores  (score du joueur 1, score du joueur 2).
        /// </returns>
        public static (int, int) CalcScore(Game game)
        {
            int nbcoupP1 = game.ScoreP1;
            int nbcoupP2 = game.ScoreP2;

            int finalScoreP1 = nbcoupP1;
            int finalScoreP2 = nbcoupP2;

            if (nbcoupP1 > nbcoupP2)
            {
                int multiplier = 2;

                if (nbcoupP1 < 5) multiplier = 10;
                else if (nbcoupP1 >= 20) multiplier = 2;
                else if (nbcoupP1 >= 5) multiplier = 5;

                finalScoreP1 *= multiplier;
            }
            else if (nbcoupP2 > nbcoupP1)
            {
                int multiplier = 2;

                if (nbcoupP2 < 5) multiplier = 10;
                else if (nbcoupP2 >= 20) multiplier = 2;
                else if (nbcoupP2 >= 5) multiplier = 5;

                finalScoreP2 *= multiplier;
            }

            return (finalScoreP1, finalScoreP2);
        }
    }
}
