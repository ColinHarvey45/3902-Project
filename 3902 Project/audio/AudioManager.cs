using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using CSE_3902_Project;

namespace Audio
{
    // Owns the game's music and sound effects. The music loops from the moment it starts, and the mute switch
    // turns only the music off; sound effects always play. Anything can ask for a sound effect by name (Sound)
    internal sealed class AudioManager : IDisposable
    {
        // Well under the sound effects, so they stand out over it (the music file is much louder than they are)
        private const float MusicVolume = 0.3f;

        private readonly Dictionary<Sound, SoundEffect> soundEffects = [];
        private SoundEffectInstance music;

        // The warning beep, repeated for as long as Link is low on hearts
        private SoundEffectInstance lowHealthBeep;

        public static AudioManager Instance { get; } = new AudioManager();

        public bool IsMuted { get; private set; }

        private AudioManager()
        {
        }

        public void LoadAllSounds(ContentManager content)
        {
            music = content.Load<SoundEffect>(ContentNames.Music).CreateInstance();
            music.IsLooped = true;
            music.Volume = MusicVolume;

            foreach (KeyValuePair<Sound, string> sound in ContentNames.SoundEffects)
                soundEffects[sound.Key] = content.Load<SoundEffect>(sound.Value);

            lowHealthBeep = soundEffects[Sound.LowHealth].CreateInstance();
            lowHealthBeep.IsLooped = true;
        }

        // Plays the sound once, over anything else playing; does nothing if the sounds haven't been loaded
        public void Play(Sound sound)
        {
            if (soundEffects.TryGetValue(sound, out SoundEffect soundEffect))
                soundEffect.Play();
        }

        // Starts the music from the beginning if it isn't playing, so the reset key doesn't restart it
        public void PlayMusic()
        {
            if (music != null && music.State != SoundState.Playing)
                music.Play();
        }

        // For the game over and win screens, which have their own sounds
        public void StopMusic()
        {
            music?.Stop();
        }

        // Starts or stops the low-health beep; asking for it again while it is already on (or off) does nothing
        public void SetLowHealthAlarm(bool on)
        {
            if (lowHealthBeep == null || on == (lowHealthBeep.State == SoundState.Playing))
                return;

            if (on)
                lowHealthBeep.Play();
            else
                lowHealthBeep.Stop();
        }

        public void ToggleMute()
        {
            IsMuted = !IsMuted;

            if (music != null)
                music.Volume = IsMuted ? 0f : MusicVolume;
        }

        public void Dispose()
        {
            music?.Dispose();
            lowHealthBeep?.Dispose();
        }
    }
}
