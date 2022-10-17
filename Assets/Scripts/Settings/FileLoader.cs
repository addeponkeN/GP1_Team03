using System.IO;
using Util;

namespace Settings
{
    public abstract class RootFile
    {
    }

    public class FileLoader<T> where T : RootFile, new()
    {
        protected T File;

        private string _filename;
        private bool _loaded;

        public FileLoader(string filename)
        {
            _filename = filename;
            _loaded = false;
        }

        public T GetFile()
        {
            if(!_loaded) {
                Load();
            }

            return File;
        }

        protected string GetFullPath()
        {
            return $"{PathHelper.ProjectName}/{_filename}";
        }

        public void Save()
        {
            JsonHelper.Save(GetFullPath(), File);
        }

        public void Load()
        {
            //  checks if directory exists, if not - creates new directory
            Directory.CreateDirectory(GetFullPath());

            string fullPath = GetFullPath();

            //  if no settings file could be found - create new & save
            if(!System.IO.File.Exists(fullPath))
            {
                CreateNew();
            }
            else
            {
                if((File = JsonHelper.Load<T>(fullPath)) == null)
                    CreateNew();
            }
        }

        private void CreateNew()
        {
            File = default;
            Save();
        }
    }
}