using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour
{
    public Collider BlockCollider;
    public Rigidbody BlockRigidbody;
    public MeshRenderer BlockMeshRenderer;

    public void SetMaterial(Material material)
    {
        BlockMeshRenderer.material = material;
    }
}
