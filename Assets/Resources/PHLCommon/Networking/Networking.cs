using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using PHL.Common.Utility;
using System;
using System.Text;

namespace PHL.Common.GenericNetworking
{
    public class UserEvent : UnityEvent<NetworkUser> { }
    public class NetworkUserStateEvent : UnityEvent<NetworkUser, NetworkObject> { }
    public class RoomEvent : SecureEvent<NetworkRoom> { }
    public class RoomListEvent : SecureEvent<List<NetworkRoom>> { }
    public class NetworkMessageEvent : SecureEvent<NetworkMessage> { }
    public class NetworkObjectEvent : SecureEvent<NetworkObject> { }

    public class Networking
    {
        public static bool connected { get; private set; }
        public static NetworkUser myUser { get; private set; }
        public static NetworkRoom myRoom { get; private set; }
        private static List<NetworkMessage> _globalCustomMessageQueue = new List<NetworkMessage>();
        private static IEnumerator _globalCustomMessageQueueRoutine;
        private static Dictionary<string, HashSet<Action<NetworkMessage>>> _customMessageHandlers = new Dictionary<string, HashSet<Action<NetworkMessage>>>();

        //Server request events
        public static NetworkMessageEvent sendCustomRequestEvent = new NetworkMessageEvent();
        public static SecureEvent sendConnectRequestEvent = new SecureEvent();
        public static SecureEvent sendDisconnectRequestEvent = new SecureEvent();
        public static SecureEvent sendRoomListRequestEvent = new SecureEvent();
        public static RoomEvent sendRoomJoinRequestEvent = new RoomEvent();
        public static SecureEvent sendRoomLeaveRequestEvent = new SecureEvent();

        //Server response events
        public static SecureEvent initializeResponseEvent = new SecureEvent();
        public static UserEvent connectResponseEvent = new UserEvent();
        public static StringEvent disconnectResponseEvent = new StringEvent();
        public static RoomListEvent roomListResponseEvent = new RoomListEvent();
        public static RoomEvent joinRoomResponseEvent = new RoomEvent();
        public static SecureEvent leaveRoomResponseEvent = new SecureEvent();
        public static NetworkMessageEvent customMessageResponseEvent = new NetworkMessageEvent();

        private static bool _showMessagesPerSecond = false;
        private static bool _showMessages;
        private static bool _showFullMessages;

        private static int _numPerSecond;
        private static float _secondCounter;
        private static string _messageString;

        public static void InitializeNetwork()
        {
            AddEventListeners();
            initializeResponseEvent.Invoke();
            CoroutineRunner.RunCoroutine(DebugLogRoutine());
        }

        private static void AddEventListeners()
        {
            connectResponseEvent.AddListener(OnConnect);
            disconnectResponseEvent.AddListener(OnDisconnect);
            joinRoomResponseEvent.AddListener(OnJoinRoom);
            leaveRoomResponseEvent.AddListener(OnLeaveRoom);
            roomListResponseEvent.AddListener(OnRoomListReceived);
            customMessageResponseEvent.AddListener(OnCustomMessage);
        }

        private static void RemoveEventListeners()
        {
            connectResponseEvent.RemoveListener(OnConnect);
            disconnectResponseEvent.RemoveListener(OnDisconnect);
            joinRoomResponseEvent.RemoveListener(OnJoinRoom);
            leaveRoomResponseEvent.RemoveListener(OnLeaveRoom);
            roomListResponseEvent.RemoveListener(OnRoomListReceived);
            customMessageResponseEvent.RemoveListener(OnCustomMessage);
        }

        public static void Connect()
        {
            sendConnectRequestEvent.Invoke();
        }

        private static void OnConnect(NetworkUser user)
        {
            myUser = user;
            StartGlobalCustomMessageQueue();

            sendRoomListRequestEvent.Invoke();
        }

        private static void OnDisconnect(string reason)
        {
            myUser = null;
            connected = false;
            StopGlobalCustomMessageQueue(true);
        }

        private static void OnRoomListReceived(List<NetworkRoom> rooms)
        {
            sendRoomJoinRequestEvent.Invoke(rooms[0]);
        }

        private static void OnJoinRoom(NetworkRoom room)
        {
            myRoom = room;
            connected = true;
            Debug.Log("Room joined!");
        }

        private static void OnLeaveRoom()
        {
            myRoom.roomUsers.Clear();
            myRoom = null;
            myUser.LeaveRoom();
        }

        public static void SetUser(NetworkUser user)
        {
            myUser = user;
        }

