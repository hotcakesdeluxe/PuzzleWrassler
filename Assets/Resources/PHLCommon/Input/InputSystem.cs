using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PHL.Common.Input
{
    public class InputSystem : MonoBehaviour
    {
        public InputDeviceIndexPair deviceIndexPair;
        public bool disabled;

        private void Start()
        {
            ClearInput();
        }

        public virtual void ClearInput()
        {

        }
    }
}