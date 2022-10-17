using System;
using UnityEngine;

namespace Settings
{
    public static class PathHelper
    {
        public const string PublisherName = "Futuregames";
        public const string ProjectName = "BikeMania";
        public const string ExternalDataName = "Data";
    
        private static readonly string AppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        private static readonly string DataPath = Application.dataPath;
        public static readonly string FilePath = $"{DataPath}/{ExternalDataName}/";
        
    }
}