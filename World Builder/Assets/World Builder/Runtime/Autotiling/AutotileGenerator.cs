using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace WorldBuilder.Autotiling
{
    public class AutotileGenerator : MonoBehaviour, ISerializationCallbackReceiver
    {
        [SerializeField] private Tileset _tileset;
        [SerializeField] private Vector3Int[] _coordinates = Array.Empty<Vector3Int>();
        [SerializeField] private GameObject[] _instances = Array.Empty<GameObject>();

        public IAutotileMap Map;

        private Dictionary<Vector3Int, GameObject> _instanceByCell = new Dictionary<Vector3Int, GameObject>();

        public void RegenerateDirty(HashSet<Vector3Int> dirtyCoordinates)
        {
            HashSet<Vector3Int> dirtyWithNeighbors = FindDirtyCells(dirtyCoordinates);

            foreach (Vector3Int c in dirtyWithNeighbors)
                RegenerateAt(c.x, c.y, c.z);
        }

        private HashSet<Vector3Int> FindDirtyCells(HashSet<Vector3Int> dirtyCoordinates)
        {
            HashSet<Vector3Int> tilesToRefresh = new HashSet<Vector3Int>();

            foreach (Vector3Int tile in dirtyCoordinates)
            {
                foreach (Vector3Int o in TileUtility.TILE_NEIGHBORS)
                {
                    Vector3Int offsetTile = tile + o;

                    if (offsetTile.x >= Map.Width - 1 ||
                        offsetTile.y >= Map.Length - 1 ||
                        offsetTile.x < 0 ||
                        offsetTile.y < 0)
                        continue;

                    tilesToRefresh.Add(offsetTile);
                }
            }

            return tilesToRefresh;
        }

        [ContextMenu(nameof(RegenerateAll))]
        public void RegenerateAll()
        {
            DestroyAll();
            GenerateAll();
        }

        [ContextMenu(nameof(DestroyAll))]
        public void DestroyAll()
        {
            foreach (GameObject instance in _instanceByCell.Values)
            {
                if (instance == null)
                    continue;

                DestroyInstance(instance);
            }

            _coordinates = Array.Empty<Vector3Int>();
            _instances = Array.Empty<GameObject>();
            _instanceByCell.Clear();
        }

        private void GenerateAll()
        {
            for (int x = 0; x < Map.Width; x++)
            {
                for (int y = 0; y < Map.Height; y++)
                {
                    for (int z = 0; z < Map.Length; z++)
                    {
                        RegenerateAt(x, y, z);
                    }
                }
            }
        }

        private void RegenerateAt(int x, int y, int z)
        {
            Vector3 offset = Map.CellSize * 0.5f;
            offset.y = 0f;
            Vector3Int cellIndex = new Vector3Int(x, y, z);

            if (_instanceByCell.ContainsKey(cellIndex))
            {
                DestroyInstance(_instanceByCell[cellIndex]);
                _instanceByCell.Remove(cellIndex);
            }

            int tileIndex = ComputeIndex(x, y, z);

            if (_tileset == null || !_tileset.TryGetTile(tileIndex, out Tile tile))
                return;

            Vector3 position = Map.WorldPosition(x, y, z) + offset;
            Quaternion rotation = Quaternion.AngleAxis(tile.Rotation * -90, Vector3.up);

            GameObject instance = CreateTileInstance(tile);
            instance.hideFlags = HideFlags.HideAndDontSave;
            Transform t = instance.transform;
            t.SetPositionAndRotation(position, rotation);
            t.localScale = Vector3.Scale(tile.Scale, _tileset.Scale);
            instance.isStatic = true;

            _instanceByCell.Add(cellIndex, instance);
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
            return Map.GetValue(x, y, z);
        }

        public void OnBeforeSerialize()
        {
            _coordinates = _instanceByCell.Keys.ToArray();
            _instances = _instanceByCell.Values.ToArray();
        }

        public void OnAfterDeserialize()
        {
            _instanceByCell = new Dictionary<Vector3Int, GameObject>();

            if (_coordinates.Length == 0 || _coordinates.Length != _instances.Length)
                return;

            int count = _coordinates.Length;

            for (int i = 0; i < count; i++)
                _instanceByCell.Add(_coordinates[i], _instances[i]);
        }
    }
}