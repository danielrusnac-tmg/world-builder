using System;
using UnityEngine;

namespace Grids
{
    [Serializable]
    public struct GridLayout
    {
        public Vector3 CellSize;
        public Vector3 Origin;

        public GridLayout(Vector3 cellSize, Vector3 origin)
        {
            CellSize = cellSize;
            Origin = origin;
        }

        public Vector3 WorldPosition(Vector3Int index)
        {
            return WorldPosition(index.x, index.y, index.z);
        }

        public Vector3 WorldPosition(int x, int y, int z)
        {
            return Origin + Vector3.Scale(new Vector3(x, y, z), CellSize) + CellSize * 0.5f;
        }

        public Vector3Int CoordinateAsVector(Vector3 worldPosition)
        {
            (int x, int y, int z) = Coordinate(worldPosition);
            return new Vector3Int(x, y, z);
        }

        public (int x, int y, int z) Coordinate(Vector3 worldPosition)
        {
            worldPosition -= CellSize * 0.5f + Origin;

            return (
                Mathf.RoundToInt(worldPosition.x / CellSize.x),
                Mathf.RoundToInt(worldPosition.y / CellSize.y),
                Mathf.RoundToInt(worldPosition.z / CellSize.z));
        }

        public Bounds CalculateBounds(int width, int height, int length)
        {
            Vector3 size = Vector3.Scale(new Vector3(width, height, length), CellSize);
            Vector3 center = Origin + size / 2;
            return new Bounds(center, size);
        }

        public static GridLayout One => new GridLayout(Vector3.one, Vector3.zero);
    }
}