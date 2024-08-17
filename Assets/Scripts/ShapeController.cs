using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ShapeController : MonoBehaviour
{
    public Material validMaterial;
    public Material invalidMaterial;
    
    public Collider[] positiveColliders;
    public Collider[] negativeColliders;
    
    public List<BlockController> validBlocks;

    private void Start()
    {
        foreach (Collider positiveCollider in positiveColliders)
        {
            positiveCollider.isTrigger = true;
        }

        foreach (var negativeCollider in negativeColliders)
        {
            negativeCollider.isTrigger = true;
        }
    }
    
    public bool CheckShape(BlockController[] blocks)
    {
        int validColliders = 0;
        
        foreach (var block in blocks)
        {
            foreach (var negativeCollider in negativeColliders)
            {
                if (block.IsCollidingWith(negativeCollider))
                {
                    block.SetMaterial(invalidMaterial);
                    Debug.Log("Intersect with bad collider");
                    return false;
                }
            }
            
            bool wasAdded = false;
            foreach (var positiveCollider in positiveColliders)
            {
                if (block.IsCollidingWith(positiveCollider))
                {
                    if (!wasAdded)
                    {
                        block.SetMaterial(validMaterial);
                        validBlocks.Add(block);
                        wasAdded = true;
                    }
                    validColliders++;
                }
            }
        } 

        if (validColliders < positiveColliders.Length)
        {
            Debug.Log(validColliders + " validated colliders on " + positiveColliders.Length);
            return false;
        } 
        
        if (validColliders == positiveColliders.Length)
        {
            FreezeBlocks();
            return true;
        }
        
        return false;
    }

    private void FreezeBlocks()
    {
        foreach (var validBlock in validBlocks)
        {
            validBlock.FreezeBlock();
        }
    }
}
