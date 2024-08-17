using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    
    public Rigidbody targetedObject;
    public Rigidbody selectedObject;

    public float dragForce = 50f;
    public float dragResistance = 10f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (selectedObject != null)
        {
            var newPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
            //selectedObject.MovePosition(newPosition);
            Debug.DrawLine(selectedObject.position, newPosition, Color.green);
            Vector3 force = (newPosition - selectedObject.position) * dragForce;
            selectedObject.AddForce(force, ForceMode.Acceleration);
        }

        if (Input.mouseScrollDelta.y > 0)
        {
            Debug.Log("Size up");
        }
        if (Input.mouseScrollDelta.y < 0)
        {
            Debug.Log("Size down");
        }
    }

    public void OnClick(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out RaycastHit hit);
            if (hit.rigidbody != null && hit.rigidbody == targetedObject)
            {
                selectedObject = hit.rigidbody;
                selectedObject.useGravity = false;
                selectedObject.drag = dragResistance;
                selectedObject.constraints = RigidbodyConstraints.FreezeRotation;
            }
        }
        else if (context.started)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out RaycastHit hit);
            if (hit.rigidbody != null)
            {
                Debug.DrawRay(ray.origin, ray.direction * 10f, Color.red, 10f);
                Debug.Log(hit.transform.gameObject.name + ": " + hit.point);
                targetedObject = hit.rigidbody;
            }
        }
        else if (context.canceled)
        {
            if (selectedObject != null)
            {
                //selectedObject.mass = 1;
                selectedObject.useGravity = true;
                selectedObject.drag = 0;
                selectedObject.constraints = RigidbodyConstraints.None;
                selectedObject.constraints = RigidbodyConstraints.FreezePositionZ;
                selectedObject = null;
            }
            targetedObject = null;
        }

        
        //Ray ray = new Ray(transform.position, transform.forward);

    }

    public void OnRotate(InputAction.CallbackContext context)
    {
        if (selectedObject != null && context.performed)
        {
            Vector2 value = context.ReadValue<Vector2>();
            if (value.x != 0f)
            { 
                selectedObject.transform.Rotate( value.x * 90, 0f,0f);
            }
            else if (value.y != 0f)
            { 
                selectedObject.transform.Rotate(0f, 0f, value.y * 90);
            }
        }
    }
}
