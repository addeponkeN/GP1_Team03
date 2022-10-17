using System.IO;

namespace Util
{
    public class FileLoader<T> where T : new()
    {
        protected T File;

        protected string Filename;
        protected string FolderPath;

        private bool _loaded;

        public FileLoader(string filename)
            : this(filename, "") { }

        public FileLoader(string filename, string folderPath)
        {
            Filename = filename;
            FolderPath = folderPath;
            _loaded = false;
        }

        public T GetFile()
        {
            if(!_loaded) {
                Load();
            }

            return File;
        }

        protected string GetFolderPath()
            => FolderPath;

        protected string GetFullPath()
            => $"{FolderPath}/{Filename}";

        public void Save()
        {
            JsonHelper.Save(GetFullPath(), File);
        }

        public void Load()
        {
            if(_loaded) return;

            _loaded = true;
            
            //  checks if directory exists, if not - creates new directory
            Directory.CreateDirectory(GetFolderPath());

            string fullPath = GetFullPath();

            //  if no file could be found - create new & save
            if(!System.IO.File.Exists(fullPath))
            {
                CreateNew();
            }
            else
            {
                //  try load file, create new if failed
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