using PHL.Common.GameEvents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PHL.Common.Utility;

public class SimplerAnimation : MonoBehaviour
{
    public enum MotionType
    {
        Relative,
        Absolute
    }

    [SerializeField] private bool _playOnStart;
    [SerializeField] private bool _loop;
    [SerializeField] private bool _randomizeStartTime;
    [SerializeField, Range(0,1)] private float _currentTime;
    [SerializeField] private Transform _animationObject;
    [SerializeField] private Space _space;
    [SerializeField] private MotionType _motionType;
    [SerializeField] private bool _animatePosition;
    [SerializeField] private Vector3 _startingPosition;
    [SerializeField] private Vector3 _endingPosition;
    [SerializeField] private bool _animateRotation;
    [SerializeField] private Vector3 _startingRotation;
    [SerializeField] private Vector3 _endingRotation;
    [SerializeField] private bool _animateScale;
    [SerializeField] private Vector3 _startingScale = Vector3.one;
    [SerializeField] private Vector3 _endingScale = Vector3.one;
    [SerializeField] private float _animationLength = 1f;
    [SerializeField] private AnimationCurve _easingCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private float _previousCurrentTime;
    private bool _forwards;
    private Vector3 _adjustedEndingPosition;
    private Vector3 _adjustedEndingRotation;
    private Vector3 _adjustedEndingScale;

    public bool playing { get; private set; }
    public float animationLength => _animationLength;

    private void Reset()
    {
        _playOnStart = true;
        _loop = true;
        _animationObject = transform;
    }

    private void Start()
    {
        if(_motionType == MotionType.Relative)
        {
            if (_space == Space.Self)
            {
                _startingPosition = _animationObject.localPosition;
                _startingRotation = _animationObject.localRotation.eulerAngles;
                //_startingScale = _animationObject.localScale;
            }
            else if(_space == Space.World)
            {
                _startingPosition = _animationObject.position;
                _startingRotation = _animationObject.rotation.eulerAngles;
                //_startingScale = _animationObject.localScale;
            }
        }

        GameEventDatabase.instance.playSimplerAnimationEvent.AddListener(PlaySimplerAnimationReceived);

        AlignToTimer();

        if(_randomizeStartTime)
        {
            _currentTime = Random.value;
        }

        if (_playOnStart)
        {
            Play(false);
        }
    }

    private void Update()
    {
        if (playing)
        {
            if (_forwards)
            {
                AlignToTimer();
                _currentTime = Mathf.MoveTowards(_currentTime, 2f, Time.deltaTime / _animationLength);

                if (_currentTime >= 1f)
                {
                    if (_loop)
                    {
                        _currentTime = 0f;
                    }
                    else
                    {
                        _currentTime = 1f;
                        playing = false;
                    }
                    AlignToTimer();
                }
            }
            else
            {
                AlignToTimer();
                _currentTime = Mathf.MoveTowards(_currentTime, -1, Time.deltaTime / _animationLength);

                if (_currentTime <= 0)
                {
                    if (_loop)
                    {
                        _currentTime = 1f;
                    }
                    else
                    {
                        _currentTime = 0f;
                        playing = false;
                    }
                    AlignToTimer();
                }
            }
        }
        else
        {
            if (!Mathf.Approximately(_currentTime, _previousCurrentTime))
            {
                AlignToTimer();
            }

            _previousCurrentTime = _currentTime;
        }
    }

    public void SetCurrentTime(float newCurrentTime)
    {
        _currentTime = newCurrentTime;
    }

    private void AlignToTimer()
    {
        float curvedTimer = _easingCurve.Evaluate(_currentTime);

        if(_motionType == MotionType.Relative)
        {
            _adjustedEndingPosition = _startingPosition + _endingPosition;
            _adjustedEndingRotation = _startingRotation + _endingRotation;
            _adjustedEndingScale = _endingScale;
        }
        else if(_motionType == MotionType.Absolute)
        {
            _adjustedEndingPosition = _endingPosition;
            _adjustedEndingRotation = _endingRotation;
            _adjustedEndingScale = _endingScale;
        }

        if (_space == Space.Self)
        {
            if (_animatePosition)
            {
                _animationObject.localPosition = Vector3.LerpUnclamped(_startingPosition, _adjustedEndingPosition, curvedTimer);
            }

            if (_animateRotation)
            {
                _animationObject.localRotation = Quaternion.SlerpUnclamped(Quaternion.Euler(_startingRotation), Quaternion.Euler(_adjustedEndingRotation), curvedTimer);
            }
        }
        else if(_space == Space.World)
        {
            if (_animatePosition)
            {
                _animationObject.position = Vector3.LerpUnclamped(_startingPosition, _adjustedEndingPosition, curvedTimer);
            }

            if (_animateRotation)
            {
                _animationObject.rotation = Quaternion.SlerpUnclamped(Quaternion.Euler(_startingRotation), Quaternion.Euler(_adjustedEndingRotation), curvedTimer);
            }
        }

        if (_animateScale)
        {
            _animationObject.localScale = Vector3.LerpUnclamped(_startingScale, _adjustedEndingScale, curvedTimer);
        }
    }

    private void PlaySimplerAnimationReceived(GameEventInfo gameEventInfo)
    {
        GameObject animationObject = gameEventInfo.GetObject<GameObject>("Object");

        if(animationObject == gameObject)
        {
            bool pingPong = gameEventInfo.GetBool("PingPong");
            bool playInReverse = gameEventInfo.GetBool("PlayInReverse");

            if ((pingPong && _forwards) || playInReverse)
            {
                PlayReverse(false);
            }
            else
            {
                Play(false);
            }
        }
    }

    [ContextMenu("Play")]
    public void Play()
    {
        Play(true);
    }

    public void Play(bool resetTime)
    {
        _forwards = true;
        playing = true;

        if (resetTime)
        {
            _currentTime = 0;
            AlignToTimer();
        }
    }

    public void Pause()
    {
        playing = false;
    }

    [ContextMenu("Stop")]
    public void Stop()
    {
        Stop(true);
    }

    public void Stop(bool resetTime)
    {
        playing = false;

        if (resetTime)
        {
            _currentTime = 0;
            AlignToTimer();
        }
    }

    [ContextMenu("Play Reverse")]
    public void PlayReverse()
    {
        PlayReverse(true);
    }

    public void PlayReverse(bool resetTime)
    {
        _forwards = false;
        playing = true;

        if (resetTime)
        {
            _currentTime = 1f;
            AlignToTimer();
        }
    }

    private void OnDrawGizmosSelected()
    {
        if(_animationObject == null)
        {
            return;
        }

        Vector3 pointA = _startingPosition;
        Vector3 pointB = _endingPosition;
        Transform parent = _animationObject.parent;

        if(parent == null)
        {
            parent = _animationObject;
        }

        if (_motionType == MotionType.Relative)
        {
            if (_space == Space.Self)
            {
                pointA = parent.TransformPoint(_startingPosition);
                pointB = parent.TransformPoint(_startingPosition + _endingPosition);
            }
            else if (_space == Space.World)
            {
                pointA = _animationObject.position + _startingPosition;
                pointB = _animationObject.position + _endingPosition;
            }
        }
        else if(_motionType == MotionType.Absolute)
        {
            if(_space == Space.Self)
            {
                pointA = parent.TransformPoint(pointA);
                pointB = parent.TransformPoint(pointB);
            }
        }

        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(pointA, pointB);
        DebugExtension.DrawArrow(pointB - (pointB - pointA).normalized * 4, (pointB - pointA).normalized * 4, Gizmos.color);
    }
}
