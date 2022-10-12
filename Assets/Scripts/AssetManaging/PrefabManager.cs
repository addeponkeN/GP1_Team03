using System.Collections.Generic;
using UnityEngine;

namespace AssetManaging
{
    public static partial class PrefabManager
    {
        private static Dictionary<string, GameObject> _prefabs;
        private static List<GameObject> _prefabList;

        private static bool _loaded = false;

        /// <summary>
        /// Get a prefab by name
        /// </summary>
        /// <param name="name">Name of prefab</param>
        /// <returns>Prefab</returns>
        public static GameObject GetPrefab(string name)
        {
            Load();
            return _prefabs[name];
        }

        public static bool TryGetPrefab(string name, out GameObject prefab)
        {
            Load();
            return _prefabs.TryGetValue(name, out prefab);
        }

        public static void Load()
        {
            if(_loaded)
                return;
                
            _loaded = true;

            _prefabs = new Dictionary<string, GameObject>();
            _prefabList = new List<GameObject>();

            var prefabs = Resources.LoadAll<GameObject>("Prefabs/");
            for(int i = 0; i < prefabs.Length; i++)
            {
                var pf = prefabs[i];
                _prefabs.Add(pf.name, pf);
                _prefabList.Add(pf);
            }

            Debug.Log($"Loaded {_prefabs.Count} prefabs.");
        }

        /// <summary>
        /// TEMPORARY
        /// Gets all prefabs at specified path
        /// </summary>
        /// <param name="prefabFolder">Path to prefabs</param>
        /// <returns>Array of all prefabs in 'prefabFolder'</returns>
        public static GameObject[] GetPrefabs(string prefabFolder) //  TEMPORARY
            => Resources.LoadAll<GameObject>($"Prefabs/{prefabFolder}");
    }

   
}