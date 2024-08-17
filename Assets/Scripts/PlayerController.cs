using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    
    public Rigidbody targetedObject;
    public Rigidbody selectedObject;

    public float dragForce = 50f;
    public float dragResistance = 10f;

    public float rotationSnap = 90;
    public float scaleSnap = 1;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (selectedObject != null)
        {
            var newPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
            Vector3 force = (newPosition - selectedObject.position) * dragForce;
            selectedObject.AddForce(force, ForceMode.Acceleration);
            
            if (Input.mouseScrollDelta.y > 0)
            {
                selectedObject.transform.localScale += Vector3.one * scaleSnap;
            }
            if (Input.mouseScrollDelta.y < 0)
            {
                selectedObject.transform.localScale -= Vector3.one * scaleSnap;
            }
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
                
                Vector3 alignedForward = NearestWorldAxis(selectedObject.transform.forward);
                Vector3 alignedUp = NearestWorldAxis(selectedObject.transform.up);
                selectedObject.rotation = Quaternion.LookRotation(alignedForward, alignedUp);
                
                selectedObject.constraints = RigidbodyConstraints.FreezeRotation;
            }
        }
        else if (context.started)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out RaycastHit hit);
            if (hit.rigidbody != null)
            {
                targetedObject = hit.rigidbody;
            }
        }
        else if (context.canceled)
        {
            if (selectedObject != null)
            {
                selectedObject.useGravity = true;
                selectedObject.drag = 0;
                selectedObject.constraints = RigidbodyConstraints.None;
                selectedObject.constraints = RigidbodyConstraints.FreezePositionZ;
                selectedObject = null;
            }
            targetedObject = null;
        }
    }

    public void OnRotate(InputAction.CallbackContext context)
    {
        if (selectedObject != null && context.performed)
        {
            Vector2 value = context.ReadValue<Vector2>();
            if (value.x != 0f)
            { 
                selectedObject.transform.Rotate( value.x * rotationSnap, 0f,0f);
            }
            else if (value.y != 0f)
            { 
                selectedObject.transform.Rotate(0f, 0f, value.y * rotationSnap);
            }
        }
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
