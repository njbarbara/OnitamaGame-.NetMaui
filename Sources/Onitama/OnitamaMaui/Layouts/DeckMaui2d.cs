using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnitamaLib.Models;

namespace OnitamaMaui.Layouts
{
    public class DeckMaui2d
    {
        private List<CardMaui> _cards;

        public DeckMaui2d(IEnumerable<OnitamaCard> cards) {
            _cards = [];
            CopyCardsFromPlayerDeck(cards);
        }

        public void CopyCardsFromPlayerDeck(IEnumerable<OnitamaCard> cards)
        {
            int cpt = 0;
            foreach (OnitamaCard card in cards)
            {
                _cards.Add(new CardMaui(card, cpt));
                cpt++;
            }
        }

        public void UpdatePlayerDecks(IEnumerable<OnitamaCard> cards)
        {
            for (int i = 0; i < _cards.Count; i++)
            {
                _cards[i].Card = cards.ElementAt(i);
            }
        }

        public IEnumerable<CardMaui> FlatMatrix2d => new ReadOnlyCollection<CardMaui>(_cards);


    }
}

