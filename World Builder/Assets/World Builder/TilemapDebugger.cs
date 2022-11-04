using Grids;
using UnityEngine;

namespace WorldBuilder
{
    public class TilemapDebugger : MonoBehaviour
    {
        [SerializeField] private Tilemap m_tilemap = new Tilemap(10, 10, FlatGridLayout.One);

        private void OnDrawGizmos()
        {
            for (int x = 0; x < m_tilemap.Width; x++)
            {
                for (int y = 0; y < m_tilemap.Height; y++)
                {
                    Gizmos.DrawWireCube(m_tilemap.WorldPosition(x, y), m_tilemap.Layout.CellSize);
                }  
            }
        }
    }
}