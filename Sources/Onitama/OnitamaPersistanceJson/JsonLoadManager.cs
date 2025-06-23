using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using OnitamaLib.Models;
using OnitamaLib.PersistanceManagers;
using System.IO;

namespace OnitamaPersistanceJson
{
    public class JsonLoadManager : IloadManager
    {
        public Game LoadGame(string fileName, string saveFolderPath)
        {
            if (string.IsNullOrEmpty(saveFolderPath))
            {
                throw new InvalidOperationException("Le dossier de sauvegarde n'est pas défini.");
            }

            string filePath = Path.Combine(saveFolderPath, fileName);

            DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(Game));

            using (FileStream stream2 = File.OpenRead(filePath))
            {
                Game? game1 = jsonSerializer.ReadObject(stream2) as Game;
                if (game1 == null)
                {
                    throw new InvalidOperationException("Erreur chargement game vide");
                }
                return game1;
            }
        }

        public static List<string> GetAllgame(string saveJsonPath)
        {
            if (!Directory.Exists(saveJsonPath))
            {
                Directory.CreateDirectory(saveJsonPath);
            }

            var fichiers = Directory.GetFiles(saveJsonPath).ToList();

            return fichiers;
        }

        public List<Game> LoadAllGames(string Path)
        {
            List<Game> games = new List<Game>();
            List<string> files = GetAllgame(Path);

            DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(Game));

            foreach (string file in files)
            {
                try
                {
                    using (FileStream stream = File.OpenRead(file))
                    {
                        Game? game = jsonSerializer.ReadObject(stream) as Game;
                        if (game != null)
                        {
                            games.Add(game);
                        }
                        else
                        {
                            Console.WriteLine($"Fichier ignoré (objet null) : {file}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erreur lors du chargement du fichier {file} : {ex.Message}");
                }
            }

            return games;
        }

        public List<OnitamaCard> InitializeGameCards()
        {
            return [
            new OnitamaCard("boar",
            [
                new(-1, 0),
                new(0, 1),
                new(1, 0),
            ]),
            new OnitamaCard("cobra",
            [
                new(-1, 0),
                new(1, 1),
                new(1, -1)
            ]),
            new OnitamaCard("crab",
            [
                new(0, 1),
                new(2, 0),
                new(-2, 0),
            ]),
            new OnitamaCard("frog",
            [
                new(-1, 1),
                new(-2, 0),
                new(1, -1)
            ]),
            new OnitamaCard("goose",
            [
                new(-1, 0),
                new(-1, 1),
                new(1,0),
                new (1, -1)
            ]),
            new OnitamaCard("monkey",
            [
                new(-1, 1),
                new(1, 1),
                new(-1, -1),
                new(1, -1)
            ]),
            new OnitamaCard("rabbit",
            [
                new(1, 1),
                new(2, 0),
                new(-1, -1)
            ])
        ];
        }
    }
}
