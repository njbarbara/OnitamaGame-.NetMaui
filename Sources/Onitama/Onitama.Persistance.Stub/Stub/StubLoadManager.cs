using System.Collections.Generic;
using OnitamaLib.Models;
using OnitamaLib.PersistanceManagers;
using OnitamaLib.Managers;
using OnitamaLib.Implementations;

namespace Onitama.Persistance.Stub;

public class StubLoadManager : IloadManager
{
    public List<Game> LoadedGames { get; private set; }
    readonly ClassicGameBoardManager BoardManager = new();
    readonly DeckManager deckManager = new();

    public StubLoadManager()
    {
        LoadedGames = LoadAllGames();
        if (LoadedGames == null || LoadedGames.Count == 0)
        {
            throw new InvalidOperationException("loadedGames is not initialized correctly.");
        }
    }

    public List<OnitamaCard> InitializeGameCards()
    {
        return [
            new OnitamaCard("rabbit",
            [
                new(-2, -1),
                new(-1, 2),
                new(1, 1),
                new(3, 0)
            ]),
            new OnitamaCard("frog",
            [
                new(-1, 0),
                new(0, 2),
                new(2, -1)
            ]),
            new OnitamaCard("crab",
            [
                new(1, -2),
                new(-2, 1),
                new(0, 3),
                new(4, 0)
            ]),
            new OnitamaCard("monkey",
            [
                new(-3, 0),
                new(1, 2),
                new(0, -1)
            ]),
            new OnitamaCard("pig",
            [
                new(2, 1),
                new(-1, -1),
                new(0, 4)
            ]),
            new OnitamaCard("monkey",
            [
                new(0, 1),
                new(1, 0),
                new(-1, -2),
                new(2, 3)
            ]),
            new OnitamaCard("goose",
            [
                new(-1, 0),
                new(1, -1),
                new(0, 2)
            ]),
            new OnitamaCard("boar",
            [
                new(-1, 0),
                new(1, 0),
                new(0, -3),
                new(3, 1)
            ])
        ];
    }




    public Game LoadGame(string path, string filename) => new(new Player("alban", Color.WHITE), new Player("najib", Color.BLACK), BoardManager.GenerateBoard(), GameMod.Classique, [.. deckManager.GenerateGameCards(InitializeGameCards())]);



    public List<Game> LoadAllGames()
    {
        return
        [
            new Game(new Player("alban1", Color.WHITE), new Player("najib", Color.BLACK), BoardManager.GenerateBoard(), GameMod.Classique, [.. deckManager.GenerateGameCards(InitializeGameCards())]),
            new Game(new Player("alban2", Color.WHITE), new OnitamaBot(), BoardManager.GenerateBoard(), GameMod.Classique, [.. deckManager.GenerateGameCards(InitializeGameCards())]),
            new Game(new Player("alban2", Color.WHITE), new Player("booster", Color.BLACK), BoardManager.GenerateBoard(), GameMod.Classique, [.. deckManager.GenerateGameCards(InitializeGameCards())]),
            new Game(new Player("booster", Color.WHITE), new Player("alban1", Color.BLACK), BoardManager.GenerateBoard(), GameMod.Classique, [.. deckManager.GenerateGameCards(InitializeGameCards())]),
            new Game(new Player("booster", Color.WHITE), new Player("najib", Color.BLACK), BoardManager.GenerateBoard(), GameMod.Classique, [.. deckManager.GenerateGameCards(InitializeGameCards())])
        ];
    }

}



