using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using WorldBuilder.Data;

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
            Maps = EditorUtility.LoadAssets<Map>().ToList();
            MapsChanged?.Invoke();
        }

        public Map CreateMap(string mapName)
        {
            string mapAssetName = $"map_{mapName.ToLowerInvariant().Replace(' ', '_')}";
            
            CreateMapScene(mapAssetName);
            Map map = CreateMapAsset(mapName, mapAssetName);
            Maps.Add(map);

            AssetDatabase.Refresh();
            UnityEditor.EditorUtility.SetDirty(map);
            
            MapsChanged?.Invoke();

            return map;
        }

        private Map CreateMapAsset(string mapName, string mapNameSnakeCase)
        {
            Map map = CreateInstance<Map>();
            map.Name = mapName;
            map.SceneName = mapNameSnakeCase;
            AssetDatabase.CreateAsset(map, $"{MapsDirectory}/{mapNameSnakeCase}.asset");
            return map;
        }

        private void CreateMapScene(string mapNameSnakeCase)
        {
            Scene mapScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            mapScene.name = mapNameSnakeCase;
            EditorSceneManager.SaveScene(mapScene, ScenesDirectory + $"/{mapNameSnakeCase}.unity");
            EditorBuildSettings.scenes = EditorBuildSettings.scenes
                .Append(new EditorBuildSettingsScene(mapScene.path, true))
                .ToArray();
        }

        public void DeleteMap(Map map)
        {
            if (Maps.Contains(map))
                Maps.Remove(map);

            DeleteMapScene(map);
            AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(map));
            AssetDatabase.Refresh();
            
            MapsChanged?.Invoke();
        }

        private void DeleteMapScene(Map map)
        {
            SceneAsset mapScene = GetMapScene(map);

            if (mapScene != null)
            {
                EditorBuildSettings.scenes = EditorBuildSettings.scenes
                    .Where(scene => scene.path != AssetDatabase.GetAssetPath(mapScene))
                    .ToArray();

                AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(mapScene));
            }
        }

        private SceneAsset GetMapScene(Map map)
        {
            SceneAsset[] scenes = EditorUtility.LoadAssets<SceneAsset>();
            
            foreach (SceneAsset sceneAsset in scenes)
            {
                if (string.Equals(sceneAsset.name, map.SceneName))
                    return sceneAsset;
            }
            
            return default;
        }
    }
}