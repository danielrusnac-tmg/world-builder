using UnityEngine;
using WorldBuilder.Data;

namespace WorldBuilder.Generation
{
    [ExecuteAlways]
    public class GridDebugger : MonoBehaviour
    {
        [SerializeField] private WorldLayer _layer;
        [SerializeField] private World _world;

        private WorldGridByte _dataLayer;

        private WorldLayout Layout => _world.Layout;
        private int Height => _dataLayer.Height;
        private int Width => _dataLayer.Width;
        private int Length => _dataLayer.Length;
        private bool IsDataLayerValid => _dataLayer != null;

        private void OnEnable()
        {
            if (_world == null)
                return;
            
            if (_world.Data.HasDataLayer<WorldGridByte>(_layer))
            {
                _dataLayer = _world.Data.GetDataLayer<WorldGridByte>(_layer);
            }
            else
            {
                _dataLayer = _world.Data.CreateDataLayer<WorldGridByte>(_layer);
            }
        }

        [ContextMenu(nameof(RefreshNoise))]
        private void RefreshNoise()
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    for (int z = 0; z < Length; z++)
                    {
                        _dataLayer.Set((byte)Random.Range(byte.MinValue, byte.MaxValue), x, y, z);
                    }
                }
            }
        }

        private void OnDrawGizmos()
        {
            if (!IsDataLayerValid)
                return;

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    for (int z = 0; z < Length; z++)
                    {
                        Gizmos.color = Color.Lerp(Color.black, Color.white, (float)_dataLayer.Get(x, y, z) / 255);
                        Gizmos.DrawCube(Layout.WorldPosition(x, y, z), Layout.CellSize * 0.95f);
                    }
                }
            }
        }
    }
}