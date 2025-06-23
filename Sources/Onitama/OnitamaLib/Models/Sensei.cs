using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace OnitamaLib.Models
{
    /// <summary>
    /// Représente un pion de type Sensei dans le jeu Onitama.
    /// Le Sensei est un pion mais avec une importance supplémentaire
    /// </summary>
    /// 



    [DataContract]
    public class Sensei : Pawn
    {
        /// <summary>
        /// Initialise un nouveau Sensei avec la couleur que l'on veut 
        /// utilise le constructeur de pawn mais en précisant que c'est un Sensei
        /// </summary>
        /// <param name="color">la couleur du Sensei</param>
        
        public Sensei(Color color)
        :
            base(color)
        {
            _isSensei = true;
        }

    }
}
