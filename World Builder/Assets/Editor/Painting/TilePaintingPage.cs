using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace WorldBuilder.Painting
{
    public class TilePaintingPage : IWorldBuilderPage
    {
        private const int ITEM_WIDTH = 100;

        private Vector2 _scroll;
        private int _selectedPaletteIndex;
        private int _selectedBrushIndex;
        private World _world;
        private Palette[] _palettes = Array.Empty<Palette>();
        private GUIContent[] _paletteContentIcons = Array.Empty<GUIContent>();

        public string Name => "Tiles";
        private bool IsInitialized => _world != null && _palettes.Length > 0;

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

        private void DrawPalette()
        {
            _scroll = EditorGUILayout.BeginScrollView(_scroll);
            DrawPaletteContent(_paletteContentIcons, ref _selectedBrushIndex);
            EditorGUILayout.EndScrollView();
        }

        private void OnSceneGUI(SceneView sceneView) { }

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
    }
}