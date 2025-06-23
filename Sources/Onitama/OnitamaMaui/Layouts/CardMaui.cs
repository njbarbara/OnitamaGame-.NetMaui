using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnitamaLib;
using OnitamaLib.Models;

namespace OnitamaMaui.Layouts
{
    public class CardMaui : ObservableObject
    {

        private OnitamaCard _card;
        private readonly int _indice;
        private bool _isSelected;

        public OnitamaCard Card
        {

        get => _card; 
        set
        {
            _card = value;
            OnPropertyChanged();
        }
        }
        public int Indice => _indice;


        public bool IsSelected {
            get => _isSelected;
            
            set
            {
                _isSelected = value;
                OnPropertyChanged();
            }
        }
        public CardMaui(OnitamaCard card, int indice)
        {
            _card = card;
            _indice = indice;
            _isSelected = false;
        }
    }
}
