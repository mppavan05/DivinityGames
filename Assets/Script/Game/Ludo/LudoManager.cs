//using MoreMountains.NiceVibrations;
using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LudoManager : MonoBehaviour
{


    public List<int> safeNoBotList = new List<int>();
    public static LudoManager Instance;
    public List<GameObject> numberObj;
    public List<GameObject> numberObj2;
    public List<GameObject> numberObj3;
    public List<GameObject> numberObj4;

    public List<int> orgListNo2 = new List<int>();
    public List<int> orgListNo3 = new List<int>();
    public List<int> orgListNo4 = new List<int>();

    public Sprite[] diceSprite;

    public List<PasaManage> pasaSocketList = new List<PasaManage>();
    public List<PasaManage> currentPlayerPasaList = new List<PasaManage>();

    public List<int> safeNo;
    public List<GameObject> pasaObjects = new List<GameObject>();

    public int playerRoundChecker;
    public Text winAmount;
    

    public Image pasaImage;
    public Sprite[] pasaSprite;

    public int currentPlayerNo;
    public int rollingPlayer;

    public int ActivePlayer;

    public int pasaCurrentNo = 1;

    public GameObject generatePasaFireParticles;

    public List<PasaManage> pasaCollectList = new List<PasaManage>();

    public List<PasaManage> pasaBotPlayer = new List<PasaManage>();

    public Image[] subPasaParentImg;

    public Image box1Img;
    public Image box1LineImg;
    public Image box2Img;
    public Image box2LineImg;
    public Image box3Img;
    public Image box3LineImg;
    public Image box4Img;
    public Image box4LineImg;
    public Image[] box1Token;
    public Image[] box2Token;
    public Image[] box3Token;
    public Image[] box4Token;
    public Image box1CircleImg;
    public Image box2CircleImg;
    public Image box3CircleImg;
    public Image box4CircleImg;

    public Image[] box1Lifes;
    public Image[] box2Lifes;
    public Image[] box3Lifes;
    public Image[] box4Lifes;

    public Color lifeOnColor;
    public Color lifeOffColor;


    public Sprite blueBoxSprite;
    public Sprite blueBoxLineSprite;
    public Sprite redBoxSprite;
    public Sprite redBoxLineSprite;
    public Sprite greenBoxSprite;
    public Sprite greenBoxLineSprite;
    public Sprite yellowBoxSprite;
    public Sprite yellowBoxLineSprite;

    public Sprite blueToken;
    public Sprite redToken;
    public Sprite greenToken;
    public Sprite yellowToken;
    public Sprite blueCircleSprite;
    public Sprite redCircleSprite;
    public Sprite greenCircleSprite;
    public Sprite yellowCircleSprite;

    public Text player1Txt;
    public Text player2Txt;
    public Text player3Txt;
    public Text player4Txt;

    public Text[] playerScores;

    public int playerScoreCnt1;
    public int playerScoreCnt2;
    public int playerScoreCnt3;
    public int playerScoreCnt4;

    public GameObject score56Anim1;
    public GameObject score56Anim2;
    public GameObject score56Anim3;
    public GameObject score56Anim4;

    public Animator shadow1;
    public Animator shadow2;
    public Animator shadow3;
    public Animator shadow4;


    public Image timerFillImg;
    public float timerSpeed;

    public int isClickAvaliableDice;
    public Color shadowOff;
    float secondsCount = 0;
    int flag = 0;
    public Text timerTxt;
    public GameObject winScreen;

    public bool isPathClick;
    public bool isOtherPlayLeft;
    public bool isTimeFinish;

    public bool isPauseGetData;
    bool isPathClickAvaliable;

    public Color blueColor;
    public Color redColor;
    public Color greenColor;
    public Color yellowColor;

    public Sprite[] profileSprite;

    public bool isOpenWin;
    public Text pasaNoTxt;
    public Color colorOff;
    public Color colorOn;

    public GameObject turnGenObj;
    public GameObject turnObj;

    public bool isCheckEnter = false;
    bool isEntered = false;
    public int botPlayerNo = 0;



    private void Awake()
    {
        StartGamePlay();
    }

    public void GenerateTurnObj()
    {
        Instantiate(turnObj, turnGenObj.transform);

    }


    public void PlayerNameManage()
    {
        if (DataManager.Instance.isTwoPlayer == true)
        {
            //int index = DataManager.Instance.playerNo;
            int index1 = 0;
            int index2 = 1;


            if (DataManager.Instance.playerNo == 3)
            {
                index1 = 1;
                index2 = 0;

            }

            //print("Index 1 : " + index1);
            //print("Index 2 : " + index2);
            player1Txt.text = UserNameStringManage(DataManager.Instance.joinPlayerDatas[index1].userName);
            player3Txt.text = UserNameStringManage(DataManager.Instance.joinPlayerDatas[index2].userName);


            //subPasaParentImg[0].sprite = profileSprite[DataManager.Instance.joinPlayerDatas[index1].avtar];
            //subPasaParentImg[2].sprite = profileSprite[DataManager.Instance.joinPlayerDatas[index2].avtar];

            Image img1 = subPasaParentImg[0];//.transform.GetChild(0).GetComponent<Image>();
            Image img2 = subPasaParentImg[2];//.transform.GetChild(0).GetComponent<Image>();
            StartCoroutine(DataManager.Instance.GetImages(DataManager.Instance.joinPlayerDatas[index1].avtar, img1));
            StartCoroutine(DataManager.Instance.GetImages(DataManager.Instance.joinPlayerDatas[index2].avtar, img2));

        }
        else if (DataManager.Instance.isFourPlayer)
        {
            int index1 = 0;
            int index2 = 1;
            int index3 = 2;
            int index4 = 3;
            
            player1Txt.text = UserNameStringManage(DataManager.Instance.joinPlayerDatas[index1].userName);
            player2Txt.text = UserNameStringManage(DataManager.Instance.joinPlayerDatas[index2].userName);
            player3Txt.text = UserNameStringManage(DataManager.Instance.joinPlayerDatas[index3].userName);
            player4Txt.text = UserNameStringManage(DataManager.Instance.joinPlayerDatas[index4].userName);

            Image img1 = subPasaParentImg[0];//.transform.GetChild(0).GetComponent<Image>();
            Image img2 = subPasaParentImg[1];//.transform.GetChild(0).GetComponent<Image>();
            Image img3 = subPasaParentImg[2];//.transform.GetChild(0).GetComponent<Image>();
            Image img4 = subPasaParentImg[3];//.transform.GetChild(0).GetComponent<Image>();
            StartCoroutine(DataManager.Instance.GetImages(DataManager.Instance.joinPlayerDatas[index1].avtar, img1));
            StartCoroutine(DataManager.Instance.GetImages(DataManager.Instance.joinPlayerDatas[index2].avtar, img2));
            StartCoroutine(DataManager.Instance.GetImages(DataManager.Instance.joinPlayerDatas[index3].avtar, img3));
            StartCoroutine(DataManager.Instance.GetImages(DataManager.Instance.joinPlayerDatas[index4].avtar, img4));
        }
    }
    public string UserNameStringManage(string name)
    {
        if (name != null && name != "")
        {
            if (name.Length > 13)
            {
                name = name.Substring(0, 10) + "...";
            }
            else
            {
                name = name;
            }
        }
        return name;
    }


    void Timer()
    {
        secondsCount -= Time.deltaTime;
        float minutes = Mathf.Floor(secondsCount / 60);
        float seconds = secondsCount % 60;


        string Min = minutes.ToString();
        string Sec = Mathf.RoundToInt(seconds).ToString();
        if (Min.Length == 1)
        {
            Min = "0" + Min;
        }
        if (Sec.Length == 1)
        {
            Sec = "0" + Sec;
        }
        if (Min.Length != 1 && Sec.Length != 1)
        {
            Min = Min;
            Sec = Sec;
        }

        string timeValue = Min + ":" + Sec;
        if (timeValue.Equals("00:00"))
        {
            print("Time Over");
            timerTxt.text = "00:00";
            WinUserShow();
            flag = 1;
        }
        if (flag != 1)
        {
            timerTxt.text = timeValue;
        }
    }


    public void WinUserShow()
    {
        winScreen.SetActive(true);
    }

    public void ClearAllData()
    {
        pasaSocketList.Clear();
        currentPlayerPasaList.Clear();
        pasaObjects.Clear();
    }


    #region Start Player Manage

    public void PlayerJoined()
    {
        ClearAllData();
        RestartTimer();
        StartGamePlay();
        pasaCurrentNo = UnityEngine.Random.Range(1, 7);
        pasaImage.sprite = pasaSprite[pasaCurrentNo - 1];
        PlayerNameManage();
        SoundManager.Instance.CasinoTurnSound();
    }

    public void StartGamePlay()
    {
        secondsCount = (TestSocketIO.Instace.playTime * 60);
        //secondsCount = (1 * 60);
        if (Instance == null)
        {
            Instance = this;
        }
        GameInIt();
        currentPlayerNo = DataManager.Instance.playerNo;
        if (DataManager.Instance.playerNo == 1)
        {
            playerRoundChecker = 1;
            ActivePlayer = 1;
            rollingPlayer = 1;
            DataManager.Instance.isDiceClick = true;
        }
        else
        {
            switch (DataManager.Instance.playerNo)
            {
                case 2:
                {
                    playerRoundChecker = 2;
                    ActivePlayer = 2;
                    rollingPlayer = 2;
                    DataManager.Instance.isDiceClick = true;
                    break;
                }
                case 3:
                {
                    playerRoundChecker = 3;
                    ActivePlayer = 3;
                    rollingPlayer = 3;
                    DataManager.Instance.isDiceClick = true;
                    break;
                }
                case 4:
                {
                    playerRoundChecker = 4;
                    ActivePlayer = 4;
                    rollingPlayer = 4;
                    DataManager.Instance.isDiceClick = true;
                    break;
                }
            }
            //ActivePlayer = 3;
            //print("Enter First Enter Bot");
           // GenerateDiceNumberStart_Bot(true);
        }
        playerRoundChecker = 1;
        ActivePlayer = 1;
        rollingPlayer = 1;
        DataManager.Instance.isDiceClick = true;


        playerScores[0].text = playerScoreCnt1.ToString();
        playerScores[1].text = playerScoreCnt2.ToString();
        playerScores[2].text = playerScoreCnt3.ToString();
        playerScores[3].text = playerScoreCnt4.ToString();

        score56Anim1.SetActive(false);
        score56Anim2.SetActive(false);
        score56Anim3.SetActive(false);
        score56Anim4.SetActive(false);

        timerFillImg.color = blueColor;


        if (DataManager.Instance.modeType == 1 || DataManager.Instance.modeType == 2)
        {
            pasaImage.color = colorOn;
            pasaNoTxt.color = colorOff;
        }
        else
        {
            pasaImage.color = colorOff;
            pasaNoTxt.color = colorOn;
        }

    }

    void GameInIt()
    {
        if (DataManager.Instance.isTwoPlayer)
        {
            for (int i = 0; i < box2Img.gameObject.transform.childCount; i++)
            {
                box2Img.gameObject.transform.GetChild(i).gameObject.SetActive(false);
            }
            for (int i = 0; i < box4Img.gameObject.transform.childCount; i++)
            {
                box4Img.gameObject.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
       
        if (DataManager.Instance.playerNo == 1)
        {
            box1Img.sprite = blueBoxSprite;
            box1LineImg.sprite = blueBoxLineSprite;
            SetTokenImages(box1Token, blueToken);
            box1CircleImg.sprite = blueCircleSprite;

            box2Img.sprite = redBoxSprite;
            box2LineImg.sprite = redBoxLineSprite;
            SetTokenImages(box2Token, redToken);
            box2CircleImg.sprite = redCircleSprite;

            box3Img.sprite = greenBoxSprite;
            box3LineImg.sprite = greenBoxLineSprite;
            SetTokenImages(box3Token, greenToken);
            box3CircleImg.sprite = greenCircleSprite;

            box4Img.sprite = yellowBoxSprite;
            box4LineImg.sprite = yellowBoxLineSprite;
            SetTokenImages(box4Token, yellowToken);
            box4CircleImg.sprite = yellowCircleSprite;

            shadow1.enabled = true;
            shadow2.enabled = false;
            shadow3.enabled = false;
            shadow4.enabled = false;
        }
        else if (DataManager.Instance.playerNo == 2)
        {
            box1Img.sprite = redBoxSprite;
            box1LineImg.sprite = redBoxLineSprite;
            SetTokenImages(box1Token, redToken);
            box1CircleImg.sprite = redCircleSprite;

            box2Img.sprite = greenBoxSprite;
            box2LineImg.sprite = greenBoxLineSprite;
            SetTokenImages(box2Token, greenToken);
            box2CircleImg.sprite = greenCircleSprite;

            box3Img.sprite = yellowBoxSprite;
            box3LineImg.sprite = yellowBoxLineSprite;
            SetTokenImages(box3Token, yellowToken);
            box3CircleImg.sprite = yellowCircleSprite;

            box4Img.sprite = blueBoxSprite;
            box4LineImg.sprite = blueBoxLineSprite;
            SetTokenImages(box3Token, blueToken);
            box4CircleImg.sprite = blueCircleSprite;

            shadow1.enabled = false;
            shadow2.enabled = true;
            shadow3.enabled = false;
            shadow4.enabled = false;
        }
        else if (DataManager.Instance.playerNo == 3)
        {
            box1Img.sprite = greenBoxSprite;
            box1LineImg.sprite = greenBoxLineSprite;
            SetTokenImages(box1Token, greenToken);
            box1CircleImg.sprite = greenCircleSprite;

            box2Img.sprite = yellowBoxSprite;
            box2LineImg.sprite = yellowBoxLineSprite;
            SetTokenImages(box2Token, yellowToken);
            box2CircleImg.sprite = yellowCircleSprite;

            box3Img.sprite = blueBoxSprite;
            box3LineImg.sprite = blueBoxLineSprite;
            SetTokenImages(box3Token, blueToken);
            box3CircleImg.sprite = blueCircleSprite;

            box4Img.sprite = redBoxSprite;
            box4LineImg.sprite = redBoxLineSprite;
            SetTokenImages(box4Token, redToken);
            box4CircleImg.sprite = redCircleSprite;

            shadow1.enabled = false;
            shadow2.enabled = false;
            shadow3.enabled = true;
            shadow4.enabled = false;
        }
        else if (DataManager.Instance.playerNo == 4)
        {
            box1Img.sprite = yellowBoxSprite;
            box1LineImg.sprite = yellowBoxLineSprite;
            SetTokenImages(box1Token, yellowToken);
            box1CircleImg.sprite = yellowCircleSprite;

            box2Img.sprite = blueBoxSprite;
            box2LineImg.sprite = blueBoxLineSprite;
            SetTokenImages(box2Token, blueToken);
            box2CircleImg.sprite = blueCircleSprite;

            box3Img.sprite = redBoxSprite;
            box3LineImg.sprite = redBoxLineSprite;
            SetTokenImages(box3Token, redToken);
            box3CircleImg.sprite = redCircleSprite;

            box4Img.sprite = greenBoxSprite;
            box4LineImg.sprite = greenBoxLineSprite;
            SetTokenImages(box4Token, greenToken);
            box4CircleImg.sprite = greenCircleSprite;

            shadow1.enabled = false;
            shadow2.enabled = false;
            shadow3.enabled = false;
            shadow4.enabled = true;
        }
    }

    void SetTokenImages(Image[] img, Sprite token)
    {
        for (int i = 0; i < img.Length; i++)
        {
            img[i].sprite = token;
        }
    }

    #endregion


    #region Pasa Image Manage

    //public void PasaImageManage(int no, int pNo, bool isSocket)
    //{


    //    subPasaParentImg[pNo - 1].sprite = diceSprite[no - 1];

    //    for (int i = 0; i < subPasaParentImg.Length; i++)
    //    {
    //        if ((pNo - 1) == i)
    //        {
    //            subPasaParentImg[i].GetComponent<Button>().interactable = true;
    //        }
    //        else
    //        {
    //            subPasaParentImg[i].GetComponent<Button>().interactable = false;
    //        }
    //    }
    //}



    #endregion


    #region Score Manage

    public void ScoreManage(int pNo, int plusNumber)
    {
        
        if (pNo == 1)
        {
            playerScoreCnt1 = playerScoreCnt1 + plusNumber;
            playerScores[0].text = playerScoreCnt1.ToString();
            if (plusNumber > 50)
            {
                score56Anim1.SetActive(true);
                StartCoroutine(OffScore56(score56Anim1));
            }
        }
        else if (pNo == 2)
        {
            playerScoreCnt2 = playerScoreCnt2 + plusNumber;
            playerScores[1].text = playerScoreCnt2.ToString();
            if (plusNumber > 50)
            {
                score56Anim2.SetActive(true);
                StartCoroutine(OffScore56(score56Anim2));
            }
        }
        else if (pNo == 3)
        {
            playerScoreCnt3 = playerScoreCnt3 + plusNumber;
            playerScores[2].text = playerScoreCnt3.ToString();
            if (plusNumber > 50)
            {
                score56Anim3.SetActive(true);
                StartCoroutine(OffScore56(score56Anim3));
            }
        }
        else if (pNo == 4)
        {
            playerScoreCnt4 = playerScoreCnt4 + plusNumber;
            playerScores[3].text = playerScoreCnt4.ToString();
            if (plusNumber > 50)
            {
                score56Anim4.SetActive(true);
                StartCoroutine(OffScore56(score56Anim4));
            }
        }
    }

    public void ScoreManageDecrese(int pNo, int decreseNumber)
    {
        if (pNo == 1)
        {
            playerScoreCnt1 = playerScoreCnt1 - decreseNumber;
            if (playerScoreCnt1 <= 0)
            {
                playerScoreCnt1 = 0;
            }
            playerScores[0].text = playerScoreCnt1.ToString();
        }
        else if (pNo == 2)
        {
            playerScoreCnt2 = playerScoreCnt2 - decreseNumber;
            if (playerScoreCnt2 <= 0)
            {
                playerScoreCnt2 = 0;
            }
            playerScores[1].text = playerScoreCnt2.ToString();
        }
        else if (pNo == 3)
        {
            playerScoreCnt3 = playerScoreCnt3 - decreseNumber;
            if (playerScoreCnt3 <= 0)
            {
                playerScoreCnt3 = 0;
            }
            playerScores[2].text = playerScoreCnt3.ToString();
        }
        else if (pNo == 4)
        {
            playerScoreCnt4 = playerScoreCnt4 - decreseNumber;
            if (playerScoreCnt4 <= 0)
            {
                playerScoreCnt4 = 0;
            }
            playerScores[3].text = playerScoreCnt4.ToString();
        }
    }

    IEnumerator OffScore56(GameObject obj56)
    {
        yield return new WaitForSeconds(2f);
        obj56.SetActive(false);
    }

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        AddBetAmount();
        winAmount.text = "₹ " + DataManager.Instance.winAmount;
        pasaCurrentNo = UnityEngine.Random.Range(1, 7);
        pasaImage.sprite = pasaSprite[pasaCurrentNo - 1];
        SoundManager.Instance.StopBackgroundMusic();
        PlayerNameManage();
        SoundManager.Instance.CasinoTurnSound();
    }
    
    public void AddBetAmount()
    {
        DataManager.Instance.DebitAmount((DataManager.Instance.betPrice).ToString(), DataManager.Instance.gameId, "Ludo-Bet-" + DataManager.Instance.gameId, "game", 0);
    }

    bool isTimeEnter = false;
    public void RestartTimer()
    {
        for (int j = 0; j < currentPlayerPasaList.Count; j++)
        {
            currentPlayerPasaList[j].isReadyForClick = false;
        }
        isTimeEnter = false;
        DataManager.Instance.isRestartManage = true;
        timerFillImg.fillAmount = 1;
        //print("Restart Timer : ");
    }

    public void PlayerChangeTurn()
    {
        //print("Enter The Player Change");
        print(ActivePlayer + "<- this is current player");

        SoundManager.Instance.UserTurnSound();
        DataManager.Instance.isDiceClick = false;
        DataManager.Instance.isTimeAuto = false;
        PlayerDiceChange();
        OtherShadowMainTain();
        RestartTimer();
    }

    public void ReconnectPasaChange()
    {
        //print("Enter The Player Change");
        //    DataManager.Instance.isDiceClick = true;
        //    DataManager.Instance.isTimeAuto = false;
        ////    PlayerDiceChange();
        //    OurShadowMainTain();
        //    RestartTimer();
        print("this is clicked in ReconnectPasaChange");

        DataManager.Instance.isDiceClick = true;
        isClickAvaliableDice = 0;
        OurShadowMaintain();
        DataManager.Instance.isTimeAuto = false;
        RestartTimer();
    }


    bool isLifeEnter = false;
    // Update is called once per frame
    void Update()
    {
        if (isOpenWin == false)
        {
            Timer();
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //LudoManager.Instance.GeneratePasaFire();
                //if (pasaCollectList.Count == 1)
                //{
                //    SoundManager.Instance.TokenKillSound();
                //    pasaCollectList[0].Move_Decrement_Steps();
                //}

                //PlayerChangeTurn();
                //RestartTimer();
            }

            if (true)
            {
                timerFillImg.fillAmount -= 1.0f / timerSpeed * Time.deltaTime;
                if (timerFillImg.fillAmount == 0 && DataManager.Instance.isTimeAuto == false)
                {
                    if (isTimeEnter == true)
                    {
                        LifeDecrease();
                    }
                }
                else if (timerFillImg.fillAmount < 0.5f && isTimeEnter == false && DataManager.Instance.isTimeAuto == false)
                {
                    isLifeEnter = false;
                    isTimeEnter = true;
                    if (DataManager.Instance.isDiceClick)
                    {
                        TickSound();
                    }
                }
            }
        }
    }
    int cntPlayer1 = 0;
    int cntPlayer2 = 0;
    int cntPlayer3 = 0;
    int cntPlayer4 = 0;
    void LifeDecrease()
    {
        SoundManager.Instance.TickTimerStop();
        if (DataManager.Instance.isTwoPlayer && isLifeEnter == false)
        {
            isLifeEnter = true;
            if (DataManager.Instance.isDiceClick == true)
            {
                SoundManager.Instance.TimeOutSound();
                if (cntPlayer1 == 2)
                {
                    isTimeFinish = true;
                    isOtherPlayLeft = false;
                    WinUserShow();
                }
                else
                {
                    box1Lifes[cntPlayer1].color = lifeOffColor;
                    cntPlayer1++;
                }
                if (BotManager.Instance.isConnectBot)
                {
                    BotChangeTurn(false, true);
                }
                else
                {
                    PlayerChangeTurn();
                }
            }
            else if (DataManager.Instance.isDiceClick == false && DataManager.Instance.isTimeAuto == false)
            {
                if (cntPlayer2 == 2)
                {
                    isOtherPlayLeft = true;
                    WinUserShow();
                }
                else
                {
                    box3Lifes[cntPlayer2].color = lifeOffColor;
                    cntPlayer2++;
                    ReconnectPasaChange();
                }
            }
            // print("Enter The Life is Decrese");
        }
        else if (DataManager.Instance.isFourPlayer && isLifeEnter == false)
        {
            isLifeEnter = true;
            switch (DataManager.Instance.isDiceClick)
            {
                case true:
                {
                    SoundManager.Instance.TimeOutSound();
                    switch (playerRoundChecker)
                    {
                        case 1 when cntPlayer1 == 2:
                            isTimeFinish = true;
                            isOtherPlayLeft = false;
                            WinUserShow();
                            break;
                        case 1:
                            box1Lifes[cntPlayer1].color = lifeOffColor;
                            cntPlayer1++;
                            break;
                        case 2 when cntPlayer2 == 2:
                            isTimeFinish = true;
                            isOtherPlayLeft = false;
                            WinUserShow();
                            break;
                        case 2:
                            box2Lifes[cntPlayer2].color = lifeOffColor;
                            cntPlayer2++;
                            break;
                        case 3 when cntPlayer3 == 2:
                            isTimeFinish = true;
                            isOtherPlayLeft = false;
                            WinUserShow();
                            break;
                        case 3:
                            box3Lifes[cntPlayer3].color = lifeOffColor;
                            cntPlayer3++;
                            break;
                        case 4 when cntPlayer4 == 2:
                            isTimeFinish = true;
                            isOtherPlayLeft = false;
                            WinUserShow();
                            break;
                        case 4:
                            box4Lifes[cntPlayer4].color = lifeOffColor;
                            cntPlayer4++;
                            break;
                    }
                    if (BotManager.Instance.isConnectBot)
                    {
                        BotChangeTurn(false, true);
                    }
                    else
                    {
                        PlayerChangeTurn();
                    }

                    break;
                }
                case false when DataManager.Instance.isTimeAuto == false:
                {
                    switch (playerRoundChecker)
                    {
                        // if (cntPlayer2 == 3)
                        // {
                        //     isOtherPlayLeft = true;
                        //     WinUserShow();
                        // }
                        // else
                        // {
                        //     box3Lifes[cntPlayer2].color = lifeOffColor;
                        //     cntPlayer2++;
                        //     ReconnectPasaChange();
                        // }
                        case 1 when cntPlayer1 == 2:
                            isOtherPlayLeft = true;
                            WinUserShow();
                            break;
                        case 1:
                            box1Lifes[cntPlayer1].color = lifeOffColor;
                            cntPlayer1++;
                            ReconnectPasaChange();
                            break;
                        case 2 when cntPlayer2 == 2:
                            isOtherPlayLeft = true;
                            WinUserShow();
                            break;
                        case 2:
                            box2Lifes[cntPlayer2].color = lifeOffColor;
                            cntPlayer2++;
                            ReconnectPasaChange();
                            break;
                        case 3 when cntPlayer3 == 2:
                            isOtherPlayLeft = true;
                            WinUserShow();
                            break;
                        case 3:
                            box3Lifes[cntPlayer3].color = lifeOffColor;
                            cntPlayer3++;
                            ReconnectPasaChange();
                            break;
                        case 4 when cntPlayer4 == 2:
                            isOtherPlayLeft = true;
                            WinUserShow();
                            break;
                        case 4:
                            box4Lifes[cntPlayer4].color = lifeOffColor;
                            cntPlayer4++;
                            ReconnectPasaChange();
                            break;
                    }

                    break;
                }
            }
            // print("Enter The Life is Decrese");
        }
    }


    public void TimerStop()
    {
        DataManager.Instance.isTimeAuto = true;
        timerFillImg.fillAmount = 0;
    }




    void TickSound()
    {
        SoundManager.Instance.TickTimerSound();
        Invoke(nameof(CheckTickSound), 1f);
    }

    void CheckTickSound()
    {
        if (timerFillImg.fillAmount != 0 && DataManager.Instance.isTimeAuto == false && DataManager.Instance.isDiceClick == true)
        {
            TickSound();
        }
    }

    #region Generate Random Number pasa
    int isSix1 = 0;
    int isSix2 = 0;
    public void PasaButtonClick()
    {
        if (DataManager.Instance.isDiceClick == true && isClickAvaliableDice == 0)
        {
            isCheckEnter = false;
            isClickAvaliableDice = 1;
            SoundManager.Instance.RollDice_Start_Sound();
            pasaImage.gameObject.GetComponent<Animator>().enabled = true;
            isPathClickAvaliable = false;
            if (DataManager.Instance.isAvaliable)
            {
                pasaCurrentNo = DataManager.Instance.currentPNo;
            }
            else
            {
                pasaCurrentNo = UnityEngine.Random.Range(1, 7);
            }
            if (pasaCurrentNo == 6)
            {
                if (isSix1 == 0)
                {
                    isSix1 = 1;
                }
                else if (isSix2 == 0)
                {
                    isSix2 = 1;
                }
                else if (isSix1 == 1 && isSix2 == 1)
                {
                    isSix1 = 0;
                    isSix2 = 0;
                    pasaCurrentNo = UnityEngine.Random.Range(1, 6);
                }
            }


            isPathClick = true;
            isClickAvaliableDice = 1;
            PlayerDice(pasaCurrentNo);
            if (DataManager.Instance.modeType == 1)
            {
                isCheckEnter = true;
            }
            Invoke(nameof(GenerateDiceNumber), 1.25f);

        }
        else
        {
            GenerateTurnObj();
        }
    }

    public void StopPasaZoom()
    {
        for (int i = 0; i < pasaCollectList.Count; i++)
        {
            if (pasaCollectList[i].playerNo == currentPlayerNo)
            {
                pasaCollectList[i].isStopZoom = true;
            }
        }
    }

    void GenerateDiceNumber()
    {
        pasaImage.gameObject.GetComponent<Animator>().enabled = false;

        SoundManager.Instance.RollDice_Stop_Sound();
        //        PasaImageManage(pasaCurrentNo, 1, false);
        pasaImage.sprite = pasaSprite[pasaCurrentNo - 1];

        pasaNoTxt.text = pasaCurrentNo.ToString();


        CheckPasaThePlayer();
        bool isBotChangeTurn = false;
        if (pasaCurrentNo == 6)
        {
            for (int i = 0; i < currentPlayerPasaList.Count; i++)
            {
                currentPlayerPasaList[i].isReadyForClick = true;
            }
        }
        else
        {
            if (pasaCollectList.Count == 0)
            {
                if (pasaCurrentNo != 6)
                {
                    isBotChangeTurn = true;
                    CheckThePasaBool();
                }

            }
        }

        if (pasaCollectList.Count != 4 && pasaCurrentNo == 6)
        {

            isPathClickAvaliable = false;
            isCheckEnter = false;
            return;
        }
        else
        {
            if (pasaCollectList.Count == 1)
            {
                if ((pasaCurrentNo + pasaCollectList[0].pasaCurrentNo) > 57)
                {
                    isClickAvaliableDice = 0;
                    //Greejesh Ludo
                    if (BotManager.Instance.isConnectBot)
                    {
                        BotChangeTurn(false, true);

                    }
                    else
                    {
                        PlayerChangeTurn();
                    }
                    isCheckEnter = false;
                    return;
                }
                else
                {
                    isPathClickAvaliable = true;
                    MovePlayer(pasaCollectList[0].playerSubNo, pasaCurrentNo);
                    PlayerStopDice();
                    isCheckEnter = true;
                    pasaCollectList[0].Move_Increment_Steps(pasaCurrentNo);
                }
            }
            else
            {

                if (pasaCollectList.Count > 0)
                {
                    int cnt = 0;
                    int pNo = pasaCollectList[0].pasaCurrentNo;
                    for (int i = 0; i < pasaCollectList.Count; i++)
                    {
                        if (pasaCollectList[i].pasaCurrentNo == pNo)
                        {
                            cnt++;
                        }
                    }
                    if (cnt == pasaCollectList.Count)
                    {
                        if (isBotChangeTurn == false)
                        {
                            isCheckEnter = true;
                            isPathClickAvaliable = true;
                            pasaCollectList[0].Move_Increment_Steps(pasaCurrentNo);
                        }
                        else
                        {
                            isCheckEnter = false;
                            // isPathClick = true;//GG
                        }


                        if (BotManager.Instance.isConnectBot == false)
                        {
                            PlayerStopDice();

                            MovePlayer(pasaCollectList[0].playerSubNo, pasaCurrentNo);
                        }
                    }
                    else
                    {
                        bool isPasaZoomEnter = false;
                        isPathClickAvaliable = false;
                        isCheckEnter = false;
                        for (int i = 0; i < pasaCollectList.Count; i++)
                        {
                            if (pasaCollectList[i].playerNo == currentPlayerNo)
                            {
                                // pasaCollectList[i].isFirstZoom = false;
                                pasaCollectList[i].isStopZoom = false;
                                isPasaZoomEnter = true;
                                //pasaCollectList[i].PlayerPasaZoom();
                            }
                        }
                    }
                }
                else
                {
                    PlayerStopDice();
                }
            }

        }
    }
    #endregion

    #region Board Box Button
    //bool isSimpleEnterButton = false;
    public void PathButtonClick(int no)
    {
        //print("IsDice Click : " + DataManager.Instance.isDiceClick);
        //print("isPathClick  : " + isPathClick);
        //print("isPathClickAvaliable  : " + isPathClickAvaliable);
        if (DataManager.Instance.isDiceClick == true && isPathClick == true && isPathClickAvaliable == false && isCheckEnter == false)// && isSimpleEnterButton == false)
        {
            //isCheckEnter = true;
            if (currentPlayerNo != DataManager.Instance.playerNo)
            {
                print("Not a Enter");
            }
            else
            {
                //isCheckEnter = true;
                CheckPasaThePlayer();
                if (checkPlayerPasa(no))
                {
                    isPathClick = false;

                    isCheckEnter = true;
                    StopPasaZoom();
                    //StopPasaZoomSix();

                    //pasaCollectList[0].Move_Increment_Steps(pasaCurrentNo);
                    for (int i = 0; i < pasaObjects.Count; i++)
                    {
                        PasaManage pasa = pasaObjects[i].GetComponent<PasaManage>();

                        if (pasa.pasaCurrentNo == no && pasa.playerNo == DataManager.Instance.playerNo && (pasa.pasaCurrentNo + pasaCurrentNo) <= 57)
                        {
                            isPathClick = false;
                            isCheckEnter = false;
                            // print("Enter The Auto move Not");
                            pasa.Move_Increment_Steps(pasaCurrentNo);
                            PlayerStopDice();
                            MovePlayer(pasa.playerSubNo, pasaCurrentNo);
                            break;
                        }
                    }
                }
                else
                {
                    //isCheckEnter = false;
                }

            }
        }
    }
    #endregion

    bool checkPlayerPasa(int no)
    {
        for (int i = 0; i < pasaCollectList.Count; i++)
        {
            if (pasaCollectList[i].pasaCurrentNo == no && pasaCollectList[i].isPlayer == true)
            {
                return true;
            }
        }
        return false;
    }

    void CheckPasaThePlayer()
    {
        pasaCollectList.Clear();

        for (int i = 0; i < pasaObjects.Count; i++)
        {
            PasaManage pasa = pasaObjects[i].GetComponent<PasaManage>();
            if (pasa.playerNo == currentPlayerNo && pasa.isPlayer == true)
            {
                if (pasa.pasaCurrentNo < 57)
                {
                    pasaCollectList.Add(pasa);
                }
            }
        }

    }



    public void GeneratePasaFire()
    {
        Destroy(Instantiate(generatePasaFireParticles, new Vector3(0, 0, 0f), Quaternion.identity), 6f);
    }

    #region Shadown Maintain
    public void OurShadowMaintain()
    {
        if (DataManager.Instance.isTwoPlayer)
        {
            shadow1.enabled = true;
            shadow3.GetComponent<Image>().color = shadowOff;
            shadow3.enabled = false;

            if (DataManager.Instance.playerNo == 3)
            {
                timerFillImg.color = greenColor;
            }
            else
            {
                timerFillImg.color = blueColor;
            }
            
        }
        else if(DataManager.Instance.isFourPlayer)
        {
            /*shadow1.enabled = true;
            shadow2.GetComponent<Image>().color = shadowOff;
            shadow2.enabled = false;
            shadow3.GetComponent<Image>().color = shadowOff;
            shadow3.enabled = false;
            shadow4.GetComponent<Image>().color = shadowOff;
            shadow4.enabled = false;
            if (DataManager.Instance.playerNo == 3)
            {
                timerFillImg.color = greenColor;
            }
            else if (DataManager.Instance.playerNo == 2)
            {
                timerFillImg.color = redColor;
            }
            else if (DataManager.Instance.playerNo == 4)
            {
                timerFillImg.color = yellowColor;
            }
            else
            {
                timerFillImg.color = blueColor;
            }*/

            switch (playerRoundChecker)
            {
                case 2:
                    shadow2.enabled = true;
                    shadow1.GetComponent<Image>().color = shadowOff;
                    shadow1.enabled = false;
                    shadow3.GetComponent<Image>().color = shadowOff;
                    shadow3.enabled = false;
                    shadow4.GetComponent<Image>().color = shadowOff;
                    shadow4.enabled = false;
                    timerFillImg.color = redColor;
                    break;
                case 3:
                    shadow3.enabled = true;
                    shadow2.GetComponent<Image>().color = shadowOff;
                    shadow2.enabled = false;
                    shadow1.GetComponent<Image>().color = shadowOff;
                    shadow1.enabled = false;
                    shadow4.GetComponent<Image>().color = shadowOff;
                    shadow4.enabled = false;
                    timerFillImg.color = greenColor;
                    break;
                case 4:
                    shadow4.enabled = true;
                    shadow2.GetComponent<Image>().color = shadowOff;
                    shadow2.enabled = false;
                    shadow3.GetComponent<Image>().color = shadowOff;
                    shadow3.enabled = false;
                    shadow1.GetComponent<Image>().color = shadowOff;
                    shadow1.enabled = false;
                    timerFillImg.color = yellowColor;
                    break;
                default:
                    shadow1.enabled = true;
                    shadow2.GetComponent<Image>().color = shadowOff;
                    shadow2.enabled = false;
                    shadow3.GetComponent<Image>().color = shadowOff;
                    shadow3.enabled = false;
                    shadow4.GetComponent<Image>().color = shadowOff;
                    shadow4.enabled = false;
                    timerFillImg.color = blueColor;
                    break;
            }
        }
    }

    public void OtherShadowMainTain()
    {
        if (DataManager.Instance.isTwoPlayer)
        {
            if (DataManager.Instance.isTwoPlayer)
            {
                shadow1.enabled = false;
                shadow1.GetComponent<Image>().color = shadowOff;
                shadow3.enabled = true;
                if (DataManager.Instance.playerNo == 1)
                {
                    ActivePlayer = 3;
                    timerFillImg.color = greenColor;
                }
                else
                {
                    ActivePlayer = 1;
                    timerFillImg.color = blueColor;
                }
                
            }
            else if ((DataManager.Instance.playerNo + 2) == 5)
            {
                shadow1.enabled = true;
                shadow3.GetComponent<Image>().color = shadowOff;
                shadow3.enabled = false;
                if (DataManager.Instance.playerNo == 1)
                {
                    ActivePlayer = 3;
                    timerFillImg.color = greenColor;
                }
                else
                {
                    ActivePlayer = 1;
                    timerFillImg.color = blueColor;
                }
            }
        }
        else if(DataManager.Instance.isFourPlayer)
        {
            switch (playerRoundChecker)
            {
                case 2:
                    shadow2.enabled = true;
                    shadow1.GetComponent<Image>().color = shadowOff;
                    shadow1.enabled = false;
                    shadow3.GetComponent<Image>().color = shadowOff;
                    shadow3.enabled = false;
                    shadow4.GetComponent<Image>().color = shadowOff;
                    shadow4.enabled = false;
                    timerFillImg.color = redColor;
                    break;
                case 3:
                    shadow3.enabled = true;
                    shadow2.GetComponent<Image>().color = shadowOff;
                    shadow2.enabled = false;
                    shadow1.GetComponent<Image>().color = shadowOff;
                    shadow1.enabled = false;
                    shadow4.GetComponent<Image>().color = shadowOff;
                    shadow4.enabled = false;
                    timerFillImg.color = greenColor;
                    break;
                case 4:
                    shadow4.enabled = true;
                    shadow2.GetComponent<Image>().color = shadowOff;
                    shadow2.enabled = false;
                    shadow3.GetComponent<Image>().color = shadowOff;
                    shadow3.enabled = false;
                    shadow1.GetComponent<Image>().color = shadowOff;
                    shadow1.enabled = false;
                    timerFillImg.color = yellowColor;
                    break;
                default:
                    shadow1.enabled = true;
                    shadow2.GetComponent<Image>().color = shadowOff;
                    shadow2.enabled = false;
                    shadow3.GetComponent<Image>().color = shadowOff;
                    shadow3.enabled = false;
                    shadow4.GetComponent<Image>().color = shadowOff;
                    shadow4.enabled = false;
                    timerFillImg.color = blueColor;
                    break;
            }
        }
    }

    #endregion
    #region SocketIO Methods


    #region SEND
    public void MovePlayer(int pasaNo, int diceNo)
    {
        JSONObject obj = new JSONObject();
        obj.AddField("PlayerID", DataManager.Instance.playerData._id);
        obj.AddField("TournamentID", DataManager.Instance.tournamentID);
        obj.AddField("PlayerNo", DataManager.Instance.playerNo);
        obj.AddField("TokenNo", pasaNo);
        obj.AddField("TokenMove", diceNo);
        obj.AddField("RoomId", TestSocketIO.Instace.roomid);

        TestSocketIO.Instace.Senddata("LudoData", obj);
    }

    void PlayerDice(int diceNo)
    {
        JSONObject obj = new JSONObject();
        obj.AddField("PlayerID", DataManager.Instance.playerData._id);
        obj.AddField("TournamentID", DataManager.Instance.tournamentID);
        obj.AddField("DiceNo", diceNo);
        obj.AddField("PlayerNo", DataManager.Instance.playerNo);
        obj.AddField("DiceManageCnt", DataManager.Instance.diceManageCnt);
        obj.AddField("RoomId", TestSocketIO.Instace.roomid);
        TestSocketIO.Instace.Senddata("LudoDiceData", obj);
    }

    public void PlayerStopDice()
    {
        JSONObject obj = new JSONObject();
        obj.AddField("PlayerID", DataManager.Instance.playerData._id);
        obj.AddField("TournamentID", DataManager.Instance.tournamentID);
        obj.AddField("RoomId", TestSocketIO.Instace.roomid);
        TestSocketIO.Instace.Senddata("LudoDiceStopData", obj);
    }

    public void PauseDataGetRequest()
    {
        JSONObject obj = new JSONObject();
        obj.AddField("PlayerID", DataManager.Instance.playerData._id);
        obj.AddField("TournamentID", DataManager.Instance.tournamentID);
        obj.AddField("RoomId", TestSocketIO.Instace.roomid);
        obj.AddField("PlayerNo", DataManager.Instance.playerNo);
        TestSocketIO.Instace.Senddata("PauseRequest", obj);
    }

    public void PlayerDiceChange()
    {
        JSONObject obj = new JSONObject();
        obj.AddField("PlayerID", DataManager.Instance.playerData._id);
        obj.AddField("TournamentID", DataManager.Instance.tournamentID);
        int noSend = 0;
        noSend = DataManager.Instance.playerNo;

        if (DataManager.Instance.playerNo == 1)
        {
            if (DataManager.Instance.isTwoPlayer)
            {
                noSend = 3;
            }
            else
            {
                noSend = 2;
            }

            if (DataManager.Instance.isFourPlayer)
            {
                noSend = playerRoundChecker;
            }
        }
        else if (DataManager.Instance.playerNo == 2)
        {
            noSend = 3;
        }
        else if (DataManager.Instance.playerNo == 3)
        {
            if (DataManager.Instance.isTwoPlayer)
            {
                noSend = 1;
            }
            else
            {
                noSend = 4;
            }
            if (DataManager.Instance.isFourPlayer)
            {
                noSend = playerRoundChecker;
            }
        }
        else if (DataManager.Instance.playerNo == 4)
        {
            noSend = 1;
        }

        obj.AddField("PlayerNo", noSend);
        obj.AddField("RoomId", TestSocketIO.Instace.roomid);

        TestSocketIO.Instace.Senddata("LudoDiceChangeData", obj);



    }
    #endregion

    #region Receive

    public void CheckThePasaBool()
    {
        if (BotManager.Instance.isConnectBot)
        {
            print("Enter The Direct Condition");
            BotChangeTurn(false, true);
        }
        else
        {
            PlayerChangeTurn();
        }
        RestartTimer();

        //RestartTimer();//Greejesh
        //DataManager.Instance.isChange = true;


        // print("Change Data Pass");
    }



    public void PauseUserDataSend()
    {
        JSONObject obj = new JSONObject();
        obj.AddField("PlayerID", DataManager.Instance.playerData._id);
        obj.AddField("TournamentID", DataManager.Instance.tournamentID);
        obj.AddField("RoomId", TestSocketIO.Instace.roomid);
        obj.AddField("PlayerNo", DataManager.Instance.playerNo);
        obj.AddField("GreenSliderValue", timerFillImg.fillAmount);
        obj.AddField("OurDot", cntPlayer2);
        bool isTurn = false;
        if (DataManager.Instance.isDiceClick)
        {
            isTurn = false;
        }
        else
        {
            isTurn = true;
        }

        obj.AddField("Turn", isTurn);

        TestSocketIO.Instace.Senddata("GetUserPauseData", obj);
    }

    public void PauseDataRetriveSocket(float fill, int number, bool isTurn)
    {
        isPauseGetData = true;
        timerFillImg.fillAmount = fill;
        if (isTurn)
        {
            print("This is clicked in pauseData retrive");
            DataManager.Instance.isDiceClick = true;
            isClickAvaliableDice = 0;
            OurShadowMaintain();
            DataManager.Instance.isTimeAuto = false;
            isTimeEnter = false;
            DataManager.Instance.isRestartManage = true;
        }
        else
        {
            DataManager.Instance.isDiceClick = false;
            isClickAvaliableDice = 1;
            OtherShadowMainTain();
            DataManager.Instance.isTimeAuto = false;
            isTimeEnter = false;
        }
        cntPlayer1 = number;
        for (int i = 0; i < box1Lifes.Length; i++)
        {
            if (i < cntPlayer1)
            {
                box1Lifes[i].color = lifeOffColor;
            }
        }

    }


    public void AutoMove(int playerNo, int tokenNo, int move)
    {
        //print("Enter The Auto Move");
        //print("Player No : " + playerNo);
        //print("Token No : " + tokenNo);
        //print("Token move : " + move);
        for (int i = 0; i < pasaSocketList.Count; i++)
        {
            if (pasaSocketList[i].playerNo == playerNo && pasaSocketList[i].playerSubNo == tokenNo)
            {
                if (pasaSocketList[i].orgParentNo == 1)
                {
                    //print("Enter Auto Move 1");
                    pasaSocketList[i].MoveStart(1, move);
                }
                else if (pasaSocketList[i].orgParentNo == 2)
                {
                    //pasaSocketList[i].MoveStart(numberObj2, move);
                    pasaSocketList[i].MoveStart(2, move);
                }
                else if (pasaSocketList[i].orgParentNo == 3)
                {
                    //                    print("Enter Auto Move 3");
                    pasaSocketList[i].MoveStart(3, move);
                    //pasaSocketList[i].MoveStart(numberObj3, move);
                }
                else if (pasaSocketList[i].orgParentNo == 4)
                {
                    //  pasaSocketList[i].MoveStart(numberObj4,move);
                    pasaSocketList[i].MoveStart(4, move);
                }
            }

        }
    }


    //    public void StopDiceLine()
    //    {
    ////        print("Enter Fill Amount");
    //        if (DataManager.Instance.isRestartManage == true)
    //        {
    //            DataManager.Instance.isRestartManage = false;
    //        }
    //        else
    //        {
    //            timerFillImg.fillAmount = 0;
    //        }

    //    }
    public void AutoDice(int no, int pNo, int currentPlayer)
    {
        playerRoundChecker = currentPlayer;
        SoundManager.Instance.RollDice_Start_Sound();
        pasaImage.gameObject.GetComponent<Animator>().enabled = true;
        StartCoroutine(GenerateDiceNumber_Socket(no, pNo, currentPlayer));
    }
    IEnumerator GenerateDiceNumber_Socket(int no, int pNo, int pNo1)
    {
        yield return new WaitForSeconds(1.25f);
        pasaImage.gameObject.GetComponent<Animator>().enabled = false;
        //print("Player No : " + pNo);
        //print("Player No 1 : " + pNo1);

        if (pNo == DataManager.Instance.playerNo)
        {
            //            PasaImageManage(no, 3, true);
        }
        else if (pNo1 == DataManager.Instance.playerNo)
        {
            //          PasaImageManage(no, 3, true);
        }
        SoundManager.Instance.RollDice_Stop_Sound();
        pasaImage.sprite = pasaSprite[no - 1];


    }
    #endregion

    #endregion

    #region  Application Pause
    private void OnApplicationPause(bool pause)
    {
        print("Pause : " + pause);
        if (pause)
        {
            //Check
            DateTime date = DateTime.Now;

            PlayerPrefs.SetString("PlayTimeDate", date.ToString());
        }
        else
        {

            PauseDataGetRequest();
            Invoke(nameof(WaitTimeToCheck), 3f);
            //gettime to diff
            GetDiffPause();
        }
    }


    void WaitTimeToCheck()
    {
        if (isPauseGetData == false)
        {
            DataManager.Instance.tournamentID = "";
            DataManager.Instance.tourEntryMoney = 0;
            DataManager.Instance.tourCommision = 0;
            DataManager.Instance.commisionAmount = 0;
            DataManager.Instance.orgIndexPlayer = 0;
            DataManager.Instance.joinPlayerDatas.Clear();
            TestSocketIO.Instace.roomid = "";
            TestSocketIO.Instace.userdata = "";
            TestSocketIO.Instace.playTime = 0;
            SceneManager.LoadScene("Main");
        }
        else if (isPauseGetData == true)
        {
            isPauseGetData = false;
        }
    }
    


    void GetDiffPause()
    {

        string getDate = PlayerPrefs.GetString("PlayTimeDate", "none");
        if (getDate == "none")
        {
            return;
        }

        int createHour = int.Parse(getDate.Split(" ")[1].Split(":")[0]);
        //int currHour = ;
        int createMinute = int.Parse(getDate.Split(" ")[1].Split(":")[1]);
        int createSecond = int.Parse(getDate.Split(" ")[1].Split(":")[2]);

        DateTime date = DateTime.Now;
        string curDate = date.ToString();
        int currHour = int.Parse(curDate.Split(" ")[1].Split(":")[0]);

        int currMinute = int.Parse(curDate.Split(" ")[1].Split(":")[1]);
        int currSecond = int.Parse(curDate.Split(" ")[1].Split(":")[2]);


        DateTime dateTime1 = DateTime.Parse(createHour + ":" + createMinute + ":" + createSecond);
        DateTime dateTime2 = DateTime.Parse(currHour + ":" + currMinute + ":" + currSecond);

        var diff = (dateTime2 - dateTime1).TotalSeconds;

        string changeString = diff.ToString();

        long diffInSeconds = long.Parse(changeString);

        if (diffInSeconds >= 180)
        {
            DataManager.Instance.tournamentID = "";
            DataManager.Instance.tourEntryMoney = 0;
            DataManager.Instance.tourCommision = 0;
            DataManager.Instance.commisionAmount = 0;
            DataManager.Instance.orgIndexPlayer = 0;
            DataManager.Instance.joinPlayerDatas.Clear();
            TestSocketIO.Instace.roomid = "";
            TestSocketIO.Instace.userdata = "";
            TestSocketIO.Instace.playTime = 0;
        }
        else
        {
            secondsCount -= diffInSeconds;
        }

    }



    #endregion

    #region Bot Manager

    bool isOneBot = false;
    bool isTwoBot = false;
    bool isOneTimeGive = false;
    int easyCount = 0;
    bool easyBoolSet = false;

    bool botFirstSix = true;
    bool botSecondSix = true;


    public void GenerateDiceNumberStart_Bot(bool isStart)
    {
        if (BotManager.Instance.isConnectBot)
        {
            if (isStart)
            {
                //BotMoveTokeStore botMoveGet = Check_Kill_Bot();
                //PasaButtonClick_Bot(botMoveGet);

                CheckPasaThePlayer_Bot();
                BotMoveTokeStore botMoveGet = new BotMoveTokeStore();
                botMoveGet.moveNo = UnityEngine.Random.Range(1, 6);

                //botMoveGet.moveNo = 6;

                PasaButtonClick_Bot(botMoveGet);
            }
            else
            {
                if (BotManager.Instance.botType == BotType.Easy)
                {
                    CheckPasaThePlayer_Bot();

                    List<PasaManage> moveBotPlayer = MoveablePlayer();
                    BotMoveTokeStore botMoveGet = new BotMoveTokeStore();

                    int botSNo = UnityEngine.Random.Range(1, 7);
                    if (botSNo == 6)
                    {
                        if (botFirstSix == false)
                        {
                            botFirstSix = true;
                            botSecondSix = false;
                        }
                        else if (botSecondSix == false)
                        {
                            botFirstSix = true;
                            botSecondSix = true;
                        }
                        else if (botFirstSix == true & botSecondSix == true)
                        {
                            botSNo = UnityEngine.Random.Range(1, 6);
                            botFirstSix = false;
                            botSecondSix = false;

                        }
                    }
                    if (moveBotPlayer.Count != 0 && botSNo != 6)
                    {
                        int moveNoTemp = UnityEngine.Random.Range(0, moveBotPlayer.Count);
                        botMoveGet.pasaToken = moveBotPlayer[moveNoTemp];
                        botMoveGet.isMoveSend = true;
                    }
                    botMoveGet.moveNo = botSNo;


                    if (easyBoolSet == false)
                    {
                        if (botMoveGet.moveNo == 6)
                        {
                            easyBoolSet = true;
                        }
                        else
                        {
                            easyCount++;
                            if (easyCount == 9)
                            {
                                botMoveGet.moveNo = 6;
                                easyBoolSet = true;
                            }
                        }
                    }
                    //botMoveGet.moveNo = 6;

                    PasaButtonClick_Bot(botMoveGet);
                }
                else if (BotManager.Instance.botType == BotType.Medium)
                {
                    int rno = UnityEngine.Random.Range(0, 5);
                    if (rno == 0 || rno == 1 || rno == 2)
                    {
                        CheckPasaThePlayer_Bot();
                        List<PasaManage> moveBotPlayer = MoveablePlayer();
                        BotMoveTokeStore botMoveGet = new BotMoveTokeStore();

                        int botSNo = UnityEngine.Random.Range(1, 7);
                        //int botSNo = 6;
                        if (botSNo == 6)
                        {
                            if (botFirstSix == false)
                            {
                                botFirstSix = true;
                                botSecondSix = false;
                            }
                            else if (botSecondSix == false)
                            {
                                botFirstSix = true;
                                botSecondSix = true;
                            }
                            else if (botFirstSix == true & botSecondSix == true)
                            {
                                botSNo = UnityEngine.Random.Range(1, 6);
                                botFirstSix = false;
                                botSecondSix = false;
                            }
                        }
                        if (moveBotPlayer.Count != 0 && botSNo != 6)
                        {
                            int moveNoTemp = UnityEngine.Random.Range(0, moveBotPlayer.Count);
                            botMoveGet.pasaToken = moveBotPlayer[moveNoTemp];
                            botMoveGet.isMoveSend = true;
                        }

                        botMoveGet.moveNo = botSNo;
                        if (easyBoolSet == false)
                        {
                            if (botMoveGet.moveNo == 6)
                            {
                                easyBoolSet = true;
                            }
                            else
                            {
                                easyCount++;
                                if (easyCount == 6)
                                {
                                    botMoveGet.moveNo = 6;
                                    easyBoolSet = true;
                                }
                            }
                        }


                        PasaButtonClick_Bot(botMoveGet);
                    }
                    else
                    {
                        BotMoveTokeStore botMoveGet = Check_Kill_Bot();
                        PasaButtonClick_Bot(botMoveGet);
                    }
                }
                else if (BotManager.Instance.botType == BotType.Hard)
                {
                    BotMoveTokeStore botMoveGet = Check_Kill_Bot();
                    PasaButtonClick_Bot(botMoveGet);
                }

            }
        }
    }

    public void PasaButtonClick_Bot(BotMoveTokeStore botMove)
    {
        SoundManager.Instance.RollDice_Start_Sound();
        pasaImage.gameObject.GetComponent<Animator>().enabled = true;
        print("Pasa Bot Con 1");
        //isPathClick = false;
        isClickAvaliableDice = 1;
        pasaCurrentNo = botMove.moveNo;
        StartCoroutine(GenerateDiceNumber_Bot(botMove));
    }


    IEnumerator GenerateDiceNumber_Bot(BotMoveTokeStore botMove)
    {
        yield return new WaitForSeconds(1.25f);
        pasaImage.gameObject.GetComponent<Animator>().enabled = false;

        SoundManager.Instance.RollDice_Stop_Sound();
        pasaImage.sprite = pasaSprite[pasaCurrentNo - 1];

        pasaNoTxt.text = pasaCurrentNo.ToString();


        if (botMove.pasaToken != null)
        {
            //if (botMove.isMoveSend)
            //{
            //    if (pasaCurrentNo == 6)
            //    {
            //        List<PasaManage> notAmove = new List<PasaManage>();
            //        for (int i = 0; i < pasaCollectList.Count; i++)
            //        {
            //            if (pasaCollectList[i].pasaCurrentNo == 0)
            //            {
            //                notAmove.Add(pasaCollectList[i]);
            //            }
            //        }
            //        if (notAmove.Count > 0)
            //        {
            //            notAmove[0].MoveStart_Bot(3, pasaCurrentNo);
            //        }
            //        else
            //        {
            //            botMove.pasaToken.MoveStart_Bot(3, pasaCurrentNo);
            //        }

            //    }
            //    else
            //    {
            //        botMove.pasaToken.MoveStart_Bot(3, pasaCurrentNo);
            //    }
            //}
            //else
            //{
            //    botMove.pasaToken.MoveStart_Bot(3, pasaCurrentNo);
            //}
            botMove.pasaToken.MoveStart_Bot(DataManager.Instance.isTwoPlayer ? 2 : playerRoundChecker, pasaCurrentNo);
        }

        else if (botMove.pasaToken == null)
        {
            if (pasaCurrentNo == 6)
            {
                List<PasaManage> notAmove = new List<PasaManage>();
                for (int i = 0; i < pasaCollectList.Count; i++)
                {
                    if (pasaCollectList[i].pasaCurrentNo == 0)
                    {
                        notAmove.Add(pasaCollectList[i]);
                    }
                }
                if (notAmove.Count > 0)
                {
                    print("Move Condition  :" + notAmove[0].gameObject.name);
                    notAmove[0].MoveStart_Bot(DataManager.Instance.isTwoPlayer ? 2 : playerRoundChecker, pasaCurrentNo);
                }
            }
            // else if (rollingPlayer == 4)
            // {
            //     Invoke(nameof(WaitAfterTurnChangeNotOut), 0f);
            //     rollingPlayer = 1;
            // }
            else
            {
                if (DataManager.Instance.isTwoPlayer)
                {
                    print("!!!!!!Enter The Else COndition First Six!!!!!!");
                    //GenerateDiceNumberStart_Bot(false);
                    Invoke(nameof(WaitAfterTurnChangeNotOut), 0f);
                }
                else if(DataManager.Instance.isFourPlayer && playerRoundChecker == 4)
                {
                    print("!!!!!!!!!!!!!!!!!!! Condision passed!!!!!!!!!!!!!!!!!!!!!!");
                    Invoke(nameof(WaitAfterTurnChangeNotOut), 0f);
                }
                else
                {
                    BotChangeTurn(false, true);
                }

            }
        }
        //else
        //{
        //    if (botMove.isMoveSend)
        //    {
        //        if (pasaCurrentNo == 6)
        //        {
        //            List<PasaManage> notAmove = new List<PasaManage>();
        //            for (int i = 0; i < pasaCollectList.Count; i++)
        //            {
        //                if (pasaCollectList[i].pasaCurrentNo == 0)
        //                {
        //                    notAmove.Add(pasaCollectList[i]);
        //                }
        //            }
        //            if (notAmove.Count > 0)
        //            {
        //                notAmove[0].MoveStart_Bot(3, pasaCurrentNo);
        //            }
        //            else
        //            {
        //                botMove.pasaToken.MoveStart_Bot(3, pasaCurrentNo);
        //            }

        //        }
        //        else
        //        {
        //            botMove.pasaToken.MoveStart_Bot(3, pasaCurrentNo);
        //        }
        //    }
        //    else
        //    {
        //        botMove.pasaToken.MoveStart_Bot(3, pasaCurrentNo);
        //    }
        //}
    }

    void WaitAfterTurnChangeNotOut()
    {
        //playerRoundChecker = 1;
        BotChangeTurn(true, false);
    }


    void CheckPasaThePlayer_Bot()
    {
        if (DataManager.Instance.isTwoPlayer)
        {
            pasaCollectList.Clear();
            int botPlayerNo = 1;
            if (DataManager.Instance.playerNo == 1)
            {
                botPlayerNo = 3;
            }
            for (int i = 0; i < pasaBotPlayer.Count; i++)
            {
                PasaManage pasa = pasaBotPlayer[i].GetComponent<PasaManage>();
                if (pasa.playerNo == botPlayerNo && pasa.isPlayer == false)
                {
                    pasaCollectList.Add(pasa);
                }
            }
        }
        else if (DataManager.Instance.isFourPlayer)
        {
            pasaCollectList.Clear();
            if (DataManager.Instance.playerNo == 1 && !isEntered)
            {
                botPlayerNo = 2;
                AddingPasaCollectionList(botPlayerNo);
                isEntered = true;
                return;
            }
            switch (playerRoundChecker)
            //switch (DataManager.Instance.playerNo)
            {
                case 2:
                {
                    botPlayerNo = playerRoundChecker;
                    AddingPasaCollectionList(botPlayerNo);
                    break;
                }
                case 3:
                {
                    botPlayerNo = playerRoundChecker;
                    AddingPasaCollectionList(botPlayerNo);
                    break;
                }
                case 4:
                {
                    botPlayerNo = playerRoundChecker;
                    AddingPasaCollectionList(botPlayerNo);
                    isEntered = false;
                    break;
                }
            }
            
        }
    }

    public void AddingPasaCollectionList(int botPlayerNo)
    {
        for (int i = 0; i < pasaBotPlayer.Count; i++)
        {
            PasaManage pasa = pasaBotPlayer[i].GetComponent<PasaManage>();
            if (pasa.playerNo == botPlayerNo && pasa.isPlayer == false)
            {
                pasaCollectList.Add(pasa);
            }
        }
    }

    BotMoveTokeStore Check_Kill_Bot()
    {
        BotMoveTokeStore botMoveTokeStore = new BotMoveTokeStore();
        CheckPasaThePlayer_Bot();

        List<PasaManage> moveBotPlayer = MoveablePlayer();
        List<PasaManage> killBotPlayer = KillPlayerBot();
        List<PasaManage> homeBotPlayer = HomePlayerBot();
        List<PasaManage> safeBotPlayer = SafePlayerBot();



        if (moveBotPlayer.Count > 0)
        {
            if (killBotPlayer.Count > 0)
            {
                PasaManage pSafeManage1 = killBotPlayer[0];
                PasaManage pSafeManage2 = killBotPlayer[1];
                bool isFindEnter = false;
                for (int i = 1; i < 7; i++)
                {
                    int checkNo = pSafeManage1.orgNo + i;
                    if (checkNo == pSafeManage2.orgNo && isFindEnter == false)
                    {
                        isFindEnter = true;
                        botMoveTokeStore.moveNo = i;
                        botMoveTokeStore.pasaToken = pSafeManage1;
                        botMoveTokeStore.isKillSend = true;
                    }
                }
            }
            else
            {
                if (homeBotPlayer.Count > 0)
                {
                    PasaManage pSafeManage = homeBotPlayer[0];
                    bool isFindEnter = false;
                    for (int i = 1; i < 7; i++)
                    {
                        int checkNo = pSafeManage.orgNo + i;
                        if (checkNo == 57 && isFindEnter == false)
                        {
                            isFindEnter = true;
                            botMoveTokeStore.moveNo = i;
                            botMoveTokeStore.pasaToken = pSafeManage;
                            botMoveTokeStore.isHomeSend = true;
                        }
                    }
                }
                else
                {
                    if (safeBotPlayer.Count > 0)
                    {

                        List<PasaManage> movePlayerOrgAv = new List<PasaManage>();
                        for (int i = 0; i < pasaObjects.Count; i++)
                        {
                            PasaManage pManageOrgDv = pasaObjects[i].GetComponent<PasaManage>();
                            if (currentPlayerPasaList.Contains(pManageOrgDv))
                            {
                                movePlayerOrgAv.Add(pManageOrgDv);
                            }
                        }

                        List<bool> checkTheMove = new List<bool>();

                        if (movePlayerOrgAv.Count > 0)
                        {
                            for (int i = 0; i < safeBotPlayer.Count; i++)
                            {
                                PasaManage pSafeManage = safeBotPlayer[i];
                                bool isUnsafe = false;
                                for (int j = 0; j < movePlayerOrgAv.Count; j++)
                                {
                                    PasaManage pMoveOrgManage = movePlayerOrgAv[i];

                                    for (int k = 1; k < 7; k++)
                                    {
                                        if (pSafeManage.orgNo == (pMoveOrgManage.orgNo + k) && isUnsafe == false)
                                        {
                                            isUnsafe = true;
                                        }

                                    }

                                    if (isUnsafe)
                                    {
                                        break;
                                    }
                                }
                                checkTheMove.Add(isUnsafe);
                            }

                            if (checkTheMove.Contains(true))
                            {
                                bool isFindEnter = false;
                                for (int i1 = 0; i1 < checkTheMove.Count; i1++)
                                {
                                    bool isGetCheck = checkTheMove[i1];
                                    if (isGetCheck)
                                    {
                                        PasaManage pSafeManage = safeBotPlayer[i1];
                                        for (int i = 1; i < 7; i++)
                                        {
                                            int checkNo = pSafeManage.orgNo + i;
                                            if ((checkNo == 1 || checkNo == 9 || checkNo == 14 || checkNo == 22 || checkNo == 27 || checkNo == 35 || checkNo == 40 || checkNo == 48) && isFindEnter == false)
                                            {
                                                isFindEnter = true;
                                                botMoveTokeStore.moveNo = i;
                                                botMoveTokeStore.pasaToken = pSafeManage;
                                                botMoveTokeStore.isSafeSend = true;
                                                //generatePasaNo = i;
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {

                                bool isKillAvliable = false;
                                //int orgFirstNo = 1;

                                //for (int i = 0; i < movePlayerOrgAv.Count; i++)
                                //{
                                //    if (movePlayerOrgAv[i].orgNo > 27 && movePlayerOrgAv[i].orgNo < 34)
                                //    {

                                //    }
                                //}
                                for (int i = 0; i < movePlayerOrgAv.Count; i++)
                                {
                                    if (movePlayerOrgAv[i].orgNo > 27 && movePlayerOrgAv[i].orgNo < 34)
                                    {
                                        isKillAvliable = true;
                                        break;
                                    }
                                }

                                if (isKillAvliable == true)
                                {
                                    int cnt = 0;
                                    bool isExist = false;
                                    for (int i = 0; i < pasaBotPlayer.Count; i++)
                                    {
                                        if (pasaBotPlayer[i].orgNo == 27 && pasaBotPlayer[i].pasaCurrentNo == 1 && isExist == false)
                                        {
                                            isExist = true;
                                        }
                                        else
                                        {
                                            if (pasaBotPlayer[i].orgNo == 0 && pasaBotPlayer[i].pasaCurrentNo == 0)
                                            {
                                                cnt++;
                                            }
                                        }
                                    }
                                    if (cnt > 0 && isExist == false)
                                    {
                                        PasaManage pSafeManage = safeBotPlayer[0];
                                        botMoveTokeStore.moveNo = 6;
                                        botMoveTokeStore.pasaToken = null;//Greejesh Create a Null
                                        botMoveTokeStore.isMoveSend = true;
                                    }
                                    else
                                    {
                                        PasaManage pSafeManage = safeBotPlayer[0];
                                        botMoveTokeStore.moveNo = Bot_Random_Genrate();
                                        botMoveTokeStore.pasaToken = pSafeManage;
                                        botMoveTokeStore.isMoveSend = true;
                                    }
                                }
                                else
                                {
                                    PasaManage pSafeManage = safeBotPlayer[0];
                                    botMoveTokeStore.moveNo = Bot_Random_Genrate();
                                    botMoveTokeStore.pasaToken = pSafeManage;
                                    botMoveTokeStore.isMoveSend = true;
                                }
                            }
                        }
                        else
                        {
                            PasaManage pSafeManage = safeBotPlayer[0];
                            botMoveTokeStore.moveNo = Bot_Random_Genrate();
                            botMoveTokeStore.pasaToken = pSafeManage;
                            botMoveTokeStore.isMoveSend = true;
                        }

                        //PasaManage pSafeManage = safeBotPlayer[0];
                        //bool isFindEnter = false;
                        //for (int i = 1; i < 7; i++)
                        //{
                        //    int checkNo = pSafeManage.orgNo + i;
                        //    if ((checkNo == 1 || checkNo == 9 || checkNo == 14 || checkNo == 22 || checkNo == 27 || checkNo == 35 || checkNo == 40 || checkNo == 48) && isFindEnter == false)
                        //    {
                        //        isFindEnter = true;
                        //        botMoveTokeStore.moveNo = i;
                        //        botMoveTokeStore.pasaToken = pSafeManage;
                        //        botMoveTokeStore.isSafeSend = true;

                        //        //generatePasaNo = i;
                        //    }
                        //}
                    }
                    else
                    {
                        //generatePasaNo = Bot_Random_Genrate();
                        botMoveTokeStore.moveNo = Bot_Random_Genrate();
                        int rno = UnityEngine.Random.Range(0, moveBotPlayer.Count);
                        botMoveTokeStore.pasaToken = moveBotPlayer[rno];
                        botMoveTokeStore.isMoveSend = true;

                    }
                }
            }
        }
        else
        {
            botMoveTokeStore.moveNo = Bot_Random_Genrate();
            botMoveTokeStore.pasaToken = null;
            // generatePasaNo = Bot_Random_Genrate();
        }

        return botMoveTokeStore;
    }

    bool isFirstEnter = false;
    bool isSecondBotEnter = false;
    int botRandomGenCnt = 0;
    int botSixCounter = 0;
    bool OneTimeEnter = false;
    int Bot_Random_Genrate()
    {


        int rGen = UnityEngine.Random.Range(1, 7);




        //First Test
        botRandomGenCnt++;
        int checkCnt = 0;
        if (botRandomGenCnt == 3)
        {
            for (int i = 0; i < pasaBotPlayer.Count; i++)
            {
                if (pasaBotPlayer[i].pasaCurrentNo == 0)
                {
                    checkCnt++;
                }
            }
        }
        if (checkCnt == 4 && OneTimeEnter == false)
        {
            OneTimeEnter = true;
            rGen = 6;
        }
        //if (botRandomGenCnt <= 2)
        //{

        //    rGen = 6;
        //}
        //else
        //{
        //    rGen = UnityEngine.Random.Range(1, 7);
        //}
        //if (botSixManage == false)
        //{
        //    botSixManage = true;

        //}

        if (rGen == 6)
        {
            botSixCounter++;
        }

        if (botSixCounter == 3)
        {
            rGen = UnityEngine.Random.Range(1, 6);
        }


        int botSNo = rGen;
        if (botSNo == 6)
        {
            if (botFirstSix == false)
            {
                botFirstSix = true;
                botSecondSix = false;
            }
            else if (botSecondSix == false)
            {
                botFirstSix = true;
                botSecondSix = true;
            }
            else if (botFirstSix == true & botSecondSix == true)
            {
                botSNo = UnityEngine.Random.Range(1, 6);
                botFirstSix = false;
                botSecondSix = false;
            }
        }

        rGen = botSNo;
        //Normal Test
        //if (rGen == 6)
        //{
        //    if (isFirstEnter == false)
        //    {
        //        isFirstEnter = true;
        //    }
        //    else if (isSecondBotEnter == false)
        //    {
        //        isSecondBotEnter = true;
        //    }
        //    else
        //    {
        //        rGen = UnityEngine.Random.Range(1, 6);
        //        isFirstEnter = false;
        //        isSecondBotEnter = false;
        //    }
        //}

        //Second Test
        //if (DataManager.Instance.isBotSix)
        //{
        //    DataManager.Instance.isBotSix = false;
        //    rGen = 6;

        //    //rGen = DataManager.Instance.botPasaNo;
        //}
        //else
        //{
        //    rGen = DataManager.Instance.botPasaNo;
        //}
        //rGen = 1;
        return rGen;
    }


    public void OnceTimeTurnBot()
    {
        RestartTimer();
        GenerateDiceNumberStart_Bot(false);
        //BotChangeTurn(false, true);
    }


    List<PasaManage> MoveablePlayer()
    {
        List<PasaManage> movePlayerAv = new List<PasaManage>();
        for (int i = 0; i < pasaCollectList.Count; i++)
        {
            if (pasaCollectList[i].pasaCurrentNo > 0)
            {
                movePlayerAv.Add(pasaCollectList[i]);
            }
        }
        return movePlayerAv;
    }

    List<PasaManage> KillPlayerBot()
    {
        List<PasaManage> movePlayerAv = new List<PasaManage>();
        for (int i = 0; i < pasaCollectList.Count; i++)
        {
            if (pasaCollectList[i].pasaCurrentNo > 0)
            {
                movePlayerAv.Add(pasaCollectList[i]);
            }
        }

        List<PasaManage> killPlayerAv = new List<PasaManage>();
        if (movePlayerAv.Count > 0)
        {
            List<PasaManage> killMainPlayer = new List<PasaManage>();

            for (int i = 0; i < pasaObjects.Count; i++)
            {
                int checkNo = pasaObjects[i].GetComponent<PasaManage>().pasaCurrentNo;
                if (checkNo != 1 && checkNo != 9 && checkNo != 14 && checkNo != 22 && checkNo != 27 && checkNo != 35 && checkNo != 40 && checkNo != 48 && pasaObjects[i].GetComponent<PasaManage>().playerNo == DataManager.Instance.playerNo)
                {
                    killMainPlayer.Add(pasaObjects[i].GetComponent<PasaManage>());
                }
            }

            PasaManage temp = null;
            if (killMainPlayer.Count > 0)
            {
                for (int i = 0; i < killMainPlayer.Count - 1; i++)
                {
                    // traverse i+1 to array length
                    for (int j = i + 1; j < killMainPlayer.Count; j++)
                    {
                        if (killMainPlayer[i].pasaCurrentNo < killMainPlayer[j].pasaCurrentNo)
                        {
                            temp = killMainPlayer[i];
                            killMainPlayer[i] = killMainPlayer[j];
                            killMainPlayer[j] = temp;
                        }
                    }
                }
                bool isKillPlayerStore = false;
                for (int i = 0; i < killMainPlayer.Count; i++)
                {
                    for (int j = 0; j < movePlayerAv.Count; j++)
                    {
                        int killPlayerOrgNo = killMainPlayer[i].orgNo;
                        int mainPlayerOrgNo = movePlayerAv[j].orgNo;
                        for (int k = 1; k < 7; k++)
                        {
                            int checkNo = mainPlayerOrgNo + k;
                            if (checkNo == killPlayerOrgNo && isKillPlayerStore == false)
                            {
                                isKillPlayerStore = true;
                                killPlayerAv.Clear();

                                // Two Type Manage First is Bot Player Kill Second Element is Get Pasa Number to kill
                                killPlayerAv.Add(movePlayerAv[j]);
                                killPlayerAv.Add(killMainPlayer[i]);
                            }
                        }
                    }
                }
            }
            else
            {
                //
                killPlayerAv = killMainPlayer;
            }


        }
        return killPlayerAv;
    }

    List<PasaManage> HomePlayerBot()
    {
        List<PasaManage> homePlayerAv = new List<PasaManage>();
        for (int i = 0; i < pasaCollectList.Count; i++)
        {
            if (pasaCollectList[i].pasaCurrentNo > 51)
            {
                homePlayerAv.Add(pasaCollectList[i]);
            }
        }


        return homePlayerAv;
    }

    List<PasaManage> SafePlayerBot()
    {
        List<PasaManage> movePlayerAv = new List<PasaManage>();
        for (int i = 0; i < pasaCollectList.Count; i++)
        {
            if (pasaCollectList[i].pasaCurrentNo > 0)
            {
                movePlayerAv.Add(pasaCollectList[i]);
            }
        }

        List<PasaManage> safePlayerAv = new List<PasaManage>();
        bool isBotSafeEnter = false;
        if (movePlayerAv.Count > 0)
        {
            for (int i = 0; i < movePlayerAv.Count; i++)
            {
                for (int j = 1; j < 7; j++)
                {
                    int checkNo = movePlayerAv[i].orgNo + j;
                    if ((checkNo == 1 || checkNo == 9 || checkNo == 14 || checkNo == 22 || checkNo == 27 || checkNo == 35 || checkNo == 40 || checkNo == 48) && isBotSafeEnter == false)
                    {
                        safePlayerAv.Add(movePlayerAv[i]);
                        isBotSafeEnter = true;
                    }
                }
            }
        }
        else
        {
            safePlayerAv.Clear();
        }
        return safePlayerAv;
    }

    /*List<PasaManage> SafeMovePlayerBot(List<PasaManage> safeBotPlayer)
    {
        List<PasaManage> movePlayerAv = new List<PasaManage>();
        for (int i = 0; i < pasaCollectList.Count; i++)
        {
            if (pasaCollectList[i].pasaCurrentNo > 0)
            {
                movePlayerAv.Add(pasaCollectList[i]);
            }
        }



        List<PasaManage> safeMovePlayerAv = new List<PasaManage>();
        bool isBotSafeEnter = false;
        if (movePlayerAv.Count > 0)
        {
            for (int i = 0; i < movePlayerAv.Count; i++)
            {
                for (int j = 1; j < 7; j++)
                {
                    int checkNo = movePlayerAv[i].orgNo - j;
                    if ((checkNo == 1 || checkNo == 9 || checkNo == 14 || checkNo == 22 || checkNo == 27 || checkNo == 35 || checkNo == 40 || checkNo == 48) && isBotSafeEnter == false)
                    {
                        safeMovePlayerAv.Add(movePlayerAv[i]);
                        isBotSafeEnter = true;
                    }
                }
            }
        }
        else
        {
            safeMovePlayerAv.Clear();
        }


        return safeMovePlayerAv;
    }
    */

    void WaitTurnChangeAfter()
    {
        GenerateDiceNumberStart_Bot(false);
    }

    public void BotChangeTurn(bool isSendBot, bool isSendPlayer)
    {
        //print("Enter The change turn : " + isSendBot + "  Else : " + isSendPlayer);
        print(ActivePlayer + "<- this is current player");
        if (DataManager.Instance.isFourPlayer)
        {
            playerRoundChecker = playerRoundChecker switch
            {
                1 => 2,
                2 => 3,
                3 => 4,
                4 => 1,
                _ => playerRoundChecker
            };
        }
        

        if (isSendPlayer)
        {
            //botSixManage = false;
            botSixCounter = 0;
            isClickAvaliableDice = 0;
            

            botFirstSix = false;
            botSecondSix = false;
            SoundManager.Instance.UserTurnSound();
            DataManager.Instance.isDiceClick = false;
            DataManager.Instance.isTimeAuto = false;
            // PlayerDiceChange();
            OtherShadowMainTain();
            RestartTimer();
            Invoke(nameof(WaitTurnChangeAfter), UnityEngine.Random.Range(0.95f, 1.5f));
            //GenerateDiceNumberStart_Bot(false);
            print("******************Bot is called*********************");
        }
        else if (isSendBot)
        {
            
            SoundManager.Instance.UserTurnSound();
            if (DataManager.Instance.GetVibration() == 0)
            {
                if (Application.platform == RuntimePlatform.Android)
                {
                    //MMNVAndroid.AndroidVibrate(100);
                }
            }
            print("this is called in bot change turn");
            DataManager.Instance.isDiceClick = true;
            isCheckEnter = false;
            LudoManager.Instance.isClickAvaliableDice = 0;
            LudoManager.Instance.OurShadowMaintain();
            DataManager.Instance.isTimeAuto = false;
            LudoManager.Instance.RestartTimer();
           // if (DataManager.Instance.modeType == 3)
           // {
                //LudoManager.Instance.DiceLessPasaButton();
           // }
           print("******************Player is called*********************");
        }
    }

    #endregion
}

[System.Serializable]
public class BotMoveTokeStore
{
    public int moveNo;
    public PasaManage pasaToken;
    public bool isMoveSend;
    public bool isKillSend;
    public bool isSafeSend;
    public bool isHomeSend;
}
