using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PHL.Common.Utility
{
    public class SmoothFloat
    {
        public enum DeltaTimeMode
        {
            Normal,
            Unscaled,
            Fixed
        }

        public float current;
        public float target { get; private set; }
        private float _velocity;
        private float _smoothSpeed;
        private float _maxSpeed;
        private DeltaTimeMode _deltaTimeMode;
        private bool _useAngle;

        public static implicit operator float(SmoothFloat smoothFloat)
        {
            return smoothFloat.current;
        }

        public SmoothFloat(float newSmoothSpeed, bool newUseAngle = false)
        {
            current = 0;
            target = 0;
            _smoothSpeed = newSmoothSpeed;
            _maxSpeed = Mathf.Infinity;
            _deltaTimeMode = DeltaTimeMode.Normal;
            _useAngle = newUseAngle;
        }

        public SmoothFloat(float newCurrent, float newTarget, float newSmoothSpeed, bool newUseAngle = false)
        {
            current = newCurrent;
            target = newTarget;
            _smoothSpeed = newSmoothSpeed;
            _maxSpeed = Mathf.Infinity;
            _deltaTimeMode = DeltaTimeMode.Normal;
            _useAngle = newUseAngle;
        }

        public SmoothFloat(float newCurrent, float newTarget, float newSmoothSpeed, float newMaxSpeed, bool newUseAngle = false)
        {
            current = newCurrent;
            target = newTarget;
            _smoothSpeed = newSmoothSpeed;
            _maxSpeed = newMaxSpeed;
            _useAngle = newUseAngle;
        }

        public SmoothFloat(float newCurrent, float newTarget, float newSmoothSpeed, DeltaTimeMode newDeltaTimeMode, bool newUseAngle = false)
        {
            current = newCurrent;
            target = newTarget;
            _smoothSpeed = newSmoothSpeed;
            _maxSpeed = Mathf.Infinity;
            _deltaTimeMode = newDeltaTimeMode;
            _useAngle = newUseAngle;
        }

        public SmoothFloat(float newCurrent, float newTarget, float newSmoothSpeed, float newMaxSpeed, DeltaTimeMode newDeltaTimeMode, bool newUseAngle = false)
        {
            current = newCurrent;
            target = newTarget;
            _smoothSpeed = newSmoothSpeed;
            _maxSpeed = newMaxSpeed;
            _deltaTimeMode = newDeltaTimeMode;
            _useAngle = newUseAngle;
        }

        public void Snap()
        {
            _velocity = 0f;
            current = target;
        }

        public void Snap(float value)
        {
            _velocity = 0f;
            target = value;
            current = value;
        }

        public void SetCurrent(float newCurrent, bool update = false)
        {
            current = newCurrent;

            if (update)
            {
                Update();
            }
        }

        public void SetTarget(float newTarget, bool update = false)
        {
            target = newTarget;

            if (update)
            {
                Update();
            }
        }

        public void AddToTarget(float addValue, bool update = false)
        {
            target += addValue;

            if (update)
            {
                Update();
            }
        }

        public void SetSmoothSpeed(float newSmoothSpeed)
        {
            _smoothSpeed = newSmoothSpeed;
        }

        public void Update()
        {
            if (_useAngle)
            {
                if (_deltaTimeMode == DeltaTimeMode.Normal)
                {
                    current = Mathf.SmoothDampAngle(current, target, ref _velocity, _smoothSpeed, _maxSpeed, Time.unscaledDeltaTime);
                }
                else if (_deltaTimeMode == DeltaTimeMode.Unscaled)
                {
                    current = Mathf.SmoothDampAngle(current, target, ref _velocity, _smoothSpeed, _maxSpeed, Time.unscaledDeltaTime);
                }
                else if (_deltaTimeMode == DeltaTimeMode.Fixed)
                {
                    current = Mathf.SmoothDampAngle(current, target, ref _velocity, _smoothSpeed, _maxSpeed, Time.fixedDeltaTime);
                }
            }
            else
            {
                if (_deltaTimeMode == DeltaTimeMode.Normal)
                {
                    current = Mathf.SmoothDamp(current, target, ref _velocity, _smoothSpeed, _maxSpeed, Time.unscaledDeltaTime);
                }
                else if (_deltaTimeMode == DeltaTimeMode.Unscaled)
                {
                    current = Mathf.SmoothDamp(current, target, ref _velocity, _smoothSpeed, _maxSpeed, Time.unscaledDeltaTime);
                }
                else if (_deltaTimeMode == DeltaTimeMode.Fixed)
                {
                    current = Mathf.SmoothDamp(current, target, ref _velocity, _smoothSpeed, _maxSpeed, Time.fixedDeltaTime);
                }
            }
        }
    }
}