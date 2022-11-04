using System;
using UnityEngine;

namespace Grids
{
    [Serializable]
    public struct FlatGridLayout
    {
        public Vector2 CellSize;
        public Vector3 Origin;

        public FlatGridLayout(Vector2 cellSize, Vector3 origin)
        {
            CellSize = cellSize;
            Origin = origin;
        }

        public FlatGridLayout(FlatGridLayout layout, int width, int height) :
            this(layout.CellSize, layout.Origin, width, height) { }


        public FlatGridLayout(Vector2 cellSize, Vector3 origin, int width, int height)
        {
            CellSize = cellSize;
            Origin = origin - Size(width, height, cellSize) * 0.5f;
        }

        public Vector3 WorldPosition(int x, int y)
        {
            Vector3 cellSize = new Vector3(CellSize.x, 0f, CellSize.y);
            return Origin + Vector3.Scale(new Vector3(x, 0, y), cellSize) + cellSize * 0.5f;
        }

        public (int x, int y) Coordinate(Vector3 worldPosition)
        {
            worldPosition -= new Vector3(CellSize.x, 0f, CellSize.y) * 0.5f + Origin;

            return (
                Mathf.RoundToInt(worldPosition.x / CellSize.x),
                Mathf.RoundToInt(worldPosition.z / CellSize.y));
        }

        public Bounds CalculateBounds(int width, int height)
        {
            Vector3 size = Vector3.Scale(Size(width, height), CellSize);
            Vector3 center = Origin + size / 2;
            return new Bounds(center, size);
        }

        public Vector3 Size(int width, int height)
        {
            return Size(width, height, CellSize);
        }

        public static Vector3 Size(int width, int height, Vector3 cellSize)
        {
            return new Vector3(width * cellSize.x, 0f, height * cellSize.z);
        }

        public static FlatGridLayout One => new FlatGridLayout(Vector2.one, Vector3.zero);
    }
}