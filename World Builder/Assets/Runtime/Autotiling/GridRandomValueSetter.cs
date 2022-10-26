using UnityEngine;
using WorldBuilder.Data;

namespace WorldBuilder.Autotiling
{
    public class GridRandomValueSetter : MonoBehaviour
    {
        [SerializeField] private WorldLayer _layer;
        [SerializeField] private World _world;
        
        private WorldGridByte _dataLayer;
        private bool IsDataLayerValid => _dataLayer != null && _world != null;

        protected virtual void OnValidate()
        {
            if (_layer == null || _world == null || !_world.Data.HasDataLayer<WorldGridByte>(_layer))
            {
                _dataLayer = null;
                return;
            }

            _dataLayer = _world.Data.GetDataLayer<WorldGridByte>(_layer);
        }

        [ContextMenu(nameof(SetRandomData))]
        private void SetRandomData()
        {
            if (!IsDataLayerValid)
                return;
            
            for (int x = 0; x < _dataLayer.Width; x++)
            {
                for (int y = 0; y < _dataLayer.Height; y++)
                {
                    for (int z = 0; z < _dataLayer.Length; z++)
                    {
                        _dataLayer.Set(GetRandomValue(), x, y, z);
                    }
                }
            }
        }

        private byte GetRandomValue()
        {
            return (byte)(Random.value > 0.5f ? 1 : 0);
        }
    }
}