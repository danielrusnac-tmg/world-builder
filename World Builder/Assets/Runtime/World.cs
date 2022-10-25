using System;
using UnityEngine;
using WorldBuilder.Data;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif

namespace WorldBuilder
{
    [ExecuteAlways]
    public class World : MonoBehaviour
    {
        public event Action Changed;
        
        [SerializeField] private Vector3Int _newSize = Vector3Int.one;
        [SerializeField] private WorldData _data = new WorldData(1, 1, 1);
        [SerializeField] private WorldLayout _layout = WorldLayout.One;

        public WorldData Data => _data;
        public WorldLayout Layout => _layout;

        private void OnEnable()
        {
            _data.Changed += OnDataChanged;
        }

        private void OnDisable()
        {
            _data.Changed -= OnDataChanged;
        }

        [ContextMenu(nameof(Resize))]
        public void Resize()
        {
            _data.Resize(_newSize.x, _newSize.y, _newSize.z);
        }

        private void OnDrawGizmos()
        {
            Bounds bounds = Layout.CalculateBounds(_data.Width, _data.Height, _data.Length);
            Gizmos.color = new Color(1f, 0.3f, 0f, 0.16f);
            Gizmos.DrawWireCube(bounds.center, bounds.size);
        }

        private void OnDataChanged()
        {
#if UNITY_EDITOR
            EditorSceneManager.MarkSceneDirty(gameObject.scene);
#endif
            
            Changed?.Invoke();
        }
    }
}