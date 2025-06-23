using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OnitamaLib.Models
{
    /// <summary>
    /// Représente une position  (X, Y) 
    /// </summary>
    /// 

    [DataContract]
    public class Position
    {
        /// <summary>
        /// Coordonnée horizontale  
        /// </summary>
        /// 

        [DataMember]
        public int PositionX { get; set; }

        /// <summary>
        /// Coordonnée verticale
        /// </summary>
        /// 

        [DataMember]
        public int PositionY { get; set; }


        /// <summary>
        /// construit une position avec un x et un y
        /// </summary>
        /// <param name="x">Coordonnée X(horizontale)</param>
        /// <param name="y">Coordonnée Y(vericale)</param>
        public Position(int x, int y)
        {
            PositionX = x;
            PositionY = y;
        }
        public Position() { }


        /// <summary>
        /// rédèfinie le Equals pour vérifier si deux positions sont égales
        /// </summary>
        /// <param name="obj">Objet à comparer</param>
        /// <returns>True si les deux positions ont les mêmes coordonnées</returns>
        public override bool Equals(object? obj)=> obj is Position p && this == p;

        /// <summary>
        /// operateur == pour comparer les postions
        /// </summary>
        public static bool operator ==(Position position1, Position position2) => position1.PositionX == position2.PositionX && position1.PositionY == position2.PositionY;

        /// <summary>
        /// operateur != pour comparer les postions
        /// </summary>
        public static bool operator !=(Position position1, Position position2) => !(position1 == position2);

        /// <summary>
        /// operateur + pour l'addition des positions
        /// </summary>
        public static Position operator +(Position a, Position b) => new(a.PositionX + b.PositionX, a.PositionY + b.PositionY);

        /// <summary>
        /// operateur - pour  la soustraction des positions
        /// </summary>
        public static Position operator -(Position a, Position b) => new(a.PositionX - b.PositionX, a.PositionY - b.PositionY);


        /// <summary>
        /// Retourne un code de hachage avec la combianaison de X et y pour position
        /// </summary>
        public override int GetHashCode() => HashCode.Combine(PositionX, PositionY);
    }
}
