using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public ShapeController[] shapeControllers;
    public PlayerController playerController;

    public float cameraLerpFactor = 0.5f;
    [Header("Debug")]
    public List<BlockController> buildingBlocks;
    public int[] blocksInventory = new int[6];
    private int currentShape;
    
    public bool DEBUG = false;
    public Camera mainCamera;
    public Vector3 cameraTarget;

    private bool gameOver = false;
    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (cameraTarget != Vector3.zero)
        {
            if (Vector3.Distance(mainCamera.transform.position, cameraTarget) > .1f)
            {
                mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, cameraTarget, cameraLerpFactor);
            }
            else
            {
                cameraTarget = Vector3.zero;
            }
        }
    }
    
    // Start is called before the first frame update
    void Awake()
    {
        foreach (var shape in shapeControllers)
        {
            shape.gameObject.SetActive(false);
        }
        
        shapeControllers[currentShape].gameObject.SetActive(true);
        SumInventory(shapeControllers[currentShape].blocksInventory);
    }
    
    public void SpawnBlock(BlockEnum blockType, BlockController[] blocks)
    {
        int blockIndex = (int)blockType;
        if (blocksInventory[blockIndex] > 0)
        {
            Debug.Log("Block available");
            blocksInventory[blockIndex]--;
            
            GameManager.Instance.uiManager.UpdateBlockUI(blocksInventory);
            var position = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y - 1, 10f));
            BlockController newBlock = Instantiate(blocks[blockIndex], position, Quaternion.identity);
            buildingBlocks.Add(newBlock);
            playerController.SelectObject(newBlock);
        }
    }

    public void Snapshot(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (DEBUG)
            {
                if (currentShape + 1 < shapeControllers.Length)
                {
                    NextLevel();
                }
            }
            else
            {
                ValidateCurrentShape();
                
            }
        }
    }

    public void ValidateCurrentShape()
    {
        if (shapeControllers[currentShape].CheckShape(buildingBlocks))
        {
            if (currentShape + 1 < shapeControllers.Length)
            {
                NextLevel();
            }
            else
            {
                shapeControllers[currentShape].gameObject.SetActive(false);
                GameManager.Instance.uiManager.ShowGameWinPanel();
            }
        }
        else
        {
            Debug.Log("Game over");
        }
    }
    
    private void NextLevel()
    {
        var oldPosition = shapeControllers[currentShape].shapeCenter.position;
        shapeControllers[currentShape].gameObject.SetActive(false);
        currentShape++;
        shapeControllers[currentShape].gameObject.SetActive(true);
        
        var cameraDirection = shapeControllers[currentShape].shapeCenter.position - oldPosition;
        cameraTarget = mainCamera.transform.position + cameraDirection;
        
        SumInventory(shapeControllers[currentShape].blocksInventory);
        GameManager.Instance.uiManager.UpdateBlockUI(blocksInventory);
    }

    public void FallingCastle(Vector3 fallingBlock)
    {
        if (!gameOver)
        {
            var cameraDirection = fallingBlock - shapeControllers[currentShape].transform.position;
            cameraTarget = mainCamera.transform.position + cameraDirection;

            Time.timeScale = 0.2f;
            GameManager.Instance.uiManager.ShowGameOverPanel();
            gameOver = true;
        }
    }

    private void SumInventory(int[] inventory)
    {
        for (int i = 0; i < 6; i++)
        {
            blocksInventory[i] += inventory[i];
        }
    }
}
