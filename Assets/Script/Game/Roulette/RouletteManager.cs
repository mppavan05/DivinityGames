using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using JetBrains.Annotations;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


[System.Serializable]
public class ChipPlaceClass
{
    public int chipNo;
    public List<int> avaliableListNo = new List<int>();
}

[System.Serializable]
public class RouleteeBetClass
{
    public int placeNo;
    public int betImgNo;
    public int betTotalAmount;
    public GameObject chipObj;
    public RouletteButtonData rouletteButtonData;
    public List<GameObject> blackPanelList = new List<GameObject>();
}

public class RouletteManager : MonoBehaviour
{

    public static RouletteManager Instance;

    public Image avatarImg;

    [Header("---Chips Manage---")]
    public Image chipImg;
    public Sprite[] chipSprite;
    public Button plusBtn;
    public Button minusBtn;
    public int betChipNo;
    public float[] betChipPrices;

    public List<int> noList = new List<int>();

    public int noGen;
    public List<GameObject> triggerObj = new List<GameObject>();
    public GameObject findTriggerObj;

    public bool isRoundOn;

    public GameObject startBettingObj;
    public Animator startBetAnim;

    public GameObject stopBettingObj;
    public Animator stopBetAnim;
    //FirstClipComplete

    [Header("--- Info Screen ---")]
    public GameObject infoScreenObj;
    public GameObject[] innerInfoObj;
    public Button leftButton;
    public Button rightButton;

    [Header("--- Menu Screen ---")]
    public GameObject menuScreenObj;

    [Header("--- Wait New Round Screen ---")]
    public GameObject waitNextRoundScreenObj;

    [Header("--- Error Screen ---")]
    public GameObject errorScreenObj;

    [Header("--- Shop Prefab ---")]
    public GameObject shopPrefab;
    public GameObject shopPrefabParent;

    [Header("--- Game UI ---")]
    public Text balanceTxt;
    public Text userNameTxt;


    [Header("---Game Use---")]

    public List<int> a12_1;
    public List<int> a12_2;
    public List<int> a12_3;

    public List<int> b12_1;
    public List<int> b12_2;
    public List<int> b12_3;

    public List<int> start18;
    public List<int> last18;

    public List<int> evenNo;
    public List<int> oddNo;

    public List<int> redNo;
    public List<int> blackNo;

    [Header("--- Bet Manage  & Game Play ---")]
    public Sprite socketChip;
    public GameObject rouletteBoardObj;
    public Image[] greenLineBoard;
    public List<GameObject> blackPanelObj;
    public List<GameObject> btnParentPanelObj;
    public List<RouletteButtonData> btnPanelObj;
    public GameObject chipPrefab;
    public GameObject chipAnimParent;
    public GameObject cancelPrefab;
    public int[] chipsValue;
    public List<RouleteeBetClass> rouleteeBets = new List<RouleteeBetClass>();
    public List<RouleteeBetClass> rouleteeBetsBefore = new List<RouleteeBetClass>();
    public List<int> undoBlockList = new List<int>();
    public Color colorOnGreen;
    public Color colorOffGreen;
    public WheelRoulette wheelRoulette;
    public BallRoulette ballRoulette;
    public GameObject ballStartPos;
    public Text timerTxt;
    public bool isGameRouletteStart;
    public bool isAveUndo;
    public int findNo;
    public float fixTimeSet;
    float secondCount = 0;
    public int timerValue = 0;

    bool isEnterTheRoulette = false;
    public bool isAdmin;
    public bool isStopBet;

