using UnityEngine;

namespace PHL.Common.Utility
{
    [CreateAssetMenu(fileName = "AnimationCurves", menuName = "PHL/AnimationCurves", order = 0)]
    public class AnimationCurves : ScriptableObjectSingleton<AnimationCurves>
    {
        public AnimationCurve ease;
        public AnimationCurve easeIn;
        public AnimationCurve easeOut;
        public AnimationCurve pingPong;
        public AnimationCurve bounce;
    }
}
