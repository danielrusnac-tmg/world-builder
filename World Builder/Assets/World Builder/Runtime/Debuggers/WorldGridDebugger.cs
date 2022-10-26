using UnityEngine;
using WorldBuilder.Data;

namespace WorldBuilder.Debuggers
{
    public abstract class WorldGridDebugger<T> : DataLayerDebugger<WorldGrid<T>>
    {
        protected override void DrawDataLayerGizmo(World world, WorldGrid<T> dataLayer)
        {
            for (int x = 0; x < dataLayer.Width; x++)
            {
                for (int y = 0; y < dataLayer.Height; y++)
                {
                    for (int z = 0; z < dataLayer.Length; z++)
                    {
                        Gizmos.color = GetColor(dataLayer.Get(x, y, z));
                        Gizmos.DrawCube(world.Layout.WorldPosition(x, y, z), world.Layout.CellSize * 0.95f);
                    }
                }
            }
        }

        protected abstract Color GetColor(T value);
    }
}