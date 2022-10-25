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
        
        public static void ShowWindow(params IWorldBuilderPage[] pages)
        {
            WorldBuilderWindow window = GetWindow<WorldBuilderWindow>();
            window.titleContent = new GUIContent("World Builder");
            window.SetPages(pages);
            window.Show();
        }

        public void SetPages(params IWorldBuilderPage[] pages)
        {
            SelectedPage?.Hide();
            _pages = pages;
            _pagesGUI = _pages.Select(page => new GUIContent(page.Name)).ToArray();
            SelectedPage?.Show();
        }
        
        private void OnEnable()
        {
            SelectedPage?.Show();
        }

        private void OnDisable()
        {
            SelectedPage?.Hide();
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