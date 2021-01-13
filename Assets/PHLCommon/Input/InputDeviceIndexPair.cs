using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PHL.Common.Utility;

namespace PHL.Common.Input
{
    public class DeviceIndexEvent : SecureEvent<InputDeviceIndexPair> { }

    [System.Serializable]
    public struct InputDeviceIndexPair
    {
        public InputDeviceData inputDeviceData;
        public int index;

        public InputDeviceIndexPair(InputDeviceData newInputDeviceData, int newIndex)
        {
            inputDeviceData = newInputDeviceData;
            index = newIndex;
        }
    }
}