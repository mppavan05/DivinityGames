using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotiBarManage : MonoBehaviour
{
    public GameObject moreObj;
    public Text titleTxt;
    public Text middleTitleTxt;
    public Text dateTxt;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void MoreButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        moreObj.SetActive(true);

        NotificationListPanel.Instance.scrollParent.transform.GetComponent<ContentSizeFitter>().enabled = false;
        HideButtonClick(this);
        Canvas.ForceUpdateCanvases();
        Invoke(nameof(SizeMaintain), 0.01f);
    }

    void Hide()
    {
        moreObj.SetActive(false);
    }



    public void HideButtonClick(NotiBarManage notiBar)
    {
        for (int i = 0; i < MainMenuManager.Instance.notiBarManages.Count; i++)
        {
            if (MainMenuManager.Instance.notiBarManages[i] != notiBar)
            {
                MainMenuManager.Instance.notiBarManages[i].Hide();
            }
        }
    }
    void SizeMaintain()
    {

        NotificationListPanel.Instance.scrollParent.transform.GetComponent<ContentSizeFitter>().enabled = true;
    }
}