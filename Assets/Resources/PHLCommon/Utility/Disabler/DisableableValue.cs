using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace PHL.Common.Utility
{
    public class DisableableValue
    {
        public bool enabled
        {
            get
            {
                return _currentDisablers.Count <= 0;
            }
        }

        public bool disabled
        {
            get
            {
                return _currentDisablers.Count > 0;
            }
        }

        private HashSet<Disabler> _currentDisablers;
        
        public DisableableValue()
        {
            _currentDisablers = new HashSet<Disabler>();
        }

        public Disabler AddDisabler()
        {
            return AddDisabler("");
        }

        public Disabler AddDisabler(string label)
        {
            Disabler newDisabler = new Disabler(label);
            AddDisabler(newDisabler);
            return newDisabler;
        }

        public void AddDisabler(Disabler newDisabler)
        {
            if(!_currentDisablers.Contains(newDisabler))
            {
                _currentDisablers.Add(newDisabler);
                newDisabler.destroyEvent.AddListener(OnDisablerDestroy);
            }
        }

        private void OnDisablerDestroy(Disabler disabler)
        {
            _currentDisablers.Remove(disabler);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (Disabler disabler in _currentDisablers)
            {
                sb.Append(disabler.ToString());
                sb.Append(", ");
            }

            return sb.ToString();
        }

        public static implicit operator bool(DisableableValue disableableValue)
        {
            return disableableValue.enabled;
        }
    }
}