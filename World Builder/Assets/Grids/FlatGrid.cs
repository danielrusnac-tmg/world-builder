using System;
using UnityEngine;

namespace Grids
{
    [Serializable]
    public class FlatGrid<T>
    {
        public event Action<int, int> Changed;

        [SerializeField, HideInInspector] private int m_width;
        [SerializeField, HideInInspector] private int m_height;
        [SerializeField, HideInInspector] private T[] m_items;

        public FlatGridLayout Layout;

        public int Width => m_width;
        public int Height => m_height;

        public FlatGrid(int width, int height) : this(width, height, FlatGridLayout.One) { }

        public FlatGrid(int width, int height, FlatGridLayout layout)
        {
            m_width = width;
            m_height = height;
            Layout = layout;
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

        public (int x, int y) Coordinate(Vector3 worldPosition)
        {
            return Layout.Coordinate(worldPosition);
        }

        public Vector3 WorldPosition(int x, int y)
        {
            return Layout.WorldPosition(x, y);
        }

        private bool IsValidCoordinate(int x, int y)
        {
            return x >= 0 && x < m_width &&
                   y >= 0 && y < m_height;
        }

        private int Flatten(int x, int y)
        {
            return Flatten(x, y, m_width);
        }

        private int Flatten(int x, int y, int width)
        {
            return x + y * width;
        }
    }
}