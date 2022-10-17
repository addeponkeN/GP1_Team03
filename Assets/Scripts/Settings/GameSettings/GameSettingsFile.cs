namespace Settings.GameSettings
{
    public class GameSettingsFile : RootFile
    {
        public SettingOption<float> MasterVolume = new("mastervol", 0.5f);
        public SettingOption<float> SfxVolume = new("sfxvol", 1f);
        public SettingOption<float> MusicVolume = new("musicvol", 0.5f);
    }
}