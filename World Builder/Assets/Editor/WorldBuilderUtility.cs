using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace WorldBuilder
{
    public static class WorldBuilderUtility
    {
        public static bool IsValidateDirectory(string directory)
        {
            if (!AssetDatabase.IsValidFolder(directory))
            {
                string[] folders = directory.Split('/');

                if (folders.Length <= 1)
                    return false;

                string currentDirectory = folders[0];

                for (int i = 1; i < folders.Length; i++)
                {
                    if (!AssetDatabase.IsValidFolder(currentDirectory + $"/{folders[i]}"))
                    {
                        AssetDatabase.CreateFolder(currentDirectory, folders[i]);
                    }

                    currentDirectory += $"/{folders[i]}";
                }

                AssetDatabase.Refresh();
            }

            return true;
        }
        
        public static T[] LoadAssets<T>() where T : Object
        {
            return LoadAssets<T>("Assets");
        }

        public static T[] LoadAssets<T>(params string[] paths) where T : Object
        {
            string[] guids = AssetDatabase.FindAssets($"t:{typeof(T).Name}", paths);

            return guids.Select(guid => AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(guid)))
                .ToArray();
        }

        public static T[] LoadPrefabs<T>(params string[] paths) where T : Object
        {
            List<T> result = new List<T>();
            GameObject[] assets = LoadAssets<GameObject>(paths);

            foreach (GameObject asset in assets)
            {
                if (asset.TryGetComponent(out T tileMesh))
                {
                    result.Add(tileMesh);
                }
            }

            return result.ToArray();
        }
    }
}