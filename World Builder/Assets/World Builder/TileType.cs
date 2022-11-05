using UnityEngine;

namespace WorldBuilder
{
    [CreateAssetMenu(menuName = "World Builder/Tile Type", fileName = "tile_")]
    public class TileType : ScriptableObject
    {
        [SerializeField] private byte m_id;

        public byte ID => m_id;
    }
}