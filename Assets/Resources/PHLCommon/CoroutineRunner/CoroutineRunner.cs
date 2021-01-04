using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PHL.Common.Utility
{
    public class CoroutineRunner
    {
        private static CoroutineRunnerObject _coroutineRunnerObject;

        private static void CreateObject()
        {
            _coroutineRunnerObject = new GameObject("CoroutineRunnerObject").AddComponent<CoroutineRunnerObject>();
            Object.DontDestroyOnLoad(_coroutineRunnerObject);
        }

        public static void RunCoroutine(IEnumerator coroutine)
        {
            if (_coroutineRunnerObject == null && coroutine != null)
            {
                CreateObject();
            }

            _coroutineRunnerObject.StartCoroutine(coroutine);
        }

        public static void StopCoroutine(IEnumerator coroutine)
        {
            if (_coroutineRunnerObject != null && coroutine != null)
            {
                _coroutineRunnerObject.StopCoroutine(coroutine);
            }
        }
    }
}