using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PHL.Common.Utility
{
    public class SceneTransitionRequest
    {
        public List<string> unloadScenes;
        public List<string> loadScenes;
        public bool useLoadingScreen;
        public float loadingScreenDelay;
        public string activeSceneName;
    }

    public class SceneTransition
    {
        private static HashSet<string> _loadedScenes = new HashSet<string>();
        private static Queue<SceneTransitionRequest> _sceneTransitionRequests = new Queue<SceneTransitionRequest>();
        private static IEnumerator _transitionRoutine;

        public static bool transitioning
        {
            get
            {
                return _transitionRoutine != null;
            }
        }

        public static bool SceneIsLoaded(string sceneName)
        {
            return _loadedScenes.Contains(sceneName);
        }

        public static void Transition(SceneTransitionRequest transitionRequest)
        {
            _sceneTransitionRequests.Enqueue(transitionRequest);

            if (!transitioning)
            {
                _transitionRoutine = TransitionRoutine(_sceneTransitionRequests.Dequeue());
                CoroutineRunner.RunCoroutine(_transitionRoutine);
            }
        }

        private static IEnumerator TransitionRoutine(SceneTransitionRequest transitionRequest)
        {
            if (transitionRequest.unloadScenes != null)
            {
                foreach (string unloadScene in transitionRequest.unloadScenes)
                {
                    if (!string.IsNullOrEmpty(unloadScene))
                    {
                        if (SceneManager.GetSceneByName(unloadScene).isLoaded)
                        {
                            AsyncOperation unload = SceneManager.UnloadSceneAsync(unloadScene);
                            yield return new WaitUntil(() => unload.isDone);
                        }

                        if (_loadedScenes.Contains(unloadScene))
                        {
                            _loadedScenes.Remove(unloadScene);
                        }
                    }
                }
            }
            
            if (transitionRequest.loadScenes != null)
            {
                foreach (string loadScene in transitionRequest.loadScenes)
                {
                    if (!string.IsNullOrEmpty(loadScene))
                    {
                        if (!SceneManager.GetSceneByName(loadScene).isLoaded)
                        {
                            AsyncOperation load = SceneManager.LoadSceneAsync(loadScene, LoadSceneMode.Additive);
                            yield return new WaitUntil(() => load.isDone);
                        }

                        if (!_loadedScenes.Contains(loadScene))
                        {
                            _loadedScenes.Add(loadScene);
                        }
                    }
                }
            }

            yield return 0f;

            if(!string.IsNullOrEmpty(transitionRequest.activeSceneName))
            {
                SceneManager.SetActiveScene(SceneManager.GetSceneByName(transitionRequest.activeSceneName));
            }

            _transitionRoutine = null;

            if (_sceneTransitionRequests.Count > 0)
            {
                _transitionRoutine = TransitionRoutine(_sceneTransitionRequests.Dequeue());
                CoroutineRunner.RunCoroutine(_transitionRoutine);
            }
        }
    }
}