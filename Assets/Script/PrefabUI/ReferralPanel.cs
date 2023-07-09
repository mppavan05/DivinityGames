using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReferralPanel : MonoBehaviour
{
    public static ReferralPanel Instance;
    public Text refferalTxt;
    

    [SerializeField] private GameObject m_ReferalPanel;
    [SerializeField] private GameObject m_EarnPanel;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        
    }

    private void OnEnable()
    {
        if (MainMenuManager.Instance != null)
        {
            MainMenuManager.Instance.screenObj.Add(this.gameObject);
        }
        refferalTxt.text = "Your Referral Code : " + DataManager.Instance.playerData.refer_code;
    }



    public void CloseButtonClick()
    {
        SoundManager.Instance.ButtonClick();

        if (MainMenuManager.Instance != null)
        {
            MainMenuManager.Instance.screenObj.Remove(this.gameObject);
        }

        Destroy(this.gameObject);
    }

    


    public void ShareButtonClick()
    {
        SoundManager.Instance.ButtonClick();

        string shareTxt = "Download Latest Divinity apk from Link Here : \n\n" + DataManager.Instance.appUrl + " \n\nUse this referral code :" + DataManager.Instance.playerData.refer_code;

        new NativeShare().Share(shareTxt);
    }
    //public void OnEarnPanel()
    //{
    //    m_EarnPanel.SetActive(true);
    //    m_ReferPanel.SetActive(false);
    //}
    //public void OnReferPanel()
    //{
    //    m_ReferPanel.SetActive(true);
    //    m_EarnPanel.SetActive(false);

    //}
    

    public void ReferDialogBtn(int no)
    {

        SoundManager.Instance.ButtonClick();
        switch (no)
        {
            case 1:
                m_ReferalPanel.SetActive(true);
                m_EarnPanel.SetActive(false);
                break;


            case 2:
                m_EarnPanel.SetActive(true);
                m_ReferalPanel.SetActive(false);
                break;



        }
    }




}
