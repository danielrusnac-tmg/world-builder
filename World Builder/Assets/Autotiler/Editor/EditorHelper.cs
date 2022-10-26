using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Autotiler
{
    internal class EditorHelper
    {
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
    }
}