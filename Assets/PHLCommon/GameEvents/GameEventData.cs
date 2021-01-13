using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;
using UnityEngine.Events;
using PHL.Texto;

namespace PHL.Common.GameEvents
{
    [CreateAssetMenu(fileName = "GameEvent", menuName = "PHL/Game Event", order = -1)]
    public class GameEventData : ScriptableObject
    {
        //#### Add new object types here ####

        public enum SupportedObjectType
        {
            GameObject,
            Position,
            Texto,
            Sprite,
            CustomValue
        }

        private static Type[] typeArray =
        {
            typeof(GameObject),
            typeof(PositionData),
            typeof(TextoData),
            typeof(Sprite),
            typeof(CustomValueData)
        };

        //##################################

        [Serializable]
        public class ObjectIDTypePair
        {
            public string objectID;
            public SupportedObjectType type;
        }
        
        public List<string> eventBools;
        public List<string> eventInts;
        public List<string> eventFloats;
        public List<string> eventStrings;
        public List<ObjectIDTypePair> eventObjects;
        public bool fmod;
        
        public static Type TypeFromEnum(SupportedObjectType typeEnum)
        {
            return typeArray[(int)(typeEnum)];
        }

        public Type TypeFromID(string id)
        {
            foreach(ObjectIDTypePair eventObject in eventObjects)
            {
                if(eventObject.objectID == id)
                {
                    return TypeFromEnum(eventObject.type);
                }
            }

            return null;
        }

        public void AddListener(UnityAction<GameEventInfo> action)
        {
            GameEventManager.AddListener(this, action);
        }
    }
}