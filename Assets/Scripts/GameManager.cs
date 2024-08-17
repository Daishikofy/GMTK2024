using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    
    public BlockController[] buildingBlocks;
    
    public ShapeController ShapeController;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Snapshot(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            var newShape = ShapeController.ValidateShape(buildingBlocks);
        }
    }
}
