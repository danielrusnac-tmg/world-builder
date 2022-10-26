using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Autotiler
{
    [CreateAssetMenu(menuName = CreatePath.ROOT + "Tileset", fileName = "tileset_")]
    public class Tileset : ScriptableObject
    {
        [SerializeField] private Vector3 _scale = Vector3.one;
        [SerializeField] private GameObject[] _source;
        [SerializeField] private Tile[] _tiles;

        public Vector3 Scale => _scale;

        public bool TryGetTile(int id, out Tile tile)
        {
            foreach (Tile marchingSquareTile in _tiles)
            {
                if (marchingSquareTile.ID == id)
                {
                    tile = marchingSquareTile;
                    return true;
                }
            }

            tile = null;
            return false;
        }
        
        private void OnValidate()
        {
            UpdateTiles();
        }

        [ContextMenu(nameof(UpdateTiles))]
        private void UpdateTiles()
        {
            Prototype[] prototypes = _source?
                .Where(go => go != null)
                .SelectMany(go => go.GetComponentsInChildren<Prototype>())
                .ToArray() ?? Array.Empty<Prototype>();

            _tiles = TileUtility.GenerateTiles(prototypes);
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }
    }
}