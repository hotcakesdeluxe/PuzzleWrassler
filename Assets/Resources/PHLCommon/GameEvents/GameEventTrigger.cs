using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace PHL.Common.GameEvents
{
    [System.Serializable]
    public class GameEventTrigger
    {
        [System.Serializable]
        public class BoolIDPair
        {
            public string id;
            public bool value;
        }

        [System.Serializable]
        public class IntIDPair
        {
            public string id;
            public int value;
        }

        [System.Serializable]
        public class FloatIDPair
        {
            public string id;
            public float value;
        }

        [System.Serializable]
        public class StringIDPair
        {
            public string id;
            public string value;
        }

        [System.Serializable]
        public class ObjectIDPair
        {
            public string id;
            public Object value;
        }

        public enum TimeType
        {
            Unscaled,
            Scaled
        }

        public GameEventData gameEventData;
        public float delay;
        public TimeType delayType;
        public List<BoolIDPair> bools = new List<BoolIDPair>();
        public List<IntIDPair> ints = new List<IntIDPair>();
        public List<FloatIDPair> floats = new List<FloatIDPair>();
        public List<StringIDPair> strings = new List<StringIDPair>();
        public List<ObjectIDPair> objects = new List<ObjectIDPair>();

        public void Trigger(bool useDelay = true)
        {
            GameEventInfo gameEventInfo = new GameEventInfo();

            foreach (BoolIDPair boolIDPair in bools)
            {
                gameEventInfo.PutBool(boolIDPair.id, boolIDPair.value);
            }

            foreach (IntIDPair intIDPair in ints)
            {
                gameEventInfo.PutInt(intIDPair.id, intIDPair.value);
            }

            foreach (FloatIDPair floatIDPair in floats)
            {
                gameEventInfo.PutFloat(floatIDPair.id, floatIDPair.value);
            }

            foreach (StringIDPair stringIDPair in strings)
            {
                gameEventInfo.PutString(stringIDPair.id, stringIDPair.value);
            }

            foreach (ObjectIDPair objectIDPair in objects)
            {
                gameEventInfo.PutObject(objectIDPair.id, objectIDPair.value);
            }

            if (delay >= 0.001f && useDelay)
            {
                GameEventManager.ExecuteTriggerList(new List<GameEventTrigger>() { this });
            }
            else
            {
                GameEventManager.Invoke(gameEventData, gameEventInfo);
            }
        }
        
        public void ClearOldCache()
        {
            for (int i = bools.Count - 1; i >= 0; i--)
            {
                if (!gameEventData.eventBools.Contains(bools[i].id))
                {
                    bools.RemoveAt(i);
                }
            }

            for (int i = ints.Count - 1; i >= 0; i--)
            {
                if (!gameEventData.eventInts.Contains(ints[i].id))
                {
                    ints.RemoveAt(i);
                }
            }

            for (int i = floats.Count - 1; i >= 0; i--)
            {
                if (!gameEventData.eventFloats.Contains(floats[i].id))
                {
                    floats.RemoveAt(i);
                }
            }

            for (int i = strings.Count - 1; i >= 0; i--)
            {
                if (!gameEventData.eventStrings.Contains(strings[i].id))
                {
                    strings.RemoveAt(i);
                }
            }

            for (int i = objects.Count - 1; i >= 0; i--)
            {
                if (gameEventData.eventObjects.Find(x => x.objectID == objects[i].id) == null)
                {
                    objects.RemoveAt(i);
                }
            }
        }

#if UNITY_EDITOR
        public static void DrawInspector(GameEventTrigger gameEventTrigger)
        {
            gameEventTrigger.gameEventData = (GameEventData)EditorGUILayout.ObjectField("Game Event", gameEventTrigger.gameEventData, typeof(GameEventData), false);

            if (gameEventTrigger.gameEventData != null)
            {
                if (gameEventTrigger.gameEventData.eventBools.Count > 0)
                {
                    GUILayout.Label("Bools", EditorStyles.boldLabel);
                }

                foreach (string boolID in gameEventTrigger.gameEventData.eventBools)
                {
                    if (gameEventTrigger.bools == null)
                    {
                        gameEventTrigger.bools = new List<BoolIDPair>();
                    }

                    BoolIDPair pair = gameEventTrigger.bools.Find(x => x.id == boolID);

                    if (pair == null)
                    {
                        pair = new BoolIDPair()
                        {
                            id = boolID,
                            value = false
                        };

                        gameEventTrigger.bools.Add(pair);
                    }

                    pair.value = EditorGUILayout.Toggle("   " + pair.id, pair.value);
                }

                if (gameEventTrigger.gameEventData.eventInts.Count > 0)
                {
                    GUILayout.Label("Ints", EditorStyles.boldLabel);
                }

                foreach (string intID in gameEventTrigger.gameEventData.eventInts)
                {
                    if (gameEventTrigger.ints == null)
                    {
                        gameEventTrigger.ints = new List<IntIDPair>();
                    }

                    IntIDPair pair = gameEventTrigger.ints.Find(x => x.id == intID);

                    if (pair == null)
                    {
                        pair = new IntIDPair()
                        {
                            id = intID,
                            value = 0
                        };

                        gameEventTrigger.ints.Add(pair);
                    }

                    pair.value = EditorGUILayout.IntField("   " + pair.id, pair.value);
                }

                if (gameEventTrigger.gameEventData.eventFloats.Count > 0)
                {
                    GUILayout.Label("Floats", EditorStyles.boldLabel);
                }

                foreach (string floatID in gameEventTrigger.gameEventData.eventFloats)
                {
                    if (gameEventTrigger.floats == null)
                    {
                        gameEventTrigger.floats = new List<FloatIDPair>();
                    }

                    FloatIDPair pair = gameEventTrigger.floats.Find(x => x.id == floatID);

                    if (pair == null)
                    {
                        pair = new FloatIDPair()
                        {
                            id = floatID,
                            value = 0
                        };

                        gameEventTrigger.floats.Add(pair);
                    }

                    pair.value = EditorGUILayout.FloatField("   " + pair.id, pair.value);
                }

                if (gameEventTrigger.gameEventData.eventStrings.Count > 0)
                {
                    GUILayout.Label("Strings", EditorStyles.boldLabel);
                }

                foreach (string stringID in gameEventTrigger.gameEventData.eventStrings)
                {
                    if (gameEventTrigger.strings == null)
                    {
                        gameEventTrigger.strings = new List<StringIDPair>();
                    }

                    StringIDPair pair = gameEventTrigger.strings.Find(x => x.id == stringID);

                    if (pair == null)
                    {
                        pair = new StringIDPair()
                        {
                            id = stringID,
                            value = ""
                        };

                        gameEventTrigger.strings.Add(pair);
                    }

                    pair.value = EditorGUILayout.TextField("   " + pair.id, pair.value);
                }

                if (gameEventTrigger.gameEventData.eventObjects.Count > 0)
                {
                    GUILayout.Label("Objects", EditorStyles.boldLabel);
                }

                foreach (GameEventData.ObjectIDTypePair objectIDTypePair in gameEventTrigger.gameEventData.eventObjects)
                {
                    if (gameEventTrigger.objects == null)
                    {
                        gameEventTrigger.objects = new List<ObjectIDPair>();
                    }

                    ObjectIDPair pair = gameEventTrigger.objects.Find(x => x.id == objectIDTypePair.objectID);

                    if (pair == null)
                    {
                        pair = new ObjectIDPair()
                        {
                            id = objectIDTypePair.objectID,
                            value = null
                        };

                        gameEventTrigger.objects.Add(pair);
                    }

                    pair.value = EditorGUILayout.ObjectField("   " + objectIDTypePair.objectID, pair.value, GameEventData.TypeFromEnum(objectIDTypePair.type), true);
                }
            }
        }
#endif
    }
}