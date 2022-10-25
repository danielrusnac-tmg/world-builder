using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using WorldBuilder.Data;

namespace WorldBuilder.Rendering.Autotiling
{
    public class WorldRendererAutotiling : WorldRenderer<WorldGridByte>, ISerializationCallbackReceiver
    {
        [Serializable]
        public class TileColumn
        {
            public Vector2Int Tile;
            public List<GameObject> Instances;
        }
        
        [SerializeField] private Tileset _tileset;
        [SerializeField] [HideInInspector] private TileColumn[] _columns = Array.Empty<TileColumn>();
        
        private Dictionary<Vector2Int, List<GameObject>> _instanceByCell =
            new Dictionary<Vector2Int, List<GameObject>>();
        
        [ContextMenu(nameof(RegenerateAll))]
        private void RegenerateAll()
        {
            DestroyAll();
            GenerateAll();
        }
        
        [ContextMenu(nameof(DestroyAll))]
        private void DestroyAll()
        {
            foreach (TileColumn tileColumn in _columns)
            {
                foreach (GameObject instance in tileColumn.Instances)
                {
                    if (instance == null)
                        continue;
                    
                    DestroyInstance(instance);
                }
            }

            _columns = Array.Empty<TileColumn>();
            _instanceByCell.Clear();

#if UNITY_EDITOR
            if (!Application.isPlaying)
                EditorSceneManager.MarkSceneDirty(gameObject.scene);
#endif
        }
        
        private bool IsInitialized => _tileset != null && IsDataLayerValid;
        
        protected override void OnDataLayerFound(WorldGridByte dataLayer)
        {
            dataLayer.CellChanged += OnCellChanged;
        }

        protected override void OnDataLayerLost()
        {
            if (DataLayer != null)
                DataLayer.CellChanged -= OnCellChanged;
        }

        private void OnCellChanged(int x, int y, int z)
        {
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
                        Vector2Int columnIndex = new Vector2Int(x, z);
                        
                        if (!_instanceByCell.ContainsKey(columnIndex))
                            _instanceByCell.Add(columnIndex, new List<GameObject>());
                        
                        int tileIndex = ComputeIndex(x, y, z);
                        
                        if (!_tileset.TryGetTile(tileIndex, out Tile tile))
                            continue;
                        
                        Vector3 position = Layout.WorldPosition(x, y, z);
                        Quaternion rotation = Quaternion.AngleAxis(tile.Rotation * -90, Vector3.up);

                        GameObject instance = CreateTileInstance(tile);

                        Transform t = instance.transform;
                        t.SetPositionAndRotation(position, rotation);
                        t.localScale = Vector3.Scale(tile.Scale, _tileset.Scale);
                        instance.isStatic = true;

                        _instanceByCell[columnIndex].Add(instance);
                    }
                }
            }

#if UNITY_EDITOR
            if (!Application.isPlaying)
                EditorSceneManager.MarkSceneDirty(gameObject.scene);
#endif
        }

        private GameObject CreateTileInstance(Tile tile)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                GameObject instance = PrefabUtility.InstantiatePrefab(tile.Prefab, transform) as GameObject;
                instance.GetComponent<Prototype>().enabled = false;
                return instance;
            }
#endif

            return Instantiate(tile.Prefab, transform);
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

        protected virtual int ComputeIndex(int x, int y, int z)
        {
            return TileUtility.ComputeID(
                GetTerrainID(x, y, z),
                GetTerrainID(x, y, z + 1),
                GetTerrainID(x + 1, y, z + 1),
                GetTerrainID(x + 1, y, z));
        }

        protected virtual byte GetTerrainID(int x, int y, int z)
        {
            return DataLayer.Get(x, y, z);
        }

        public void OnBeforeSerialize()
        {
            _columns = new TileColumn[_instanceByCell.Count];

            int i = 0;
            foreach (Vector2Int key in _instanceByCell.Keys)
            {
                _columns[i] = new TileColumn()
                {
                    Tile = key,
                    Instances = _instanceByCell[key]
                };

                i++;
            }
        }

        public void OnAfterDeserialize()
        {
            _instanceByCell = new Dictionary<Vector2Int, List<GameObject>>();

            if (_columns == null || _columns.Length == 0)
                return;

            int count = _columns.Length;

            for (int i = 0; i < count; i++)
                _instanceByCell.Add(_columns[i].Tile, _columns[i].Instances);
        }
    }
}