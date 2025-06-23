using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnitamaLib.Models;
using OnitamaLib.PersistanceManagers;

namespace Onitama.Persistance.Stub;

public class StubSaveManager : ISaveManager
{
    public bool SaveGame(Game game,string path, string filename)=> true;

}
