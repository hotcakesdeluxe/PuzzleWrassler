using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PHL.Common.Utility
{
    public class SpinObject : MonoBehaviour
    {
        [SerializeField] private Transform _object;
        [SerializeField] private UpdateType _updateType;
        [SerializeField] private Space _space;
        [SerializeField] private Vector3 _spin;

        private void Reset()
        {
            _object = transform;
        }

        private void Update()
        {
            if (_updateType == UpdateType.Update)
            {
                _object.Rotate(_spin * Time.deltaTime, _space);
            }
        }

        private void FixedUpdate()
        {
            if (_updateType == UpdateType.FixedUpdate)
            {
                _object.Rotate(_spin * Time.fixedDeltaTime, _space);
            }
        }

        private void LateUpdate()
        {
            if (_updateType == UpdateType.LateUpdate)
            {
                _object.Rotate(_spin * Time.deltaTime, _space);
            }
        }
    }
}