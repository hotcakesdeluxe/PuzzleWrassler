using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PHL.Common.GenericNetworking
{
    [System.Serializable]
    public class NetworkItemPrefabIDPair
    {
        public int prefabID;
        public GameObject prefab;
    }

    public class NetworkItemManager : MonoBehaviour
    {
        public static NetworkItemManager instance;
        private void Awake()
        {
            instance = this;
        }

        [SerializeField] private List<NetworkItemPrefabIDPair> _prefabIDPairs;

        private List<NetworkItem> _items;

        public NetworkItem GetItemByUniqueID(int uniqueID)
        {
            return _items.Find(x => x.uniqueID == uniqueID);
        }

        public void InitializeItemManager()
        {
            _items = new List<NetworkItem>();
        }

        private void Update()
        {
            CleanItemList();
        }

        private void CleanItemList()
        {
            for (int i = _items.Count - 1; i >= 0; i--)
            {
                if (_items[i] == null)
                {
                    _items.RemoveAt(i);
                }
            }
        }

        public void OnSpawnItem(int prefabID, int uniqueID, NetworkObject data)
        {
            GameObject networkItemInstance = Instantiate(_prefabIDPairs.Find(x => x.prefabID == prefabID).prefab, transform) as GameObject;
            NetworkItem networkItem = networkItemInstance.GetComponent<NetworkItem>();
            networkItem.Initialize(uniqueID, prefabID, data);

            _items.Add(networkItem);
        }

        public void OnDestroyItem(NetworkItem networkItem)
        {
            if (_items.Contains(networkItem))
            {
                _items.Remove(networkItem);
            }

            Destroy(networkItem.gameObject);
        }

        public void OnNetworkItemUpdate(NetworkItem item, NetworkObject data)
        {
            if (item == null)
            {
                return;
            }

            item.ReceiveUpdate(data);
        }
    }
}