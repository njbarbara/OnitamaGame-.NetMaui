using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
[assembly: InternalsVisibleTo("OnitamaTests")]

namespace OnitamaLib.Models
{
    /// <summary>
    /// Représente un joueur dans le jeu Onitama.
    /// il a un nom,une couleur (White ou Black) et un état (booléen True or False) true il est un bot False il est humain
    /// </summary>
    /// 

    [DataContract]
    [KnownType(typeof(OnitamaBot))] 

    public class Player: ObservableObject

    {

        
        /// <summary>
        /// Nom du joueur.
        /// </summary>

        /// 

        [DataMember]
        public string Name 
        {
            get => name;
            private set
            {
                if (name == value)
                    return;
                name = value;
                OnPropertyChanged();
            }
        }
        private string name = string.Empty;


        /// <summary>
        /// donne l'accès de bot a la classe dérivé  OnitamaBot pour pouvoir lui permettre de mettre l'état à true
        /// </summary>
        /// 
        [DataMember]
        protected bool _isBot;
        /// <summary>
        /// Indique si ce joueur est un bot.
        /// </summary>
        public bool IsBot => _isBot;

        /// <summary>
        /// Couleur associée au joueur.
        /// </summary>
        /// 

        [DataMember]
        public Color Color { get; private set; }


        /// <summary>
        /// Initialise un nouveau joueur avec un nom et une couleur ( il s'agit d'un joueur humain)
        /// </summary>
        /// <param name="name">Nom du joueur avec une vérification pour éviter qu'il soit null ou vide</param>
        /// <param name="color">la couleur associée au joueur</param>
        /// <exception cref="ArgumentException">Si le nom est vide ou invalide</exception>
        public  Player([NotNull]string name, Color color)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("erreur nom vide.");
            }
            Color = color;
            Name = name;
            _isBot = false;
        }


        /// <summary>
        /// rédifinition pour le égal pour comparer les Player pour vérifier le nom et la couleur
        /// </summary>
        /// <param name="obj">Objet à comparer</param>
        /// <returns>True si l’objet est un joueur sont égaux</returns>
        public override bool Equals(object? obj) => obj is Player other && Name == other.Name && Color.Equals(other.Color);

        /// <summary>
        /// Retourne le code de hachage du joueur (son nom et sa couleur)
        /// </summary>
        /// <returns>code de hachage du joueur</returns>
        public override int GetHashCode() => HashCode.Combine(Name, Color);
    }
}




