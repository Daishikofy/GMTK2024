using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject gameOverPanel;
    public GameObject gameWinPanel;
    public GameObject gameEndPanel;

    public string sceneName;

    public void Start()
    {
        gameOverPanel.SetActive(false);
        gameWinPanel.SetActive(false);
        gameEndPanel.SetActive(false);
    }

    public void ShowGameOverPanel()
    {
        gameEndPanel.SetActive(true);
        gameOverPanel.SetActive(true);
    }

    public void ShowGameWinPanel()
    {
        gameEndPanel.SetActive(true);
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
