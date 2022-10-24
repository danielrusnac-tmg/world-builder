using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace WorldBuilder
{
    [CreateAssetMenu(menuName = "World Builder/Map", fileName = "map_")]
    public class Map : ScriptableObject
    {
        public string Name;
        public string SceneName;
        
        public bool IsLoaded => SceneManager.GetSceneByName(SceneName).isLoaded;

        public IEnumerator Load()
        {
            if (IsLoaded)
                yield break;

            yield return SceneManager.LoadSceneAsync(SceneName, LoadSceneMode.Additive);
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(SceneName));
        }

        public IEnumerator Unload()
        {
            if (!IsLoaded)
                yield break;

            yield return SceneManager.UnloadSceneAsync(SceneName);
        }
    }
}