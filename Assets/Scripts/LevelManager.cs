using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public ShapeController[] shapeControllers;
    public PlayerController playerController;
    [Header("Debug")]
    public List<BlockController> buildingBlocks;
    public int[] blocksInventory = new int[6];
    private int currentShape;
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
            var position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y - 1, 10f));
            BlockController newBlock = Instantiate(blocks[blockIndex], position, Quaternion.identity);
            buildingBlocks.Add(newBlock);
            playerController.SelectObject(newBlock.BlockRigidbody);
        }
    }

    public void Snapshot(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            ValidateCurrentShape();
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
        Vector3 oldPosition = shapeControllers[currentShape].shapeCenter.position;
        shapeControllers[currentShape].gameObject.SetActive(false);
        currentShape++;
        shapeControllers[currentShape].gameObject.SetActive(true);

        Vector3 cameraMovement = shapeControllers[currentShape].shapeCenter.position - oldPosition;
        Camera.main.transform.position += cameraMovement;
        SumInventory(shapeControllers[currentShape].blocksInventory);
        
    }

    public void FallingCastle()
    {
        Time.timeScale = 0.2f;
        GameManager.Instance.uiManager.ShowGameOverPanel();
    }

    private void SumInventory(int[] inventory)
    {
        for (int i = 0; i < 6; i++)
        {
            blocksInventory[i] += inventory[i];
        }
    }
}
