using UnityEditor;

namespace Autotiler
{
    [CustomEditor(typeof(Prototype))]
    public class PrototypeEditor : Editor
    {
        private Prototype _prototype;

        private void OnEnable()
        {
            _prototype = (Prototype)target;
            PrototypeGizmoDrawer.LoadTileTypes();
        }

        private void OnSceneGUI()
        {
            PrototypeGizmoDrawer.OnSceneGUI(_prototype);
        }
    }
}