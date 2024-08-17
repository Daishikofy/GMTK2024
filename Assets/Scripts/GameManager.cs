using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    
    public BlockController[] buildingBlocks;
    
    public ShapeController ShapeController;
    // Start is called before the first frame update
    void Awake()
    {
        SceneManager.LoadScene("GameUI", LoadSceneMode.Additive);
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
