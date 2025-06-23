using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
namespace OnitamaLib.Exceptions
{
    [Serializable]
    /// <summary>
    /// Exception levée lorsqu'on tente de supprimer un pion inexistant du plateau.
    /// </summary>
    public class NoPawnException : Exception, ISerializable
    {
        /// <summary>
        /// Coordonnée X où l'exception a été lancée.
        /// </summary>
        public int Xcoordinates { get; }

        /// <summary>
        /// Coordonnée Y où l'exception a été lancée.
        /// </summary>
        public int Ycoordinates { get; }

        public NoPawnException(string message, int x, int y)
            : base(message)
        {
            Xcoordinates = x;
            Ycoordinates = y;
        }
    }
}
