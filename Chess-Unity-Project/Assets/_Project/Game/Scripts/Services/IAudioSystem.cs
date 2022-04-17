namespace SteampunkChess
{
    public interface IAudioSystem : IService
    {
        void StartBackgroundMusicLoop();
        void PlaySound(Sounds sounds);

        void SetMusicVolume();

        void SetSoundsVolume();
    }
}