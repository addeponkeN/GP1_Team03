using System;
using System.IO;
using Util;

namespace Settings
{
    public static class GameSettings
    {
        public const string PublisherName = "Futuregames";
        public const string ProjectName = "BikeMania";

        private const string Filetype = "cfg";
        private const string Filename = "settings";

        private static readonly string AppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        private static readonly string SavePath = $"{AppData}/{PublisherName}/{ProjectName}/";


        private static GameSettingsFile _file;

        public static GameSettingsFile Get
        {
            get
            {
                if(_file == null)
                {
                    Load();
                }

                return _file;
            }
        }

        private static string GetSettingsFullPath()
        {
            return $"{SavePath}/{Filename}.{Filetype}";
        }

        public static void Save()
        {
            JsonHelper.Save(GetSettingsFullPath(), _file);
        }

        public static void Load()
        {
            //  checks if directory exists, if not - creates new directory
            Directory.CreateDirectory(SavePath);

            string fullPath = GetSettingsFullPath();

            //  if no settings file could be found - create new & save
            if(!File.Exists(fullPath))
            {
                CreateNew();
            }
            else
            {
                if((_file = JsonHelper.Load<GameSettingsFile>(fullPath)) == null)
                    CreateNew();
            }
        }

        private static void CreateNew()
        {
            _file = new GameSettingsFile();
            Save();
        }
    }
}