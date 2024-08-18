using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class BlockUIButton : MonoBehaviour
{
    public Button button;
    public TextMeshProUGUI buttonText;
    public BlockEnum blockType;
    public int currentValue = 0;
    
    private BlockUIButtonManager blockUIButtonManager;
    private void Start()
    {
        UpdateButtonState();
    }

    public void SetValue(int value)
    {
        currentValue = value;
        UpdateButtonState();
    }

    private void UpdateButtonState()
    {
        buttonText.text = currentValue.ToString();
        if (currentValue == 0)
        {
            button.interactable = false;
        }
    }

    public void OnClick()
    {
        GameManager.Instance.SpawnBlock(blockType);
    }
}
