using UnityEngine;

namespace WorldBuilder.Painting
{
    [CreateAssetMenu(menuName = CreatePath.PAINTING + "Palette", fileName = "palette_")]
    public class Palette : ScriptableObject
    {
        [SerializeField] private string _name;
        [SerializeField] [TextArea] private string _descriptions;
        [SerializeField] private Brush[] _brushes;

        public string Name => _name;
        public string Descriptions => _descriptions;
        public Brush[] Brushes => _brushes;
    }
}