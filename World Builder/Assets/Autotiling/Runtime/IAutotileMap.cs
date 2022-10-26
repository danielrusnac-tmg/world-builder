using UnityEngine;

namespace Autotiling
{
    public interface IAutotileMap
    {
        int Width { get; }
        int Height { get; }
        int Length { get; }
        Vector3 CellSize { get; }
        Vector3 WorldPosition(int x, int y, int z);
        byte GetValue(int x, int y, int z);
    }
}