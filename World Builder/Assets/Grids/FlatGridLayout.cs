using System;
using UnityEngine;

namespace Grids
{
    [Serializable]
    public struct FlatGridLayout
    {
        public Vector3 CellSize;
        public Vector3 Origin;
        public FlatGridOrientation Orientation;

        public FlatGridLayout(Vector3 cellSize, Vector3 origin, FlatGridOrientation orientation)
        {
            CellSize = cellSize;
            Origin = origin;
            Orientation = orientation;
        }

        public FlatGridLayout(Vector3 cellSize, Vector3 origin, FlatGridOrientation orientation, int width, int height)
        {
            CellSize = cellSize;
            Origin = origin - Size(width, height, cellSize, orientation) * 0.5f;
            Orientation = orientation;
        }

        public Vector3 WorldPosition(int x, int y)
        {
            Vector3 positionCoordinate = new Vector3(x, 0, 0);

            switch (Orientation)
            {
                case FlatGridOrientation.XY:
                    positionCoordinate.y = y;
                    break;
                case FlatGridOrientation.XZ:
                    positionCoordinate.z = y;
                    break;
            }

            return Origin + Vector3.Scale(positionCoordinate, CellSize) + CellSize * 0.5f;
        }

        public (int x, int y) Coordinate(Vector3 worldPosition)
        {
            worldPosition -= CellSize * 0.5f + Origin;
            float yPosition = Orientation == FlatGridOrientation.XY ? worldPosition.y : worldPosition.z;

            return (
                Mathf.RoundToInt(worldPosition.x / CellSize.x),
                Mathf.RoundToInt(yPosition / CellSize.y));
        }

        public Bounds CalculateBounds(int width, int height)
        {
            Vector3 size = Vector3.Scale(Size(width, height), CellSize);
            Vector3 center = Origin + size / 2;
            return new Bounds(center, size);
        }

        public Vector3 Size(int width, int height)
        {
            return Size(width, height, CellSize, Orientation);
        }

        public static Vector3 Size(int width, int height, Vector3 cellSize, FlatGridOrientation orientation)
        {
            float widthSize = width * cellSize.x;
            float heightSize = height * cellSize.z;

            return orientation switch
            {
                FlatGridOrientation.XY => new Vector3(widthSize, heightSize, 0f),
                FlatGridOrientation.XZ => new Vector3(widthSize, 0f, heightSize),
                _ => throw new ArgumentOutOfRangeException(nameof(orientation), orientation, null)
            };
        }

        public static FlatGridLayout One => new FlatGridLayout(Vector3.one, Vector3.zero, FlatGridOrientation.XZ);
    }
}