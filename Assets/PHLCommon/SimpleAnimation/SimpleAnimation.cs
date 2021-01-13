using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PHL.Common.Utility
{
    public class SimpleAnimation : MonoBehaviour
    {
        [SerializeField] private Transform _object;
        [SerializeField] private float _length = 1f;
        [SerializeField] private bool _playOnStart;
        [SerializeField] private bool _loop;
        [SerializeField] private Space _space = Space.Self;
        [SerializeField] private bool _relative = true;
        [SerializeField] private bool _unscaledTime;
        [SerializeField] private bool _disableObjectAtEnd;
        [SerializeField] private bool _useOnEnable;

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

        private float _timer;
        private Vector3 _startPosition;
        private Vector3 _startRotation;
        private Vector3 _startScale;

        public bool playing { get; private set; }

        private void Start()
        {
            if (_relative)
            {
                if (_space == Space.Self)
                {
                    _startPosition = _object.localPosition;
                    _startRotation = _object.localRotation.eulerAngles;
                    _startScale = _object.localScale;
                }
                else if (_space == Space.World)
                {
                    _startPosition = _object.position;
                    _startRotation = _object.rotation.eulerAngles;
                    _startScale = _object.localScale;
                }
            }
            else
            {
                _startPosition = Vector3.zero;
                _startRotation = Vector3.zero;
                _startScale = Vector3.one;
            }

            if (_playOnStart)
            {
                Play();
                SampleCurrentTime();
            }
        }

        private void OnEnable()
        {
            if(_useOnEnable)
            {
                Start();
            }
        }

        private void Update()
        {
            if (playing)
            {
                if (_unscaledTime)
                {
                    _timer = Mathf.MoveTowards(_timer, 2f, Time.unscaledDeltaTime / _length);
                }
                else
                {
                    _timer = Mathf.MoveTowards(_timer, 2f, Time.deltaTime / _length);
                }

                if (_timer >= 1f)
                {
                    if (_loop)
                    {
                        _timer -= 1f;
                    }
                    else
                    {
                        _timer = 1f;
                        Pause();
                        
                        if(_disableObjectAtEnd)
                        {
                            _object.gameObject.SetActive(false);
                        }
                    }
                }

                SampleCurrentTime();
            }
        }

        public void Play()
        {
            _object.gameObject.SetActive(true);
            playing = true;
        }

        public void Pause()
        {
            playing = false;
        }

        public void Stop()
        {
            Pause();
            _timer = 0;
        }

        public void ResetTime()
        {
            _timer = 0;
        }

        public void SampleCurrentTime()
        {
            SampleTime(_timer);
        }

        public void SampleTime(float time)
        {
            Vector3 newPosition = _startPosition;
            newPosition.x += _xPosition.Evaluate(time);
            newPosition.y += _yPosition.Evaluate(time);
            newPosition.z += _zPosition.Evaluate(time);

            Vector3 newRotation = _startRotation;
            newRotation.x += _xRotation.Evaluate(time);
            newRotation.y += _yRotation.Evaluate(time);
            newRotation.z += _zRotation.Evaluate(time);

            Vector3 newScale = _startScale;
            newScale.x *= _xScale.Evaluate(time);
            newScale.y *= _yScale.Evaluate(time);
            newScale.z *= _zScale.Evaluate(time);

            if (_space == Space.Self)
            {
                _object.localPosition = newPosition;
                _object.localRotation = Quaternion.Euler(newRotation);
                _object.localScale = newScale;
            }
            else if (_space == Space.World)
            {
                _object.position = newPosition;
                _object.rotation = Quaternion.Euler(newRotation);
                _object.localScale = newScale;
            }
        }

        public void SetLoop (bool isLooping)
        {
            _loop = isLooping;
        }

        public void ToggleLoop ()
        {
            _loop = !_loop;
        }

        private void OnDisable()
        {
            if(_useOnEnable)
            {
                Stop();

                if (_space == Space.Self)
                {
                    _object.localPosition = _startPosition;
                    _object.localRotation = Quaternion.Euler(_startRotation);
                    _object.localScale = _startScale;
                }
                else if (_space == Space.World)
                {
                    _object.position = _startPosition;
                    _object.rotation = Quaternion.Euler(_startRotation);
                    _object.localScale = _startScale;
                }
            }
        }
    }
}