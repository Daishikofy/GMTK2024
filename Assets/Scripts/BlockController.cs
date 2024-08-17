using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class BlockController : MonoBehaviour
{
    public Collider BlockCollider;
    public Rigidbody BlockRigidbody;
    public MeshRenderer BlockMeshRenderer;

    public List<Collider> CollidersInTrigger;

    private void Start()
    {
        BlockCollider = GetComponent<Collider>();
        BlockRigidbody = GetComponent<Rigidbody>();
        BlockMeshRenderer = GetComponent<MeshRenderer>();
    }

    public void SetMaterial(Material material)
    {
        BlockMeshRenderer.material = material;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(gameObject.name + " triggered with " + other.name);
        CollidersInTrigger.Add(other);
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log(gameObject.name + " exit with " + other.name);
        CollidersInTrigger.Remove(other);
    }

    public bool IsCollidingWith(Collider other)
    {
        return CollidersInTrigger.Remove(other);
    }
}
