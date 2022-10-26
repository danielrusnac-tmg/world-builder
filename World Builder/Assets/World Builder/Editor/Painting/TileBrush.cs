using Autotiling;
using UnityEngine;
using WorldBuilder.Data;

namespace WorldBuilder.Painting
{
    [CreateAssetMenu(menuName = CreatePath.BRUSHES + "Tile Brush", fileName = "brush_")]
    public class TileBrush : Brush
    {
        [SerializeField] private TileType _paintType;
        [SerializeField] private TileType _eraseType;
        [SerializeField] private WorldLayer _layer;
        [SerializeField] private bool _fillHeight = true;
        [SerializeField] [HideInInspector] private GUIContent _guiContent;

        private int _fillY;
        public override GUIContent Preview => _guiContent;

        private byte PaintID => (byte)(_paintType == null ? 0 : _paintType.ID);
        private byte EraseID => (byte)(_eraseType == null ? 0 : _eraseType.ID);

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

            if (data.IsErase)
            {
                OnErase(dataLayer, data);
            }
            else
            {
                OnPaint(dataLayer, data);
            }
        }

        private void OnPaint(WorldGridByte dataLayer, PaintData data)
        {
            if (data.StartCoordinate == data.Coordinate)
            {
                _fillY = dataLayer.Get(data.Coordinate) == PaintID ? data.Coordinate.y + 1 : data.Coordinate.y;
                _fillY = Mathf.Min(_fillY, dataLayer.Height - 1);
            }

            if (_fillHeight)
            {
                for (int y = 0; y <= _fillY; y++)
                    dataLayer.Set(PaintID, new Vector3Int(data.Coordinate.x, y, data.Coordinate.z));
            }
            else
            {
                if (dataLayer.Get(data.Coordinate) == PaintID && data.Coordinate.y <= data.StartCoordinate.y)
                    data.Coordinate.y = Mathf.Min(data.Coordinate.y, dataLayer.Height - 1);

                dataLayer.Set(PaintID, data.Coordinate);
            }
        }

        private void OnErase(WorldGridByte dataLayer, PaintData data)
        {
            if (data.StartCoordinate == data.Coordinate)
                _fillY = data.StartCoordinate.y;
            
            if (_fillHeight)
            {
                for (int y = dataLayer.Height - 1; y >= _fillY; y--)
                    dataLayer.Set(EraseID, new Vector3Int(data.Coordinate.x, y, data.Coordinate.z));
            }
            else
            {
                dataLayer.Set(EraseID, data.Coordinate);
            }
        }
    }
}