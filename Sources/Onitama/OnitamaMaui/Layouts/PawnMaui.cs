using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnitamaLib;
using OnitamaLib.Models;
namespace OnitamaMaui.Layouts
{
    public class PawnMaui : ObservableObject
    {
        private readonly Position _position;
        private readonly Pawn? _pawn;
        private string? _backgroundColor;
        public string? ImageSource { get; set; }

        
        public string? BackgroundColor
        {
            get => _backgroundColor;
            set { _backgroundColor = value; OnPropertyChanged(); }
        }
        public Position Position => _position;

        public Pawn? Pawn => _pawn;

        public PawnMaui(Position position, Pawn? pawn)
        {
            if(pawn != null && pawn.Color == OnitamaLib.Models.Color.WHITE)
            {
                if (pawn.IsSensei) ImageSource = "seinsei_blanc.png";
                else ImageSource = "pion_blanc.png";
            }
            else if(pawn != null) {
       
                if (pawn.IsSensei) ImageSource = "seinsei_noir.png";
                else ImageSource = "pion_noir.png";
            }
            _position = position;
            _pawn = pawn;
            _backgroundColor = "beige";
        }
    }
}
