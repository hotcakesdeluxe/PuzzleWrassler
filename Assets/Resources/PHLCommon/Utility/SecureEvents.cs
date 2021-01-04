using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

namespace PHL.Common.Utility
{
    public class BoolEvent : SecureEvent<bool> { }
    public class IntEvent : SecureEvent<int> { }
    public class FloatEvent : SecureEvent<float> { }
    public class StringEvent : SecureEvent<string> { }

    public class SecureEvent : UnityEvent
    {
        private HashSet<UnityAction> _listeners;

        public SecureEvent()
        {
            _listeners = new HashSet<UnityAction>();
        }

        new public void AddListener(UnityAction call)
        {
            if (!HasListener(call))
            {
                base.AddListener(call);
                _listeners.Add(call);
            }
        }

        public void AddListener(UnityAction call, bool noDuplicates)
        {
            if (!noDuplicates || !HasListener(call))
            {
                base.AddListener(call);
                _listeners.Add(call);
            }
        }

        new public void RemoveListener(UnityAction call)
        {
            base.RemoveListener(call);
            _listeners.Remove(call);
        }

        new public void RemoveAllListeners()
        {
            base.RemoveAllListeners();
            _listeners.Clear();
        }

        public bool HasListener(UnityAction call)
        {
            return _listeners.Contains(call);
        }

        new public void Invoke()
        {
            CleanEvent();
            base.Invoke();
        }

        public static SecureEvent operator +(SecureEvent secureEvent, UnityAction call)
        {
            secureEvent.AddListener(call);
            return secureEvent;
        }

        public static SecureEvent operator -(SecureEvent secureEvent, UnityAction call)
        {
            secureEvent.RemoveListener(call);
            return secureEvent;
        }

        private void CleanEvent()
        {
            _listeners.RemoveWhere(x => x.Target == null || x.Target.Equals(null));
        }
    }

    public class SecureEvent<T0> : UnityEvent<T0>
    {
        private HashSet<UnityAction<T0>> _listeners;

        public SecureEvent()
        {
            _listeners = new HashSet<UnityAction<T0>>();
        }

        new public void AddListener(UnityAction<T0> call)
        {
            if (!HasListener(call))
            {
                base.AddListener(call);
                _listeners.Add(call);
            }
        }

        public void AddListener(UnityAction<T0> call, bool noDuplicates)
        {
            if (!noDuplicates || !HasListener(call))
            {
                base.AddListener(call);
                _listeners.Add(call);
            }
        }

        new public void RemoveListener(UnityAction<T0> call)
        {
            base.RemoveListener(call);
            _listeners.Remove(call);
        }

        new public void RemoveAllListeners()
        {
            base.RemoveAllListeners();
            _listeners.Clear();
        }
        
        public bool HasListener(UnityAction<T0> call)
        {
            return _listeners.Contains(call);
        }

        new public void Invoke(T0 param1)
        {
            CleanEvent();
            base.Invoke(param1);
        }

        public static SecureEvent<T0> operator +(SecureEvent<T0> secureEvent, UnityAction<T0> call)
        {
            secureEvent.AddListener(call);
            return secureEvent;
        }

        public static SecureEvent<T0> operator -(SecureEvent<T0> secureEvent, UnityAction<T0> call)
        {
            secureEvent.RemoveListener(call);
            return secureEvent;
        }

        private void CleanEvent()
        {
            _listeners.RemoveWhere(x => x.Target == null || x.Target.Equals(null));
        }
    }

    public class SecureEvent<T0, T1> : UnityEvent<T0, T1>
    {
        private HashSet<UnityAction<T0, T1>> _listeners;

        public SecureEvent()
        {
            _listeners = new HashSet<UnityAction<T0, T1>>();
        }

        new public void AddListener(UnityAction<T0, T1> call)
        {
            if (!HasListener(call))
            {
                base.AddListener(call);
                _listeners.Add(call);
            }
        }

        public void AddListener(UnityAction<T0, T1> call, bool noDuplicates)
        {
            if (!noDuplicates || !HasListener(call))
            {
                base.AddListener(call);
                _listeners.Add(call);
            }
        }

        new public void RemoveListener(UnityAction<T0, T1> call)
        {
            base.RemoveListener(call);
            _listeners.Remove(call);
        }

        new public void RemoveAllListeners()
        {
            base.RemoveAllListeners();
            _listeners.Clear();
        }
        
        public bool HasListener(UnityAction<T0, T1> call)
        {
            return _listeners.Contains(call);
        }

