using UnityEngine;

namespace WorldBuilder.Data
{
    public abstract class DataLayer : ScriptableObject
    {
        public abstract void Resize(int width, int height, int length);
    }
}