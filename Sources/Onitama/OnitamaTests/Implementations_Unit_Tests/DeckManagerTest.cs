using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnitamaLib.Implementations;

using OnitamaLib.Managers;
using OnitamaLib.Models;
using Onitama.Persistance.Stub;
using OnitamaLib.PersistanceManagers;

namespace OnitamaTests.Implementations_Unit_Tests
{
    public class DeckManagerTests
    {
        readonly StubLoadManager loadManager;
        readonly DeckManager deckManager;
        private readonly Game game;


        public DeckManagerTests() { 
            loadManager = new StubLoadManager();
            game = loadManager.LoadGame("test", "chemin test");
            deckManager = new DeckManager();
        }   

        [Fact]
        public void SwitchCard_ShouldMoveCardFromPlayerDeckToStack()
        {
            var initialDeck = game.Player1;
            Assert.NotEmpty(initialDeck);
            OnitamaCard initialStackCard = game.CardInStack;
            Assert.NotNull(initialStackCard);

            OnitamaCard cardToPlay = initialDeck.First(); 
            deckManager.SwitchCard(cardToPlay, game); 
            Assert.DoesNotContain(cardToPlay, game.Player1);
            Assert.Contains(initialStackCard, game.Player1);
            Assert.NotEqual(initialStackCard, game.CardInStack);
        }


        [Fact]
        public void SwitchCard_ShouldThrowException_WhenCardNotInDeck()
        {
            OnitamaCard invalidCard = new("invalidCard", []);

            Assert.Throws<ArgumentException>(() => deckManager.SwitchCard(invalidCard, game));
        }
    }

}
