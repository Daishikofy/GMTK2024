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
    
    public List<Collider> validBlocks;

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

    public GameObject ValidateShape(BlockController[] blocks)
    {
        if (CheckShape(blocks))
        {
            Debug.Log("Create a new shape");
            return null;
            /*
            GameObject result = Instantiate(new GameObject(), transform.position, Quaternion.identity);
            foreach (var validBlock in validBlocks)
            {
                var newBlock = Instantiate(validBlock, result.transform);
                newBlock.isTrigger = true;
                newBlock.GetComponent<Rigidbody>().isKinematic = true;
            }

            return result;*/
        }
        Debug.Log("Do nothing");
        return null;
    }

    public bool CheckShape(BlockController[] blocks)
    {
        int validColliders = 0;
        
        foreach (var block in blocks)
        {
            //Debug.Log("Check block : " + block.gameObject.name);
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
                //Debug.Log("collider: " + positiveCollider.bounds + " - object : " + block.BlockCollider.bounds);
                if (block.IsCollidingWith(positiveCollider))
                {
                   // Debug.Log("INTERSECTION");
                    if (!wasAdded)
                    {
                        block.SetMaterial(validMaterial);
                        validBlocks.Add(positiveCollider);
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
            return true;
        }
        
        return false;
    }
}
