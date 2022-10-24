using UnityEngine;
using WorldBuilder.Data;

namespace WorldBuilder
{
    public class World : MonoBehaviour
    {
        [SerializeField] private WorldData _data = new WorldData(1, 1, 1);
        [SerializeField] private WorldLayout _layout = WorldLayout.One;
    }
}
