using UnityEngine;
using WorldBuilder.Data;

namespace WorldBuilder.Debuggers
{
    public abstract class DataLayerDebugger<T> : MonoBehaviour where T : DataLayer
    {
        [SerializeField] private WorldLayer _layer;
        [SerializeField] private World _world;
        
        private T _dataLayer;
        protected bool IsDataLayerValid => _dataLayer != null;
        
        protected virtual void OnValidate()
        {
            if (_layer == null || _world == null || !_world.Data.HasDataLayer<T>(_layer))
            {
                _dataLayer = null;
                return;
            }

            _dataLayer = _world.Data.GetDataLayer<T>(_layer);
        }
        
        private void OnDrawGizmos()
        {
            if (!IsDataLayerValid)
                return;

            DrawDataLayerGizmo(_world, _dataLayer);
        }

        protected abstract void DrawDataLayerGizmo(World world, T dataLayer);
    }
}