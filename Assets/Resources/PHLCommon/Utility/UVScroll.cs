using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PHL.Common.Utility
{
    public class UVScroll : MonoBehaviour
    {
        public Vector2 scrollSpeed;
        public Renderer myRenderer;

        private void Reset()
        {
            if(myRenderer == null)
            {
                myRenderer = GetComponent<Renderer>();
            }
        }

        public void Update()
        {
            Vector2 newOffset = myRenderer.material.mainTextureOffset;
            newOffset += scrollSpeed * Time.deltaTime;
            
            while(newOffset.x > 1f)
            {
                newOffset.x -= 1f;
            }

            while (newOffset.x < 0f)
            {
                newOffset.x += 1f;
            }

            while (newOffset.y > 1f)
            {
                newOffset.y -= 1f;
            }

            while (newOffset.y < 0f)
            {
                newOffset.y += 1f;
            }

            myRenderer.material.mainTextureOffset = newOffset;
        }
    }
}