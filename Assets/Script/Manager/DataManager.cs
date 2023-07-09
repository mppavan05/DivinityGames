using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using SimpleJSON;
using UnityEngine.Networking;
using System;
using UnityEngine.UI;

public enum GameType
{
    None,
    Teen_Patti,
    Rummy,
    Ball_Pool,
    Poker,
    Archery,
    Ludo,
    Slot,
    Carrom,
    Chess
}

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;



    public string url;
    public PlayerData playerData;

    public float adminPercentage;

    public GameType gameMode;


    public int playerNo;
    public bool isTwoPlayer;
    public bool isFourPlayer;

    [Header("--- Basic ---")]
    public string gameType;
    public string appVersion;
    public string appUrl;
    public int currentPNo;

    [Header("---Tournament---")]
    public float tourEntryMoney;
    public float winAmount;
    public string tournamentID;
    public string gameId;
    public List<JoinPlayerData> joinPlayerDatas = new List<JoinPlayerData>();
    public List<JoinPlayerData> leaveUpdatePlayerDatas = new List<JoinPlayerData>();
    public int tourCommision;
    public float commisionAmount;
    public int orgIndexPlayer;
    public List<TournamentData> tournamentData = new List<TournamentData>();
    public bool isTournamentLoaded;
    public float tourBonuseCutAmount = 0;
    public float refundDebitAmount;
    public float refundBonusAmount;

    [Header("--Ludo--")]
    public int modeType;
    public bool isDiceClick;
    public bool isTimeAuto;
    [HideInInspector]
    public int diceManageCnt;
    public bool isBotSix;
    public int botPasaNo;
    public bool isRestartManage;
    public bool isAvaliable;
    public float betPrice;



    [Header("---Teen Patti---")]
    public float chaalLimit;
    public float potLimit;






    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
    
    private void Start()
    { 
        GetVersionUpdate();
        GetTournament();
        //MainMenuManager.Instance.GetTran();
    }
    #region Storage

    public void SetLoginValue(string value)
    {
        PlayerPrefs.SetString("LoginValue", value);
    }
    public string GetLoginValue()
    {
        return PlayerPrefs.GetString("LoginValue", "N");
    }

    public void SetSound(int no)
    {
        PlayerPrefs.SetInt("SoundValue", no);
    }
    public int GetSound()
    {
        return PlayerPrefs.GetInt("SoundValue", 0);
    }
    public void SetVibration(int no)
    {
        PlayerPrefs.SetInt("VibrationValue", no);
    }
    public int GetVibration()
    {
        return PlayerPrefs.GetInt("VibrationValue", 0);
    }

    public void SetNotification(int no)
    {
        PlayerPrefs.SetInt("NotificationValue", no);
    }
    public int GetNotification()
    {
        return PlayerPrefs.GetInt("NotificationValue", 0);
    }

    public void SetFriendRequest(int no)
    {
        PlayerPrefs.SetInt("FriendRequestValue", no);
    }
    public int GetFriendRequest()
    {
        return PlayerPrefs.GetInt("FriendRequestValue", 0);
    }

    public void SetBankValue(int no)
    {
        PlayerPrefs.SetInt("AddedBankValue", no);
    }

    public int GetBankValue()
    {
        return PlayerPrefs.GetInt("AddedBankValue", 0);
    }
    public void SetAvatarValue(int no)
    {
        PlayerPrefs.SetInt("AvatarValue", no);
    }

    public int GetAvatarValue()
    {
        return PlayerPrefs.GetInt("AvatarValue", 0);
    }

    public void SetDefaultPlayerName(string s)
    {
        string setData = "user" + UnityEngine.Random.Range(99, 999) + s;
        //print("Set Data : " + setData);
        PlayerPrefs.SetString("Default_User_Name", setData);
    }
    public string GetDefaultPlayerName()
    {
        return PlayerPrefs.GetString("Default_User_Name");
    }


    public void SetPlayedGame(int no)
    {
        PlayerPrefs.SetInt("PlayedGame", no);
    }

    public int GetPlayedGame()
    {
        return PlayerPrefs.GetInt("PlayedGame", 0);
    }

    public void SetWonMoneyGame(float no)
    {
        PlayerPrefs.SetFloat("WonPlayedGame", no);
    }

    public float GetWonMoneyGame()
    {
        return PlayerPrefs.GetFloat("WonPlayedGame", 0);
    }
    #endregion


    public void UserTurnVibrate()
    {
        // Check if the device supports vibration
        if (!SystemInfo.supportsVibration) return;
        // Vibrate the device for 500 milliseconds
        Handheld.Vibrate();
    }


    public string GetModeToSceneName(GameType type)
    {
        string gameName = "";
        if (type == GameType.Teen_Patti)
        {
            gameName = "TeenPatti";
        }
        /*else if (type == GameType.Andar_Bahar)
        {
            gameName = "AndarBahar";
        }
        else if (type == GameType.Roulette)
        {
            gameName = "Rouletee";
        }*/
        else if (type == GameType.Poker)
        {
            gameName = "Poker";
        }
        /*else if (type == GameType.Dragon_Tiger)
        {
            gameName = "DragonTiger";
        }*/
        else if (type == GameType.Ludo)
        {
            gameName = "Ludo";
        }
        else if (type == GameType.Archery)
        {
            gameName = "Archery";
        }
        else if (type == GameType.Carrom)
        {
            gameName = "Carrom";
        }
        else if (type == GameType.Chess)
        {
            gameName = "Chess";
        }
        else if (type == GameType.Slot)
        {
            gameName = "Slot";
        }
        return gameName;
    }

    #region Null Maintain
    public bool IsNullOrEmpty(string value)
    {
        return value == null || value.Length == 0;
    }

    #endregion

    #region User Maintain


    public void AddRoomUser(string userId, string userName, string lobbyId, string balance, int playerNo, string avtar)
    {
        JoinPlayerData joinPlayer = new JoinPlayerData();
        joinPlayer.userId = userId;
        joinPlayer.userName = userName;
        joinPlayer.balance = balance;
        joinPlayer.playerNo = playerNo;
        joinPlayer.lobbyId = lobbyId;
        joinPlayer.avtar = avtar;

        int check = 0;
        for (int i = 0; i < joinPlayerDatas.Count; i++)
        {
            if (joinPlayerDatas[i].userId == joinPlayer.userId)
            {
                check++;
            }

        }
        if (check == 0)
        {
            if (joinPlayer.userId == DataManager.Instance.playerData._id)
            {
                MainMenuManager.Instance.MainMenuMoneyCut();
            }
            joinPlayerDatas.Add(joinPlayer);
        }
        else
        {
            return;
        }
    }

    public bool CheckRoomUser(string userId)
    {
        JoinPlayerData joinPlayer = new JoinPlayerData();
        joinPlayer.userId = userId;


        int check = 0;
        for (int i = 0; i < joinPlayerDatas.Count; i++)
        {
            if (joinPlayerDatas[i].userId == joinPlayer.userId)
            {
                check++;
            }

        }
        if (check == 0)
        {
            return true;
        }
        return false;
    }

    #endregion
    
     public void JoinMoneyCut(string gameName)
    {
        //DataManager.Instance.refundBonusAmount = 0;
        //DataManager.Instance.refundDebitAmount = 0;
        if (tourEntryMoney != 0)
        {
            print("Main Check Bonus Balanace : " + float.Parse(DataManager.Instance.playerData.bonus));
            if (float.Parse(playerData.bonus) <= 0)
            {
                //DataManager.Instance.refundDebitAmount = (tourEntryMoney / 1);
                DataManager.Instance.DebitAmount((tourEntryMoney / 1).ToString(), TestSocketIO.Instace.roomid, "Play " + gameName + " " + TestSocketIO.Instace.roomid, "join", 1);

                //print("Enter the Total Balance 1  : " + DataManager.Instance.tourEntryMoney);
            }
            else
            {
                //float customCommision = 10;
                //float bonus = (float.Parse(DataManager.Instance.playerData.bonus) * 10);
                float bonus = (float.Parse(playerData.bonus) * 1);

                //print("Second Bonus Manager : " + bonus);
                //float cutMoney = (float)((DataManager.Instance.tourEntryMoney * customCommision) / 100);
                float cutMoney = tourBonuseCutAmount * 1;
                //print("Second Cut Money : " + cutMoney);
                if (tourBonuseCutAmount != 0)
                {
                    if ((bonus - cutMoney) < 0)
                    {
                        refundDebitAmount = (tourEntryMoney / 1);
                        DebitAmount((tourEntryMoney / 1).ToString(), TestSocketIO.Instace.roomid, "Play " + gameName + " " + TestSocketIO.Instace.roomid, "join", 1);
                    }
                    else
                    {
                        refundBonusAmount = (cutMoney / 1);
                        refundDebitAmount = ((tourEntryMoney / 1) - (cutMoney / 1));

                        BonusDebitAmount((cutMoney / 1).ToString(), TestSocketIO.Instace.roomid, "Play " + gameName + " " + TestSocketIO.Instace.roomid, "join");
                        DebitAmount(((tourEntryMoney / 1) - (cutMoney / 1)).ToString(), TestSocketIO.Instace.roomid, "Play " + gameName + " " + TestSocketIO.Instace.roomid, "join", 1);
                    }
                }
                else
                {
                    refundDebitAmount = (tourEntryMoney / 1);

                    DebitAmount((tourEntryMoney / 1).ToString(), TestSocketIO.Instace.roomid, "Play " + gameName + " " + TestSocketIO.Instace.roomid, "join", 1);
                }
            }

        }

    }
    
    #region Version Update

    public void GetVersionUpdate()
    {
        StartCoroutine(GetVersionUpdates());
    }

    IEnumerator GetVersionUpdates()
    {
        UnityWebRequest request = UnityWebRequest.Get(DataManager.Instance.url + "/api/v1/players/versionlist");
        request.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString("token"));
        yield return request.SendWebRequest();

        if (request.error == null && !request.isNetworkError)
        {
            print("Get Request Message : " + request.downloadHandler.text);

            JSONNode keys = JSON.Parse(request.downloadHandler.text.ToString());

            JSONNode data = JSON.Parse(keys["data"].ToString());



            if (data.Count > 0)
            {
                appVersion = data[0]["versionControle"];
                appUrl = data[0]["appLink"];
                //DataManager.Instance.appVersion = 0.1.ToString();

                InternetManager.Instance.CheckUpdate();
            }
            else
            {
                //settingUpdateObj.SetActive(false);
            }
            //if (value.Count > 0)
            //{
            //    notificationRedDot.SetActive(true);
            //}
            //else
            //{
            //    notificationRedDot.SetActive(false);
            //}
        }

    }

    #endregion

    #region Tournaments

    public void GetTournament()
    {
        StartCoroutine(GetTournaments());
    }
    IEnumerator GetTournaments()
    {
        tournamentData.Clear();
        UnityWebRequest request = UnityWebRequest.Get(url + "/api/v1/players/tournaments");

        request.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString("token"));
        yield return request.SendWebRequest();

        if (request.error == null && !request.isNetworkError)
        {
            print("Tour Data : " + request.downloadHandler.text);

            JSONNode values = JSON.Parse(request.downloadHandler.text.ToString());
            JSONNode data = JSON.Parse(values["data"].ToString());
            
            string login = GetLoginValue();
            if (values["success"] == false && login == "Y")
            {
                // if token expires then login vale is set to NO then show login screen.
                SetLoginValue("N");
                yield break;
            }

            for (int i = 0; i < data.Count; i++)
            {
                TournamentData t = new TournamentData
                {
                    bot = data[i]["bot"],
                    bonusAmountDeduction = data[i]["bonusAmountDeduction"],
                    active = data[i]["active"],
                    _id = data[i]["_id"],
                    name = data[i]["name"]
                };

                t.modeType = int.Parse(data[i]["mode"]) switch
                {
                    1 => GameType.Slot,
                    2 => GameType.Poker,
                    3 => GameType.Rummy,
                    4 => GameType.Teen_Patti,
                    5 => GameType.Carrom,
                    6 => GameType.Ludo,
                    7 => GameType.Archery,
                    8 => GameType.Ball_Pool,
                    9 => GameType.Chess,
                    _ => t.modeType
                };
                t.betAmount = data[i]["betAmount"];
                t.minBet = data[i]["minBet"];
                t.maxBet = data[i]["maxBet"];
                t.maxPayout = data[i]["maxPayout"];
                t.challLimit = data[i]["challLimit"];
                t.potLimit = data[i]["potLimit"];
                t.players = int.Parse(data[i]["players"]);
                t.winner = int.Parse(data[i]["winner"]);

                float winAmount = 0;
                for (int j = 0; j < data[i]["winnerRow"].Count; j++)
                {
                    t.winnerRow.Add(data[i]["winnerRow"][j]);
                    winAmount += data[i]["winnerRow"][j];
                }

                t.totalWinAmount = winAmount;

                t.time = float.Parse(data[i]["time"]);
                t.complexity = int.Parse(data[i]["complexity"]);
                t.interval = int.Parse(data[i]["interval"]);
                t._v = data[i]["__v"];
                t.createdAt = data[i]["createdAt"];
                t.updatedAt = data[i]["updatedAt"];
                if (t.active)
                {
                    tournamentData.Add(t);
                }
            }

        }
        else
        {
            Logger.log.Log(request.error.ToString());
        }

    }

    #endregion


    #region Debit and Credit Player Manage



    #region Credit
    
    public void SendLeaderBoardData(string winPlayerId, float amountWon, string tourId, int winner, string gameId, string note, string players, string status)
    {
        WWWForm form = new WWWForm();
        form.AddField("playerId", winPlayerId);
        form.AddField("amountWon", amountWon.ToString());
        form.AddField("tournamentId", tourId);
        form.AddField("winner", winner);
        form.AddField("gameId", gameId);
        form.AddField("gameStatus", "won");
        form.AddField("note", note);
        form.AddField("players", players);
        form.AddField("gameStatus", status);

        //print("PlayerId : " + winPlayerId);
        //print("amountWon : " + amountWon);
        //print("tournamentId : " + tourId);
        //print("winner : " + winner);
        //print("gameId : " + gameId);
        //print("gameStatus : " + "won");
        //print("note : " + note);
        StartCoroutine(SendLeaderBoardDatas(form));
    }


    IEnumerator SendLeaderBoardDatas(WWWForm form)
    {
        //WaitPanelManager.Instance.OpenPanel();

        UnityWebRequest request = UnityWebRequest.Post(url + "/api/v1/players/saveleaderboard", form);
        request.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString("token"));

        yield return request.SendWebRequest();
        if (request.error == null)
        {
            print("request.downloadHandler.text Leader Board: " + request.downloadHandler.text);
            JSONNode values = JSON.Parse(request.downloadHandler.text.ToString());
            if (values["success"] == true)
            {
                //WaitPanelManager.Instance.ClosePanel();

            }
        }
        else
        {
            print("Leader Erro : " + request.error.ToString());
        }
    }

    //float com = (float)(bid.amount * Datamanger.Intance.data.commission) / 100;
    //LocalPlayer.Instace.addamount(bid.amount, TestSocketIO.Instace.roomid, "Internal Bid Won", "won", com);
    public void AddAmount(float amount, string roomid, string note, string log, float adminc, int winNo)
    {
        StartCoroutine(SendWonamount(amount, roomid, note, log, adminc, winNo));
    }

    IEnumerator SendWonamount(float amount, string roomid, string note, string log, float adminc, int winNo)
    {
        print("Win Amount : " + amount);
        WWWForm form = new WWWForm();
        form.AddField("amount", amount.ToString());
        form.AddField("gameId", roomid);
        form.AddField("note", note);
        form.AddField("logType", log);
        form.AddField("tournamentId", DataManager.Instance.tournamentID);
        form.AddField("adminCommision", adminc.ToString());
        form.AddField("betNo", winNo);
        //UnityWebRequest request = UnityWebRequest.Post(url + "/api/v1/players/credit", form);
        UnityWebRequest request = UnityWebRequest.Post(url + "/api/v1/players/game/won", form);
        request.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString("token"));

        yield return request.SendWebRequest();
        if (request.error == null)
        {
            print("Data Credit : " + request.downloadHandler.text.ToString());
            JSONNode values = JSON.Parse(request.downloadHandler.text.ToString());
            if (values["success"] == true)
            {
                JSONNode data = JSON.Parse(values["data"].ToString());
                Setplayerdata(data);
                //playerData.balance = data[nameof(DataManager.Instance.playerData.balance)].ToString().Trim('"');
                //playerData.deposit = data[nameof(DataManager.Instance.playerData.deposit)];
                //playerData.winings = data[nameof(DataManager.Instance.playerData.winings)];
                //playerData.bonus = data[nameof(DataManager.Instance.playerData.bonus)];
            }
        }
        else
        {
            print("Data Credit : " + request.error);

        }
    }
    #endregion
    
    #region Refund

    public void UserRefund(string gameName)
    {
        print("Refund Debit Amount : " + refundDebitAmount);
        print("Refund Bonus Amount : " + refundBonusAmount);
        if (refundDebitAmount != 0)
        {
            RefundAmount(refundDebitAmount.ToString(), TestSocketIO.Instace.roomid, "Refund " + gameName + " " + TestSocketIO.Instace.roomid, "refund");
        }
        if (refundBonusAmount != 0)
        {
            BonusDebitAmount_Credit(refundBonusAmount.ToString(), "Refund " + gameName + " " + TestSocketIO.Instace.roomid, "refund");
        }
    }

    public void RefundAmount(string amount, string roomId, string note, string logType)
    {
        print("amount :  " + amount);
        WWWForm form = new WWWForm();
        form.AddField("amount", amount);
        form.AddField("gameId", roomId);
        form.AddField("note", note);
        form.AddField("logType", "refund");
        form.AddField("adminCommision", 0);
        RefundAmount_Send(form);
    }

    void RefundAmount_Send(WWWForm form)
    {

        StartCoroutine(Refund_Amount_Ienum(form));
    }

    IEnumerator Refund_Amount_Ienum(WWWForm form)
    {
        WaitPanelManager.Instance.OpenPanel();

        UnityWebRequest request = UnityWebRequest.Post(DataManager.Instance.url + "/api/v1/players/reverse", form);
        request.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString("token"));
        yield return request.SendWebRequest();

        //print("Request Refund Downloader : " + request.downloadHandler.text);
        JSONNode values = JSON.Parse(request.downloadHandler.text.ToString());
        JSONNode data = JSON.Parse(values["data"].ToString());
        WaitPanelManager.Instance.ClosePanel();

        //Balance_Txt.text = data["balance"].ToString().Trim('"');
        //        playerData.balance = data[nameof(DataManager.Instance.playerData.balance)].ToString().Trim('"');
        //playerData.deposit = data[nameof(DataManager.Instance.playerData.deposit)];
        //playerData.winings = data[nameof(DataManager.Instance.playerData.winings)];
        //playerData.bonus = data[nameof(DataManager.Instance.playerData.bonus)];
    }


    #endregion


    #region Debit

    public void DebitAmount(string amount, string roomId, string note, string logType, int betNo)
    {
        print("amount :  " + amount);
        WWWForm form = new WWWForm();
        form.AddField("amount", amount);
        form.AddField("gameId", roomId);
        form.AddField("note", note);
        form.AddField("logType", logType);
        form.AddField("betNo", betNo);
        DebitAmount_Send(form);
    }

    void DebitAmount_Send(WWWForm form)
    {
        StartCoroutine(Debit_Amount_Ienum(form));
    }

    IEnumerator Debit_Amount_Ienum(WWWForm form)
    {
        UnityWebRequest request = UnityWebRequest.Post(DataManager.Instance.url + "/api/v1/players/debit", form);
        request.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString("token"));
        yield return request.SendWebRequest();
        JSONNode values = JSON.Parse(request.downloadHandler.text.ToString());
        JSONNode data = JSON.Parse(values["data"].ToString());

        print("Debit Time Value : " + request.downloadHandler.text);
        Setplayerdata(data);
        //Balance_Txt.text = data["balance"].ToString().Trim('"');
        //        playerData.balance = data[nameof(DataManager.Instance.playerData.balance)].ToString().Trim('"');
        //playerData.deposit = data[nameof(DataManager.Instance.playerData.deposit)];
        //playerData.winings = data[nameof(DataManager.Instance.playerData.winings)];
        //playerData.bonus = data[nameof(DataManager.Instance.playerData.bonus)];
    }
    

    #endregion


    #region Bonus

    public void BonusDebitAmount(string amount, string roomId, string note, string logType)
    {
        WWWForm form = new WWWForm();
        form.AddField("amount", amount);
        form.AddField("gameId", roomId);
        form.AddField("note", note);
        form.AddField("logType", logType);
        BonusDebitAmount_Send(form);
    }

    void BonusDebitAmount_Send(WWWForm form)
    {
        StartCoroutine(Bonus_Debit_Amount_Ienum(form));
    }

    IEnumerator Bonus_Debit_Amount_Ienum(WWWForm form)
    {
        WaitPanelManager.Instance.OpenPanel();

        UnityWebRequest request = UnityWebRequest.Post(DataManager.Instance.url + "/api/v1/players/debitBonus", form);
        request.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString("token"));
        yield return request.SendWebRequest();
        JSONNode values = JSON.Parse(request.downloadHandler.text.ToString());
        JSONNode data = JSON.Parse(values["data"].ToString());
        WaitPanelManager.Instance.ClosePanel();
    }


    public void BonusDebitAmount_Credit(string amount, string note, string logType)
    {
        WWWForm form = new WWWForm();
        form.AddField("amount", amount);
        form.AddField("note", note);
        form.AddField("logType", logType);
        BonusDebitAmount_Send_Credit(form);
    }

    void BonusDebitAmount_Send_Credit(WWWForm form)
    {
        StartCoroutine(Bonus_Debit_Amount_Ienum_Credit(form));
    }

    IEnumerator Bonus_Debit_Amount_Ienum_Credit(WWWForm form)
    {
        WaitPanelManager.Instance.OpenPanel();

        UnityWebRequest request = UnityWebRequest.Post(DataManager.Instance.url + "/api/v1/players/creditBonus", form);
        request.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString("token"));
        yield return request.SendWebRequest();
        JSONNode values = JSON.Parse(request.downloadHandler.text.ToString());
        JSONNode data = JSON.Parse(values["data"].ToString());
        WaitPanelManager.Instance.ClosePanel();

        //Balance_Txt.text = data["balance"].ToString().Trim('"');
        //playerData.balance = data[nameof(DataManager.Instance.playerData.balance)].ToString().Trim('"');
        //playerData.deposit = data[nameof(DataManager.Instance.playerData.deposit)];
        //playerData.winings = data[nameof(DataManager.Instance.playerData.winings)];
        //playerData.bonus = data[nameof(DataManager.Instance.playerData.bonus)];
    }


    #endregion



    public void Setplayerdata(JSONNode data)
    {
        Debug.Log("User Data===:::" + data.ToString());

        if (data[nameof(playerData.balance)] == "")
        {
            data[nameof(playerData.balance)] = "";
        }
        DataManager.Instance.playerData.balance = ((float)data[nameof(DataManager.Instance.playerData.balance)]).ToString("F2");
        playerData.kycStatus = data[nameof(playerData.kycStatus)];
        if (data[nameof(playerData.wonCount)] == "")
        {
            data[nameof(playerData.wonCount)] = "";
        }
        playerData.wonCount = data[nameof(playerData.wonCount)];
        if (data[nameof(playerData.joinCount)] == "")
        {
            data[nameof(playerData.joinCount)] = "";
        }
        playerData.joinCount = data[nameof(playerData.joinCount)];
        playerData.deposit = (data[nameof(playerData.deposit)] * 10).ToString();
        playerData.winings = (data[nameof(playerData.winings)] * 10).ToString();
        playerData.bonus = (data[nameof(playerData.bonus)] * 10).ToString();
        playerData._id = data[nameof(playerData._id)];
        playerData.phone = data[nameof(playerData.phone)];
        playerData.aadharNumber = data[nameof(playerData.aadharNumber)];
        playerData.refer_code = data[nameof(playerData.refer_code)];
        playerData.email = data[nameof(playerData.email)];
        playerData.firstName = data[nameof(playerData.firstName)];
        playerData.lastName = data[nameof(playerData.lastName)];
        playerData.gender = data[nameof(playerData.gender)];
        playerData.state = data[nameof(playerData.state)];
        playerData.createdAt = RemoveQuotes(data[nameof(playerData.createdAt)].ToString());
        playerData.countryCode = data[nameof(playerData.countryCode)];

        string getName = data[nameof(playerData.dob)];
        if (getName == "" || getName == null)
        {
            playerData.dob = "none";
        }
        else
        {
            playerData.dob = RemoveQuotes(data[nameof(playerData.dob)]);
        }
        playerData.panNumber = data[nameof(playerData.panNumber)];
        playerData.membership = "free";
        //DataManager.Instance.playerData.membership = data[nameof(DataManager.Instance.playerData.membership)];
        playerData.avatar = GetAvatarValue();
        playerData.refer_count = data[nameof(playerData.refer_count)];
        playerData.refrer_level = data[nameof(playerData.refrer_level)];
        playerData.refrer_amount_total = data[nameof(playerData.refrer_amount_total)];

        playerData.refer_lvl1_count = data[nameof(playerData.refer_lvl1_count)];
        playerData.refer_vip_count = data[nameof(playerData.refer_vip_count)];
        playerData.refer_deposit_count = data[nameof(playerData.refer_deposit_count)];


        if (CheckNullOrEmpty(GetDefaultPlayerName()) && CheckNullOrEmpty(playerData.firstName))
        {
            print("Sub String : ");
            DataManager.Instance.SetDefaultPlayerName(DataManager.Instance.playerData.phone.Substring(0, 5));
            DataManager.Instance.playerData.firstName = DataManager.Instance.GetDefaultPlayerName();
        }
        else if (CheckNullOrEmpty(playerData.firstName))
        {
            DataManager.Instance.playerData.firstName = DataManager.Instance.GetDefaultPlayerName();
        }


        if (TeenPattiManager.Instance != null)
        {
            TeenPattiManager.Instance.DisplayCurrentBalance();
        }
        if (MainMenuManager.Instance != null)
        {
            MainMenuManager.Instance.coinTxt.text = playerData.balance.ToString();
            MainMenuManager.Instance.secondCoinText.text = MainMenuManager.Instance.coinTxt.text;
        }
        if (RouletteManager.Instance != null)
        {
            RouletteManager.Instance.UpdateNameBalance();
        }
        if (DragonTigerManager.Instance != null)
        {
            DragonTigerManager.Instance.UpdateNameBalance();
        }
        if (AndarBaharManager.Instance != null)
        {
            AndarBaharManager.Instance.UpdateNameBalance();
        }
    }

    public bool CheckNullOrEmpty(string value)
    {
        return value == null || value.Length == 0;
    }
    public string RemoveQuotes(string s)
    {
        string str = s;
        string newstr = str.Replace("\"", "");
        return newstr;
    }

    public void SetLoginEmail(int no)
    {
        PlayerPrefs.SetInt("LoginEmailCheck", no);
    }


    public int GetLoginEmail()
    {
        return PlayerPrefs.GetInt("LoginEmailCheck", 0);
    }

    #region Recently Manage

    public void SetRecent1(string gameName)
    {
        PlayerPrefs.SetString("GameNameRecent1", gameName);
    }


    public string GetRecent1()
    {
        return PlayerPrefs.GetString("GameNameRecent1", "none");
    }

    public void SetRecent2(string gameName)
    {
        PlayerPrefs.SetString("GameNameRecent2", gameName);
    }


    public string GetRecent2()
    {
        return PlayerPrefs.GetString("GameNameRecent2", "none");
    }

    public void SetRecent3(string gameName)
    {
        PlayerPrefs.SetString("GameNameRecent3", gameName);
    }


    public string GetRecent3()
    {
        return PlayerPrefs.GetString("GameNameRecent3", "none");
    }

    public void SetRecent4(string gameName)
    {
        PlayerPrefs.SetString("GameNameRecent4", gameName);
    }


    public string GetRecent4()
    {
        return PlayerPrefs.GetString("GameNameRecent4", "none");
    }
    #endregion

    #endregion

    public IEnumerator GetImages(string URl, Image image)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(URl);
        yield return request.SendWebRequest();

        if (request.error == null)
        {
            if (image != null)
            {
                var texture = DownloadHandlerTexture.GetContent(request);
                Rect rect = new Rect(0, 0, texture.width, texture.height);
                image.sprite = Sprite.Create(texture, rect, new Vector2(0, 0));
                image.color = new Color(255, 255, 255, 255);
            }
        }
    }
}
[System.Serializable]
public class PlayerData
{
    public string role;
    public string balance;
    public string kycStatus;
    public string wonCount;
    public string joinCount;
    public string deposit;
    public string winings;
    public string bonus;
    public string _id;
    public string phone;
    public string status;
    public string countryCode;
    public string createdAt;
    public string deviceType;
    public string aadharNumber;
    public string country;
    public string dob;
    public string email;
    public string firstName;
    public string lastName;
    public string gender;
    public string panNumber;
    public int avatar;
    public string refer_code;
    public string membership;
    public string state;
    public string refer_count;
    public string refrer_level;
    public string refrer_amount_total;
    public string refer_lvl1_count;
    public string refer_vip_count;
    public string refer_deposit_count;
}
[System.Serializable]
public class JoinPlayerData
{
    public string userId;
    public string userName;
    public string balance;
    public string lobbyId;
    public int playerNo;
    public string avtar;
    public string pPicture;
}
