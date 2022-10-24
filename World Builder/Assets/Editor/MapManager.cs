using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace WorldBuilder
{
    public class MapManager : ScriptableSingleton<MapManager>
    {
        public event Action MapsChanged;
        private int _selectedMapIndex;
        public List<Map> Maps;
        
        public Map SelectedMap => Maps.Count > 0 ? Maps[SelectedMapIndex] : null;

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
            Map map = CreateInstance<Map>();
            map.Name = mapName;
            AssetDatabase.CreateAsset(map, $"Assets/map_{mapName.ToLowerInvariant().Replace(' ', '_')}.asset");
            Maps.Add(map);
            AssetDatabase.Refresh();
            MapsChanged?.Invoke();
            
            return map;
        }

        public void DeleteMap(Map map)
        {
            if (Maps.Contains(map))
                Maps.Remove(map);

            AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(map));
            AssetDatabase.Refresh();
            MapsChanged?.Invoke();
        }
    }
}