        new public void Invoke(T0 param1, T1 param2)
        {
            CleanEvent();
            base.Invoke(param1, param2);
        }

        public static SecureEvent<T0, T1> operator +(SecureEvent<T0, T1> secureEvent, UnityAction<T0, T1> call)
        {
            secureEvent.AddListener(call);
            return secureEvent;
        }

        public static SecureEvent<T0, T1> operator -(SecureEvent<T0, T1> secureEvent, UnityAction<T0, T1> call)
        {
            secureEvent.RemoveListener(call);
            return secureEvent;
        }

        private void CleanEvent()
        {
            _listeners.RemoveWhere(x => x.Target == null || x.Target.Equals(null));
        }
    }

    public class SecureEvent<T0, T1, T2> : UnityEvent<T0, T1, T2>
    {
        private HashSet<UnityAction<T0, T1, T2>> _listeners;

        public SecureEvent()
        {
            _listeners = new HashSet<UnityAction<T0, T1, T2>>();
        }

        new public void AddListener(UnityAction<T0, T1, T2> call)
        {
            if (!HasListener(call))
            {
                base.AddListener(call);
                _listeners.Add(call);
            }
        }

        public void AddListener(UnityAction<T0, T1, T2> call, bool noDuplicates)
        {
            if (!noDuplicates || !HasListener(call))
            {
                base.AddListener(call);
                _listeners.Add(call);
            }
        }

        new public void RemoveListener(UnityAction<T0, T1, T2> call)
        {
            base.RemoveListener(call);
            _listeners.Remove(call);
        }

        new public void RemoveAllListeners()
        {
            base.RemoveAllListeners();
            _listeners.Clear();
        }

        public bool HasListener(UnityAction<T0, T1, T2> call)
        {
            return _listeners.Contains(call);
        }

        new public void Invoke(T0 param1, T1 param2, T2 param3)
        {
            CleanEvent();
            base.Invoke(param1, param2, param3);
        }

        public static SecureEvent<T0, T1, T2> operator +(SecureEvent<T0, T1, T2> secureEvent, UnityAction<T0, T1, T2> call)
        {
            secureEvent.AddListener(call);
            return secureEvent;
        }

        public static SecureEvent<T0, T1, T2> operator -(SecureEvent<T0, T1, T2> secureEvent, UnityAction<T0, T1, T2> call)
        {
            secureEvent.RemoveListener(call);
            return secureEvent;
        }

        private void CleanEvent()
        {
            _listeners.RemoveWhere(x => x.Target == null || x.Target.Equals(null));
        }
    }

    public class SecureEvent<T0, T1, T2, T3> : UnityEvent<T0, T1, T2, T3>
    {
        private HashSet<UnityAction<T0, T1, T2, T3>> _listeners;

        public SecureEvent()
        {
            _listeners = new HashSet<UnityAction<T0, T1, T2, T3>>();
        }

        new public void AddListener(UnityAction<T0, T1, T2, T3> call)
        {
            if (!HasListener(call))
            {
                base.AddListener(call);
                _listeners.Add(call);
            }
        }

        public void AddListener(UnityAction<T0, T1, T2, T3> call, bool noDuplicates)
        {
            if (!noDuplicates || !HasListener(call))
            {
                base.AddListener(call);
                _listeners.Add(call);
            }
        }

        new public void RemoveListener(UnityAction<T0, T1, T2, T3> call)
        {
            base.RemoveListener(call);
            _listeners.Remove(call);
        }

        new public void RemoveAllListeners()
        {
            base.RemoveAllListeners();
            _listeners.Clear();
        }

        public bool HasListener(UnityAction<T0, T1, T2, T3> call)
        {
            return _listeners.Contains(call);
        }

        new public void Invoke(T0 param1, T1 param2, T2 param3, T3 param4)
        {
            CleanEvent();
            base.Invoke(param1, param2, param3, param4);
        }

        public static SecureEvent<T0, T1, T2, T3> operator +(SecureEvent<T0, T1, T2, T3> secureEvent, UnityAction<T0, T1, T2, T3> call)
        {
            secureEvent.AddListener(call);
            return secureEvent;
        }

        public static SecureEvent<T0, T1, T2, T3> operator -(SecureEvent<T0, T1, T2, T3> secureEvent, UnityAction<T0, T1, T2, T3> call)
        {
            secureEvent.RemoveListener(call);
            return secureEvent;
        }

        private void CleanEvent()
        {
            _listeners.RemoveWhere(x => x.Target == null || x.Target.Equals(null));
        }
    }
}