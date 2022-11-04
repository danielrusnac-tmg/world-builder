using System;

namespace Grids
{
    public class Grid<T>
    {
        public event Action<int, int> Changed;

        public readonly int Width;
        public readonly int Height;

        private readonly T[] m_items;

        public Grid(int width, int height)
        {
            Width = width;
            Height = height;
            m_items = new T[width * height];
        }

        public T Get(int x, int y)
        {
            if (!IsValidCoordinate(x, y))
                return default;

            return m_items[Flatten(x, y)];
        }

        public bool Set(T value, int x, int y)
        {
            if (!IsValidCoordinate(x, y))
                return false;

            m_items[Flatten(x, y)] = value;
            Changed?.Invoke(x, y);

            return true;
        }

        private bool IsValidCoordinate(int x, int y)
        {
            return x >= 0 && x < Width &&
                   y >= 0 && y < Height;
        }

        private int Flatten(int x, int y)
        {
            return Flatten(x, y, Width);
        }

        private int Flatten(int x, int y, int width)
        {
            return x + y * width;
        }
    }
}