using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance => instance;
    
    public string firstLevelName;
    public string UISceneName = "GameUI";
    
    [Header("Debug")]
    public string mainSceneName;
    public string currentLoadedLevel;
    
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        
        /////
        
        if (SceneManager.GetSceneByName(UISceneName).name != UISceneName)
        {
            SceneManager.LoadScene(UISceneName, LoadSceneMode.Additive);
        }
        
        if (SceneManager.GetSceneByName(firstLevelName).name != firstLevelName)
        {
            LoadLevel(firstLevelName);
        }
        else
        {
            currentLoadedLevel = firstLevelName;
        }
    }

    public void RestartLevel()
    {
        LoadLevel(currentLoadedLevel);
    }

    public async void LoadLevel(string sceneName)
    { 
        if (sceneName != UISceneName)
        {
           await SceneManager.UnloadSceneAsync(currentLoadedLevel);
        }  
        
        await SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
        
        currentLoadedLevel = sceneName;
    }
}
