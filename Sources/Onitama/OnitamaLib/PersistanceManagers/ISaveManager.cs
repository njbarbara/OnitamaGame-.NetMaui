using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnitamaLib.Models;

namespace OnitamaLib.PersistanceManagers
{
    public interface ISaveManager
    {
        public bool SaveGame(Game game,string path,string filename);
    }
}
