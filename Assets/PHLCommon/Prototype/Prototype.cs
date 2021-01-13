using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PHL.Common.Utility
{
    /// <summary>
    /// Should be used for SO's which will have
    /// runtime instances of the data it uses. 
    /// The prototype is preset data that it starts with.
    /// </summary>
    /// <typeparam name="T">The component data it uses</typeparam>
    public abstract class Prototype<T> : ScriptableObject
    {
        public event Action<T> DataChanged;
        public event Action<T> DataLoaded;

        [Space(15)]
        [Header("Data Properties")]
        [Tooltip("If true, allows the data to be modified.")]
        [SerializeField] private bool _editable;
        [SerializeField] protected T _data;
        public T Data
        {
            get { return _data; }
            set
            {
                if (_editable)
                {
                    _data = value;
                    OnDataChanged();
                }
            }
        }

        public bool IsNull() => _data == null ? true : false;
        
        public T Clone()
        {
            string json = SerializeToJson();
            return (T) JsonUtility.FromJson<T>(json);
        }
        public string SerializeToJson() => JsonUtility.ToJson(_data); 
        public void OnDataChanged() => DataChanged?.Invoke(_data);   
        public void OnDataLoaded() => DataLoaded?.Invoke(_data);

        [ContextMenu("Clear Data")]
        private void ClearData() => _data = default(T);
    }
}