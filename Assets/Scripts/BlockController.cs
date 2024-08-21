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


    public bool isSelected = false;
    private bool IsFalling = false;

    private float initialDragResistance;
    private void Awake()
    {
        BlockMeshRenderer = GetComponent<MeshRenderer>();
        BlockRigidbody = GetComponent<Rigidbody>();
        initialDragResistance = BlockRigidbody.drag;
    }

    private void Update()
    {
        if (IsFreezed && !IsFalling)
        {
            var dist = Vector3.Distance(transform.position, freezePosition);
            if (dist > 0.4f)
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

    public void SelectBlock(float dragResistance)
    {
        isSelected = true;
        BlockRigidbody.useGravity = false;
        BlockRigidbody.drag = dragResistance;
                
        Vector3 alignedForward = NearestWorldAxis(BlockRigidbody.transform.forward);
        Vector3 alignedUp = NearestWorldAxis(BlockRigidbody.transform.up);
        BlockRigidbody.rotation = Quaternion.LookRotation(alignedForward, alignedUp);
                
        BlockRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
    }

    public void ReleaseBlock()
    {
        isSelected = false;
        BlockRigidbody.useGravity = true;
        BlockRigidbody.drag = initialDragResistance;
        BlockRigidbody.constraints = RigidbodyConstraints.None;
        BlockRigidbody.constraints = RigidbodyConstraints.FreezePositionZ;
    }
    
    private static Vector3 NearestWorldAxis(Vector3 v)
    {
        if (Mathf.Abs(v.x) < Mathf.Abs(v.y))
        {
            v.x = 0;
            if (Mathf.Abs(v.y) < Mathf.Abs(v.z))
                v.y = 0;
            else
                v.z = 0;
        }
        else
        {
            v.y = 0;
            if (Mathf.Abs(v.x) < Mathf.Abs(v.z))
                v.x = 0;
            else
                v.z = 0;
        }
        return v;
    }
}
