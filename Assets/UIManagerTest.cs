using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.SceneManagement;
using WebSocketSharp;

public class UIManagerTest : MonoBehaviour
{
    public static UIManagerTest instance;

    [SerializeField] private GameObject ReferandEarnPanel;
    [SerializeField] private GameObject EventPanel;
    [SerializeField] private GameObject MailPanel;
    [SerializeField] private GameObject SettingPanel;
    [SerializeField] private GameObject Store;
    [SerializeField] private GameObject Rules;
    [SerializeField] private GameObject ParentShopObj;
    [SerializeField] private GameObject RankingPanel;
    [SerializeField] private GameObject ServicesPanel;
    [SerializeField] private GameObject FriendsPanel;
    [SerializeField] private GameObject BonusPanel;
    [SerializeField] private GameObject VIPPanel;

    [Header("Rummy")]
    [SerializeField] private GameObject RummyModes;
    [SerializeField] private GameObject RummyTournament;

    [Header("Setting")]
    public GameObject Menue;
    public GameObject Back;

    [Header("Withdraw")]
    public GameObject WithdrawMode;

    public GameObject LudoCommingSoonPanel;
    
    private void Awake()
    {
        instance = this;
    }
    public void EventPanelShow()
    {
        EventPanel.SetActive(true);
    }

    public void BackMenue()
    {
        SceneManager.LoadScene("Main");
    }
    public void LoadLudo()
    {
        SceneManager.LoadScene("Ludo");
    }
    public void LoadPoker()
    {
        SceneManager.LoadScene("Poker");
    }
    public void LoadRummy()
    {
        SceneManager.LoadScene("Rummy");
    }
    public void LoadPool()
    {
        SceneManager.LoadScene("8BallPool");
    }
    public void LoadSlot()
    {
        SceneManager.LoadScene("Slots");
    }
    public void LoadChess()
    {
        SceneManager.LoadScene("Chess");
    }
    public void LoadCarrom()
    {
        SceneManager.LoadScene("Carrom");
    }
    public void LoadArchery()
    {
        SceneManager.LoadScene("Archery");
    }
    public void LoadTeenPatti()
    {
        SceneManager.LoadScene("TeenPatti");
    }
    public void CLoseWindow()
    {
        ReferandEarnPanel.SetActive(false);
    }
    public void CLoseEventPanle()
    {
        EventPanel.SetActive(false);
    }
    public void OpenMailPanel()
    {
        MailPanel.SetActive(true);
    }
    public void CloseMailPanel()
    {
        MailPanel.SetActive(false);
    }
    public void OpenReferandEarnPanel()
    {
        ReferandEarnPanel.SetActive(true);
    }
    public void OpenSEttingPanel()
    {
        Debug.Log("clicked");
        SettingPanel.SetActive(true);
    }
    public void CloseSettingPanel()
    {
        SettingPanel.SetActive(false);
    }
    public void StoreOpen()
    {
        Instantiate(Store, ParentShopObj.transform);
    }
    public void RulesOpen()
    {
        Rules.SetActive(true);
    }
    public void CLoseRule()
    {
        Rules.SetActive(false);
    }
    public void OpenRankingPanel()
    {
        RankingPanel.SetActive(true);
    }
    public void CloseRankingPanel()
    {
        RankingPanel?.SetActive(false);
    }
    public void OpenServicesPanel()
    {
        ServicesPanel.SetActive(true);
    }
    public void CloseServicesPanel()
    {
        ServicesPanel.SetActive(false);
    }
    public void OpenFriendsPanel()
    {
        FriendsPanel.SetActive(true);
    }
    public void CloseFriendsPanel()
    {
        FriendsPanel.SetActive(false);
    }
    public void OpenBonusPanel()
    {
        BonusPanel.SetActive(true);
    }
    public void CloseBonus()
    {
        BonusPanel?.SetActive(false);
    }
    public void OpenVIPPanel()
    {
        VIPPanel.SetActive(true);   
    }
    public void CloseVIPPanel()
    {
        VIPPanel.SetActive(false);
    }
    public void OpenRummyModes()
    {
        RummyModes.SetActive(true);
    }
    public void CloseRummyModes()
    {
        RummyModes.SetActive(false);
    }
    public void OpenRummyTournament()
    {
        RummyTournament.SetActive(true);
    }
    public void CloseRummyTournament()
    {
        RummyTournament.SetActive(false);
    }
    public void OpenMainMeneue()
    {
        Menue.SetActive(true);
    }
    public void OpenBackMenue()
    {
        Back.SetActive(true);
    }
    public void CLoseMenue()
    {
        Menue.SetActive(false);
    }
    public void CloseBack()
    {
        Back.SetActive(false);
    }
    public void OpenWithdrawMode()
    {
        WithdrawMode.SetActive(true);
    }

    public void OpenCommingSoonPanel()
    {
        LudoCommingSoonPanel.SetActive(true);
    }
    public void CloseCommingSoonPanel()
    {
        LudoCommingSoonPanel.SetActive(false);
    }

}
