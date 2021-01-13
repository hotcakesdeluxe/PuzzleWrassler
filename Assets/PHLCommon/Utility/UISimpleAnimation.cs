using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PHL.Common.Utility
{
    public class UISimpleAnimation : MonoBehaviour
    {
        [SerializeField] private RectTransform[] _items;
        [SerializeField] private float _length = 1f;
        [SerializeField] private Vector2 _startDelay;
        [SerializeField] private Space _space = Space.Self;
        [SerializeField] private bool _playOnStart;
        [SerializeField] private bool _loop;
        [SerializeField] private bool _unscaledTime;
        [SerializeField] private bool _destroyObjectAtEnd;

        [Header("Position")]
        [SerializeField] private AnimationCurve _xPosition = AnimationCurve.EaseInOut(0, 0, 1, 0);
        [SerializeField] private AnimationCurve _yPosition = AnimationCurve.EaseInOut(0, 0, 1, 0);
        [SerializeField] private AnimationCurve _zPosition = AnimationCurve.EaseInOut(0, 0, 1, 0);

        [Header("Rotation")]
        [SerializeField] private AnimationCurve _xRotation = AnimationCurve.EaseInOut(0, 0, 1, 0);
        [SerializeField] private AnimationCurve _yRotation = AnimationCurve.EaseInOut(0, 0, 1, 0);
        [SerializeField] private AnimationCurve _zRotation = AnimationCurve.EaseInOut(0, 0, 1, 0);

        [Header("Scale")]
        [SerializeField] private AnimationCurve _xScale = AnimationCurve.EaseInOut(0, 1, 1, 1);
        [SerializeField] private AnimationCurve _yScale = AnimationCurve.EaseInOut(0, 1, 1, 1);
        [SerializeField] private AnimationCurve _zScale = AnimationCurve.EaseInOut(0, 1, 1, 1);

        [Header("Alpha")]
        [SerializeField] private bool _animateAlpha;
        [SerializeField] private AnimationCurve _alphaStartEnd = AnimationCurve.EaseInOut(0, 1, 1, 1);

        private List<float> _timers = new List<float>();
        private List<Vector3> _startPositions = new List<Vector3>();
        private List<Vector3> _startRotations = new List<Vector3>();
        private List<Vector3> _startScales = new List<Vector3>();
        private List<CanvasGroup> _cgs = new List<CanvasGroup>();

        public bool playing { get; private set; }

        private void Start()
        {
            foreach (RectTransform rect in _items)
            {
                _startPositions.Add(rect.localPosition);
                _startRotations.Add(rect.localEulerAngles);
                _startScales.Add(rect.localScale);
                _timers.Add(-Random.Range(_startDelay.x, _startDelay.y));

                if (_animateAlpha)
                {
                    if (rect.GetComponent<CanvasGroup>() != null)
                    {
                        _cgs.Add(rect.GetComponent<CanvasGroup>());
                    }
                    else
                    {
                        _cgs.Add(rect.gameObject.AddComponent<CanvasGroup>());
                    }
                    _cgs[_cgs.Count - 1].alpha = _alphaStartEnd.Evaluate(0);
                }
            }

            if (_playOnStart)
            {
                Play();
                SampleTime(0);
            }
        }

        private void Update()
        {
            if (playing)
            {
                for (int i = 0; i < _timers.Count; i++)
                {
                    if (_unscaledTime)
                    {
                        _timers[i] += Time.unscaledDeltaTime / _length;
                    }
                    else
                    {
                        _timers[i] += Time.deltaTime / _length;
                    }

                    if(_timers[i] > 0)
                    {
                        SampleTime(i);
                    }

                    if (_timers[i] >= 1f)
                    {
                        if (_loop)
                        {
                            _timers[i] = -Random.Range(_startDelay.x, _startDelay.y);
                        }
                    }
                }

                if (!_loop  && _timers[_timers.Count - 1] >= 1f)
                {
                    Pause();

                    if (_destroyObjectAtEnd)
                    {
                        Destroy(gameObject);
                    }
                }
            }
        }

        public void Play()
        {
            gameObject.SetActive(true);
            playing = true;
        }

        public void Pause()
        {
            playing = false;
        }

        public void SampleTime(int index)
        {
            Vector3 newPosition = Vector3.zero;
            newPosition.x += _xPosition.Evaluate(_timers[index]);
            newPosition.y += _yPosition.Evaluate(_timers[index]);
            newPosition.z += _zPosition.Evaluate(_timers[index]);

            Vector3 newRotation = _startRotations[index];
            newRotation.x += _xRotation.Evaluate(_timers[index]);
            newRotation.y += _yRotation.Evaluate(_timers[index]);
            newRotation.z += _zRotation.Evaluate(_timers[index]);

            Vector3 newScale = _startScales[index];
            newScale.x *= _xScale.Evaluate(_timers[index]);
            newScale.y *= _yScale.Evaluate(_timers[index]);
            newScale.z *= _zScale.Evaluate(_timers[index]);

            if (_space == Space.Self)
            {
                _items[index].localPosition = _startPositions[index] + _items[index].TransformVector(newPosition);
            }
            else
            {
                _items[index].localPosition = newPosition;
            }

            _items[index].localRotation = Quaternion.Euler(newRotation);
            _items[index].localScale = newScale;

            if (_animateAlpha)
            {
                _cgs[index].alpha = _alphaStartEnd.Evaluate(_timers[index]);
            }
        }
    }
}