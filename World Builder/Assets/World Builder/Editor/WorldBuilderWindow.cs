using System.Linq;
using UnityEditor;
using UnityEngine;

namespace WorldBuilder
{
    public class WorldBuilderWindow : EditorWindow
    {
        private int _selectedPageIndex;
        private GUIContent[] _pagesGUI;
        private WorldBuilderPage[] _pages;

        private WorldBuilderPage SelectedPage => _pages == null || _pages.Length == 0
            ? null
            : _pages[_selectedPageIndex];

        [MenuItem("World Builder/Open")]
        public static void ShowWindow()
        {
            WorldBuilderWindow window = GetWindow<WorldBuilderWindow>();
            window.titleContent = new GUIContent("World Builder");
            window.SetPages(EditorHelper.LoadAssets<WorldBuilderPage>());
            window.Show();
        }

        private void SetPages(params WorldBuilderPage[] pages)
        {
            HideSelectedPage();
            
            _pages = pages;
            _pagesGUI = _pages.Select(page => new GUIContent(page.Title)).ToArray();
            
            ShowSelectedPage();
        }

        private void OnEnable()
        {
            ShowSelectedPage();
        }

        private void OnDisable()
        {
            HideSelectedPage();
        }

        private void OnGUI()
        {
            if (SelectedPage == null)
            {
                EditorGUILayout.HelpBox("No pages in the project.", MessageType.Info);
                return;
            }
            
            int newSelectedIndex = GUILayout.SelectionGrid(_selectedPageIndex, _pagesGUI, _pages.Length);

            if (newSelectedIndex != _selectedPageIndex)
            {
                HideSelectedPage();
                _selectedPageIndex = newSelectedIndex;
                ShowSelectedPage();
            }

            SelectedPage.OnGUI();
        }
        
        private void HideSelectedPage()
        {
            if (SelectedPage != null)
                SelectedPage.Hide();
        }

        private void ShowSelectedPage()
        {
            if (SelectedPage != null)
                SelectedPage.Show();
        }
    }
}