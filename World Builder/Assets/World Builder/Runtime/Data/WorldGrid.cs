using System;
using System.Linq;
using UnityEngine;

namespace WorldBuilder.Data
{
    public class WorldGrid<T> : DataLayer
    {
        public event Action<int, int, int> CellChanged;

        public int Width;
        public int Height;
        public int Length;
        public T[] Items;

        public WorldGrid()
        {
            Width = 1;
            Height = 1;
            Length = 1;
            Items = Array.Empty<T>();
        }

        public override void Resize(int width, int height, int length)
        {
            T[] oldItems = Items.ToArray();
            Items = new T[width * height * length];

            int minWidth = Mathf.Min(Width, width);
            int minHeight = Mathf.Min(Height, height);
            int minLength = Mathf.Min(Length, length);

            for (int x = 0; x < minWidth; x++)
            {
                for (int y = 0; y < minHeight; y++)
                {
                    for (int z = 0; z < minLength; z++)
                    {
                        Items[Flatten(x, y, z, width, height)] = oldItems[Flatten(x, y, z, Width, Height)];
                    }
                }
            }

            Width = width;
            Height = height;
            Length = length;
        }

        public T Get(Vector3Int coordinate)
        {
            return Get(coordinate.x, coordinate.y, coordinate.z);
        }

        public T Get(int x, int y, int z)
        {
            if (!IsValidCoordinate(x, y, z))
                return default;

            return Items[Flatten(x, y, z)];
        }

        public bool Set(T value, Vector3Int coordinate)
        {
            return Set(value, coordinate.x, coordinate.y, coordinate.z);
        }

        public bool Set(T value, int x, int y, int z)
        {
            if (!IsValidCoordinate(x, y, z))
                return false;

            Items[Flatten(x, y, z)] = value;
            CellChanged?.Invoke(x, y, z);
            OnChanged();

            return true;
        }

        private bool IsValidCoordinate(int x, int y, int z)
        {
            return x >= 0 && x < Width &&
                   y >= 0 && y < Height &&
                   z >= 0 && z < Length;
        }

        private int Flatten(int x, int y, int z)
        {
            return Flatten(x, y, z, Width, Height);
        }

        private int Flatten(int x, int y, int z, int width, int height)
        {
            return x + y * width + z * width * height;
        }
    }
}