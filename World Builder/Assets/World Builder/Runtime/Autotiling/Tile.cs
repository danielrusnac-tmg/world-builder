using System;
using UnityEngine;

namespace WorldBuilder.Autotiling
{
    [Serializable]
    public class Tile
    {
        public string Name;
        public int ID;
        public int Rotation;
        public Vector3Int Scale;
        public GameObject Prefab;
        public Mesh Mesh;
        public Material[] Materials;
    }
}