using System.Linq;
using UnityEditor;
using UnityEngine;

namespace WorldBuilder
{
    public class WorldBuilderWindow : EditorWindow
    {
        private int _selectedPageIndex;
        private GUIContent[] _pagesGUI;
        private IWorldBuilderPage[] _pages;

        private IWorldBuilderPage SelectedPage => _pages[_selectedPageIndex];

        [MenuItem("World Builder/Editor")]
        private static void ShowWindow()
        {
            WorldBuilderWindow window = GetWindow<WorldBuilderWindow>();
            window.titleContent = new GUIContent("World Builder");
            window.Show();
        }

        private void OnEnable()
        {
            _pages = new IWorldBuilderPage[]
            {
                new MapManagerPage()
            };

            _pagesGUI = _pages.Select(page => new GUIContent(page.Name)).ToArray();
            SelectedPage.Show();
        }

        private void OnDisable()
        {
            SelectedPage.Hide();
        }

        private void OnGUI()
        {
            int newSelectedIndex = GUILayout.SelectionGrid(_selectedPageIndex, _pagesGUI, _pages.Length);

            if (newSelectedIndex != _selectedPageIndex)
            {
                SelectedPage.Hide();
                _selectedPageIndex = newSelectedIndex;
                SelectedPage.Show();
            }
            
            SelectedPage.OnGUI();
        }
    }
}