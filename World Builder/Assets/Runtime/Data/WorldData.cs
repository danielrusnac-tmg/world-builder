using System;
using System.Collections.Generic;

namespace WorldBuilder.Data
{
    [Serializable]
    public class WorldData
    {
        public int Width;
        public int Height;
        public int Length;
        
        private Dictionary<WorldLayer, IDataLayer> _dataLayers;

        public WorldData(int width, int height, int length)
        {
            Width = width;
            Height = height;
            Length = length;
            
            _dataLayers = new Dictionary<WorldLayer, IDataLayer>();
        }

        public T GetOrCreateDataLayer<T>(WorldLayer layer) where T : IDataLayer
        {
            if(!_dataLayers.ContainsKey(layer))
                _dataLayers.Add(layer, CreateDataLayer<T>());
            
            return _dataLayers[layer] is T ? (T)_dataLayers[layer] : default;
        }

        public void Resize(int width, int height, int length)
        {
            foreach (IDataLayer dataLayer in _dataLayers.Values)
                dataLayer.Resize(width, height, length);
        }

        private T CreateDataLayer<T>() where T : IDataLayer
        {
            T dataLayer =  Activator.CreateInstance<T>();
            dataLayer.Resize(Width, Height, Length);
            return dataLayer;
        }
    }
}