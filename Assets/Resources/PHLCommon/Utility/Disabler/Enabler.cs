using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PHL.Common.Utility
{
    public class EnablerEvent : SecureEvent<Enabler> { }

    public class Enabler
    {
        private string _label;

        public override string ToString()
        {
            return string.Format("Enabler[{0}]", _label);
        }

        public EnablerEvent destroyEvent { get; protected set; }

        public Enabler()
        {
            destroyEvent = new EnablerEvent();
        }

        public Enabler(string label) : this()
        {
            _label = label;
        }

        public void Destroy()
        {
            destroyEvent.Invoke(this);
        }
    }
}