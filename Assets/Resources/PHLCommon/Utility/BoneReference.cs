using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BoneReference// : ISerializationCallbackReceiver
{
    public Transform root;
    public Transform bone;


    /*public Transform root;
    public Transform bone;
    public string bonePath;

    public void UpdateBone()
    {
        if (root != null)
        {
            if (bone == null && !string.IsNullOrEmpty(bonePath))
            {
                bone = root.Find(bonePath);
            }
            else if(bone != null)
            {
                if(string.IsNullOrEmpty(bonePath))
                {
                    string newBonePath = bone.name;
                    Transform boneTraverser = bone;

                    while(boneTraverser.parent != null && boneTraverser.parent != root)
                    {
                        boneTraverser = boneTraverser.parent;
                        newBonePath = boneTraverser.name + "/" + newBonePath;
                    }

                    bonePath = newBonePath;
                }
                else
                {
                    Transform boneFromPath = root.Find(bonePath);

                    if (boneFromPath != bone)
                    {
                        string newBonePath = bone.name;
                        Transform boneTraverser = bone;

                        while (boneTraverser.parent != null && boneTraverser.parent != root)
                        {
                            boneTraverser = boneTraverser.parent;
                            newBonePath = boneTraverser.name + "/" + newBonePath;
                        }

                        bonePath = newBonePath;
                    }
                }
            }
        }
    }

    public void OnAfterDeserialize()
    {
        
    }

    public void OnBeforeSerialize()
    {
        UpdateBone();
    }*/
}
