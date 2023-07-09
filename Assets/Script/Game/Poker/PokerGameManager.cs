using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


[System.Serializable]
public class PokerWinDataMaintain
{
    public int ruleNo;
    public List<CardSuffle> winList;

}
public class PokerWinDataWithPlayer
{
    public int ruleNo;
    public List<CardSuffle> winList;
    public PokerPlayer player;

}

public class PokerGameManager : MonoBehaviour
{
    public static PokerGameManager Instance;


    [Header("---Game Bet---")]
    public float sbAmount;
    public float bbAmount;
    public float lastPrice;
    public bool isWin;
    public float timerSpeed;
    public bool isAllIn;

    [Header("--- Bet ---")]
    public GameObject betPrefab;
    public GameObject targetBetObj;
    public float player1BetAmount;
    public float player2BetAmount;
    public float player3BetAmount;
    public float player4BetAmount;
    public float player5BetAmount;

    [Header("---Poker Game UI---")]
    public GameObject errorScreenObj;
    public bool isAdmin;
    public int playerNo;
    public bool isGameStop;
    public GameObject cardTmpPrefab;
    public GameObject prefabParent;
    public GameObject cardTmpStart;
    public GameObject playerFindScreenObj;
    public Sprite simpleCardSprite;
    public Sprite packCardSprite;
    public Text potTxt;
    public float potAmount;
    public float totalBetAmount;
    private int[] numbers = { 5, 10, 50, 100, 250, 500, 1000 };
    private int currentIndex = 0;

    [Header("---Poker Down On Object---")]
    public GameObject downObjectOnObj;
    public GameObject flodBtn;
    public GameObject callBtn;
    public GameObject allInBtn;
    public GameObject raiseBtn;
    public Text callPriceTxt;
    public Text raisePriceTxt;
    public float raisePrice = 0;


    [Header("---Second Panel---")]
    public GameObject secondScreenObj;
    public GameObject secondSubScreenObj;
    public GameObject secondUpBtnObj;
    public Slider sliderValue;
    public Button minusBtn;
    public Button plusBtn;

    [Header("---Game Play---")]
    public int gameDealerNo;
    public List<CardSuffle> cardSuffles = new List<CardSuffle>();
    public List<ListStoreData> listStoreDatas = new List<ListStoreData>();
    public List<int> mainList = new List<int>();
    public List<CardSuffle> cardSufflesGen = new List<CardSuffle>();
    public List<CardSuffle> cardSufflesSort = new List<CardSuffle>();

    public List<CardSuffle> newCardSS = new List<CardSuffle>();
    public List<CardSuffle> newCardSS1 = new List<CardSuffle>();
    public List<CardSuffle> cardResult = new List<CardSuffle>();


    public List<PokerPlayer> pokerPlayers = new List<PokerPlayer>();
    public List<PokerPlayer> playerSquList = new List<PokerPlayer>();
    public PokerPlayer player1;
    public PokerPlayer player2;
    public PokerPlayer player3;
    public PokerPlayer player4;
    public PokerPlayer player5;

    [Header("--- Down Object Off ---")]

    public GameObject waitNextRoundScreenObj;
    public GameObject downObjectOff;
    public GameObject[] tickObj;
    public Image[] blackBtnObj;
    public Sprite blackBtnOn;
    public Sprite blackBtnOff;


    [Header("--- Menu Screen ---")]
    public GameObject menuScreenObj;
    public GameObject settingsScreenObj;

    [Header("--- Rule Screen ---")]
    public GameObject ruleScreenObj;

    [Header("--- Prefab ---")]
    public GameObject shopPrefab;
    public GameObject shopPrefabParent;

    [Header("--- Open Message Screen ---")]
    public GameObject messageScreeObj;
    public GameObject giftScreenObj;
    
    public GameObject soundOn;
    public GameObject soundOff;
    public GameObject vibOn;
    public GameObject vibOff;
    public GameObject sfxOn;
    public GameObject sfxOff;
    

    [Header("--- Chat Panel ---")]
    public GameObject chatPanelParent;
    public GameObject chatMePrefab;
    public GameObject chatOtherPrefab;

    [Header("--- Gift Maintain ---")]
    public GameObject giftParentObj;
    public GameObject giftPrefab;
    public List<GiftBox> giftBoxes = new List<GiftBox>();


    [Header("--- Cards Maintain ---")]
    public CardSuffle card1;
    public CardSuffle card2;
    public CardSuffle card3;
    public CardSuffle card4;
    public CardSuffle card5;

    public GameObject card1Pos;
    public GameObject card2Pos;
    public GameObject card3Pos;
    public GameObject card4Pos;
    public GameObject card5Pos;

    public GameObject startCard;
    public GameObject commonCard;
    public Sprite commonCardImg;
    
    public bool isGameStarted;



