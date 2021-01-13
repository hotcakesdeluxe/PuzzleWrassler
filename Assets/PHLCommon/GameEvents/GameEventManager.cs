using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using PHL.Common.Utility;
using System;

namespace PHL.Common.GameEvents
{
    public class GameEvent : SecureEvent<GameEventInfo> { }

    public static class GameEventManager
    {
        private static Dictionary<GameEventData, GameEvent> _gameEventsBacking;
        private static Dictionary<GameEventData, GameEvent> _gameEvents
        {
            get
            {
                if(_gameEventsBacking == null)
                {
                    _gameEventsBacking = new Dictionary<GameEventData, GameEvent>();
                }

                return _gameEventsBacking;
            }
        }

        public static void AddListener(GameEventData gameEvent, UnityAction<GameEventInfo> action)
        {
            if(!_gameEvents.ContainsKey(gameEvent))
            {
                _gameEvents.Add(gameEvent, new GameEvent());
            }

            _gameEvents[gameEvent].AddListener(action);
        }

        public static void RemoveListener(GameEventData gameEvent, UnityAction<GameEventInfo> action)
        {
            if (_gameEvents.ContainsKey(gameEvent))
            {
                _gameEvents[gameEvent].RemoveListener(action);
            }
        }

        public static void Invoke(GameEventData gameEvent, GameEventInfo info)
        {
            if(_gameEvents.ContainsKey(gameEvent))
            {
                _gameEvents[gameEvent].Invoke(info);
            }
        }

        public static void ExecuteTriggerList(List<GameEventTrigger> triggers)
        {
            if (triggers != null && triggers.Count > 0)
            {
                CoroutineRunner.RunCoroutine(ExecuteTriggerListRoutine(triggers, null));
            }
        }

        public static void ExecuteTriggerList(List<GameEventTrigger> triggers, Action action)
        {
            if (triggers != null && triggers.Count > 0)
            {
                CoroutineRunner.RunCoroutine(ExecuteTriggerListRoutine(triggers, action));
            }
        }

        //Extension method
        public static void Execute(this List<GameEventTrigger> triggers)
        {
            ExecuteTriggerList(triggers, null);
        }

        public static void Execute(this List<GameEventTrigger> triggers, Action callback)
        {
            ExecuteTriggerList(triggers, callback);
        }

        private static IEnumerator ExecuteTriggerListRoutine(List<GameEventTrigger> triggers, Action action)
        {
            foreach (GameEventTrigger trigger in triggers)
            {
                if (trigger.delay >= 0.001f)
                {
                    if (trigger.delayType == GameEventTrigger.TimeType.Unscaled)
                    {
                        yield return new WaitForSecondsRealtime(trigger.delay);
                    }
                    else if(trigger.delayType == GameEventTrigger.TimeType.Scaled)
                    {
                        yield return new WaitForSeconds(trigger.delay);
                    }
                }
                
                trigger.Trigger(false);
            }

            action?.Invoke();
        }
    }
}