        public static void AddCustomMessageReceiver(string messageID, Action<NetworkMessage> action)
        {
            if(!_customMessageHandlers.ContainsKey(messageID))
            {
                _customMessageHandlers.Add(messageID, new HashSet<Action<NetworkMessage>>());
            }

            _customMessageHandlers[messageID].Add(action);
        }

        private static void OnCustomMessage(NetworkMessage message)
        {
            if (_customMessageHandlers.ContainsKey(message.messageID))
            {
                foreach(Action<NetworkMessage> action in _customMessageHandlers[message.messageID])
                {
                    action.Invoke(message);
                }
            }
        }

        public static void SendCustomMessage(NetworkMessage message)
        {
            if (_showMessagesPerSecond)
            {
                _numPerSecond++;

                if (_showMessages)
                {
                    if (_showFullMessages)
                    {
                        _messageString += message.ToString() + "\n";
                    }
                    else
                    {
                        _messageString += message.messageID + ", ";
                    }
                }
            }

            sendCustomRequestEvent.Invoke(message);
        }

        public static void ExecuteCustomMessageQueue(List<NetworkMessage> messages, float delay)
        {
            IEnumerator newQueue = ExecuteCustomMessageQueueRoutine(messages, delay);
            CoroutineRunner.RunCoroutine(newQueue);
        }

        private static IEnumerator ExecuteCustomMessageQueueRoutine(List<NetworkMessage> messages, float delay)
        {
            foreach (NetworkMessage message in messages)
            {
                SendCustomMessage(message);
                yield return new WaitForSeconds(delay);
            }
        }

        private static void StartGlobalCustomMessageQueue()
        {
            StopGlobalCustomMessageQueue();

            _globalCustomMessageQueueRoutine = GlobalCustomMessageQueueRoutine();
            CoroutineRunner.RunCoroutine(_globalCustomMessageQueueRoutine);
        }

        private static void StopGlobalCustomMessageQueue(bool clearQueue = false)
        {
            if (_globalCustomMessageQueueRoutine != null)
            {
                CoroutineRunner.StopCoroutine(_globalCustomMessageQueueRoutine);
                _globalCustomMessageQueueRoutine = null;
            }

            if (clearQueue)
            {
                _globalCustomMessageQueue.Clear();
            }
        }

        private static IEnumerator GlobalCustomMessageQueueRoutine()
        {
            if (_globalCustomMessageQueue == null)
            {
                _globalCustomMessageQueue = new List<NetworkMessage>();
            }

            while (true)
            {
                if (_globalCustomMessageQueue.Count > 0)
                {
                    SendCustomMessage(_globalCustomMessageQueue[0]);
                    _globalCustomMessageQueue.RemoveAt(0);
                }

                yield return new WaitForSeconds(0.05f);
            }
        }

        public static void AddToGlobalCustomMessageQueue(NetworkMessage message)
        {
            _globalCustomMessageQueue.Add(message);
        }

        private static IEnumerator DebugLogRoutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(1f);

                if (_showMessagesPerSecond)
                {
                    Debug.Log("Messages in the last second: " + _numPerSecond);

                    if (_showMessages)
                    {
                        Debug.Log("Messages: " + _messageString);
                    }
                }

