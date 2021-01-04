using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurveAttribute : PropertyAttribute
{
    public Color color;

    public CurveAttribute(float r, float g, float b)
    {
        this.color = new Color(r, g, b);
    }
}