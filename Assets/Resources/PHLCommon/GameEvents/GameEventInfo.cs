using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PHL.Common.GameEvents
{
    [System.Serializable]
    public class GameEventInfo
    {
        private Dictionary<string, bool> bools = new Dictionary<string, bool>();
        private Dictionary<string, int> ints = new Dictionary<string, int>();
        private Dictionary<string, float> floats = new Dictionary<string, float>();
        private Dictionary<string, string> strings = new Dictionary<string, string>();
        private Dictionary<string, Object> objects = new Dictionary<string, Object>();
        private Dictionary<string, string> fmodEvents = new Dictionary<string, string>();

        public bool GetBool(string id)
        {
            if (bools.ContainsKey(id))
            {
                return bools[id];
            }

            return false;
        }

        public void PutBool(string id, bool value)
        {
            bools.Add(id, value);
        }

        public bool HasBool(string id)
        {
            return bools.ContainsKey(id);
        }

        public int GetInt(string id)
        {
            if (ints.ContainsKey(id))
            {
                return ints[id];
            }

            return 0;
        }

        public void PutInt(string id, int value)
        {
            ints.Add(id, value);
        }

        public bool HasInt(string id)
        {
            return ints.ContainsKey(id);
        }

        public float GetFloat(string id)
        {
            if (floats.ContainsKey(id))
            {
                return floats[id];
            }

            return 0f;
        }

        public void PutFloat(string id, float value)
        {
            floats.Add(id, value);
        }

        public bool HasFloat(string id)
        {
            return floats.ContainsKey(id);
        }

        public string GetString(string id)
        {
            if (strings.ContainsKey(id))
            {
                return strings[id];
            }

            return "";
        }
        
        public void PutString(string id, string value)
        {
            strings.Add(id, value);
        }

        public bool HasString(string id)
        {
            return strings.ContainsKey(id);
        }
        
        public Object GetObject(string id)
        {
            if (objects.ContainsKey(id))
            {
                return objects[id];
            }

            return null;
        }

        public T GetObject<T>(string id) where T : Object
        {
            if (objects.ContainsKey(id))
            {
                if(objects[id] == null)
                {
                    return null;
                }

                return (T)(objects[id]);
            }

            return null;
        }

        public void PutObject(string id, Object value)
        {
            objects.Add(id, value);
        }
        
        public bool HasObject(string id)
        {
            return objects.ContainsKey(id);
        }

        public string GetFMODEvent(string id)
        {
            if (fmodEvents.ContainsKey(id))
            {
                return fmodEvents[id];
            }

            return "";
        }

        public void PutFMODEvent(string id, string value)
        {
            fmodEvents.Add(id, value);
        }

        public bool HasFMODEvent(string id)
        {
            return fmodEvents.ContainsKey(id);
        }
    }
}