    public float totalCurrentInvest = 0;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            NewPlayerEnter();
        }
        UpdateNameBalance();

    }


    public void ReGenerateBoard()
    {
        for (int i = 0; i < triggerObj.Count; i++)
        {
            triggerObj[i].transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(false);
            triggerObj[i].transform.GetChild(0).gameObject.SetActive(false);
        }

        findNo = 0;
        for (int i = 0; i < noList.Count; i++)
        {
            if (noList[i] == noGen)
            {
                findNo = i;
                break;
            }
        }
        findTriggerObj = triggerObj[findNo];
        wheelRoulette.transform.rotation = Quaternion.Euler(Vector3.zero);
        //ballRoulette.transform.position = ballStartPos.transform.position;
        wheelRoulette.UpdateWheel();
        wheelRoulette.speed = 13;
        wheelRoulette.collider.radius = 308.7f;

        for (int i = 0; i < greenLineBoard.Length; i++)
        {
            greenLineBoard[i].color = colorOffGreen;
        }
        ballRoulette.UpdateBallSecond();
        isRoundOn = false;
        isGameRouletteStart = true;
    }



    public void UpdateNameBalance()
    {
        userNameTxt.text = DataManager.Instance.playerData.firstName.ToString();
        balanceTxt.text = DataManager.Instance.playerData.balance.ToString();
    }

    public void ObjectAvaliable()
    {
        //print(findTriggerObj.gameObject.name);
        findTriggerObj.transform.GetChild(0).gameObject.SetActive(true);
    }

    bool isStart = false;
    // Start is called before the first frame update
    void Start()
    {
        betChipNo = 0;
        //CreateAdmin();
        ChipUpdateUI();

        if (Instance == null)
        {

            //isStart = true;
        }
        StartCoroutine(DataManager.Instance.GetImages(PlayerPrefs.GetString("ProfileURL"), avatarImg));

    }

    #region New Player Entry Time Maintain

    public void NewPlayerEnter()
    {
        if (DataManager.Instance.joinPlayerDatas.Count == 1)
        {
            isEnterTheRoulette = true;
            //RestartTimer();

            StartCoroutine(StartBettingOff());
            CreateAdmin();
        }
        else
        {

            isEnterTheRoulette = true;

            print("Enter The New Player Time");
            waitNextRoundScreenObj.SetActive(true);
            FindDataAdminRouletee();
            //Get Details Admin


        }
    }
    #endregion


    public void GiveUserData()
    {

        //RestartTimer();
        StartCoroutine(StartBettingOff());
    }

    IEnumerator StartBettingOff()
    {
        SoundManager.Instance.CasinoTurnSound();
        isStopBet = true;
        for (int i = 0; i < blackPanelObj.Count; i++)
        {
            blackPanelObj[i].SetActive(false);
        }
        for (int i = 0; i < btnParentPanelObj.Count; i++)
        {
            for (int j = 0; j < btnParentPanelObj[i].transform.childCount; j++)
            {
                Destroy(btnParentPanelObj[i].transform.GetChild(j).transform.gameObject);
            }
        }
        startBettingObj.SetActive(true);
        yield return new WaitForSeconds(3f);

        rouleteeBetsBefore.Clear();
        for (int i = 0; i < rouleteeBets.Count; i++)
        {
            RouleteeBetClass rouleteeBet = rouleteeBets[i];
            rouleteeBetsBefore.Add(rouleteeBet);


        }
        rouleteeBets.Clear();
        startBetAnim.SetInteger("FirstClipComplete", 1);
        yield return new WaitForSeconds(1f);
        startBettingObj.SetActive(false);
        RestartTimer();
    }

    IEnumerator StopBettingOff()
    {

        stopBettingObj.SetActive(true);
        yield return new WaitForSeconds(3f);

        stopBetAnim.SetInteger("FirstClipComplete", 0);
        yield return new WaitForSeconds(1f);
        stopBettingObj.SetActive(false);
        ReGenerateBoard();
        rouletteBoardObj.SetActive(true);
    }

    public void RestartTimer()
    {
        //for (int i = 0; i < blackPanelObj.Count; i++)
        //{
        //    blackPanelObj[i].SetActive(false);
        //}
        //for (int i = 0; i < btnParentPanelObj.Count; i++)
        //{
        //    for (int j = 0; j < btnParentPanelObj[i].transform.childCount; j++)
        //    {
        //        Destroy(btnParentPanelObj[i].transform.GetChild(j));
        //    }
        //}


        if (isAdmin)
        {
            SetRoomData();
        }
        isStopBet = false;
        isEnterTheRoulette = false;
        timerValue = ((int)fixTimeSet);
        timerTxt.text = timerValue.ToString();
        secondCount = fixTimeSet;
    }

    // Update is called once per frame
    void Update()
    {
        if (timerValue == 0 && isEnterTheRoulette == false)
        {
            isEnterTheRoulette = true;
            StartCoroutine(StopBettingOff());

        }
        else if (!timerTxt.text.Equals("0"))
        {
            secondCount -= Time.deltaTime;
            timerValue = ((int)secondCount);
            timerTxt.text = timerValue.ToString();
        }

        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    ReGenerateBoard();
        //    rouletteBoardObj.SetActive(true);
        //}
    }
    #region Menu Screen

    public void OpenMenuScreen()
    {
        menuScreenObj.SetActive(true);
    }

    public void HomeButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        TestSocketIO.Instace.LeaveRoom();
        SceneManager.LoadScene("Main");
    }

    public void InfoButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        OpenInfoScreenObj();
    }

    public void StoreButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        Instantiate(shopPrefab, shopPrefabParent.transform);
    }
    public void CloseMenuScreen()
    {
        SoundManager.Instance.ButtonClick();
        menuScreenObj.SetActive(false);
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

    #region Info Screen

    int infoScreenNo = 0;
    public void OpenInfoScreenObj()
    {
        infoScreenObj.SetActive(true);
        infoScreenNo = 0;
        UpdateInfo();
    }

    public void LeftButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        infoScreenNo--;
        UpdateInfo();
    }

    public void RightButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        infoScreenNo++;
        UpdateInfo();
    }

    void UpdateInfo()
    {
        if (infoScreenNo == 0)
        {
            leftButton.interactable = false;
            rightButton.interactable = true;
        }
        else if (infoScreenNo == innerInfoObj.Length - 1)
        {
            leftButton.interactable = true;
            rightButton.interactable = false;
        }
        else
        {
            leftButton.interactable = true;
            rightButton.interactable = true;
        }

        for (int i = 0; i < innerInfoObj.Length; i++)
        {
            if (i == infoScreenNo)
            {
                innerInfoObj[i].SetActive(true);
            }
            else
            {
                innerInfoObj[i].SetActive(false);
            }
        }


    }


    public void BackToInfoButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        infoScreenObj.SetActive(false);
    }

    #endregion

    #region Button Click

    public void SpinButtonClick()
    {

    }

    public void UndoButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        if (rouleteeBets.Count != 0)
        {
            //isAveUndo = true;


            UndoManage();
        }
    }
    public void RebeatButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        if (rouleteeBetsBefore.Count > 0)
        {
            OnRebet();
        }
    }

    public void ClearButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        ClearFunction();
    }

    public void MenuButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        OpenMenuScreen();
    }

    public void MinButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        betChipNo--;
        ChipUpdateUI();
    }

    public void MaxButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        betChipNo++;
        ChipUpdateUI();
    }


    #endregion

    #region Rebet

    void OnRebet()
    {
        if (rouleteeBets.Count != 0)
        {
            ClearFunction();
        }

        for (int i = 0; i < rouleteeBetsBefore.Count; i++)
        {
            rouleteeBetsBefore[i].rouletteButtonData.SimpleRebetGenerate(rouleteeBetsBefore[i]);
        }
    }

    #endregion

    #region Other

    void ChipUpdateUI()
    {
        if (betChipNo == 0)
        {
            minusBtn.interactable = false;
            plusBtn.interactable = true;
        }
        else if (betChipNo == chipSprite.Length - 1)
        {
            minusBtn.interactable = true;
            plusBtn.interactable = false;
        }
        else
        {
            minusBtn.interactable = true;
            plusBtn.interactable = true;
        }

        chipImg.sprite = chipSprite[betChipNo];
    }

    #endregion



    #region Undo Manage
    void ClearFunction()
    {
        for (int i = 0; i < rouleteeBets.Count; i++)
        {
            for (int j = 0; j < rouleteeBets[i].blackPanelList.Count; j++)
            {
                rouleteeBets[i].blackPanelList[j].SetActive(false);
            }
            GameObject chipObj = rouleteeBets[i].chipObj;
            chipObj.transform.DOMove(chipAnimParent.transform.position, 0.2f).OnComplete(() =>
            {


            });
        }

        rouleteeBets.Clear();
    }

    public void ClearSpecificNo(int no)
    {
        for (int i = 0; i < rouleteeBets.Count; i++)
        {
            if (rouleteeBets[i].placeNo == no)
            {
                for (int j = 0; j < rouleteeBets[i].blackPanelList.Count; j++)
                {
                    rouleteeBets[i].blackPanelList[j].SetActive(false);
                }
                GameObject chipObj = rouleteeBets[i].chipObj;
                chipObj.transform.DOMove(chipAnimParent.transform.position, 0.2f).OnComplete(() =>
                {


                });

                rouleteeBets.Remove(rouleteeBets[i]);
            }

        }

        //rouleteeBets.Remove(no);

    }

    void UndoManage()
    {
        if (isAveUndo == false)
        {
            isAveUndo = true;
            for (int i = 0; i < rouleteeBets.Count; i++)
            {
                GameObject chipObj = rouleteeBets[i].chipObj;
                GameObject closeGenObj = Instantiate(cancelPrefab, chipObj.transform);
                closeGenObj.name = "CloseChip";
                undoBlockList.Add(rouleteeBets[i].placeNo);
            }
        }
        else
        {
            for (int i = 0; i < rouleteeBets.Count; i++)
            {
                GameObject chipObj = rouleteeBets[i].chipObj;
                for (int j = 0; j < chipObj.transform.childCount; j++)
                {
                    GameObject closeObj = chipObj.transform.GetChild(j).gameObject;
                    if (closeObj.name == "CloseChip")
                    {
                        Destroy(closeObj);
                        isAveUndo = false;
                    }
                }
                undoBlockList.Clear();
            }
        }
    }

    #endregion

    #region Win Rules

    public float WinRuleNo(int sNo)
    {
        int winRuleNo = 0;
        float mulValue = 0;
        if (sNo >= 0 && sNo <= 36)
        {
            winRuleNo = 1;
            mulValue = 35;
        }
        if (sNo >= 71 && sNo <= 127)
        {
            winRuleNo = 2;
            mulValue = 17;
        }
        if (sNo >= 130 && sNo <= 141)
        {
            winRuleNo = 3;
            mulValue = 11;
        }
        if (sNo >= 49 && sNo <= 70)
        {
            winRuleNo = 4;
            mulValue = 8;
        }
        if (sNo == 128 || sNo == 129)
        {
            winRuleNo = 5;
            mulValue = 5;
        }
        if (sNo == 45 || sNo == 46)
        {
            winRuleNo = 6;
            mulValue = 1;
        }
        if (sNo == 44 || sNo == 47)
        {
            winRuleNo = 7;
            mulValue = 1;
        }
        if (sNo == 43 || sNo == 48)
        {
            winRuleNo = 8;
            mulValue = 1;
        }

        if (sNo == 40 || sNo == 41 || sNo == 42)
        {
            winRuleNo = 9;
            mulValue = 2;
        }

        if (sNo == 40 || sNo == 41 || sNo == 42)
        {
            winRuleNo = 10;
            mulValue = 2;
        }
        return mulValue;
    }

    public float WinManager()
    {
        float winvalue = 0;
        for (int i = 0; i < rouleteeBets.Count; i++)
        {
            if (rouleteeBets[i].rouletteButtonData.btnAvaliableNo.Contains(noGen))
            {
                winvalue += (rouleteeBets[i].betTotalAmount * WinRuleNo(rouleteeBets[i].rouletteButtonData.chipNo));
            }
        }
        return winvalue;
    }

    #endregion


    #region Check Money

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

    public float GetPrice(int no)
    {
        return betChipPrices[no];
    }

    #endregion

    #region Socket Maintain

    public void GetBetSocket(int no)
    {
        for (int i = 0; i < btnPanelObj.Count; i++)
        {
            if (btnPanelObj[i].chipNo == no)
            {
                btnPanelObj[i].Add_Socket_Chip(socketChip);
            }
        }

    }

    #endregion

    #region Admin Maintain

    void CreateAdmin()
    {
        if (DataManager.Instance.joinPlayerDatas[0].userId.Equals(DataManager.Instance.playerData._id))
        {
            isAdmin = true;
            SetRoomData();
        }
    }

    public void SetRoomData()
    {
        JSONObject obj = new JSONObject();
        //obj.AddField("DeckNo", UnityEngine.Random.Range(0, 37));
        obj.AddField("DeckNo", 2);
        obj.AddField("dateTime", DateTime.UtcNow.ToString());
        obj.AddField("gameMode", 2);
        TestSocketIO.Instace.SetRoomdata(TestSocketIO.Instace.roomid, obj);
    }

    public void GetRoomData(int no)
    {
        noGen = no;
        if (waitNextRoundScreenObj.activeSelf == true)
        {
            isEnterTheRoulette = true;
            waitNextRoundScreenObj.SetActive(false);
            timerValue = (int)fixTimeSet;
            secondCount = timerValue;
            SoundManager.Instance.CasinoTurnSound();
            DataManager.Instance.UserTurnVibrate();
            CenterToAddUser();
        }

    }

    public void ChangeAAdmin(string leavePlayerId, string adminId)
    {
        DataManager.Instance.joinPlayerDatas.Clear();
        for (int i = 0; i < DataManager.Instance.leaveUpdatePlayerDatas.Count; i++)
        {
            DataManager.Instance.joinPlayerDatas.Add(DataManager.Instance.leaveUpdatePlayerDatas[i]);
        }
        if (DataManager.Instance.playerData._id.Equals(DataManager.Instance.leaveUpdatePlayerDatas[0].userId))
        {
            isAdmin = true;
            if (DataManager.Instance.joinPlayerDatas.Count == 1)
            {
                //RoundGenerate();

                StartCoroutine(StartBettingOff());
            }
        }
        else
        {
            isAdmin = false;
        }


    }


    public void FindDataAdminRouletee()
    {

        JSONObject obj = new JSONObject();
        obj.AddField("PlayerID", DataManager.Instance.playerData._id);
        obj.AddField("TournamentID", DataManager.Instance.tournamentID);
        obj.AddField("RoomId", TestSocketIO.Instace.roomid);

        TestSocketIO.Instace.Senddata("FindDataRouletteAdmin", obj);
    }

    public void SendAdminDataPlayer(string playerID)
    {

        JSONObject obj = new JSONObject();
        obj.AddField("AdminPlayerID", DataManager.Instance.playerData._id);
        obj.AddField("ReceivePlayerID", playerID);
        obj.AddField("TournamentID", DataManager.Instance.tournamentID);
        obj.AddField("RoomId", TestSocketIO.Instace.roomid);
        obj.AddField("Time", timerValue);
        obj.AddField("RouletteNumber", noGen);
        TestSocketIO.Instace.Senddata("SendAdminRouleteeData", obj);


    }

    public void GetAdminDataPlayer(int time, int no)
    {
        if (time > 4)
        {
            isEnterTheRoulette = true;
            waitNextRoundScreenObj.SetActive(false);
            timerValue = time;
            secondCount = timerValue;
            noGen = no;
            CenterToAddUser();
            //aaa
        }
    }


    void CenterToAddUser()
    {
        isStopBet = true;
        totalCurrentInvest = 0;
        for (int i = 0; i < blackPanelObj.Count; i++)
        {
            blackPanelObj[i].SetActive(false);
        }
        for (int i = 0; i < btnParentPanelObj.Count; i++)
        {
            for (int j = 0; j < btnParentPanelObj[i].transform.childCount; j++)
            {
                Destroy(btnParentPanelObj[i].transform.GetChild(j).transform.gameObject);
            }
        }

        rouleteeBetsBefore.Clear();
        for (int i = 0; i < rouleteeBets.Count; i++)
        {
            RouleteeBetClass rouleteeBet = rouleteeBets[i];
            rouleteeBetsBefore.Add(rouleteeBet);
        }
        rouleteeBets.Clear();



        isStopBet = false;
        isEnterTheRoulette = false;
        //timerValue = ((int)fixTimeSet);
        timerTxt.text = timerValue.ToString();
        //secondCount = fixTimeSet;
    }

    #endregion
}
//Complete