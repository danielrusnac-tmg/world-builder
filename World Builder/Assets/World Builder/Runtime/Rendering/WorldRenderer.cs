using UnityEngine;
using WorldBuilder.Data;

namespace WorldBuilder.Rendering
{
    [ExecuteAlways]
    public abstract class WorldRenderer<T> : MonoBehaviour where T : DataLayer
    {
        [SerializeField] private WorldLayer _layer;
        [SerializeField] private World _world;

        protected T DataLayer;
        protected World World => _world;
        protected WorldLayout Layout => _world == null ? default : _world.Layout;
        protected bool IsDataLayerValid => DataLayer != null && _world != null;

        protected virtual void OnValidate()
        {
            if (_layer == null || _world == null || !_world.Data.HasDataLayer<T>(_layer))
            {
                if (DataLayer != null)
                {
                    OnDataLayerLost();
                    DataLayer = null;
                }

                return;
            }

            if (DataLayer != null)
            {
                OnDataLayerLost();
                DataLayer = null;
            }
            
            DataLayer = _world.Data.GetDataLayer<T>(_layer);
            OnDataLayerFound(DataLayer);
        }

        protected virtual void Awake()
        {
            DataLayer = _world.Data.GetDataLayer<T>(_layer);

            if (DataLayer == null)
                return;

            OnDataLayerFound(DataLayer);
        }

        protected virtual void OnDirtyResolve() { }
        protected abstract void OnDataLayerFound(T dataLayer);
        protected abstract void OnDataLayerLost();
    }
}