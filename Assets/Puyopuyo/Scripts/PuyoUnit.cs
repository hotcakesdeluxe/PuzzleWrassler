using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuyoUnit : MonoBehaviour
{
    private Color[] colorArray = { Color.blue, Color.green, Color.red, Color.cyan };
    public bool activelyFalling = true;
    public bool forcedDownwards = false;

    public int colorIdx;

    void Awake(){
        colorIdx = Random.Range(0,4);
        GetComponent<SpriteRenderer>().color = colorArray[colorIdx];
    }

    public IEnumerator DropToFloor(){
        Debug.Log("dropped to the floor");
        WaitForSeconds wait = new WaitForSeconds( .25f );
        Vector3 currentPos = RoundVector(gameObject.transform.position);
        for(int row = (int)currentPos.y - 1; row >= 0;  row--){
            int currentX = (int)currentPos.x;
            Debug.Log(currentX + " current x");
            Debug.Log(row + " row minus one");
            if(GameBoard.IsEmpty(currentX, row)){
                Debug.Log("droppin");
                forcedDownwards = true; 
                GameBoard.Clear(currentX, row + 1);
                GameBoard.Add(currentX, row, gameObject.transform);                    
                gameObject.transform.position += Vector3.down;
                yield return wait;
            } else { 
                activelyFalling = false;
                forcedDownwards = false;
                break;
            }
        }
        forcedDownwards = false;
        activelyFalling = false;
    }

    public void DropToFloorExternal(){
        StartCoroutine(DropToFloor());
    }

    public Vector3 RoundVector(Vector3 vect){
        return new Vector2(Mathf.Round(vect.x), Mathf.Round(vect.y));
    }
}
