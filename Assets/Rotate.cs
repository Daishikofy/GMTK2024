using System;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public int velocity;
    public Rigidbody rigidbody;
    
    private void Update()
    {
        rigidbody.AddTorque(Vector3.up * velocity * Time.deltaTime, ForceMode.VelocityChange);
    }
}