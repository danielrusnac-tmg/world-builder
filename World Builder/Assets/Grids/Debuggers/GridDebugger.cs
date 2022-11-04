using System.Text;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Grids.Debuggers
{
    public class GridDebugger<T> : MonoBehaviour
    {
        [SerializeField, Min(0)] private int m_width = 10;
        [SerializeField, Min(0)] private int m_height = 10;
        [SerializeField] private FlatGridLayout m_layout = FlatGridLayout.One;
        [SerializeField] [HideInInspector] private FlatGrid<T> m_grid;

        private StringBuilder m_builder = new StringBuilder();

        private void Reset()
        {
            RegenerateGrid();
        }

        private void OnValidate()
        {
            RegenerateGrid();
        }

        private void RegenerateGrid()
        {
            m_grid = new FlatGrid<T>(m_width, m_height);
        }

        public void SetGrid(FlatGrid<T> grid)
        {
            m_grid = grid;
        }

        private void OnDrawGizmosSelected()
        {
            DrawGridGizmo(m_grid);
        }

        private void DrawGridGizmo(FlatGrid<T> grid)
        {
#if UNITY_EDITOR
            for (int x = 0; x < grid.Width; x++)
            {
                for (int y = 0; y < grid.Height; y++)
                {
                    m_builder.Clear();
                    m_builder.AppendLine($"({x}, {y})");
                    m_builder.AppendLine(grid.Get(x, y).ToString());

                    Vector3 cellPosition = m_layout.WorldPosition(x, y);
                    Handles.Label(cellPosition, m_builder.ToString());
                    Gizmos.DrawWireCube(cellPosition, new Vector3(m_layout.CellSize.x, 0f, m_layout.CellSize.y));
                }
            }
#endif
        }
    }
}