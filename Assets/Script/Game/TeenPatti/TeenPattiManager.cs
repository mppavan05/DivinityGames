using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

[System.Serializable]
public class TeenPattiWinMaintain
{
    public int ruleNo;
    public List<CardSuffle> winList = new List<CardSuffle>();
}
public class TeenPattiManager : MonoBehaviour
{
    
    public static TeenPattiManager Instance;

    public GameObject waitNextRoundScreenObj;
    public GameObject playerFindScreenObj;

    [Header("---Game Play---")] 
    public int gameDealerNo;
    public bool isWin = false;
    public List<CardSuffle> cardSuffles = new List<CardSuffle>();
    public List<ListStoreData> listStoreDatas = new List<ListStoreData>();
    public List<int> mainList = new List<int>();
    public List<CardSuffle> cardSufflesGen = new List<CardSuffle>();
    public List<CardSuffle> cardSufflesSort = new List<CardSuffle>();

    public List<CardSuffle> newCardSS = new List<CardSuffle>();
    public List<CardSuffle> newCardSS1 = new List<CardSuffle>();


    public List<TeenPattiPlayer> teenPattiPlayers = new List<TeenPattiPlayer>();
    public List<TeenPattiPlayer> playerSquList = new List<TeenPattiPlayer>();

    public TeenPattiPlayer player1;
    public TeenPattiPlayer player2;
    public TeenPattiPlayer player3;
    public TeenPattiPlayer player4;
    public TeenPattiPlayer player5;

    public Sprite packCardSprite;
    public Sprite simpleCardSprite;
    public float[] chipPrice;
    public Sprite[] chipsSprite;
    public GameObject chipObj;
    public Transform[] playerPosition; 
    public List<GameObject> spawnedCoins = new List<GameObject>();
    public BoxCollider2D boxCollider;
    public float minBoardX;
    public float maxBoardX;
    public float minBoardY;
    public float maxBoardY;


    [Header("--- Menu Screen ---")]
    public GameObject menuScreenObj;
    public GameObject settingsScreenObj;

    [Header("--- Rule Screen ---")]
    public GameObject ruleScreenObj;

    [Header("--- Open Message Screen ---")]
    public GameObject messageScreeObj;
    public GameObject giftScreenObj;

    public GameObject soundOn;
    public GameObject soundOff;
    public GameObject vibOn;
    public GameObject vibOff;
    public GameObject sfxOn;
    public GameObject sfxOff;
    

    [Header("--- Prefab ---")]
    public GameObject targetBetObj;
    public GameObject betPrefab;
    public GameObject cardTmpPrefab;
    public GameObject prefabParent;
    public GameObject cardTmpStart;
    public GameObject shopPrefab;
    public GameObject shopPrefabParent;


    [Header("--- Chat Panel ---")]
    public GameObject chatPanelParent;
    public GameObject chatMePrefab;
    public GameObject chatOtherPrefab;

    [Header("--- Game UI ---")]
    public GameObject errorScreenObj;
    public GameObject slideShowPanel;
    public Text totalPriceTxt;
    public Button showButton;
    public GameObject bottomBox;
    public GameObject rulesTab;
    public Text rulesText;
    public Text betAmountTxt;
    public Text priceBtnTxt;
    public Button plusBtn;
    public Button minusBtn;
    public bool isAdmin;
    public int playerNo;
    public int currentPlayer;
    public float timerSpeed;
    public bool isGameStop;
    private int[] numbers = { 5, 10, 50, 100, 250, 500, 1000, 5000 };
    public int currentPriceIndex = 0;
    public int runningPriceIndex;
    public Image sideShowPopupImage;
    public float delay;
    private bool isPopupOpen = false;
    public GameObject exitPanel;
    public GameObject entryPopup;
    public Text winAnimationTxt;


    [Header("--- Gift Maintain ---")]
    public GameObject giftParentObj;
    public GameObject giftPrefab;
    public List<GiftBox> giftBoxes = new List<GiftBox>();

    [Header("--- Game Data Maintain ---")]
    public float totalBetAmount;
    public float playerBetAmount;
    public float bootValue;
    public float potLimitValue;
    public float minLimitValue;
    public float maxLimitValue;
    public float incrementNo;
    public float bonusUseValue;
    float minChaalValue;
    float maxChaalValue;
    float minBlindValue;
    float maxBlindValue;
    public float currentPriceValue;

    public TeenPattiPlayer slideShowPlayer;
    public List<TeenPattiWinMaintain> winMaintain = new List<TeenPattiWinMaintain>();
    public bool isGameStarted;

    public bool isBotActivate;
    public int roundCounter;
    public int boxDisplayCount;
    
    
    

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        //ShowPopUp();
        SoundManager.Instance.StopBackgroundMusic();
        roundCounter = 0;
        boxDisplayCount = 0;
        rulesTab.SetActive(false);

