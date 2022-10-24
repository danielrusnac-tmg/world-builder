using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace WorldBuilder
{
    public class MapManager : ScriptableSingleton<MapManager>
    {
        private const string DEFAULT_DIRECTORY = "Assets";

        public event Action MapsChanged;
        public DefaultAsset MapsFolder;
        public DefaultAsset ScenesFolder;
        public List<Map> Maps;

        private int _selectedMapIndex;

        public Map SelectedMap => Maps.Count > 0 ? Maps[SelectedMapIndex] : null;
        private string MapsDirectory => MapsFolder == null 
            ? DEFAULT_DIRECTORY 
            : AssetDatabase.GetAssetPath(MapsFolder);

        private string ScenesDirectory => ScenesFolder == null 
            ? DEFAULT_DIRECTORY 
            : AssetDatabase.GetAssetPath(ScenesFolder);

        public int SelectedMapIndex
        {
            get => Mathf.Clamp(_selectedMapIndex, 0, Maps.Count - 1);
            set => _selectedMapIndex = value;
        }

        public void SearchForMapsInProject()
        {
            Maps = WorldBuilderUtility.LoadAssets<Map>().ToList();
            MapsChanged?.Invoke();
        }

        public Map CreateMap(string mapName)
        {
            string mapNameSnakeCase = $"map_{mapName.ToLowerInvariant().Replace(' ', '_')}";
            
            Scene mapScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            mapScene.name = mapNameSnakeCase;
            EditorSceneManager.SaveScene(mapScene, ScenesDirectory + $"/{mapNameSnakeCase}.unity");
            EditorBuildSettings.scenes = EditorBuildSettings.scenes
                .Append(new EditorBuildSettingsScene(mapScene.path, true))
                .ToArray();

            Map map = CreateInstance<Map>();
            map.Name = mapName;
            map.SceneName = mapNameSnakeCase;
            AssetDatabase.CreateAsset(map, $"{MapsDirectory}/{mapNameSnakeCase}.asset");
        
            Maps.Add(map);

            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
            EditorUtility.SetDirty(map);
            MapsChanged?.Invoke();

            return map;
        }

        public void DeleteMap(Map map)
        {
            if (Maps.Contains(map))
                Maps.Remove(map);

            Scene mapScene = GetMapScene(map);

            if (mapScene.IsValid())
            {
                EditorBuildSettings.scenes = EditorBuildSettings.scenes
                    .Where(scene => scene.path != mapScene.path)
                    .ToArray();
                AssetDatabase.DeleteAsset(mapScene.path);
            }

            AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(map));
            AssetDatabase.Refresh();
            MapsChanged?.Invoke();
        }

        private Scene GetMapScene(Map map)
        {
            for (var i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                Scene scene = SceneManager.GetSceneByBuildIndex(i);

                if (scene.name == map.SceneName)
                    return scene;
            }

            return default;
        }
    }
}