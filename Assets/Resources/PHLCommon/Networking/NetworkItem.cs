using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PHL.Common.GenericNetworking
{
    public class NetworkItem : MonoBehaviour
    {
        [HideInInspector] public NetworkObjectEvent receiveUpdateEvent = new NetworkObjectEvent();

        public int uniqueID { get; private set; }
        public int prefabID { get; private set; }
        public NetworkObject data { get; private set; }

        public void Initialize(int newUniqueID, int newPrefabID, NetworkObject newData)
        {
            uniqueID = newUniqueID;
            prefabID = newPrefabID;

            data = new NetworkObject();
            ReceiveUpdate(newData);
        }

        public void ReceiveUpdate(NetworkObject newData)
        {
            List<string> keys = new List<string>(newData.bools.Keys);
            foreach (string key in keys)
            {
                if (data.bools.ContainsKey(key))
                {
                    data.bools[key] = newData.bools[key];
                }
                else
                {
                    data.bools.Add(key, newData.bools[key]);
                }
            }

            keys = new List<string>(newData.ints.Keys);
            foreach (string key in keys)
            {
                if (data.ints.ContainsKey(key))
                {
                    data.ints[key] = newData.ints[key];
                }
                else
                {
                    data.ints.Add(key, newData.ints[key]);
                }
            }

            keys = new List<string>(newData.floats.Keys);
            foreach (string key in keys)
            {
                if (data.floats.ContainsKey(key))
                {
                    data.floats[key] = newData.floats[key];
                }
                else
                {
                    data.floats.Add(key, newData.floats[key]);
                }
            }

            keys = new List<string>(newData.strings.Keys);
            foreach (string key in keys)
            {
                if (data.strings.ContainsKey(key))
                {
                    data.strings[key] = newData.strings[key];
                }
                else
                {
                    data.strings.Add(key, newData.strings[key]);
                }
            }

            receiveUpdateEvent.Invoke(newData);
        }
    }
}
