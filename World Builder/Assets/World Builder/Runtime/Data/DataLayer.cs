using System;
using UnityEngine;

namespace WorldBuilder.Data
{
    public abstract class DataLayer : ScriptableObject
    {
        public event Action Changed;
        
        public abstract void Resize(int width, int height, int length);

        protected void OnChanged()
        {
            Changed?.Invoke();
        }
    }
}