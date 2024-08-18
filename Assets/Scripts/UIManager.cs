using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject gameOverPanel;
    public GameObject gameWinPanel;

    public string sceneName;

    public void Start()
    {
        gameOverPanel.SetActive(false);
        gameWinPanel.SetActive(false);
    }

    public void ShowGameOverPanel()
    {
        gameOverPanel.SetActive(true);
    }

    public void ShowGameWinPanel()
    {
        gameWinPanel.SetActive(true);
    }

    public void OnRestartButton()
    {
        GameManager.Instance?.RestartLevel();
    }

    public void ChangeLevel()
    {
        GameManager.Instance?.LoadLevel(sceneName);
    }
    
}
