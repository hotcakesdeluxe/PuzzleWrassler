using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BlockGarbageData", menuName = "BlockGarbage")]
public class BlockGarbageData : ScriptableObject
{
    [SerializeField]
    public BlockColor[] firstRow;
    public BlockColor[] secondRow;
    public BlockColor[] thirdRow;
    public BlockColor[] fourthRow;

    

}

public enum BlockColor
{
    strike,
    grapple,
    aerial,
    submission
}

