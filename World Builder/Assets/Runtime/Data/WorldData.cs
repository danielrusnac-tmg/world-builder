using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WorldBuilder.Data
{
    [Serializable]
    public class WorldData : ISerializationCallbackReceiver
    {
        public int Width;
        public int Height;
        public int Length;

        [SerializeField] private WorldLayer[] _layers;
        [SerializeField] private DataLayer[] _data;

        private Dictionary<WorldLayer, DataLayer> _dataLayers;

        public WorldData(int width, int height, int length)
        {
            Width = width;
            Height = height;
            Length = length;

            _dataLayers = new Dictionary<WorldLayer, DataLayer>();
        }

        public T CreateDataLayer<T>(WorldLayer layer) where T : DataLayer
        {
            if (_dataLayers.ContainsKey(layer))
                _dataLayers.Remove(layer);

            T dataLayer = ScriptableObject.CreateInstance<T>();
            dataLayer.Resize(Width, Height, Length);
            _dataLayers.Add(layer, dataLayer);
            
            return dataLayer;
        }

        public T GetDataLayer<T>(WorldLayer layer) where T : DataLayer
        {
            if (HasDataLayer<T>(layer))
                return _dataLayers[layer] as T;

            return default;
        }

        public bool HasDataLayer<T>(WorldLayer layer) where T : DataLayer
        {
            return _dataLayers.ContainsKey(layer) && _dataLayers[layer] is T;
        }

        public void Resize(int width, int height, int length)
        {
            Width = width;
            Height = height;
            Length = length;

            foreach (DataLayer dataLayer in _dataLayers.Values)
                dataLayer.Resize(width, height, length);
        }

        public void OnBeforeSerialize()
        {
            if (_dataLayers == null || _dataLayers.Count == 0)
            {
                _layers = Array.Empty<WorldLayer>();
                _data = Array.Empty<DataLayer>();
                return;
            }

            _layers = _dataLayers.Keys.ToArray();
            _data = _dataLayers.Values.ToArray();
        }

        public void OnAfterDeserialize()
        {
            _dataLayers = new Dictionary<WorldLayer, DataLayer>();

            if (_layers == null || _data == null || _layers.Length == 0 || _layers.Length != _data.Length)
                return;

            for (int i = 0; i < _layers.Length; i++)
            {
                if (_layers[i] == null || _data[i] is null || _dataLayers.ContainsKey(_layers[i]))
                    continue;

                _dataLayers.Add(_layers[i], _data[i]);
            }
        }
    }
}