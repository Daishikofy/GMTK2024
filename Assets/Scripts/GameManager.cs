using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance => instance;
    public BlockController[] buildingBlocks;
    
    public ShapeController[] shapeControllers;

    private int currentShape;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        instance = this;
        
        SceneManager.LoadScene("GameUI", LoadSceneMode.Additive);
        foreach (var shape in shapeControllers)
        {
            shape.gameObject.SetActive(false);
        }
        shapeControllers[currentShape].gameObject.SetActive(true);
        buildingBlocks = FindObjectsByType<BlockController>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
    }

    public void Snapshot(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (shapeControllers[currentShape].CheckShape(buildingBlocks))
            {
                if (currentShape < shapeControllers.Length)
                {
                    NextLevel();
                }
                else
                {
                    Debug.Log("YOU WON");
                }
            }
            else
            {
                Debug.Log("Game over");
            }
        }
    }

    private void NextLevel()
    {
        shapeControllers[currentShape].gameObject.SetActive(false);
        currentShape++;
        shapeControllers[currentShape].gameObject.SetActive(true);
    }

    public void FallingCastle()
    {
        Time.timeScale = 0.2f;
    }
}
