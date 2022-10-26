using UnityEngine;

namespace WorldBuilder
{
    public abstract class WorldBuilderPage : ScriptableObject
    {
        [SerializeField] private string _title;

        public virtual string Title => _title;
        
        public abstract void Show();
        public abstract void Hide();
        public abstract void OnGUI();
    }
}