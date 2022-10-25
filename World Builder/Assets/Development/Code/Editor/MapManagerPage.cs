using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace WorldBuilder
{
    public class MapManagerPage : IWorldBuilderPage
    {
        private GUIContent[] _mapsGUi;

        private MapManager MapManager => MapManager.instance;

        public string Name => "Maps";

        public void Show()
        {
            MapManager.SearchForMapsInProject();
            MapManager.MapsChanged += OnMapsChanged;
            OnMapsChanged();
        }

        public void Hide()
        {
            MapManager.MapsChanged -= OnMapsChanged;
        }

        public void OnGUI()
        {
            GUILayout.BeginHorizontal(GUILayout.ExpandHeight(true));
                GUILayout.BeginVertical("box", GUILayout.Width(200), GUILayout.ExpandHeight(true));
                    DrawMapList();
                    DrawCreateButton();
                GUILayout.EndVertical();
                GUILayout.BeginVertical("box", GUILayout.ExpandHeight(true));
                    DrawSelectedMap();
                GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }

        private void DrawCreateButton()
        {
            if (GUILayout.Button("Create Map"))
            {
                MapCreationWindow window = ScriptableObject.CreateInstance<MapCreationWindow>();
                window.ShowModal();
                GUIUtility.ExitGUI();
            }
        }

        private void DrawMapList()
        {
            if (MapManager.Maps.Count == 0)
            {
                EditorGUILayout.HelpBox("No maps found in the project.", MessageType.Info);
            }
            else
            {
                MapManager.SelectedMapIndex = GUILayout.SelectionGrid(MapManager.SelectedMapIndex, _mapsGUi, 1);
            }
        }

        private void OnMapsChanged()
        {
            _mapsGUi = CreateMapsGUI();
        }

        private GUIContent[] CreateMapsGUI()
        {
            if (MapManager.Maps.Count == 0)
                return Array.Empty<GUIContent>();

            return MapManager.Maps
                .Select(map => new GUIContent(map.Name))
                .ToArray();
        }

        private void DrawSelectedMap()
        {
            if (MapManager.Maps.Count == 0)
            {
                EditorGUILayout.HelpBox("No map selected.", MessageType.Info);
            }
            else
            {
                GUILayout.Label(MapManager.SelectedMap.Name);

                if (GUILayout.Button("Delete"))
                {
                    MapManager.DeleteMap(MapManager.SelectedMap);
                }    
            }
        }
    }
}