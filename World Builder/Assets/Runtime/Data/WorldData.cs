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
                _dataLayers.Add(layer, Activator.CreateInstance<T>());
            
            return _dataLayers[layer] is T ? (T)_dataLayers[layer] : default;
        }
    }
}