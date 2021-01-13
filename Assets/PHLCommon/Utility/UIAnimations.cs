using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace PHL.Common.Utility
{
    public class UIAnimations
    {
        public static IEnumerator Fade(CanvasGroup canvas, AnimationCurve curve, float start, float finish, float speed, bool unscaled = false, Action callback = null)
        {
            float timer = 0;
            while(timer < 1)
            {
                timer += GetTime(unscaled, speed);
                canvas.alpha = ExtraMath.Map(curve.Evaluate(timer), 0, 1, start, finish);
                yield return 0f;
            }

            callback?.Invoke();
        }

        public static IEnumerator Fade(Image image, AnimationCurve curve, float start, float finish, float speed, Action callback = null)
        {
            float timer = 0;
            Color tempColor = image.color;

            while (timer < 1)
            {
                timer += Time.deltaTime * speed;
                tempColor.a = ExtraMath.Map(curve.Evaluate(timer), 0, 1, start, finish);
                image.color = tempColor;
                yield return 0f;
            }

            callback?.Invoke();
        }

        public static IEnumerator Fade(SpriteRenderer sprite, AnimationCurve curve, float start, float finish, float speed, Action callback = null)
        {
            float timer = 0;
            Color tempColor = sprite.color;

            while (timer < 1)
            {
                timer += Time.deltaTime * speed;
                tempColor.a = ExtraMath.Map(curve.Evaluate(timer), 0, 1, start, finish);
                sprite.color = tempColor;
                yield return 0f;
            }

            callback?.Invoke();
        }

        public static IEnumerator MoveFadeScale(CanvasGroup canvas, RectTransform rect, AnimationCurve curve, float startAlpha, float endAlpha, Vector2 distanceToMove, Vector3 startScale, Vector3 endScale, float speed, Action callback = null)
        {
            float timer = 0;
            Vector2 startPos = rect.anchoredPosition;

            while (timer < 1)
            {
                timer += Time.deltaTime * speed;
                canvas.alpha = ExtraMath.Map(curve.Evaluate(timer), 0, 1, startAlpha, endAlpha);
                rect.anchoredPosition = Vector2.Lerp(startPos + distanceToMove, startPos, curve.Evaluate(timer));
                rect.localScale = Vector3.Lerp(startScale, endScale, curve.Evaluate(timer));
                yield return 0f;
            }

            callback?.Invoke();
        }

        public static IEnumerator Scale(Transform target, AnimationCurve curve, Vector3 start, Vector3 finish, float speed, bool unscaled = false, Action callback = null)
        {
            float timer = 0;
            while (timer < 1)
            {
                timer += GetTime(unscaled, speed);
                target.localScale = Vector3.Lerp(start, finish, curve.Evaluate(timer));
                yield return 0f;
            }

            callback?.Invoke();
        }

        public static IEnumerator Move(Transform target, AnimationCurve curve, Vector3 start, Vector3 finish, float speed, Action callback = null)
        {
            float timer = 0;
            while (timer < 1)
            {
                timer += Time.deltaTime * speed;
                target.localPosition = Vector3.Lerp(start, finish, curve.Evaluate(timer));
                yield return 0f;
            }

            callback?.Invoke();
        }

        public static IEnumerator LerpColor(Image image, AnimationCurve curve, Color start, Color finish, float speed, Action callback = null)
        {
            float timer = 0;
            while (timer < 1)
            {
                timer += Time.deltaTime * speed;
                image.color = Color.Lerp(start, finish, curve.Evaluate(timer));
                yield return 0f;
            }

            callback?.Invoke();
        }

        private static float GetTime(bool unscaled, float speed)
        {
            if (unscaled)
            {
                return Time.unscaledDeltaTime * speed;
            }
            else
            {
                return Time.deltaTime * speed;
            }
        }
    }
}
