using System;
using UnityEngine;

namespace Autotiler
{
    [Serializable]
    public class Corner
    {
        [SerializeField] public TileType _tile;

        public byte TerrainID => _tile == null ? byte.MinValue : _tile.ID;
        public Color DebugColor => _tile == null ? Color.magenta : _tile.DebugColor;
    }
}