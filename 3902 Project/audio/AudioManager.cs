using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System;

namespace Audio
{
    // Owns the game's music and sound effects. The music loops from the moment it starts; the mute
    // switch turns off everything that plays through MonoGame's SoundEffect, so sound effects added
    // later are muted by it automatically
    internal sealed class AudioManager : IDisposable
    {
        private SoundEffectInstance music;

        public static AudioManager Instance { get; } = new AudioManager();

        public bool IsMuted { get; private set; }

        private AudioManager()
        {
        }

        public void LoadAllSounds(ContentManager content)
        {
            SoundEffect track = content.Load<SoundEffect>("BackgroundMusic");
            music = track.CreateInstance();
            music.IsLooped = true;
        }

        // Does nothing if the music is already playing, so the reset key doesn't restart it
        public void PlayMusic()
        {
            if (music != null && music.State != SoundState.Playing)
                music.Play();
        }

        public void ToggleMute()
        {
            IsMuted = !IsMuted;
            SoundEffect.MasterVolume = IsMuted ? 0f : 1f;
        }

        public void Dispose()
        {
            music?.Dispose();
        }
    }
}