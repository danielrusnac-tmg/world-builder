using UnityEngine;
using WorldBuilder.Data;

namespace WorldBuilder.Painting
{
    public struct PaintData
    {
        public Vector3Int StartCoordinate;
        public Vector3Int Coordinate;
        public bool IsErase;
        public WorldData Data;
    }
}