        //Invoke(nameof(CheckWin), 15f);
        //StartGamePlay();
        for (int i = 0; i < teenPattiPlayers.Count; i++)
        {
            teenPattiPlayers[i].gameObject.SetActive(false);
        }
        playerFindScreenObj.SetActive(true);
        DisplayCurrentBalance();
        PlayerFound();
        ManageSoundButtons();

    }

    private void ShowPopUp()
    {
        entryPopup.gameObject.SetActive(true);
        Time.timeScale = 0;
    }

    public void CloseEntryPopup()
    {
        entryPopup.gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    public void PlayerFound()
    {

        if (DataManager.Instance.joinPlayerDatas.Count >= TestSocketIO.Instace.teenPattiRequirePlayer)
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

    public void DisplayCurrentBalance()
    {
        totalPriceTxt.text = DataManager.Instance.playerData.balance.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        playerNo = player1.playerNo;
        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            // MatchResult();
            BetAnim(teenPattiPlayers[UnityEngine.Random.Range(0, 3)], 0.5f);
        }*/
    }

    #region GamePlay Manager

    public TeenPattiWinMaintain MatchResult(CardSuffle card1, CardSuffle card2, CardSuffle card3)
    {
        TeenPattiWinMaintain teenPattiWinMaintain = new TeenPattiWinMaintain();

        List<CardSuffle> newData = new List<CardSuffle>();
        newData.Add(card1);
        newData.Add(card2);
        newData.Add(card3);

        newCardSS = newData;


        newCardSS1 = NewSort(newData);


        bool isColor = IsColorMatch(newCardSS1);
        List<CardSuffle> threeCards = GetThreeCard(newCardSS1);
        List<CardSuffle> twoCards = GetTwoCard(newCardSS1);
        List<CardSuffle> ronCards = RonValue(newCardSS1);
        List<CardSuffle> highCards = HighCard(newCardSS1);



        //ronCard

        if (threeCards.Count == 3)
        {
            //Three List
            teenPattiWinMaintain.ruleNo = 1;
            teenPattiWinMaintain.winList = threeCards;
        }
        else if (isColor && ronCards.Count == 3)
        {
            teenPattiWinMaintain.ruleNo = 2;
            teenPattiWinMaintain.winList = ronCards;
        }
        else if (ronCards.Count == 3)
        {
            //ron List
            teenPattiWinMaintain.ruleNo = 3;
            teenPattiWinMaintain.winList = ronCards;
        }
        else if (isColor)
        {
            //High Card
            teenPattiWinMaintain.ruleNo = 4;
            teenPattiWinMaintain.winList = highCards;
        }
        else if (twoCards.Count == 3)
        {
            //Two Cards
            teenPattiWinMaintain.ruleNo = 5;
            teenPattiWinMaintain.winList = twoCards;
        }
        else if (highCards.Count == 3)
        {
            //High Cards
            teenPattiWinMaintain.ruleNo = 6;
            teenPattiWinMaintain.winList = highCards;
        }


        return teenPattiWinMaintain;
        //GetTwoCard(newCardSS1);
    }

    List<CardSuffle> GetTwoCard(List<CardSuffle> cards)
    {
        List<CardSuffle> twoCardSuffle = new List<CardSuffle>();
        int cnt1 = 0;
        int cnt2 = 0;
        int startNo = cards[0].cardNo;
        int endNo = cards[1].cardNo;
        print("Card Satrt No : " + cards.Count);
        print("Card End No : " + endNo);
        for (int i = 0; i < cards.Count; i++)
        {


            if (cards[i].cardNo == startNo)
            {
                cnt1++;
            }
            else if (cards[i].cardNo == endNo)
            {
                cnt2++;
            }
        }
        print("card cnt 1 : " + cnt1);
        print("card cnt 2 : " + cnt2);
        if (cnt1 == 2)
        {
            int noEnter = -1;
            for (int i = 0; i < cards.Count; i++)
            {
                print("Card No : " + cards[i].cardNo);
                if (cards[i].cardNo == startNo)
                {
                    print("Enter Card");
                    twoCardSuffle.Add(cards[i]);
                }
                else
                {
                    noEnter = i;
                }
            }
            //print("No Enter : " + noEnter);

            twoCardSuffle.Add(cards[noEnter]);

        }
        else if (cnt2 == 2)
        {
            int noEnter = -1;
            for (int i = 0; i < cards.Count; i++)
            {
                if (cards[i].cardNo == endNo)
                {
                    twoCardSuffle.Add(cards[i]);
                }
                else
                {
                    noEnter = i;
                }
            }
            twoCardSuffle.Add(cards[noEnter]);
        }


        print("Enter twoCard Suffle Count : " + twoCardSuffle.Count);
        return twoCardSuffle;

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

        if (cnt1 >= 3 || cnt2 >= 3 || cnt3 >= 3 || cnt4 >= 3)
        {
            return true;
        }

        return false;
    }

    List<CardSuffle> RonValue(List<CardSuffle> cards)
    {


        List<CardSuffle> ronvalue = new List<CardSuffle>();
        bool isRon = false;
        if (cards[1].cardNo == cards[0].cardNo + 1 && cards[2].cardNo == cards[0].cardNo + 2)
        {
            isRon = true;
        }
        else if (cards[0].cardNo == 2 && cards[1].cardNo == 3 && cards[2].cardNo == 14)
        {
            isRon = true;
        }

        if (isRon)
        {
            ronvalue = cards;
        }



        print("is Ron : " + isRon);
        return ronvalue;
    }

    List<CardSuffle> GetThreeCard(List<CardSuffle> cards)
    {
        List<CardSuffle> threeCardSuffle = new List<CardSuffle>();
        int cnt = 0;
        int startNo = cards[0].cardNo;

        for (int i = 0; i < cards.Count; i++)
        {
            if (cards[i].cardNo == startNo)
            {
                cnt++;
            }
        }

        if (cnt == 3)
        {
            threeCardSuffle = cards;
        }

        for (int i = 0; i < threeCardSuffle.Count; i++)
        {
            print(i + "-" + threeCardSuffle[i].cardNo);
        }

        return threeCardSuffle;

    }

    List<CardSuffle> HighCard(List<CardSuffle> cards)
    {
        print("high cards count : " + cards.Count);
        List<CardSuffle> highCards = new List<CardSuffle>();
        for (int i = cards.Count - 1; i >= 0; i--)
        {
            highCards.Add(cards[i]);
        }

        return highCards;
    }

    List<CardSuffle> NewSort(List<CardSuffle> cards)
    {
        List<CardSuffle> newCards = new List<CardSuffle>();
        //newCards = cards;
        for (int i = 0; i < cardSufflesSort.Count; i++)
        {
            for (int j = 0; j < cards.Count; j++)
            {
                if (cardSufflesSort[i].cardNo == cards[j].cardNo && cardSufflesSort[i].color == cards[j].color)
                {
                    CardSuffle c = new CardSuffle();
                    c.cardNo = cardSufflesSort[i].cardNo;
                    c.color = cardSufflesSort[i].color;
                    c.cardSprite = cardSufflesSort[i].cardSprite;
                    //newCards.Add(cardSufflesSort[i]);
                    newCards.Add(c);
                    break;
                }
            }
        }
        print("new cards Count : " + newCards.Count);
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

    #endregion

    public void EnableSeeCards()
    {
        boxDisplayCount++;
        if (boxDisplayCount != 4) return;
        SeeButtonClick();
    }
    
    #region GamePlay Button And Other Manage

    public void SeeButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        for (int i = 0; i < player1.seeObj.Length; i++)
        {
            player1.seeObj[i].SetActive(false);
        }

        player1.CardDisplay();
        DisplayRules();

        currentPriceValue = minLimitValue * 2;
        player1.isSeen = true;
        player1.isBlind = false;
        player1.isPack = false;
        currentPriceIndex = 1;
        priceBtnTxt.text = "Chaal\n" + currentPriceValue;
        ChangeCardStatus("SEEN", player1.playerNo);
    }

    public void GiftButtonClick(TeenPattiPlayer giftPlayer)
    {
        SoundManager.Instance.ButtonClick();
        giftScreenObj.SetActive(true);
        GiftSendManager.Instance.gameName = "TeenPatti";
        GiftSendManager.Instance.teenPattiOtherPlayer = giftPlayer;
    }

    public IEnumerator RestartGamePlay()
    {
        isGameStarted = false;
        DeleteAllCoins();
        yield return new WaitForSeconds(6f);

        //print("Enther The Generate Player");
        if (isAdmin)
        {
            CheckNewPlayers();
            StartGamePlay();
            //SetRoomData();
            //TestSocketIO.Instace.SetGameId(DataManager.Instance.tournamentID);
            print("Enther The Generate Player1");
            //isBotActivate = true;

        }
    }


    public void StartGamePlay()
    {
        //StartCoroutine(RestartGamePlay());
        if (isAdmin)
        {
            SetRoomData();
            TestSocketIO.Instace.SetGameId(DataManager.Instance.tournamentID);
        }
        SoundManager.Instance.CasinoTurnSound();
        isGameStop = true;
        waitNextRoundScreenObj.SetActive(false);
        playerFindScreenObj.SetActive(false);
        bootValue = 1f;
        potLimitValue = 30f;
        minLimitValue = 5f;
        maxLimitValue = 1000f;
        incrementNo = 5f;
        minChaalValue = minLimitValue * 2;
        maxChaalValue = maxLimitValue * 2;
        minBlindValue = minLimitValue;
        maxBlindValue = maxLimitValue;
        

        for (int i = 0; i < teenPattiPlayers.Count; i++)
        {
            teenPattiPlayers[i].isSeen = false;
            teenPattiPlayers[i].isPack = false;
            teenPattiPlayers[i].isBlind = true;
            teenPattiPlayers[i].userTurnCount = 0;
            teenPattiPlayers[i].SetActiveTrue();
            teenPattiPlayers[i].inactiveCount = 0;
        }

        currentPriceValue = minLimitValue;
        priceBtnTxt.text = "Blind\n" + currentPriceValue;
        minusBtn.interactable = false;
        rulesTab.SetActive(false);
        roundCounter = 0;
        totalBetAmount = 0;
        boxDisplayCount = 0;
        runningPriceIndex = 0;
        currentPriceIndex = 0;
        //StartBet();//Greejesh
        betAmountTxt.text = totalBetAmount.ToString();
        for (int i = 0; i < teenPattiPlayers.Count; i++)
        {
            TeenPattiPlayer currentPlayer = teenPattiPlayers[i];
            for (int j = 0; j < currentPlayer.playerWinObj.Length; j++)
            {
                currentPlayer.playerWinObj[j].SetActive(false);
            }
            currentPlayer.packImg.SetActive(false);
            currentPlayer.seenImg.SetActive(false);
            currentPlayer.cardImg1.gameObject.SetActive(false);
            currentPlayer.cardImg2.gameObject.SetActive(false);
            currentPlayer.cardImg3.gameObject.SetActive(false);
            currentPlayer.delearObj.SetActive(false);

            for (int j = 0; j < currentPlayer.seeObj.Length; j++)
            {
                currentPlayer.seeObj[j].SetActive(false);
            }
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
                    player1.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                    player1.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                    player1.playerNo = (i + 1);
                    player1.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                    player1.UpdateAvatar();
                }
                else
                {
                    player2.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
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
                            player3.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                            player3.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                            player3.playerNo = (i + 1);
                            playerSquList.Add(player3);
                            player3.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                            player3.UpdateAvatar();
                            playerSquList.Add(player1);

                            cntPlayer++;
                        }
                        else if (cntPlayer == 1)
                        {
                            player2.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
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
                    player1.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                    player1.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                    player1.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                    player1.UpdateAvatar();
                    player1.playerNo = (i + 1);
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
                            player2.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                            player2.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                            player2.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                            player2.playerNo = (i + 1);
                            player2.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                            player2.UpdateAvatar();
                            playerSquList.Add(player2);
                            //playerSquList.Add(player1);
                            cntPlayer++;
                        }
                        else if (cntPlayer == 2)
                        {
                            player3.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
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
                            player5.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                            player5.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                            player5.playerNo = (i + 1);
                            player5.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                            player5.UpdateAvatar();
                            playerSquList.Add(player5);
                            //playerSquList.Add(player1);
                            cntPlayer++;
                        }
                        else if (cntPlayer == 2)
                        {
                            player2.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
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
                            player4.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                            player4.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                            player4.playerNo = (i + 1);
                            player4.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                            player4.UpdateAvatar();
                            playerSquList.Add(player4);
                            //playerSquList.Add(player1);
                            cntPlayer++;
                        }
                        else if (cntPlayer == 2)
                        {
                            player5.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                            player5.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                            player5.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                            player5.playerNo = (i + 1);
                            player5.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                            player5.UpdateAvatar();
                            playerSquList.Add(player5);
                            cntPlayer++;
                          
                        }
                        else if (cntPlayer == 3)
                        {
                            player2.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
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
                            player3.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                            player3.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                            player3.playerNo = (i + 1);
                            player3.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                            player3.UpdateAvatar();
                            playerSquList.Add(player3);
                           // playerSquList.Add(player1);
                            cntPlayer++;
                        }
                        else if (cntPlayer == 2)
                        {
                            player4.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
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
        }
        
        int playerSend = DataManager.Instance.joinPlayerDatas.Count;

        float speed = 0.2f;

        // first card animation
        for (int i = 0; i < teenPattiPlayers.Count; i++)
        {
            //if(teenPattiPlayers[i].)
            //while (p < playerSend)
            //{
            if (i < playerSend)
            {
                GameObject obj = Instantiate(cardTmpPrefab, prefabParent.transform);
                SoundManager.Instance.CasinoCardMoveSound();
                obj.transform.position = cardTmpStart.transform.position;

                obj.transform.DOMove(teenPattiPlayers[i].cardImg1.transform.position, speed).OnComplete(() =>
                {
                    Destroy(obj);
                    teenPattiPlayers[i].cardImg1.gameObject.SetActive(true);
                });

                yield return new WaitForSeconds(speed);
            }

        }
        yield return new WaitForSeconds(speed);
        // second card animation
        for (int i = 0; i < teenPattiPlayers.Count; i++)
        {
            //if(teenPattiPlayers[i].)
            //while (p < playerSend)
            //{
            if (i < playerSend)
            {
                GameObject obj = Instantiate(cardTmpPrefab, prefabParent.transform);
                SoundManager.Instance.CasinoCardMoveSound();
                obj.transform.position = cardTmpStart.transform.position;


                obj.transform.DOMove(teenPattiPlayers[i].cardImg2.transform.position, speed).OnComplete(() =>
                {
                    Destroy(obj);
                    teenPattiPlayers[i].cardImg2.gameObject.SetActive(true);
                });
                yield return new WaitForSeconds(speed);
            }

        }
        yield return new WaitForSeconds(speed);
        //Third card animation
        for (int i = 0; i < teenPattiPlayers.Count; i++)
        {
            //if(teenPattiPlayers[i].)
            //while (p < playerSend)
            //{
            if (i < playerSend)
            {
                GameObject obj = Instantiate(cardTmpPrefab, prefabParent.transform);
                SoundManager.Instance.CasinoCardMoveSound();
                obj.transform.position = cardTmpStart.transform.position;


                obj.transform.DOMove(teenPattiPlayers[i].cardImg3.transform.position, speed).OnComplete(() =>
                {
                    Destroy(obj);
                    teenPattiPlayers[i].cardImg3.gameObject.SetActive(true);
                });
                yield return new WaitForSeconds(speed);
            }

        }
        
        yield return new WaitForSeconds(speed);
        for (int i = 0; i < player1.seeObj.Length; i++)
        {
            player1.seeObj[i].SetActive(true);
        }
        

        for (int i = 0; i < playerSquList.Count; i++)
        {
            if (playerSquList[i].playerNo == gameDealerNo)
            {
                playerSquList[i].RestartFillLine();
                playerSquList[i].delearObj.SetActive(true);
                if (playerSquList[i].playerNo == player1.playerNo)
                {
                    ShowTextChange();
                    bottomBox.SetActive(true);
                    DataManager.Instance.UserTurnVibrate();
                    EnableSeeCards();
                }
                else
                {
                    bottomBox.SetActive(false);
                }
            }
            else
            {
                playerSquList[i].delearObj.SetActive(false);
                playerSquList[i].NotATurn();
            }
        }
        
        isGameStop = false;

        for (int i = 0; i < playerSquList.Count; i++)
        {
            if (playerSquList[i].gameObject.activeSelf == true)
            {
                playerSquList[i].CardGenerate();
            }
        }
       
        ActivateBotPlayers();
        isBotActivate = true;
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

    #region Panel Button
    
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

    public void MessageButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        messageScreeObj.SetActive(true);
    }


    public void PackButtonClick()
    {
        if (!isGameStop)
        {
            SoundManager.Instance.ButtonClick();
            ChangeCardStatus("PACK", player1.playerNo);
            bottomBox.SetActive(false);
        }
    }

    public void LobbyButtonClick()
    {
        menuScreenObj.SetActive(false);
        exitPanel.gameObject.SetActive(true);
        Time.timeScale = 0;
    }

    public void CloseExitPopup()
    {
        exitPanel.gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    public void ShowButtonClick(Text t)
    {
        if (isGameStop) return;
        SoundManager.Instance.ButtonClick();
        //BetAnim(player1, currentPriceValue);
            

        switch (t.text)
        {
            case "Show":
                ShowCardToAllUser();
                CheckFinalWinner("Show");
                //SendTeenPattiBet(player1.playerNo, 0, "Show", "", "");
                //ChangePlayerTurn(player1.playerNo);
                break;
            case "Side\nShow":
                if (CheckMoney(currentPriceValue) == false)
                {
                    SoundManager.Instance.ButtonClick();
                    OpenErrorScreen();
                    return;
                }
                // BetAnim(player1, currentPriceValue);
                SendTeenPattiBet(player1.playerNo, currentPriceValue, "SideShow", slideShowPlayer.playerId, player1.playerId);
                //DataManager.Instance.DebitAmount((currentPriceValue).ToString(), DataManager.Instance.gameId, "TeenPatti-Bet-" + DataManager.Instance.gameId, "game", 1);
                //playerBetAmount += currentPriceValue;
                //Game Stop and check the card and one card is pack
                /*showButton.interactable = false;
                Invoke(nameof(OnPopupButtonClick), delay);*/// for making popup
                break;
        }
    }
    
    private void OnPopupButtonClick()
    {
        if (isPopupOpen) return;
        isPopupOpen = true;
        sideShowPopupImage.gameObject.SetActive(true);
        //sideShowPopupImage.transform.localScale = Vector3.zero;
        /*sideShowPopupImage.transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack).OnComplete(() =>
        {
        });*/
        StartCoroutine(WaitForPopupClose());
    }
    
    private IEnumerator WaitForPopupClose()
    {
        yield return new WaitUntil(() => !isPopupOpen);
        showButton.interactable = true;
    }
    
    public void ClosePopup()
    {
        if (!isPopupOpen) return;
        isPopupOpen = false;
        sideShowPopupImage.transform.DOScale(0f, 0.3f).SetEase(Ease.InBack).OnComplete(() =>
        {
            sideShowPopupImage.gameObject.SetActive(false);
        });
    }

    public void MinusButtonClick()
    {
        if (isGameStop) return;
        /*SoundManager.Instance.ButtonClick();
            minusBtn.interactable = false;
            plusBtn.interactable = true;

            currentPriceValue /= 2;
            if (!player1.isPack && player1.isBlind)
            {

                priceBtnTxt.text = "Blind\n" + currentPriceValue;
            }
            else if (!player1.isPack && player1.isSeen)
            {

                priceBtnTxt.text = "Chaal\n" + currentPriceValue;
            }*/
        SoundManager.Instance.ButtonClick();
        // minusBtn.interactable = false;
        // plusBtn.interactable = true;
        if (currentPriceIndex > 0 && currentPriceIndex > runningPriceIndex)
        {
            currentPriceIndex--;
            currentPriceValue = numbers[currentPriceIndex];
            //PriceTxt.text = price.ToString();
            if (currentPriceIndex == numbers.Length - 1)
            {
                plusBtn.interactable = false;
                minusBtn.interactable = true;
            }
            else if (currentPriceIndex == 0)
            {
                plusBtn.interactable = true;
                minusBtn.interactable = false;
            }
            else
            {
                plusBtn.interactable = true;
                minusBtn.interactable = true;
            }
        }
        else
        {
            plusBtn.interactable = true;
            minusBtn.interactable = false;
        }

        priceBtnTxt.text = player1.isPack switch
        {
            //currentPriceValue /= 2;
            false when player1.isBlind => "Blind\n" + currentPriceValue,
            false when player1.isSeen => "Chaal\n" + currentPriceValue,
            _ => priceBtnTxt.text
        };
    }

    public void PlusButtonClick()
    {
        if (isGameStop) return;
        /*SoundManager.Instance.ButtonClick();
            plusBtn.interactable = false;
            minusBtn.interactable = true;

            currentPriceValue *= 2;
            if (!player1.isPack && player1.isBlind)
            {

                priceBtnTxt.text = "Blind\n" + currentPriceValue;
            }
            else if (!player1.isPack && player1.isSeen)
            {

                priceBtnTxt.text = "Chaal\n" + currentPriceValue;
            }*/
        SoundManager.Instance.ButtonClick();
        // plusBtn.interactable = false;
        // minusBtn.interactable = true;
            
        if (currentPriceIndex < numbers.Length - 1)
        {
            currentPriceIndex++;
            currentPriceValue = numbers[currentPriceIndex];
            //PriceTxt.text = price.ToString();
            if (currentPriceIndex == numbers.Length - 1)
            {
                plusBtn.interactable = false;
                minusBtn.interactable = true;
            }
            else if (currentPriceIndex == 0)
            {
                plusBtn.interactable = true;
                minusBtn.interactable = false;
            }
            else
            {
                plusBtn.interactable = true;
                minusBtn.interactable = true;
            }
        }

        priceBtnTxt.text = player1.isPack switch
        {
            //currentPriceValue *= 2;
            false when player1.isBlind => "Blind\n" + currentPriceValue,
            false when player1.isSeen => "Chaal\n" + currentPriceValue,
            _ => priceBtnTxt.text
        };
    }

    public void StartBet()
    {
        if (CheckMoney(currentPriceValue) == false)
        {
            SoundManager.Instance.ButtonClick();
            OpenErrorScreen();
            return;
        }
        SoundManager.Instance.ThreeBetSound();
        BetAnim(player1, currentPriceValue, currentPriceIndex);
        DataManager.Instance.DebitAmount((currentPriceValue).ToString(), DataManager.Instance.gameId, "TeenPatti-Bet-" + DataManager.Instance.gameId, "game", 2);
        playerBetAmount += currentPriceValue;
    }

    public void BetButtonClick()
    {
        if (!isGameStop)
        {
            if (CheckMoney(currentPriceValue) == false)
            {
                SoundManager.Instance.ThreeBetSound();
                OpenErrorScreen();
                return;
            }
            SoundManager.Instance.ThreeBetSound();
            BetAnim(player1, currentPriceValue, currentPriceIndex);
            DataManager.Instance.DebitAmount((currentPriceValue).ToString(), DataManager.Instance.gameId, "TeenPatti-Bet-" + DataManager.Instance.gameId, "game", 3);
            playerBetAmount += currentPriceValue;

            // bonusUseValue
            // User Maintain
            runningPriceIndex = currentPriceIndex;
            SendTeenPattiBet(player1.playerNo, currentPriceValue, "Bet", "", "");
            ChangePlayerTurn(player1.playerNo);
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

    #endregion

    #region Menu Panel

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
            Time.timeScale = 1;
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

    #region Rule Panel

    void OpenRuleScreen()
    {
        ruleScreenObj.SetActive(true);
    }

    public void CloseRuleButton()
    {
        SoundManager.Instance.ButtonClick();
        ruleScreenObj.SetActive(false);
    }

    #endregion

    #region Bet Maintain

    public void BetAnim(TeenPattiPlayer player, float amount, int priceIndex)
    {
        /*GameObject genBetObj = Instantiate(betPrefab, prefabParent.transform);
        genBetObj.transform.GetChild(1).GetComponent<Text>().text = amount.ToString();
        genBetObj.transform.position = player.sendBetObj.transform.position;
        genBetObj.transform.DOMove(targetBetObj.transform.position, 0.3f).OnComplete(() =>
        {
            Destroy(genBetObj);
            totalBetAmount += amount;
            betAmountTxt.text = totalBetAmount.ToString();
        });*/
        totalBetAmount += amount;
        betAmountTxt.text = totalBetAmount.ToString();
        
        SpawnCoin(priceIndex);
    }
    
    public void GetBotBetNo(int num, int botPlayerNo)
    {
        if (isAdmin) return;
        switch (roundCounter)
        {
            case <= 1:
                switch (num)
                {
                    case 1:
                    {
                        int index = playerSquList.FindIndex(playerSqu => playerSqu.playerNo == botPlayerNo);
                        //ChangeCardStatus("SEEN", botPlayerNo);
                        if(playerSquList[index].isPack) return;
                        BetAnim(playerSquList[index], currentPriceValue, currentPriceIndex);
                        SoundManager.Instance.ThreeBetSound();
                        break;
                    }
                    case 2:
                    {
                        int index = playerSquList.FindIndex(playerSqu => playerSqu.playerNo == botPlayerNo);
                        //ChangeCardStatus("SEEN", botPlayerNo);
                        if(playerSquList[index].isPack) return;
                        BetAnim(playerSquList[index], currentPriceValue, currentPriceIndex);
                        SoundManager.Instance.ThreeBetSound();
                        break;
                    }
                    case 3:
                    {
                        int index = playerSquList.FindIndex(playerSqu => playerSqu.playerNo == botPlayerNo);
                        //ChangeCardStatus("SEEN", botPlayerNo);
                        if(playerSquList[index].isPack) return;
                        BetAnim(playerSquList[index], currentPriceValue, currentPriceIndex);
                        SoundManager.Instance.ThreeBetSound();
                        break;
                    }
                    case 4:
                    {
                        int index = playerSquList.FindIndex(playerSqu => playerSqu.playerNo == botPlayerNo);
                        //ChangeCardStatus("SEEN", botPlayerNo);
                        if(playerSquList[index].isPack) return;
                        BetAnim(playerSquList[index], currentPriceValue, currentPriceIndex);
                        SoundManager.Instance.ThreeBetSound();
                        break;
                    }
                    case 5:
                    {
                        int index = playerSquList.FindIndex(playerSqu => playerSqu.playerNo == botPlayerNo);
                        //ChangeCardStatus("SEEN", botPlayerNo);
                        if(playerSquList[index].isPack) return;
                        BetAnim(playerSquList[index], currentPriceValue, currentPriceIndex);
                        SoundManager.Instance.ThreeBetSound();
                        break;
                    }
                }

                break;
            case 2:
                switch (num)
                {
                    case 1:
                    {
                        int index = playerSquList.FindIndex(playerSqu => playerSqu.playerNo == botPlayerNo);
                        //ChangeCardStatus("SEEN", botPlayerNo);
                        if(playerSquList[index].isPack) return;
                        BetAnim(playerSquList[index], currentPriceValue, currentPriceIndex);
                        SoundManager.Instance.ThreeBetSound();
                        break;
                    }
                    case 2:
                    {
                        int index = playerSquList.FindIndex(playerSqu => playerSqu.playerNo == botPlayerNo);
                        //ChangeCardStatus("SEEN", botPlayerNo);
                        if(playerSquList[index].isPack) return;
                        BetAnim(playerSquList[index], currentPriceValue, currentPriceIndex);
                        SoundManager.Instance.ThreeBetSound();
                        break;
                    }
                    case 3:
                    {
                        int index = playerSquList.FindIndex(playerSqu => playerSqu.playerNo == botPlayerNo);
                        //ChangeCardStatus("SEEN", botPlayerNo);
                        if(playerSquList[index].isPack) return;
                        BetAnim(playerSquList[index], currentPriceValue, currentPriceIndex);
                        SoundManager.Instance.ThreeBetSound();
                        break;
                    }
                    case 4:
                    {
                        int index = playerSquList.FindIndex(playerSqu => playerSqu.playerNo == botPlayerNo);
                        //ChangeCardStatus("SEEN", botPlayerNo);
                        if(playerSquList[index].isPack) return;
                        BetAnim(playerSquList[index], currentPriceValue, currentPriceIndex);
                        SoundManager.Instance.ThreeBetSound();
                        break;
                    }
                    case 5:
                    {
                        //ChangeCardStatus("SEEN", botPlayerNo);
                        if(playerSquList[botPlayerNo].isPack) return;
                        break;
                    }
                }

                break;
            case 3:
                switch (num)
                {
                    case 1:
                    {
                        //ChangeCardStatus("SEEN", botPlayerNo);
                        if(playerSquList[botPlayerNo].isPack) return;
                        break;
                    }
                    case 2:
                    {
                        int index = playerSquList.FindIndex(playerSqu => playerSqu.playerNo == botPlayerNo);
                        //ChangeCardStatus("SEEN", botPlayerNo);
                        if(playerSquList[index].isPack) return;
                        BetAnim(playerSquList[index], currentPriceValue, currentPriceIndex);
                        SoundManager.Instance.ThreeBetSound();
                        break;
                    }
                    case 3:
                    {
                        int index = playerSquList.FindIndex(playerSqu => playerSqu.playerNo == botPlayerNo);
                        //ChangeCardStatus("SEEN", botPlayerNo);
                        if(playerSquList[index].isPack) return;
                        BetAnim(playerSquList[index], currentPriceValue, currentPriceIndex);
                        SoundManager.Instance.ThreeBetSound();
                        break;
                    }
                    case 4:
                    {
                        int index = playerSquList.FindIndex(playerSqu => playerSqu.playerNo == botPlayerNo);
                        //ChangeCardStatus("SEEN", botPlayerNo);
                        if(playerSquList[index].isPack) return;
                        BetAnim(playerSquList[index], currentPriceValue, currentPriceIndex);
                        SoundManager.Instance.ThreeBetSound();
                        break;
                    }
                    case 5:
                    {
                        //ChangeCardStatus("SEEN", botPlayerNo);
                        if(playerSquList[botPlayerNo].isPack) return;
                        break;
                    }
                }

                break;
            case >= 4:
                switch (num)
                {
                    case 1:
                    {
                        int index = playerSquList.FindIndex(playerSqu => playerSqu.playerNo == botPlayerNo);
                        //ChangeCardStatus("SEEN", botPlayerNo);
                        if(playerSquList[index].isPack) return;
                        BetAnim(playerSquList[index], currentPriceValue, currentPriceIndex);
                        SoundManager.Instance.ThreeBetSound();
                        break;
                    }
                    case 2:
                    {
                        int index = playerSquList.FindIndex(playerSqu => playerSqu.playerNo == botPlayerNo);
                        //ChangeCardStatus("SEEN", botPlayerNo);
                        if(playerSquList[index].isPack) return;
                        BetAnim(playerSquList[index], currentPriceValue, currentPriceIndex);
                        SoundManager.Instance.ThreeBetSound();
                        break;
                    }
                    case 3:
                    {
                        //ChangeCardStatus("SEEN", botPlayerNo);
                        if(playerSquList[botPlayerNo].isPack) return;
                        break;
                    }
                    case 4:
                    {
                        int index = playerSquList.FindIndex(playerSqu => playerSqu.playerNo == botPlayerNo);
                        //ChangeCardStatus("SEEN", botPlayerNo);
                        if(playerSquList[index].isPack) return;
                        BetAnim(playerSquList[index], currentPriceValue, currentPriceIndex);
                        SoundManager.Instance.ThreeBetSound();
                        break;
                    }
                    case 5:
                    {
                        int index = playerSquList.FindIndex(playerSqu => playerSqu.playerNo == botPlayerNo);
                        //ChangeCardStatus("SEEN", botPlayerNo);
                        if(playerSquList[index].isPack) return;
                        BetAnim(playerSquList[index], currentPriceValue, currentPriceIndex);
                        SoundManager.Instance.ThreeBetSound();
                        break;
                    }
                }

                break;
        }
        
    }


    #endregion

    #region SlideShowPanel
    public void Accept_SlideShow(string sendId, string currentId)
    {
        SlideShowSendSocket( sendId, currentId,"Accept");
        StartCoroutine(CheckSlideShowWinner( sendId,currentId, false));
    }

    public void Cancel_SlideShow(string sendId, string currentId)
    {
        SlideShowSendSocket(  sendId,currentId,"Cancel");
    }
    
    private void SpawnCoin(int priceIndex)
    {
        //Instantiate(chipObj, boxCollider.transform);
        Vector3 dPos = GetRandomPosInBoxCollider2D();
        GameObject coin = Instantiate(chipObj, playerPosition[currentPlayer - 1]);
        coin.transform.GetComponent<Image>().sprite = chipsSprite[priceIndex];
        //coin.transform.position = new Vector3(targetBetObj.transform.position.x, targetBetObj.transform.position.y, 0f);
        ChipGenerate(coin, dPos);
        spawnedCoins.Add(coin);
        /*GameObject genBetObj = Instantiate(chipObj, playerPosition[currentPlayer - 1]);
        genBetObj.transform.GetComponent<Image>().sprite = chipsSprite[currentPriceIndex];
        genBetObj.transform.position = playerPosition[currentPlayer - 1].transform.position;
        genBetObj.transform.DOMove(targetBetObj.transform.position, 0.3f).OnComplete(() =>
        {
            spawnedCoins.Add(genBetObj);
        });*/
    }
    
    private Vector3 GetRandomPosInBoxCollider2D()
    {
        Bounds bounds = boxCollider.bounds;
        float x = UnityEngine.Random.Range(bounds.min.x, bounds.max.x);
        float y = UnityEngine.Random.Range(bounds.min.y, bounds.max.y);
        return new Vector3(x, y, 90f);
    }
    public void ChipGenerate(GameObject chip, Vector3 endPos)
    {
        chip.transform.DORotate(new Vector3(0, 0, UnityEngine.Random.Range(0, 360)), 0.2f);
        chip.transform.DOMove(endPos, 0.2f).OnComplete(() =>
        {
            chip.transform.DOScale(new Vector3(0.8f, 0.8f, 0.8f), 0.1f).OnComplete(() =>
            {
                chip.transform.DOScale(Vector3.one, 0.07f);
            });
        });
    }
    
    
    public void DeleteAllCoins()
    {
        foreach (GameObject coin in spawnedCoins)
        {
            Destroy(coin);
        }
        spawnedCoins.Clear();
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

    #region Show Maintain

    public void DisplayRules()
    {
        switch (player1.ruleNo)
        {
            case 1:
                rulesTab.SetActive(true);
                rulesText.text = "TRAIL";
                break;
            case 2:
                rulesTab.SetActive(true);
                rulesText.text = "PURE";
                break;
            case 3:
                rulesTab.SetActive(true);
                rulesText.text = "SEQUENCE";
                break;
            case 4:
                rulesTab.SetActive(true);
                rulesText.text = "COLOR";
                break;
            case 5:
                rulesTab.SetActive(true);
                rulesText.text = "PAIR";
                break;
            case 6:
                rulesTab.SetActive(true);
                rulesText.text = "HIGH";
                break;
        }
    }

    public void ShowTextChange()
    {
        /*List<TeenPattiPlayer> avaliablePlayer = new List<TeenPattiPlayer>();
        List<TeenPattiPlayer> withOutPlayerList = new List<TeenPattiPlayer>();
        for (int i = 0; i < playerSquList.Count; i++)
        {
            if (playerSquList[i].gameObject.activeSelf == true && playerSquList[i].isSeen == true && playerSquList[i].isPack == false && playerSquList[i].isBlind == false)
            {
                avaliablePlayer.Add(playerSquList[i]);
                if (playerSquList[i] != player1)
                {
                    withOutPlayerList.Add(playerSquList[i]);
                }
            }
        }
        int tCnt = 0;
        for (int i = 0; i < playerSquList.Count; i++)
        {
            if (playerSquList[i].gameObject.activeSelf == true && playerSquList[i].isPack == false)
            {
                tCnt++;
            }
        }
        if (avaliablePlayer.Count == 1)
        {
            //win
        }
        else if (avaliablePlayer.Count == 2 || tCnt == 2)
        {
            showButton.interactable = true;
            showButton.transform.GetChild(0).GetComponent<Text>().text = "Show";
        }
        else if (avaliablePlayer.Count > 2)
        {
            //int playerN = player1.playerNo;
            //for (int i = 0; i < teenPattiPlayers.Count; i++)
            //{
            //    if (teenPattiPlayers[i].gameObject.activeSelf == true && teenPattiPlayers[i].isSeen == true && teenPattiPlayers[i].isBlind == false && teenPattiPlayers[i].isPack == false && teenPattiPlayers[i] != player1)
            //    {
            //        int cPNo = teenPattiPlayers[i].playerNo;
            //        if (playerN == cPNo + 1)
            //        {

            //        }
            //    }
            //}

            slideShowPlayer = null;
            int currentPlayerNo = player1.playerNo;


            if (avaliablePlayer.Count == 3)
            {

                for (int j = 0; j < withOutPlayerList.Count; j++)
                {
                    int targetNo = withOutPlayerList[j].playerNo;

                    int checkNo = targetNo + 1;

                    if (slideShowPlayer == null)
                    {
                        if (currentPlayerNo == 1)
                        {
                            bool isSkip = player3.isBlind == true;

                            if (checkNo == 4 && isSkip == false)
                            {
                                showButton.interactable = true;
                                showButton.transform.GetChild(0).GetComponent<Text>().text = "Side\nShow";
                                slideShowPlayer = withOutPlayerList[j - 1];
                            }
                        }
                        else if (currentPlayerNo == 2)
                        {
                            bool isSkip = player1.isBlind == true;

                            if (checkNo == 2 && isSkip == false)
                            {
                                showButton.interactable = true;
                                showButton.transform.GetChild(0).GetComponent<Text>().text = "Side\nShow";
                                slideShowPlayer = withOutPlayerList[j - 1];
                            }
                        }
                        else if (currentPlayerNo == 3)
                        {
                            bool isSkip = player2.isBlind == true;

                            if (checkNo == 3 && isSkip == false)
                            {
                                showButton.interactable = true;
                                showButton.transform.GetChild(0).GetComponent<Text>().text = "Side\nShow";
                                slideShowPlayer = withOutPlayerList[j - 1];
                            }
                        }
                    }

                }
            }
            else if (avaliablePlayer.Count == 4)
            {
                for (int j = 0; j < withOutPlayerList.Count; j++)
                {
                    int targetNo = withOutPlayerList[j].playerNo;

                    int checkNo = targetNo + 1;

                    if (slideShowPlayer == null)
                    {
                        if (currentPlayerNo == 1)
                        {
                            bool isSkip = player4.isBlind == true;
                            if (checkNo == 5 && isSkip == false)
                            {
                                showButton.interactable = true;
                                showButton.transform.GetChild(0).GetComponent<Text>().text = "Side\nShow";
                                slideShowPlayer = withOutPlayerList[j - 1];
                            }
                            else if (checkNo == 4 && isSkip == false)
                            {
                                showButton.interactable = true;
                                showButton.transform.GetChild(0).GetComponent<Text>().text = "Side\nShow";
                                slideShowPlayer = withOutPlayerList[j - 1];
                            }
                        }
                        else if (currentPlayerNo == 2)
                        {
                            bool isSkip = player1.isBlind == true;
                            if (checkNo == 2 && isSkip == false)
                            {
                                showButton.interactable = true;
                                showButton.transform.GetChild(0).GetComponent<Text>().text = "Side\nShow";
                                slideShowPlayer = withOutPlayerList[j - 1];
                            }
                            else if (checkNo == 5 && isSkip == false)
                            {
                                showButton.interactable = true;
                                showButton.transform.GetChild(0).GetComponent<Text>().text = "Side\nShow";
                                slideShowPlayer = withOutPlayerList[j - 1];
                            }
                        }
                        else if (currentPlayerNo == 3)
                        {
                            bool isSkip = player2.isBlind == true;
                            if (checkNo == 2 && isSkip == false)
                            {
                                showButton.interactable = true;
                                showButton.transform.GetChild(0).GetComponent<Text>().text = "Side\nShow";
                                slideShowPlayer = withOutPlayerList[j - 1];
                            }
                            else if (checkNo == 3 && isSkip == false)
                            {
                                showButton.interactable = true;
                                showButton.transform.GetChild(0).GetComponent<Text>().text = "Side\nShow";
                                slideShowPlayer = withOutPlayerList[j - 1];
                            }
                        }
                        else if (currentPlayerNo == 4)
                        {
                            bool isSkip = player3.isBlind == true;
                            if (checkNo == 3 && isSkip == false)
                            {
                                showButton.interactable = true;
                                showButton.transform.GetChild(0).GetComponent<Text>().text = "Side\nShow";
                                slideShowPlayer = withOutPlayerList[j - 1];
                            }
                            else if (checkNo == 4 && isSkip == false)
                            {
                                showButton.interactable = true;
                                showButton.transform.GetChild(0).GetComponent<Text>().text = "Side\nShow";
                                slideShowPlayer = withOutPlayerList[j - 1];
                            }
                        }
                    }

                }
            }
            else if (avaliablePlayer.Count == 5)
            {
                for (int j = 0; j < withOutPlayerList.Count; j++)
                {
                    int targetNo = withOutPlayerList[j].playerNo;

                    int checkNo = targetNo + 1;

                    if (slideShowPlayer == null)
                    {
                        if (currentPlayerNo == 1)
                        {
                            bool isSkip = player5.isBlind == true;
                            if (checkNo == 6 && isSkip == false)
                            {
                                showButton.interactable = true;
                                showButton.transform.GetChild(0).GetComponent<Text>().text = "Side\nShow";
                                slideShowPlayer = withOutPlayerList[j - 1];
                            }
                            else if (checkNo == 5 && isSkip == false)
                            {
                                showButton.interactable = true;
                                showButton.transform.GetChild(0).GetComponent<Text>().text = "Side\nShow";
                                slideShowPlayer = withOutPlayerList[j - 1];
                            }
                            else if (checkNo == 4 && isSkip == false)
                            {
                                showButton.interactable = true;
                                showButton.transform.GetChild(0).GetComponent<Text>().text = "Side\nShow";
                                slideShowPlayer = withOutPlayerList[j - 1];
                            }
                        }
                        else if (currentPlayerNo == 2)
                        {
                            bool isSkip = player1.isBlind == true;
                            if (checkNo == 2 && isSkip == false)
                            {
                                showButton.interactable = true;
                                showButton.transform.GetChild(0).GetComponent<Text>().text = "Side\nShow";
                                slideShowPlayer = withOutPlayerList[j - 1];
                            }
                            else if (checkNo == 6 && isSkip == false)
                            {
                                showButton.interactable = true;
                                showButton.transform.GetChild(0).GetComponent<Text>().text = "Side\nShow";
                                slideShowPlayer = withOutPlayerList[j - 1];
                            }
                            else if (checkNo == 5 && isSkip == false)
                            {
                                showButton.interactable = true;
                                showButton.transform.GetChild(0).GetComponent<Text>().text = "Side\nShow";
                                slideShowPlayer = withOutPlayerList[j - 1];
                            }
                        }
                        else if (currentPlayerNo == 3)
                        {
                            bool isSkip = player2.isBlind == true;
                            if (checkNo == 3 && isSkip == false)
                            {
                                showButton.interactable = true;
                                showButton.transform.GetChild(0).GetComponent<Text>().text = "Side\nShow";
                                slideShowPlayer = withOutPlayerList[j - 1];
                            }
                            else if (checkNo == 2 && isSkip == false)
                            {
                                showButton.interactable = true;
                                showButton.transform.GetChild(0).GetComponent<Text>().text = "Side\nShow";
                                slideShowPlayer = withOutPlayerList[j - 1];
                            }
                            else if (checkNo == 6 && isSkip == false)
                            {
                                showButton.interactable = true;
                                showButton.transform.GetChild(0).GetComponent<Text>().text = "Side\nShow";
                                slideShowPlayer = withOutPlayerList[j - 1];
                            }
                        }
                        else if (currentPlayerNo == 4)
                        {
                            bool isSkip = player3.isBlind == true;
                            if (checkNo == 4 && isSkip == false)
                            {
                                showButton.interactable = true;
                                showButton.transform.GetChild(0).GetComponent<Text>().text = "Side\nShow";
                                slideShowPlayer = withOutPlayerList[j - 1];
                            }
                            else if (checkNo == 3 && isSkip == false)
                            {
                                showButton.interactable = true;
                                showButton.transform.GetChild(0).GetComponent<Text>().text = "Side\nShow";
                                slideShowPlayer = withOutPlayerList[j - 1];
                            }
                            else if (checkNo == 2 && isSkip == false)
                            {
                                showButton.interactable = true;
                                showButton.transform.GetChild(0).GetComponent<Text>().text = "Side\nShow";
                                slideShowPlayer = withOutPlayerList[j - 1];
                            }
                        }
                        else if (currentPlayerNo == 5)
                        {
                            bool isSkip = player4.isBlind == true;
                            if (checkNo == 5 && isSkip == false)
                            {
                                showButton.interactable = true;
                                showButton.transform.GetChild(0).GetComponent<Text>().text = "Side\nShow";
                                slideShowPlayer = withOutPlayerList[j - 1];
                            }
                            else if (checkNo == 4 && isSkip == false)
                            {
                                showButton.interactable = true;
                                showButton.transform.GetChild(0).GetComponent<Text>().text = "Side\nShow";
                                slideShowPlayer = withOutPlayerList[j - 1];
                            }
                            else if (checkNo == 3 && isSkip == false)
                            {
                                showButton.interactable = true;
                                showButton.transform.GetChild(0).GetComponent<Text>().text = "Side\nShow";
                                slideShowPlayer = withOutPlayerList[j - 1];
                            }
                        }
                    }

                }
            }

        }
        else
        {
            showButton.interactable = false;
            showButton.transform.GetChild(0).GetComponent<Text>().text = "Show";
        }*/
        
        switch (roundCounter)
        {
            case <= 2:
                showButton.interactable = false;
                showButton.transform.GetChild(0).GetComponent<Text>().text = "Side\nShow";
                break;
            case >= 3:
                showButton.interactable = true;
                showButton.transform.GetChild(0).GetComponent<Text>().text = "Side\nShow";
                break;
        }

        List<TeenPattiPlayer> availablePlayer = playerSquList.Where(t => t.gameObject.activeSelf && !t.isPack).ToList();
        // showing next player for side show
        var myIndex = availablePlayer.IndexOf(player1);
        var nextIndex = (myIndex + 1) % availablePlayer.Count;
        var nextPlayer = teenPattiPlayers[nextIndex];
        slideShowPlayer = nextPlayer;

        if (availablePlayer.Count >= 3) return;
        showButton.interactable = true;
        showButton.transform.GetChild(0).GetComponent<Text>().text = "Show";
    }


    #endregion

    #region Game Restart Round Maintain

    public void GameRestartRound()
    {

    }

    #endregion

    #region Socket Manager

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

        //print("Send Id : " + sendPlayerID);
        //print("Receive Id : " + receivePlayerId);
        for (int i = 0; i < teenPattiPlayers.Count; i++)
        {
            if (teenPattiPlayers[i].playerId == sendPlayerID)
            {
                sendPlayerObj = teenPattiPlayers[i].fillLine.gameObject;
            }
            else if (teenPattiPlayers[i].playerId == receivePlayerId)
            {
                receivePlayerObj = teenPattiPlayers[i].fillLine.gameObject;
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



        //if (playerID.Equals(DataManager.Instance.playerData._id))
        //{
        //    TypeMessageBox typeMessageBox = Instantiate(chatMePrefab, chatPanelParent.transform).GetComponent<TypeMessageBox>();
        //    typeMessageBox.Update_Message_Box(msg);
        //}
        //else
        //{
        //    TypeMessageBox typeMessageBox = Instantiate(chatOtherPrefab, chatPanelParent.transform).GetComponent<TypeMessageBox>();
        //    typeMessageBox.Update_Message_Box(msg);
        //}
        //Canvas.ForceUpdateCanvases();
    }


    void CreateAdmin()
    {
        if (DataManager.Instance.joinPlayerDatas[0].userId.Equals(DataManager.Instance.playerData._id))
        {
            isAdmin = true;
            //SetRoomData();
            //TestSocketIO.Instace.SetGameId(DataManager.Instance.tournamentID);
        }
        //Greejesh Set PlayerNo
        //int nameCnt = 1;
        //for (int i = 0; i < DataManager.Instance.joinPlayerDatas.Count; i++)
        //{
        //    if (DataManager.Instance.joinPlayerDatas[i].userId.Equals(DataManager.Instance.playerData._id))
        //    {
        //        playerNo = (i + 1);
        //        player1.playerNo = (i + 1);
        //    }
        //    else
        //    {
        //        if (nameCnt == 1)
        //        {
        //            player2.playerNo = (i + 1);
        //            nameCnt++;
        //        }
        //        else if (nameCnt == 2)
        //        {
        //            player3.playerNo = (i + 1);
        //            nameCnt++;
        //        }
        //        else if (nameCnt == 3)
        //        {
        //            player4.playerNo = (i + 1);
        //            nameCnt++;
        //        }
        //        else if (nameCnt == 4)
        //        {
        //            player5.playerNo = (i + 1);
        //            nameCnt++;
        //        }
        //    }
        //}
    }
    public void SetRoomData()
    {
        JSONObject obj = new JSONObject();
        int dealerNo = 0;
        
        dealerNo = UnityEngine.Random.Range(1, DataManager.Instance.joinPlayerDatas.Count + 1);
        
        obj.AddField("DeckNo", UnityEngine.Random.Range(0, 300));
        obj.AddField("PlayerID", DataManager.Instance.playerData._id);
        obj.AddField("DeckNo2", dealerNo);
        //obj.AddField("DeckNo", 154);
        obj.AddField("dateTime", DateTime.UtcNow.ToString());
        obj.AddField("gameMode", 1);
        TestSocketIO.Instace.SetRoomdata(TestSocketIO.Instace.roomid, obj);
    }

    public void GetRoomData(int deckNo, int dealerNo, string playerId)
    {
        //if (playerId != DataManager.Instance.playerData._id) return;
        //print("Deck no : " + deckNo);`
        mainList = listStoreDatas[deckNo].noList;
        gameDealerNo = dealerNo;
        currentPlayer = dealerNo;
        // changing card sprite to default
        foreach (var t in teenPattiPlayers)
        {
            t.CardGenerate();
        }

        if (isAdmin) return;
        if (waitNextRoundScreenObj.activeSelf)
        {
            waitNextRoundScreenObj.SetActive(false);
        }
        
        //MainMenuManager.Instance.CheckPlayers();

        StartGamePlay();
    }




    public void ChangePlayerTurn(int pNo)
    {

        JSONObject obj = new JSONObject();
        obj.AddField("PlayerID", DataManager.Instance.playerData._id);
        obj.AddField("TournamentID", DataManager.Instance.tournamentID);
        obj.AddField("RoomId", DataManager.Instance.gameId);
        obj.AddField("PlayerNo", pNo);
        obj.AddField("Action", "ChangePlayerTurn");
        TestSocketIO.Instace.Senddata("TeenPattiChangeTurnData", obj);
    }


    public void SlideShowSendSocket(string slideShowCancelPlayerID, string slideShowPlayerID, string type)
    {

        JSONObject obj = new JSONObject();
        obj.AddField("PlayerID", DataManager.Instance.playerData._id);
        obj.AddField("TournamentID", DataManager.Instance.tournamentID);
        obj.AddField("RoomId", DataManager.Instance.gameId);
        obj.AddField("SlideShowCancelPlayerId", slideShowCancelPlayerID);
        obj.AddField("SlideShowPlayerId", slideShowPlayerID);
        obj.AddField("SlideShowType", type);
        obj.AddField("Action", "SideShowRequest");
        TestSocketIO.Instace.Senddata("TeenPattiSlideShowData", obj);
    }


    public void SendTeenPattiBet(int pNo, float amount, string betType, string playerSlideShowSend, string playerIdSlideShow)
    {
        JSONObject obj = new JSONObject();
        obj.AddField("PlayerID", DataManager.Instance.playerData._id);
        obj.AddField("TournamentID", DataManager.Instance.tournamentID);
        obj.AddField("RoomId", DataManager.Instance.gameId);
        obj.AddField("PlayerNo", pNo);
        obj.AddField("BetAmount", amount);
        obj.AddField("BetType", betType);
        obj.AddField("playerSlideShowSendId", playerSlideShowSend);
        obj.AddField("playerIdSlideShowId", playerIdSlideShow);
        obj.AddField("Action", "PlaceBet");
        TestSocketIO.Instace.Senddata("TeenPattiSendBetData", obj);
    }


    public void SetTeenPattiWon(string winnerPlayerId)
    {
        JSONObject obj = new JSONObject();
        obj.AddField("PlayerID", DataManager.Instance.playerData._id);
        obj.AddField("TournamentID", DataManager.Instance.tournamentID);
        obj.AddField("RoomId", DataManager.Instance.gameId);
        obj.AddField("WinnerPlayerId", winnerPlayerId);
        //obj.AddField("WinnerList", value);
        obj.AddField("Action", "WinData");
        TestSocketIO.Instace.Senddata("TeenPattiWinnerData", obj);
    }

    public void ChangeCardStatus(string value, int pno)
    {
        JSONObject obj = new JSONObject();
        obj.AddField("PlayerID", DataManager.Instance.playerData._id);
        obj.AddField("TournamentID", DataManager.Instance.tournamentID);
        obj.AddField("RoomId", DataManager.Instance.gameId);
        obj.AddField("PlayerNo", pno);
        obj.AddField("CardStatus", value);
        obj.AddField("Action", "CardStatus");

        player1.isSeen = true;

        TestSocketIO.Instace.Senddata("TeenPattiChangeCardStatus", obj);
    }

    bool isCheckTurnPack(int nextPlayerNo)
    {
        for (int i = 0; i < playerSquList.Count; i++)
        {
            if (playerSquList[i].gameObject.activeSelf == true && playerSquList[i].playerNo == nextPlayerNo && playerSquList[i].isPack == true)
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
        //if (playerNo == DataManager.Instance.joinPlayerDatas.Count)//5
        if (playerNo == 5)
        {
            nextPlayerNo = 1;
            roundCounter++;
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
        //nextPlayerNo = playerNo;

        print("Next Player No : " + nextPlayerNo);
        currentPlayer = nextPlayerNo;
        for (int i = 0; i < playerSquList.Count; i++)
        {
            if (playerSquList[i].playerNo == nextPlayerNo)
            {
                playerSquList[i].RestartFillLine();
                if (playerSquList[i].playerNo == nextPlayerNo && playerSquList[i] == player1)
                {
                    ShowTextChange();
                    bottomBox.SetActive(true);
                    DataManager.Instance.UserTurnVibrate();
                    EnableSeeCards();
                }
                else
                {
                    bottomBox.SetActive(false);
                }

            }
            else
            {
                playerSquList[i].NotATurn();
            }
        }
    }

    //public void GetPlayerTurn(int playerNo)
    //{

    //    int nextPlayerNo = 0;
    //    if (playerNo == 1)
    //    {
    //        nextPlayerNo = 2;


    //    }
    //    else if (playerNo == 2)
    //    {
    //        if (DataManager.Instance.joinPlayerDatas.Count == 2)
    //        {
    //            nextPlayerNo = 1;
    //        }
    //        else
    //        {
    //            bool isTurnClose = false;
    //            for (int i = 0; i < teenPattiPlayers.Count; i++)
    //            {
    //                if (teenPattiPlayers[i].playerNo == 3 && teenPattiPlayers[i].isPack == true)
    //                {
    //                    isTurnClose = true;
    //                }
    //            }
    //            if (isTurnClose == false)
    //            {
    //                nextPlayerNo = 3;
    //            }
    //            else
    //            {
    //                nextPlayerNo = 1;
    //            }
    //        }
    //    }
    //    else if (playerNo == 3)
    //    {
    //        if (DataManager.Instance.joinPlayerDatas.Count == 3)
    //        {
    //            nextPlayerNo = 1;
    //        }
    //        else
    //        {
    //            bool isTurnClose = false;
    //            for (int i = 0; i < teenPattiPlayers.Count; i++)
    //            {
    //                if (teenPattiPlayers[i].playerNo == 4 && teenPattiPlayers[i].isPack == true)
    //                {
    //                    isTurnClose = true;
    //                }
    //            }
    //            if (isTurnClose == false)
    //            {
    //                nextPlayerNo = 4;
    //            }
    //            else
    //            {
    //                nextPlayerNo = 1;
    //            }
    //        }
    //    }
    //    else if (playerNo == 4)
    //    {
    //        if (DataManager.Instance.joinPlayerDatas.Count == 4)
    //        {
    //            nextPlayerNo = 1;
    //        }
    //        else
    //        {
    //            bool isTurnClose = false;
    //            for (int i = 0; i < teenPattiPlayers.Count; i++)
    //            {
    //                if (teenPattiPlayers[i].playerNo == 5 && teenPattiPlayers[i].isPack == true)
    //                {
    //                    isTurnClose = true;
    //                }
    //            }
    //            if (isTurnClose == false)
    //            {
    //                nextPlayerNo = 5;
    //            }
    //            else
    //            {
    //                nextPlayerNo = 1;
    //            }
    //        }
    //    }
    //    else if (playerNo == 5)
    //    {
    //        if (DataManager.Instance.joinPlayerDatas.Count == 5)
    //        {
    //            nextPlayerNo = 1;
    //        }
    //    }



    //    //nextPlayerNo = playerNo;
    //    for (int i = 0; i < teenPattiPlayers.Count; i++)
    //    {
    //        if (teenPattiPlayers[i].playerNo == nextPlayerNo)
    //        {
    //            teenPattiPlayers[i].RestartFillLine();
    //            if (teenPattiPlayers[i].playerNo == player1.playerNo)
    //            {
    //                bottomBox.SetActive(true);
    //            }
    //            else
    //            {
    //                bottomBox.SetActive(false);
    //            }
    //        }
    //        else
    //        {
    //            teenPattiPlayers[i].NotATurn();
    //        }
    //    }
    //}

    public void CreditWinnerAmount(string playerID)
    {
        float winnerAmount = (float)totalBetAmount;
        
        //print("Win No : " + winnerNo[i]);
        for (int j = 0; j < teenPattiPlayers.Count; j++)
        {
            if (teenPattiPlayers[j].playerId == playerID && teenPattiPlayers[j].gameObject.activeSelf == true)
            {

                //Generate Number
                GameObject genBetObj = Instantiate(betPrefab, prefabParent.transform);
                genBetObj.transform.GetChild(1).GetComponent<Text>().text = winnerAmount.ToString();
                genBetObj.transform.position = targetBetObj.transform.position;
                totalBetAmount = 0;
                //betAmountTxt.text = winnerAmount.ToString();
                genBetObj.transform.DOMove(teenPattiPlayers[j].sendBetObj.transform.position, 0.3f).OnComplete(() =>
                {
                    //betAmountTxt.text = winnerAmount.ToString();
                    /*if (teenPattiPlayers[j].playerNo == player1.playerNo)
                    {
                        //Add to  winnner Amount

                        float adminPercentage = DataManager.Instance.adminPercentage;

                        float winAmount = winnerAmount;
                        float adminCommssion = (adminPercentage / 100);
                        float playerWinAmount = winAmount - (winAmount * adminCommssion);

                        print(playerWinAmount + "<-------- Crediting amount in animation");

                        if (playerWinAmount != 0)
                        {
                            SoundManager.Instance.CasinoWinSound();
                            DataManager.Instance.AddAmount((float)(playerWinAmount), DataManager.Instance.gameId, "TeenPatti-Win-" + DataManager.Instance.gameId, "won", (float)(adminCommssion), player1.playerNo);
                        }
                    }*/
                });
                
                // Happening outside Dotween animation
                if (teenPattiPlayers[j].playerNo == player1.playerNo)
                {
                    //Add to  winnner Amount

                    float adminPercentage = DataManager.Instance.adminPercentage;

                    float winAmount = winnerAmount;
                    float adminCommssion = (adminPercentage / 100);
                    float playerWinAmount = winAmount - (winAmount * adminCommssion);

                    print(playerWinAmount + "<-------- Crediting amount Outside animation");

                    if (playerWinAmount != 0)
                    {
                        SoundManager.Instance.CasinoWinSound();
                        winAnimationTxt.gameObject.SetActive(true);
                        winAnimationTxt.text = "+" + playerWinAmount;
                        Invoke(nameof(WinAmountTextOff), 1.5f);
                        DataManager.Instance.AddAmount((float)(playerWinAmount), DataManager.Instance.gameId, "TeenPatti-Win-" + DataManager.Instance.gameId, "won", (float)(adminCommssion), player1.playerNo);
                    }
                }

                Destroy(genBetObj, 0.4f);
            }
        }
        
        
        Invoke(nameof(GameRestartRound), 0.4f);
    }
    
    public void WinAmountTextOff()
    {
        winAnimationTxt.gameObject.SetActive(false);

    }

    public void GetBet(int playerNo, float amount, string type, string playerSlideShowSendId, string playerIdSlideShowId)
    {

        if (type == "Show")
        {
            //ShowCardToAllUser("Show", true);
        }
        else if (type == "SideShow")
        {
            //print("Enter The First Slide Show");
            if (playerSlideShowSendId.Equals(player1.playerId) && !playerIdSlideShowId.Equals(player1.playerId))
            {
                print("Enter The Second Side Show");

                slideShowPanel.SetActive(true);
                TeenPattiSlideShow.Instance.sendId = playerSlideShowSendId;
                TeenPattiSlideShow.Instance.currentId = playerIdSlideShowId;
            }
        }
        int playerIndex = 0;
        for (int i = 0; i < teenPattiPlayers.Count; i++)
        {
            if (teenPattiPlayers[i].playerNo == playerNo)
            {
                playerIndex = i;
            }
        }

        bool isB = false;
        bool isS = false;
        if (type != "SideShow")
        {
            BetAnim(teenPattiPlayers[playerIndex], amount, currentPriceIndex);
        }
        if (teenPattiPlayers[playerIndex].isBlind)
        {
            isB = true;
        }
        else if (teenPattiPlayers[playerIndex].isSeen)
        {
            isS = true;
        }
        //currentPriceValue = amount;
        if (!player1.isPack && player1.isBlind)
        {
            if (isS)
            {
                //currentPriceValue /= 2;
                currentPriceValue = currentPriceValue;
            }
            else if (isB)
            {
                currentPriceValue = currentPriceValue;
            }
            priceBtnTxt.text = "Blind\n" + currentPriceValue;
        }
        else if (!player1.isPack && player1.isSeen)
        {
            if (isS)
            {
                currentPriceValue = currentPriceValue;
            }
            else if (isB)
            {
                currentPriceValue = currentPriceValue * 2;
            }
            priceBtnTxt.text = "Chaal\n" + currentPriceValue;
        }
    }


    public void SlideShow_Accpet_Socket(string playerId1, string playerId2)
    {
        if (DataManager.Instance.playerData._id.Equals(playerId1) || DataManager.Instance.playerData._id != playerId2)
        {
            StartCoroutine(CheckSlideShowWinner(playerId1, playerId2, true));
        }

    }

    public void SlideShow_Cancel_Socket()
    {
        ChangePlayerTurn(player1.playerNo);
    }

    public void GetCardStatus(string value, int playerNo)
    {
        //print("Card Status : " + value + "    Player No  :" + playerNo);
        for (int i = 0; i < playerSquList.Count; i++)
        {
            if (playerSquList[i].gameObject.activeSelf == true && playerSquList[i].playerNo == playerNo)
            {
                if (value.Equals("SEEN"))
                {
                    playerSquList[i].isSeen = true;
                    playerSquList[i].isBlind = false;
                    playerSquList[i].isPack = false;
                    ShowTextChange();
                    //ShowTextChange(teenPattiPlayers[i]);

                    playerSquList[i].seenImg.SetActive(true);
                }
                else if (value.Equals("PACK"))
                {
                    playerSquList[i].isPack = true;
                    playerSquList[i].isBlind = false;
                    playerSquList[i].isSeen = false;
                    for (int j = 0; j < playerSquList[i].seeObj.Length; j++)
                    {
                        playerSquList[i].seeObj[j].SetActive(false);
                    }
                    playerSquList[i].packImg.SetActive(true);
                    //CheckWin();
                    CheckPackTime(playerSquList[i]);
                    // Greejesh Pack Check



                }
            }
        }


    }


    void CheckPackTime(TeenPattiPlayer packPlayer)
    {
        print("Enter The Check Player");
        List<TeenPattiPlayer> livePlayers = new List<TeenPattiPlayer>();
        print(teenPattiPlayers.Count);
        for (int i = 0; i < teenPattiPlayers.Count; i++)
        {
            if (teenPattiPlayers[i].isPack == false && teenPattiPlayers[i].gameObject.activeSelf == true)
            {
                livePlayers.Add(teenPattiPlayers[i]);
            }
        }
        print("livePlayers.Count : " + livePlayers.Count);
        packPlayer.isTurn = false;
        if (livePlayers.Count == 1)
        {
            livePlayers[0].isTurn = false;
            string winValue = ",";
            winValue += livePlayers[0].playerNo + ",";
            if (livePlayers[0].playerNo == playerNo)
            {
                SetTeenPattiWon(livePlayers[0].playerId);
                Debug.LogWarning("------------------won is called-------------------------------------");
            }
            foreach (var t in livePlayers[0].playerWinObj)
            {
                t.SetActive(true);
            }

            StartCoroutine(RestartGamePlay());
        }
        else
        {
            if (isAdmin)
            {
                ChangePlayerTurn(packPlayer.playerNo);
            }
        }
    }


    public void ChangeAAdmin(string leavePlayerId, string adminId)
    {

        /*bool isAdminLeave = false;
        for (int i = 0; i < teenPattiPlayers.Count; i++)
        {
            if (teenPattiPlayers[i].playerId.Equals(leavePlayerId) && teenPattiPlayers[i].playerNo == 1)
            {
                isAdminLeave = true;
            }
        }*/
        
        for (int i = 0; i < teenPattiPlayers.Count; i++)
        {
            if (teenPattiPlayers[i].playerId.Equals(leavePlayerId))
            {
                teenPattiPlayers[i].isPack = true;
                teenPattiPlayers[i].isBlind = false;
                teenPattiPlayers[i].isSeen = false;
                for (int j = 0; j < teenPattiPlayers[i].seeObj.Length; j++)
                {
                    teenPattiPlayers[i].seeObj[j].SetActive(false);
                }
                teenPattiPlayers[i].packImg.SetActive(true);
                
                //teenPattiPlayers[i].gameObject.SetActive(false);
                //DataManager.Instance.joinPlayerDatas.Remove(DataManager.Instance.joinPlayerDatas[i]);
                StartCoroutine(WaitGameToCompleteRemovePlayer(CheckLeftPlayer, i));
                CheckPackTime(teenPattiPlayers[i]);
                if (teenPattiPlayers[i].isTurn)
                {
                    ChangePlayerTurn(teenPattiPlayers[i].playerNo);
                }
            }
        }
        
        /*if (isAdminLeave)
        {
            if (player1.playerId.Equals(adminId))
            {
                isAdmin = true;
            }
            else
            {
                isAdmin = false;
            }
        }*/

        /*for (int i = 0; i < teenPattiPlayers.Count; i++)
        {
            if (teenPattiPlayers[i].gameObject.activeSelf)
            {
                string playerIdGet = teenPattiPlayers[i].playerId;

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
                    teenPattiPlayers[i].gameObject.SetActive(false);
                    teenPattiPlayers.RemoveAt(i);
                    teenPattiPlayers[i].isPack = true;
                    teenPattiPlayers[i].isBlind = false;
                    teenPattiPlayers[i].isSeen = false;
                    teenPattiPlayers[i].packImg.SetActive(true);
                    CheckPackTime(teenPattiPlayers[i]);
                }
            }
        }*/
        
        if (DataManager.Instance.playerData._id.Equals(DataManager.Instance.joinPlayerDatas[0].userId))
        {
            isAdmin = true;
            if (DataManager.Instance.joinPlayerDatas.Count == 5 && waitNextRoundScreenObj.activeSelf)
            {
                DataManager.Instance.joinPlayerDatas.RemoveAt(0);
                //RoundGenerate();
                CheckJoinedPlayers();
                StartGamePlay();
                if (waitNextRoundScreenObj.activeSelf)
                {
                    waitNextRoundScreenObj.SetActive(false);
                }
                ResetBot();
                ActivateBotPlayers();
            }
        }
        else
        {
            if (!isGameStarted)
            {
                isAdmin = false;
            }

            if (DataManager.Instance.joinPlayerDatas.Count < 6) return;
            int index = DataManager.Instance.joinPlayerDatas.FindIndex(leftPlayer => leftPlayer.userId == leavePlayerId);
            DataManager.Instance.joinPlayerDatas.Remove(DataManager.Instance.joinPlayerDatas[index]);
        }
    }

    public void CheckJoinedPlayers()
    {
        DataManager.Instance.joinPlayerDatas = DataManager.Instance.joinPlayerDatas.Where(player => !player.avtar.StartsWith("http://206.189.140.131/assets/img/profile-picture/")).ToList();
        // assiging new remaining bot players
        if (DataManager.Instance.joinPlayerDatas.Count <= 4)
        {
            MainMenuManager.Instance.CheckPlayers();
        }
    }

    #endregion

    #region CheckWin

    public IEnumerator CheckSlideShowWinner(string playerId1, string playerId2, bool isSocket)
    {
        int cnt = 0;

        //List<TeenPattiPlayer> teenSlideShowPlayers = new List<TeenPattiPlayer>();
        for (int i = 0; i < teenPattiPlayers.Count; i++)
        {
            if (isSocket && teenPattiPlayers[i].gameObject.activeSelf == true && teenPattiPlayers[i].isPack == false && (teenPattiPlayers[i].playerId.Equals(playerId1) || teenPattiPlayers[i].playerId.Equals(playerId2)))
            {
                cnt++;
                teenPattiPlayers[i].CardDisplay();
                //teenSlideShowPlayers.Add(teenPattiPlayers[i]);
            }
        }
        if (CheckMoney(currentPriceValue) == false)
        {
            SoundManager.Instance.ButtonClick();
            OpenErrorScreen();
            yield break;
        }
        BetAnim(player1, currentPriceValue, currentPriceIndex);
        DataManager.Instance.DebitAmount((currentPriceValue).ToString(), DataManager.Instance.gameId, "TeenPatti-Bet-" + DataManager.Instance.gameId, "game", 1);
        playerBetAmount += currentPriceValue;
        yield return new WaitForSeconds(0.75f);

        /*if (teenSlideShowPlayers.Count == 2)
        {
            TeenPattiPlayer slidePlayer1 = teenPattiPlayers[0];
            TeenPattiPlayer slidePlayer2 = teenPattiPlayers[1];

            if (slidePlayer1.ruleNo < slidePlayer2.ruleNo)
            {
                if (isSocket)
                {
                    ChangeCardStatus("PACK", slidePlayer2.playerNo);
                    ChangePlayerTurn(slidePlayer2.playerNo);
                }
                //pack slideplayer2
            }
            else if (slidePlayer2.ruleNo < slidePlayer1.ruleNo)
            {
                //pack slideplayer1
                if (isSocket)
                {
                    ChangeCardStatus("PACK", slidePlayer1.playerNo);
                    ChangePlayerTurn(slidePlayer1.playerNo);
                }
            }
            else if (slidePlayer2.ruleNo == slidePlayer1.ruleNo)
            {
                if (slidePlayer1.ruleNo == 1)
                {
                    if (slidePlayer1.card1.cardNo > slidePlayer2.card1.cardNo)
                    {
                        //pack slide player 2
                        if (isSocket)
                        {
                            ChangeCardStatus("PACK", slidePlayer2.playerNo);
                            ChangePlayerTurn(slidePlayer2.playerNo);
                        }
                    }
                    else if (slidePlayer2.card1.cardNo > slidePlayer1.card1.cardNo)
                    {
                        //pack slide player 1
                        if (isSocket)
                        {
                            ChangeCardStatus("PACK", slidePlayer1.playerNo);
                            ChangePlayerTurn(slidePlayer1.playerNo);
                        }
                    }
                    else if (slidePlayer1.card1.cardNo == slidePlayer2.card1.cardNo)
                    {
                        //pack slide player 1
                        if (isSocket)
                        {
                            ChangeCardStatus("PACK", slidePlayer1.playerNo);
                            ChangePlayerTurn(slidePlayer2.playerNo);
                        }
                    }
                }
                else if (slidePlayer1.ruleNo == 5)
                {
                    if (slidePlayer1.card1.cardNo > slidePlayer2.card1.cardNo)
                    {
                        //pack slide player 2
                        if (isSocket)
                        {
                            ChangeCardStatus("PACK", slidePlayer2.playerNo);
                            ChangePlayerTurn(slidePlayer2.playerNo);
                        }
                    }
                    else if (slidePlayer2.card1.cardNo > slidePlayer1.card1.cardNo)
                    {
                        //pack slide player 1
                        if (isSocket)
                        {
                            ChangeCardStatus("PACK", slidePlayer1.playerNo);
                            ChangePlayerTurn(slidePlayer1.playerNo);
                        }
                    }
                    else if (slidePlayer1.card1.cardNo == slidePlayer2.card1.cardNo)
                    {

                        if (slidePlayer1.card3.cardNo > slidePlayer2.card3.cardNo)
                        {
                            //pack slide player 2
                            if (isSocket)
                            {
                                ChangeCardStatus("PACK", slidePlayer2.playerNo);
                                ChangePlayerTurn(slidePlayer2.playerNo);
                            }
                        }
                        else if (slidePlayer2.card3.cardNo > slidePlayer1.card3.cardNo)
                        {
                            //pack slide player 1
                            if (isSocket)
                            {
                                ChangeCardStatus("PACK", slidePlayer1.playerNo);
                                ChangePlayerTurn(slidePlayer1.playerNo);
                            }
                        }
                        else if (slidePlayer1.card3.cardNo == slidePlayer2.card3.cardNo)
                        {
                            //pack slide player 1
                            if (isSocket)
                            {
                                ChangeCardStatus("PACK", slidePlayer1.playerNo);
                                ChangePlayerTurn(slidePlayer1.playerNo);
                            }
                        }
                    }
                }
                else
                {
                    int highestNo1 = 0;
                    if (slidePlayer1.card1.cardNo > slidePlayer2.card1.cardNo)
                    {
                        //pack slide player 2
                        if (isSocket)
                        {
                            ChangeCardStatus("PACK", slidePlayer2.playerNo);
                            ChangePlayerTurn(slidePlayer2.playerNo);
                        }
                    }
                    else if (slidePlayer1.card1.cardNo < slidePlayer2.card1.cardNo)
                    {
                        //pack slide player 1
                        if (isSocket)
                        {
                            ChangeCardStatus("PACK", slidePlayer1.playerNo);
                            ChangePlayerTurn(slidePlayer1.playerNo);
                        }
                    }
                    else
                    {
                        if (slidePlayer1.card2.cardNo > slidePlayer2.card2.cardNo)
                        {
                            //pack slide player 2
                            if (isSocket)
                            {
                                ChangeCardStatus("PACK", slidePlayer2.playerNo);
                                ChangePlayerTurn(slidePlayer2.playerNo);
                            }
                        }
                        else if (slidePlayer1.card2.cardNo < slidePlayer2.card2.cardNo)
                        {
                            //pack slide player 1
                            if (isSocket)
                            {
                                ChangeCardStatus("PACK", slidePlayer1.playerNo);
                                ChangePlayerTurn(slidePlayer1.playerNo);
                            }
                        }
                        else
                        {
                            if (slidePlayer1.card3.cardNo > slidePlayer2.card3.cardNo)
                            {
                                //pack slide player 2
                                if (isSocket)
                                {
                                    ChangeCardStatus("PACK", slidePlayer2.playerNo);
                                    ChangePlayerTurn(slidePlayer2.playerNo);
                                }
                            }
                            else if (slidePlayer1.card3.cardNo < slidePlayer2.card3.cardNo)
                            {
                                //pack slide player 1
                                if (isSocket)
                                {
                                    ChangeCardStatus("PACK", slidePlayer1.playerNo);
                                    ChangePlayerTurn(slidePlayer1.playerNo);
                                }
                            }
                            else if (slidePlayer1.card3.cardNo == slidePlayer2.card3.cardNo)
                            {
                                //pack slide player 1
                                if (isSocket)
                                {
                                    ChangeCardStatus("PACK", slidePlayer1.playerNo);
                                    ChangePlayerTurn(slidePlayer1.playerNo);
                                }
                            }
                        }
                    }

                }

            }
        }*/

    }


    public void ShowCardToAllUser()
    {
        winMaintain.Clear();
        foreach (var t in teenPattiPlayers.Where(t => t.gameObject.activeSelf == true && (t.isSeen || t.isBlind) && t.isPack == false))
        {
            t.CardDisplay();
        }
        //CheckFinalWinner(type);
        bottomBox.SetActive(false);
    }

    public string CheckFinalWinner(string type)
    {
        List<TeenPattiPlayer> teenPattiWinner = new List<TeenPattiPlayer>();

        foreach (var t in teenPattiPlayers.Where(t => t.gameObject.activeSelf == true && (t.isSeen || t.isBlind) && t.isPack == false))
        {
            foreach (var t1 in t.seeObj)
            {
                t1.SetActive(false);
            }
        }
        
        /*List<TeenPattiPlayer> teenPattiWinner = teenPattiPlayers.Where(p => p.gameObject.activeSelf && (p.isSeen || p.isBlind) && !p.isPack).OrderByDescending(p => p.ruleNo).ThenByDescending(p => p.isBot && p.ruleNo == 6).ToList();

        if (teenPattiWinner.Count > 0)
        {
            TeenPattiPlayer winner = teenPattiWinner[0];
            teenPattiWinner.Clear();
            teenPattiWinner.Add(winner);
        }
        else
        {
            teenPattiWinner.Clear();
        }*/

        foreach (var player in teenPattiPlayers)
        {
            player.SumOfPlayerCards();
        }
        
        List<TeenPattiPlayer> sortedNumbersDescending = teenPattiPlayers.OrderByDescending(n => n.sumOfCards).ToList();
        
        for (int i = 0; i < sortedNumbersDescending.Count; i++)
        {
            //calculate sum of 3 card value and store in a varible
            if (sortedNumbersDescending[i].gameObject.activeSelf == true && (sortedNumbersDescending[i].isSeen || sortedNumbersDescending[i].isBlind) && sortedNumbersDescending[i].isPack == false)
            {
                bool isEnter = false;
                for (int j = 0; j < teenPattiWinner.Count; j++)
                {
                    if (teenPattiWinner[j].ruleNo > sortedNumbersDescending[i].ruleNo)
                    {
                        isEnter = true;
                    }
                }
                // Highest rule number player will be added to teenpattiwinner list
                if (isEnter == true)
                {
                    teenPattiWinner.Clear();

                    teenPattiWinner.Add(sortedNumbersDescending[i]);
                    //print("Clear");
                }
                else if (teenPattiWinner.Count == 0)
                {
                    teenPattiWinner.Add(sortedNumbersDescending[i]);

                }//print("Add");
            }
        }
        
        
        /*for (int i = 0; i < teenPattiPlayers.Count; i++)
        {
            if (teenPattiPlayers[i].gameObject.activeSelf == true && (teenPattiPlayers[i].isSeen || teenPattiPlayers[i].isBlind) && teenPattiPlayers[i].isPack == false)
            {
                bool isEnter = false;
                for (int j = 0; j < teenPattiWinner.Count; j++)
                {
                    if (teenPattiWinner[j].ruleNo > teenPattiPlayers[i].ruleNo)
                    {
                        isEnter = true;
                    }
                }
                if (isEnter == true)
                {
                    teenPattiWinner.Clear();

                    teenPattiWinner.Add(teenPattiPlayers[i]);
                    //print("Clear");
                }
                else if (teenPattiWinner.Count == 0)
                {
                    teenPattiWinner.Add(teenPattiPlayers[i]);

                }//print("Add");
            }
        }*/
        ShowWinPlayer(type, teenPattiWinner);
        
        CreditWinnerAmount(teenPattiWinner[0].playerId);
        SetTeenPattiWon(teenPattiWinner[0].playerId);
        return teenPattiWinner[0].playerId;
    }

    public void HandelTeenPattiWinData(string winnerPlayerId)
    {
        List<TeenPattiPlayer> winnerPlayer = teenPattiPlayers.Where(p => p.playerId == winnerPlayerId).ToList();
                
        if (winnerPlayer.Count > 0)
        {
            ShowCardToAllUser();
            ShowWinPlayer("Show", winnerPlayer);
        }
    }

    public void ShowWinPlayer(string type, List<TeenPattiPlayer> teenPattiWinner)
    {
        isBotActivate = false;
        if (teenPattiWinner.Count == 1)
        {
            int rule = teenPattiWinner[0].ruleNo;
            string winValue = ",";
            winValue += teenPattiWinner[0].playerNo + ",";
            if (teenPattiWinner[0].playerNo == playerNo)
            {
                if (teenPattiWinner[0].playerNo == playerNo)
                {
                    //SetTeenPattiWon(winValue);
                    //Debug.LogWarning("------------------won is called-------------------------------------");
                }
            }

            //print("Rule 1 : " + rule);
            //win
            foreach (var t in teenPattiWinner[0].playerWinObj)
            {
                t.SetActive(true);
            }
            SoundManager.Instance.CasinoWinSound();

            StartCoroutine(RestartGamePlay());
        }
        else if (teenPattiWinner.Count > 1)
        {
            /*int rule = teenPattiWinner[0].ruleNo;

            //print("Rule 2 : " + rule);
            switch (rule)
            {
                case 1:
                {
                    int highestNo1 = teenPattiWinner[0].card1.cardNo;
                    highestNo1 = teenPattiWinner.Select(t => t.card1.cardNo).Prepend(highestNo1).Max();

                    List<TeenPattiPlayer> playerList1 =
                        teenPattiWinner.Where(t => highestNo1 == t.card1.cardNo).ToList();

                    if (playerList1.Count == 1)
                    {
                        //win
                        string winValue = ",";
                        winValue += playerList1[0].playerNo + ",";
                        if (playerList1[0].playerNo == playerNo)
                        {
                            if (playerList1[0].playerNo == playerNo)
                            {
                                SetTeenPattiWon(winValue); // with player id // Moved in click
                                Debug.LogWarning(
                                    "------------------won is called-------------------------------------");
                            }
                        }

                        foreach (var t in playerList1[0].playerWinObj)
                        {
                            t.SetActive(true);
                        }

                        StartCoroutine(RestartGamePlay());
                    }

                    break;
                }
                case 5:
                {
                    int highestNo1 = teenPattiWinner[0].card1.cardNo;
                    highestNo1 = teenPattiWinner.Select(t => t.card1.cardNo).Prepend(highestNo1).Max();

                    List<TeenPattiPlayer> playerList1 =
                        teenPattiWinner.Where(t => highestNo1 == t.card1.cardNo).ToList();

                    if (playerList1.Count == 1)
                    {
                        //win
                        string winValue = ",";
                        winValue += playerList1[0].playerNo + ",";
                        if (playerList1[0].playerNo == playerNo)
                        {
                            SetTeenPattiWon(winValue);
                            Debug.LogWarning("------------------won is called-------------------------------------");
                        }

                        foreach (var t in playerList1[0].playerWinObj)
                        {
                            t.SetActive(true);
                        }

                        StartCoroutine(RestartGamePlay());
                    }
                    else
                    {
                        int highestNo3 = teenPattiWinner[0].card3.cardNo;
                        highestNo3 = teenPattiWinner.Select(t => t.card3.cardNo).Prepend(highestNo3).Max();

                        List<TeenPattiPlayer> playerList3 =
                            teenPattiWinner.Where(t => highestNo3 == t.card3.cardNo).ToList();

                        if (playerList3.Count == 1)
                        {
                            //win
                            string winValue = ",";
                            winValue += playerList3[0].playerNo + ",";
                            if (playerList3[0].playerNo == playerNo)
                            {
                                SetTeenPattiWon(winValue);
                                Debug.LogWarning(
                                    "------------------won is called-------------------------------------");
                            }

                            foreach (var t in playerList3[0].playerWinObj)
                            {
                                t.SetActive(true);
                            }

                            StartCoroutine(RestartGamePlay());
                        }
                        else
                        {
                            //win
                            if (type == "Show")
                            {
                                ChangeCardStatus("PACK", player1.playerNo);
                                //ChangePlayerTurn(player1.playerNo);
                            }
                            else
                            {
                                string winValue = ",";
                                winValue += playerList1[0].playerNo + ",";
                                foreach (var t in playerList3)
                                {
                                    winValue += t.playerNo + ",";
                                    foreach (var t1 in t.playerWinObj)
                                    {
                                        t1.SetActive(true);
                                    }
                                }

                                StartCoroutine(RestartGamePlay());
                                if (playerList3[0].playerNo == playerNo)
                                {
                                    SetTeenPattiWon(winValue);
                                    Debug.LogWarning(
                                        "------------------won is called-------------------------------------");
                                }
                            }
                        }
                    }

                    break;
                }
                default:
                {
                    int highestNo1 = teenPattiWinner[0].card1.cardNo;
                    highestNo1 = teenPattiWinner.Select(t => t.card1.cardNo).Prepend(highestNo1).Max();

                    List<TeenPattiPlayer> playerList1 =
                        teenPattiWinner.Where(t => highestNo1 == t.card1.cardNo).ToList();

                    if (playerList1.Count == 1)
                    {
                        //win
                        string winValue = ",";
                        winValue += playerList1[0].playerNo + ",";
                        if (playerList1[0].playerNo == playerNo)
                        {
                            SetTeenPattiWon(winValue);
                            Debug.LogWarning("------------------won is called-------------------------------------");
                        }

                        foreach (var t in playerList1[0].playerWinObj)
                        {
                            t.SetActive(true);
                        }

                        StartCoroutine(RestartGamePlay());
                    }
                    else
                    {
                        int highestNo2 = teenPattiWinner[0].card2.cardNo;
                        highestNo2 = teenPattiWinner.Select(t => t.card2.cardNo).Prepend(highestNo2).Max();

                        List<TeenPattiPlayer> playerList2 =
                            teenPattiWinner.Where(t => highestNo2 == t.card2.cardNo).ToList();

                        if (playerList2.Count == 1)
                        {
                            //win
                            string winValue = ",";
                            winValue += playerList2[0].playerNo + ",";
                            if (playerList2[0].playerNo == playerNo)
                            {
                                SetTeenPattiWon(winValue);
                                Debug.LogWarning(
                                    "------------------won is called-------------------------------------");
                            }

                            foreach (var t in playerList2[0].playerWinObj)
                            {
                                t.SetActive(true);
                            }

                            StartCoroutine(RestartGamePlay());
                        }
                        else
                        {
                            int highestNo3 = teenPattiWinner[0].card3.cardNo;
                            highestNo3 = teenPattiWinner.Select(t => t.card3.cardNo).Prepend(highestNo3).Max();

                            List<TeenPattiPlayer> playerList3 =
                                teenPattiWinner.Where(t => highestNo3 == t.card3.cardNo).ToList();

                            if (playerList3.Count == 1)
                            {
                                //win
                                string winValue = ",";
                                winValue += playerList3[0].playerNo + ",";
                                if (playerList3[0].playerNo == playerNo)
                                {
                                    SetTeenPattiWon(winValue);
                                    Debug.LogWarning(
                                        "------------------won is called-------------------------------------");
                                }

                                foreach (var t in playerList3[0].playerWinObj)
                                {
                                    t.SetActive(true);
                                }

                                StartCoroutine(RestartGamePlay());
                            }
                            else
                            {
                                //win
                                if (type == "Show")
                                {
                                    ChangeCardStatus("PACK", player1.playerNo);
                                    //ChangePlayerTurn(player1.playerNo);
                                }
                                else
                                {
                                    string winValue = ",";
                                    foreach (var t in playerList3)
                                    {
                                        winValue += t.playerNo + ",";
                                        foreach (var t1 in t.playerWinObj)
                                        {
                                            t1.SetActive(true);
                                        }
                                    }

                                    StartCoroutine(RestartGamePlay());
                                    if (playerList3[0].playerNo == playerNo)
                                    {
                                        SetTeenPattiWon(winValue);
                                        Debug.LogWarning(
                                            "------------------won is called-------------------------------------");
                                    }
                                }
                            }
                        }
                    }

                    break;
                }
            }*/
        }
    }

    #endregion
}