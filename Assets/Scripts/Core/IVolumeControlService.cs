namespace Core
{
    public interface IVolumeControlService
    {
        int MaxVolumeValue { get; }
        int SoundVolume { get; }
        int MusicVolume { get; }
        
        void SetMusicVolume(int value);
        void SetSoundVolume(int value);
    }
}