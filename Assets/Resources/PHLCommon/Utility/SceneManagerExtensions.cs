using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneManagerExtensions
{
    public static List<Scene> GetSceneInstances(string scene)
    {
        List<Scene> _loadedScenes = new List<Scene>();

        int loadedScenes = SceneManager.sceneCount;
        
        for(int i =0; i < loadedScenes; i++)
        {
            Scene loadedScene = SceneManager.GetSceneAt(i);
            
            if(loadedScene.name == scene)
            {
                _loadedScenes.Add(loadedScene);
            }
        }
        return _loadedScenes;
    }

}
