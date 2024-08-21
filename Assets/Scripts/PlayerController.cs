using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    
    public BlockController targetedObject;
    public BlockController selectedObject;

    public float dragForce = 50f;
    public float dragResistance = 10f;

    public float rotationSnap = 90;
    public float scaleSnap = 1;

    public float minScale = 0f;
    public float maxScale = 3f;
    
    void FixedUpdate()
    {
        if (selectedObject != null)
        {
            var newPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
            Vector3 force = (newPosition - selectedObject.transform.position) * dragForce;
            selectedObject.BlockRigidbody.AddForce(force, ForceMode.Acceleration);
            
            if (Input.mouseScrollDelta.y > 0)
            {
                if (selectedObject.transform.localScale.x + scaleSnap <= maxScale)
                    selectedObject.transform.localScale += Vector3.one * scaleSnap;
            }
            if (Input.mouseScrollDelta.y < 0)
            {
                if (selectedObject.transform.localScale.x - scaleSnap > minScale)
                    selectedObject.transform.localScale -= Vector3.one * scaleSnap;
            }
        }
    }

    public void OnClick(InputAction.CallbackContext context)
    {
        if (context.performed && selectedObject == null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out RaycastHit hit);
            var block = hit.rigidbody?.gameObject.GetComponent<BlockController>();
            if (block != null && block == targetedObject)
            {
                SelectObject(block);
            }
        }
        else if (context.started)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out RaycastHit hit);
            var block = hit.rigidbody?.gameObject.GetComponent<BlockController>();
            if (block != null)
            {
                targetedObject = block;
            }
        }
        else if (context.canceled)
        {
            if (selectedObject != null)
            {
                selectedObject.ReleaseBlock();
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

    public void SelectObject(BlockController block)
    {
        selectedObject = block;
        selectedObject.SelectBlock(dragResistance);
    }
}
