using OnitamaLib;
using OnitamaLib.Events;
using OnitamaLib.Managers;
using OnitamaLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnitamaMaui.Layouts
{
    public class CurrentPlayerMaui : ObservableObject
    {
        private readonly Game _game;

        private bool _isCurrentPlayer1;
        private bool _isCurrentPlayer2;

        private int _nbPawnEatenPlayer1;
        private int _nbPawnEatenPlayer2;

        public int NbPawnEatenPlayer1 { 
            get => _nbPawnEatenPlayer1;
            private set
            {
                _nbPawnEatenPlayer1 = value;
                OnPropertyChanged();
            }
        }

        public int NbPawnEatenPlayer2 {
            get => _nbPawnEatenPlayer2;
            private set
            {
                _nbPawnEatenPlayer2 = value;
                OnPropertyChanged();
            }
        }

        private readonly IGameBoardManager _boardManager;


        public bool IsCurrentPlayer1 {
            get => _isCurrentPlayer1;
            set {
                _isCurrentPlayer1 = value;
                OnPropertyChanged();
            } 
                            
        }
        public bool IsCurrentPlayer2 { 
            get => _isCurrentPlayer2;
            set
            {
                _isCurrentPlayer2 = value;
                OnPropertyChanged();
            }
        }

        public CurrentPlayerMaui(Game game, IGameBoardManager BoardManager) { 
            _game = game;
            IsCurrentPlayer1 = (_game.CurrentPlayer == _game.GetPlayer(0));
            IsCurrentPlayer2 = (_game.CurrentPlayer == _game.GetPlayer(1));

            _boardManager = BoardManager;

            _game.TurnChanged += ChangedCurrentPlayer;
            _game.Board.BoardChanged += ChangeNbPawnEaten;
        }

        public void ChangeNbPawnEaten(object? sender, BoardChangedEventArgs e)
        {
            NbPawnEatenPlayer1 = 5-_boardManager.GetPawnsPositionsColor(_game.GetPlayer(0).Color, _game.Board).Positions.Count();
            NbPawnEatenPlayer2 = 5-_boardManager.GetPawnsPositionsColor(_game.GetPlayer(1).Color, _game.Board).Positions.Count();
        }

        private void ChangedCurrentPlayer(object? sender, TurnChangedEventArgs e)
        {
            IsCurrentPlayer1 = (e.Player == _game.GetPlayer(0));
            IsCurrentPlayer2 = (e.Player == _game.GetPlayer(1));
        }
    }
}
