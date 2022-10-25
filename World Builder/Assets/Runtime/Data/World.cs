using UnityEngine;

namespace WorldBuilder.Data
{
    public class World : MonoBehaviour
    {
        [SerializeField] private Vector3Int _newSize = Vector3Int.one;
        [SerializeField] private WorldData _data = new WorldData(1, 1, 1);
        [SerializeField] private WorldLayout _layout = WorldLayout.One;

        public WorldData Data => _data;
        public WorldLayout Layout => _layout;

        [ContextMenu(nameof(Resize))]
        public void Resize()
        {
            _data.Resize(_newSize.x, _newSize.y, _newSize.z);
        }
    }
}
