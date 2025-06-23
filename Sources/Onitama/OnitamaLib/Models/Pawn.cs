using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OnitamaLib.Models
{
    /// <summary>
    /// Représente un pion dans le jeu 
    /// Un pion possède une couleur ( on utilisera la class Sensei qui hérite pour dire que c'est un sensei
    /// </summary>
    /// 


    [DataContract]
    [KnownType(typeof(Sensei))]
    public class Pawn
    {
        /// <summary>
        /// donne l'accès à la classe dérivé Sensei pour lui permettre de changer l'état de IsSensei
        /// </summary>
        /// 
        [DataMember]
        protected bool _isSensei;


        /// <summary>
        /// bool indiquant si ce pion est un Sensei
        /// </summary>
        public bool IsSensei => _isSensei;

        /// <summary>
        /// Couleur associée au pion
        /// </summary>
        /// 

        [DataMember]
        public Color Color { get; private set; }


        /// <summary>
        /// Initialise un nouveau pion avec la couleur voulu
        /// Par défaut, le pion n'est pas un Sensei
        /// </summary>
        /// <param name="color">la couleur du pion.</param>
        public Pawn(Color color)
        {
            Color = color;
            _isSensei = false;
        }

        public Pawn() { }
    }
}
