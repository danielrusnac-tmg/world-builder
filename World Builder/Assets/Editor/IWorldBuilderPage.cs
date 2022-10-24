namespace WorldBuilder
{
    public interface IWorldBuilderPage
    {
        string Name { get; }
        
        void Show();
        void Hide();
        void OnGUI();
    }
}