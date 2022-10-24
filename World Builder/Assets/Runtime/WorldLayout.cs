using System;
using UnityEngine;

namespace WorldBuilder
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

        public static WorldLayout One => new WorldLayout(Vector3.one, Vector3.zero);
    }
}