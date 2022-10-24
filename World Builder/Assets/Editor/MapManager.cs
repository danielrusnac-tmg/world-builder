using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace WorldBuilder
{
    public class MapManager : ScriptableSingleton<MapManager>
    {
        public List<Map> Maps;

        public void SearchForMapsInProject()
        {
            Maps = WorldBuilderEditorUtility.LoadAssets<Map>().ToList();
        }
    }
}