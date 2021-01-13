using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PHL.Common.Utility;

namespace PHL.Common.GameEvents
{
    [CreateAssetMenu(fileName = "GameEventDatabase", menuName = "PHL/Singletons/GameEvent Database", order = 0)]
    public class GameEventDatabase : ScriptableObjectSingleton<GameEventDatabase>
    {
        [Header("Event Data")]
        public GameEventData quitEvent;
        public GameEventData startConversationEvent;
        public GameEventData playSimplerAnimationEvent;
        public GameEventData pushRubberBandAnim;
        public GameEventData enableOrDisableObjectEvent;
        public GameEventData checkpointRechedEvent;
        public GameEventData checkpointDestroyedEvent;
        public GameEventData destroyObjectEvent;
        public GameEventData instantiateObjectEvent;
        public GameEventData playParticleSystemEvent;
        public GameEventData stopParticleSystemEvent;
        public GameEventData setCustomValueEvent;
        public GameEventData addToCustomValueEvent;

        [Header("Event Triggers")]
        public GameEventTrigger fadeToBlackTrigger;
        public GameEventTrigger fadeFromBlackTrigger;
    }
}