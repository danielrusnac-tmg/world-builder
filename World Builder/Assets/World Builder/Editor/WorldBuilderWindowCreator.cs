using UnityEditor;
using WorldBuilder.Painting;

namespace WorldBuilder
{
    public static class WorldBuilderWindowCreator
    {
        [MenuItem("World Builder/Open")]
        private static void OpenWorldBuilderWindow()
        {
            WorldBuilderWindow.ShowWindow(new TilePaintingPage());
        }
    }
}