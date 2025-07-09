using System.Collections.Generic;
using MoreMountains.NiceVibrations;
using UnityEngine;

namespace SuperMarketGame
{
    public class AudioService : MonoBehaviour, IAudioService
    {
        [Header("Audio Sources")]
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource SoundSource;

        [Header("Audio Clips")]
        [SerializeField] private AudioClip bgMusicClip;
        [SerializeField] private AudioClip[] audioClips;

        private Dictionary<string, AudioClip> _AudioClipsDic;
        private static AudioService _instance;
        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
                return;
            }
            _instance = this;

            _AudioClipsDic = new Dictionary<string, AudioClip>();

            foreach (var clips in audioClips)
            {
                _AudioClipsDic[clips.name] = clips;
            }
            new AudioManager(this);
            MMVibrationManager.iOSInitializeHaptics();
            PlayMusic(bgMusicClip);
            DontDestroyOnLoad(this);
        }

        public void PlaySounds(string soundName)
        {

            if (_AudioClipsDic.TryGetValue(soundName, out var clips))
            {
                SoundSource.PlayOneShot(clips);
                //   SoundSource.clip = clips;
                //  SoundSource.Play();
            }
        }


        public void SetVolumn(float Volume)
        {
            SoundSource.volume = Volume;
        }

        public void StopSounds()
        {
            SoundSource.Pause();
        }

        public void PlayMusic(AudioClip clip)
        {
            if (clip == null) return;

            musicSource.clip = clip;
            musicSource.loop = true;
            musicSource.Play();
        }

        public void StopMusic() => musicSource.Stop();
        public void SetMusicVolume(float volume) => musicSource.volume = Mathf.Clamp01(volume);


    }
}