﻿using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace WorldBuilder.Rendering.Autotiling
{
    [CreateAssetMenu(menuName = "World Builder/Autotiling/Tileset", fileName = "tileset_")]
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

        public Matrix4x4 GetTransformation(Vector3 position, Tile tile)
        {
            return Matrix4x4.TRS(
                position,
                Quaternion.AngleAxis(tile.Rotation * -90, Vector3.up),
                tile.Scale);
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