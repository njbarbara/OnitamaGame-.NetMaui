using Plugin.Maui.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace OnitamaMaui.Functionalities
{
    public class SoundManager : ISoundManager
    {
        private readonly IAudioManager _audioManager;
        private IAudioPlayer? gameMusic;
        private IAudioPlayer? gameSound;


        public SoundManager(IAudioManager audioManager)
        {
            _audioManager = audioManager;
        }

        public async Task PlayMusic(string source) {
            gameMusic = _audioManager.CreatePlayer(await FileSystem.OpenAppPackageFileAsync(source));
            gameMusic.Loop = true;
            gameMusic.Play();
        }

        public async Task PlaySound(string source) {
            gameSound = _audioManager.CreatePlayer(await FileSystem.OpenAppPackageFileAsync(source));
            gameSound.Play();
        }

        public void StopMusic() => gameMusic?.Stop();

        public void SetGameMusicVolume(float volume)
        {
            if (gameMusic != null)
                gameMusic.Volume = volume;
        }

        public void SetGameSoundVolume(float volume)
        {
            if (gameSound != null)
                gameSound.Volume = volume;
        }

    }
}
