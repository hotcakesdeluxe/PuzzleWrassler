using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockObject : MonoBehaviour
{
    public int column;
    private MeshRenderer _meshRenderer;
    public string blockType { get; private set; }
    public bool isFalling = true;
    public static float fallSpeed = 0.5f;
    public float fallSpeedMultiplier = 1f;
    private Vector3 _spawnPosition;

    public void Initialize(int startColumn)
    {
        blockType = GetRandomElement();
        _meshRenderer = GetComponent<MeshRenderer>();
        _meshRenderer.material.SetColor("_Color", GetColorByType(blockType));
        column = startColumn;
        _spawnPosition = transform.position;
        isFalling = true;
        //this.transform.local = new Vector3(0, this.transform.position.y, this.transform.position.z);
    }
    public void UpdatePosition(float columnHeight)
    {
        if (transform.position.y <= columnHeight)
        {
            isFalling = false;
        }
        if (isFalling)
        {
            float actualFallSpeed = Mathf.Min(30f, fallSpeed * fallSpeedMultiplier);
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                actualFallSpeed = Mathf.Max(10f, actualFallSpeed);
            }
            transform.position += Vector3.down * actualFallSpeed * Time.deltaTime;
            transform.position = new Vector3(column+_spawnPosition.x, transform.position.y, 0);
        }
       
    }
    private string GetRandomElement()
    {
        // return "fire"; // DEBUG
        float randomVal = Random.value * 4;
        if (randomVal <= 1) return "earth";
        else if (randomVal <= 2) return "air";
        else if (randomVal <= 3) return "water";
        else return "fire";
    }
    private Color GetColorByType(string type)
    {
        if (type == "earth")
        {
            return new Color(0.5f, 0.25f, 0f);
        }
        else if (type == "air")
        {
            return new Color(0.5f, 1f, 0.7f);
        }
        else if (type == "water")
        {
            return Color.blue;
        }
        else
        {
            return Color.red;
        }
    }
}
