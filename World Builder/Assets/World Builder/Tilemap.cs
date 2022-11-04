using System;
using Grids;

namespace WorldBuilder
{
    [Serializable]
    public class Tilemap : FlatGrid<byte>
    {
        public Tilemap(int width, int height) : base(width, height) { }
    }
}