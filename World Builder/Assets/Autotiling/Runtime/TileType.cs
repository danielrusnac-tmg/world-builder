using UnityEngine;

namespace Autotiling
{
    [CreateAssetMenu(menuName = CreatePath.ROOT + "Tile Type", fileName = "tile_")]
    public class TileType : ScriptableObject
    {
        [SerializeField] private byte _id;
        [SerializeField] private Color _debugColor;
        [SerializeField] private string _name;
        [SerializeField] private bool _showAsOption = true;

        public byte ID => _id;
        public Color DebugColor => _debugColor;
        public bool ShowAsOption => _showAsOption;
        public string Name => _name;
    }
}