                _numPerSecond = 0;
                _messageString = "";
            }
        }
    }
    
    public class NetworkRoom
    {
        public int roomID { get; private set; }
        public string roomName { get; private set; }
        public List<NetworkUser> roomUsers { get; private set; }
        public NetworkObject publicParameters { get; private set; }   //Seen from lobby
        public NetworkObject privateParameters { get; private set; }  //Seen from inside room

        public UserEvent userJoinEvent = new UserEvent();
        public UserEvent userLeaveEvent = new UserEvent();

        public NetworkRoom(int newRoomID, string newRoomName)
        {
            roomID = newRoomID;
            roomName = newRoomName;
            publicParameters = new NetworkObject();
            privateParameters = new NetworkObject();
        }

        public NetworkUser GetUserByID(int id)
        {
            return roomUsers.Find(x => x.userID == id);
        }

        public void SetUsers(List<NetworkUser> users)
        {
            roomUsers = users;
        }

        public void UserJoin(NetworkUser user)
        {
            roomUsers.Add(user);
            userJoinEvent.Invoke(user);
        }

        public void UserLeave(NetworkUser user)
        {
            roomUsers.Remove(user);
            userLeaveEvent.Invoke(user);
        }
    }
    
    public class NetworkMessage
    {
        public string messageID;
        public NetworkObject parameters;

        public NetworkMessage(string newMessageID)
        {
            messageID = newMessageID;
            parameters = new NetworkObject();
        }

        public override string ToString()
        {
            string s = messageID + ": " + parameters.ToString();
            return s;
        }
    }

    public class NetworkObject
    {
        public Dictionary<string, bool> bools;
        public Dictionary<string, int> ints;
        public Dictionary<string, float> floats;
        public Dictionary<string, string> strings;
        public Dictionary<string, NetworkObject> networkObjects;

        public Dictionary<string, bool[]> boolArrays;
        public Dictionary<string, int[]> intArrays;
        public Dictionary<string, float[]> floatArrays;
        public Dictionary<string, string[]> stringArrays;
        public Dictionary<string, NetworkObject[]> networkObjectArrays;

        public NetworkObject()
        {
            bools = new Dictionary<string, bool>();
            ints = new Dictionary<string, int>();
            floats = new Dictionary<string, float>();
            strings = new Dictionary<string, string>();
            networkObjects = new Dictionary<string, NetworkObject>();

            boolArrays = new Dictionary<string, bool[]>();
            intArrays = new Dictionary<string, int[]>();
            floatArrays = new Dictionary<string, float[]>();
            stringArrays = new Dictionary<string, string[]>();
            networkObjectArrays = new Dictionary<string, NetworkObject[]>();
        }
        
        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            if (bools.Count > 0)
            {
                stringBuilder.Append("Bools: ");

                foreach (KeyValuePair<string, bool> keyValuePair in bools)
                {
                    stringBuilder.Append("[\"" + keyValuePair.Key + "\", " + keyValuePair.Value.ToString() + "] ");
                }
            }

            if (ints.Count > 0)
            {
                stringBuilder.Append("Ints: ");

                foreach (KeyValuePair<string, int> keyValuePair in ints)
                {
                    stringBuilder.Append("[\"" + keyValuePair.Key + "\", " + keyValuePair.Value.ToString() + "] ");
                }
            }

            if (floats.Count > 0)
            {
                stringBuilder.Append("Floats: ");

                foreach (KeyValuePair<string, float> keyValuePair in floats)
                {
                    stringBuilder.Append("[\"" + keyValuePair.Key + "\", " + keyValuePair.Value.ToString() + "] ");
                }
            }

            if (strings.Count > 0)
            {
                stringBuilder.Append("Strings: ");

                foreach (KeyValuePair<string, string> keyValuePair in strings)
                {
                    stringBuilder.Append("[\"" + keyValuePair.Key + "\", \"" + keyValuePair.Value.ToString() + "\"] ");
                }
            }

            if (networkObjects.Count > 0)
            {
                stringBuilder.Append("Network Objects: ");

                foreach (KeyValuePair<string, NetworkObject> keyValuePair in networkObjects)
                {
                    stringBuilder.Append("[\"" + keyValuePair.Key + "\", \"" + networkObjects[keyValuePair.Key].ToString() + "\"] ");
                }
            }


            if (boolArrays.Count > 0)
            {
                stringBuilder.Append("Bool Arrays: ");

                foreach (KeyValuePair<string, bool[]> keyValuePair in boolArrays)
                {
                    foreach (bool boolValue in keyValuePair.Value)
                    {
                        stringBuilder.Append("[\"" + keyValuePair.Key + "\", \"" + boolValue.ToString() + "\"], ");
                    }
                }
            }

            if (intArrays.Count > 0)
            {
                stringBuilder.Append("Int Arrays: ");

                foreach (KeyValuePair<string, int[]> keyValuePair in intArrays)
                {
                    foreach (int intValue in keyValuePair.Value)
                    {
                        stringBuilder.Append("[\"" + keyValuePair.Key + "\", \"" + intValue.ToString() + "\"], ");
                    }
                }
            }

            if (floatArrays.Count > 0)
            {
                stringBuilder.Append("Float Arrays: ");

                foreach (KeyValuePair<string, float[]> keyValuePair in floatArrays)
                {
                    foreach (float floatValue in keyValuePair.Value)
                    {
                        stringBuilder.Append("[\"" + keyValuePair.Key + "\", \"" + floatValue.ToString() + "\"], ");
                    }
                }
            }

            if (stringArrays.Count > 0)
            {
                stringBuilder.Append("String Arrays: ");

                foreach (KeyValuePair<string, string[]> keyValuePair in stringArrays)
                {
                    foreach (string stringValue in keyValuePair.Value)
                    {
                        stringBuilder.Append("[\"" + keyValuePair.Key + "\", \"" + stringValue.ToString() + "\"], ");
                    }
                }
            }

            if (networkObjectArrays.Count > 0)
            {
                stringBuilder.Append("Network Object Arrays: ");

                foreach (KeyValuePair<string, NetworkObject[]> keyValuePair in networkObjectArrays)
                {
                    foreach(NetworkObject networkObject in keyValuePair.Value)
                    {
                        stringBuilder.Append("[\"" + keyValuePair.Key + "\", \"" + networkObject.ToString() + "\"], ");
                    }
                }
            }

            return stringBuilder.ToString();
        }
        
        public void CombineWithNetworkObject(NetworkObject objectToAdd, bool overrideValues = true)
        {
            foreach (KeyValuePair<string, bool> keyValuePair in objectToAdd.bools)
            {
                if (bools.ContainsKey(keyValuePair.Key))
                {
                    if (overrideValues)
                    {
                        bools[keyValuePair.Key] = keyValuePair.Value;
                    }
                }
                else
                {
                    bools.Add(keyValuePair.Key, keyValuePair.Value);
                }
            }

            foreach (KeyValuePair<string, int> keyValuePair in objectToAdd.ints)
            {
                if (ints.ContainsKey(keyValuePair.Key))
                {
                    if (overrideValues)
                    {
                        ints[keyValuePair.Key] = keyValuePair.Value;
                    }
                }
                else
                {
                    ints.Add(keyValuePair.Key, keyValuePair.Value);
                }
            }

            foreach (KeyValuePair<string, float> keyValuePair in objectToAdd.floats)
            {
                if (floats.ContainsKey(keyValuePair.Key))
                {
                    if (overrideValues)
                    {
                        floats[keyValuePair.Key] = keyValuePair.Value;
                    }
                }
                else
                {
                    floats.Add(keyValuePair.Key, keyValuePair.Value);
                }
            }

            foreach (KeyValuePair<string, string> keyValuePair in objectToAdd.strings)
            {
                if (strings.ContainsKey(keyValuePair.Key))
                {
                    if (overrideValues)
                    {
                        strings[keyValuePair.Key] = keyValuePair.Value;
                    }
                }
                else
                {
                    strings.Add(keyValuePair.Key, keyValuePair.Value);
                }
            }

            foreach (KeyValuePair<string, NetworkObject> keyValuePair in objectToAdd.networkObjects)
            {
                if (networkObjects.ContainsKey(keyValuePair.Key))
                {
                    if (overrideValues)
                    {
                        networkObjects[keyValuePair.Key] = keyValuePair.Value;
                    }
                }
                else
                {
                    networkObjects.Add(keyValuePair.Key, keyValuePair.Value);
                }
            }


            foreach (KeyValuePair<string, bool[]> keyValuePair in objectToAdd.boolArrays)
            {
                if (boolArrays.ContainsKey(keyValuePair.Key))
                {
                    if (overrideValues)
                    {
                        boolArrays[keyValuePair.Key] = keyValuePair.Value;
                    }
                }
                else
                {
                    boolArrays.Add(keyValuePair.Key, keyValuePair.Value);
                }
            }

            foreach (KeyValuePair<string, int[]> keyValuePair in objectToAdd.intArrays)
            {
                if (intArrays.ContainsKey(keyValuePair.Key))
                {
                    if (overrideValues)
                    {
                        intArrays[keyValuePair.Key] = keyValuePair.Value;
                    }
                }
                else
                {
                    intArrays.Add(keyValuePair.Key, keyValuePair.Value);
                }
            }

            foreach (KeyValuePair<string, float[]> keyValuePair in objectToAdd.floatArrays)
            {
                if (floatArrays.ContainsKey(keyValuePair.Key))
                {
                    if (overrideValues)
                    {
                        floatArrays[keyValuePair.Key] = keyValuePair.Value;
                    }
                }
                else
                {
                    floatArrays.Add(keyValuePair.Key, keyValuePair.Value);
                }
            }

            foreach (KeyValuePair<string, string[]> keyValuePair in objectToAdd.stringArrays)
            {
                if (stringArrays.ContainsKey(keyValuePair.Key))
                {
                    if (overrideValues)
                    {
                        stringArrays[keyValuePair.Key] = keyValuePair.Value;
                    }
                }
                else
                {
                    stringArrays.Add(keyValuePair.Key, keyValuePair.Value);
                }
            }

            foreach (KeyValuePair<string, NetworkObject[]> keyValuePair in objectToAdd.networkObjectArrays)
            {
                if (networkObjectArrays.ContainsKey(keyValuePair.Key))
                {
                    if (overrideValues)
                    {
                        networkObjectArrays[keyValuePair.Key] = keyValuePair.Value;
                    }
                }
                else
                {
                    networkObjectArrays.Add(keyValuePair.Key, keyValuePair.Value);
                }
            }
        }
    }
    
    public class NetworkUser
    {
        public int userID { get; private set; }
        public string userName { get; private set; }
        public bool isMe { get; private set; }
        public bool isLead { get; private set; }
        public NetworkObject userData { get; private set; }
        public NetworkObjectEvent receiveUpdateEvent = new NetworkObjectEvent();

        public NetworkUser(int newUserID, string newUserName)
        {
            userID = newUserID;
            userName = newUserName;
            userData = new NetworkObject();
        }

        public void CheckIsLead(int leadID)
        {
            isLead = userID == leadID;
        }

        public void CheckIsMe(int meID)
        {
            isMe = userID == meID;
        }
        
        public void ReceiveUpdate(NetworkObject newData)
        {
            List<string> keys = new List<string>(newData.bools.Keys);
            for (int k = 0; k < keys.Count; k++)
            {
                string key = keys[k];
                if (userData.bools.ContainsKey(key))
                {
                    userData.bools[key] = newData.bools[key];
                }
                else
                {
                    userData.bools.Add(key, newData.bools[key]);
                }
            }

            keys = new List<string>(newData.ints.Keys);
            for (int k = 0; k < keys.Count; k++)
            {
                string key = keys[k];
                if (userData.ints.ContainsKey(key))
                {
                    userData.ints[key] = newData.ints[key];
                }
                else
                {
                    userData.ints.Add(key, newData.ints[key]);
                }
            }

            keys = new List<string>(newData.floats.Keys);
            for (int k = 0; k < keys.Count; k++)
            {
                string key = keys[k];
                if (userData.floats.ContainsKey(key))
                {
                    userData.floats[key] = newData.floats[key];
                }
                else
                {
                    userData.floats.Add(key, newData.floats[key]);
                }
            }

            keys = new List<string>(newData.strings.Keys);
            for (int k = 0; k < keys.Count; k++)
            {
                string key = keys[k];
                if (userData.strings.ContainsKey(key))
                {
                    userData.strings[key] = newData.strings[key];
                }
                else
                {
                    userData.strings.Add(key, newData.strings[key]);
                }
            }

            keys = new List<string>(newData.networkObjects.Keys);
            for (int k = 0; k < keys.Count; k++)
            {
                string key = keys[k];
                if (userData.networkObjects.ContainsKey(key))
                {
                    userData.networkObjects[key] = newData.networkObjects[key];
                }
                else
                {
                    userData.networkObjects.Add(key, newData.networkObjects[key]);
                }
            }

            keys = new List<string>(newData.boolArrays.Keys);
            for (int k = 0; k < keys.Count; k++)
            {
                string key = keys[k];
                if (userData.boolArrays.ContainsKey(key))
                {
                    userData.boolArrays[key] = newData.boolArrays[key];
                }
                else
                {
                    userData.boolArrays.Add(key, newData.boolArrays[key]);
                }
            }

            keys = new List<string>(newData.intArrays.Keys);
            for (int k = 0; k < keys.Count; k++)
            {
                string key = keys[k];
                if (userData.intArrays.ContainsKey(key))
                {
                    userData.intArrays[key] = newData.intArrays[key];
                }
                else
                {
                    userData.intArrays.Add(key, newData.intArrays[key]);
                }
            }

            keys = new List<string>(newData.floatArrays.Keys);
            for (int k = 0; k < keys.Count; k++)
            {
                string key = keys[k];
                if (userData.floatArrays.ContainsKey(key))
                {
                    userData.floatArrays[key] = newData.floatArrays[key];
                }
                else
                {
                    userData.floatArrays.Add(key, newData.floatArrays[key]);
                }
            }

            keys = new List<string>(newData.stringArrays.Keys);
            for (int k = 0; k < keys.Count; k++)
            {
                string key = keys[k];
                if (userData.stringArrays.ContainsKey(key))
                {
                    userData.stringArrays[key] = newData.stringArrays[key];
                }
                else
                {
                    userData.stringArrays.Add(key, newData.stringArrays[key]);
                }
            }

            keys = new List<string>(newData.networkObjectArrays.Keys);
            for (int k = 0; k < keys.Count; k++)
            {
                string key = keys[k];
                if (userData.networkObjectArrays.ContainsKey(key))
                {
                    userData.networkObjectArrays[key] = newData.networkObjectArrays[key];
                }
                else
                {
                    userData.networkObjectArrays.Add(key, newData.networkObjectArrays[key]);
                }
            }

            receiveUpdateEvent.Invoke(newData);
        }

        public void LeaveRoom()
        {
            userID = -1;
        }
    }
}