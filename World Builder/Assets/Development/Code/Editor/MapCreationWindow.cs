using UnityEditor;
using UnityEditor.SceneTemplate;
using UnityEngine;

namespace WorldBuilder
{
    public class MapCreationWindow : EditorWindow
    {
        private string _mapName;

        private void OnGUI()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Name:");
            _mapName = GUILayout.TextField(_mapName);
            GUILayout.EndHorizontal();

            MapManager.instance.MapsFolder = EditorGUILayout.ObjectField(MapManager.instance.MapsFolder, 
                typeof(DefaultAsset), false) as DefaultAsset;
            
            MapManager.instance.ScenesFolder = EditorGUILayout.ObjectField(MapManager.instance.ScenesFolder, 
                typeof(DefaultAsset), false) as DefaultAsset;

            if (GUILayout.Button("Create"))
            {
                if (string.IsNullOrWhiteSpace(_mapName))
                {
                    EditorUtility.DisplayDialog("Failed to Create Map", "Invalid name for a map!", "Ok");
                    return;
                }

                Close();
                MapManager.instance.CreateMap(_mapName);
            }
        }
    }
}