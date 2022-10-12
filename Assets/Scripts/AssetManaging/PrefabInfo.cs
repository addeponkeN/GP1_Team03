using UnityEngine;

namespace AssetManaging
{
    /// <summary>
    /// Contains file info of a prefab asset 
    /// </summary>
    public class PrefabInfo
    {
        /// <summary>
        /// Path to the prefab asset.
        /// Starting from /Resources/Prefabs/{path}
        /// </summary>
        public string FullPath { get; private set; }
        
        /// <summary>
        /// The name of the prefab asset.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The prefab
        /// </summary>
        public GameObject Prefab { get; private set; }

        public PrefabInfo(string fullPath, GameObject prefab)
        {
            FullPath = fullPath;
            Prefab = prefab;
            
            int indexOfLastSlash = fullPath.LastIndexOf('/');
            Name = FullPath.Substring(indexOfLastSlash, FullPath.Length - indexOfLastSlash);
        }
    }
}