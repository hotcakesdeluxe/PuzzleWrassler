using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PHL.Common.Utility
{
    public class Wiggle : MonoBehaviour
    {
        [SerializeField] private Transform _wiggler;
        [SerializeField] private Vector3 _positionFrequency;
        [SerializeField] private Vector3 _positionAmplitude;
        [SerializeField] private Vector3 _rotationFrequency;
        [SerializeField] private Vector3 _rotationAmplitude;

        private Vector3 _startLocalPosition;
        private Quaternion _startLocalRotation;
        private float _timer;
        private Vector3 _positionChange;
        private Vector3 _rotationChange;
        private Perlin _perlin;

        private void Start()
        {
            _startLocalPosition = _wiggler.localPosition;
            _startLocalRotation = _wiggler.localRotation;

            _perlin = new Perlin();
        }

        private void Update()
        {
            //Loop timer to prevent floating point errors
            _timer += Time.deltaTime;
            if (_timer > 10000f)
            {
                _timer = -10000f;
            }

            _positionChange = Vector3.zero;

            if (!Mathf.Approximately(_positionAmplitude.x, 0f))
            {
                _positionChange.x = _perlin.Noise(_timer * _positionFrequency.x) * _positionAmplitude.x;
            }

            if (!Mathf.Approximately(_positionAmplitude.y, 0f))
            {
                _positionChange.y = _perlin.Noise(_timer * _positionFrequency.y + 122.234f) * _positionAmplitude.y;
            }

            if (!Mathf.Approximately(_positionAmplitude.z, 0f))
            {
                _positionChange.z = _perlin.Noise(_timer * _positionFrequency.z + 352.763f) * _positionAmplitude.z;
            }

            _rotationChange = Vector3.zero;

            if (!Mathf.Approximately(_rotationAmplitude.x, 0f))
            {
                _rotationChange.x = _perlin.Noise(_timer * _rotationFrequency.x + 561.562f) * _rotationAmplitude.x;
            }

            if (!Mathf.Approximately(_rotationAmplitude.y, 0f))
            {
                _rotationChange.y = _perlin.Noise(_timer * _rotationFrequency.y + 712.254f) * _rotationAmplitude.y;
            }

            if (!Mathf.Approximately(_rotationAmplitude.z, 0f))
            {
                _rotationChange.z = _perlin.Noise(_timer * _rotationFrequency.z + 940.967f) * _rotationAmplitude.z;
            }

            _wiggler.localPosition = _startLocalPosition + _positionChange;
            _wiggler.localRotation = _startLocalRotation;
            _wiggler.Rotate(_rotationChange, Space.Self);
        }
    }
}