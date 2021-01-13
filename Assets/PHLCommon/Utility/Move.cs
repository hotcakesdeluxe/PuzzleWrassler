using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    [SerializeField] private Vector3 _motion;
    [SerializeField] private Space _space;

    private void Update()
    {
        if (_space == Space.Self)
        {
            transform.position += transform.TransformDirection(_motion * Time.deltaTime);
        }
        else
        {
            transform.position += _motion * Time.deltaTime;
        }
    }
}
