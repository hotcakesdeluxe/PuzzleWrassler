using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PHL.Common.Input
{
    public class InputController : MonoBehaviour
    {
        [SerializeField] protected List<RuntimePlatform> _supportedPlatforms;
        [SerializeField] protected List<InputDeviceData> _supportedDevices;

        protected virtual InputSystem GetCurrentInputSystem()
        {
            return null;
        }

        protected virtual bool InputActive()
        {
            if (GetCurrentInputSystem() == null)
            {
                return false;
            }

            if (GetCurrentInputSystem().deviceIndexPair.inputDeviceData == null)
            {
                return false;
            }

            if (!_supportedPlatforms.Contains(Application.platform))
            {
                return false;
            }

            if (!_supportedDevices.Contains(GetCurrentInputSystem().deviceIndexPair.inputDeviceData))
            {
                return false;
            }

            return true;
        }

        protected virtual void UpdateInput()
        {
            //Do stuff!
        }

        protected virtual void Update()
        {
            if (InputActive())
            {
                UpdateInput();
            }
        }
    }
}