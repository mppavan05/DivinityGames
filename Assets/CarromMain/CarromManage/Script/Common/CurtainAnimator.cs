using UnityEngine;

public class CurtainAnimator : MonoBehaviour
{
    public CarromGameManager manager;

    public OfflineGameManager offlinemanager;

    private void Start()
    {
    }

    private void Update()
    {
    }

    public void CurtainClosed()
    {
        if (manager != null)
        {
            //manager.CurtainClosed();
        }
        if (offlinemanager != null)
        {
            offlinemanager.CurtainClosed();
        }
    }
}