    bool isCheck_Off = false;
    bool isFold_Off = false;
    bool isCall_Off = false;
    private bool _allBetEqual;
    private bool _isFlopShowDone;
    private bool _isRiverShowDone;
    
    
    public bool isBotActivate;
    public int counter;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }


    public void PlayerFound()
    {
        print("Enter The Player Found Screen");
        if (DataManager.Instance.joinPlayerDatas.Count >= TestSocketIO.Instace.pokerRequirePlayer)
        {
            CreateAdmin();
            if (DataManager.Instance.joinPlayerDatas.Count > 5)
            {
                StartCoroutine(WaitGameToComplete(CheckNewPlayers));
            }
            if (DataManager.Instance.joinPlayerDatas.Count == 5 && isAdmin)
            {
                if (!isGameStarted)
                {
                    StartGamePlay();
                }
            }
            else
            {
                if (isAdmin) return;
                if (!isGameStarted)
                {
                    waitNextRoundScreenObj.SetActive(true);
                }
            }
        }
        else
        {
            playerFindScreenObj.SetActive(true);
        }
    }
    
    private IEnumerator WaitGameToComplete(Action callback)
    {
        yield return new WaitUntil(() => !isGameStarted);
        callback();
    }
    
    private IEnumerator WaitGameToCompleteRemovePlayer(System.Action<int> callback, int parameter)
    {
        yield return new WaitUntil(() => !isGameStarted);
        callback(parameter);
    }
    
    
    public void CheckNewPlayers()
    {
        // removing bot players from list with string common bot string name
        DataManager.Instance.joinPlayerDatas = DataManager.Instance.joinPlayerDatas.Where(player => !player.avtar.StartsWith("http://206.189.140.131/assets/img/profile-picture/")).ToList();
        // assiging new remaining bot players
        if (DataManager.Instance.joinPlayerDatas.Count <= 4)
        {
            MainMenuManager.Instance.CheckPlayers();
        }
        ResetBot();
        //Activating bots
        ActivateBotPlayers();
        
        print("_______________________This Function is called ---------------------------------");
    }
    
    public void CheckLeftPlayer(int index)
    {
        //DataManager.Instance.joinPlayerDatas.Remove(DataManager.Instance.joinPlayerDatas[index]);
        DataManager.Instance.joinPlayerDatas = DataManager.Instance.joinPlayerDatas.Where(player => !player.avtar.StartsWith("http://206.189.140.131/assets/img/profile-picture/")).ToList();
        if (DataManager.Instance.joinPlayerDatas.Count <= 4)
        {
            MainMenuManager.Instance.CheckPlayers();
        }
        ResetBot();
        //Activating bots
        ActivateBotPlayers();
    }


    // Start is called before the first frame update
    void Start()
    {
        SoundManager.Instance.StopBackgroundMusic();
        //OpenOffScreen();
        potTxt.text = "0";
        lastPrice = 5f;
        PlayerFound();
        sliderValue.onValueChanged.AddListener(delegate { OnSliderValueChanged(); });
        //StartCoroutine(DisplayCards());
        ManageSoundButtons();
    }

    void Update()
    {
        playerNo = player1.playerNo;
        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            //Invoke(nameof(DisplayCards), 2.5f);
        }*/

        UpdateBetAmount();
    }

    public IEnumerator DisplayCards()
    {
        yield return new WaitForSeconds(1.02f);
        
    }


    void WinBeforeAllDataManage()
    {
        CancelInvoke(nameof(CheckBetAmount));
        /*List<PokerWinDataWithPlayer> winData = new List<PokerWinDataWithPlayer>();
        for (int i = 0; i < pokerPlayers.Count; i++)
        {
            if (pokerPlayers[i].isFold == false && pokerPlayers[i].gameObject.activeSelf == true)
            {
                PokerWinDataMaintain data = pokerPlayers[i].CardDisplay();
                PokerWinDataWithPlayer passData = new PokerWinDataWithPlayer();
                if (data.winList.Count == 5)
                {
                    passData.ruleNo = data.ruleNo;
                    passData.winList = data.winList;
                    passData.player = pokerPlayers[i];
                    winData.Add(passData);
                }
            }
        }


        if (winData.Count == 1)
        {
            //win amount = pot amount
            string winValue = ",";
            winValue += winData[0].player + ",";
            if (winData[0].player.playerNo == playerNo)
            {
                if (winData[0].player.playerNo == playerNo)
                {
                    SetPokerWon(winValue);
                }
            }
            for (int i = 0; i < winData[0].player.playerWinObj.Length; i++)
            {
                winData[0].player.playerWinObj[i].SetActive(true);
            }
            
            StartCoroutine(RestartGamePlay());
        }
        else if (winData.Count == 2)
        {
            int highestRuleNo = 0;
            for (int i = 0; i < winData.Count; i++)
            {
                if (highestRuleNo < winData[i].ruleNo)
                {
                    highestRuleNo = winData[i].ruleNo;
                }
            }

            List<PokerWinDataWithPlayer> sortList1 = new List<PokerWinDataWithPlayer>();
            int highPlayerCnt = 0;
            for (int i = 0; i < winData.Count; i++)
            {
                if (highestRuleNo == winData[i].ruleNo)
                {

                    sortList1.Add(winData[i]);
                    highPlayerCnt++;
                }
            }
            if (highPlayerCnt == 1)
            {
                string winValue = ",";
                winValue += winData[0].player + ",";
                if (winData[0].player.playerNo == playerNo)
                {
                    if (winData[0].player.playerNo == playerNo)
                    {
                        SetPokerWon(winValue);
                    }
                }
                for (int i = 0; i < winData.Count; i++)
                {
                    if (winData[i].ruleNo == highestRuleNo)
                    {
                        for (int j = 0; j < winData[0].player.playerWinObj.Length; j++)
                        {
                            winData[i].player.playerWinObj[j].SetActive(true);
                        }
                    }
                }
                StartCoroutine(RestartGamePlay());
            }
            else if (highPlayerCnt == 2)
            {
                if (highestRuleNo == 1)
                {

                }
                else if (highestRuleNo == 2)
                {
                    int mainNo = winData[0].winList[0].cardNo;
                    //for (int i = 0; i <sortList1.Count;i++)
                    //{
                    //    if(sortList1[i].winList[0].cardNo<)
                    //}
                }
                else if (highestRuleNo == 3)
                {

                }
                else if (highestRuleNo == 4)
                {

                }
                else if (highestRuleNo == 5)
                {

                }
                else if (highestRuleNo == 6)
                {

                }
                else if (highestRuleNo == 7)
                {

                }
                else if (highestRuleNo == 8)
                {

                }
                else if (highestRuleNo == 9)
                {

                }
            }
        }
        else if (winData.Count == 3)
        {
             int highestRuleNo = 0;
            for (int i = 0; i < winData.Count; i++)
            {
                if (highestRuleNo < winData[i].ruleNo)
                {
                    highestRuleNo = winData[i].ruleNo;
                }
            }

            List<PokerWinDataWithPlayer> sortList1 = new List<PokerWinDataWithPlayer>();
            int highPlayerCnt = 0;
            for (int i = 0; i < winData.Count; i++)
            {
                if (highestRuleNo == winData[i].ruleNo)
                {

                    sortList1.Add(winData[i]);
                    highPlayerCnt++;
                }
            }
            if (highPlayerCnt == 1)
            {
                for (int i = 0; i < winData.Count; i++)
                {
                    if (winData[i].ruleNo == highestRuleNo)
                    {
                        for (int j = 0; j < winData[0].player.playerWinObj.Length; j++)
                        {
                            winData[i].player.playerWinObj[j].SetActive(true);
                        }
                    }
                }
            }
            else if (highPlayerCnt == 2)
            {
                if (highestRuleNo == 1)
                {

                }
                else if (highestRuleNo == 2)
                {
                    int mainNo = winData[0].winList[0].cardNo;
                    //for (int i = 0; i <sortList1.Count;i++)
                    //{
                    //    if(sortList1[i].winList[0].cardNo<)
                    //}
                }
                else if (highestRuleNo == 3)
                {

                }
                else if (highestRuleNo == 4)
                {

                }
                else if (highestRuleNo == 5)
                {

                }
                else if (highestRuleNo == 6)
                {

                }
                else if (highestRuleNo == 7)
                {

                }
                else if (highestRuleNo == 8)
                {

                }
                else if (highestRuleNo == 9)
                {

                }
            }

        }
        else if (winData.Count == 4)
        {
            int highestRuleNo = 0;
            for (int i = 0; i < winData.Count; i++)
            {
                if (highestRuleNo < winData[i].ruleNo)
                {
                    highestRuleNo = winData[i].ruleNo;
                }
            }

            List<PokerWinDataWithPlayer> sortList1 = new List<PokerWinDataWithPlayer>();
            int highPlayerCnt = 0;
            for (int i = 0; i < winData.Count; i++)
            {
                if (highestRuleNo == winData[i].ruleNo)
                {

                    sortList1.Add(winData[i]);
                    highPlayerCnt++;
                }
            }
            if (highPlayerCnt == 1)
            {
                for (int i = 0; i < winData.Count; i++)
                {
                    if (winData[i].ruleNo == highestRuleNo)
                    {
                        for (int j = 0; j < winData[0].player.playerWinObj.Length; j++)
                        {
                            winData[i].player.playerWinObj[j].SetActive(true);
                        }
                    }
                }
            }
            else if (highPlayerCnt == 2)
            {
                if (highestRuleNo == 1)
                {

                }
                else if (highestRuleNo == 2)
                {
                    int mainNo = winData[0].winList[0].cardNo;
                    //for (int i = 0; i <sortList1.Count;i++)
                    //{
                    //    if(sortList1[i].winList[0].cardNo<)
                    //}
                }
                else if (highestRuleNo == 3)
                {

                }
                else if (highestRuleNo == 4)
                {

                }
                else if (highestRuleNo == 5)
                {

                }
                else if (highestRuleNo == 6)
                {

                }
                else if (highestRuleNo == 7)
                {

                }
                else if (highestRuleNo == 8)
                {

                }
                else if (highestRuleNo == 9)
                {

                }
            }

        }
        else if (winData.Count == 5)
        {
             int highestRuleNo = 0;
            for (int i = 0; i < winData.Count; i++)
            {
                if (highestRuleNo < winData[i].ruleNo)
                {
                    highestRuleNo = winData[i].ruleNo;
                }
            }

            List<PokerWinDataWithPlayer> sortList1 = new List<PokerWinDataWithPlayer>();
            int highPlayerCnt = 0;
            for (int i = 0; i < winData.Count; i++)
            {
                if (highestRuleNo == winData[i].ruleNo)
                {

                    sortList1.Add(winData[i]);
                    highPlayerCnt++;
                }
            }
            if (highPlayerCnt == 1)
            {
                string winValue = ",";
                winValue += sortList1[0].player + ",";
                if (sortList1[0].player.playerNo == playerNo)
                {
                    if (sortList1[0].player.playerNo == playerNo)
                    {
                        SetPokerWon(winValue);
                    }
                }
                for (int i = 0; i < winData.Count; i++)
                {
                    if (winData[i].ruleNo == highestRuleNo)
                    {
                        for (int j = 0; j < winData[0].player.playerWinObj.Length; j++)
                        {
                            winData[i].player.playerWinObj[j].SetActive(true);
                        }
                    }
                }
                StartCoroutine(RestartGamePlay());
            }
            else if (highPlayerCnt == 2)
            {
                if (highestRuleNo == 1)
                {
                    string winValue = ",";
                    winValue += sortList1[1].player + ",";
                    if (sortList1[1].player.playerNo == playerNo)
                    {
                        if (sortList1[1].player.playerNo == playerNo)
                        {
                            SetPokerWon(winValue);
                        }
                    }
                    for (int i = 0; i < winData.Count; i++)
                    {
                        if (winData[i].ruleNo == highestRuleNo)
                        {
                            for (int j = 0; j < winData[0].player.playerWinObj.Length; j++)
                            {
                                winData[i].player.playerWinObj[j].SetActive(true);
                            }
                        }
                    }
                    StartCoroutine(RestartGamePlay());
                }
                else if (highestRuleNo == 2)
                {
                    int mainNo = winData[0].winList[0].cardNo;
                    for (int i = 0; i <sortList1.Count;i++)
                    {
                        if (sortList1[i].winList[0].cardNo > mainNo)
                        {
                            for (int j = 0; j < winData[0].player.playerWinObj.Length; j++)
                            {
                                winData[i].player.playerWinObj[j].SetActive(true);
                            }
                        }
                        else
                        {
                            for (int j = 0; j < winData[0].player.playerWinObj.Length; j++)
                            {
                                winData[i].player.playerWinObj[j].SetActive(true);
                            }
                        }
                    }
                    StartCoroutine(RestartGamePlay());
                }
                else if (highestRuleNo == 3)
                {
                    int firstCard1 = 0;
                    int firstCard2 = 0;
                    
                    int secondCard1 = 0;
                    int secondCard2 = 0;
                    

                    int mainCardNum = sortList1[0].winList[0].cardNo;
                    foreach (var t in sortList1[0].winList)
                    {
                        t.cardNo = t.cardNo == mainCardNum ? firstCard1 : firstCard2;
                    }
                    int secondMainCardNum = sortList1[1].winList[0].cardNo;
                    foreach (var t in sortList1[1].winList)
                    {
                        t.cardNo = t.cardNo == secondMainCardNum ? secondCard1 : secondCard2;
                    }

                    if (firstCard1 == secondCard1)
                    {
                        if (firstCard2 > secondCard2)
                        {
                            for (int j = 0; j < winData[0].player.playerWinObj.Length; j++)
                            {
                                sortList1[0].player.playerWinObj[j].SetActive(true);
                            }
                        }
                        else
                        {
                            for (int j = 0; j < winData[0].player.playerWinObj.Length; j++)
                            {
                                sortList1[1].player.playerWinObj[j].SetActive(true);
                            }
                        }
                    }

                    if (firstCard1 > secondCard1)
                    {
                        for (int j = 0; j < winData[0].player.playerWinObj.Length; j++)
                        {
                            sortList1[0].player.playerWinObj[j].SetActive(true);
                        }
                    }
                    else
                    {
                        for (int j = 0; j < winData[0].player.playerWinObj.Length; j++)
                        {
                            sortList1[1].player.playerWinObj[j].SetActive(true);
                        }
                    }
                    StartCoroutine(RestartGamePlay());
                }
                else if (highestRuleNo == 4)
                {
                    StartCoroutine(RestartGamePlay());

                }
                else if (highestRuleNo == 5)
                {
                    StartCoroutine(RestartGamePlay());

                }
                else if (highestRuleNo == 6)
                {
                    StartCoroutine(RestartGamePlay());

                }
                else if (highestRuleNo == 7)
                {

                }
                else if (highestRuleNo == 8)
                {

                }
                else if (highestRuleNo == 9)
                {
                    StartCoroutine(RestartGamePlay());
                }
            }
            else if (highPlayerCnt == 3)
            {
                StartCoroutine(RestartGamePlay());
            }
            else if (highPlayerCnt == 4)
            {
                StartCoroutine(RestartGamePlay());
            }
            else if (highPlayerCnt == 5)
            {
                 if (highestRuleNo == 1)
                 {
                     for (int i = 0; i < winData.Count; i++)
                     {
                         if (winData[i].ruleNo == highestRuleNo)
                         {
                             for (int j = 0; j < winData[0].player.playerWinObj.Length; j++)
                             {
                                 winData[i].player.playerWinObj[j].SetActive(true);
                             }
                         }
                     }
                     StartCoroutine(RestartGamePlay());
                 }
                 else if (highestRuleNo == 2)
                 {
                     int mainNo = winData[0].winList[0].cardNo;
                     for (int i = 0; i <sortList1.Count;i++)
                     {
                         if (sortList1[i].winList[0].cardNo > mainNo)
                         {
                             for (int j = 0; j < winData[0].player.playerWinObj.Length; j++)
                             {
                                 winData[i].player.playerWinObj[j].SetActive(true);
                             }
                         }
                         else
                         {
                             for (int j = 0; j < winData[0].player.playerWinObj.Length; j++)
                             {
                                 winData[i].player.playerWinObj[j].SetActive(true);
                             }
                         }
                     }
                     StartCoroutine(RestartGamePlay());
                    
                 }
                 else if (highestRuleNo == 3)
                 {
                     int firstCard1 = 0;
                     int firstCard2 = 0;
                    
                     int secondCard1 = 0;
                     int secondCard2 = 0;
                    

                     int mainCardNum = sortList1[0].winList[0].cardNo;
                     foreach (var t in sortList1[0].winList)
                     {
                         t.cardNo = t.cardNo == mainCardNum ? firstCard1 : firstCard2;
                     }
                     int secondMainCardNum = sortList1[1].winList[0].cardNo;
                     foreach (var t in sortList1[1].winList)
                     {
                         t.cardNo = t.cardNo == secondMainCardNum ? secondCard1 : secondCard2;
                     }

                     if (firstCard1 == secondCard1)
                     {
                         if (firstCard2 > secondCard2)
                         {
                             for (int j = 0; j < winData[0].player.playerWinObj.Length; j++)
                             {
                                 sortList1[0].player.playerWinObj[j].SetActive(true);
                             }
                         }
                         else
                         {
                             for (int j = 0; j < winData[0].player.playerWinObj.Length; j++)
                             {
                                 sortList1[1].player.playerWinObj[j].SetActive(true);
                             }
                         }
                     }

                     if (firstCard1 > secondCard1)
                     {
                         for (int j = 0; j < winData[0].player.playerWinObj.Length; j++)
                         {
                             sortList1[0].player.playerWinObj[j].SetActive(true);
                         }
                     }
                     else
                     {
                         for (int j = 0; j < winData[0].player.playerWinObj.Length; j++)
                         {
                             sortList1[1].player.playerWinObj[j].SetActive(true);
                         }
                     }
                     StartCoroutine(RestartGamePlay());
                 }
                 else if (highestRuleNo == 4)
                 {
                     int mainNo = winData[0].winList[0].cardNo;
                     for (int i = 0; i <sortList1.Count;i++)
                     {
                         if (sortList1[i].winList[0].cardNo > mainNo)
                         {
                             for (int j = 0; j < winData[0].player.playerWinObj.Length; j++)
                             {
                                 winData[i].player.playerWinObj[j].SetActive(true);
                             }
                         }
                         else
                         {
                             for (int j = 0; j < winData[0].player.playerWinObj.Length; j++)
                             {
                                 winData[i].player.playerWinObj[j].SetActive(true);
                             }
                         }
                     }
                     StartCoroutine(RestartGamePlay());

                 }
                 else if (highestRuleNo == 5)
                 {
                     int firstCard1 = 0;
                     int firstCard2 = 0;
                    
                     int secondCard1 = 0;
                     int secondCard2 = 0;
                    

                     int mainCardNum = sortList1[0].winList[0].cardNo;
                     foreach (var t in sortList1[0].winList)
                     {
                         t.cardNo = t.cardNo == mainCardNum ? firstCard1 : firstCard2;
                     }
                     int secondMainCardNum = sortList1[1].winList[0].cardNo;
                     foreach (var t in sortList1[1].winList)
                     {
                         t.cardNo = t.cardNo == secondMainCardNum ? secondCard1 : secondCard2;
                     }

                     if (firstCard1 == secondCard1)
                     {
                         if (firstCard2 > secondCard2)
                         {
                             for (int j = 0; j < winData[0].player.playerWinObj.Length; j++)
                             {
                                 sortList1[0].player.playerWinObj[j].SetActive(true);
                             }
                         }
                         else
                         {
                             for (int j = 0; j < winData[0].player.playerWinObj.Length; j++)
                             {
                                 sortList1[1].player.playerWinObj[j].SetActive(true);
                             }
                         }
                     }

                     if (firstCard1 > secondCard1)
                     {
                         for (int j = 0; j < winData[0].player.playerWinObj.Length; j++)
                         {
                             sortList1[0].player.playerWinObj[j].SetActive(true);
                         }
                     }
                     else
                     {
                         for (int j = 0; j < winData[0].player.playerWinObj.Length; j++)
                         {
                             sortList1[1].player.playerWinObj[j].SetActive(true);
                         }
                     }
                     StartCoroutine(RestartGamePlay());

                 }
                 else if (highestRuleNo == 6)
                 {
                     int mainNo = winData[0].winList[0].cardNo;
                     for (int i = 0; i <sortList1.Count;i++)
                     {
                         if (sortList1[i].winList[0].cardNo > mainNo)
                         {
                             for (int j = 0; j < winData[0].player.playerWinObj.Length; j++)
                             {
                                 winData[i].player.playerWinObj[j].SetActive(true);
                             }
                         }
                         else
                         {
                             for (int j = 0; j < winData[0].player.playerWinObj.Length; j++)
                             {
                                 winData[i].player.playerWinObj[j].SetActive(true);
                             }
                         }
                     }
                     StartCoroutine(RestartGamePlay());

                 }
                 else if (highestRuleNo == 7)
                 {
                     int firstCard1 = 0;
                     int firstCard2 = 0;
                    
                     int secondCard1 = 0;
                     int secondCard2 = 0;
                    

                     int mainCardNum = sortList1[0].winList[0].cardNo;
                     foreach (var t in sortList1[0].winList)
                     {
                         t.cardNo = t.cardNo == mainCardNum ? firstCard1 : firstCard2;
                     }
                     int secondMainCardNum = sortList1[1].winList[0].cardNo;
                     foreach (var t in sortList1[1].winList)
                     {
                         t.cardNo = t.cardNo == secondMainCardNum ? secondCard1 : secondCard2;
                     }

                     if (firstCard1 == secondCard1)
                     {
                         if (firstCard2 > secondCard2)
                         {
                             for (int j = 0; j < winData[0].player.playerWinObj.Length; j++)
                             {
                                 sortList1[0].player.playerWinObj[j].SetActive(true);
                             }
                         }
                         else
                         {
                             for (int j = 0; j < winData[0].player.playerWinObj.Length; j++)
                             {
                                 sortList1[1].player.playerWinObj[j].SetActive(true);
                             }
                         }
                     }

                     if (firstCard1 > secondCard1)
                     {
                         for (int j = 0; j < winData[0].player.playerWinObj.Length; j++)
                         {
                             sortList1[0].player.playerWinObj[j].SetActive(true);
                         }
                     }
                     else
                     {
                         for (int j = 0; j < winData[0].player.playerWinObj.Length; j++)
                         {
                             sortList1[1].player.playerWinObj[j].SetActive(true);
                         }
                     }
                     StartCoroutine(RestartGamePlay());

                 }
                 else if (highestRuleNo == 8)
                 {
                     int mainNo = winData[0].winList[0].cardNo;
                     for (int i = 0; i <sortList1.Count;i++)
                     {
                         if (sortList1[i].winList[0].cardNo > mainNo)
                         {
                             for (int j = 0; j < winData[0].player.playerWinObj.Length; j++)
                             {
                                 winData[i].player.playerWinObj[j].SetActive(true);
                             }
                         }
                         else
                         {
                             for (int j = 0; j < winData[0].player.playerWinObj.Length; j++)
                             {
                                 winData[i].player.playerWinObj[j].SetActive(true);
                             }
                         }
                     }
                     StartCoroutine(RestartGamePlay());

                 }
                 else if (highestRuleNo == 9)
                 {
                     int firstCard1 = 0;
                     int firstCard2 = 0;
                    
                     int secondCard1 = 0;
                     int secondCard2 = 0;
                    

                     int mainCardNum = sortList1[0].winList[0].cardNo;
                     foreach (var t in sortList1[0].winList)
                     {
                         t.cardNo = t.cardNo == mainCardNum ? firstCard1 : firstCard2;
                     }
                     int secondMainCardNum = sortList1[1].winList[0].cardNo;
                     foreach (var t in sortList1[1].winList)
                     {
                         t.cardNo = t.cardNo == secondMainCardNum ? secondCard1 : secondCard2;
                     }

                     if (firstCard1 == secondCard1)
                     {
                         if (firstCard2 > secondCard2)
                         {
                             for (int j = 0; j < winData[0].player.playerWinObj.Length; j++)
                             {
                                 sortList1[0].player.playerWinObj[j].SetActive(true);
                             }
                         }
                         else
                         {
                             for (int j = 0; j < winData[0].player.playerWinObj.Length; j++)
                             {
                                 sortList1[1].player.playerWinObj[j].SetActive(true);
                             }
                         }
                     }

                     if (firstCard1 > secondCard1)
                     {
                         for (int j = 0; j < winData[0].player.playerWinObj.Length; j++)
                         {
                             sortList1[0].player.playerWinObj[j].SetActive(true);
                         }
                     }
                     else
                     {
                         for (int j = 0; j < winData[0].player.playerWinObj.Length; j++)
                         {
                             sortList1[1].player.playerWinObj[j].SetActive(true);
                         }
                     }
                     StartCoroutine(RestartGamePlay());

                 }
            }
            
        }*/
        
        /*List<PokerWinDataWithPlayer> winData = new List<PokerWinDataWithPlayer>();
        
        // Sort the win data list by rule number
        winData.Sort((a, b) => a.ruleNo.CompareTo(b.ruleNo));

        // Get the winning player number
        int winningPlayerNo = winData[0].player.playerNo;

        // Set the win flag for the winning player
        if (winningPlayerNo == playerNo)
        {
            SetPokerWon(winValue);
        }

        // Show the winning objects for the winning player
        for (int i = 0; i < winData[0].player.playerWinObj.Length; i++)
        {
            winData[0].player.playerWinObj[i].SetActive(true);
        }

         // Restart the gameplay coroutine
        StartCoroutine(RestartGamePlay());*/
        
        // Get win data for active and non-folded players
        /*List<PokerWinDataWithPlayer> winData = pokerPlayers
            .Where(p => !p.isFold && p.gameObject.activeSelf)
            .Select(p => new PokerWinDataWithPlayer
            {
                ruleNo = p.CardDisplay().ruleNo,
                winList = p.CardDisplay().winList,
                player = p
            })
            .Where(data => data.winList.Count < 10)
            .ToList();
    
        // Get the winner with the highest rule number
        PokerWinDataWithPlayer winner = winData
            .OrderBy(data => data.ruleNo)
            .FirstOrDefault();

        if (winner != null)
        {
            if (winner.player.playerNo == playerNo)
            {
                SetPokerWon(winner.player.playerNo.ToString());
            }
        
            foreach (GameObject obj in winner.player.playerWinObj)
            {
                obj.SetActive(true);
            }
        
            StartCoroutine(RestartGamePlay());
        }*/
        
        List<PokerWinDataWithPlayer> winData = new List<PokerWinDataWithPlayer>();
        PokerWinDataWithPlayer highestRuleData = null;
        int highestRuleNo = 0;

        // Iterate over each player and add to winData if they meet the criteria
        foreach (PokerPlayer player in playerSquList)
        {
            if (!player.isFold && player.gameObject.activeSelf)
            {
                PokerWinDataMaintain data = player.CardDisplay();
                if (data.winList.Count > 0 && data.winList.Count < 9)
                {
                    PokerWinDataWithPlayer passData = new PokerWinDataWithPlayer();
                    passData.ruleNo = data.ruleNo;
                    passData.winList = data.winList;
                    passData.player = player;
                    winData.Add(passData);

                    if (data.ruleNo > highestRuleNo)
                    {
                        highestRuleNo = data.ruleNo;
                        highestRuleData = passData;
                    }
                }
            }
        }

        // Get the winner with the highest rule number
        /*if (highestRuleData != null)
        {
            if (highestRuleData.player.playerNo == playerNo)
            {
                SetPokerWon(highestRuleData.player.playerNo.ToString());
            }
            foreach (GameObject obj in highestRuleData.player.playerWinObj)
            {
                obj.SetActive(true);
            }
            StartCoroutine(RestartGamePlay());
        }*/
        
        if (highestRuleData != null && isAdmin)
        {
            List<PokerWinDataWithPlayer> winners = new List<PokerWinDataWithPlayer>();
            foreach (PokerWinDataWithPlayer data in winData)
            {
                if (data.ruleNo == highestRuleNo)
                {
                    winners.Add(data);
                }
            }

            // Choose a random winner if there are multiple winners
            PokerWinDataWithPlayer winningData;
            if (winners.Count > 1)
            {
                
                int index = UnityEngine.Random.Range(0, winners.Count);
                winningData = winners[index];
            }
            else
            {
                winningData = winners[0];
            }

            // Set the winning animation for the winner
            //CallFinalWinner(winningData);
            SetPokerWonData(winningData.player.playerId);
        }
        
        StartCoroutine(DestroyCards());
    }

    public void CallFinalWinner(string winnerPlayerId)
    {
        var winningData = playerSquList.Find(player => player.playerId == winnerPlayerId);
        if (winningData != null)
        {
            if (winningData.playerNo == playerNo)
            {
                SetPokerWon(winningData.playerNo.ToString());
            }

            foreach (GameObject obj in winningData.playerWinObj)
            {
                obj.SetActive(true);
            }
            SoundManager.Instance.CasinoWinSound();
        }

        StartCoroutine(RestartGamePlay());
        
    }

    private IEnumerator DestroyCards()
    {
        yield return new WaitForSeconds(6f);
        Destroy(card1Pos.transform.GetChild(0).gameObject);
        Destroy(card2Pos.transform.GetChild(0).gameObject);
        Destroy(card3Pos.transform.GetChild(0).gameObject);
        Destroy(card4Pos.transform.GetChild(0).gameObject);
        Destroy(card5Pos.transform.GetChild(0).gameObject);
    }


    #region Second Panel

    public void OpenSecondPanel()
    {
        secondScreenObj.SetActive(true);
    }
    
    public void OpenOnScreen()
    {
        downObjectOnObj.SetActive(true);
        downObjectOff.SetActive(false);

        callPriceTxt.text = lastPrice.ToString();
        raisePriceTxt.text = lastPrice.ToString();
        raisePrice = lastPrice;

        if (isFold_Off)
        {
            SendPokerPlayerFold(player1.playerId);

        }
        else if (isAllIn)
        {
            callBtn.SetActive(false);
            raiseBtn.SetActive(false);
            allInBtn.SetActive(true);
        }
        else if (isCheck_Off)
        {

        }
        else if (isCall_Off)
        {
            SendPokerBet(player1.playerNo, lastPrice, "call");
        }
        
    }
    
    public IEnumerator RestartGamePlay()
    {
        isGameStarted = false;
        yield return new WaitForSeconds(6f);

        //print("Enther The Generate Player");
        if (isAdmin)
        {
            StartGamePlay();
            //SetRoomData();
            //TestSocketIO.Instace.SetGameId(DataManager.Instance.tournamentID);
            print("Enther The Generate Player1");
            //isBotActivate = true;

        }
    }

    public void OpenOffScreen()
    {
        downObjectOnObj.SetActive(false);
        secondSubScreenObj.SetActive(false);
        secondUpBtnObj.transform.DORotate(new Vector3(0, 0, 0), 0.1f);
        downObjectOff.SetActive(true);
        for (int i = 0; i < tickObj.Length; i++)
        {
            tickObj[i].SetActive(false);
            blackBtnObj[i].sprite = blackBtnOff;
        }
        isCheck_Off = false;
        isFold_Off = false;
        isCall_Off = false;
    }

    public void OffButtonClick(int no)
    {
        SoundManager.Instance.ButtonClick();
        if (no == 0)
        {
            // fold button
            isFold_Off = true;
            isCheck_Off = false;
            isCall_Off = false;
            
        }
        else if (no == 1)
        {
            // check button
            isFold_Off = false;
            isCheck_Off = true;
            isCall_Off = false;
        }
        else if (no == 2)
        {
            //call button
            isFold_Off = false;
            isCheck_Off = false;
            isCall_Off = true;
        }
        for (int i = 0; i < tickObj.Length; i++)
        {
            if (i == no)
            {
                tickObj[i].SetActive(true);
                blackBtnObj[i].sprite = blackBtnOn;
            }
            else
            {
                tickObj[i].SetActive(false);
                blackBtnObj[i].sprite = blackBtnOff;
            }
        }
    }
    
    

    public void Second_Fold_ButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        SendPokerPlayerFold(player1.playerId);
        ChangePlayerTurn(player1.playerNo);
    }

    public void Second_Call_ButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        //SoundManager.Instance.ThreeBetSound();
        if (CheckMoney(lastPrice) == false)
        {
            SoundManager.Instance.ButtonClick();
            OpenErrorScreen();
            return;
        }
        SoundManager.Instance.ThreeBetSound();
        BetAnim(player1, lastPrice);
        DataManager.Instance.DebitAmount((lastPrice).ToString(), DataManager.Instance.gameId, "Poker-Bet-" + DataManager.Instance.gameId, "game", 1);
        SendPokerBet(player1.playerNo, lastPrice, "call");
        ChangePlayerTurn(player1.playerNo);
    }

    public void Second_Raise_ButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        SoundManager.Instance.ThreeBetSound();
        if (raisePrice == 100)
        {
            SendPokerBet(player1.playerNo, raisePrice, "allin");

        }
        else
        {
            SendPokerBet(player1.playerNo, raisePrice, "raise");
        }
        if (CheckMoney(raisePrice) == false)
        {
            SoundManager.Instance.ButtonClick();
            OpenErrorScreen();
            return;
        }
        SoundManager.Instance.ThreeBetSound();
        lastPrice = raisePrice;
        BetAnim(player1, raisePrice);
        DataManager.Instance.DebitAmount((raisePrice).ToString(), DataManager.Instance.gameId, "Poker-Bet-" + DataManager.Instance.gameId, "game", 2);
        ChangePlayerTurn(player1.playerNo);
    }

    public void Second_AllIn_ButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        SendPokerBet(player1.playerNo, raisePrice, "allin");
        ChangePlayerTurn(player1.playerNo);

    }

    public void Second_Up_ButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        if (secondUpBtnObj.transform.rotation.z != 0)
        {
            secondSubScreenObj.SetActive(false);
            secondUpBtnObj.transform.DORotate(new Vector3(0, 0, 0), 0.1f);
        }
        else
        {
            secondSubScreenObj.SetActive(true);
            raisePriceTxt.text = raisePrice.ToString();
            secondUpBtnObj.transform.DORotate(new Vector3(0, 0, 180), 0.1f);

        }

    }
    
    public void SoundButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        if (soundOn.activeSelf)
        {
            DataManager.Instance.SetSound(1);
            SoundManager.Instance.StopBackgroundMusic();
            soundOn.SetActive(false);
            soundOff.SetActive(true);
        }
        else
        {
            DataManager.Instance.SetSound(0);
            soundOff.SetActive(false);
            soundOn.SetActive(true);
            SoundManager.Instance.StartBackgroundMusic();
        }
    }
    
    public void VibrationButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        if (vibOn.activeSelf)
        {
            DataManager.Instance.SetVibration(1);
            //SoundManager.Instance.StopBackgroundMusic();
            vibOn.SetActive(false);
            vibOff.SetActive(true);
        }
        else
        {
            DataManager.Instance.SetVibration(0);
            vibOff.SetActive(false);
            vibOn.SetActive(true);
            //SoundManager.Instance.StartBackgroundMusic();
        }
    }
    
    public void SfxButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        if (sfxOn.activeSelf)
        {
            sfxOn.SetActive(false);
            sfxOff.SetActive(true);
        }
        else
        {
            sfxOn.SetActive(true);
            sfxOff.SetActive(false);
        }
    }

    private void ManageSoundButtons()
    {
        if (DataManager.Instance.GetSound() == 0)
        {
            soundOn.SetActive(true);
            soundOff.SetActive(false);
        }
        else
        {
            soundOn.SetActive(false);
            soundOff.SetActive(true);
        }

        if (DataManager.Instance.GetVibration() == 0)
        {
            vibOn.SetActive(true);
            vibOff.SetActive(false);
        }
        else
        {
            vibOn.SetActive(false);
            vibOff.SetActive(true);
        }
    }


    /*public void Second_DropDown_Minus_ButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        if (sliderValue.value > 0)
        {
            if (sliderValue.value > 0 && sliderValue.value <= 0.25)
            {
                raisePrice -= 5f;
                sliderValue.value = 0;
                minusBtn.interactable = false;
                plusBtn.interactable = true;
            }
            else if (sliderValue.value > 0.25 && sliderValue.value <= 0.5)
            {
                raisePrice -= 5f;
                sliderValue.value = 0.25f;

                minusBtn.interactable = true;
                plusBtn.interactable = true;

            }
            else if (sliderValue.value > 0.5 && sliderValue.value <= 0.75)
            {
                raisePrice -= 5f;
                sliderValue.value = 0.5f;


                minusBtn.interactable = true;
                plusBtn.interactable = true;
            }
            else if (sliderValue.value > 0.75 && sliderValue.value <= 1)
            {
                raisePrice -= 5;
                sliderValue.value = 0.75f;


                minusBtn.interactable = true;
                plusBtn.interactable = true;
            }
        }
        raisePriceTxt.text = raisePrice.ToString();
    }
    public void Second_DropDown_Plus_ButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        if (sliderValue.value > 0)
        {
            if (sliderValue.value > 0 && sliderValue.value <= 0.25)
            {
                raisePrice += 5;
                sliderValue.value = 0.25f;
                minusBtn.interactable = true;
                plusBtn.interactable = true;
            }
            else if (sliderValue.value > 0.25 && sliderValue.value <= 0.5)
            {
                raisePrice += 5f;
                sliderValue.value = 0.5f;
                minusBtn.interactable = true;
                plusBtn.interactable = true;
            }
            else if (sliderValue.value > 0.5 && sliderValue.value <= 0.75)
            {
                raisePrice += 5f;
                sliderValue.value = 0.75f;
                minusBtn.interactable = true;
                plusBtn.interactable = true;
            }
            else if (sliderValue.value > 0.75 && sliderValue.value <= 1)
            {
                raisePrice += 5f;
                sliderValue.value = 1f;
                minusBtn.interactable = true;
                plusBtn.interactable = false;
            }
            raisePriceTxt.text = raisePrice.ToString();
        }
    }*/
    
    public void Second_DropDown_Minus_ButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        if (raisePrice > 10)
        {
            raisePrice -= 10;
            raisePriceTxt.text = raisePrice.ToString();
            sliderValue.value = raisePrice / 100f;

            // Enable plus button if value is not at max
            plusBtn.interactable = (raisePrice < 100);

            // Disable minus button if value is at min
            minusBtn.interactable = (raisePrice > 10);
        }
    }

    public void Second_DropDown_Plus_ButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        if (raisePrice < 100)
        {
            raisePrice += 10;
            raisePriceTxt.text = raisePrice.ToString();
            sliderValue.value = raisePrice / 100f;

            // Enable minus button if value is not at min
            minusBtn.interactable = (raisePrice > 10);

            // Disable plus button if value is at max
            plusBtn.interactable = (raisePrice < 100);
        }
    }

    public void Second_DropDown_Menu_ButtonClick(int no)
    {
        SoundManager.Instance.ButtonClick();
        if (no == 1)
        {
            //max
            raisePrice = 100;

        }
        else if (no == 2)
        {
            //pot
            raisePrice = 50;
        }
        else if (no == 3)
        {
            //3/4
            raisePrice = lastPrice * 4;

        }
        else if (no == 4)
        {
            //1/2
            raisePrice = lastPrice * 2;
        }
        else if (no == 5)
        {
            //Min
            raisePrice = lastPrice;
        }
        sliderValue.value = raisePrice / 100f;
        raisePriceTxt.text = raisePrice.ToString();
    }
    
    public void OnSliderValueChanged()
    {
        raisePrice = Mathf.RoundToInt(sliderValue.value * 100f);
        raisePriceTxt.text = raisePrice.ToString();
        
        minusBtn.interactable = (raisePrice > 10);
        
        plusBtn.interactable = (raisePrice < 100);
    }

    #endregion

    #region Game Play Manager
    
    public void DisplayCurrentBalance()
    {
        player1.playerBalanceTxt.text = DataManager.Instance.playerData.balance.ToString();
    }

    public void UpdateBetAmount()
    {
        player1BetAmount = player1.betAmount;
        player2BetAmount = player2.betAmount;
        player3BetAmount = player3.betAmount;
        player4BetAmount = player4.betAmount;
        player5BetAmount = player5.betAmount;
        
        //CheckBetAmount();
    }

    public void CheckBetAmount()
    {
    /*bool equal = true;
    float betAmount = playerSquList[0].betAmount;
    for (int i = 1; i < playerSquList.Count; i++)
    {
        if (!Mathf.Approximately(playerSquList[i].betAmount ,betAmount))
        {
            equal = false;
            break;
        }
    }
    // if (player1BetAmount != player2BetAmount || player2BetAmount != player3BetAmount ||
    //     player3BetAmount != player4BetAmount || player4BetAmount != player5BetAmount) return;
    if (!equal) return;
    if (!(player1BetAmount > 0)) return;

    switch (_allBetEqual)
    {
        case false:
            StartCoroutine(FlopCardShow());
            _allBetEqual = true;
            Player1BetIn();
            Player2BetIn();
            Player3BetIn();
            Player4BetIn();
            Player5BetIn();
            ResetBetAmount();
            return;
        case true when !_isFlopShowDone:
            StartCoroutine(TurnCardShow());
            _isFlopShowDone = true;
            Player1BetIn();
            Player2BetIn();
            Player3BetIn();
            Player4BetIn();
            Player5BetIn();
            ResetBetAmount();
            return;
        case true when (_isFlopShowDone && !_isRiverShowDone):
            isGameStop = false;
            StartCoroutine(RiverCardShow());
            _isRiverShowDone = true;
            Player1BetIn();
            Player2BetIn();
            Player3BetIn();
            Player4BetIn();
            Player5BetIn();
            ResetBetAmount();
            WinPoker();
            return;
        default:
            print("All values are equal");
            break;
    }*/
    
      // Check if all non-folded players have bet the same amount
       bool equal = true;
       float betAmount = 0f;
       bool isFirstPlayer = true;
       foreach (var playerSquare in playerSquList.Where(playerSquare => !playerSquare.isFold))
       {
           if (isFirstPlayer)
           {
               betAmount = playerSquare.betAmount;
               isFirstPlayer = false;
           }
           else
           {
               if (!Mathf.Approximately(playerSquare.betAmount, betAmount))
               {
                   equal = false;
                   break;
               }
           }
       }
   
       // If not all non-folded players have bet the same amount, exit the method
       if (!equal) return;
   
       // If no non-folded player has bet anything, exit the method
       if (betAmount <= 0f) return;
   
       // Switch on the game state based on which cards have been shown
       switch (_allBetEqual)
       {
           case false:
               StartCoroutine(FlopCardShow());
               _allBetEqual = true;
               Player1BetIn();
               Player2BetIn();
               Player3BetIn();
               Player4BetIn();
               Player5BetIn();
               ResetBetAmount();
               break;
           case true when !_isFlopShowDone:
               StartCoroutine(TurnCardShow());
               _isFlopShowDone = true;
               Player1BetIn();
               Player2BetIn();
               Player3BetIn();
               Player4BetIn();
               Player5BetIn();
               ResetBetAmount();
               break;
           case true when (_isFlopShowDone && !_isRiverShowDone):
               isGameStop = false;
               StartCoroutine(RiverCardShow());
               _isRiverShowDone = true;
               Player1BetIn();
               Player2BetIn();
               Player3BetIn();
               Player4BetIn();
               Player5BetIn();
               ResetBetAmount();
               WinPoker();
               break;
           default:
               print("All values are equal");
               break;
       }
    }
    

    public void ResetBetAmount()
    {
        foreach (var activePlayers in playerSquList)
        {
            activePlayers.betAmount = 0f;
            activePlayers.betTxt.text = activePlayers.betAmount.ToString();
        }
    }

    public IEnumerator FlopCardShow()
    {
        GameObject obj = Instantiate(commonCard, card1Pos.transform);
        SoundManager.Instance.CasinoCardMoveSound();
        obj.transform.position = startCard.transform.position;
        obj.SetActive(true);
        Vector3 firstPos = card1Pos.transform.position;
        firstPos.x -= 0.2f;
        obj.transform.DOMove(firstPos, 0.4f).OnComplete(() =>
        {
            SoundManager.Instance.CasinoCardSwipeSound();

            obj.transform.DOScale(new Vector3(0, 1, 1), 0.2f).OnComplete(() =>
            {
                obj.transform.GetComponent<Image>().sprite = card1.cardSprite;
                obj.transform.DOScale(new Vector3(1, 1, 1), 0.2f).OnComplete(() =>
                {
                    if (card1.cardNo == cardSufflesGen[0].cardNo)
                    {
                        print("Card1");
                    }
                    else
                    {
                        print("card didnt match");
                    }
                });
            });
            
        });
        yield return new WaitForSeconds(0.4f);
        StartCoroutine(SecondCardShow());
    }
    public IEnumerator SecondCardShow()
    {
        GameObject obj = Instantiate(commonCard, card2Pos.transform);
        SoundManager.Instance.CasinoCardMoveSound();
        obj.transform.position = startCard.transform.position;
        obj.SetActive(true);
        Vector3 secondPos = card2Pos.transform.position;
        secondPos.x -= 0.2f;
        obj.transform.DOMove(secondPos, 0.4f).OnComplete(() =>
        {
            SoundManager.Instance.CasinoCardSwipeSound();

            obj.transform.DOScale(new Vector3(0, 1, 1), 0.2f).OnComplete(() =>
            {
                obj.transform.GetComponent<Image>().sprite = card2.cardSprite;
                obj.transform.DOScale(new Vector3(1, 1, 1), 0.2f).OnComplete(() =>
                {
                    if (card2.cardNo == cardSufflesGen[0].cardNo)
                    {
                        print("Card2");
                    }
                    else
                    {
                        print("card didnt match");
                    }
                });
            });
            
        });
        yield return new WaitForSeconds(0.4f);
        StartCoroutine(ThirdCardShow());
    }
    public IEnumerator ThirdCardShow()
    {
        GameObject obj = Instantiate(commonCard, card3Pos.transform);
        SoundManager.Instance.CasinoCardMoveSound();
        obj.transform.position = startCard.transform.position;
        obj.SetActive(true);
        Vector3 thirdPos = card3Pos.transform.position;
        thirdPos.x -= 0.2f;
        obj.transform.DOMove(thirdPos, 0.4f).OnComplete(() =>
        {
            SoundManager.Instance.CasinoCardSwipeSound();

            obj.transform.DOScale(new Vector3(0, 1, 1), 0.2f).OnComplete(() =>
            {
                obj.transform.GetComponent<Image>().sprite = card3.cardSprite;
                obj.transform.DOScale(new Vector3(1, 1, 1), 0.2f).OnComplete(() =>
                {
                    if (card3.cardNo == cardSufflesGen[0].cardNo)
                    {
                        print("Card3");
                    }
                    else
                    {
                        print("card didnt match");
                    }
                });
            });
            
        });
        yield return new WaitForSeconds(2f);
    }
    public IEnumerator TurnCardShow()
    {
        GameObject obj = Instantiate(commonCard, card4Pos.transform);
        SoundManager.Instance.CasinoCardMoveSound();
        obj.transform.position = startCard.transform.position;
        obj.SetActive(true);
        Vector3 fourthPos = card4Pos.transform.position;
        fourthPos.x -= 0.2f;
        obj.transform.DOMove(fourthPos, 0.4f).OnComplete(() =>
        {
            SoundManager.Instance.CasinoCardSwipeSound();

            obj.transform.DOScale(new Vector3(0, 1, 1), 0.2f).OnComplete(() =>
            {
                obj.transform.GetComponent<Image>().sprite = card4.cardSprite;
                obj.transform.DOScale(new Vector3(1, 1, 1), 0.2f).OnComplete(() =>
                {
                    if (card4.cardNo == cardSufflesGen[0].cardNo)
                    {
                        print("Card4");
                    }
                    else
                    {
                        print("card didnt match");
                    }
                });
            });
            
        });
        yield return new WaitForSeconds(2f);
    }
    public IEnumerator RiverCardShow()
    {
        GameObject obj = Instantiate(commonCard, card5Pos.transform);
        SoundManager.Instance.CasinoCardMoveSound();
        obj.transform.position = startCard.transform.position;
        obj.SetActive(true);
        Vector3 fifthPos = card5Pos.transform.position;
        fifthPos.x -= 0.2f;
        obj.transform.DOMove(fifthPos, 0.4f).OnComplete(() =>
        {
            SoundManager.Instance.CasinoCardSwipeSound();

            obj.transform.DOScale(new Vector3(0, 1, 1), 0.2f).OnComplete(() =>
            {
                obj.transform.GetComponent<Image>().sprite = card5.cardSprite;
                obj.transform.DOScale(new Vector3(1, 1, 1), 0.2f).OnComplete(() =>
                {
                    if (card5.cardNo == cardSufflesGen[0].cardNo)
                    {
                        print("Card5");
                    }
                    else
                    {
                        print("card didnt match");
                    }
                });
            });
            
        });
        yield return new WaitForSeconds(4f);
    }
    public PokerWinDataMaintain MatchResult(CardSuffle cards1, CardSuffle cards2, CardSuffle cards3, CardSuffle cards4, CardSuffle cards5, CardSuffle card6, CardSuffle card7)
    {
        //CardSuffle suffles1 = new CardSuffle();
        //suffles1.cardNo = 10;
        //suffles1.color = CardColorType.Clubs;
        //CardSuffle suffles2 = new CardSuffle();
        //suffles2.cardNo = 13;
        //suffles2.color = CardColorType.Diamonds;
        //CardSuffle suffles3 = new CardSuffle();
        //suffles3.cardNo = 10;
        //suffles3.color = CardColorType.Diamonds;
        //CardSuffle suffles4 = new CardSuffle();
        //suffles4.cardNo = 13;
        //suffles4.color = CardColorType.Clubs;
        //CardSuffle suffles5 = new CardSuffle();
        //suffles5.cardNo = 10;
        //suffles5.color = CardColorType.Hearts;


        //CardSuffle suffles6 = new CardSuffle();
        //suffles6.cardNo = 5;
        //suffles6.color = CardColorType.Clubs;

        //CardSuffle suffles7 = new CardSuffle();
        //suffles7.cardNo = 5;
        //suffles7.color = CardColorType.Spades;

        //c

        PokerWinDataMaintain pokerWinData = new PokerWinDataMaintain();
        List<CardSuffle> newData = new List<CardSuffle>();
        newData.Add(cards1);
        newData.Add(cards2);
        newData.Add(cards3);
        newData.Add(cards4);
        newData.Add(cards5);
        newData.Add(card6);
        newData.Add(card7);

        newCardSS = newData;

        newCardSS1 = NewSort(newData);
        bool isColor = IsColorMatch(newCardSS1);
        
        List<CardSuffle> getRonList = ConvertCardNo(RonValue(newCardSS1));
        List<CardSuffle> getRonColorList = ConvertCardNo(RonColorValue(newCardSS1, GetFindColor(newCardSS1)));

        List<CardSuffle> sameCardList = SameCardGet(newCardSS1);

        List<CardSuffle> getFourCards = GetFourCard(sameCardList, newCardSS1);
        List<CardSuffle> getThreeTwoCards = GetThreeTwoCard(sameCardList, newCardSS1);
        List<CardSuffle> getTwoTwoCards = GetTwoTwoCard(sameCardList, newCardSS1);
        List<CardSuffle> getThreeCards = GetThreeCard(sameCardList, newCardSS1);
        List<CardSuffle> getTwoCards = GetTwoCard(sameCardList, newCardSS1);
        List<CardSuffle> getHighCard = GetHighCard(newCardSS1);

        List<CardSuffle> colorCardList = new List<CardSuffle>();

        if (isColor)
        {
            colorCardList = GetHighColorCard(newCardSS1, GetFindColor(newCardSS1));
        }


        //print("getRonColorList : " + getRonColorList.Count);

        cardResult = getThreeTwoCards;
        if (getRonColorList.Count == 5)
        {
            print("Enjoy");//Rules1

            pokerWinData.ruleNo = 1;
            pokerWinData.winList = getRonColorList;

        }
        else if (getFourCards.Count == 5)
        {
            //Rule2

            pokerWinData.ruleNo = 2;
            pokerWinData.winList = getFourCards;
        }
        else if (getThreeTwoCards.Count == 5)
        {
            //Rule3
            pokerWinData.ruleNo = 3;
            pokerWinData.winList = getThreeTwoCards;
        }
        else if (colorCardList.Count == 5)
        {
            //Rule4
            pokerWinData.ruleNo = 4;
            pokerWinData.winList = colorCardList;
        }
        else if (getRonList.Count == 5)
        {
            //Rule5
            pokerWinData.ruleNo = 5;
            pokerWinData.winList = getRonList;
        }
        else if (getThreeCards.Count == 5)
        {
            //Rule6
            pokerWinData.ruleNo = 6;
            pokerWinData.winList = getThreeCards;
        }
        else if (getTwoTwoCards.Count == 5)
        {
            //Rule 7
            pokerWinData.ruleNo = 7;
            pokerWinData.winList = getTwoTwoCards;
        }
        else if (getTwoCards.Count == 5)
        {
            //Rule 8
            pokerWinData.ruleNo = 8;
            pokerWinData.winList = getTwoCards;
        }
        else if (getHighCard.Count == 5)
        {
            // Rule 9
            pokerWinData.ruleNo = 9;
            pokerWinData.winList = getHighCard;
        }
        return pokerWinData;
    }


    List<CardSuffle> ConvertCardNo(List<CardSuffle> cards)
    {
        for (int i = 0; i < cards.Count; i++)
        {
            if (cards[i].cardNo == 1)
            {
                cards[i].cardNo = 14;
            }
            else if (cards[i].cardNo == 13)
            {
                cards[i].cardNo = 11;
            }
            else if (cards[i].cardNo == 11)
            {
                cards[i].cardNo = 13;
            }
        }
        return cards;
    }


    bool IsColorMatch(List<CardSuffle> cards)
    {
        int cnt1 = 0;
        int cnt2 = 0;
        int cnt3 = 0;
        int cnt4 = 0;
        for (int i = 0; i < cards.Count; i++)
        {
            if (cards[i].color == CardColorType.Clubs)
            {
                cnt1++;
            }
            else if (cards[i].color == CardColorType.Diamonds)
            {
                cnt2++;
            }
            else if (cards[i].color == CardColorType.Spades)
            {
                cnt3++;
            }
            else if (cards[i].color == CardColorType.Hearts)
            {
                cnt4++;
            }
        }

        if (cnt1 >= 5 || cnt2 >= 5 || cnt3 >= 5 || cnt4 >= 5)
        {
            return true;
        }

        return false;
    }

    CardColorType GetFindColor(List<CardSuffle> cards)
    {
        int cnt1 = 0;
        int cnt2 = 0;
        int cnt3 = 0;
        int cnt4 = 0;
        for (int i = 0; i < cards.Count; i++)
        {
            if (cards[i].color == CardColorType.Clubs)
            {
                cnt1++;
            }
            else if (cards[i].color == CardColorType.Diamonds)
            {
                cnt2++;
            }
            else if (cards[i].color == CardColorType.Spades)
            {
                cnt3++;
            }
            else if (cards[i].color == CardColorType.Hearts)
            {
                cnt4++;
            }
        }

        if (cnt1 >= 5)
        {
            return CardColorType.Clubs;
        }
        else if (cnt2 >= 5)
        {
            return CardColorType.Diamonds;
        }
        else if (cnt3 >= 5)
        {
            return CardColorType.Spades;
        }
        else if (cnt4 >= 5)
        {
            return CardColorType.Hearts;
        }
        return CardColorType.Clubs;
    }

    List<CardSuffle> GetHighColorCard(List<CardSuffle> cards, CardColorType cardColor)
    {

        List<CardSuffle> highCard = new List<CardSuffle>();
        for (int i = cards.Count - 1; i > 0; i--)
        {
            if (highCard.Count < 5 && cards[i].color == cardColor)
            {
                highCard.Add(cards[i]);
            }
        }
        if (highCard.Count != 5)
        {
            highCard.Clear();
        }
        return highCard;

    }


    List<CardSuffle> RonColorValue(List<CardSuffle> cards, CardColorType cardColor)
    {
        print("Card Coolor : " + cardColor);
        //x
        List<CardSuffle> cardUnique = new List<CardSuffle>();
        List<CardSuffle> ronList = new List<CardSuffle>();

        for (int i = 0; i < cards.Count; i++)
        {
            int xNo = cards[i].cardNo;
            bool isEnter = false;
            for (int j = 0; j < cardUnique.Count; j++)
            {
                if (xNo == cardUnique[j].cardNo)
                {
                    isEnter = true;
                }
            }
            if (!isEnter && cards[i].color == cardColor)
            {
                cardUnique.Add(cards[i]);
            }
        }
        //for (int i = 0; i < cardUnique.Count; i++)
        //{
        //    print("Unique Card No : " + cardUnique[i].cardNo + "----" + cardUnique[i].color.ToString());
        //}

        print("Card Unique Count : " + cardUnique.Count + " Card Color : " + cardColor);
        //for (int i = 0; i < cardUnique.Count; i++)
        //{
        //    if (cardUnique[i].color != cardColor)
        //    {
        //        print("Card Color : " + cardColor);
        //        cardUnique.Remove(cardUnique[i]);
        //    }
        //}


        if (cardUnique.Count == 5)
        {
            int no = cardUnique[0].cardNo;
            int cnt = 0;
            for (int i = 0; i < cardUnique.Count; i++)
            {
                if (no + i == cardUnique[i].cardNo)
                {
                    cnt++;
                }
            }
            if (cnt == 5)
            {
                for (int i = 0; i < cardUnique.Count; i++)
                {
                    ronList.Add(cardUnique[i]);
                }

            }
        }
        else if (cardUnique.Count == 6)
        {
            int no = cardUnique[0].cardNo;
            int no1 = cardUnique[1].cardNo;
            int cnt = 0;
            int cnt1 = 0;
            for (int i = 0; i < cardUnique.Count - 1; i++)
            {
                if (no + i == cardUnique[i].cardNo)
                {
                    cnt++;
                }
            }
            for (int i = 1; i < cardUnique.Count; i++)
            {
                if (no1 + (i - 1) == cardUnique[i].cardNo)
                {
                    cnt1++;
                }
            }

            print("cnt : " + cnt);
            print("cnt1 : " + cnt1);
            if (cnt1 == 5)
            {
                for (int i = 1; i < cardUnique.Count; i++)
                {
                    if (ronList.Count < 5)
                    {
                        ronList.Add(cardUnique[i]);
                    }
                }
            }
            else if (cnt == 5)
            {
                for (int i = 0; i < cardUnique.Count; i++)
                {
                    if (ronList.Count < 5)
                    {
                        ronList.Add(cardUnique[i]);
                    }
                }
            }
        }
        else if (cardUnique.Count == 7)
        {
            int no = cardUnique[0].cardNo;
            int no1 = cardUnique[1].cardNo;
            int no2 = cardUnique[2].cardNo;
            int cnt = 0;
            int cnt1 = 0;
            int cnt2 = 0;
            for (int i = 0; i < cardUnique.Count - 2; i++)
            {
                if (no + i == cardUnique[i].cardNo)
                {
                    cnt++;
                }
            }
            for (int i = 1; i < cardUnique.Count - 1; i++)
            {
                if (no1 + (i - 1) == cardUnique[i].cardNo)
                {
                    cnt1++;
                }
            }
            for (int i = 2; i < cardUnique.Count; i++)
            {
                if (no2 + (i - 2) == cardUnique[i].cardNo)
                {
                    cnt2++;
                }
            }
            if (cnt2 == 5)
            {
                for (int i = 2; i < cardUnique.Count; i++)
                {
                    if (ronList.Count < 5)
                    {
                        ronList.Add(cardUnique[i]);
                    }
                }
            }
            else if (cnt1 == 5)
            {
                for (int i = 1; i < cardUnique.Count; i++)
                {
                    if (ronList.Count < 5)
                    {
                        ronList.Add(cardUnique[i]);
                    }
                }
            }
            else if (cnt == 5)
            {
                for (int i = 0; i < cardUnique.Count; i++)
                {
                    if (ronList.Count < 5)
                    {
                        ronList.Add(cardUnique[i]);
                    }
                }
            }
        }





        List<int> ronCustomList = new List<int>();
        ronCustomList.Add(2);
        ronCustomList.Add(3);
        ronCustomList.Add(4);
        ronCustomList.Add(5);
        ronCustomList.Add(14);
        ronCustomList.Add(1);
        int cntLast = 0;
        if (cardUnique.Count >= 5)
        {

            for (int i = 0; i < cardUnique.Count; i++)
            {
                if (ronCustomList.Contains(cardUnique[i].cardNo))
                {
                    cntLast++;
                }
            }
        }

        print("ron Liost : " + ronList.Count);
        print("cntLast 111: " + cntLast);

        if (cntLast == 5)
        {
            if (ronList.Count != 0)
            {
                if (ronList[0].cardNo == 10)
                {
                    return ronList;
                }
            }

            ronList.Clear();
            ronList.Add(cardUnique[0]);
            ronList.Add(cardUnique[1]);
            ronList.Add(cardUnique[2]);
            ronList.Add(cardUnique[3]);
            for (int i = 4; i < cardUnique.Count; i++)
            {
                if ((cardUnique[i].cardNo == 14 || cardUnique[i].cardNo == 1) && ronList.Count < 5)
                {
                    ronList.Add(cardUnique[i]);
                }
            }
        }

        return ronList;
    }

    List<CardSuffle> RonValue(List<CardSuffle> cards)
    {


        List<CardSuffle> cardUnique = new List<CardSuffle>();
        List<CardSuffle> ronList = new List<CardSuffle>();

        for (int i = 0; i < cards.Count; i++)
        {
            int xNo = cards[i].cardNo;
            bool isEnter = false;
            for (int j = 0; j < cardUnique.Count; j++)
            {
                if (xNo == cardUnique[j].cardNo)
                {
                    isEnter = true;
                }
            }
            if (!isEnter)
            {
                cardUnique.Add(cards[i]);
            }
        }
        if (cardUnique.Count == 5)
        {
            int no = cardUnique[0].cardNo;
            int cnt = 0;
            for (int i = 0; i < cardUnique.Count; i++)
            {
                if (no + i == cardUnique[i].cardNo)
                {
                    cnt++;
                }
            }
            if (cnt == 5)
            {
                for (int i = 0; i < cardUnique.Count; i++)
                {
                    ronList.Add(cardUnique[i]);
                }

            }
        }
        else if (cardUnique.Count == 6)
        {
            int no = cardUnique[0].cardNo;
            int no1 = cardUnique[1].cardNo;
            int cnt = 0;
            int cnt1 = 0;
            for (int i = 0; i < cardUnique.Count - 1; i++)
            {
                if (no + i == cardUnique[i].cardNo)
                {
                    cnt++;
                }
            }
            for (int i = 1; i < cardUnique.Count; i++)
            {
                if (no1 + (i - 1) == cardUnique[i].cardNo)
                {
                    cnt1++;
                }
            }
            if (cnt1 == 5)
            {
                for (int i = 0; i < cardUnique.Count; i++)
                {
                    ronList.Add(cardUnique[i]);
                }
            }
            else if (cnt == 5)
            {
                for (int i = 0; i < cardUnique.Count; i++)
                {
                    ronList.Add(cardUnique[i]);
                }
            }
        }
        else if (cardUnique.Count == 7)
        {
            int no = cardUnique[0].cardNo;
            int no1 = cardUnique[1].cardNo;
            int no2 = cardUnique[2].cardNo;
            int cnt = 0;
            int cnt1 = 0;
            int cnt2 = 0;
            for (int i = 0; i < cardUnique.Count - 2; i++)
            {
                if (no + i == cardUnique[i].cardNo)
                {
                    cnt++;
                }
            }
            for (int i = 1; i < cardUnique.Count - 1; i++)
            {
                if (no1 + (i - 1) == cardUnique[i].cardNo)
                {
                    cnt1++;
                }
            }
            for (int i = 2; i < cardUnique.Count; i++)
            {
                if (no2 + (i - 2) == cardUnique[i].cardNo)
                {
                    cnt2++;
                }
            }
            if (cnt2 == 5)
            {
                for (int i = 0; i < cardUnique.Count; i++)
                {
                    ronList.Add(cardUnique[i]);
                }
            }
            else if (cnt1 == 5)
            {
                for (int i = 0; i < cardUnique.Count; i++)
                {
                    ronList.Add(cardUnique[i]);
                }
            }
            else if (cnt == 5)
            {
                for (int i = 0; i < cardUnique.Count; i++)
                {
                    ronList.Add(cardUnique[i]);
                }
            }
        }


        List<int> ronCustomList = new List<int>();
        ronCustomList.Add(2);
        ronCustomList.Add(3);
        ronCustomList.Add(4);
        ronCustomList.Add(5);
        ronCustomList.Add(14);
        int cntLast = 0;
        if (cardUnique.Count >= 5)
        {
            for (int i = 0; i < cardUnique.Count; i++)
            {
                if (ronCustomList.Contains(cardUnique[i].cardNo))
                {
                    cntLast++;
                }
            }
        }



        if (cntLast == 5)
        {
            if (ronList.Count != 0)
            {
                if (ronList[0].cardNo == 10)
                {
                    return ronList;
                }
            }

            ronList.Clear();
            ronList.Add(cardUnique[0]);
            ronList.Add(cardUnique[1]);
            ronList.Add(cardUnique[2]);
            ronList.Add(cardUnique[3]);

            for (int i = 4; i < cardUnique.Count; i++)
            {
                if (cardUnique[i].cardNo == 14)
                {
                    ronList.Add(cardUnique[i]);
                }
            }
        }
        return ronList;
    }

    List<CardSuffle> NewSort(List<CardSuffle> cards)
    {
        List<CardSuffle> newCards = new List<CardSuffle>();
        //newCards = cards;
        for (int i = 0; i < cards.Count; i++)
        {
            for (int j = 0; j < cardSufflesSort.Count; j++)
            {
                if (cardSufflesSort[j].cardNo == cards[i].cardNo && cardSufflesSort[j].color == cards[i].color)
                {
                    CardSuffle c = new CardSuffle();
                    c.cardNo = cardSufflesSort[j].cardNo;
                    c.color = cardSufflesSort[j].color;
                    c.cardSprite = cardSufflesSort[j].cardSprite;
                    //newCards.Add(cardSufflesSort[i]);
                    newCards.Add(c);
                    break;
                }
            }
        }

        for (int i = 0; i < newCards.Count; i++)
        {
            if (newCards[i].cardNo == 1)
            {
                newCards[i].cardNo = 14;
            }
            else if (newCards[i].cardNo == 11)
            {
                newCards[i].cardNo = 13;
            }
            else if (newCards[i].cardNo == 13)
            {
                newCards[i].cardNo = 11;

            }
        }

        return newCards;
    }

    List<CardSuffle> SameCardGet(List<CardSuffle> cards)
    {
        List<CardSuffle> sameCards = new List<CardSuffle>();

        for (int i = 0; i < cards.Count; i++)
        {
            int no = cards[i].cardNo;
            for (int j = 0; j < cards.Count; j++)
            {
                if (cards[j].cardNo == no)
                {
                    sameCards.Add(cards[i]);
                    break;
                }
            }
        }



        return sameCards;
    }

    List<CardSuffle> GetFourCard(List<CardSuffle> cards, List<CardSuffle> sortCard)
    {


        List<CardSuffle> fourListSuffle = new List<CardSuffle>();
        List<int> noGet = new List<int>();
        for (int i = 0; i < cards.Count; i++)
        {
            int firstCardNo = cards[i].cardNo;
            if (!noGet.Contains(firstCardNo))
            {
                int cnt = 0;
                for (int j = 0; j < cards.Count; j++)
                {
                    if (cards[j].cardNo == firstCardNo)
                    {
                        cnt++;
                    }
                }
                if (cnt >= 4)
                {
                    noGet.Add(firstCardNo);
                }
            }
        }
        if (noGet.Count > 0)
        {
            for (int i = 0; i < cards.Count; i++)
            {
                for (int j = 0; j < noGet.Count; j++)
                {
                    if (noGet[j] == cards[i].cardNo && fourListSuffle.Count < 4)
                    {
                        fourListSuffle.Add(cards[i]);
                    }
                }
            }


            for (int i = sortCard.Count - 1; i > 0; i--)
            {
                if (fourListSuffle.Count < 5 && fourListSuffle[0].cardNo != sortCard[i].cardNo)
                {
                    fourListSuffle.Add(sortCard[i]);
                }
            }

        }
        return fourListSuffle;

    }

    List<CardSuffle> GetThreeTwoCard(List<CardSuffle> cards, List<CardSuffle> sortCard)
    {
        List<CardSuffle> fourListSuffle = new List<CardSuffle>();
        List<int> noGet = new List<int>();
        List<int> noGet1 = new List<int>();
        for (int i = cards.Count - 1; i > 0; i--)
        {
            int firstCardNo = cards[i].cardNo;
            if (!noGet.Contains(firstCardNo))
            {
                int cnt = 0;
                for (int j = 0; j < cards.Count; j++)
                {
                    if (cards[j].cardNo == firstCardNo)
                    {
                        cnt++;
                    }
                }
                if (cnt == 3)
                {
                    noGet.Add(firstCardNo);
                }
                else if (cnt == 2)
                {
                    noGet1.Add(firstCardNo);
                }
            }
        }
        if (noGet.Count > 0)
        {

            for (int i = 0; i < cards.Count; i++)
            {
                for (int j = 0; j < noGet.Count; j++)
                {
                    if (noGet[j] == cards[i].cardNo && fourListSuffle.Count < 3)
                    {
                        fourListSuffle.Add(cards[i]);
                    }
                }
            }
        }
        if (noGet1.Count > 0)
        {

            for (int i = 0; i < cards.Count; i++)
            {
                for (int j = 0; j < noGet1.Count; j++)
                {
                    if (noGet1[j] == cards[i].cardNo && fourListSuffle.Count < 5)
                    {
                        fourListSuffle.Add(cards[i]);
                    }
                }
            }
        }
        if (fourListSuffle.Count > 0)
        {
            for (int i = sortCard.Count - 1; i > 0; i--)
            {
                if (fourListSuffle.Count < 5 && fourListSuffle[0].cardNo != sortCard[i].cardNo)
                {
                    fourListSuffle.Add(sortCard[i]);

                }
            }
        }

        return fourListSuffle;
    }

    List<CardSuffle> GetTwoTwoCard(List<CardSuffle> cards, List<CardSuffle> sortCard)
    {
        List<CardSuffle> fourListSuffle = new List<CardSuffle>();
        List<int> noGet = new List<int>();
        for (int i = cards.Count - 1; i > 0; i--)
        {
            int firstCardNo = cards[i].cardNo;
            if (!noGet.Contains(firstCardNo) && noGet.Count < 2)
            {
                int cnt = 0;
                for (int j = 0; j < cards.Count; j++)
                {
                    if (cards[j].cardNo == firstCardNo)
                    {
                        cnt++;
                    }
                }
                if (cnt == 2)
                {
                    noGet.Add(firstCardNo);
                }
            }
        }
        if (noGet.Count == 2)
        {

            for (int i = 0; i < cards.Count; i++)
            {
                for (int j = 0; j < noGet.Count; j++)
                {
                    if (noGet[0] == cards[i].cardNo && fourListSuffle.Count < 2)
                    {
                        fourListSuffle.Add(cards[i]);
                    }
                }
            }
            for (int i = 0; i < cards.Count; i++)
            {
                for (int j = 0; j < noGet.Count; j++)
                {
                    if (noGet[1] == cards[i].cardNo && fourListSuffle.Count < 4)
                    {
                        fourListSuffle.Add(cards[i]);
                    }
                }
            }

        }

        if (fourListSuffle.Count > 0)
        {
            for (int i = sortCard.Count - 1; i > 0; i--)
            {
                if (fourListSuffle.Count < 5 && noGet[0] != sortCard[i].cardNo && noGet[1] != sortCard[i].cardNo)
                {
                    fourListSuffle.Add(sortCard[i]);

                }
            }
        }

        return fourListSuffle;
    }

    List<CardSuffle> GetThreeCard(List<CardSuffle> cards, List<CardSuffle> sortCard)
    {


        List<CardSuffle> fourListSuffle = new List<CardSuffle>();
        List<int> noGet = new List<int>();
        for (int i = cards.Count - 1; i > 0; i--)
        {
            int firstCardNo = cards[i].cardNo;
            if (!noGet.Contains(firstCardNo))
            {
                int cnt = 0;
                for (int j = 0; j < cards.Count; j++)
                {
                    if (cards[j].cardNo == firstCardNo)
                    {
                        cnt++;
                    }
                }
                if (cnt >= 3)
                {
                    noGet.Add(firstCardNo);
                }
            }
        }
        if (noGet.Count > 0)
        {
            for (int i = 0; i < cards.Count; i++)
            {
                for (int j = 0; j < noGet.Count; j++)
                {
                    if (noGet[j] == cards[i].cardNo && fourListSuffle.Count < 3)
                    {
                        fourListSuffle.Add(cards[i]);
                    }
                }
            }


            for (int i = sortCard.Count - 1; i > 0; i--)
            {
                if (fourListSuffle.Count < 5 && fourListSuffle[0].cardNo != sortCard[i].cardNo)
                {
                    fourListSuffle.Add(sortCard[i]);
                }
            }

        }

        return fourListSuffle;

    }

    List<CardSuffle> GetTwoCard(List<CardSuffle> cards, List<CardSuffle> sortCard)
    {


        List<CardSuffle> fourListSuffle = new List<CardSuffle>();
        List<int> noGet = new List<int>();
        for (int i = cards.Count - 1; i > 0; i--)
        {
            int firstCardNo = cards[i].cardNo;
            if (!noGet.Contains(firstCardNo))
            {
                int cnt = 0;
                for (int j = 0; j < cards.Count; j++)
                {
                    if (cards[j].cardNo == firstCardNo)
                    {
                        cnt++;
                    }
                }
                if (cnt >= 2)
                {
                    noGet.Add(firstCardNo);
                }
            }
        }
        if (noGet.Count > 0)
        {
            for (int i = 0; i < cards.Count; i++)
            {
                for (int j = 0; j < noGet.Count; j++)
                {
                    if (noGet[j] == cards[i].cardNo && fourListSuffle.Count < 2)
                    {
                        fourListSuffle.Add(cards[i]);
                    }
                }
            }


            for (int i = sortCard.Count - 1; i > 0; i--)
            {
                if (fourListSuffle.Count < 5 && fourListSuffle[0].cardNo != sortCard[i].cardNo)
                {
                    fourListSuffle.Add(sortCard[i]);
                }
            }

        }
        return fourListSuffle;

    }

    List<CardSuffle> GetHighCard(List<CardSuffle> cards)
    {

        List<CardSuffle> highCard = new List<CardSuffle>();
        for (int i = cards.Count - 1; i > 0; i--)
        {
            if (highCard.Count < 5)
            {
                highCard.Add(cards[i]);
            }
        }
        return highCard;

    }
    #endregion


    #region Other Button

    public void GiftButtonClick(PokerPlayer giftPlayer)
    {
        print("gift Button Click");
        giftScreenObj.SetActive(true);
        GiftSendManager.Instance.gameName = "Poker";
        GiftSendManager.Instance.pokerOtherPlayer = giftPlayer;
    }

    public void MessageButtonClick()
    {
        messageScreeObj.SetActive(true);
    }

    public void MenuButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        OpenMenuScreen();
    }
    
    public void MenuCloseButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        CloseMenuScreen();
    }
    
    public void SettingsButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        OpenSettingsScreen();
    }
    
    public void SettingsCloseButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        CloseSettingsScreen();
    }



    #endregion

    #region Menu Screen

    void OpenMenuScreen()
    {
        menuScreenObj.SetActive(true);
    }
    
    void CloseMenuScreen()
    {
        menuScreenObj.SetActive(false);
    }
    
    void OpenSettingsScreen()
    {
        settingsScreenObj.SetActive(true);
    }
    
    void CloseSettingsScreen()
    {
        settingsScreenObj.SetActive(false);
    }

    public void CloseMenuScreenButton()
    {
        SoundManager.Instance.ButtonClick();
        menuScreenObj.SetActive(false);
    }

    public void MenuSubButtonClick(int no)
    {
        SoundManager.Instance.ButtonClick();
        if (no == 1)
        {
            TestSocketIO.Instace.LeaveRoom();
            SoundManager.Instance.StartBackgroundMusic();
            SceneManager.LoadScene("Main");
        }
        else if (no == 2)
        {
            OpenRuleScreen();
        }
        else if (no == 3)
        {
            //Shop
            Instantiate(shopPrefab, shopPrefabParent.transform);
        }
    }

    #endregion
    
    #region Error Screen
    public void OpenErrorScreen()
    {
        errorScreenObj.SetActive(true);
    }

    public void Error_Ok_ButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        errorScreenObj.SetActive(false);
    }

    public void Error_Shop_ButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        Instantiate(shopPrefab, shopPrefabParent.transform);
        errorScreenObj.SetActive(false);
    }
    
    public bool CheckMoney(float money)
    {

        float currentBalance = float.Parse(DataManager.Instance.playerData.balance);
        if ((currentBalance - money) < 0)
        {
            return false;
        }
        else
        {
            return true;
        }
        return false;
    }


    #endregion

    #region Rule Panel

    void OpenRuleScreen()
    {
        ruleScreenObj.SetActive(true);
    }

    public void CloseRuleButton()
    {
        ruleScreenObj.SetActive(false);
    }

    #endregion

    #region Socket

    public void GetWinners(List<int> winnerNo)
    {
        float winnerAmount = potAmount / winnerNo.Count;
        for (int i = 0; i < winnerNo.Count; i++)
        {
            print("Win No : " + winnerNo[i]);
            for (int j = 0; j < playerSquList.Count; j++)
            {
                if (playerSquList[j].playerNo == winnerNo[i] && playerSquList[j].gameObject.activeSelf == true)
                {
                    //Generate Number
                    GameObject genBetObj = Instantiate(betPrefab, prefabParent.transform);
                    genBetObj.transform.GetChild(1).GetComponent<Text>().text = winnerAmount.ToString();
                    genBetObj.transform.position = targetBetObj.transform.position;
                    totalBetAmount = 0;
                    potTxt.text = winnerAmount.ToString();
                    if (playerSquList[j].playerNo == player1.playerNo)
                    {
                        //Add to  winnner Amount

                        float adminPercentage = DataManager.Instance.adminPercentage;

                        float winAmount = winnerAmount;
                        float adminCommssion = (adminPercentage / 100);
                        float playerWinAmount = winAmount - (winAmount * adminCommssion);

                        print(playerWinAmount);

                        if (playerWinAmount != 0)
                        {
                            SoundManager.Instance.CasinoWinSound();
                            DataManager.Instance.AddAmount((float)(playerWinAmount), DataManager.Instance.gameId, "Poker-Win-" + DataManager.Instance.gameId, "won", (float)(adminCommssion), player1.playerNo);
                        }
                    }
                    Destroy(genBetObj, 0.4f);
                }
                
            }
        }
    }

    void CreateAdmin()
    {
        if (DataManager.Instance.joinPlayerDatas[0].userId.Equals(DataManager.Instance.playerData._id))
        {
            isAdmin = true;
           // SetRoomData();
            //TestSocketIO.Instace.SetGameId(DataManager.Instance.tournamentID);
        }
    }
    public void SetRoomData()
    {
        JSONObject obj = new JSONObject();
        int noGet = 0;


        noGet = UnityEngine.Random.Range(1, DataManager.Instance.joinPlayerDatas.Count + 1);


        obj.AddField("DeckNo1", UnityEngine.Random.Range(0, 300));
        obj.AddField("DeckNo2", noGet);
        obj.AddField("dateTime", DateTime.UtcNow.ToString());
        obj.AddField("gameMode", 5);
        TestSocketIO.Instace.SetRoomdata(TestSocketIO.Instace.roomid, obj);
    }
    
    public void SetPokerWon(string value)
    {
        JSONObject obj = new JSONObject();
        obj.AddField("PlayerID", DataManager.Instance.playerData._id);
        obj.AddField("TournamentID", DataManager.Instance.tournamentID);
        obj.AddField("RoomId", TestSocketIO.Instace.roomid);
        obj.AddField("WinnerList", value);
        TestSocketIO.Instace.Senddata("PokerWinnerData", obj);
    }

    public void GetRoomData(int deckNo, int dealearNo)
    {
        //print("Deck no : " + deckNo);
        mainList = listStoreDatas[deckNo].noList;
        gameDealerNo = dealearNo;
        
        foreach (var t in playerSquList.Where(t => t.gameObject.activeSelf == true))
        {
            t.CardGenerate();
        }
        
        if (isAdmin) return;
        if (waitNextRoundScreenObj.activeSelf)
        {
            waitNextRoundScreenObj.SetActive(false);
        }
        StartGamePlay();
        //for (int i = 0; i < teenPattiPlayers.Count; i++)
        //{
        //    teenPattiPlayers[i].CardGenerate();
        //}
        // mainList = listStoreDatas[deckNo].noList;
        // for (int i = 0; i < mainList.Count; i++)
        // {
        //     for (int j = 0; j < cardSuffles.Count; j++)
        //     {
        //         if (j == mainList[i])
        //         {
        //             cardSufflesGen.Add(cardSuffles[j]);
        //         }
        //     }
        // }
    }

    public void GetChat(string playerID, string msg)
    {
        if (playerID.Equals(DataManager.Instance.playerData._id))
        {
            TypeMessageBox typeMessageBox = Instantiate(chatMePrefab, chatPanelParent.transform).GetComponent<TypeMessageBox>();
            typeMessageBox.Update_Message_Box(msg);
        }
        else
        {
            TypeMessageBox typeMessageBox = Instantiate(chatOtherPrefab, chatPanelParent.transform).GetComponent<TypeMessageBox>();
            typeMessageBox.Update_Message_Box(msg);
        }
        Canvas.ForceUpdateCanvases();
    }
    public void GetGift(string sendPlayerID, string receivePlayerId, int giftNo)
    {
        GameObject sendPlayerObj = null;
        GameObject receivePlayerObj = null;

        for (int i = 0; i < pokerPlayers.Count; i++)
        {
            if (pokerPlayers[i].playerId == sendPlayerID)
            {
                sendPlayerObj = pokerPlayers[i].fillLine.gameObject;
            }
            else if (pokerPlayers[i].playerId == receivePlayerId)
            {
                receivePlayerObj = pokerPlayers[i].fillLine.gameObject;
            }
        }

        GameObject giftGen = Instantiate(giftPrefab, giftParentObj.transform);

        for (int i = 0; i < giftBoxes.Count; i++)
        {
            if (i == giftNo)
            {
                giftGen.transform.GetComponent<Image>().sprite = giftBoxes[i].giftSprite;
            }
        }
        giftGen.transform.position = sendPlayerObj.transform.position;
        giftGen.transform.DOMove(receivePlayerObj.transform.position, 0.4f).OnComplete(() =>
        {
            giftGen.transform.DOMove(receivePlayerObj.transform.position, 1f).OnComplete(() =>
            {

                giftGen.transform.DOScale(Vector3.zero, 0.5f).OnComplete(() =>
                {
                    Destroy(giftGen);
                });

            });
        });

    }


    public void ChangePlayerTurn(int pNo)
    {

        JSONObject obj = new JSONObject();
        obj.AddField("PlayerID", DataManager.Instance.playerData._id);
        obj.AddField("TournamentID", DataManager.Instance.tournamentID);
        obj.AddField("RoomId", TestSocketIO.Instace.roomid);
        obj.AddField("PlayerNo", pNo);
        TestSocketIO.Instace.Senddata("PokerChangeTurnData", obj);
    }


    bool isCheckTurnPack(int nextPlayerNo)
    {
        for (int i = 0; i < pokerPlayers.Count; i++)
        {
            if (pokerPlayers[i].gameObject.activeSelf == true && pokerPlayers[i].playerNo == nextPlayerNo && pokerPlayers[i].isFold == true)
            {
                return true;
            }
        }
        return false;
    }

    public void GetPlayerTurn(int playerNo)
    {
        bool isPlayerNotEnter = false;
        int nextPlayerNo = 0;
        if (playerNo == 5)//5
        {
            nextPlayerNo = 1;
        }
        else
        {
            nextPlayerNo = playerNo + 1;
        }
        if (nextPlayerNo == 1)
        {
            if (isCheckTurnPack(nextPlayerNo) == false)
            {
                nextPlayerNo = 1;
            }
            else
            {
                nextPlayerNo = 2;
                if (isCheckTurnPack(nextPlayerNo) == false)
                {
                    nextPlayerNo = 2;
                }
                else
                {
                    nextPlayerNo = 3;
                    if (isCheckTurnPack(nextPlayerNo) == false)
                    {
                        nextPlayerNo = 3;
                    }
                    else
                    {
                        nextPlayerNo = 4;
                        if (isCheckTurnPack(nextPlayerNo) == false)
                        {
                            nextPlayerNo = 4;
                        }
                        else
                        {
                            nextPlayerNo = 5;
                            if (isCheckTurnPack(nextPlayerNo) == false)
                            {
                                nextPlayerNo = 5;
                            }
                        }
                    }
                }
            }
        }
        else if (nextPlayerNo == 2)
        {
            if (isCheckTurnPack(nextPlayerNo) == false)
            {
                nextPlayerNo = 2;
            }
            else
            {
                nextPlayerNo = 3;
                if (isCheckTurnPack(nextPlayerNo) == false)
                {
                    nextPlayerNo = 3;
                }
                else
                {
                    nextPlayerNo = 4;
                    if (isCheckTurnPack(nextPlayerNo) == false)
                    {
                        nextPlayerNo = 4;
                    }
                    else
                    {
                        nextPlayerNo = 5;
                        if (isCheckTurnPack(nextPlayerNo) == false)
                        {
                            nextPlayerNo = 5;
                        }
                        else
                        {
                            nextPlayerNo = 1;
                            if (isCheckTurnPack(nextPlayerNo) == false)
                            {
                                nextPlayerNo = 1;
                            }
                        }
                    }
                }
            }
        }
        else if (nextPlayerNo == 3)
        {
            if (isCheckTurnPack(nextPlayerNo) == false)
            {
                nextPlayerNo = 3;
            }
            else
            {
                nextPlayerNo = 4;
                if (isCheckTurnPack(nextPlayerNo) == false)
                {
                    nextPlayerNo = 4;
                }
                else
                {
                    nextPlayerNo = 5;
                    if (isCheckTurnPack(nextPlayerNo) == false)
                    {
                        nextPlayerNo = 5;
                    }
                    else
                    {
                        nextPlayerNo = 1;
                        if (isCheckTurnPack(nextPlayerNo) == false)
                        {
                            nextPlayerNo = 1;
                        }
                        else
                        {
                            nextPlayerNo = 2;
                            if (isCheckTurnPack(nextPlayerNo) == false)
                            {
                                nextPlayerNo = 2;
                            }
                        }
                    }
                }
            }
        }
        else if (nextPlayerNo == 4)
        {
            if (isCheckTurnPack(nextPlayerNo) == false)
            {
                nextPlayerNo = 4;
            }
            else
            {
                nextPlayerNo = 5;
                if (isCheckTurnPack(nextPlayerNo) == false)
                {
                    nextPlayerNo = 5;
                }
                else
                {
                    nextPlayerNo = 1;
                    if (isCheckTurnPack(nextPlayerNo) == false)
                    {
                        nextPlayerNo = 1;
                    }
                    else
                    {
                        nextPlayerNo = 2;
                        if (isCheckTurnPack(nextPlayerNo) == false)
                        {
                            nextPlayerNo = 2;
                        }
                        else
                        {
                            nextPlayerNo = 3;
                            if (isCheckTurnPack(nextPlayerNo) == false)
                            {
                                nextPlayerNo = 3;
                            }
                        }
                    }
                }
            }
        }
        else if (nextPlayerNo == 5)
        {
            if (isCheckTurnPack(nextPlayerNo) == false)
            {
                nextPlayerNo = 5;
            }
            else
            {
                nextPlayerNo = 1;
                if (isCheckTurnPack(nextPlayerNo) == false)
                {
                    nextPlayerNo = 1;
                }
                else
                {
                    nextPlayerNo = 2;
                    if (isCheckTurnPack(nextPlayerNo) == false)
                    {
                        nextPlayerNo = 2;
                    }
                    else
                    {
                        nextPlayerNo = 3;
                        if (isCheckTurnPack(nextPlayerNo) == false)
                        {
                            nextPlayerNo = 3;
                        }
                        else
                        {
                            nextPlayerNo = 4;
                            if (isCheckTurnPack(nextPlayerNo) == false)
                            {
                                nextPlayerNo = 4;
                            }
                        }
                    }
                }
            }
        }

        for (int i = 0; i < pokerPlayers.Count; i++)
        {
            if (pokerPlayers[i].playerNo == nextPlayerNo)
            {
                pokerPlayers[i].RestartFillLine();
                if (pokerPlayers[i].playerNo == nextPlayerNo && pokerPlayers[i] == player1)
                {
                    //bottomBox.SetActive(true);

                    OpenOnScreen();
                }
                else
                {
                    // bottomBox.SetActive(false);
                    OpenOffScreen();
                }

            }
            else
            {
                pokerPlayers[i].NotATurn();

            }
        }
    }

    #endregion

    #region Game Play UI Player
    
    public void StartGamePlay()
    {
        if (isAdmin)
        {
            SetRoomData();
            TestSocketIO.Instace.SetGameId(DataManager.Instance.tournamentID);
        }
        //Checking bet amount is equal of current players
        InvokeRepeating(nameof(CheckBetAmount),0f,1f);
        waitNextRoundScreenObj.SetActive(false);
        playerFindScreenObj.SetActive(false);
        SoundManager.Instance.CasinoTurnSound();
        DataManager.Instance.UserTurnVibrate();
        for (int i = 0; i < pokerPlayers.Count; i++)
        {
            pokerPlayers[i].isOneTimeEnter = false;
            pokerPlayers[i].isFold = false;
            pokerPlayers[i].isTurn = false;
            pokerPlayers[i].isCalled = false;
            pokerPlayers[i].isBot = false;
            pokerPlayers[i].cardImg1.gameObject.SetActive(false);
            pokerPlayers[i].cardImg2.gameObject.SetActive(false);
            pokerPlayers[i].foldImg.SetActive(false);
            pokerPlayers[i].cardImg1.sprite = commonCardImg;
            pokerPlayers[i].cardImg2.sprite = commonCardImg;
            for (int j = 0; j < pokerPlayers[i].playerWinObj.Length; j++)
            {
                pokerPlayers[i].playerWinObj[j].SetActive(false);
            }
        }

        totalBetAmount = 0f;
        potAmount = 0f;
        potTxt.text = potAmount.ToString();
        _allBetEqual = false;
        _isFlopShowDone = false;
        _isRiverShowDone = false;
        
        for (int i = 0; i < pokerPlayers.Count; i++)
        {
            pokerPlayers[i].gameObject.SetActive(false);
        }
        
        isGameStarted = true;

        StartCoroutine(DataMaintain());

    }
    IEnumerator DataMaintain()
    {
        playerSquList.Clear();
        if (DataManager.Instance.joinPlayerDatas.Count == 2)
        {
            player1.gameObject.SetActive(true);
            player2.gameObject.SetActive(true);
            player3.gameObject.SetActive(false);
            player4.gameObject.SetActive(false);
            player5.gameObject.SetActive(false);

            for (int i = 0; i < DataManager.Instance.joinPlayerDatas.Count; i++)
            {
                if (DataManager.Instance.joinPlayerDatas[i].userId.Equals(DataManager.Instance.playerData._id))
                {
                    player1.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                    player1.playerBalanceTxt.text = DataManager.Instance.joinPlayerDatas[i].balance;
                    player1.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                    player1.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                    player1.playerNo = (i + 1);
                    player1.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                    player1.UpdateAvatar();
                }
                else
                {
                    player2.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                    player2.playerBalanceTxt.text = DataManager.Instance.joinPlayerDatas[i].balance;
                    player2.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                    player2.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                    player2.playerNo = (i + 1);
                    player2.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                    player2.UpdateAvatar();
                }
            }


            if (player1.playerNo == 1)
            {
                playerSquList.Add(player1);
                playerSquList.Add(player2);
            }
            else if (player1.playerNo == 2)
            {
                playerSquList.Add(player2);
                playerSquList.Add(player1);
            }
        }
        else if (DataManager.Instance.joinPlayerDatas.Count == 3)
        {
            player1.gameObject.SetActive(true);
            for (int i = 0; i < DataManager.Instance.joinPlayerDatas.Count; i++)
            {
                if (DataManager.Instance.joinPlayerDatas[i].userId.Equals(DataManager.Instance.playerData._id))
                {
                    player1.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                    player1.playerBalanceTxt.text = DataManager.Instance.joinPlayerDatas[i].balance;
                    player1.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                    player1.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                    player1.playerNo = (i + 1);
                    player1.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                    player1.UpdateAvatar();
                }
            }

            if (player1.playerNo == 1)
            {
                player2.gameObject.SetActive(true);
                player3.gameObject.SetActive(false);
                player4.gameObject.SetActive(true);
                player5.gameObject.SetActive(false);

                playerSquList.Add(player1);
                int cntPlayer = 0;
                for (int i = 0; i < DataManager.Instance.joinPlayerDatas.Count; i++)
                {
                    if (!DataManager.Instance.joinPlayerDatas[i].userId.Equals(DataManager.Instance.playerData._id) && cntPlayer < 2)
                    {
                        if (cntPlayer == 0)
                        {

                            player2.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                            player2.playerBalanceTxt.text = DataManager.Instance.joinPlayerDatas[i].balance;
                            player2.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                            player2.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                            player2.playerNo = (i + 1);
                            player2.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                            player2.UpdateAvatar();
                            playerSquList.Add(player2);
                            cntPlayer++;

                        }
                        else if (cntPlayer == 1)
                        {
                            player4.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                            player4.playerBalanceTxt.text = DataManager.Instance.joinPlayerDatas[i].balance;
                            player4.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                            player4.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                            player4.playerNo = (i + 1);
                            player4.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                            player4.UpdateAvatar();
                            playerSquList.Add(player4);
                            cntPlayer++;
                        }
                    }
                }
            }
            else if (player1.playerNo == 2)
            {
                player2.gameObject.SetActive(true);
                player3.gameObject.SetActive(true);
                player4.gameObject.SetActive(false);
                player5.gameObject.SetActive(false);
                int cntPlayer = 0;
                for (int i = 0; i < DataManager.Instance.joinPlayerDatas.Count; i++)
                {
                    if (!DataManager.Instance.joinPlayerDatas[i].userId.Equals(DataManager.Instance.playerData._id) && cntPlayer < 2)
                    {
                        if (cntPlayer == 0)
                        {

                            player3.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                            player3.playerBalanceTxt.text = DataManager.Instance.joinPlayerDatas[i].balance;
                            player3.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                            player3.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                            player3.playerNo = (i + 1);
                            player3.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                            player3.UpdateAvatar();
                            playerSquList.Add(player3);
                            playerSquList.Add(player1);
                            cntPlayer++;
                        }
                        else if (cntPlayer == 1)
                        {
                            player2.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                            player2.playerBalanceTxt.text = DataManager.Instance.joinPlayerDatas[i].balance;
                            player2.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                            player2.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                            player2.playerNo = (i + 1);
                            player2.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                            player2.UpdateAvatar();
                            playerSquList.Add(player2);

                            cntPlayer++;
                        }
                    }
                }
            }
            else if (player1.playerNo == 3)
            {
                player2.gameObject.SetActive(false);
                player3.gameObject.SetActive(true);
                player4.gameObject.SetActive(false);
                player5.gameObject.SetActive(true);
                int cntPlayer = 0;
                for (int i = 0; i < DataManager.Instance.joinPlayerDatas.Count; i++)
                {
                    if (!DataManager.Instance.joinPlayerDatas[i].userId.Equals(DataManager.Instance.playerData._id) && cntPlayer < 2)
                    {
                        if (cntPlayer == 0)
                        {
                            player5.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                            player5.playerBalanceTxt.text = DataManager.Instance.joinPlayerDatas[i].balance;
                            player5.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                            player5.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                            player5.playerNo = (i + 1);
                            player5.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                            player5.UpdateAvatar();
                            playerSquList.Add(player5);
                            cntPlayer++;
                        }
                        else if (cntPlayer == 1)
                        {
                            player3.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                            player3.playerBalanceTxt.text = DataManager.Instance.joinPlayerDatas[i].balance;
                            player3.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                            player3.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                            player3.playerNo = (i + 1);
                            player3.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                            player3.UpdateAvatar();
                            playerSquList.Add(player3);
                            playerSquList.Add(player1);
                            cntPlayer++;
                        }
                    }
                }
            }
        }
        else if (DataManager.Instance.joinPlayerDatas.Count == 4)
        {
            player1.gameObject.SetActive(true);
            for (int i = 0; i < DataManager.Instance.joinPlayerDatas.Count; i++)
            {
                if (DataManager.Instance.joinPlayerDatas[i].userId.Equals(DataManager.Instance.playerData._id))
                {
                    player1.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                    player1.playerBalanceTxt.text = DataManager.Instance.joinPlayerDatas[i].balance;
                    player1.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                    player1.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                    player1.playerNo = (i + 1);
                    player1.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                    player1.UpdateAvatar();
                }
            }

            if (player1.playerNo == 1)
            {
                player2.gameObject.SetActive(true);
                player3.gameObject.SetActive(false);
                player4.gameObject.SetActive(true);
                player5.gameObject.SetActive(true);
                playerSquList.Add(player1);

                int cntPlayer = 0;
                for (int i = 0; i < DataManager.Instance.joinPlayerDatas.Count; i++)
                {
                    if (!DataManager.Instance.joinPlayerDatas[i].userId.Equals(DataManager.Instance.playerData._id) && cntPlayer < 3)
                    {
                        if (cntPlayer == 0)
                        {
                            player2.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                            player2.playerBalanceTxt.text = DataManager.Instance.joinPlayerDatas[i].balance;
                            player2.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                            player2.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                            player2.playerNo = (i + 1);
                            player2.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                            player2.UpdateAvatar();
                            playerSquList.Add(player2);
                            cntPlayer++;
                        }
                        else if (cntPlayer == 1)
                        {
                            player4.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                            player4.playerBalanceTxt.text = DataManager.Instance.joinPlayerDatas[i].balance;
                            player4.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                            player4.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                            player4.playerNo = (i + 1);
                            player4.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                            player4.UpdateAvatar();
                            playerSquList.Add(player4);
                            cntPlayer++;
                        }
                        else if (cntPlayer == 2)
                        {
                            player5.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                            player5.playerBalanceTxt.text = DataManager.Instance.joinPlayerDatas[i].balance;
                            player5.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                            player5.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                            player5.playerNo = (i + 1);
                            player5.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                            player5.UpdateAvatar();
                            playerSquList.Add(player5);
                            cntPlayer++;
                        }
                    }
                }
            }
            else if (player1.playerNo == 2)
            {
                player2.gameObject.SetActive(true);
                player3.gameObject.SetActive(true);
                player4.gameObject.SetActive(true);
                player5.gameObject.SetActive(false);

                int cntPlayer = 0;
                for (int i = 0; i < DataManager.Instance.joinPlayerDatas.Count; i++)
                {
                    if (!DataManager.Instance.joinPlayerDatas[i].userId.Equals(DataManager.Instance.playerData._id) && cntPlayer < 3)
                    {
                        if (cntPlayer == 0)
                        {
                            player3.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                            player3.playerBalanceTxt.text = DataManager.Instance.joinPlayerDatas[i].balance;
                            player3.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                            player3.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                            player3.playerNo = (i + 1);
                            player3.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                            player3.UpdateAvatar();
                            playerSquList.Add(player3);
                            playerSquList.Add(player1);
                            cntPlayer++;
                        }
                        else if (cntPlayer == 1)
                        {
                            player2.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                            player2.playerBalanceTxt.text = DataManager.Instance.joinPlayerDatas[i].balance;
                            player2.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                            player2.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                            player2.playerNo = (i + 1);
                            player2.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                            player2.UpdateAvatar();
                            playerSquList.Add(player2);
                            cntPlayer++;
                        }
                        else if (cntPlayer == 2)
                        {
                            player4.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                            player4.playerBalanceTxt.text = DataManager.Instance.joinPlayerDatas[i].balance;
                            player4.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                            player4.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                            player4.playerNo = (i + 1);
                            player4.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                            player4.UpdateAvatar();
                            playerSquList.Add(player4);
                            cntPlayer++;
                        }
                    }
                }
            }
            else if (player1.playerNo == 3)
            {
                player2.gameObject.SetActive(true);
                player3.gameObject.SetActive(false);
                player4.gameObject.SetActive(true);
                player5.gameObject.SetActive(true);

                int cntPlayer = 0;
                for (int i = 0; i < DataManager.Instance.joinPlayerDatas.Count; i++)
                {
                    if (!DataManager.Instance.joinPlayerDatas[i].userId.Equals(DataManager.Instance.playerData._id) && cntPlayer < 3)
                    {
                        if (cntPlayer == 0)
                        {
                            player5.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                            player5.playerBalanceTxt.text = DataManager.Instance.joinPlayerDatas[i].balance;
                            player5.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                            player5.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                            player5.playerNo = (i + 1);
                            player5.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                            player5.UpdateAvatar();
                            playerSquList.Add(player5);
                            cntPlayer++;
                        }
                        else if (cntPlayer == 1)
                        {
                            player3.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                            player3.playerBalanceTxt.text = DataManager.Instance.joinPlayerDatas[i].balance;
                            player3.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                            player3.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                            player3.playerNo = (i + 1);
                            player3.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                            player3.UpdateAvatar();
                            playerSquList.Add(player3);
                            playerSquList.Add(player1);
                            cntPlayer++;
                        }
                        else if (cntPlayer == 2)
                        {
                            player2.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                            player2.playerBalanceTxt.text = DataManager.Instance.joinPlayerDatas[i].balance;
                            player2.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                            player2.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                            player2.playerNo = (i + 1);
                            player2.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                            player2.UpdateAvatar();
                            playerSquList.Add(player2);
                            cntPlayer++;
                        }
                    }
                }
            }
            else if (player1.playerNo == 4)
            {
                player2.gameObject.SetActive(false);
                player3.gameObject.SetActive(true);
                player4.gameObject.SetActive(true);
                player5.gameObject.SetActive(true);

                int cntPlayer = 0;
                for (int i = 0; i < DataManager.Instance.joinPlayerDatas.Count; i++)
                {
                    if (!DataManager.Instance.joinPlayerDatas[i].userId.Equals(DataManager.Instance.playerData._id) && cntPlayer < 3)
                    {
                        if (cntPlayer == 0)
                        {
                            player4.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                            player4.playerBalanceTxt.text = DataManager.Instance.joinPlayerDatas[i].balance;
                            player4.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                            player4.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                            player4.playerNo = (i + 1);
                            player4.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                            player4.UpdateAvatar();
                            playerSquList.Add(player4);
                            cntPlayer++;
                        }
                        else if (cntPlayer == 1)
                        {
                            player5.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                            player5.playerBalanceTxt.text = DataManager.Instance.joinPlayerDatas[i].balance;
                            player5.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                            player5.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                            player5.playerNo = (i + 1);
                            player5.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                            player5.UpdateAvatar();
                            playerSquList.Add(player5);
                            cntPlayer++;
                        }
                        else if (cntPlayer == 2)
                        {
                            player3.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                            player3.playerBalanceTxt.text = DataManager.Instance.joinPlayerDatas[i].balance;
                            player3.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                            player3.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                            player3.playerNo = (i + 1);
                            player3.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                            player3.UpdateAvatar();
                            playerSquList.Add(player3);
                            playerSquList.Add(player1);

                            cntPlayer++;
                        }
                    }
                }
            }
        }
        else if (DataManager.Instance.joinPlayerDatas.Count == 5)
        {
            player1.gameObject.SetActive(true);
            for (int i = 0; i < DataManager.Instance.joinPlayerDatas.Count; i++)
            {
                if (DataManager.Instance.joinPlayerDatas[i].userId.Equals(DataManager.Instance.playerData._id))
                {
                    player1.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                    player1.playerBalanceTxt.text = DataManager.Instance.joinPlayerDatas[i].balance;
                    player1.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                    player1.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                    player1.playerNo = (i + 1);
                    player1.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                    player1.UpdateAvatar();
                }
            }
            if (player1.playerNo == 1)
            {
                player2.gameObject.SetActive(true);
                player3.gameObject.SetActive(true);
                player4.gameObject.SetActive(true);
                player5.gameObject.SetActive(true);
                playerSquList.Add(player1);

                int cntPlayer = 0;
                for (int i = 0; i < DataManager.Instance.joinPlayerDatas.Count; i++)
                {
                    if (!DataManager.Instance.joinPlayerDatas[i].userId.Equals(DataManager.Instance.playerData._id) && cntPlayer < 4)
                    {
                        if (cntPlayer == 0)
                        {
                            player2.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                            player2.playerBalanceTxt.text = DataManager.Instance.joinPlayerDatas[i].balance;
                            player2.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                            player2.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                            player2.playerNo = (i + 1);
                            player2.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                            player2.UpdateAvatar();
                            playerSquList.Add(player2);
                            cntPlayer++;
                        }
                        else if (cntPlayer == 1)
                        {
                            player3.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                            player3.playerBalanceTxt.text = DataManager.Instance.joinPlayerDatas[i].balance;
                            player3.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                            player3.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                            player3.playerNo = (i + 1);
                            player3.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                            player3.UpdateAvatar();
                            playerSquList.Add(player3);
                            cntPlayer++;
                        }
                        else if (cntPlayer == 2)
                        {
                            player4.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                            player4.playerBalanceTxt.text = DataManager.Instance.joinPlayerDatas[i].balance;
                            player4.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                            player4.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                            player4.playerNo = (i + 1);
                            player4.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                            player4.UpdateAvatar();
                            playerSquList.Add(player4);
                            cntPlayer++;
                        }
                        else if (cntPlayer == 3)
                        {
                            player5.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                            player5.playerBalanceTxt.text = DataManager.Instance.joinPlayerDatas[i].balance;
                            player5.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                            player5.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                            player5.playerNo = (i + 1);
                            player5.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                            player5.UpdateAvatar();
                            playerSquList.Add(player5);
                            cntPlayer++;
                        }
                    }
                }
            }
            else if (player1.playerNo == 2)
            {
                player2.gameObject.SetActive(true);
                player3.gameObject.SetActive(true);
                player4.gameObject.SetActive(true);
                player5.gameObject.SetActive(true);
                playerSquList.Add(player1);

                int cntPlayer = 0;
                for (int i = 0; i < DataManager.Instance.joinPlayerDatas.Count; i++)
                {
                    if (!DataManager.Instance.joinPlayerDatas[i].userId.Equals(DataManager.Instance.playerData._id) && cntPlayer < 4)
                    {
                        if (cntPlayer == 0)
                        {
                            player5.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                            player5.playerBalanceTxt.text = DataManager.Instance.joinPlayerDatas[i].balance;
                            player5.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                            player5.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                            player5.playerNo = (i + 1);
                            player5.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                            player5.UpdateAvatar();
                            playerSquList.Add(player5);
                            //playerSquList.Add(player1);
                            cntPlayer++;
                        }
                        else if (cntPlayer == 1)
                        {
                            player2.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                            player2.playerBalanceTxt.text = DataManager.Instance.joinPlayerDatas[i].balance;
                            player2.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                            player2.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                            player2.playerNo = (i + 1);
                            player2.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                            player2.UpdateAvatar();
                            playerSquList.Add(player2);
                            cntPlayer++;
                        }
                        else if (cntPlayer == 2)
                        {
                            player3.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                            player3.playerBalanceTxt.text = DataManager.Instance.joinPlayerDatas[i].balance;
                            player3.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                            player3.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                            player3.playerNo = (i + 1);
                            player3.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                            player3.UpdateAvatar();
                            playerSquList.Add(player3);
                            cntPlayer++;
                        }
                        else if (cntPlayer == 3)
                        {
                            player4.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                            player4.playerBalanceTxt.text = DataManager.Instance.joinPlayerDatas[i].balance;
                            player4.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                            player4.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                            player4.playerNo = (i + 1);
                            player4.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                            player4.UpdateAvatar();
                            playerSquList.Add(player4);
                            cntPlayer++;
                        }
                    }
                }
            }
            else if (player1.playerNo == 3)
            {
                player2.gameObject.SetActive(true);
                player3.gameObject.SetActive(true);
                player4.gameObject.SetActive(true);
                player5.gameObject.SetActive(true);
                playerSquList.Add(player1);

                int cntPlayer = 0;
                for (int i = 0; i < DataManager.Instance.joinPlayerDatas.Count; i++)
                {
                    if (!DataManager.Instance.joinPlayerDatas[i].userId.Equals(DataManager.Instance.playerData._id) && cntPlayer < 4)
                    {
                        if (cntPlayer == 0)
                        {
                            player4.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                            player4.playerBalanceTxt.text = DataManager.Instance.joinPlayerDatas[i].balance;
                            player4.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                            player4.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                            player4.playerNo = (i + 1);
                            player4.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                            player4.UpdateAvatar();
                            playerSquList.Add(player4);
                            cntPlayer++;
                        }
                        else if (cntPlayer == 1)
                        {
                            player5.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                            player5.playerBalanceTxt.text = DataManager.Instance.joinPlayerDatas[i].balance;
                            player5.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                            player5.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                            player5.playerNo = (i + 1);
                            player5.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                            player5.UpdateAvatar();
                            playerSquList.Add(player5);
                           // playerSquList.Add(player1);
                            cntPlayer++;
                        }
                        else if (cntPlayer == 2)
                        {
                            player2.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                            player2.playerBalanceTxt.text = DataManager.Instance.joinPlayerDatas[i].balance;
                            player2.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                            player2.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                            player2.playerNo = (i + 1);
                            player2.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                            player2.UpdateAvatar();
                            playerSquList.Add(player2);
                            cntPlayer++;
                        }
                        else if (cntPlayer == 3)
                        {
                            player3.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                            player3.playerBalanceTxt.text = DataManager.Instance.joinPlayerDatas[i].balance;
                            player3.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                            player3.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                            player3.playerNo = (i + 1);
                            player3.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                            player3.UpdateAvatar();
                            playerSquList.Add(player3);
                            cntPlayer++;
                        }
                    }
                }
            }
            else if (player1.playerNo == 4)
            {
                player2.gameObject.SetActive(true);
                player3.gameObject.SetActive(true);
                player4.gameObject.SetActive(true);
                player5.gameObject.SetActive(true);
                playerSquList.Add(player1);

                int cntPlayer = 0;
                for (int i = 0; i < DataManager.Instance.joinPlayerDatas.Count; i++)
                {
                    if (!DataManager.Instance.joinPlayerDatas[i].userId.Equals(DataManager.Instance.playerData._id) && cntPlayer < 4)
                    {
                        if (cntPlayer == 0)
                        {
                            player3.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                            player3.playerBalanceTxt.text = DataManager.Instance.joinPlayerDatas[i].balance;
                            player3.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                            player3.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                            player3.playerNo = (i + 1);
                            player3.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                            player3.UpdateAvatar();
                            playerSquList.Add(player3);
                            cntPlayer++;
                        }
                        else if (cntPlayer == 1)
                        {
                            player4.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                            player4.playerBalanceTxt.text = DataManager.Instance.joinPlayerDatas[i].balance;
                            player4.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                            player4.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                            player4.playerNo = (i + 1);
                            player4.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                            player4.UpdateAvatar();
                            playerSquList.Add(player4);
                            cntPlayer++;
                        }
                        else if (cntPlayer == 2)
                        {
                            player5.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                            player5.playerBalanceTxt.text = DataManager.Instance.joinPlayerDatas[i].balance;
                            player5.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                            player5.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                            player5.playerNo = (i + 1);
                            player5.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                            player5.UpdateAvatar();
                            playerSquList.Add(player5);
                            //playerSquList.Add(player1);
                            cntPlayer++;
                        }
                        else if (cntPlayer == 3)
                        {
                            player2.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                            player2.playerBalanceTxt.text = DataManager.Instance.joinPlayerDatas[i].balance;
                            player2.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                            player2.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                            player2.playerNo = (i + 1);
                            player2.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                            player2.UpdateAvatar();
                            playerSquList.Add(player2);
                            cntPlayer++;
                        }
                    }
                }
            }
            else if (player1.playerNo == 5)
            {
                player2.gameObject.SetActive(true);
                player3.gameObject.SetActive(true);
                player4.gameObject.SetActive(true);
                player5.gameObject.SetActive(true);
                playerSquList.Add(player1);

                int cntPlayer = 0;
                for (int i = 0; i < DataManager.Instance.joinPlayerDatas.Count; i++)
                {
                    if (!DataManager.Instance.joinPlayerDatas[i].userId.Equals(DataManager.Instance.playerData._id) && cntPlayer < 4)
                    {
                        if (cntPlayer == 0)
                        {
                            player2.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                            player2.playerBalanceTxt.text = DataManager.Instance.joinPlayerDatas[i].balance;
                            player2.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                            player2.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                            player2.playerNo = (i + 1);
                            player2.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                            player2.UpdateAvatar();
                            playerSquList.Add(player2);
                            cntPlayer++;
                        }
                        else if (cntPlayer == 1)
                        {
                            player3.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                            player3.playerBalanceTxt.text = DataManager.Instance.joinPlayerDatas[i].balance;
                            player3.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                            player3.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                            player3.playerNo = (i + 1);
                            player3.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                            player3.UpdateAvatar();
                            playerSquList.Add(player3);
                            cntPlayer++;
                        }
                        else if (cntPlayer == 2)
                        {
                            player4.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                            player4.playerBalanceTxt.text = DataManager.Instance.joinPlayerDatas[i].balance;
                            player4.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                            player4.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                            player4.playerNo = (i + 1);
                            player4.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                            player4.UpdateAvatar();
                            playerSquList.Add(player4);
                            cntPlayer++;
                        }
                        else if (cntPlayer == 3)
                        {
                            player5.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                            player5.playerBalanceTxt.text = DataManager.Instance.joinPlayerDatas[i].balance;
                            player5.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                            player5.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                            player5.playerNo = (i + 1);
                            player5.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                            player5.UpdateAvatar();
                            playerSquList.Add(player5);
                            //playerSquList.Add(player1);
                            cntPlayer++;
                        }
                    }
                }
            }
        }
        
        int playerSend = DataManager.Instance.joinPlayerDatas.Count;

        float speed = 0.2f;

        for (int i = 0; i < pokerPlayers.Count; i++)
        {
            GameObject obj = Instantiate(cardTmpPrefab, prefabParent.transform);
            SoundManager.Instance.CasinoCardMoveSound();
            obj.transform.position = cardTmpStart.transform.position;

            obj.transform.DOMove(pokerPlayers[i].cardImg1.transform.position, speed).OnComplete(() =>
            {
                Destroy(obj);
                pokerPlayers[i].cardImg1.gameObject.SetActive(true);
            });

            yield return new WaitForSeconds(speed);
        }
        yield return new WaitForSeconds(speed);
        for (int i = 0; i < pokerPlayers.Count; i++)
        {
            GameObject obj = Instantiate(cardTmpPrefab, prefabParent.transform);
            SoundManager.Instance.CasinoCardMoveSound();
            obj.transform.position = cardTmpStart.transform.position;


            obj.transform.DOMove(pokerPlayers[i].cardImg2.transform.position, speed).OnComplete(() =>
            {
                Destroy(obj);
                pokerPlayers[i].cardImg2.gameObject.SetActive(true);
            });
            yield return new WaitForSeconds(speed);
        }
        yield return new WaitForSeconds(speed);


        for (int i = 0; i < playerSquList.Count; i++)
        {
            if (playerSquList[i].playerNo == gameDealerNo)
            {
                playerSquList[i].delearObj.SetActive(true);
                playerSquList[i].isTurn = true;
            }
            else
            {
                playerSquList[i].delearObj.SetActive(false);
            }
        }
        
        bool isSB = false;
        bool isBB = false;
        if (gameDealerNo == 1 && player1.playerNo != gameDealerNo)
        {
            if (DataManager.Instance.joinPlayerDatas.Count == 2)
            {
                if (player1.playerNo == 2)
                {
                    isSB = true;
                    isBB = false;
                }
            }
            if (DataManager.Instance.joinPlayerDatas.Count == 3 || DataManager.Instance.joinPlayerDatas.Count == 4 || DataManager.Instance.joinPlayerDatas.Count == 5)
            {
                if (player1.playerNo == 2)
                {
                    isSB = true;
                    isBB = false;
                }
                else if (player1.playerNo == 3)
                {
                    isSB = false;
                    isBB = true;
                }
            }
        }
        else if (gameDealerNo == 2 && player1.playerNo != gameDealerNo)
        {
            if (DataManager.Instance.joinPlayerDatas.Count == 2)
            {
                if (player1.playerNo == 1)
                {
                    isSB = true;
                    isBB = false;
                }
            }
            if (DataManager.Instance.joinPlayerDatas.Count == 3)
            {
                if (player1.playerNo == 1)
                {
                    isSB = false;
                    isBB = true;
                }
                if (player1.playerNo == 3)
                {
                    isSB = true;
                    isBB = false;
                }
            }
            if (DataManager.Instance.joinPlayerDatas.Count == 4)
            {
                if (player1.playerNo == 3)
                {
                    isSB = true;
                    isBB = false;
                }
                if (player1.playerNo == 4)
                {
                    isSB = false;
                    isBB = true;
                }
            }

        }
        else if (gameDealerNo == 3 && player1.playerNo != gameDealerNo)
        {
            if (DataManager.Instance.joinPlayerDatas.Count == 3)
            {
                if (player1.playerNo == 1)
                {
                    isSB = true;
                    isBB = false;
                }
                else if (player1.playerNo == 2)
                {
                    isSB = false;
                    isBB = true;
                }

            }
            if (DataManager.Instance.joinPlayerDatas.Count == 4)
            {
                if (player1.playerNo == 4)
                {
                    isSB = true;
                    isBB = false;
                }
                else if (player1.playerNo == 1)
                {
                    isSB = false;
                    isBB = true;
                }
            }
            if (DataManager.Instance.joinPlayerDatas.Count == 5)
            {
                if (player1.playerNo == 4)
                {
                    isSB = true;
                    isBB = false;
                }
                else if (player1.playerNo == 5)
                {
                    isSB = false;
                    isBB = true;
                }
            }
        }
        else if (gameDealerNo == 4 && player1.playerNo != gameDealerNo)
        {
            if (DataManager.Instance.joinPlayerDatas.Count == 4)
            {
                if (player1.playerNo == 1)
                {
                    isSB = true;
                    isBB = false;
                }
                else if (player1.playerNo == 2)
                {
                    isSB = false;
                    isBB = true;
                }

            }
            if (DataManager.Instance.joinPlayerDatas.Count == 5)
            {
                if (player1.playerNo == 5)
                {
                    isSB = true;
                    isBB = false;
                }
                else if (player1.playerNo == 1)
                {
                    isSB = false;
                    isBB = true;
                }

            }
        }
        else if (gameDealerNo == 5 && player1.playerNo != gameDealerNo)
        {
            if (DataManager.Instance.joinPlayerDatas.Count == 5)
            {
                if (player1.playerNo == 1)
                {
                    isSB = true;
                    isBB = false;
                }
                else if (player1.playerNo == 2)
                {
                    isSB = false;
                    isBB = true;
                }

            }
        }

        //float betAmount = 0;
        if (isSB == true)
        {
            // single bet
           // player1.PlayerSetBet(sbAmount, "start");
        }
        else if (isBB == true)
        {
            // double bet
            //player1.PlayerSetBet(bbAmount, "start");
        }

        lastPrice = bbAmount;
       


        if (player1.delearObj.activeSelf == true)
        {
            player1.RestartFillLine();
            //player1.PlayerSetBet(lastPrice,);
            OpenOnScreen();
        }
        else
        {
            player1.NotATurn();
            OpenOffScreen();
        }

        //for (int i = 0; i < playerSquList.Count; i++)
        //{
        //    if (playerSquList[i].playerNo == 1)
        //    {
        //        playerSquList[i].RestartFillLine();
        //        if (playerSquList[i].playerNo == player1.playerNo)
        //        {
        //            //bottomBox.SetActive(true);
        //        }
        //        else
        //        {
        //            //bottomBox.SetActive(false);
        //        }
        //    }
        //    else
        //    {
        //        playerSquList[i].NotATurn();
        //    }
        //}

        for (int i = 0; i < playerSquList.Count; i++)
        {
            if (playerSquList[i].gameObject.activeSelf == true)
            {
                playerSquList[i].CardGenerate();
            }
        }

        
        //StartCoroutine(TurnCardShow());
        //StartCoroutine(RiverCardShow());
        
        player1.DisplayPlayerCard();
        isGameStop = true;
        ActivateBotPlayers();
        
    }
    public void ResetBot()
    {
        player1.isBot = false;
        player2.isBot = false;
        player3.isBot = false;
        player4.isBot = false;
        player5.isBot = false;
    }

    
    private void ActivateBotPlayers()
    {
        /*switch (isAdmin)
        {
            case true:
                switch (MainMenuManager.Instance.botPlayers)
                {
                    case 4:
                        player2.isBot = true;
                        player3.isBot = true;
                        player4.isBot = true;
                        player5.isBot = true;
                        break;
                    case 3:
                        player3.isBot = true;
                        player4.isBot = true;
                        player5.isBot = true;
                        break;
                    case 2:
                        player4.isBot = true;
                        player5.isBot = true;
                        break;
                    case 1:
                        player5.isBot = true;
                        break;
                }

                break;
            case false:
                switch (MainMenuManager.Instance.botPlayers)
                {
                    case 4:
                        player2.isBot = true;
                        player3.isBot = true;
                        player4.isBot = true;
                        player5.isBot = true;
                        break;
                    case 3:
                        player2.isBot = true;
                        player3.isBot = true;
                        player4.isBot = true;
                        break;
                    case 2:
                        player2.isBot = true;
                        player3.isBot = true;
                        break;
                    case 1:
                        player2.isBot = true;
                        break;
                }

                break;
        }*/
        
        for (int i = 0; i < DataManager.Instance.joinPlayerDatas.Count; i++)
        {
            if (DataManager.Instance.joinPlayerDatas[i].userId.EndsWith("TeenPatti"))
            {
                playerSquList[i].isBot = true;
            }
        }
    }

    #endregion

    #region Bet Anim

    public void GetBotBetNo(int num, int botPlayerNo, float betAmount)
    {
        if (isAdmin) return;
        switch (num)
        {
            case 1:
            {
                int index = playerSquList.FindIndex(playerSqu => playerSqu.playerNo == botPlayerNo);
                if(playerSquList[index].isFold) return;
                BetAnim(playerSquList[index], betAmount);
                SoundManager.Instance.ThreeBetSound();
                break;
            }
            case 2:
            {
                int index = playerSquList.FindIndex(playerSqu => playerSqu.playerNo == botPlayerNo);
                if(playerSquList[index].isFold) return;
                BetAnim(playerSquList[index], betAmount);
                SoundManager.Instance.ThreeBetSound();
                break;
            }
            case 3:
            {
                int index = playerSquList.FindIndex(playerSqu => playerSqu.playerNo == botPlayerNo);
                if(playerSquList[index].isFold) return;
                BetAnim(playerSquList[index], betAmount);
                SoundManager.Instance.ThreeBetSound();
                break;
            }
            case 4:
            {
                break;
            }
        }
    }

    public void BetAnim(PokerPlayer player, float amount)
    {
        GameObject genBetObj = Instantiate(betPrefab, prefabParent.transform);
        genBetObj.transform.GetChild(1).GetComponent<Text>().text = amount.ToString();
        genBetObj.transform.position = player.avatarImg.transform.position;
        genBetObj.transform.DOMove(player.betObj.transform.position, 0.3f).OnComplete(() =>
        {
            Destroy(genBetObj);
            player.betAmount += amount;
            totalBetAmount += player.betAmount;
            player.betTxt.text = player.betAmount.ToString();
        });
    }

    public void Player1BetIn()
    {
        GameObject genBetObj = Instantiate(betPrefab, player1.betObj.transform);
        genBetObj.transform.GetChild(1).GetComponent<Text>().text = player1.betAmount.ToString();
        genBetObj.transform.position = player1.betObj.transform.position;
        genBetObj.transform.DOMove(targetBetObj.transform.position, 0.3f).OnComplete(() =>
        {
            Destroy(genBetObj);
            potAmount = totalBetAmount;
            potTxt.text = potAmount.ToString();
        });
            
    }
    public void Player2BetIn()
    {
        GameObject genBetObj = Instantiate(betPrefab, player2.betObj.transform);
        genBetObj.transform.GetChild(1).GetComponent<Text>().text = player2.betAmount.ToString();
        genBetObj.transform.position = player2.betObj.transform.position;
        genBetObj.transform.DOMove(targetBetObj.transform.position, 0.3f).OnComplete(() =>
        {
            Destroy(genBetObj);
            potAmount = totalBetAmount;
            potTxt.text = potAmount.ToString();
        });
           
    }
    public void Player3BetIn()
    {
        GameObject genBetObj = Instantiate(betPrefab, player3.betObj.transform);
        genBetObj.transform.GetChild(1).GetComponent<Text>().text = player3.betAmount.ToString();
        genBetObj.transform.position = player3.betObj.transform.position;
        genBetObj.transform.DOMove(targetBetObj.transform.position, 0.3f).OnComplete(() =>
        {
            Destroy(genBetObj);
            potAmount = totalBetAmount;
            potTxt.text = potAmount.ToString();
        });
            
    }
    public void Player4BetIn()
    {
        GameObject genBetObj = Instantiate(betPrefab, player4.betObj.transform);
        genBetObj.transform.GetChild(1).GetComponent<Text>().text = player4.betAmount.ToString();
        genBetObj.transform.position = player4.betObj.transform.position;
        genBetObj.transform.DOMove(targetBetObj.transform.position, 0.3f).OnComplete(() =>
        {
            Destroy(genBetObj);
            potAmount = totalBetAmount;
            potTxt.text = potAmount.ToString();
        });
            
    }
    public void Player5BetIn()
    {
        GameObject genBetObj = Instantiate(betPrefab, player5.betObj.transform);
        genBetObj.transform.GetChild(1).GetComponent<Text>().text = player5.betAmount.ToString();
        genBetObj.transform.position = player5.betObj.transform.position;
        genBetObj.transform.DOMove(targetBetObj.transform.position, 0.3f).OnComplete(() =>
        {
            Destroy(genBetObj);
            potAmount = totalBetAmount;
            potTxt.text = potAmount.ToString();
        });
           
    }


    public void SendPokerBet(int pNo, float amount, string betType)
    {
        JSONObject obj = new JSONObject();
        obj.AddField("PlayerID", DataManager.Instance.playerData._id);
        obj.AddField("TournamentID", DataManager.Instance.tournamentID);
        obj.AddField("RoomId", TestSocketIO.Instace.roomid);
        obj.AddField("PlayerNo", pNo);
        obj.AddField("BetAmount", amount);
        obj.AddField("BetType", betType);//raise,allin,call,start
        TestSocketIO.Instace.Senddata("PokerSendBetData", obj);
    }

    public void SendPokerPlayerFold(string foldPlayer)
    {
        JSONObject obj = new JSONObject();
        obj.AddField("PlayerID", DataManager.Instance.playerData._id);
        obj.AddField("TournamentID", DataManager.Instance.tournamentID);
        obj.AddField("RoomId", TestSocketIO.Instace.roomid);
        obj.AddField("FoldPlayerId", foldPlayer);
        TestSocketIO.Instace.Senddata("PokerSendFlodData", obj);
    }
    
    public void SetPokerWonData(string winnerPlayerId)
    {
        JSONObject obj = new JSONObject();
        obj.AddField("PlayerID", DataManager.Instance.playerData._id);
        obj.AddField("TournamentID", DataManager.Instance.tournamentID);
        obj.AddField("RoomId", TestSocketIO.Instace.roomid);
        obj.AddField("WinnerPlayerId", winnerPlayerId);
        //obj.AddField("WinnerList", value);
        obj.AddField("Action", "WinData");
        TestSocketIO.Instace.Senddata("PokerFinalWinnerData", obj);
    }

    public void GetPokerBet(int playerNo, float betAmount, string betType)
    {
        if (betType == "allin")
        {
            isAllIn = true;
            lastPrice = betAmount;
        }
        else if (betType == "raise")
        {
            lastPrice = betAmount;
        }
        else if (betType == "call")
        {
            lastPrice = betAmount;
        }
        else if (betType == "start")
        {

        }

        for (int i = 0; i < pokerPlayers.Count; i++)
        {
            if (pokerPlayers[i].playerNo == playerNo && pokerPlayers[i].gameObject.activeSelf == true)
            {
                pokerPlayers[i].PlayerSetSocketBet(betAmount, betType);
            }
        }
    }
    public void GetPokerFold(string playerId)
    {
        for (int i = 0; i < pokerPlayers.Count; i++)
        {
            if (pokerPlayers[i].playerId == playerId && pokerPlayers[i].gameObject.activeSelf == true)
            {
                pokerPlayers[i].isFold = true;
                pokerPlayers[i].foldImg.SetActive(true);
            }
        }
        ChangePlayerTurn(playerNo);
    }
    #endregion

    #region Win

    public void WinPoker()
    {
        WinBeforeAllDataManage();
    }

    #endregion
    

    public void ChangeAAdmin(string leavePlayerId, string adminId)
    {

        if (DataManager.Instance.playerData._id.Equals(DataManager.Instance.joinPlayerDatas[0].userId))
        {
            isAdmin = true;

            if (DataManager.Instance.joinPlayerDatas.Count == 5 && waitNextRoundScreenObj.activeSelf)
            {
                if (waitNextRoundScreenObj.activeSelf == true)
                {
                    waitNextRoundScreenObj.SetActive(false);
                }
            }
        }
        else
        {
            if (!isGameStarted)
            {
                isAdmin = false;
            }
        }
        for (int i = 0; i < pokerPlayers.Count; i++)
        {
            if (pokerPlayers[i].playerId.Equals(leavePlayerId))
            {
                pokerPlayers[i].isFold = true;
                pokerPlayers[i].foldImg.SetActive(true);
                //pokerPlayers[i].gameObject.SetActive(false);
                StartCoroutine(WaitGameToCompleteRemovePlayer(CheckLeftPlayer, i));
                if (pokerPlayers[i].isTurn)
                {
                    ChangePlayerTurn(pokerPlayers[i].playerNo);
                }
            }
        }

        /*for (int i = 0; i < pokerPlayers.Count; i++)
        {
            if (pokerPlayers[i].gameObject.activeSelf == true)
            {
                string playerIdGet = pokerPlayers[i].playerId;

                bool isEnter = false;
                for (int j = 0; j < DataManager.Instance.joinPlayerDatas.Count; j++)
                {
                    if (playerIdGet.Equals(DataManager.Instance.joinPlayerDatas[j].userId))
                    {

                        isEnter = true;
                    }
                }

                if (isEnter == false)
                {
                    pokerPlayers[i].isFold = true;
                    pokerPlayers[i].foldImg.SetActive(true);
                    //pokerPlayers[i].gameObject.SetActive(false);
                }
            }

        }*/

        if (DataManager.Instance.joinPlayerDatas.Count == 1)
        {
            WinPoker();
        }


    }
}


