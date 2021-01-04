using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowFinger : MonoBehaviour
{
    void Update()
    {
         if (Input.GetMouseButton(0) || Input.touchCount > 0)
         {
             // get mouse position in screen space
             // (if touch, gets average of all touches)
             Vector3 screenPos = Input.mousePosition;
             // set a distance from the camera
             screenPos.z = 0.01f;
             // convert mouse position to world space
             Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
 
             // get current position of this GameObject
             Vector3 newPos = transform.position;
             // set x position to mouse world-space x position
             newPos.x = worldPos.x;
             // apply new position
             transform.position = newPos;
         }
    }
}
