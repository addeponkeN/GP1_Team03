using System;
using UnityEngine;

namespace Util
{
    public static class PathHelper
    {
        public const string PublisherName = "Futuregames";
        public const string ProjectName = "BikeMania";
        public const string ExternalDataName = "Data";

        public const string LeaderboardFilename = "leaderboard.json";
        public const string GameSettingsFilename = "gamesettings.cfg";
    
        private static readonly string AppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        private static readonly string DataPath = Application.dataPath;
        public static readonly string ExternalDataPath = $"{DataPath}/{ExternalDataName}/";
        
    }
}