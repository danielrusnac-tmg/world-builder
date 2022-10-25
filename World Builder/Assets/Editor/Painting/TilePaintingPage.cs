using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using WorldBuilder.Data;

namespace WorldBuilder.Painting
{
    public class TilePaintingPage : IWorldBuilderPage
    {
        private const int ITEM_WIDTH = 100;

        private bool _isDragging;
        private int _selectedPaletteIndex;
        private int _selectedBrushIndex;
        protected Vector3Int StartDragTarget;
        private Vector2 _scroll;
        private LayerMask _mask;
        private World _world;
        private HashSet<Vector3Int> _draggingSet = new HashSet<Vector3Int>();
        private Palette[] _palettes = Array.Empty<Palette>();
        private GUIContent[] _paletteContentIcons = Array.Empty<GUIContent>();

        public string Name => "Tiles";
        private bool IsInitialized => _world != null && _palettes.Length > 0 && SelectedPalette.Brushes.Length > 0;

        private Palette SelectedPalette => _palettes[_selectedPaletteIndex];

        private Brush SelectedBrush => SelectedPalette.Brushes[_selectedBrushIndex];

        public void Show()
        {
            _world = SearchWorldInCurrentScene();
            _palettes = SearchPalettesInProject();
            RefreshPaletteContent();

            SceneView.duringSceneGui += OnSceneGUI;
        }

        public void Hide()
        {
            SceneView.duringSceneGui -= OnSceneGUI;
        }

        public void OnGUI()
        {
            if (!IsInitialized)
            {
                EditorGUILayout.HelpBox("No world object ion the scene.", MessageType.Info);
                return;
            }

            int newIndex = EditorGUILayout.Popup("Palette", _selectedPaletteIndex,
                _palettes.Select(palette => palette.Name).ToArray());

            if (newIndex != _selectedPaletteIndex)
            {
                _selectedPaletteIndex = newIndex;
                RefreshPaletteContent();
            }
            
            DrawPalette();
        }

        private void OnSceneGUI(SceneView sceneView)
        {
            SelectCellInWorld(_world.Layout, _mask, out Vector3 point, out Vector3 normal, out Vector3Int coordinate);
            DrawCell(coordinate, _world.Layout);
            
            Event e = Event.current;

            if (e.type == EventType.Layout)
            {
                int controlId = GUIUtility.GetControlID(GetHashCode(), FocusType.Passive);
                HandleUtility.AddDefaultControl(controlId);
            }

            if (e.type == EventType.MouseUp)
            {
                _isDragging = false;
                _draggingSet.Clear();
                return;
            }

            if (e.type != EventType.MouseDown && e.type != EventType.MouseDrag || e.button != 0 || e.alt || e.shift)
                return;

            if (e.type == EventType.MouseDown)
            {
                _isDragging = true;
                StartDragTarget = coordinate;
            }

            Action<Vector3Int> action = GetAction(e);

            if (_isDragging && !_draggingSet.Contains(coordinate))
            {
                action(coordinate);
                _draggingSet.Add(coordinate);
            }
        }

        private Action<Vector3Int> GetAction(Event e)
        {
            return target => SelectedBrush.Paint(new PaintData
            {
                Coordinate = target,
                IsErase = e.control,
                Data = _world.Data
            });
        }

        private void DrawPalette()
        {
            _scroll = EditorGUILayout.BeginScrollView(_scroll);
            DrawPaletteContent(_paletteContentIcons, ref _selectedBrushIndex);
            EditorGUILayout.EndScrollView();
        }

        private void RefreshPaletteContent()
        {
            _paletteContentIcons = SelectedPalette != null
                ? SelectedPalette.Brushes.Select(brush => brush.Preview).ToArray()
                : null;
        }

        private static World SearchWorldInCurrentScene()
        {
            GameObject worldHolder = SceneManager
                .GetActiveScene()
                .GetRootGameObjects()
                .FirstOrDefault(go => go.name.Equals(Constants.WORLD_HOLDER_NAME));

            return worldHolder != null
                ? worldHolder.GetComponent<World>()
                : null;
        }

        private Palette[] SearchPalettesInProject()
        {
            return EditorUtility.LoadAssets<Palette>();
        }

        private static bool DrawPaletteContent(GUIContent[] content, ref int selected)
        {
            selected = Mathf.Clamp(selected, 0, content.Length);

            int elementCount = Screen.width / ITEM_WIDTH;
            int verticalCount = Mathf.CeilToInt((float) content.Length / elementCount);
            float height = (float) Screen.width / elementCount * verticalCount;

            int oldSelected = selected;
            selected = GUILayout.SelectionGrid(selected, content, elementCount,
                GUILayout.Width(Screen.width - 7), GUILayout.Height(height));

            return oldSelected != selected;
        }
        
        private static void SelectCellInWorld(WorldLayout layout, LayerMask mask, out Vector3 point, out Vector3 normal, out Vector3Int coordinate)
        {
            Event e = Event.current;
            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            var offset = new Vector3(0f, layout.CellSize.y * 0.5f, 0f);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, mask))
            {
                coordinate = layout.CoordinateAsVector(hit.point - hit.normal * 0.1f);
                point = hit.point;
                normal = hit.normal;
            }
            else
            {
                Plane bottomPlane = new Plane(Vector3.down, new Vector3(0f, layout.CellSize.y * 0.5f, 0f));
                bottomPlane.Raycast(ray, out float enterBot);
                coordinate = layout.CoordinateAsVector(ray.GetPoint(enterBot));
                point = ray.GetPoint(enterBot) + offset;
                normal = Vector3.up;
            }
        }

        private static void DrawCell(Vector3Int coordinate, WorldLayout layout)
        {
            Handles.DrawWireCube(layout.WorldPosition(coordinate), layout.CellSize);
        }
    }
}