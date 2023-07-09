
using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class DragonTigerManager : MonoBehaviour
{

    [System.Serializable]
    public class DragonTigerBetClass
    {
        public int no;
        public float price;
        public GameObject betObj;
    }

    public Image avatarImg;

    public static DragonTigerManager Instance;


    public GameObject dragonAnim;
    public GameObject tigerAnim;

    public GameObject winParticles;
    public Text winAnimationTxt;

    [Header("--- Menu Screen ---")]
    public Text userNameTxt;
    public Text balanceTxt;

    [Header("--- Menu Screen ---")]
    public GameObject menuScreenObj;

    [Header("--- Rule Screen ---")]
    public GameObject ruleScreenObj;

    [Header("--- Error Screen ---")]
    public GameObject errorScreenObj;

    [Header("--- Prefab ---")]
    public GameObject shopPrefab;
    public GameObject shopPrefabParent;

    public GameObject chip;


    [Header("--- Chip Generate Position ---")]
    public float minDragonX;
    public float maxDragonX;
    public float minDragonY;
    public float maxDragonY;

    public float minTieX;
    public float maxTieX;
    public float minTieY;
    public float maxTieY;


    public float minTigerX;
    public float maxTigerX;
    public float minTigerY;
    public float maxTigerY;

    public float downValue;
    public float upValue;

    [Header("--- Start Bet ---")]
    public GameObject startBetObj;
    public GameObject stopBetObj;

    [Header("--- Game Play Maintain ---")]

    public GameObject waitNextRoundScreenObj;

    public List<CardSuffle> cardSuffles = new List<CardSuffle>();
    public CardSuffle cardSuffle1;
    public CardSuffle cardSuffle2;
    public int cardNo1;
    public int cardNo2;
    public GameObject[] chipBtn;
    public GameObject chipObj;
    public float[] chipPrice;
    public Sprite[] chipsSprite;
    public GameObject dragonParent;
    public GameObject tigerParent;
    public GameObject tieParent;
    public GameObject ourProfile;
    public GameObject otherProfile;
    public GameObject cardObj;
    public GameObject cardCenterObj;
    public GameObject cardGen1;
    public GameObject cardGen2;
    public GameObject cardGenPre1;
    public GameObject cardGenPre2;

    public Text dragonPriceTxt;
    public Text tigerPriceTxt;
    public Text tiePriceTxt;

    public float dragonTotalPrice;
    public float tigerTotalPrice;
    public float tieTotalPrice;

    public int selectChipNo;
    public Text timerTxt;
    public float fixTimerValue;
    public float timerValue;
    public float secondCount;
    bool isEnterBetStop;

    public bool isWin = true;
    public int winNo = 0;


    bool isAdmin = false;
    public List<GameObject> genChipList_Dragon = new List<GameObject>();
    public List<GameObject> genChipList_Tiger = new List<GameObject>();
    public List<GameObject> genChipList_Tie = new List<GameObject>();

    public List<DragonTigerBetClass> dragonTigerBetClasses = new List<DragonTigerBetClass>();




    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void UpdateNameBalance()
    {
        userNameTxt.text = DataManager.Instance.playerData.firstName.ToString();
        balanceTxt.text = DataManager.Instance.playerData.balance.ToString();
    }

    // Start is called before the first frame update
    void Start()
    {

        StartCoroutine(DataManager.Instance.GetImages(PlayerPrefs.GetString("ProfileURL"), avatarImg));
        ChipAnimMaintain(0);

        CreateAdmin();

        if (DataManager.Instance.joinPlayerDatas.Count == 1 || isAdmin)
        {

            StartCoroutine(StartBet());
        }
        else
        {
            waitNextRoundScreenObj.SetActive(true);
        }
        UpdateNameBalance();

        //StartCoroutine(StartBet());
    }

    // Update is called once per frame
    void Update()
    {
        if (timerValue == 0 && isEnterBetStop == false && waitNextRoundScreenObj.activeSelf == false)
        {
            isEnterBetStop = true;
            //Stop Bet
            StartCoroutine(StopBet());
        }
        else if (!timerTxt.text.Equals("Star Betting Time 0"))
        {
            secondCount -= Time.deltaTime;
            timerValue = ((int)secondCount);
            timerTxt.text = "Star Betting Time " + timerValue.ToString();
        }

        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    RestartTimer();
        //}
    }


    #region Cards Maintain
    public void Card_Open_Match()
    {
        SoundManager.Instance.CasinoCardSwipeSound();
        cardGenPre1.transform.DOScale(new Vector3(0, 1, 1), 0.25f).OnComplete(() =>
        {
            cardGenPre1.transform.GetComponent<Image>().sprite = cardSuffle1.cardSprite;
            cardGenPre1.transform.DOScale(new Vector3(1, 1, 1), 0.25f).OnComplete(() =>
            {
                cardGenPre1.transform.DOScale(new Vector3(0.85f, 0.85f, 0.85f), 0.25f).OnComplete(() =>
                   {
                       cardGenPre1.transform.DOScale(new Vector3(1f, 1f, 1f), 0.1f).OnComplete(() =>
                       {
                           SecondCardNo();
                       });
                   });
            });
        });
    }

    void SecondCardNo()
    {
        SoundManager.Instance.CasinoCardSwipeSound();

        cardGenPre2.transform.DOScale(new Vector3(0, 1, 1), 0.25f).OnComplete(() =>
        {
            cardGenPre2.transform.GetComponent<Image>().sprite = cardSuffle2.cardSprite;
            cardGenPre2.transform.DOScale(new Vector3(1, 1, 1), 0.25f).OnComplete(() =>
            {
                cardGenPre2.transform.DOScale(new Vector3(0.85f, 0.85f, 0.85f), 0.25f).OnComplete(() =>
                {
                    cardGenPre2.transform.DOScale(new Vector3(1f, 1f, 1f), 0.1f).OnComplete(() =>
                    {
                        CardMatch();
                    });
                });
            });
        });
    }

    public void CardMatch()
    {
        isWin = true;
        winNo = 0;
        if (cardSuffle1.cardNo == cardSuffle2.cardNo)
        {
            winNo = 1; // tie
        }
        else if (cardSuffle1.cardNo > cardSuffle2.cardNo)
        {
            winNo = 2; // dragon
        }
        else if (cardSuffle1.cardNo < cardSuffle2.cardNo)
        {
            winNo = 3; // tiger
        }

        print("Win No : " + winNo);

        StartCoroutine(AnimationOpen(winNo));

    }
    //a

    public IEnumerator AnimationOpen(int winNo)
    {
        float waitTime = 0;
        if (winNo == 2 || winNo == 3)
        {
            waitTime = 3.16f;
            if (winNo == 2)
            {
                dragonAnim.SetActive(true);

            }
            else if (winNo == 3)
            {
                tigerAnim.SetActive(true);

            }
        }

        yield return new WaitForSeconds(waitTime);
        dragonAnim.SetActive(false);
        tigerAnim.SetActive(false);
        if (winNo == 1)
        {

            float animSpeed = 0.3f;
            int tNum = genChipList_Tie.Count + genChipList_Tiger.Count + genChipList_Dragon.Count;

            for (int i = 0; i < genChipList_Dragon.Count; i++)
            {
                int no = i;

                genChipList_Dragon[no].transform.DOScale(Vector3.zero, animSpeed);
                genChipList_Dragon[no].transform.DOMove(cardCenterObj.transform.position, animSpeed).OnComplete(() =>
                {
                    Vector3 rPos = new Vector3(UnityEngine.Random.Range(minTieX, maxTieX), UnityEngine.Random.Range(minTieY, maxTieY));
                    genChipList_Dragon[no].transform.DOMove(rPos, animSpeed);
                    genChipList_Dragon[no].transform.DOScale(Vector3.one, animSpeed);
                    genChipList_Dragon[no].transform.SetParent(tieParent.transform);
                    genChipList_Dragon.Add(genChipList_Dragon[no]);
                    genChipList_Dragon[no].transform.DORotate(new Vector3(0, 0, UnityEngine.Random.Range(0, 360)), animSpeed).OnComplete(() =>
                    {
                        UpdateList(tNum, genChipList_Tie, genChipList_Dragon);
                    });
                });
            }

            for (int i = 0; i < genChipList_Tiger.Count; i++)
            {

                int no = i;
                genChipList_Tiger[no].transform.DOScale(Vector3.zero, animSpeed);
                genChipList_Tiger[no].transform.DOMove(cardCenterObj.transform.position, animSpeed).OnComplete(() =>
                {

                    Vector3 rPos = new Vector3(UnityEngine.Random.Range(minTieX, maxTieX), UnityEngine.Random.Range(minTieY, maxTieY));
                    genChipList_Tiger[no].transform.DOMove(rPos, animSpeed);
                    genChipList_Tiger[no].transform.DOScale(Vector3.one, animSpeed);
                    genChipList_Tiger[no].transform.SetParent(tieParent.transform);
                    genChipList_Tiger.Add(genChipList_Dragon[no]);
                    genChipList_Tiger[no].transform.DORotate(new Vector3(0, 0, UnityEngine.Random.Range(0, 360)), animSpeed).OnComplete(() =>
                    {
                        genChipList_Tie.RemoveAt(no);

                        UpdateList(tNum, genChipList_Tie, genChipList_Tiger);

                    });
                });
            }
        }
        else if (winNo == 2)
        {
            float animSpeed = 0.3f;


            int tNum = genChipList_Tie.Count + genChipList_Tiger.Count + genChipList_Dragon.Count;
            for (int i = 0; i < genChipList_Tie.Count; i++)
            {
                int no = i;

                genChipList_Tie[no].transform.DOScale(Vector3.zero, animSpeed);
                genChipList_Tie[no].transform.DOMove(cardCenterObj.transform.position, animSpeed).OnComplete(() =>
                {
                    Vector3 rPos = new Vector3(UnityEngine.Random.Range(minDragonX, maxDragonX), UnityEngine.Random.Range(minDragonY, maxDragonY));
                    genChipList_Tie[no].transform.DOMove(rPos, animSpeed);

                    genChipList_Tie[no].transform.DOScale(Vector3.one, animSpeed);
                    genChipList_Tie[no].transform.SetParent(dragonParent.transform);
                    genChipList_Dragon.Add(genChipList_Tie[no]);

                    genChipList_Tie[no].transform.DORotate(new Vector3(0, 0, UnityEngine.Random.Range(0, 360)), animSpeed).OnComplete(() =>
                    {
                        genChipList_Tie.RemoveAt(no);

                        UpdateList(tNum, genChipList_Dragon, genChipList_Tie);
                    });
                });
            }

            for (int i = 0; i < genChipList_Tiger.Count; i++)
            {

                int no = i;
                genChipList_Tiger[no].transform.DOScale(Vector3.zero, animSpeed);
                genChipList_Tiger[no].transform.DOMove(cardCenterObj.transform.position, animSpeed).OnComplete(() =>
                {
                    Vector3 rPos = new Vector3(UnityEngine.Random.Range(minDragonX, maxDragonX), UnityEngine.Random.Range(minDragonY, maxDragonY));
                    genChipList_Tiger[no].transform.DOMove(rPos, animSpeed);
                    genChipList_Tiger[no].transform.DOScale(Vector3.one, animSpeed);
                    genChipList_Tiger[no].transform.SetParent(dragonParent.transform);
                    genChipList_Dragon.Add(genChipList_Tiger[no]);
                    genChipList_Tiger[no].transform.DORotate(new Vector3(0, 0, UnityEngine.Random.Range(0, 360)), animSpeed).OnComplete(() =>
                    {
                        genChipList_Tiger.Remove(genChipList_Tiger[no]);
                        UpdateList(tNum, genChipList_Dragon, genChipList_Tiger);
                    });
                });
            }
        }
        else if (winNo == 3)
        {
            float animSpeed = 0.3f;

            int tNum = genChipList_Tie.Count + genChipList_Tiger.Count + genChipList_Dragon.Count;
            for (int i = 0; i < genChipList_Dragon.Count; i++)
            {
                int no = i;
                genChipList_Dragon[no].transform.DOScale(Vector3.zero, animSpeed);
                genChipList_Dragon[no].transform.DOMove(cardCenterObj.transform.position, animSpeed).OnComplete(() =>
                {
                    Vector3 rPos = new Vector3(UnityEngine.Random.Range(minTigerX, maxTigerX), UnityEngine.Random.Range(minTigerY, maxTigerY));
                    genChipList_Dragon[no].transform.DOMove(rPos, animSpeed);
                    genChipList_Dragon[no].transform.DOScale(Vector3.one, animSpeed);

                    genChipList_Dragon[no].transform.SetParent(tigerParent.transform);
                    genChipList_Tiger.Add(genChipList_Dragon[no]);

                    genChipList_Dragon[no].transform.DORotate(new Vector3(0, 0, UnityEngine.Random.Range(0, 360)), animSpeed).OnComplete(() =>
                    {

                        UpdateList(tNum, genChipList_Tiger, genChipList_Dragon);

                    });
                });
            }

            for (int i = 0; i < genChipList_Tie.Count; i++)
            {
                int no = i;
                genChipList_Tie[no].transform.DOScale(Vector3.zero, animSpeed);

                genChipList_Tie[no].transform.DOMove(cardCenterObj.transform.position, animSpeed).OnComplete(() =>
                {
                    Vector3 rPos = new Vector3(UnityEngine.Random.Range(minTigerX, maxTigerX), UnityEngine.Random.Range(minTigerY, maxTigerY));
                    genChipList_Tie[no].transform.DOMove(rPos, animSpeed);
                    genChipList_Tie[no].transform.DOScale(Vector3.one, animSpeed);
                    genChipList_Tie[no].transform.SetParent(tigerParent.transform);
                    genChipList_Tiger.Add(genChipList_Tie[no]);
                    genChipList_Tie[no].transform.DORotate(new Vector3(0, 0, UnityEngine.Random.Range(0, 360)), animSpeed).OnComplete(() =>
                    {

                        UpdateList(tNum, genChipList_Tiger, genChipList_Tie);


                    });
                });
            }
        }

        int cNo = 0;
        if (winNo == 1)
        {
            cNo = 3;

        }
        else if (winNo == 2)
        {
            cNo = 1;
        }
        else if (winNo == 3)
        {
            cNo = 2;
        }

        float investPrice = 0;
        float otherPrice = 0;
        for (int i = 0; i < dragonTigerBetClasses.Count; i++)
        {
            if (dragonTigerBetClasses[i].no == cNo)
            {
                investPrice += dragonTigerBetClasses[i].price;
            }
            else
            {
                otherPrice += dragonTigerBetClasses[i].price;
            }
        }
        print("invest Price : " + investPrice);
        print("Other Price : " + otherPrice);
        float mainInvestPrice = investPrice;

        if (winNo == 1)
        {
            investPrice *= 8;
        }
        else if (winNo == 2)
        {
            investPrice *= 2;
        }
        else if (winNo == 3)
        {
            investPrice *= 2;
        }


        float adminPercentage = DataManager.Instance.adminPercentage;




        float winAmount = investPrice;
        float adminCommssion = (winAmount / adminPercentage);
        float playerWinAmount = winAmount - adminCommssion;

        otherPrice -= playerWinAmount;

        if (playerWinAmount != 0)
        {
            winParticles.SetActive(true);
            SoundManager.Instance.CasinoWinSound();
            winAnimationTxt.gameObject.SetActive(true);
            winAnimationTxt.text = "+" + playerWinAmount;
            Invoke(nameof(WinAmountTextOff), 1.5f);
            //DataManager.Instance.AddAmount((float)(playerWinAmount), TestSocketIO.Instace.roomid, "Dragon_Tiger-Win-" + TestSocketIO.Instace.roomid, "won", (float)(adminCommssion));
        }

        if (otherPrice != 0)
        {
            //otherPrice Direct Profit
        }
        Invoke(nameof(WinAfterRoundChange), 4f);
    }


    public void WinAmountTextOff()
    {
        winAnimationTxt.gameObject.SetActive(false);

    }
    void WinAfterRoundChange()
    {
        winParticles.SetActive(false);

        StartCoroutine(StartBet());
    }

    void UpdateList(int no, List<GameObject> list, List<GameObject> list1)
    {


        if (no == list.Count)
        {
            list1.Clear();
        }
        else
        {
            return;
        }
        float moveSpeed = 0.2f;

        if (winNo == 1)
        {
            if (genChipList_Dragon.Count == 0 && genChipList_Tiger.Count == 0)
            {
                for (int i = 0; i < genChipList_Tie.Count; i++)
                {
                    bool isEnter = false;
                    for (int j = 0; j < dragonTigerBetClasses.Count; j++)
                    {
                        if (dragonTigerBetClasses[j].betObj == genChipList_Tie[i])
                        {
                            isEnter = true;
                        }
                    }
                    int no1 = i;
                    if (isEnter == false)
                    {

                        genChipList_Tie[i].transform.DOMove(otherProfile.transform.position, moveSpeed);

                        genChipList_Tie[i].transform.DOScale(Vector3.zero, moveSpeed).OnComplete(() =>
                        {
                            Destroy(genChipList_Tie[i]);
                            if (no1 == genChipList_Tie.Count)
                            {
                                //Restart
                            }
                        });
                    }
                    else
                    {

                        genChipList_Tie[i].transform.DOMove(ourProfile.transform.position, moveSpeed);

                        genChipList_Tie[i].transform.DOScale(Vector3.zero, moveSpeed).OnComplete(() =>
                        {
                            Destroy(genChipList_Tie[i]);
                            if (no1 == genChipList_Tie.Count)
                            {
                                //Restart
                            }
                        });
                    }


                }
            }
        }
        else if (winNo == 2)
        {
            if (genChipList_Tie.Count == 0 && genChipList_Tiger.Count == 0)
            {
                for (int i = 0; i < genChipList_Dragon.Count; i++)
                {
                    bool isEnter = false;
                    for (int j = 0; j < dragonTigerBetClasses.Count; j++)
                    {
                        if (dragonTigerBetClasses[j].betObj == genChipList_Dragon[i])
                        {
                            isEnter = true;
                        }
                    }
                    int no1 = i;
                    if (isEnter == false)
                    {

                        genChipList_Dragon[i].transform.DOMove(otherProfile.transform.position, moveSpeed);
                        genChipList_Dragon[i].transform.DOScale(Vector3.zero, moveSpeed).OnComplete(() =>
                        {
                            if (no1 == genChipList_Dragon.Count)
                            {
                                //Restart
                            }
                            Destroy(genChipList_Dragon[i]);
                        });
                    }
                    else
                    {

                        genChipList_Dragon[i].transform.DOMove(ourProfile.transform.position, moveSpeed);
                        genChipList_Dragon[i].transform.DOScale(Vector3.zero, moveSpeed).OnComplete(() =>
                        {
                            if (no1 == genChipList_Dragon.Count)
                            {
                                //Restart
                            }
                            Destroy(genChipList_Dragon[i]);
                        });
                    }


                }
            }
        }
        else if (winNo == 3)
        {
            if (genChipList_Dragon.Count == 0 && genChipList_Tie.Count == 0)
            {
                for (int i = 0; i < genChipList_Tiger.Count; i++)
                {
                    bool isEnter = false;
                    for (int j = 0; j < dragonTigerBetClasses.Count; j++)
                    {
                        if (dragonTigerBetClasses[j].betObj == genChipList_Tiger[i])
                        {
                            isEnter = true;
                        }
                    }
                    int no1 = i;
                    if (isEnter == false)
                    {

                        genChipList_Tiger[i].transform.DOMove(otherProfile.transform.position, moveSpeed);
                        genChipList_Tiger[i].transform.DOScale(Vector3.zero, moveSpeed).OnComplete(() =>
                        {
                            Destroy(genChipList_Tiger[i]);
                            if (no1 == genChipList_Tiger.Count)
                            {
                                //Restart
                            }
                        });
                    }
                    else
                    {

                        genChipList_Tiger[i].transform.DOMove(ourProfile.transform.position, moveSpeed);
                        genChipList_Tiger[i].transform.DOScale(Vector3.zero, moveSpeed).OnComplete(() =>
                        {
                            Destroy(genChipList_Tiger[i]);
                            if (no1 == genChipList_Tiger.Count)
                            {
                                //Restart
                            }
                        });
                    }


                }
            }
        }



    }

    void UpdateBoardPrice()
    {
        dragonPriceTxt.text = dragonTotalPrice.ToString("F2");
        tigerPriceTxt.text = tigerTotalPrice.ToString("F2");
        tiePriceTxt.text = tieTotalPrice.ToString("F2");
    }

    #endregion


    #region Round Maintain

    public IEnumerator StartBet()
    {
        SoundManager.Instance.CasinoTurnSound();
        DataManager.Instance.UserTurnVibrate();
        if (cardGenPre1 != null)
        {
            Destroy(cardGenPre1);
        }
        if (cardGenPre2 != null)
        {
            Destroy(cardGenPre2);
        }
        isEnterBetStop = true;
        startBetObj.SetActive(true);
        if (isAdmin)
        {
            cardNo1 = UnityEngine.Random.Range(0, 53);
            cardNo2 = UnityEngine.Random.Range(0, 53);

            //cardNo1 = 0;
            //cardNo2 = 1;


            while (cardNo1 == cardNo2)
            {
                cardNo2 = UnityEngine.Random.Range(0, 53);
            }

            cardSuffle1 = cardSuffles[cardNo1];
            cardSuffle2 = cardSuffles[cardNo2];
            SetRoomData(cardNo1, cardNo2)
;
        }
        yield return new WaitForSeconds(1.35f);
        startBetObj.SetActive(false);
        RestartTimer();

    }

    public IEnumerator StopBet()
    {

        isEnterBetStop = true;
        stopBetObj.SetActive(true);
        yield return new WaitForSeconds(1.02f);
        stopBetObj.SetActive(false);
        Card_Open_Match();
    }

    public void RestartTimer()
    {
        dragonTigerBetClasses.Clear();
        genChipList_Dragon.Clear();
        genChipList_Tiger.Clear();
        genChipList_Tie.Clear();
        isWin = false;
        timerValue = fixTimerValue;
        secondCount = timerValue;
        timerTxt.text = "Star Betting Time " + timerValue.ToString();
        isEnterBetStop = false;

        dragonTotalPrice = 0;
        tigerTotalPrice = 0;
        tieTotalPrice = 0;
        UpdateBoardPrice();

        //        if (isAdmin)
        //        {
        //            cardNo1 = UnityEngine.Random.Range(0, 53);
        //            cardNo2 = UnityEngine.Random.Range(0, 53);

        //            //cardNo1 = 0;
        //            //cardNo2 = 1;


        //            while (cardNo1 == cardNo2)
        //            {
        //                cardNo2 = UnityEngine.Random.Range(0, 53);
        //            }

        //            cardSuffle1 = cardSuffles[cardNo1];
        //            cardSuffle2 = cardSuffles[cardNo2];
        //            SetRoomData(cardNo1, cardNo2)
        //;
        //        }

        GenerateCards();
    }

    void GenerateCards()
    {
        float moveSpeed = 0.5f;
        cardGenPre1 = Instantiate(cardObj, cardGen1.transform);
        cardGenPre1.transform.position = cardCenterObj.transform.position;
        cardGenPre1.transform.localScale = Vector3.zero;
        cardGenPre1.transform.DOMove(cardGen1.transform.position, moveSpeed);
        cardGenPre1.transform.DOScale(Vector3.one, moveSpeed + 0.1f);
        SoundManager.Instance.CasinoCardMoveSound();

        cardGenPre2 = Instantiate(cardObj, cardGen2.transform);
        cardGenPre2.transform.position = cardCenterObj.transform.position;
        cardGenPre2.transform.localScale = Vector3.zero;
        cardGenPre2.transform.DOMove(cardGen2.transform.position, moveSpeed);
        SoundManager.Instance.CasinoCardMoveSound();
        cardGenPre2.transform.DOScale(Vector3.one, moveSpeed + 0.1f).OnComplete(() =>
        {

            //Game Continue
        });


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

    #region GamePlay

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


    public void GameThreeButton(int no)
    {
        if (no == 1)
        {
            bool isMoneyAv = CheckMoney(chipPrice[selectChipNo]);
            if (isMoneyAv == false)
            {
                SoundManager.Instance.ButtonClick();
                OpenErrorScreen();
                return;
            }

            SoundManager.Instance.ThreeBetSound();
            //DataManager.Instance.DebitAmount(((float)(chipPrice[selectChipNo])).ToString(), TestSocketIO.Instace.roomid, "Dragon_Tiger-Bet-" + TestSocketIO.Instace.roomid, "game");

            Vector3 rPos = new Vector3(UnityEngine.Random.Range(minDragonX, maxDragonX), UnityEngine.Random.Range(minDragonY, maxDragonY));
            GameObject chipGen = Instantiate(chipObj, dragonParent.transform);
            chipGen.transform.GetComponent<Image>().sprite = chipsSprite[selectChipNo];
            chipGen.transform.position = ourProfile.transform.position;
            dragonTotalPrice += chipPrice[selectChipNo];
            genChipList_Dragon.Add(chipGen);
            ChipGenerate(chipGen, rPos);

            DragonTigerBetClass betClass = new DragonTigerBetClass();
            betClass.no = no;
            betClass.price = chipPrice[selectChipNo];
            betClass.betObj = chipGen;
            dragonTigerBetClasses.Add(betClass);
        }
        else if (no == 2)
        {
            bool isMoneyAv = CheckMoney(chipPrice[selectChipNo]);
            if (isMoneyAv == false)
            {
                SoundManager.Instance.ButtonClick();

                OpenErrorScreen();
                return;
            }

            SoundManager.Instance.ThreeBetSound();
            //DataManager.Instance.DebitAmount(((float)(chipPrice[selectChipNo])).ToString(), TestSocketIO.Instace.roomid, "Dragon_Tiger-Bet-" + TestSocketIO.Instace.roomid, "game");

            Vector3 rPos = new Vector3(UnityEngine.Random.Range(minTigerX, maxTigerX), UnityEngine.Random.Range(minTigerY, maxTigerY));
            GameObject chipGen = Instantiate(chipObj, tigerParent.transform);
            chipGen.transform.GetComponent<Image>().sprite = chipsSprite[selectChipNo];
            chipGen.transform.position = ourProfile.transform.position;
            tigerTotalPrice += chipPrice[selectChipNo];
            genChipList_Tiger.Add(chipGen);
            ChipGenerate(chipGen, rPos);

            DragonTigerBetClass betClass = new DragonTigerBetClass();
            betClass.no = no;
            betClass.price = chipPrice[selectChipNo];
            betClass.betObj = chipGen;

            dragonTigerBetClasses.Add(betClass);
        }
        else if (no == 3)
        {
            bool isMoneyAv = CheckMoney(chipPrice[selectChipNo]);
            if (isMoneyAv == false)
            {
                SoundManager.Instance.ButtonClick();
                OpenErrorScreen();
                return;
            }

            SoundManager.Instance.ThreeBetSound();
            //DataManager.Instance.DebitAmount(((float)(chipPrice[selectChipNo])).ToString(), TestSocketIO.Instace.roomid, "Dragon_Tiger-Bet-" + TestSocketIO.Instace.roomid, "game");

            Vector3 rPos = new Vector3(UnityEngine.Random.Range(minTieX, maxTieX), UnityEngine.Random.Range(minTieY, maxTieY));
            GameObject chipGen = Instantiate(chipObj, tieParent.transform);
            chipGen.transform.GetComponent<Image>().sprite = chipsSprite[selectChipNo];
            genChipList_Tiger.Add(chipGen);
            chipGen.transform.position = ourProfile.transform.position;
            tieTotalPrice += chipPrice[selectChipNo];
            genChipList_Tie.Add(chipGen);
            ChipGenerate(chipGen, rPos);

            DragonTigerBetClass betClass = new DragonTigerBetClass();
            betClass.no = no;
            betClass.price = chipPrice[selectChipNo];
            betClass.betObj = chipGen;
            dragonTigerBetClasses.Add(betClass);
        }

        SendDargonTigerBet(no, selectChipNo);
        UpdateBoardPrice();
    }

    public void ChipButtonClick(int no)
    {
        SoundManager.Instance.ButtonClick();
        ChipAnimMaintain(no);
    }

    void ChipAnimMaintain(int no)
    {
        selectChipNo = no;
        for (int i = 0; i < chipBtn.Length; i++)
        {
            if (i == no)
            {
                chipBtn[i].transform.DOMoveY(upValue, 0.05f);
            }
            else
            {
                chipBtn[i].transform.DOMoveY(downValue, 0.05f);
            }
        }
    }

    #region Chip Genearte

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

    #endregion

    #endregion

    #region Socket Maintain

    public void SendDargonTigerBet(int boxNo, int chipNo)
    {

        JSONObject obj = new JSONObject();
        obj.AddField("PlayerID", DataManager.Instance.playerData._id);
        obj.AddField("TournamentID", DataManager.Instance.tournamentID);
        obj.AddField("RoomId", TestSocketIO.Instace.roomid);
        obj.AddField("boxNo", boxNo);
        obj.AddField("chipNo", chipNo);
        TestSocketIO.Instace.Senddata("SendDragonTigerBet", obj);
    }


    public void GetDragonTigerBet(int boxNo, int chipNo)
    {
        if (boxNo == 1)
        {
            Vector3 rPos = new Vector3(UnityEngine.Random.Range(minDragonX, maxDragonX), UnityEngine.Random.Range(minDragonY, maxDragonY));
            GameObject chipGen = Instantiate(chipObj, dragonParent.transform);
            chipGen.transform.GetComponent<Image>().sprite = chipsSprite[chipNo];
            chipGen.transform.position = otherProfile.transform.position;
            dragonTotalPrice += chipPrice[chipNo];
            genChipList_Dragon.Add(chipGen);
            ChipGenerate(chipGen, rPos);
        }
        else if (boxNo == 2)
        {
            Vector3 rPos = new Vector3(UnityEngine.Random.Range(minTigerX, maxTigerX), UnityEngine.Random.Range(minTigerY, maxTigerY));
            GameObject chipGen = Instantiate(chipObj, tigerParent.transform);
            chipGen.transform.GetComponent<Image>().sprite = chipsSprite[chipNo];
            chipGen.transform.position = otherProfile.transform.position;
            tigerTotalPrice += chipPrice[chipNo];
            genChipList_Tiger.Add(chipGen);
            ChipGenerate(chipGen, rPos);
        }
        else if (boxNo == 3)
        {
            Vector3 rPos = new Vector3(UnityEngine.Random.Range(minTieX, maxTieX), UnityEngine.Random.Range(minTieY, maxTieY));
            GameObject chipGen = Instantiate(chipObj, tieParent.transform);
            chipGen.transform.GetComponent<Image>().sprite = chipsSprite[chipNo];
            genChipList_Tiger.Add(chipGen);
            chipGen.transform.position = otherProfile.transform.position;
            tieTotalPrice += chipPrice[chipNo];
            genChipList_Tie.Add(chipGen);
            ChipGenerate(chipGen, rPos);
        }
        UpdateBoardPrice();
    }
    #endregion


    #region Admin Maintain

    void CreateAdmin()
    {
        if (DataManager.Instance.joinPlayerDatas[0].userId.Equals(DataManager.Instance.playerData._id))
        {
            isAdmin = true;
        }
    }

    public void SetRoomData(int no1, int no2)
    {
        JSONObject obj = new JSONObject();
        obj.AddField("DeckNo1", no1);
        obj.AddField("DeckNo2", no2);
        //obj.AddField("DeckNo", 300);
        obj.AddField("dateTime", DateTime.UtcNow.ToString());
        obj.AddField("gameMode", 4);
        TestSocketIO.Instace.SetRoomdata(TestSocketIO.Instace.roomid, obj);
    }

    public void GetRoomData(int no1, int no2)
    {
        cardNo1 = no1;
        cardNo2 = no2;


        cardSuffle1 = cardSuffles[cardNo1];
        cardSuffle2 = cardSuffles[cardNo2];

        if (!isAdmin)
        {
            //deckNo = no;
            StartCoroutine(StartBet());
            //RoundGenerate();
            if (waitNextRoundScreenObj.activeSelf == true)
            {
                waitNextRoundScreenObj.SetActive(false);
            }
        }
    }

    public void ChangeAAdmin(string leavePlayerId, string adminId)
    {
        if (DataManager.Instance.playerData._id.Equals(DataManager.Instance.leaveUpdatePlayerDatas[0].userId))
        {
            isAdmin = true;
            if (DataManager.Instance.joinPlayerDatas.Count == 1 && waitNextRoundScreenObj.activeSelf == true)
            {
                StartCoroutine(StartBet());
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
    }

    #endregion

}
