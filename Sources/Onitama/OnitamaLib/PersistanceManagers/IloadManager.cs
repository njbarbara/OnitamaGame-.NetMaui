using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnitamaLib.Models;

namespace OnitamaLib.PersistanceManagers
{
    public interface IloadManager
    {
        public Game LoadGame(string path,string filename);

        public List<OnitamaCard> InitializeGameCards();
    }
}
