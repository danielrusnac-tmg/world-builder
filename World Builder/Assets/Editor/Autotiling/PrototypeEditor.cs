using UnityEditor;

namespace WorldBuilder.Autotiling
{
    [CustomEditor(typeof(Prototype))]
    public class PrototypeEditor : Editor
    {
        private Prototype _prototype;

        private void OnEnable()
        {
            _prototype = (Prototype)target;
        }

        private void OnSceneGUI()
        {
            PrototypeGizmoDrawer.OnSceneGUI(_prototype);
        }
    }
}