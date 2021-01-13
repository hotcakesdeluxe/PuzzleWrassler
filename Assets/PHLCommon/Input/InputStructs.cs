using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PHL.Common.Input
{
    [System.Serializable]
    public struct InputButton
    {
        private InputSystem _parentSystem;
        private bool _onDown;
        private bool _down;
        private bool _onUp;
        private bool _up;

        public bool onDown
        {
            get
            {
                if (_parentSystem.disabled)
                {
                    return false;
                }

                return _onDown;
            }
        }

        public bool down
        {
            get
            {
                if (_parentSystem.disabled)
                {
                    return false;
                }

                return _down;
            }
        }

        public bool onUp
        {
            get
            {
                if(_parentSystem.disabled)
                {
                    return false;
                }

                return _onUp;
            }
        }

        public bool up
        {
            get
            {
                if(_parentSystem.disabled)
                {
                    return true;
                }

                return _up;
            }
        }
        
        public InputButton(InputSystem parentSystem)
        {
            _parentSystem = parentSystem;
            _onDown = false;
            _down = false;
            _onUp = false;
            _up = true;
        }

        public void UpdateButton(bool pressed)
        {
            if (pressed)
            {
                if (_down)
                {
                    _onDown = false;
                }
                else
                {
                    _onDown = true;
                }

                _down = true;
                _onUp = false;
                _up = false;
            }
            else
            {
                if (_up)
                {
                    _onUp = false;
                }
                else
                {
                    _onUp = true;
                }

                _up = true;
                _onDown = false;
                _down = false;
            }
        }

        public void Toggle()
        {
            UpdateButton(!_down);
        }
    }

    public struct InputAxis
    {
        private InputSystem _parentSystem;
        private Vector2 _axisHidden;
        public Vector2 axisRaw
        {
            set
            {
                _axisHidden = value;
                UpdateAxis();
            }
            get
            {
                if(_parentSystem.disabled)
                {
                    return Vector2.zero;
                }

                return _axisHidden;
            }
        }

        public Vector2 axis { get; private set; }
        public float deadzone { get; private set; }

        public float x
        {
            set
            {
                _axisHidden.x = value;
                UpdateAxis();
            }
            get
            {
                return axis.x;
            }
        }

        public float y
        {
            set
            {
                _axisHidden.y = value;
                UpdateAxis();
            }
            get
            {
                return axis.y;
            }
        }

        public InputAxis(InputSystem parentSystem, float newDeadzone = 0.1f)
        {
            _parentSystem = parentSystem;
            _axisHidden = new Vector2();
            axis = new Vector2();
            deadzone = newDeadzone;
        }

        private void UpdateAxis()
        {
            if (_parentSystem.disabled)
            {
                axis = Vector2.zero;
            }
            else
            {
                axis = _axisHidden;
                float squareMagnitude = axis.sqrMagnitude;

                if (squareMagnitude > 1)
                {
                    axis = axis.normalized;
                }

                float squareDeadzone = deadzone * deadzone;
                if (squareMagnitude < squareDeadzone)
                {
                    axis = Vector2.zero;
                }
            }
        }
    }
}