using UnityEngine;
using WorldBuilder.Data;
using WorldBuilder.Rendering.Autotiling;

namespace WorldBuilder.Painting
{
    [CreateAssetMenu(menuName = CreatePath.BRUSHES + "Tile Brush", fileName = "brush_")]
    public class TileBrush : Brush
    {
        [SerializeField] private TileType _paintType;
        [SerializeField] private TileType _eraseType;
        [SerializeField] private WorldLayer _layer;
        [SerializeField] [HideInInspector] private GUIContent _guiContent;

        public override GUIContent Preview => _guiContent;

        private byte PaintID => (byte) (_paintType == null ? 0 : _paintType.ID);
        private byte EraseID => (byte) (_eraseType == null ? 0 : _eraseType.ID);

        private void OnValidate()
        {
            _guiContent = new GUIContent
            {
                text = _paintType != null ? _paintType.name : "?"
            };
        }

        public override void Paint(PaintData data)
        {
            if (_layer == null || data.Data == null)
            {
                Debug.LogWarning($"Invalid paint data! {data}", this);
                return;
            }

            WorldGridByte dataLayer = data.Data.GetOrCreateDataLayer<WorldGridByte>(_layer);
            dataLayer.Set(data.IsErase ? EraseID : PaintID, data.Coordinate);
        }
    }
}