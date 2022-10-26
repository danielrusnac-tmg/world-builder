using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WorldBuilder.Autotiling
{
    public static class TileUtility
    {
        public static readonly Vector3[] CORNER_DIRECTIONS;
        
        private static readonly Vector3Int[] TILE_INDEX_OFFSETS;

        public static readonly Vector3Int[] TILE_NEIGHBORS = new[]
        {
            new Vector3Int(0, 0, 0),
            new Vector3Int(-1, 0, 0),
            new Vector3Int(0, 0, -1),
            new Vector3Int(-1, 0, -1),
        };

        static TileUtility()
        {
            CORNER_DIRECTIONS = new[]
            {
                new Vector3(-1f, 0f, -1f),
                new Vector3(-1f, 0f, 1f),
                new Vector3(1f, 0f, 1f),
                new Vector3(1f, 0f, -1f)
            };

            TILE_INDEX_OFFSETS = new[]
            {
                new Vector3Int(0, 0, 0),
                new Vector3Int(0, 0, 1),
                new Vector3Int(1, 0, 1),
                new Vector3Int(1, 0, 0)
            };
        }

        private static int ComputeID(Prototype prototype, Symmetry symmetry, int rotation)
        {
            int[] index = GetSymmetryCorners(symmetry);

            return ComputeID(
                prototype.Corners[(index[0] + rotation) % 4].TerrainID,
                prototype.Corners[(index[1] + rotation) % 4].TerrainID,
                prototype.Corners[(index[2] + rotation) % 4].TerrainID,
                prototype.Corners[(index[3] + rotation) % 4].TerrainID
            );
        }

        private static Vector3Int GetSymmetryScale(Symmetry symmetry, int rotation)
        {
            switch (symmetry)
            {
                case Symmetry.MirrorX when rotation == 0 || rotation == 2:
                    return new Vector3Int(-1, 1, 1);
                case Symmetry.MirrorX when rotation == 1 || rotation == 3:
                    return new Vector3Int(1, 1, -1);
                case Symmetry.MirrorZ when rotation == 0 || rotation == 2:
                    return new Vector3Int(1, 1, -1);
                case Symmetry.MirrorZ when rotation == 1 || rotation == 3:
                    return new Vector3Int(-1, 1, 1);
                case Symmetry.MirrorXZ:
                    return new Vector3Int(-1, 1, -1);
            }

            return Vector3Int.one;
        }

        private static int[] GetSymmetryCorners(Symmetry symmetry)
        {
            switch (symmetry)
            {
                case Symmetry.MirrorX:
                    return new[] { 3, 2, 1, 0 };
                case Symmetry.MirrorZ:
                    return new[] { 1, 0, 3, 2 };
                case Symmetry.MirrorXZ:
                    return new[] { 2, 3, 0, 1 };
            }

            return new[] { 0, 1, 2, 3 };
        }

        public static int ComputeID(int bl, int tl, int tr, int br)
        {
            return bl | (br << 3) | (tl << 6) | (tr << 9);
        }

        public static Tile[] GenerateTiles(Prototype[] prototypes)
        {
            Dictionary<int, Tile> tileByID = new Dictionary<int, Tile>();

            foreach (Prototype prototype in prototypes)
            {
                for (int symmetry = 0; symmetry < 4; symmetry++)
                {
                    for (int rotation = 0; rotation < 4; rotation++)
                    {
                        Tile tile = CreateTile(prototype, rotation, (Symmetry)symmetry);

                        if (tileByID.ContainsKey(tile.ID))
                            continue;

                        tileByID.Add(tile.ID, tile);
                    }
                }
            }

            return tileByID.Values.ToArray();
        }

        private static Tile CreateTile(Prototype prototype, int rotation, Symmetry symmetry)
        {
            int id = ComputeID(prototype, symmetry, rotation);

            return new Tile
            {
                Rotation = rotation,
                ID = id,
                Prefab = prototype.gameObject,
                Scale = GetSymmetryScale(symmetry, rotation),
                Name = $"{id:000}_{prototype.name}_R{rotation}_{symmetry}"
            };
        }

        private enum Symmetry
        {
            None = 0,
            MirrorX = 1,
            MirrorZ = 2,
            MirrorXZ = 3
        }
    }
}