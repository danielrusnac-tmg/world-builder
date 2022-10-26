using UnityEngine;

namespace WorldBuilder.Autotiling
{
    [CreateAssetMenu(menuName = CreatePath.AUTOTILING + "Tile Type", fileName = "tile_")]
    public class TileType : ScriptableObject
    {
        [SerializeField] private byte _id;
        [SerializeField] private Color _debugColor;
        
        public byte ID => _id;
        public Color DebugColor => _debugColor;
    }
}