using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PHL.Common.Utility
{
    public class DisablerEvent : SecureEvent<Disabler> { }

    public class Disabler
    {
        private string _label;

        public override string ToString()
        {
            return string.Format("Disabler[{0}]", _label);
        }

        public DisablerEvent destroyEvent { get; protected set; }

        public Disabler()
        {
            destroyEvent = new DisablerEvent();
        }

        public Disabler(string label) : this()
        {
            _label = label;
        }

        public void Destroy()
        {
            destroyEvent.Invoke(this);
        }
    }
}