using UnityEngine;

public class PowerMiniGameUI : MonoBehaviour
{
    public void OnClickRestorePower()
    {
        if (Stage3PowerManager.Instance != null)
            Stage3PowerManager.Instance.RestorePower();
    }

    public void OnClickClose()
    {
        if (Stage3PowerManager.Instance != null)
            Stage3PowerManager.Instance.CloseMiniGame();
    }
}