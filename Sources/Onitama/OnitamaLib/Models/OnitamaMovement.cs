using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OnitamaLib.Models
{
    /// <summary>
    /// Représente un ensemble de mouvements possibles, donnés par la liste des positions possibles
    /// </summary>
    /// 

    [DataContract]
    
    public class OnitamaMovement
    {

        /// <summary>
        /// Liste privés des positions 
        /// </summary>
        /// 
        [DataMember]
        private readonly List<Position> _positions;

        /// <summary>
        /// Expose en tant que IEnumerable les positions de mouvement  pour protéger le code en donnant le moins possible
        /// </summary>
        /// 

        public IEnumerable<Position> Positions => new ReadOnlyCollection<Position>(_positions);


        /// <summary>
        /// Initialise un nouvel ensemble de mouvements avec une liste de positions 
        /// </summary>
        /// <param name="positions">Liste des positions qui donne les déplacements possibles</param>
        public OnitamaMovement(List<Position> positions) => _positions = new List<Position>([.. positions]);


        /// <summary>
        /// Initialise un  ensemble de mouvements vide
        /// </summary>
        public OnitamaMovement() { _positions = []; }
    }
}
