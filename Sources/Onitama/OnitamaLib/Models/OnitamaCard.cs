using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OnitamaLib.Models
{
    /// <summary>
    /// Représente une carte  elle possède un nom et un ensemble de mouvements possibles.
    /// </summary>
    /// 


    [DataContract]
    [KnownType(typeof(OnitamaMovement))] 
    public class OnitamaCard
    {


        /// <summary>
        ///  mouvements possibles 
        /// </summary>
        /// 

        [DataMember]
        public OnitamaMovement Movement { get; private set; }

        /// <summary>
        /// Nom de la carte.
        /// </summary>
        /// 

        [DataMember]
        public string Name { get; private set; }


        /// <summary>
        /// Indique si le mouvement voulu est dans les mouvements disponible sur la carte
        /// </summary>
        /// <param name="pawnPosition">Position actuelle du pion</param>
        /// <param name="pawnDestination">Position voulu du pion</param>
        /// <returns>True si le mouvement est présent sinon il retourne False</returns>
        public bool IsInCardMovements(Position pawnPosition, Position pawnDestination) => Movement.Positions.Contains(pawnDestination - pawnPosition);


        /// <summary>
        /// Initialise une nouvelle carte avec un nom et une liste de positions pour former les mouvements
        /// </summary>
        /// <param name="name">Nom de la carte</param>
        /// <param name="positions">Liste des mouvements(qui changera en fonction de  la position du pion elle donne juste une direction par exemple 3 x et 1 y donne 3 horizontale et 1 verticale</param>
        /// <exception cref="ArgumentException">exception si le nom de la carte est vide ou nul</exception>
        public OnitamaCard(string name, List<Position> positions)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("erreur nom de la carte vide.");
            }
            Name = name;
            Movement = new OnitamaMovement([.. positions]);

        }

        /// <summary>
        /// Détermine si cette carte est égale à une autre en comparant le nom et les mouvements
        /// </summary>
        public override bool Equals(object? obj) => obj is OnitamaCard other &&  Name == other.Name && Movement.Positions.SequenceEqual(other.Movement.Positions);

        /// <summary>
        /// Retourne un code de hachage  sur le nom
        /// </summary>
        public override int GetHashCode() => Name.GetHashCode();


    }
}