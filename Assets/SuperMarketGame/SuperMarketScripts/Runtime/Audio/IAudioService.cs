namespace SuperMarketGame
{
    public interface IAudioService
    {
        void PlayMusic(UnityEngine.AudioClip clip);
        void StopMusic();
        void SetMusicVolume(float volume);

        // SFX
        void PlaySounds(string soundName);
        void StopSounds();
        void SetVolumn(float volume);
    }
}