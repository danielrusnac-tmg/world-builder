using System;
using System.Text;
using Grids;
using UnityEditor;
using UnityEngine;

namespace WorldBuilder
{
    [Serializable]
    public class Heightmap : FlatGrid<byte>
    {
        public Heightmap(int width, int height) : base(width, height) { }
    }

    [Serializable]
    public class Terrain : FlatGrid<byte>
    {
        public Terrain(int width, int height) : base(width, height) { }
    }

    [Serializable]
    public class WorldData
    {
        public Heightmap Heightmap;
        public Terrain Terrain;
        public FlatGridLayout Layout;

        public WorldData(int width, int height, FlatGridLayout layout)
        {
            Heightmap = new Heightmap(width, height);
            Terrain = new Terrain(width, height);
            Layout = layout;
        }
    }

    public class World : MonoBehaviour
    {
        [SerializeField] private WorldData m_data = new WorldData(10, 10, FlatGridLayout.One);
        
        private StringBuilder m_builder = new StringBuilder();

        private void OnDrawGizmos()
        {
            DrawGridGizmo(m_data.Heightmap);
        }

        [ContextMenu(nameof(LogWorldData))]
        private void LogWorldData()
        {
            Debug.Log(JsonUtility.ToJson(m_data, true));
        }

        private void DrawGridGizmo<T>(FlatGrid<T> grid)
        {
#if UNITY_EDITOR
            for (int x = 0; x < grid.Width; x++)
            {
                for (int y = 0; y < grid.Height; y++)
                {
                    m_builder.Clear();
                    m_builder.AppendLine($"({x}, {y})");
                    m_builder.AppendLine(grid.Get(x, y).ToString());

                    Vector3 cellPosition = m_data.Layout.WorldPosition(x, y);
                    Handles.Label(cellPosition, m_builder.ToString());
                    Gizmos.DrawWireCube(cellPosition, new Vector3( m_data.Layout.CellSize.x, 0f,  m_data.Layout.CellSize.y));
                }
            }
#endif
        }
    }
}