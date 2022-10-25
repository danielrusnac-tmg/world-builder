using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using WorldBuilder.Data;

namespace WorldBuilder.Rendering
{
    public class WorldRendererGridByte : WorldRenderer<WorldGridByte>
    {
        [SerializeField] private int _minValue = 100;
        [SerializeField] private GameObject _prefab;
        [SerializeField] [HideInInspector] private List<GameObject> _prefabInstances = new List<GameObject>();

        private bool IsInitialized => _prefab != null && IsDataLayerValid;

        [ContextMenu(nameof(RegenerateAll))]
        public void RegenerateAll()
        {
            DestroyAll();
            GenerateAll();
        }

        [ContextMenu(nameof(DestroyAll))]
        public void DestroyAll()
        {
            foreach (GameObject instance in _prefabInstances)
                DestroyInstance(instance);

            _prefabInstances.Clear();
        }

        protected override void OnDataLayerFound(WorldGridByte dataLayer)
        {
            dataLayer.CellChanged += OnCellChanged;
        }

        protected override void OnDataLayerLost()
        {
            if (DataLayer != null)
                DataLayer.CellChanged -= OnCellChanged;
        }

        private void OnCellChanged(int arg1, int arg2, int arg3)
        {
            RegenerateAll();
        }

        private void GenerateAll()
        {
            if (!IsInitialized)
                return;

            for (int x = 0; x < DataLayer.Width; x++)
            {
                for (int y = 0; y < DataLayer.Height; y++)
                {
                    for (int z = 0; z < DataLayer.Length; z++)
                    {
                        if (DataLayer.Get(x, y, z) > _minValue)
                            continue;

                        _prefabInstances.Add(CreatePrefab(Layout.WorldPosition(x, y, z)));
                    }
                }
            }
        }

        private void DestroyInstance(GameObject instance)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                DestroyImmediate(instance);
                return;
            }
#endif

            Destroy(instance);
        }

        private GameObject CreatePrefab(Vector3 worldPosition)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                GameObject instance = PrefabUtility.InstantiatePrefab(_prefab, transform) as GameObject;
                instance.transform.position = worldPosition;
                return instance;
            }
#endif

            return Instantiate(_prefab, worldPosition, Quaternion.identity, transform);
        }
    }
}