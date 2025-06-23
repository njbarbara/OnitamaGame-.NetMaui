using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;
using OnitamaLib.Models;
using OnitamaLib.PersistanceManagers;

namespace OnitamaPersistanceJson
{

    public class JsonSaveManager : ISaveManager
    {
        public bool SaveGame(Game game, string filePath, string filename)
        {
            try
            {
                string? dossier = Path.GetDirectoryName(filePath); 
                if (dossier != null && !Directory.Exists(dossier)) 
                {
                    Directory.CreateDirectory(dossier);
                }

                game.FileName = filename;

                DataContractJsonSerializer jsonSerializer = new(typeof(Game));

                using (FileStream stream = File.Create(filePath))
                {
                    using (var writer = JsonReaderWriterFactory.CreateJsonWriter(
                                 stream,
                                 System.Text.Encoding.UTF8,
                                 false,
                                 true))
                    {
                        jsonSerializer.WriteObject(writer, game);
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
    }



}



