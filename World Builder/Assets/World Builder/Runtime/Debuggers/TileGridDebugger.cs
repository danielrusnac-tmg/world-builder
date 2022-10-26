using UnityEngine;
using WorldBuilder.Autotiling;

namespace WorldBuilder.Debuggers
{
    public class TileGridDebugger : WorldGridDebugger<byte>
    {
        [SerializeField] private TileType[] _tiles;

        protected override Color GetColor(byte value)
        {
            foreach (TileType tile in _tiles)
            {
                if (tile == null)
                    continue;
                
                if (tile.ID == value)
                    return tile.DebugColor;
            }
            
            return Color.magenta;
        }
    }
}