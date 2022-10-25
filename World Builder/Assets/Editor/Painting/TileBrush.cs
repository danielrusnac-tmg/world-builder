using UnityEngine;
using WorldBuilder.Rendering.Autotiling;

namespace WorldBuilder.Painting
{
    [CreateAssetMenu(menuName = CreatePath.BRUSHES + "Tile Brush", fileName = "brush_")]
    public class TileBrush : Brush
    {
        [SerializeField] private TileType _tileType;
        [SerializeField] [HideInInspector] private GUIContent _guiContent;

        public override GUIContent Preview => _guiContent;

        private void OnValidate()
        {
            _guiContent = new GUIContent
            {
                text = _tileType != null ? _tileType.name : "?"
            };
        }

        public override void Paint(PaintData data) { }
    }
}