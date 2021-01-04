using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace PHL.Common.Utility
{
    public class EnableableValue
    {
        public bool enabled
        {
            get
            {
                return _currentEnablers.Count > 0;
            }
        }

        public bool disabled
        {
            get
            {
                return _currentEnablers.Count <= 0;
            }
        }

        private HashSet<Enabler> _currentEnablers;
        
        public EnableableValue()
        {
            _currentEnablers = new HashSet<Enabler>();
        }

        public Enabler AddEnabler()
        {
            return AddEnabler("");
        }

        public Enabler AddEnabler(string label)
        {
            Enabler newEnabler = new Enabler(label);
            AddEnabler(newEnabler);
            return newEnabler;
        }
        
        private void AddEnabler(Enabler newEnabler)
        {
            if(!_currentEnablers.Contains(newEnabler))
            {
                _currentEnablers.Add(newEnabler);
                newEnabler.destroyEvent.AddListener(OnEnablerDestroy);
            }
        }

        private void OnEnablerDestroy(Enabler enabler)
        {
            _currentEnablers.Remove(enabler);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (Enabler enabler in _currentEnablers)
            {
                sb.Append(enabler.ToString());
                sb.Append(", ");
            }

            return sb.ToString();
        }

        public static implicit operator bool(EnableableValue enableableValue)
        {
            return enableableValue.enabled;
        }
    }
}