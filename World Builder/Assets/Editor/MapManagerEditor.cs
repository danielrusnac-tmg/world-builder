using UnityEditor;

namespace WorldBuilder
{
    [CustomEditor(typeof(MapManager))]
    public class MapManagerEditor : Editor
    {
        private MapManager _mapManager;

        private void OnEnable()
        {
            _mapManager = (MapManager)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }
}