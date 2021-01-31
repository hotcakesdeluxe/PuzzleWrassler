using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PHL.Common.Utility;

public class Block : MonoBehaviour
{
    public string blockType;
    private MeshRenderer _meshRenderer;
    private BlockBoard _blockBoard;
    public bool activelyFalling = true;
    public bool forcedDownwards = false;
    public int colorIdx;

    void Update()
    {
        if (_blockBoard != null)
        {
            _blockBoard.DebugBoard();
        }

    }

    public void Initialize(BlockBoard blockBoard)
    {
        _blockBoard = blockBoard;
        //blockType = GetRandomElement();
        _meshRenderer = GetComponent<MeshRenderer>();
        _meshRenderer.material.SetColor("_Color", GetColorByType(blockType));
    }
    public IEnumerator DropToFloorRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(.1f);
        Vector3 currentPos = RoundVector(gameObject.transform.position);
        for (int row = Mathf.FloorToInt(currentPos.y) - 1; row >= 0; row--)
        {
            int currentX = Mathf.FloorToInt(currentPos.x);
            if (_blockBoard.IsEmpty(currentX, row))
            {
                forcedDownwards = true;
                _blockBoard.Clear(currentX, row + 1);
                _blockBoard.Add(currentX, row, gameObject.transform);
                gameObject.transform.position += Vector3.down;
                yield return wait;
            }
            else
            {
                activelyFalling = false;
                forcedDownwards = false;
                break;
            }
        }
        forcedDownwards = false;
        activelyFalling = false;
    }

    public void DropToFloor()
    {
        StartCoroutine(DropToFloorRoutine());
    }


    /*private string GetRandomElement()
    {
        float randomVal = Random.value * 4;
        if (randomVal <= 1) return "strike";
        else if (randomVal <= 2) return "grapple";
        else if (randomVal <= 3) return "aerial";
        else return "submission";
    }*/

    private Color GetColorByType(string type)
    {
        if (type == "strike")
        {
            colorIdx = 0;
            return Color.green;
        }
        else if (type == "grapple")
        {
            colorIdx = 1;
            return Color.cyan;
        }
        else if (type == "aerial")
        {
            colorIdx = 2;
            return Color.magenta;
        }
        else
        {
            colorIdx = 3;
            return Color.red;
        }
    }
    private Vector3 RoundVector(Vector3 vect)
    {
        return new Vector2(Mathf.Round(vect.x), Mathf.Round(vect.y));
    }

}
