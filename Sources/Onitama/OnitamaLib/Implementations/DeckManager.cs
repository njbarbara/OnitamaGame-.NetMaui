using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnitamaLib.Managers;
using OnitamaLib.Models;
using System.Security.Cryptography;
using OnitamaLib.Events;

namespace OnitamaLib.Implementations
{
    public class DeckManager : IDeckManager
    {
        private readonly RandomNumberGenerator _randomGenerator;

        public event EventHandler<CardInStackChangedEventArgs>? CardInStackChanged;
        protected virtual void OnCardInStackChanged(CardInStackChangedEventArgs args) => CardInStackChanged?.Invoke(this, args);

        public DeckManager() => _randomGenerator = RandomNumberGenerator.Create();

        public void SwitchCard(OnitamaCard card, Game game)
        {
            if (!game.GetDeckCurrentPlayer().Contains(card))
                throw new ArgumentException("Carte ou joueur invalide");
            game.RemoveCardFromPlayer(game.CurrentPlayer, card);
            game.AddCard2Player(game.CurrentPlayer, game.CardInStack);
            game.AddCardInStack(card);

            OnCardInStackChanged(new CardInStackChangedEventArgs(game.CardInStack)); 
        }
        
        public IEnumerable<OnitamaCard> GenerateGameCards(List<OnitamaCard> gamecards)
        {
            List <OnitamaCard> cards = new (5);
            int index;
            byte[] data;

            for (int i = 0; i < 5; i++) {
                data = new byte[4];
                _randomGenerator.GetBytes(data);
                index = BitConverter.ToInt32(data, 0) % gamecards.Count;
                if (index < 0)
                    index += gamecards.Count;
                cards.Add(gamecards[index]);
                gamecards.RemoveAt(index);
            }
            return cards;
        }       
    }
}
