using System;
using UnityEngine;

namespace WorldBuilder.Data
{
    [Serializable]
    public struct WorldLayout
    {
        public Vector3 CellSize;
        public Vector3 Origin;

        public WorldLayout(Vector3 cellSize, Vector3 origin)
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

        public static WorldLayout One => new WorldLayout(Vector3.one, Vector3.zero);
    }
}