using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PHL.Common.Utility
{
    public class Spin : MonoBehaviour
    {
        [SerializeField] private UpdateType _updateType;
        [SerializeField] private Space _space;
        [SerializeField] private Vector3 _spin;

        private void Update()
        {
            if (_updateType == UpdateType.Update)
            {
                transform.Rotate(_spin * Time.deltaTime, _space);
            }
        }

        private void FixedUpdate()
        {
            if (_updateType == UpdateType.FixedUpdate)
            {
                transform.Rotate(_spin * Time.fixedDeltaTime, _space);
            }
        }

        private void LateUpdate()
        {
            if (_updateType == UpdateType.LateUpdate)
            {
                transform.Rotate(_spin * Time.deltaTime, _space);
            }
        }
    }
}