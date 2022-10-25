using System;

namespace WorldBuilder.Data
{
    [Serializable]
    public class WorldGrid<T> : IDataLayer
    {
        public int Width;
        public int Height;
        public int Length;
        public T[] Items;
        
        public void Resize(int width, int height, int length)
        {
            Width = width;
            Height = height;
            Length = length;
            Items = new T[width * height * length];
        }

        public T Get(int x, int y, int z)
        {
            if (!IsValidCoordinate(x, y, z))
                return default;
            
            return Items[ArrayUtility.Flatten(x, y, z, Width, Height)];
        }

        public bool Set(T value, int x, int y, int z)
        {
            if (!IsValidCoordinate(x, y, z))
                return false;

            Items[ArrayUtility.Flatten(x, y, z, Width, Height)] = value;
            
            return true;
        }

        private bool IsValidCoordinate(int x, int y, int z)
        {
            return x >= 0 && x < Width &&
                   y >= 0 && y < Height &&
                   z >= 0 && z < Length;
        }
    }
}