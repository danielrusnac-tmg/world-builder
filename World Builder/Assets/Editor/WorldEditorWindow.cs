using UnityEditor;
using UnityEngine;

namespace WorldBuilder
{
    public class WorldEditorWindow : EditorWindow
    {
        [MenuItem("World Builder/Editor")]
        private static void ShowWindow()
        {
            WorldEditorWindow window = GetWindow<WorldEditorWindow>();
            window.titleContent = new GUIContent("World Builder");
            window.Show();
        }

        private void OnGUI()
        {
            
        }
    }
}