using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnitamaLib;
using OnitamaLib.Events;
using OnitamaLib.Managers;
using OnitamaLib.Models;
namespace OnitamaMaui.Layouts
{
    public class BoardMaui2d : ObservableObject
    {
        private readonly PawnMaui[,] matrix;
        private readonly OnitamaMovement _forbiddenPositions;

        public BoardMaui2d(Board board, OnitamaMovement forbiddenPosition)
        {
            matrix = new PawnMaui[board.Width,board.Height];
            _forbiddenPositions = forbiddenPosition;

            PlacePawnMauiOnMatrix(board);
            PlaceForbiddenPositions();


            board.BoardChanged += OnBoardChanged;
        }

        private void OnBoardChanged(object? sender, BoardChangedEventArgs e)
        {
            PlacePawnMauiOnMatrix(e.Board);
            PlaceForbiddenPositions();
            OnPropertyChanged(nameof(FlatMatrix2d));
        }



        private void PlacePawnMauiOnMatrix(Board board)
        {
            for (int i = 0; i < board.Width; i++)
            {
                for (int j = 0; j < board.Height; j++)
                {
                    matrix[i, j] = new PawnMaui(new Position(i, j), board.GetPawnAt(i, j));
                }
            }
        }

        private void PlaceForbiddenPositions()
        {
            foreach (Position position in _forbiddenPositions.Positions)
                matrix[position.PositionX, position.PositionY].ImageSource = "apocalypse_block.png";
        }
        public int NbRows => matrix?.GetLength(0) ?? 0;
        public int NbColumns => matrix?.GetLength(1) ?? 0;

        public PawnMaui[,] Matrix
        {
            get
            {
                if (matrix == null) return new PawnMaui[,] { { } };

                PawnMaui[,] mat = new PawnMaui[NbRows, NbColumns];
                for (int numRow = 0; numRow < NbRows; numRow++)
                {
                    for (int numCol = 0; numCol < NbColumns; numCol++)
                    {
                        mat[numRow, numCol] = matrix[numRow, numCol];
                    }
                }
                return matrix;
            }
        }

        public void PlaceAvailableMoves(OnitamaMovement movement)
        {
            foreach (Position position in movement.Positions)
            {
                if (matrix[position.PositionX, position.PositionY].Pawn == null)
                    matrix[position.PositionX, position.PositionY].BackgroundColor = "red";
                else
                    matrix[position.PositionX, position.PositionY].BackgroundColor = "blue";
            }
        }

        public void RemoveAvailableMoves()
        {
            foreach (PawnMaui pawn in matrix)
                   pawn.BackgroundColor = "white";
        }
        public IEnumerable<PawnMaui> FlatMatrix2d
        {
            get
            {
                List<PawnMaui> flatMatrix = [];

                if (matrix == null) return flatMatrix;

                for (int numRow = 0; numRow < NbRows; numRow++)
                {
                    for (int numCol = 0; numCol < NbColumns; numCol++)
                    {
                        flatMatrix.Add(matrix[numRow, numCol]);
                    }
                }
                return flatMatrix;
            }
        }
    }
}

