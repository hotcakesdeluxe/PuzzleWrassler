using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBlock : MonoBehaviour
{
    private int[,] pointsGrid;
    private GameObject[,] blockGrid;
    private List<GameObject> _blocksToDestroy = new List<GameObject>();

    public void CheckForDestroyBlocks(int setScoreMultiplier)
    {
        blockGrid = BlockWell.blockGrid;
        /*scoreMultiplier = setScoreMultiplier;
        numFallingBlocks = 0;*/

        Queue<int[]> destroySquares = new Queue<int[]>();

        int numDestroySquares = 0;
        pointsGrid = new int[BlockWell.width, BlockWell.height];  // All values should initialize to zero
        for (int i = 0; i < BlockWell.width - 1; i++)
        {
            for (int j = 0; j < BlockWell.height - 1; j++)
            {
                GameObject blockObject = blockGrid[i, j];
                if (blockObject == null)
                {
                    break;  // blocks in a vertical stack must be contiguous
                }
                BlockScript blockScript = blockObject.GetComponent<BlockScript>();
                string type = blockScript.type;
                if (i + 1 >= BlockWell.width || j + 1 >= BlockWell.height ||
                    blockGrid[i + 1, j] == null || blockGrid[i + 1, j].GetComponent<BlockScript>().type != type ||
                    blockGrid[i, j + 1] == null || blockGrid[i, j + 1].GetComponent<BlockScript>().type != type ||
                    blockGrid[i + 1, j + 1] == null || blockGrid[i + 1, j + 1].GetComponent<BlockScript>().type != type)
                {
                    continue;
                }
                else
                {
                    // Add block to pointsGrid
                    pointsGrid[i, j] += 3; // CheckBlock will add +1, for a total of 4 points per DestroySquare block
                    pointsGrid[i + 1, j] += 3;
                    pointsGrid[i, j + 1] += 3;
                    pointsGrid[i + 1, j + 1] += 3;
                    destroySquares.Enqueue(new int[2] { i, j });
                    numDestroySquares++;
                }
            }
        }
        if (numDestroySquares == 0)
        {
            //blockPair.InitializeBlockPair();
        }
        else
        {
           /* Debug.Log("BLOCK BONUS: " + numDestroySquares * numDestroySquares * numDestroySquares * scoreMultiplier * manager.speed);
            manager.addPoints(numDestroySquares * numDestroySquares * numDestroySquares * scoreMultiplier * manager.speed);
            debugPointsFromBlocks += numDestroySquares * numDestroySquares * numDestroySquares * scoreMultiplier * manager.speed;
            startBlockDestroy(destroySquares);*/
        }
    }
}
