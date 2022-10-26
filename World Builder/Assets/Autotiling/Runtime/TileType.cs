using UnityEngine;

namespace Autotiling
{
    [CreateAssetMenu(menuName = CreatePath.ROOT + "Tile Type", fileName = "tile_")]
    public class TileType : ScriptableObject
    {
        [SerializeField] private byte _id;
        [SerializeField] private Color _debugColor;
        
        public byte ID => _id;
        public Color DebugColor => _debugColor;
    }
}