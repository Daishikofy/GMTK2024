using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance => instance;
    
    public string firstLevelName;
    
    public BlockController[] blocks;
    
    [Header("Debug")]
    public string currentLoadedLevel;
    public LevelManager currentLevelManager;
    public UIManager uiManager;
    
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        uiManager.UpdateBlockUI(currentLevelManager.blocksInventory);
    }

    public void SpawnBlock(BlockEnum blockType)
    {
        currentLevelManager.SpawnBlock(blockType, blocks);
    }

    public void RestartLevel()
    {
        LoadLevel(currentLoadedLevel);
    }

    public async void LoadLevel(string sceneName)
    {
        await SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
        
        currentLoadedLevel = sceneName;

        currentLevelManager = FindFirstObjectByType<LevelManager>();
        uiManager = FindFirstObjectByType<UIManager>();
        
        uiManager.UpdateBlockUI(currentLevelManager.blocksInventory);
    }
}
