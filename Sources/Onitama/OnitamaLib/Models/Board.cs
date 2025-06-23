using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization.Metadata;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.ObjectModel;
using OnitamaLib.Exceptions;
using OnitamaLib.Events;
using System.Runtime.Serialization;
using System.Reflection;

namespace OnitamaLib.Models
{
    /// <summary>
    /// Représente un plateau de jeu Onitama de dimensions fixes (_sizeX x _sizeY) et gère le placement des pions.
    /// </summary>
    /// 

    [DataContract]
    public class Board
    {
        // Constantes de taille du plateau

        [DataMember]
        private const int _sizeX = 5;


        [DataMember]
        private const int _sizeY = 5;

        /// <summary>
        /// Événement déclenché lorsqu'une modification est effectuée sur le plateau.
        /// </summary>
        public event EventHandler<BoardChangedEventArgs>? BoardChanged;

        /// <summary>
        /// Invoque l'événement BoardChanged.
        /// </summary>
        /// <param name="args">Arguments décrivant le changement.</param>
        protected virtual void OnBoardChanged(BoardChangedEventArgs args) => BoardChanged?.Invoke(this, args);



        // Tableau interne de pions
        private  Pawn?[,] _pawns;

        /// <summary>
        /// Largeur effective du plateau.
        /// </summary>
        /// 
      
        public int Width => _pawns.GetLength(0);

        /// <summary>
        /// Hauteur effective du plateau.
        /// </summary>
        /// 
      
        public int Height => _pawns.GetLength(1);

        /// <summary>
        /// Renvoie la position du palais pour une couleur donnée.
        /// </summary>
        /// <param name="color">Couleur du joueur.</param>
        /// <returns>Position du palais.</returns>
        public Position GetPalace(Color color) => (color == Color.WHITE) ? new Position(Width / 2, 0) : new Position(Width / 2, Height - 1);

        /// <summary>
        /// Récupère le pion à la position spécifiée.
        /// </summary>
        /// <param name="x">Coordonnée X.</param>
        /// <param name="y">Coordonnée Y.</param>
        /// <returns>Pawn ou null.</returns>
        public Pawn? GetPawnAt(int x, int y)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height)
                return null;
            return _pawns[x, y];
        }

        /// <summary>
        /// Surcharge de GetPawnAt avec Position.
        /// </summary>
        /// <param name="position">Position ciblée.</param>
        /// <returns>Pawn ou null.</returns>
        public Pawn? GetPawnAt(Position position) => GetPawnAt(position.PositionX, position.PositionY);

        /// <summary>
        /// Liste en lecture seule de tous les emplacements du plateau.
        /// </summary>
        public IEnumerable<Pawn?> Pawns
        {
            get
            {
                List<Pawn?> tmp = [];
                for (int i = 0; i < Width; i++)
                {
                    for (int j = 0; j < Height; j++)
                        tmp.Add(_pawns[i, j]);
                }
                return new ReadOnlyCollection<Pawn?>(tmp);
            }
        }

        /// <summary>
        /// Vérifie si une position est à l'intérieur du plateau.
        /// </summary>
        /// <param name="position">Position à tester.</param>
        /// <returns>True si valide.</returns>
        public bool IsInBoard(Position position) => position.PositionX >= 0 && position.PositionX < Width && position.PositionY >= 0 && position.PositionY < Height;

        /// <summary>
        /// Vérifie la présence d'un pion à la position.
        /// </summary>
        /// <param name="position">Position à vérifier.</param>
        /// <returns>True si un pion est présent.</returns>
        public bool DoIhavePawn(Position position) => (_pawns[position.PositionX, position.PositionY] != null);

        /// <summary>
        /// Vérifie la présence d'un pion aux coordonnées.
        /// </summary>
        /// <param name="x">Coordonnée X.</param>
        /// <param name="y">Coordonnée Y.</param>
        /// <returns>True si un pion est présent.</returns>
        public bool DoIhavePawn(int x, int y) => (_pawns[x, y] != null);

        /// <summary>
        /// Retire le pion à la position spécifiée.
        /// </summary>
        /// <param name="position">Position du pion.</param>
        /// <exception cref="NoPawnException">Si aucune pièce n'est présente.</exception>
        public void RemovePawn(Position position)
        {
            if (DoIhavePawn(position)) _pawns[position.PositionX, position.PositionY] = null;
            else throw new NoPawnException("No pawn here.", position.PositionX, position.PositionY);
        }

        /// <summary>
        /// Place un pion à la position (x, y).
        /// </summary>
        /// <param name="pawn">Pion à placer.</param>
        /// <param name="x">Coordonnée X.</param>
        /// <param name="y">Coordonnée Y.</param>
        public void PlacePawn(Pawn pawn, int x, int y)
        {
            _pawns[x, y] = pawn;
            OnBoardChanged(new BoardChangedEventArgs(this));
        }

        /// <summary>
        /// Surcharge de PlacePawn avec Position.
        /// </summary>
        /// <param name="pawn">Pion à placer.</param>
        /// <param name="position">Position cible.</param>
        public void PlacePawn(Pawn pawn, Position position) => PlacePawn(pawn, position.PositionX, position.PositionY);

        /// <summary>
        /// Initialise un plateau vide de taille 5x5.
        /// </summary>
        public Board() => _pawns = new Pawn[_sizeX, _sizeY];

        [DataMember(Name = "Pawns")]
        private Pawn?[][]? _pawnsSerialized;

        [OnSerializing]
        void OnSerializing(StreamingContext context)
        {
            _pawnsSerialized = new Pawn?[_sizeX][];
            for (int x = 0; x < _sizeX; x++)
            {
                _pawnsSerialized[x] = new Pawn?[_sizeY];
                for (int y = 0; y < _sizeY; y++)
                {
                    _pawnsSerialized[x][y] = _pawns[x, y];
                }
            }
        }

        [OnDeserializing]
        private void OnDeserializing(StreamingContext context)
        {
            _pawns = new Pawn?[_sizeX, _sizeY];
        }


        [OnDeserialized]
        void OnDeserialized(StreamingContext context)
        {
            if (_pawnsSerialized != null)
            {
                for (int x = 0; x < _pawnsSerialized.Length; x++)
                {
                    for (int y = 0; y < _pawnsSerialized[x].Length; y++)
                    {
                        _pawns[x, y] = _pawnsSerialized[x][y];
                    }
                }
            }
        }

       




    }
}

    
