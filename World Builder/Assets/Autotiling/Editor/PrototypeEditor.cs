using UnityEditor;

namespace Autotiling
{
    [CustomEditor(typeof(Prototype))]
    public class PrototypeEditor : UnityEditor.Editor
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