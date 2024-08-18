using System;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour
{
    public MeshRenderer BlockMeshRenderer;
    public Rigidbody BlockRigidbody;
    
    public List<Collider> CollidersInTrigger;

    private bool IsFreezed;
    private Vector3 freezePosition;
    

    private bool IsFalling = false;
    private void Start()
    {
        BlockMeshRenderer = GetComponent<MeshRenderer>();
        BlockRigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (IsFreezed && !IsFalling)
        {
            var dist = Vector3.Distance(transform.position, freezePosition);
            if (dist > 0.2f)
            {
                GameManager.Instance.currentLevelManager.FallingCastle(transform.position);
                IsFalling = true;
            }
        }
    }

    public void SetMaterial(Material material)
    {
        BlockMeshRenderer.material = material;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        CollidersInTrigger.Add(other);
    }
    private void OnTriggerExit(Collider other)
    {
        CollidersInTrigger.Remove(other);
    }

    public bool IsCollidingWith(Collider other)
    {
        return CollidersInTrigger.Remove(other);
    }

    public void FreezeBlock()
    {
        freezePosition = transform.position;
        gameObject.layer = 2;
        IsFreezed = true;
    }
}
