using System;
using System;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AndarBaharManager : MonoBehaviour
{


    public static AndarBaharManager Instance;


    [Header("--- Andar Bahar Players ---")]
    public AndarBaharPlayer player1;
    public AndarBaharPlayer player2;
    public AndarBaharPlayer player3;
    public AndarBaharPlayer player4;
    public AndarBaharPlayer player5;
    public AndarBaharPlayer player6;
    public AndarBaharPlayer player7;
    public AndarBaharPlayer player8;

    public List<AndarBaharPlayer> andarBaharPlayerList = new List<AndarBaharPlayer>();

    [Header("---UI---")]
    public Text winAnimationTxt;
    public Text andarPriceTxt;
    public Text baharPriceTxt;
    public Text mainPriceTxt;
    public GameObject errorScreenObj;
    public GameObject waitNextRoundScreenObj;
    public Image avatarWait;
    public Text waitUserNameTxt;
    public GameObject playerFindScreenObj;
    public Image avatarPlayerFind;
    public Text playerFindUserNameTxt;


    public Button andarPlusButton;
    public Button andarMinusButton;
    public Button baharPlusButton;
    public Button baharMinusButton;
    public Text timerTxt;
    public float secondCount;
    public int timerValue;
    public float fixTimeSet;
    public Sprite cardBackSide;

    [Header("---Game Menu UI---")]
    public Animator startBettingScreenObj;
    public Animator stopBettingScreenObj;
    public GameObject AndarPopUp;
    public GameObject BaharPopUp;

    [Header("---History Cards---")]
    public List<int> winList;
    public List<GameObject> historyCards = new List<GameObject>();
    public GameObject HistoryCardHolder;
    public GameObject AndarCard;
    public GameObject BaharCard;

    [Header("---Win Maintain---")]
    public GameObject andarEarnObj;
    public GameObject baharEarnObj;

    [Header("--- Open Message Screen ---")]
    public GameObject messageScreeObj;
    public GameObject giftScreenObj;

    [Header("--- Chat Panel ---")]
    public GameObject chatPanelParent;
    public GameObject chatMePrefab;
    public GameObject chatOtherPrefab;

    [Header("--- Gift Maintain ---")]
    public GameObject giftParentObj;
    public GameObject giftPrefab;
    public List<GiftBox> giftBoxes = new List<GiftBox>();

    [Header("---Game Play---")]
    public List<CardSuffle> cardSuffles = new List<CardSuffle>();
    public List<ListStoreData> listStoreDatas = new List<ListStoreData>();
    public List<int> mainList = new List<int>();
    public List<CardSuffle> cardSufflesGen = new List<CardSuffle>();

    [Header("---Card Maintain---")]
    public GameObject leftCard;
    public GameObject rightCard;
    public GameObject centerCard;
    public GameObject startCard;
    public GameObject cardGenObj;
    public List<GameObject> andarPlayerObj = new List<GameObject>();
    public List<GameObject> baharPlayerObj = new List<GameObject>();

    public GameObject andarCPrefab;
    public GameObject baharCPrefab;
    public GameObject andarParentPos;
    public GameObject baharParentPos;

    public float tableMaxLimit;
    public float tableMinLimit;
    public float incrementNo;
    public float balance;

    public int cardCnt = 0;

    public bool isPlaceBet;
    public bool isAndarWin;
    public bool isBaharWin;
    public bool isGamePlayContinue;

    public float multi_X_Earn;


    [Header("--- Menu Screen ---")]
    public GameObject menuScreenObj;

    [Header("--- Rule Screen ---")]
    public GameObject ruleScreenObj;

    [Header("--- Prefab ---")]
    public GameObject shopPrefab;
    public GameObject shopPrefabParent;

    public List<AndarBaharPlaceBet> andarBaharPlaceBets = new List<AndarBaharPlaceBet>();

    public bool isAdmin;
    public int deckNo;

    public float totalInvestAndar;
    public float totalInvestBahar;

    private bool _isAndarActive;
    private bool _isBaharActive;

    private int _tempAndarNum;
    private int _tempBaharNum;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }


    public void UpdateNameBalance()
    {

        mainPriceTxt.text = DataManager.Instance.playerData.balance.ToString();
    }


    // Start is called before the first frame update
    void Start()
    {
        SoundManager.Instance.StopBackgroundMusic();
        AndarPopUp.gameObject.SetActive(false);
        BaharPopUp.gameObject.SetActive(false);


        incrementNo = tableMaxLimit / tableMinLimit;

        andarPriceTxt.text = tableMinLimit.ToString();
        andarPlusButton.interactable = true;
        andarMinusButton.interactable = false;


        baharPriceTxt.text = tableMinLimit.ToString();
        baharPlusButton.interactable = true;
        baharMinusButton.interactable = false;

        for (int i = 0; i < andarBaharPlayerList.Count; i++)
        {
            andarBaharPlayerList[i].gameObject.SetActive(false);
        }

        _isAndarActive = false;
        _isBaharActive = false;
        UpdateNameBalance();

        PlayerFound();
        //HistoryLoader(DataManager.Instance.winList);


    }
    
    


    public void PlayerFound()
    {
        if (DataManager.Instance.joinPlayerDatas.Count >= TestSocketIO.Instace.andarBaharRequirePlayer)
        {
            CreateAdmin();
            if (DataManager.Instance.joinPlayerDatas.Count == 1 && isAdmin)
            {
                RoundGenerate();
            }
            else
            {
                if (isAdmin) return;
                // foreach (var t in andarBaharPlayerList)
                // {
                //     t.gameObject.SetActive(false);
                // }
                waitUserNameTxt.text = DataManager.Instance.playerData.firstName;
                if (isPlaceBet == false)
                {
                    waitNextRoundScreenObj.SetActive(true);
                    print("--------------------------Is game waitNextRoundScreenObj false---------------------------------");
                }
            }

        }
        else
        {

            for (int i = 0; i < andarBaharPlayerList.Count; i++)
            {
                andarBaharPlayerList[i].gameObject.SetActive(false);
            }
            playerFindUserNameTxt.text = DataManager.Instance.playerData.firstName;
            //playerFindScreenObj.SetActive(true);
        }
        
    }

    void UpdateAndarBaharPrice()
    {
        mainPriceTxt.text = DataManager.Instance.playerData.balance;
    }
    
    public void HistoryLoader(string data)
    {
        if(data != "")
        {
            winList = new List<int>(data.Split(',').Select(x => int.Parse(x)));
        }
        
        foreach (var t in winList)
        {
            HistoryTacker(t);
        }
    }


    void ListGenerate()
    {
        for (int i = 0; i < 300; i++)
        {
            List<int> data = new List<int>();
            for (int j = 0; j < 52; j++)
            {
                while (true)
                {
                    int rno = UnityEngine.Random.Range(1, 53);
                    if (!data.Contains(rno))
                    {
                        data.Add(rno);
                        break;
                    }
                }
            }
            ListStoreData lData = new ListStoreData();
            lData.noList = data;
            listStoreDatas.Add(lData);
        }
    }

    void RoundGenerate()
    {
        //BotPlayerManager.Instance.TurnOffBotBet();
        totalInvestAndar = 0;
        totalInvestBahar = 0;
        _tempAndarNum = 0;
        _tempBaharNum = 0;
        waitNextRoundScreenObj.SetActive(false);
        playerFindScreenObj.SetActive(false);
        cardSufflesGen.Clear();
        
        //BotPlayerManager.Instance.UpdateBalance();

        SoundManager.Instance.CasinoTurnSound();
        DataManager.Instance.UserTurnVibrate();
        if (isAdmin)
        {
            SetRoomData();
            TestSocketIO.Instace.SetGameId(DataManager.Instance.tournamentID);
        }

        for (int i = 0; i < cardGenObj.transform.childCount; i++)
        {
            Destroy(cardGenObj.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < andarBaharPlayerList.Count; i++)
        {
            andarBaharPlayerList[i].andarObj.SetActive(true);
            andarBaharPlayerList[i].baharObj.SetActive(true);
        }

        isGamePlayContinue = false;
        isPlaceBet = false;
        cardCnt = 0;
        isAndarWin = false;
        isBaharWin = false;
        andarBaharPlaceBets.Clear();
        for (int i = 0; i < cardGenObj.transform.childCount; i++)
        {
            Destroy(cardGenObj.transform.GetChild(i).gameObject);
        }
        startBettingScreenObj.gameObject.SetActive(true);
        Invoke(nameof(StartBettingObjOff), 2.5f);
        for (int i = 0; i < andarPlayerObj.Count; i++)
        {
            Text t = andarPlayerObj[i].transform.GetChild(1).GetComponent<Text>();
            t.text = "0";
            andarPlayerObj[i].SetActive(false);
        }

        for (int i = 0; i < baharPlayerObj.Count; i++)
        {
            Text t = baharPlayerObj[i].transform.GetChild(1).GetComponent<Text>();
            t.text = "0";
            baharPlayerObj[i].SetActive(false);
        }

        for (int i = 0; i < andarBaharPlayerList.Count; i++)
        {
            andarBaharPlayerList[i].andarObj.SetActive(false);
            andarBaharPlayerList[i].baharObj.SetActive(false);
        }

        int cnt = 1;
        for (int i = 0; i < andarBaharPlayerList.Count; i++)
        {
            if (i < DataManager.Instance.joinPlayerDatas.Count)
            {
                if (DataManager.Instance.joinPlayerDatas[i].userId.Equals(DataManager.Instance.playerData._id))
                {
                    andarBaharPlayerList[0].gameObject.SetActive(true);
                    andarBaharPlayerList[0].playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                    andarBaharPlayerList[0].playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                    andarBaharPlayerList[0].avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                    andarBaharPlayerList[0].UpdateAvatar();
                }
                else
                {
                    andarBaharPlayerList[cnt].gameObject.SetActive(true);
                    andarBaharPlayerList[cnt].playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                    andarBaharPlayerList[cnt].playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                    andarBaharPlayerList[cnt].avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                    andarBaharPlayerList[cnt].UpdateAvatar();

                    cnt++;
                }
            }
            else
            {
                andarBaharPlayerList[i].gameObject.SetActive(false);
                andarBaharPlayerList[i].playerId = "";
                andarBaharPlayerList[i].playerNameTxt.text = "";
            }
        }
        
        //BotPlayerManager.Instance.Invoke(nameof(BotPlayerManager.StartBotBetting), 5f);
        print("Botplayer is called");


        mainList = listStoreDatas[deckNo].noList;
        timerValue = ((int)fixTimeSet);
        timerTxt.text = timerValue.ToString();
        secondCount = fixTimeSet;

        for (int i = 0; i < mainList.Count; i++)
        {
            for (int j = 0; j < cardSuffles.Count; j++)
            {
                if (j == mainList[i])
                {
                    cardSufflesGen.Add(cardSuffles[j]);
                }
            }
        }
        //BotPlayerManager.Instance.OpenBotPlayer();
    }

    void AndarWinPopup()
    {
        AndarPopUp.gameObject.SetActive(false);
    }
    
    void BaharWinPopup()
    {
        BaharPopUp.gameObject.SetActive(false);
    }

    void StartBettingObjOff()
    {
        startBettingScreenObj.gameObject.SetActive(false);
        isPlaceBet = true;
        CenterAnimation(cardSufflesGen[cardCnt]);
        cardCnt++;
    }
    void StopBettingObjOff()
    {
        stopBettingScreenObj.gameObject.SetActive(false);
        ContinueGamePlay();
    }

    void ContinueGamePlay()
    {
        if (!isAdmin) return;
        isPlaceBet = false;

        if (isAndarWin == false && isBaharWin == false)
        {
            if (cardCnt % 2 == 0)
            {
                print("******************* This is is bahar active -> " + cardCnt);
                IncrementBahar();
                if (_isBaharActive)
                {
                    cardCnt = _tempBaharNum;
                    print("-------------------This is is bahar active -> " + cardCnt);
                }

                RightAnimation(cardSufflesGen[cardCnt]);
                SetTempNo(cardCnt, true, false);
                cardCnt++;
                _isBaharActive = false;
            }
            else if (cardCnt % 2 == 1)
            {
                print("******************* This is is andar active -> " + cardCnt);
                IncrementAndar();
                if (_isAndarActive)
                {
                    cardCnt = _tempAndarNum;
                    print("-------------------This is is andar active -> " + cardCnt);
                }

                LeftAnimation(cardSufflesGen[cardCnt]);
                SetTempNo(cardCnt, false, true);
                cardCnt++;
                _isAndarActive = false;
            }
        }
    }


    void PlayerSetNo(int no, bool isAndar, bool isBahar, float value)
    {
        if (isAndar)
        {
            for (int i = 0; i < andarPlayerObj.Count; i++)
            {
                if (i == (no - 1))
                {
                    andarPlayerObj[i].SetActive(true);
                    Text t = andarPlayerObj[i].transform.GetChild(1).GetComponent<Text>();

                    float value1 = float.Parse(t.text);
                    t.text = (value1 + value).ToString();
                    Canvas.ForceUpdateCanvases();
                }
                else
                {
                    andarPlayerObj[i].SetActive(false);
                }
            }
        }
        else if (isBahar)
        {
            for (int i = 0; i < baharPlayerObj.Count; i++)
            {
                if (i == (no - 1))
                {
                    baharPlayerObj[i].SetActive(true);
                    Text t = baharPlayerObj[i].transform.GetChild(1).GetComponent<Text>();

                    float value1 = float.Parse(t.text);
                    t.text = (value1 + value).ToString();
                    Canvas.ForceUpdateCanvases();
                }
                else
                {
                    baharPlayerObj[i].SetActive(false);
                }
            }
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (timerValue == 0 && isGamePlayContinue == false && waitNextRoundScreenObj.activeSelf == false && playerFindScreenObj.activeSelf == false)
        {
            isGamePlayContinue = true;
            stopBettingScreenObj.gameObject.SetActive(true);
            Invoke(nameof(StopBettingObjOff), 2.5f);
            //StartCoroutine(StopBettingOff());

        }
        else if (!timerTxt.text.Equals("0"))
        {
            secondCount -= Time.deltaTime;
            timerValue = ((int)secondCount);
            timerTxt.text = timerValue.ToString();
        }
    }

    #region Animation

    public void LeftAnimation(CardSuffle suffleData)
    {
        GameObject obj = Instantiate(leftCard, cardGenObj.transform);
        SoundManager.Instance.CasinoCardMoveSound();
        obj.transform.position = startCard.transform.position;
        obj.SetActive(true);
        Vector3 leftPos = leftCard.transform.position;
        leftPos.x -= 0.2f;
        obj.transform.DOMove(leftPos, 0.4f).OnComplete(() =>
        {
            SoundManager.Instance.CasinoCardSwipeSound();

            obj.transform.DOScale(new Vector3(0, 1, 1), 0.2f).OnComplete(() =>
               {
                   obj.transform.GetComponent<Image>().sprite = suffleData.cardSprite;
                   obj.transform.DOScale(new Vector3(1, 1, 1), 0.2f).OnComplete(() =>
                   {
                       if (suffleData.cardNo == cardSufflesGen[0].cardNo)
                       {
                           isAndarWin = true;
                           isBaharWin = false;
                           WinEnter();
                           print("Andar Win Player");
                           SoundManager.Instance.TokenHomeSound();
                           AndarPopUp.gameObject.SetActive(true);
                           Invoke(nameof(AndarWinPopup), 2f);
                           UpdateHistoryCard(1);
                           //HistoryTacker(1);
                       }
                       else
                       {
                           ContinueGamePlay();
                       }
                   });
               });
            //obj.transform.DORotate(new Vector3(0, -90, 0), 0.2f).OnComplete(() =>

        });
    }

    public void RightAnimation(CardSuffle suffleData)
    {
        GameObject obj = Instantiate(rightCard, cardGenObj.transform);
        SoundManager.Instance.CasinoCardMoveSound();
        obj.transform.position = startCard.transform.position;
        obj.SetActive(true);
        Vector3 rightPos = rightCard.transform.position;
        rightPos.x -= 0.3f;
        obj.transform.DOMove(rightPos, 0.4f).OnComplete(() =>
        {
            SoundManager.Instance.CasinoCardSwipeSound();
            obj.transform.DOScale(new Vector3(0, 1, 1), 0.2f).OnComplete(() =>
            {
                obj.transform.GetComponent<Image>().sprite = suffleData.cardSprite;
                obj.transform.DOScale(new Vector3(1, 1, 1), 0.2f).OnComplete(() =>
                {
                    if (suffleData.cardNo == cardSufflesGen[0].cardNo)
                    {
                        isBaharWin = true;
                        isAndarWin = false;
                        WinEnter();
                        print("Bahar Win Player");
                        SoundManager.Instance.TokenHomeSound();
                        BaharPopUp.gameObject.SetActive(true);
                        Invoke(nameof(BaharWinPopup), 2f);
                        UpdateHistoryCard(2);
                        //HistoryTacker(2);
                    }
                    else
                    {
                        ContinueGamePlay();
                    }
                });
            });
            //obj.transform.DORotate(new Vector3(0, -90, 0), 0.2f).OnComplete(() =>

        });
    }

    void WinEnter()
    {
        if (isBaharWin)
        {
            float baharValue = 0;
            float playerValue = 0;
            float betPrice = 0;
            for (int j = 0; j < andarBaharPlayerList.Count; j++)
            {
                if (andarBaharPlayerList[j].gameObject.activeSelf == true)
                {
                    string playerId = andarBaharPlayerList[j].playerId;
                    for (int i = 0; i < andarBaharPlaceBets.Count; i++)
                    {
                        AndarBaharPlaceBet bet = andarBaharPlaceBets[i];
                        if (bet.playerId.Equals(playerId))
                        {
                            if (bet.isBahar)
                            {
                                baharValue = bet.value;
                            }
                            if (bet.playerId.Equals(DataManager.Instance.playerData._id))
                            {
                                playerValue = baharValue;
                                betPrice = playerValue;
                            }
                        }
                    }

                    // if (baharValue > 0)
                    // {
                    //     GameObject genObj = Instantiate(baharEarnObj, cardGenObj.transform);
                    //     genObj.transform.position = startCard.transform.position;
                    //     float newValue = baharValue * multi_X_Earn;
                    //     genObj.transform.GetChild(1).GetComponent<Text>().text = newValue.ToString();
                    //     genObj.transform.DOMove(baharPlayerObj[4].transform.position, 0.4f).OnComplete(() =>
                    //      {
                    //          genObj.transform.GetChild(1).GetComponent<Text>().text = (newValue).ToString();
                    //          genObj.transform.DOMove(baharPlayerObj[4].transform.parent.parent.transform.position, 0.4f);
                    //          //genObj.transform.DOMoveZ(0.00000001f, 0.2f).OnComplete(() =>
                    //          // {
                    //          //     genObj.transform.DOScale(Vector3.zero, 0.2f).OnComplete(() =>
                    //          //      {
                    //          //          //Plaus Value
                    //          //          //Destroy(genObj);
                    //          //      });
                    //          // });
                    //      });
                    // }
                }
            }

            playerValue *= 2;


            float otherPrice = totalInvestAndar + totalInvestBahar;
            float adminPercentage = DataManager.Instance.adminPercentage;




            /*float winAmount = playerValue;
            float adminCommssion = (adminPercentage / 100);
            float playerWinAmount = winAmount - (winAmount * adminCommssion);*/
            float winReward = playerValue - betPrice;
            float adminCommission = adminPercentage / 100;
            float winAmount = winReward - (winReward * adminCommission);
            float playerWinAmount = betPrice + winAmount;

            otherPrice -= playerWinAmount;

            if (playerWinAmount != 0)
            {
                player1.winParticleObj.SetActive(false);
                player1.winParticleObj.SetActive(true);
                SoundManager.Instance.CasinoWinSound();
                winAnimationTxt.gameObject.SetActive(true);
                winAnimationTxt.text = "+" + playerWinAmount;
                Invoke(nameof(WinAmountTextOff), 1.5f);
                DataManager.Instance.AddAmount((float)(playerWinAmount), DataManager.Instance.gameId, "AndarBahar-Win-" + DataManager.Instance.gameId, "won", (float)(adminCommission), 2);
            }

            if (otherPrice != 0)
            {
                //otherPrice Direct Profit
            }
            print("playerValue : " + playerValue);
        }
        else if (isAndarWin)
        {
            float andarValue = 0;
            float playerValue = 0;
            float betPrice = 0;
            for (int j = 0; j < andarBaharPlayerList.Count; j++)
            {
                if (andarBaharPlayerList[j].gameObject.activeSelf == true)
                {
                    string playerId = andarBaharPlayerList[j].playerId;
                    for (int i = 0; i < andarBaharPlaceBets.Count; i++)
                    {
                        AndarBaharPlaceBet bet = andarBaharPlaceBets[i];
                        if (bet.playerId.Equals(playerId))
                        {
                            if (bet.isAndar)
                            {
                                andarValue = bet.value;
                            }
                            if (bet.playerId.Equals(DataManager.Instance.playerData._id))
                            {
                                playerValue = andarValue;
                                betPrice = playerValue;
                            }
                        }
                    }

                    // if (andarValue > 0)
                    // {
                    //     GameObject genObj = Instantiate(andarEarnObj, cardGenObj.transform);
                    //     genObj.transform.position = startCard.transform.position;
                    //     float newValue = andarValue * multi_X_Earn;
                    //     print("New Value : " + newValue);
                    //     genObj.transform.GetChild(1).GetComponent<Text>().text = newValue.ToString();
                    //     genObj.transform.DOMove(andarPlayerObj[4].transform.position, 0.4f).OnComplete(() =>
                    //     {
                    //         genObj.transform.GetChild(1).GetComponent<Text>().text = (newValue).ToString();
                    //         genObj.transform.DOMove(andarPlayerObj[4].transform.parent.parent.transform.position, 0.4f);
                    //         //genObj.transform.DOMoveZ(0.00000001f, 0.2f).OnComplete(() =>
                    //         //{
                    //         //    genObj.transform.DOScale(Vector3.zero, 0.2f).OnComplete(() =>
                    //         //    {
                    //         //        //Plaus Value
                    //         //        //Destroy(genObj);
                    //         //    });
                    //         //});
                    //     });
                    //}
                }
            }

            playerValue *= 2;


            float otherPrice = totalInvestAndar + totalInvestBahar;
            float adminPercentage = DataManager.Instance.adminPercentage;




            /*float winAmount = playerValue;
            float adminCommssion = (adminPercentage / 100);
            float playerWinAmount = winAmount - (winAmount * adminCommssion);*/
            float winReward = playerValue - betPrice;
            float adminCommission = adminPercentage / 100;
            float winAmount = winReward - (winReward * adminCommission);
            float playerWinAmount = betPrice + winAmount;

            otherPrice -= playerWinAmount;

            if (playerWinAmount != 0)
            {
                player1.winParticleObj.SetActive(false);
                player1.winParticleObj.SetActive(true);
                SoundManager.Instance.CasinoWinSound();
                winAnimationTxt.gameObject.SetActive(true);
                winAnimationTxt.text = "+" + playerWinAmount;
                Invoke(nameof(WinAmountTextOff), 1.5f);

                DataManager.Instance.AddAmount((float)(playerWinAmount), DataManager.Instance.gameId, "AndarBahar-Win-" + DataManager.Instance.gameId, "won", (float)(adminCommission), 1);
            }

            if (otherPrice != 0)
            {
                //otherPrice Direct Profit
            }

        }

        Invoke(nameof(RoundWinAfterCreate), 4f);
    }


    public void RoundWinAfterCreate()
    {
        player1.winParticleObj.SetActive(false);
        if (isAdmin)
        {

            if (DataManager.Instance.joinPlayerDatas.Count >= TestSocketIO.Instace.andarBaharRequirePlayer)
            {

                //CreateAdmin();
                if (DataManager.Instance.joinPlayerDatas.Count == 1 || isAdmin)
                {
                    RoundGenerate();
                }
                else
                {
                    for (int i = 0; i < andarBaharPlayerList.Count; i++)
                    {
                        andarBaharPlayerList[i].gameObject.SetActive(false);
                    }
                    waitUserNameTxt.text = DataManager.Instance.playerData.firstName;
                    waitNextRoundScreenObj.SetActive(true);
                }
            }
            else
            {
                for (int i = 0; i < andarBaharPlayerList.Count; i++)
                {
                    andarBaharPlayerList[i].gameObject.SetActive(false);
                }
                playerFindUserNameTxt.text = DataManager.Instance.playerData.firstName;
                playerFindScreenObj.SetActive(true);
            }
            //RoundGenerate();aaa
        }
    }

    public void WinAmountTextOff()
    {
        winAnimationTxt.gameObject.SetActive(false);
    }
    
    private void HistoryTacker(int winNo)
    {
        switch (winNo)
        {
            case 1:
            {
                var chip = Instantiate(AndarCard, HistoryCardHolder.transform);
                historyCards.Add(chip);
                var firstObject = historyCards[0];
                historyCards.RemoveAt(0);
                Destroy(firstObject);
                break;
            }
            case 2:
            {
                var chip = Instantiate(BaharCard, HistoryCardHolder.transform);
                historyCards.Add(chip);
                var firstObject = historyCards[0];
                historyCards.RemoveAt(0);
                Destroy(firstObject);
                break;
            }
            default:
            {
                print("No data to track");
                break;
            }
        }
    }
    
    private void UpdateHistoryCard(int num)
    {
        if (!isAdmin) return;
        winList.RemoveAt(0);
        winList.Add(num);
        
        foreach (var t in winList)
        {
            HistoryTacker(t);
        }
        
        //DataManager.Instance.winList = string.Join(",", winList.Select(x => x.ToString()).ToArray());
       // SetWinData(DataManager.Instance.winList);
    }
    
    public void GetUpdatedHistory(string data)
    {
        if (isAdmin) return;
        if(data != "")
        {
            winList = new List<int>(data.Split(',').Select(x => int.Parse(x)));
        }
        
        foreach (var t in winList)
        {
            HistoryTacker(t);
        }
    }

    public void CenterAnimation(CardSuffle suffleData)
    {
        GameObject obj = Instantiate(centerCard, cardGenObj.transform);
        obj.transform.position = startCard.transform.position;
        obj.SetActive(true);
        Vector3 centerPos = centerCard.transform.position;
        obj.transform.DOMove(centerPos, 0.4f).OnComplete(() =>
        {

            obj.transform.DOScale(new Vector3(0, 1, 1), 0.2f).OnComplete(() =>
            {
                obj.transform.GetComponent<Image>().sprite = suffleData.cardSprite;
                obj.transform.DOScale(new Vector3(1, 1, 1), 0.2f).OnComplete(() =>
                {
                    obj.transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.2f).OnComplete(() =>
                    {
                        obj.transform.DOScale(Vector3.one, 0.2f);
                    });
                });
            });
            //obj.transform.DORotate(new Vector3(0, -90, 0), 0.2f).OnComplete(() =>

        });
    }
    #endregion


    #region Button Click

    public void Andar_Plus_ButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        float andarPrice = float.Parse(andarPriceTxt.text);
        if (andarPrice < tableMaxLimit)
        {
            andarPrice += incrementNo;
            andarPriceTxt.text = andarPrice.ToString();
            if (andarPrice == tableMaxLimit)
            {
                andarPlusButton.interactable = false;
                andarMinusButton.interactable = true;
            }
            else if (andarPrice == tableMinLimit)
            {
                andarPlusButton.interactable = true;
                andarMinusButton.interactable = false;
            }
            else
            {
                andarPlusButton.interactable = true;
                andarMinusButton.interactable = true;
            }
        }
    }

    public void Andar_Minus_ButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        float andarPrice = float.Parse(andarPriceTxt.text);
        if (andarPrice > tableMinLimit)
        {
            andarPrice -= incrementNo;
            andarPriceTxt.text = andarPrice.ToString();
            if (andarPrice == tableMaxLimit)
            {
                andarPlusButton.interactable = false;
                andarMinusButton.interactable = true;
            }
            else if (andarPrice == tableMinLimit)
            {
                andarPlusButton.interactable = true;
                andarMinusButton.interactable = false;
            }
            else
            {
                andarPlusButton.interactable = true;
                andarMinusButton.interactable = true;
            }
        }
    }
    public void Andar_ButtonClick()
    {

        if (isPlaceBet)
        {
            //GameObject coinObj = Instantiate(andarCPrefab, cardGenObj.transform);
            //coinObj.transform.position = andarParentPos.transform.position;
            //coinObj.transform.localScale = Vector3.zero;
            //float andarPrice = float.Parse(andarPriceTxt.text);
            ////PlayerSetNo(5, true, false, andarPrice);
            //coinObj.transform.DOScale(Vector3.one, 0.25f);
            //coinObj.transform.DOMove(andarPlayerObj[4].transform.GetChild(0).transform.position, 0.25f).OnComplete(() =>
            //{
            //    for (int i = 0; i < andarPlayerObj.Count; i++)
            //    {
            //        if (i == (5 - 1))
            //        {
            //            andarPlayerObj[i].SetActive(true);
            //            Text t = andarPlayerObj[i].transform.GetChild(1).GetComponent<Text>();
            //            PlaceBet(true, false, andarPrice);

            //            float value1 = float.Parse(t.text);
            //            t.text = (value1 + andarPrice).ToString();
            //            Canvas.ForceUpdateCanvases();
            //        }
            //    }
            //    Destroy(coinObj);
            //});
            float andarPrice = float.Parse(andarPriceTxt.text);
            if (CheckMoney(andarPrice) == false)
            {
                SoundManager.Instance.ButtonClick();
                OpenErrorScreen();
                return;
            }
            SoundManager.Instance.ThreeBetSound();
            totalInvestAndar += andarPrice;
            DataManager.Instance.DebitAmount((andarPrice).ToString(), DataManager.Instance.gameId, "AndarBahar-Bet-" + DataManager.Instance.gameId, "game", 1);

            player1.PlayerGenerateBet(true, false);
        }
    }

    public void Bahar_Plus_ButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        float baharPrice = float.Parse(baharPriceTxt.text);
        if (baharPrice < tableMaxLimit)
        {
            baharPrice += incrementNo;
            baharPriceTxt.text = baharPrice.ToString();
            if (baharPrice == tableMaxLimit)
            {
                baharPlusButton.interactable = false;
                baharMinusButton.interactable = true;
            }
            else if (baharPrice == tableMinLimit)
            {
                baharPlusButton.interactable = true;
                baharMinusButton.interactable = false;
            }
            else
            {
                baharPlusButton.interactable = true;
                baharMinusButton.interactable = true;
            }
        }
    }

    public void Bahar_Minus_ButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        float baharPrice = float.Parse(baharPriceTxt.text);
        if (baharPrice > tableMinLimit)
        {
            baharPrice -= incrementNo;
            baharPriceTxt.text = baharPrice.ToString();
            if (baharPrice == tableMaxLimit)
            {
                baharPlusButton.interactable = false;
                baharMinusButton.interactable = true;
            }
            else if (baharPrice == tableMinLimit)
            {
                baharPlusButton.interactable = true;
                baharMinusButton.interactable = false;
            }
            else
            {
                baharPlusButton.interactable = true;
                baharMinusButton.interactable = true;
            }
        }
    }
    public void Bahar_ButtonClick()
    {
        if (isPlaceBet)
        {
            //GameObject coinObj = Instantiate(baharCPrefab, cardGenObj.transform);
            //coinObj.transform.position = baharParentPos.transform.position;
            //coinObj.transform.localScale = Vector3.zero;
            //float baharPrice = float.Parse(baharPriceTxt.text);
            ////PlayerSetNo(5, true, false, andarPrice);
            //coinObj.transform.DOScale(Vector3.one, 0.25f);
            //coinObj.transform.DOMove(baharPlayerObj[4].transform.GetChild(0).transform.position, 0.25f).OnComplete(() =>
            //{
            //    for (int i = 0; i < baharPlayerObj.Count; i++)
            //    {
            //        if (i == (5 - 1))
            //        {
            //            baharPlayerObj[i].SetActive(true);
            //            Text t = baharPlayerObj[i].transform.GetChild(1).GetComponent<Text>();
            //            PlaceBet(false, true, baharPrice);
            //            float value1 = float.Parse(t.text);
            //            t.text = (value1 + baharPrice).ToString();
            //            Canvas.ForceUpdateCanvases();
            //        }
            //    }
            //    Destroy(coinObj);
            //});
            float baharPrice = float.Parse(baharPriceTxt.text);
            if (CheckMoney(baharPrice) == false)
            {
                SoundManager.Instance.ButtonClick();
                OpenErrorScreen();
                return;
            }


            SoundManager.Instance.ThreeBetSound();
            totalInvestBahar += baharPrice;
            DataManager.Instance.DebitAmount((baharPrice).ToString(), DataManager.Instance.gameId, "AndarBahar-Bet-" + DataManager.Instance.gameId, "game", 2);
            player1.PlayerGenerateBet(false, true);


        }
    }



    public void PlaceBet(bool isAndar, bool isBahar, float value, string playerID)
    {
        AndarBaharPlaceBet placeBet = new AndarBaharPlaceBet();
        placeBet.isAndar = isAndar;
        placeBet.isBahar = isBahar;
        placeBet.playerId = playerID;
        placeBet.value = value;

        bool isCheck = false;
        for (int i = 0; i < andarBaharPlaceBets.Count; i++)
        {
            AndarBaharPlaceBet bet = andarBaharPlaceBets[i];
            if (bet.playerId.Equals(placeBet.playerId))
            {
                if (bet.isAndar == placeBet.isAndar)
                {
                    isCheck = true;
                    bet.value += placeBet.value;
                }
                else if (bet.isBahar == placeBet.isBahar)
                {
                    isCheck = true;
                    bet.value += placeBet.value;
                }
            }
        }

        if (!isCheck)
        {
            andarBaharPlaceBets.Add(placeBet);
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


    #endregion


    #region Other Button

    public void MenuButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        OpenMenuScreen();
    }

    #endregion

    #region Menu Screen

    void OpenMenuScreen()
    {
        menuScreenObj.SetActive(true);
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

    #region Other

    public void GiftButtonClick(AndarBaharPlayer giftPlayer)
    {
        SoundManager.Instance.ButtonClick();
        giftScreenObj.SetActive(true);
        GiftSendManager.Instance.gameName = "AndarBahar";
        GiftSendManager.Instance.andarBaharOtherPlayer = giftPlayer;
    }

    public void MessageButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        messageScreeObj.SetActive(true);
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

    #region Admin Maintain

    void CreateAdmin()
    {
        if (DataManager.Instance.joinPlayerDatas[0].userId.Equals(DataManager.Instance.playerData._id))
        {
            print("Enter Room Admin");
            isAdmin = true;
            //SetRoomData();
        }
    }

    public void SetRoomData()
    {
        print("Enter The Room Data Set");
        JSONObject obj = new JSONObject();
        int dNo = UnityEngine.Random.Range(0, 300);
        deckNo = dNo;
        obj.AddField("DeckNo", dNo);
        //obj.AddField("DeckNo", 300);
        obj.AddField("dateTime", DateTime.UtcNow.ToString());
        obj.AddField("gameMode", 3);
        //obj.AddField("WinList", DataManager.Instance.winList);
        TestSocketIO.Instace.SetRoomdata(TestSocketIO.Instace.roomid, obj);
    }
    
    public void SetWinData(string winListData)
    {
        JSONObject obj = new JSONObject();
        obj.AddField("WinList", winListData);
        //obj.AddField("DeckNo", 300);
        TestSocketIO.Instace.SetRoomdata(TestSocketIO.Instace.roomid, obj);
    }
    
    public void SetTempNo(int tempNo, bool isRight, bool isLeft)
    {
        JSONObject obj = new JSONObject();
        obj.AddField("PlayerID", DataManager.Instance.playerData._id);
        obj.AddField("TournamentID", DataManager.Instance.tournamentID);
        obj.AddField("RoomId", TestSocketIO.Instace.roomid);
        obj.AddField("TempNo", tempNo);
        obj.AddField("Right", isRight);
        obj.AddField("Left", isLeft);
        TestSocketIO.Instace.Senddata("AndarBaharTempNo", obj);
    }

    public void GetRoomData(int no, string data)
    {
        print("Get Room Data");
        if (!isAdmin)
        {
            deckNo = no;
            RoundGenerate();
            if (waitNextRoundScreenObj.activeSelf == true)
            {
                waitNextRoundScreenObj.SetActive(false);
            }
            
            HistoryLoader(data);
        }
    }

    public void GetTempNo(int tempNo, bool isRight, bool isLeft)
    {
        if (isAdmin) return;
        isPlaceBet = false;

        if (isAndarWin == false && isBaharWin == false)
        {
            if (isRight)
            {
                RightAnimation(cardSufflesGen[tempNo]);
            }
            else if (isLeft)
            {
                LeftAnimation(cardSufflesGen[tempNo]);
            }
        }
    }
    

    public void ChangeAAdmin(string leavePlayerId, string adminId)
    {

        if (DataManager.Instance.playerData._id.Equals(DataManager.Instance.joinPlayerDatas[0].userId))
        {
            isAdmin = true;

            if (DataManager.Instance.joinPlayerDatas.Count == 1 && waitNextRoundScreenObj.activeSelf == true)
            {
                RoundGenerate();
                if (waitNextRoundScreenObj.activeSelf == true)
                {
                    waitNextRoundScreenObj.SetActive(false);
                }
            }
        }
        else
        {
            isAdmin = false;
        }



        for (int i = 0; i < andarBaharPlayerList.Count; i++)
        {
            if (andarBaharPlayerList[i].playerId.Equals(leavePlayerId))
            {
                andarBaharPlayerList[i].gameObject.SetActive(false);
            }
        }

        for (int i = 0; i < andarBaharPlayerList.Count; i++)
        {
            if (andarBaharPlayerList[i].gameObject.activeSelf == true)
            {
                string playerIdGet = andarBaharPlayerList[i].playerId;

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
                    andarBaharPlayerList[i].gameObject.SetActive(false);
                }
            }
        }
        
    }

    public void UserDataMaintain()
    {

        bool isSetAdmin = false;

        for (int i = 0; i < andarBaharPlayerList.Count; i++)
        {
            andarBaharPlayerList[i].gameObject.SetActive(false);
        }
        if (DataManager.Instance.joinPlayerDatas[0].userId.Equals(DataManager.Instance.playerData._id))
        {
            isAdmin = true;
            isSetAdmin = true;
            player1.playerNo = 1;
            player1.playerId = DataManager.Instance.joinPlayerDatas[0].userId;
            player1.tournamentId = DataManager.Instance.joinPlayerDatas[0].lobbyId;
        }
        int cntTmp = 1;
        for (int i = 0; i < DataManager.Instance.joinPlayerDatas.Count; i++)
        {
            if ((DataManager.Instance.joinPlayerDatas[i].userId == DataManager.Instance.playerData._id))
            {
                if (i == 0)
                {
                    isAdmin = true;
                }
                andarBaharPlayerList[0].gameObject.SetActive(true);
                andarBaharPlayerList[0].playerNo = i + 1;
                andarBaharPlayerList[0].playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                andarBaharPlayerList[0].tournamentId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
            }
            else
            {
                andarBaharPlayerList[cntTmp].gameObject.SetActive(true);
                andarBaharPlayerList[cntTmp].playerNo = i + 1;
                andarBaharPlayerList[cntTmp].playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                andarBaharPlayerList[cntTmp].tournamentId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                cntTmp++;
            }
        }
    }


    #endregion

    #region Socket Bet

    public void SendBet(bool isAndar, bool isBahar, float value)
    {
        JSONObject obj = new JSONObject();
        obj.AddField("PlayerID", DataManager.Instance.playerData._id);
        obj.AddField("TournamentID", DataManager.Instance.tournamentID);
        obj.AddField("RoomId", TestSocketIO.Instace.roomid);
        obj.AddField("isAndar", isAndar);
        obj.AddField("isBahar", isBahar);
        obj.AddField("value", value);
        TestSocketIO.Instace.Senddata("AndarBaharBetData", obj);
    }

    public void GetBet(bool isAndar, bool isBahar, string betPlayerId, float value)
    {
        for (int i = 0; i < andarBaharPlayerList.Count; i++)
        {
            if (andarBaharPlayerList[i].gameObject.activeSelf)
            {
                if (andarBaharPlayerList[i].playerId.Equals(betPlayerId))
                {
                    andarBaharPlayerList[i].GetSocketBet(isAndar, isBahar, value);
                }
            }
        }
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

        //print("Send Id : " + sendPlayerID);
        //print("Receive Id : " + receivePlayerId);
        for (int i = 0; i < andarBaharPlayerList.Count; i++)
        {
            if (andarBaharPlayerList[i].playerId == sendPlayerID)
            {
                sendPlayerObj = andarBaharPlayerList[i].gameObject;
            }
            else if (andarBaharPlayerList[i].playerId == receivePlayerId)
            {
                receivePlayerObj = andarBaharPlayerList[i].gameObject;
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

    #endregion

    #region WinnerDecider
    

    public void IncrementAndar()
    {
        //Bahar win
        if (cardSufflesGen[0].cardNo == cardSufflesGen[cardCnt].cardNo) 
        {
            _tempBaharNum = cardCnt;
            _isBaharActive = true;
            if (totalInvestAndar > totalInvestBahar)
            {
                cardCnt += 2;
            }
        }
    }
    public void IncrementBahar()
    {
        //Andar win
        if (cardSufflesGen[0].cardNo == cardSufflesGen[cardCnt].cardNo)
        {
            _tempAndarNum = cardCnt;
            _isAndarActive = true;
            if (totalInvestAndar < totalInvestBahar)
            {
                cardCnt += 2;
            }
        }
    }

    #endregion
}
public enum CardColorType
{
    Clubs = 1,
    Diamonds = 2,
    Spades = 3,
    Hearts = 4

}
[System.Serializable]
public class CardSuffle
{
    public int cardNo;
    public CardColorType color;//1234-1-fulli,2-red cerkat,3-black,4-red heart
    public Sprite cardSprite;
}
[System.Serializable]
public class ListStoreData
{
    public List<int> noList = new List<int>();
}

[System.Serializable]
public class AndarBaharPlaceBet
{
    public bool isAndar;
    public bool isBahar;
    public string playerId;
    public float value;
}
