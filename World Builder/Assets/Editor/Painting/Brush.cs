using UnityEngine;

namespace WorldBuilder.Painting
{
    public abstract class Brush : ScriptableObject
    {
        public abstract GUIContent Preview { get; }
        public abstract void Paint(PaintData data);
    }
}