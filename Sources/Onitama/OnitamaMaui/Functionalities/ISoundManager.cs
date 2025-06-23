using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnitamaMaui.Functionalities
{
    public interface ISoundManager
    {
        public Task PlayMusic(string source);
        public Task PlaySound(string source);

        public void SetGameMusicVolume(float volume);

        public void SetGameSoundVolume(float volume);
        public void StopMusic();


    }
}
