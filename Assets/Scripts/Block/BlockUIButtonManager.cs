using UnityEngine;

public class BlockUIButtonManager : MonoBehaviour
{
    public BlockUIButton[] blockButtons;

    public void UpdateBlockCount(int[] blockCount)
    {
        for (int i = 0; i < 6; i++)
        {
            blockButtons[i].SetValue(blockCount[i]);
        }
    }


}
