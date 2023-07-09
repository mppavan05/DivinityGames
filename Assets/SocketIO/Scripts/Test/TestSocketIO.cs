using System.Collections;
using UnityEngine;
using SocketIO;
using SimpleJSON;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Collections.Generic;
using WebSocketSharp;
using System;
//using MoreMountains.NiceVibrations;

public class TestSocketIO : MonoBehaviour
{
    private SocketIOComponent socket;
    public static TestSocketIO Instace;
    public GameObject connectServerObj;
    public GameObject sessionRestoreObj;
    public string roomid;
    public string userdata;
    public float playTime;
    public bool isSocketError;
    public string lobbyId;

    [Header("--- Game Play Maintain ---")]
    public int teenPattiRequirePlayer;
    public int pokerRequirePlayer;
    public int andarBaharRequirePlayer;
    public int ludoRequirePlayer;
    
    [Header("--- Waiting Screen ---")]
    public GameObject loadingScreen; // Reference to the loading screen game object

    private void Awake()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Instace = this;
    }
    public void Start()
    {
        DontDestroyOnLoad(gameObject);
        GameObject go = GameObject.Find("SocketIO");
        socket = go.GetComponent<SocketIOComponent>();
        DontDestroyOnLoad(go);
        socket.On("open", TestOpen);
        socket.On("error", TestError);
        socket.On("close", TestClose);
        socket.On("res", HandelEvents);
    }

    public void TestOpen(SocketIOEvent e)
    {
        Debug.Log("[SocketIO] Open received===::: " + e.name + " " + e.data);
        // if(DataManager.Instance.isSocketError)
        // {
        //     DataManager.Instance.OpenSeesionRestore();
        // }
    }

    public void TestError(SocketIOEvent e)
    {
        Debug.Log("[SocketIO] Error received===::: " + e.name + " " + e.data);
        // if(isSocketError == false && DailyReward.Instance!=null) 
        // {
        //     connectServerObj.SetActive(true);
        //     isSocketError = true;
        //     // DataManager.Instance.OpenConnectServer();
        // }
    }

    public void TestClose(SocketIOEvent e)
    {
        Debug.Log("[SocketIO] Close received===::: " + e.name + " " + e.data);
    }

    public void HandelEvents(SocketIOEvent data)
    {
        string ev = "";
        print(data);
        JSONNode values = JSON.Parse(data.data.ToString());
        ev = values["ev"].Value.ToString().Trim('"');
        //Debug.Log("Events===:::" + ev.ToString());

        switch (ev)
        {
            case "join":
                setroomid(data.data.ToString());
                break;
            case "LudoData":
                SetLudoData(values.ToString());
                break;
            case "LudoDiceData":
                SetLudoDiceData(values.ToString());
                break;

            case "LudoDiceStopData":
                SetLudoStopDiceData(values.ToString());
                break;
            case "LudoDiceChangeData":
                SetLudoChangeDiceData(values.ToString());
                break;

            case "PauseRequest":
                PausePlayerRequest(values.ToString());
                break;
            case "GetUserPauseData":
                PauseGetPlayerData(values.ToString());
                break;
            case "leave":
                ResetRole(data.data.ToString());
                break;
            case "disconnect":
                ResetRole(data.data.ToString());
                break;

            // Other
            case "setGameData":
                GetRoomData(data.data.ToString());
                break;

            case "SendChatMessage":
                SendChatMessageManage(values.ToString());
                break;

            case "SendGiftMessage":
                SendGiftMessageManage(values.ToString());
                break;
            
            case "setGameId":
                HandelSetGameId(values.ToString());
                break;
            // Teen Patti

            case "TeenPattiChangeTurnData":
                SetChangeTeenPatti(values.ToString());
                break;
            case "TeenPattiBotBetNo":
                SetBotBetTeenPatti(values.ToString());
                break;

            case "TeenPattiChangeCardStatus":
                SetChangeStatusTeenPatti(values.ToString());
                break;

            case "TeenPattiSendBetData":
                SetBetTeenPatti(values.ToString());
                break;

            case "TeenPattiSlideShowData":
                SetSlideShowTeenPatti(values.ToString());
                break;

            case "TeenPattiWinnerData":
                SetWinTeenPatti(values.ToString());
                break;

            //Roulette

            /*case "RouletteSendBetData":
                SetBetRoulette(values.ToString());
                break;
            case "FindDataRouletteAdmin":
                FindRouletteData(values.ToString());
                break;
            case "SendAdminRouleteeData":
                SendAdminRouleteeData(values.ToString());
                break;*/

            //AndarBahar

            /*case "AndarBaharBetData":
                SetBetAndarBahar(values.ToString());
                break;*/

            // Dragon Tiger

            /*case "SendDragonTigerBet":
                SetBetDragonTiger(values.ToString());
                break;*/


            // Poker

            case "PokerChangeTurnData":
                SetChangePoker(values.ToString());
                break;
            case "PokerSendBetData":
                SetBetPoker(values.ToString());
                break;
            case "PokerBotBetNo":
                SetBotBetPoker(values.ToString());
                break;
            case "PokerSendFlodData":
                SetFoldPoker(values.ToString());
                break;
            case "PokerWinnerData":
                SetWinPoker(values.ToString());
                break;
            case "PokerFinalWinnerData":
                HandelWinPoker(values.ToString());
                break;
            
            //Archery
            case "ArcheryChangeData":
                SetArcheryChange(values.ToString());
                break;
            
            // Carrom
            case "CarromSlideData":
                SetSliderChange(values.ToString());
                break;
        }
    }

    public void ArcheryJoinRoom()
    {
        JSONObject userdata = new JSONObject();
        userdata.AddField("userId", DataManager.Instance.playerData._id.ToString().Trim('"'));

        if (DataManager.Instance.playerData.firstName == null)
        {
            userdata.AddField("name", DataManager.Instance.GetDefaultPlayerName().ToString().Trim('"'));
        }
        else
        {
            userdata.AddField("name", DataManager.Instance.playerData.firstName.ToString().Trim('"'));
        }
        userdata.AddField("balance", DataManager.Instance.playerData.balance);
        userdata.AddField("lobbyId", DataManager.Instance.tournamentID);
        userdata.AddField("maxp", 2);
        userdata.AddField("avtar", PlayerPrefs.GetString("ProfileURL"));
        //userdata.AddField("token", PlayerPrefs.GetString("token"));

        if (DataManager.Instance.CheckRoomUser(DataManager.Instance.playerData._id.ToString().Trim('"')) == true)
        {
            socket.Emit("join", userdata);
        }
        
    }
    
    public void SlotJoinRoom()
    {
        JSONObject userdata = new JSONObject();
        userdata.AddField("userId", DataManager.Instance.playerData._id.ToString().Trim('"'));

        if (DataManager.Instance.playerData.firstName == null)
        {
            userdata.AddField("name", DataManager.Instance.GetDefaultPlayerName().ToString().Trim('"'));
        }
        else
        {
            userdata.AddField("name", DataManager.Instance.playerData.firstName.ToString().Trim('"'));
        }
        userdata.AddField("balance", DataManager.Instance.playerData.balance);
        userdata.AddField("lobbyId", DataManager.Instance.tournamentID);
        userdata.AddField("maxp", 1);
        userdata.AddField("avtar", PlayerPrefs.GetString("ProfileURL"));
        //userdata.AddField("token", PlayerPrefs.GetString("token"));

        if (DataManager.Instance.CheckRoomUser(DataManager.Instance.playerData._id.ToString().Trim('"')) == true)
        {
            socket.Emit("join", userdata);
        }
    }
    public void LudoJoinroom()
    {
        JSONObject userdata = new JSONObject();
        userdata.AddField("userId", DataManager.Instance.playerData._id.ToString().Trim('"'));

        if (DataManager.Instance.playerData.firstName == null)
        {
            userdata.AddField("name", DataManager.Instance.GetDefaultPlayerName().ToString().Trim('"'));
        }
        else
        {
            userdata.AddField("name", DataManager.Instance.playerData.firstName.ToString().Trim('"'));
        }
        userdata.AddField("balance", DataManager.Instance.playerData.balance);
        userdata.AddField("lobbyId", DataManager.Instance.tournamentID);
        userdata.AddField("maxp", LudoSelector.Instance.numberOfPlayers);
        userdata.AddField("avtar", PlayerPrefs.GetString("ProfileURL"));

        if (DataManager.Instance.CheckRoomUser(DataManager.Instance.playerData._id.ToString().Trim('"')) == true)
        {
            socket.Emit("join", userdata);
        }
        
        print("This is the data -> " + userdata);
    }

    public void TeenPattiJoinroom()
    {
        JSONObject userdata = new JSONObject();
        userdata.AddField("userId", DataManager.Instance.playerData._id.ToString().Trim('"'));

        if (DataManager.Instance.playerData.firstName == null)
        {
            userdata.AddField("name", DataManager.Instance.GetDefaultPlayerName().ToString().Trim('"'));
        }
        else
        {
            userdata.AddField("name", DataManager.Instance.playerData.firstName.ToString().Trim('"'));
        }
        userdata.AddField("balance", DataManager.Instance.playerData.balance);
        userdata.AddField("lobbyId", DataManager.Instance.tournamentID);
        userdata.AddField("maxp", 5);
        userdata.AddField("avtar", PlayerPrefs.GetString("ProfileURL"));

        if (DataManager.Instance.CheckRoomUser(DataManager.Instance.playerData._id.ToString().Trim('"')) == true)
        {
            socket.Emit("join", userdata);
        }
    }

    public void RouletteJoinroom()
    {
        JSONObject userdata = new JSONObject();
        userdata.AddField("userId", DataManager.Instance.playerData._id.ToString().Trim('"'));

        if (DataManager.Instance.playerData.firstName == null)
        {
            userdata.AddField("name", DataManager.Instance.GetDefaultPlayerName().ToString().Trim('"'));
        }
        else
        {
            userdata.AddField("name", DataManager.Instance.playerData.firstName.ToString().Trim('"'));
        }
        userdata.AddField("balance", DataManager.Instance.playerData.balance);
        userdata.AddField("lobbyId", DataManager.Instance.tournamentID);
        userdata.AddField("maxp", 99999);
        userdata.AddField("avtar", PlayerPrefs.GetString("ProfileURL"));

        if (DataManager.Instance.CheckRoomUser(DataManager.Instance.playerData._id.ToString().Trim('"')) == true)
        {
            socket.Emit("join", userdata);
        }
    }
    public void AndarBaharJoinroom()
    {
        JSONObject userdata = new JSONObject();
        userdata.AddField("userId", DataManager.Instance.playerData._id.ToString().Trim('"'));

        if (DataManager.Instance.playerData.firstName == null)
        {
            userdata.AddField("name", DataManager.Instance.GetDefaultPlayerName().ToString().Trim('"'));
        }
        else
        {
            userdata.AddField("name", DataManager.Instance.playerData.firstName.ToString().Trim('"'));
        }
        userdata.AddField("balance", DataManager.Instance.playerData.balance);
        userdata.AddField("lobbyId", DataManager.Instance.tournamentID);
        userdata.AddField("maxp", 8);
        userdata.AddField("avtar", PlayerPrefs.GetString("ProfileURL"));

        if (DataManager.Instance.CheckRoomUser(DataManager.Instance.playerData._id.ToString().Trim('"')) == true)
        {
            socket.Emit("join", userdata);
        }
    }

    public void Senddata_Btn()
    {
        JSONObject data = new JSONObject();
        data.AddField("msg", "Hello");
        Senddata("msg", data);
    }

    public void Senddata(string ev, JSONObject data)
    {
        JSONObject keys = new JSONObject();
        keys.AddField("room", roomid);
        keys.AddField("ev", ev);
        keys.AddField("data", data);
        //Debug.Log("SendData===:::" + keys.ToString());
        socket.Emit("sendToRoom", keys);
    }
    
    public void SetBetData(int chipNo, string betAmount)
    {
        JSONObject keys = new JSONObject();
        keys.AddField("room", roomid);
        keys.AddField("amount", int.Parse(betAmount));
        keys.AddField("betNo", chipNo);
        //Debug.Log("SendData===:::" + keys.ToString());
        print("This is sending data -> " + keys);
        socket.Emit("setBetData", keys);
    }
    
    public void GetBetData()
    {
        JSONObject keys = new JSONObject();
        keys.AddField("room", roomid);
        print("SendData Form GETBETDATA===:::" + keys);
        socket.Emit("getBetData", keys);
    }


    //bool isGenerate = false;
    public void setroomid(string data)
    {
        print("Room Data : " + data);
        //print("Join Player");
        JSONNode values = JSON.Parse(data);
        JSONNode obj = JSON.Parse(values["data"].ToString());
        roomid = obj["roomName"].Value.ToString();

        //print("Room Name : " + roomid);
        if (SceneManager.GetActiveScene().name == "Main")
        {
            userdata = obj["users"].ToString();
        }
        else
        {
            userdata = obj["users"].ToString();
        }

        if (obj["users"][0]["maxp"] == 1 && SceneManager.GetActiveScene().name == "Main")
        {
            for (int i = 0; i < obj.Count; i++)
            {
                if (values["data"]["users"][i]["name"] == null || values["data"]["users"][i]["userId"] == null ||
                    values["data"]["users"][i]["lobbyId"] == null) continue;
                DataManager.Instance.AddRoomUser(values["data"]["users"][i]["userId"], values["data"]["users"][i]["name"], values["data"]["users"][i]["lobbyId"], values["data"]["users"][i]["balance"], i, values["data"]["users"][i]["avtar"]);
            }
            
            MainMenuManager.Instance.LoadNewGame("Slot");
        }

        if (obj["users"][0]["maxp"] == 5)
        {
            //Teen Patti

            //print("teen Pattoi : " + );
            for (int i = 0; i < obj.Count; i++)// For MultiPlayer
            {
                if (values["data"]["users"][i]["name"] != null && values["data"]["users"][i]["userId"] != null && values["data"]["users"][i]["lobbyId"] != null)
                {
                    DataManager.Instance.AddRoomUser(values["data"]["users"][i]["userId"], values["data"]["users"][i]["name"], values["data"]["users"][i]["lobbyId"], values["data"]["users"][i]["balance"], i, values["data"]["users"][i]["avtar"]);
                }
            }
            //if (obj["users"].Count >= teenPattiRequirePlayer && SceneManager.GetActiveScene().name == "Main")
            //{
            //    //Open Scene
            //    SceneManager.LoadScene(DataManager.Instance.GetModeToSceneName(DataManager.Instance.gameMode));
            //}

            /*if (SceneManager.GetActiveScene().name == "Main")
            {// For Single Player
                for (int i = 0; i < obj.Count; i++)
                {
                    if (values["data"]["users"][i]["name"] != null && values["data"]["users"][i]["userId"] != null && values["data"]["users"][i]["lobbyId"] != null)
                    {
                        DataManager.Instance.AddRoomUser(values["data"]["users"][i]["userId"], values["data"]["users"][i]["name"], values["data"]["users"][i]["lobbyId"], values["data"]["users"][i]["balance"], i, values["data"]["users"][i]["avtar"]);
                    }
                }
            }*/

            /*if (DataManager.Instance.joinPlayerDatas.Count > 1)
            {// For Single Player
                var playerId = DataManager.Instance.playerData._id; // player ID to match
                var joinPlayerDatas = DataManager.Instance.joinPlayerDatas; // list of join player data
                joinPlayerDatas.RemoveAll(joinPlayerData => joinPlayerData.userId != playerId);
            }*/
            
            
            
            // if there is 3 players then bot is disabled 
            if (DataManager.Instance.joinPlayerDatas.Count <= 4)
            {
                MainMenuManager.Instance.CheckPlayers();
            }
            
            //if (obj["users"].Count >= teenPattiRequirePlayer && SceneManager.GetActiveScene().name == "Main")
            //{
            //    //Open Scene
            //    SceneManager.LoadScene(DataManager.Instance.GetModeToSceneName(DataManager.Instance.gameMode));
            //}

            if (SceneManager.GetActiveScene().name == "Main")
            {
                SceneManager.LoadScene(DataManager.Instance.GetModeToSceneName(DataManager.Instance.gameMode));
            }

            if (SceneManager.GetActiveScene().name == "TeenPatti")//obj["users"].Count >= teenPattiRequirePlayer && 
            {
                if (TeenPattiManager.Instance != null)
                {
                    TeenPattiManager.Instance.PlayerFound();
                }
            }
            if (SceneManager.GetActiveScene().name == "Poker")//obj["users"].Count >= pokerRequirePlayer && 
            {
                if (PokerGameManager.Instance != null)
                {
                    PokerGameManager.Instance.PlayerFound();
                }
            }
        }
        if (obj["users"][0]["maxp"] == 99999)
        {
            //Teen Patti

            //print("teen Pattoi : " + );
            for (int i = 0; i < obj.Count; i++)
            {
                if (values["data"]["users"][i]["name"] != null && values["data"]["users"][i]["userId"] != null && values["data"]["users"][i]["lobbyId"] != null)
                {
                    DataManager.Instance.AddRoomUser(values["data"]["users"][i]["userId"], values["data"]["users"][i]["name"], values["data"]["users"][i]["lobbyId"], values["data"]["users"][i]["balance"], i, values["data"]["users"][i]["avtar"]);
                }
            }
            //if (obj["users"].Count )
            //{
            //Open Scene

            if (SceneManager.GetActiveScene().name == "Main")
            {
                SceneManager.LoadScene(DataManager.Instance.GetModeToSceneName(DataManager.Instance.gameMode));
            }
            //}
        }
        if (obj["users"][0]["maxp"] == 8)
        {
            //Teen Patti

            //print("teen Pattoi : " + );
            for (int i = 0; i < obj.Count; i++)
            {
                if (values["data"]["users"][i]["name"] != null && values["data"]["users"][i]["userId"] != null && values["data"]["users"][i]["lobbyId"] != null)
                {
                    DataManager.Instance.AddRoomUser(values["data"]["users"][i]["userId"], values["data"]["users"][i]["name"], values["data"]["users"][i]["lobbyId"], values["data"]["users"][i]["balance"], i, values["data"]["users"][i]["avtar"]);
                }
            }
            //if (obj["users"].Count >= andarBaharRequirePlayer && SceneManager.GetActiveScene().name == "Main")
            //{
            //    SceneManager.LoadScene(DataManager.Instance.GetModeToSceneName(DataManager.Instance.gameMode));
            //}

            if (SceneManager.GetActiveScene().name == "Main")
            {
                SceneManager.LoadScene(DataManager.Instance.GetModeToSceneName(DataManager.Instance.gameMode));
            }
            if (obj["users"].Count >= andarBaharRequirePlayer && SceneManager.GetActiveScene().name == "AndarBahar")
            {
                if (AndarBaharManager.Instance != null)
                {
                    AndarBaharManager.Instance.PlayerFound();
                }
            }

        }

        else if (obj["users"][0]["maxp"] == 2)
        {

           // 2player ludo
            print("Ludo Manage Player Number : " + DataManager.Instance.playerNo);
            print("Ludo Player Number Assign : " + obj["users"]);
            if (DataManager.Instance.playerNo == 0)
            {
                print("Enter The First Player  Con");
                if (obj["users"][0]["maxp"] == 2)
                {
                    DataManager.Instance.isTwoPlayer = true;
                    DataManager.Instance.playerNo = obj["users"].Count;

                    if (DataManager.Instance.gameMode == GameType.Ludo)
                    {
                        if (DataManager.Instance.playerNo == 2)
                        {
                            DataManager.Instance.playerNo = 3;
                        }
                    }

                }
            }


            for (int i = 0; i < obj.Count; i++)
            {
                int pNo = 0;
                if (DataManager.Instance.isTwoPlayer)
                {
                    if (i == 0)
                    {
                        pNo = 1;
                    }
                    else if (i == 1)
                    {
                        pNo = 3;
                    }
                }
                else
                {
                    pNo = i + 1;
                }
                if (values["data"]["users"][i]["name"] != null && values["data"]["users"][i]["userId"] != null && values["data"]["users"][i]["lobbyId"] != null)
                {
                    DataManager.Instance.AddRoomUser(values["data"]["users"][i]["userId"], values["data"]["users"][i]["name"], values["data"]["users"][i]["lobbyId"], values["data"]["users"][i]["balance"], pNo, values["data"]["users"][i]["avtar"]);
                }
                /*if (obj["users"].Count == 2 && SceneManager.GetActiveScene().name == "Main")
                {
                    SceneManager.LoadScene(DataManager.Instance.GetModeToSceneName(DataManager.Instance.gameMode));
                }*/

            }
            /*if (obj["users"].Count == 1 && SceneManager.GetActiveScene().name == "Main")
            {
                SceneManager.LoadScene(DataManager.Instance.GetModeToSceneName(DataManager.Instance.gameMode));
            }*/

            if (DataManager.Instance.gameMode == GameType.Archery)
            {
                /*if (DataManager.Instance.joinPlayerDatas.Count == 1)
                {
                    MainMenuManager.Instance.OpenLoadScreen();
                }

                SceneManager.LoadScene(DataManager.Instance.GetModeToSceneName(DataManager.Instance.gameMode));*/
                if (!loadingScreen.activeSelf)
                {
                    StartWaitingProcess();
                }

                if (loadingScreen.activeSelf && DataManager.Instance.joinPlayerDatas.Count == 2)
                {
                    print("Inside the pass");
                    //StartGame();
                    CancelInvoke(nameof(CheckPlayersJoined));
                    CheckPlayersJoined();
                }
                print("not passed");
            }
            
            if (DataManager.Instance.gameMode == GameType.Carrom)
            {
                SceneManager.LoadScene(DataManager.Instance.GetModeToSceneName(DataManager.Instance.gameMode));
            }
            
            if (DataManager.Instance.gameMode == GameType.Chess)
            {
                if (DataManager.Instance.joinPlayerDatas.Count == 1)
                {
                    MainMenuManager.Instance.OpenLoadScreen();
                }
                SceneManager.LoadScene(DataManager.Instance.GetModeToSceneName(DataManager.Instance.gameMode));
            }

            if (SceneManager.GetActiveScene().name != "Ludo") return;
            if (LudoManager.Instance == null) return;
            LudoManager.Instance.PlayerJoined();
            //SetGameId(DataManager.Instance.tournamentID);
            print("Inside socket is called");


            //if (DataManager.Instance.isGameTestMode == tr)
            //{
            // if (DataManager.Instance.joinPlayerDatas.Count == 2)
            // {
            //     //StartCoroutine(MainMenuManager.Instance.LoadScene());
            //     MainMenuManager.Instance.ChangeScene();
            //     //DataManager.Instance.isBot = true;
            // }
            //}

            //if (DataManager.Instance.joinPlayerDatas.Count == 1)
            //{
            //    MainMenuManager.Instance.OpenTournamentLoadScreen();
            //}
        }
        else if (obj["users"][0]["maxp"] == 4)
        {

            print("Ludo Manage Player Number : " + DataManager.Instance.playerNo);
            print("Ludo Player Number Assign : " + obj["users"]);
            if (DataManager.Instance.playerNo == 0)
            {
                print("Enter The First Player  Con");
                if (obj["users"][0]["maxp"] == 4)
                {
                    DataManager.Instance.isFourPlayer = true;
                    DataManager.Instance.playerNo = obj["users"].Count;

                    if (DataManager.Instance.gameMode == GameType.Ludo)
                    {
                        if (DataManager.Instance.playerNo == 2)
                        {
                            DataManager.Instance.playerNo = 1; //
                        }
                    }

                }
            }


            for (int i = 0; i < 5; i++)
            {
                int pNo = 0;
                if (DataManager.Instance.isFourPlayer)
                {
                    if (i == 0)
                    {
                        pNo = 1;
                    }
                    else if (i == 1)
                    {
                        pNo = 2;
                    }
                    else if (i == 2)
                    {
                        pNo = 3;
                    }
                    else if (i == 3)
                    {
                        pNo = 4;
                    }
                }
                else
                {
                    pNo = i + 1;
                }
                if (values["data"]["users"][i]["name"] != null && values["data"]["users"][i]["userId"] != null && values["data"]["users"][i]["lobbyId"] != null)
                {
                    DataManager.Instance.AddRoomUser(values["data"]["users"][i]["userId"], values["data"]["users"][i]["name"], values["data"]["users"][i]["lobbyId"], values["data"]["users"][i]["balance"], pNo, values["data"]["users"][i]["avtar"]);
                }
                //if (obj["users"].Count == 2 && SceneManager.GetActiveScene().name == "Main")
                //{
                //    SceneManager.LoadScene(DataManager.Instance.GetModeToSceneName(DataManager.Instance.gameMode));
                //}

            }

            if (SceneManager.GetActiveScene().name != "Ludo") return;
            if (LudoManager.Instance == null) return;
            LudoManager.Instance.PlayerJoined();
            print("Inside socket is called");
        }
    }

    #region Waiting screen

    private void StartWaitingProcess()
    {
        loadingScreen.SetActive(true);
        if (DataManager.Instance.joinPlayerDatas.Count == 2)
        {
            CheckPlayersJoined();
            return;
        }
        Invoke(nameof(CheckPlayersJoined), 1f);
        //CancelInvoke(nameof(CheckPlayersJoined));
    }
    
    private void CheckPlayersJoined()
    {
        
        // Check if any players have joined
        if (DataManager.Instance.joinPlayerDatas.Count == 2)
        {
            // Start the game
            StartGame();
        }
        else
        {
            // Add the bot player
            AddBotPlayer();
        }
    }
    
    private void AddBotPlayer()
    {
        // Add the bot player to the DataManager
        MainMenuManager.Instance.OpenLoadScreen();
        StartGame();
    }
    
    private void StartGame()
    {
        StartCoroutine(LoadSceneAndDeactivateLoadingScreen());
    }
    
    IEnumerator LoadSceneAndDeactivateLoadingScreen()
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(DataManager.Instance.GetModeToSceneName(DataManager.Instance.gameMode));

        // Wait until the scene has finished loading
        while (!asyncOperation.isDone)
        {
            yield return null;
        }

        // Scene has finished loading, deactivate the loading screen
        loadingScreen.SetActive(false);
    }
    #endregion


    #region Ludo
    public void SetLudoData(string values)
    {
        if (SceneManager.GetActiveScene().name == "Ludo")
        {
            JSONNode value = JSON.Parse(values);
            JSONNode data = JSON.Parse(value["data"].ToString());

            string playerId = data["PlayerID"];
            string tourId = data["TournamentID"];
            int tokenNo = data["TokenNo"];
            int tokenMove = data["TokenMove"];
            int playerNo = data["PlayerNo"];

            string sRoomId = data["RoomId"];


            if (DataManager.Instance.playerData._id != playerId && tourId == DataManager.Instance.tournamentID && sRoomId == roomid && playerNo != DataManager.Instance.playerNo)
            {
                //print("Move Player No : " + playerNo);
                //print("Move Token No : " + tokenNo);
                //print("Move Token Move : " + tokenMove);
                LudoManager.Instance.AutoMove(playerNo, tokenNo, tokenMove);
            }
        }

    }
    public void SetLudoDiceData(string values)
    {
        if (SceneManager.GetActiveScene().name == "Ludo")
        {
            JSONNode value = JSON.Parse(values);
            JSONNode data = JSON.Parse(value["data"].ToString());

            string playerId = data["PlayerID"];
            string tourId = data["TournamentID"];
            int playerNo = data["PlayerNo"];
            //int DiceManageCnt = data["DiceManageCnt"];
            int diceNo = data["DiceNo"];
            string sRoomId = data["RoomId"];
            if (DataManager.Instance.playerData._id != playerId && tourId == DataManager.Instance.tournamentID && sRoomId == roomid && playerNo != DataManager.Instance.playerNo)
            {
                LudoManager.Instance.AutoDice(diceNo, DataManager.Instance.playerNo, playerNo);
            }
        }
    }

    [HideInInspector]
    public int changeCnt = 0;
    public void SetLudoChangeDiceData(string values)
    {
        if (SceneManager.GetActiveScene().name == "Ludo")
        {
            JSONNode value = JSON.Parse(values);
            JSONNode data = JSON.Parse(value["data"].ToString());

            string playerId = data["PlayerID"];
            string tourId = data["TournamentID"];
            int playerNo = data["PlayerNo"];
            int diceNo = data["DiceNo"];
            string sRoomId = data["RoomId"];

            if (DataManager.Instance.playerData._id != playerId)
            {
                //print("Data Player Id : " + DataManager.Instance.playerData._id);
                //print("Get Player Id : " + playerId);
                //print("Player No : " + playerNo);
                if (tourId == DataManager.Instance.tournamentID && sRoomId == roomid && playerNo == DataManager.Instance.playerNo)
                {
                    SoundManager.Instance.UserTurnSound();
                    if (DataManager.Instance.GetVibration() == 0)
                    {
                        if (Application.platform == RuntimePlatform.Android)
                        {
                            //MMNVAndroid.AndroidVibrate(100);
                        }
                    }

                    DataManager.Instance.isDiceClick = true;

                    LudoManager.Instance.isClickAvaliableDice = 0;
                    LudoManager.Instance.OurShadowMaintain();
                    DataManager.Instance.isTimeAuto = false;
                    LudoManager.Instance.RestartTimer();
                    //if (DataManager.Instance.modeType == 3)
                    //{
                    //    LudoManager.Instance.DiceLessPasaButton();
                    //}
                }

            }


        }
    }

    public void PausePlayerRequest(string values)
    {
        if (SceneManager.GetActiveScene().name == "Ludo")
        {
            JSONNode value = JSON.Parse(values);
            JSONNode data = JSON.Parse(value["data"].ToString());

            string playerId = data["PlayerID"];
            string tourId = data["TournamentID"];
            int playerNo = data["PlayerNo"];
            string sRoomId = data["RoomId"];

            if (DataManager.Instance.playerData._id != playerId)
            {
                print("Data Player Id : " + DataManager.Instance.playerData._id);
                print("Get Player Id : " + playerId);
                print("Player No : " + playerNo);
                if (tourId == DataManager.Instance.tournamentID && sRoomId == roomid && playerNo != DataManager.Instance.playerNo)
                {

                    LudoManager.Instance.PauseUserDataSend();

                }

            }


        }
    }

    public void PauseGetPlayerData(string values)
    {

        if (SceneManager.GetActiveScene().name == "Ludo")
        {
            JSONNode value = JSON.Parse(values);
            JSONNode data = JSON.Parse(value["data"].ToString());

            string playerId = data["PlayerID"];
            string tourId = data["TournamentID"];
            int playerNo = data["PlayerNo"];
            string sRoomId = data["RoomId"];
            float sliderValue = data["GreenSliderValue"];
            int dot = data["OurDot"];
            bool isTurn = data["Turn"];



            if (DataManager.Instance.playerData._id != playerId)
            {
                print("Data Player Id : " + DataManager.Instance.playerData._id);
                print("Get Player Id : " + playerId);
                print("Player No : " + playerNo);
                if (tourId == DataManager.Instance.tournamentID && sRoomId == roomid && playerNo != DataManager.Instance.playerNo)
                {
                    LudoManager.Instance.PauseDataRetriveSocket(sliderValue, dot, isTurn);
                }

            }


        }
    }


    public void SetLudoStopDiceData(string values)
    {
        if (SceneManager.GetActiveScene().name == "Ludo")
        {
            JSONNode value = JSON.Parse(values);
            JSONNode data = JSON.Parse(value["data"].ToString());

            string playerId = data["PlayerID"];
            string tourId = data["TournamentID"];

            string sRoomId = data["RoomId"];


            if (/*DataManager.Instance.playerData._id != playerId &&*/ tourId == DataManager.Instance.tournamentID && sRoomId == roomid)
            {
                //LudoManager.Instance.StopDiceLine();
            }
        }
    }

    #endregion


    #region Teen patti

    public void SetChangeTeenPatti(string values)
    {
        if (SceneManager.GetActiveScene().name == "TeenPatti")
        {
            JSONNode value = JSON.Parse(values);
            JSONNode data = JSON.Parse(value["data"].ToString());

            string playerId = data["PlayerID"];
            string tourId = data["TournamentID"];
            int playerNo = data["PlayerNo"];
            string sRoomId = data["RoomId"];

            print("Player Id : " + playerId);
            if (tourId == DataManager.Instance.tournamentID && sRoomId == DataManager.Instance.gameId)// && playerId == DataManager.Instance.playerData._id // to make single player
            {
                //print("Teen Patti playerNo : " + playerNo);

                TeenPattiManager.Instance.GetPlayerTurn(playerNo);
            }
        }

    }
    
    public void SetBotBetTeenPatti(string values)
    {
        if (SceneManager.GetActiveScene().name == "TeenPatti")
        {
            JSONNode value = JSON.Parse(values);
            JSONNode data = JSON.Parse(value["data"].ToString());
            
            string playerId = data["PlayerID"];
            string tourId = data["TournamentID"];
            string sRoomId = data["RoomId"];
            int botBetNo = data["BotNo"];
            int botPlayerNo = data["BotPlayerNo"];
            
            if (tourId == DataManager.Instance.tournamentID && playerId != DataManager.Instance.playerData._id)
            {
                //print("Teen Patti playerNo : " + playerNo);

                TeenPattiManager.Instance.GetBotBetNo(botBetNo, botPlayerNo);
            }
        }
    }

    public void SetBetTeenPatti(string values)
    {
        if (SceneManager.GetActiveScene().name == "TeenPatti")
        {
            JSONNode value = JSON.Parse(values);
            JSONNode data = JSON.Parse(value["data"].ToString());

            string playerID = data["PlayerID"];
            string tourId = data["TournamentID"];
            int playerNo = data["PlayerNo"];
            string sRoomId = data["RoomId"];
            float betAmount = data["BetAmount"];
            string betType = data["BetType"];
            string playerSlideShowSendId = data["playerSlideShowSendId"];
            string playerIdSlideShowId = data["playerIdSlideShowId"];

            if (!playerID.Equals(DataManager.Instance.playerData._id) && tourId == DataManager.Instance.tournamentID)// && playerID == DataManager.Instance.playerData._id
            {
                //print("Teen Patti playerNo : " + playerNo);

                TeenPattiManager.Instance.GetBet(playerNo, betAmount, betType, playerSlideShowSendId, playerIdSlideShowId);
            }
        }

    }
    public void SetSlideShowTeenPatti(string values)
    {
        if (SceneManager.GetActiveScene().name == "TeenPatti")
        {
            JSONNode value = JSON.Parse(values);
            JSONNode data = JSON.Parse(value["data"].ToString());

            string playerID = data["PlayerID"];
            string tourId = data["TournamentID"];
            int playerNo = data["PlayerNo"];
            string sRoomId = data["RoomId"];
            string SlideShowCancelPlayerId = data["SlideShowCancelPlayerId"];
            string SlideShowPlayerId = data["SlideShowCancelPlayerId"];
            string SlideShowType = data["SlideShowType"];




            if (playerID.Equals(SlideShowPlayerId) && tourId == DataManager.Instance.tournamentID && playerID != DataManager.Instance.playerData._id)
            {
                //print("Teen Patti playerNo : " + playerNo);
                if (SlideShowType == "Accept")
                {
                    TeenPattiManager.Instance.SlideShow_Accpet_Socket(SlideShowPlayerId, playerID);
                }
                else if (SlideShowType == "Cancel")
                {
                    TeenPattiManager.Instance.SlideShow_Cancel_Socket();
                }
            }
        }

    }
    public void SetWinTeenPatti(string values)
    {
        if (SceneManager.GetActiveScene().name == "TeenPatti")
        {
            JSONNode value = JSON.Parse(values);
            JSONNode data = JSON.Parse(value["data"].ToString());

            string playerID = data["PlayerID"];
            string tourId = data["TournamentID"];
            string sRoomId = data["RoomId"];
            string WinnerPlayerId = data["WinnerPlayerId"];




            if (tourId == DataManager.Instance.tournamentID && sRoomId == DataManager.Instance.gameId)//playerID == DataManager.Instance.playerData._id
            {
                //print("Teen Patti playerNo : " + playerNo);
                //WinnerList

                //string[] a1 = WinnerList.Split(",");
                /*List<int> winnerNumber = new List<int>();
                for (int i = 0; i < a1.Length; i++)
                {
                    if (a1[i] != null && a1[i].Length != 0)
                    {
                        try
                        {
                            int winnerNo = int.Parse(a1[i]);
                            winnerNumber.Add(winnerNo);
                        }
                        catch (Exception e)
                        {

                        }
                    }
                }*/
                //TeenPattiManager.Instance.CreditWinnerAmount(playerID);

                TeenPattiManager.Instance.HandelTeenPattiWinData(WinnerPlayerId);
            }
        }

    }

    public void SetChangeStatusTeenPatti(string values)
    {
        if (SceneManager.GetActiveScene().name == "TeenPatti")
        {
            JSONNode value = JSON.Parse(values);
            JSONNode data = JSON.Parse(value["data"].ToString());

            string playerId = data["PlayerID"];
            string tourId = data["TournamentID"];
            int playerNo = data["PlayerNo"];
            string sRoomId = data["RoomId"];

            string cardStatus = data["CardStatus"];
            if (tourId == DataManager.Instance.tournamentID && sRoomId == DataManager.Instance.gameId )//playerId == DataManager.Instance.playerData._id
            {
                print("Teen Patti playerNo : " + playerNo);
                TeenPattiManager.Instance.GetCardStatus(cardStatus, playerNo);
            }
        }

    }
    #endregion


    #region Roulette

    public void SetBetRoulette(string values)
    {
        if (SceneManager.GetActiveScene().name == "Rouletee")
        {
            JSONNode value = JSON.Parse(values);
            JSONNode data = JSON.Parse(value["data"].ToString());

            string playerID = data["PlayerID"];
            string tourId = data["TournamentID"];
            int playerNo = data["PlayerNo"];
            string sRoomId = data["RoomId"];
            int chipNo = data["chipNo"];
            if (!playerID.Equals(DataManager.Instance.playerData._id) && tourId == DataManager.Instance.tournamentID && sRoomId == roomid)
            {
                //print("Teen Patti playerNo : " + playerNo);
                RouletteManager.Instance.GetBetSocket(chipNo);
            }
        }

    }

    public void FindRouletteData(string values)
    {
        if (SceneManager.GetActiveScene().name == "Rouletee")
        {
            JSONNode value = JSON.Parse(values);
            JSONNode data = JSON.Parse(value["data"].ToString());

            string playerID = data["PlayerID"];
            string tourId = data["TournamentID"]; ;
            string sRoomId = data["RoomId"];
            if (!playerID.Equals(DataManager.Instance.playerData._id) && tourId == DataManager.Instance.tournamentID && sRoomId == roomid && RouletteManager.Instance.isAdmin)
            {
                RouletteManager.Instance.SendAdminDataPlayer(playerID);
            }
        }

    }

    public void SendAdminRouleteeData(string values)
    {
        if (SceneManager.GetActiveScene().name == "Rouletee")
        {
            JSONNode value = JSON.Parse(values);
            JSONNode data = JSON.Parse(value["data"].ToString());

            string adminPlayerID = data["AdminPlayerID"];
            string receivePlayerID = data["ReceivePlayerID"];
            string tourId = data["TournamentID"];
            string sRoomId = data["RoomId"];
            int time = data["Time"];
            int rouletteNumber = data["RouletteNumber"];

            if (receivePlayerID.Equals(DataManager.Instance.playerData._id) && tourId == DataManager.Instance.tournamentID && sRoomId == roomid && !RouletteManager.Instance.isAdmin)
            {
                RouletteManager.Instance.GetAdminDataPlayer(time, rouletteNumber);
            }
        }

    }

    #endregion


    #region AndarBahar

    public void SetBetAndarBahar(string values)
    {
        if (SceneManager.GetActiveScene().name == "AndarBahar")
        {
            JSONNode value = JSON.Parse(values);
            JSONNode data = JSON.Parse(value["data"].ToString());

            string playerID = data["PlayerID"];
            string tourId = data["TournamentID"];
            string sRoomId = data["RoomId"];
            bool isAndar = data["isAndar"];
            bool isBahar = data["isBahar"];
            float value1 = data["value"];

            if (!playerID.Equals(DataManager.Instance.playerData._id) && tourId == DataManager.Instance.tournamentID && sRoomId == roomid)
            {
                AndarBaharManager.Instance.GetBet(isAndar, isBahar, playerID, value1);
            }
        }

    }
    #endregion

    #region DragonTiger

    public void SetBetDragonTiger(string values)
    {
        if (SceneManager.GetActiveScene().name == "DragonTiger")
        {
            JSONNode value = JSON.Parse(values);
            JSONNode data = JSON.Parse(value["data"].ToString());

            string playerID = data["PlayerID"];
            string tourId = data["TournamentID"];
            string sRoomId = data["RoomId"];
            int boxNo = data["boxNo"];
            int chipNo = data["chipNo"];

            if (!playerID.Equals(DataManager.Instance.playerData._id) && tourId == DataManager.Instance.tournamentID && sRoomId == roomid)
            {
                DragonTigerManager.Instance.GetDragonTigerBet(boxNo, chipNo);
            }
        }

    }

    #endregion

    #region Poker

    public void SetChangePoker(string values)
    {
        if (SceneManager.GetActiveScene().name == "Poker")
        {
            JSONNode value = JSON.Parse(values);
            JSONNode data = JSON.Parse(value["data"].ToString());

            string playerId = data["PlayerID"];
            string tourId = data["TournamentID"];
            int playerNo = data["PlayerNo"];
            string sRoomId = data["RoomId"];

            print("Player Id : " + playerId);
            if (tourId == DataManager.Instance.tournamentID && sRoomId == roomid)
            {
                //print("Teen Patti playerNo : " + playerNo);

                PokerGameManager.Instance.GetPlayerTurn(playerNo);
            }
        }

    }
    
    public void SetBotBetPoker(string values)
    {
        if (SceneManager.GetActiveScene().name == "Poker")
        {
            JSONNode value = JSON.Parse(values);
            JSONNode data = JSON.Parse(value["data"].ToString());
            
            string tourId = data["TournamentID"];
            string sRoomId = data["RoomId"];
            int botBetNo = data["BotNo"];
            int botPlayerNo = data["BotPlayerNo"];
            float botBetAmount = data["BetAmount"];
            
            if (tourId == DataManager.Instance.tournamentID && sRoomId == roomid)
            {
                //print("Teen Patti playerNo : " + playerNo);

                PokerGameManager.Instance.GetBotBetNo(botBetNo, botPlayerNo, botBetAmount);
            }
        }
    }


    public void SetBetPoker(string values)
    {
        if (SceneManager.GetActiveScene().name == "Poker")
        {
            JSONNode value = JSON.Parse(values);
            JSONNode data = JSON.Parse(value["data"].ToString());

            string playerID = data["PlayerID"];
            string tourId = data["TournamentID"];
            int playerNo = data["PlayerNo"];
            string sRoomId = data["RoomId"];
            float betAmount = data["BetAmount"];
            string betType = data["BetType"];

            if (!playerID.Equals(DataManager.Instance.playerData._id) && tourId == DataManager.Instance.tournamentID && sRoomId == roomid)
            {
                //print("Teen Patti playerNo : " + playerNo);

                PokerGameManager.Instance.GetPokerBet(playerNo, betAmount, betType);
            }
        }

    }

    public void SetFoldPoker(string values)
    {
        if (SceneManager.GetActiveScene().name == "Poker")
        {
            JSONNode value = JSON.Parse(values);
            JSONNode data = JSON.Parse(value["data"].ToString());

            string playerID = data["PlayerID"];
            string tourId = data["TournamentID"];
            string playerNo = data["FoldPlayerId"];
            string sRoomId = data["RoomId"];

            if (tourId == DataManager.Instance.tournamentID && sRoomId == roomid)
            {
                //print("Teen Patti playerNo : " + playerNo);

                PokerGameManager.Instance.GetPokerFold(playerNo);
            }
        }

    }
    
    public void HandelWinPoker(string values)
    {
        if (SceneManager.GetActiveScene().name != "Poker") return;
        JSONNode value = JSON.Parse(values);
        JSONNode data = JSON.Parse(value["data"].ToString());

        string playerID = data["PlayerID"];
        string tourId = data["TournamentID"];
        string sRoomId = data["RoomId"];
        string winnerPlayerId = data["WinnerPlayerId"];
            
        if (tourId == DataManager.Instance.tournamentID && sRoomId == roomid)
        {
            PokerGameManager.Instance.CallFinalWinner(winnerPlayerId);
        }
    }

    
    public void SetWinPoker(string values)
    {
        if (SceneManager.GetActiveScene().name == "Poker")
        {
            JSONNode value = JSON.Parse(values);
            JSONNode data = JSON.Parse(value["data"].ToString());

            string playerID = data["PlayerID"];
            string tourId = data["TournamentID"];
            string sRoomId = data["RoomId"];
            string WinnerList = data["WinnerList"];




            if (tourId == DataManager.Instance.tournamentID && sRoomId == roomid)
            {
                //print("Teen Patti playerNo : " + playerNo);
                //WinnerList

                string[] a1 = WinnerList.Split(",");
                List<int> winnerNumber = new List<int>();
                for (int i = 0; i < a1.Length; i++)
                {
                    if (a1[i] != null && a1[i].Length != 0)
                    {
                        try
                        {
                            int winnerNo = int.Parse(a1[i]);
                            winnerNumber.Add(winnerNo);
                        }
                        catch (Exception e)
                        {

                        }
                    }
                }
                PokerGameManager.Instance.GetWinners(winnerNumber);
            }
        }

    }
    #endregion
    
    #region Archery

    public void SetArcheryChange(string values)
    {
        if (SceneManager.GetActiveScene().name == "Archery")
        {
            JSONNode value = JSON.Parse(values);
            JSONNode data = JSON.Parse(value["data"].ToString());
            print("Tic Tac : " + values);
            string playerId = data["PlayerID"];
            string tourId = data["TournamentID"];
            int playerNo = data["PlayerNo"];
            string sRoomId = data["RoomId"];
            int score = data["Score"];


            if (DataManager.Instance.playerData._id != playerId)
            {
                //print("Data Player Id : " + DataManager.Instance.playerData._id);
                //print("Get Player Id : " + playerId);
                //print("Player No : " + playerNo);
                if (tourId == DataManager.Instance.tournamentID && sRoomId == roomid && playerNo == DataManager.Instance.playerNo)
                {
                    ArcheryScroreManager.Instance.ArcherySendDataReceiveData(score);
                }

            }


        }



    }

    #endregion

    #region Carrom

    private void SetSliderChange(string values)
    {
        if (SceneManager.GetActiveScene().name != "Carrom") return;
        JSONNode value = JSON.Parse(values);
        JSONNode data = JSON.Parse(value["data"].ToString());
        print("Slider : " + values);
        string playerId = data["PlayerID"];
        string tourId = data["TournamentID"];
        int playerNo = data["PlayerNo"];
        string sRoomId = data["RoomId"];
        float pos = data["SliderPos"];


        if (DataManager.Instance.playerData._id == playerId) return;
        if (tourId == DataManager.Instance.tournamentID && sRoomId == roomid && playerNo != DataManager.Instance.playerNo)
        {
            CarromSocketManager.Instance.Strike_Slider_Receive(pos);
        }
    }


    #endregion

    #region Set Room Data

    public void SetRoomdata(string roomId, JSONObject data)
    {
        JSONObject keys = new JSONObject();
        keys.AddField("room", roomId);
        keys.AddField("gameData", data);
        Debug.Log("SendData===:::" + keys);
        socket.Emit("setGameData", keys);
    }
    
    public void HandelSetGameId(string values)
    {
        JSONNode value = JSON.Parse(values);
        JSONNode data = JSON.Parse(value["data"].ToString());
        DataManager.Instance.gameId = data["gameId"];
        Debug.Log("SendData===:::" + data);
    }
    
    
    public void SetGameId(string lobbyId)
    {
        JSONObject keys = new JSONObject();
        keys.AddField("room", roomid);
        keys.AddField("lobbyId", lobbyId);
        Debug.Log("SendData===:::" + keys);
        socket.Emit("setGameId", keys);
    }

    public void SetLobbyCount(string lobbyId)
    {
        JSONObject keys = new JSONObject();
        keys.AddField("lobbyId", lobbyId);

        //Debug.Log("SendData===:::" + keys.ToString());
        socket.Emit("lobbyStat", keys);
    }
    

    public void SendChatMessageManage(string values)
    {

        JSONNode value = JSON.Parse(values);
        JSONNode data = JSON.Parse(value["data"].ToString());

        string sRoomId = data["RoomId"];
        string msg = data["Message"];
        string gameName = data["gameName"];
        string playerId = data["PlayerID"];
        if (sRoomId == roomid)
        {
            if (gameName == "TeenPatti")
            {
                TeenPattiManager.Instance.GetChat(playerId, msg);
            }
            else if (gameName == "AndarBahar")
            {
                AndarBaharManager.Instance.GetChat(playerId, msg);
            }
            else if (gameName == "Poker")
            {
                AndarBaharManager.Instance.GetChat(playerId, msg);
            }
        }

    }

    public void SendGiftMessageManage(string values)
    {

        JSONNode value = JSON.Parse(values);
        JSONNode data = JSON.Parse(value["data"].ToString());

        string sRoomId = data["RoomId"];
        string gameName = data["gameName"];
        string sendPlayerID = data["SendPlayerID"];
        string receivePlayerID = data["ReceivePlayerID"];
        int giftNo = data["GiftNo"];
        if (sRoomId == roomid)
        {
            if (gameName == "TeenPatti")
            {
                TeenPattiManager.Instance.GetGift(sendPlayerID, receivePlayerID, giftNo);
            }
            else if (gameName == "AndarBahar")
            {
                AndarBaharManager.Instance.GetGift(sendPlayerID, receivePlayerID, giftNo);
            }
            else if (gameName == "Poker")
            {
                PokerGameManager.Instance.GetGift(sendPlayerID, receivePlayerID, giftNo);
            }
        }


    }


    public void GetRoomData(string data)
    {
        JSONNode value = JSON.Parse(data);
        print(value.ToString());
        JSONNode valueData = JSON.Parse(value["data"].ToString());

        if (roomid.Equals(valueData["room"]))
        {
            int gameMode = valueData["gameData"]["gameMode"];
            int deckNo = valueData["gameData"]["DeckNo"];
            int deckNo1 = valueData["gameData"]["DeckNo1"];
            int deckNo2 = valueData["gameData"]["DeckNo2"];
            string winData = valueData["gameData"]["WinList"];
            string playerId = valueData["gameData"]["PlayerID"];

            if (gameMode == 1)
            {
                TeenPattiManager.Instance. GetRoomData(deckNo, deckNo2, playerId);
            }
            else if (gameMode == 2)
            {
                RouletteManager.Instance.GetRoomData(deckNo);
            }
            else if (gameMode == 3)
            {
                //AndarBaharManager.Instance.GetRoomData(deckNo);
            }
            else if (gameMode == 4)
            {
                DragonTigerManager.Instance.GetRoomData(deckNo1, deckNo2);
            }
            else if (gameMode == 5)
            {
                PokerGameManager.Instance.GetRoomData(deckNo1, deckNo2);
            }
        }

        //JSONNode data = JSON.Parse(value["data"].ToString());
    }

    #endregion



    public void LeaveRoom()
    {
        JSONObject obj = new JSONObject();
        obj.AddField("room", roomid);
        obj.AddField("lobbyId", DataManager.Instance.tournamentID);
        obj.AddField("userId", DataManager.Instance.playerData._id);
        DataManager.Instance.tournamentID = "";
        DataManager.Instance.tourEntryMoney = 0;
        DataManager.Instance.tourCommision = 0;
        DataManager.Instance.commisionAmount = 0;
        DataManager.Instance.orgIndexPlayer = 0;
        DataManager.Instance.joinPlayerDatas.Clear();
        roomid = "";
        userdata = "";
        playTime = 0;
        //obj.AddField("userId", Datamanger.Intance.userID.Trim('"'));
        socket.Emit("leave", obj);
    }

    public void ResetRole(string values)
    {

        JSONNode keys = JSON.Parse(values.ToString());
        JSONNode data = JSON.Parse(keys["data"]["users"].ToString());
        userdata = data.ToString();
        bool isAvaliable = false;


        print("Key Data : " + keys.ToString());
        print("data Data : " + data.ToString());

        string room = keys["data"]["room"];
        string leaveUserId = keys["data"]["userId"];
        string playerId = data[0]["userId"];
        string lobbyID = data[0]["lobbyId"];



        //if (DataManager.Instance.isTwoPlayer)
        //{
        //print("Room Id : " + roomid);
        //print("leaver Id : " + leaveUserId);
        //print("player Id : " + playerId);
        //print("Lobby Id : " + lobbyID);
        if (DataManager.Instance.gameMode == GameType.Teen_Patti)
        {
            if (TeenPattiManager.Instance != null)
            {
                int index = DataManager.Instance.joinPlayerDatas.FindIndex(player => player.userId == leaveUserId);
                if (index != -1)
                {
                    DataManager.Instance.joinPlayerDatas.RemoveAt(index);
                }
                
                if (room == roomid && lobbyID == DataManager.Instance.tournamentID)
                {
                    TeenPattiManager.Instance.ChangeAAdmin(leaveUserId, playerId);
                }
            }
        }
        if (DataManager.Instance.gameMode == GameType.Poker)
        {
            if (PokerGameManager.Instance != null)
            {
                //DataManager.Instance.leaveUpdatePlayerDatas.Clear();

                /*DataManager.Instance.joinPlayerDatas.Clear();
                for (int i = 0; i < data.Count; i++)
                {
                    JoinPlayerData jData = new JoinPlayerData();
                    jData.userId = data[i]["userId"];
                    jData.userName = data[i]["name"];
                    jData.balance = data[i]["balance"];
                    jData.lobbyId = data[i]["lobbyId"];
                    jData.playerNo = 0;
                    jData.avtar = data[i]["avtar"];
                    //DataManager.Instance.leaveUpdatePlayerDatas.Add(jData);
                    DataManager.Instance.joinPlayerDatas.Add(jData);

                }*/
                
                int index = DataManager.Instance.joinPlayerDatas.FindIndex(player => player.userId == leaveUserId);
                if (index != -1)
                {
                    DataManager.Instance.joinPlayerDatas.RemoveAt(index);
                }
                
                if (room == roomid && lobbyID == DataManager.Instance.tournamentID)
                {
                    PokerGameManager.Instance.ChangeAAdmin(leaveUserId, playerId);
                }
            }
        }

        /*if (DataManager.Instance.gameMode == GameType.Roulette)
        {
            if (RouletteManager.Instance != null)
            {
                //DataManager.Instance.leaveUpdatePlayerDatas.Clear();
                DataManager.Instance.joinPlayerDatas.Clear();
                for (int i = 0; i < data.Count; i++)
                {
                    JoinPlayerData jData = new JoinPlayerData();
                    jData.userId = data[i]["userId"];
                    jData.userName = data[i]["name"];
                    jData.balance = data[i]["balance"];
                    jData.lobbyId = data[i]["lobbyId"];
                    jData.playerNo = 0;
                    jData.avtar = data[i]["avtar"];
                    //DataManager.Instance.leaveUpdatePlayerDatas.Add(jData);
                    DataManager.Instance.joinPlayerDatas.Add(jData);

                }

                if (room == roomid && lobbyID == DataManager.Instance.tournamentID)
                {
                    RouletteManager.Instance.ChangeAAdmin(leaveUserId, playerId);
                }
            }
        }
        if (DataManager.Instance.gameMode == GameType.Andar_Bahar)
        {
            if (AndarBaharManager.Instance != null)
            {
                //DataManager.Instance.leaveUpdatePlayerDatas.Clear();
                DataManager.Instance.joinPlayerDatas.Clear();
                for (int i = 0; i < data.Count; i++)
                {
                    JoinPlayerData jData = new JoinPlayerData();
                    jData.userId = data[i]["userId"];
                    jData.userName = data[i]["name"];
                    jData.balance = data[i]["balance"];
                    jData.lobbyId = data[i]["lobbyId"];
                    jData.playerNo = 0;
                    jData.avtar = data[i]["avtar"];
                    //DataManager.Instance.leaveUpdatePlayerDatas.Add(jData);
                    DataManager.Instance.joinPlayerDatas.Add(jData);

                }
                //print("Room 1: " + room);
                //print("Room 1: " + roomid);
                if (room == roomid && lobbyID == DataManager.Instance.tournamentID)
                {
                    AndarBaharManager.Instance.ChangeAAdmin(leaveUserId, playerId);
                }
            }
        }
        if (DataManager.Instance.gameMode == GameType.Dragon_Tiger)
        {
            if (DragonTigerManager.Instance != null)
            {
                DataManager.Instance.leaveUpdatePlayerDatas.Clear();
                for (int i = 0; i < data.Count; i++)
                {
                    JoinPlayerData jData = new JoinPlayerData();
                    jData.userId = data[i]["userId"];
                    jData.userName = data[i]["name"];
                    jData.balance = data[i]["balance"];
                    jData.lobbyId = data[i]["lobbyId"];
                    jData.playerNo = 0;
                    jData.avtar = data[i]["avtar"];
                    DataManager.Instance.leaveUpdatePlayerDatas.Add(jData);

                }

                if (room == roomid && lobbyID == DataManager.Instance.tournamentID)
                {
                    DragonTigerManager.Instance.ChangeAAdmin(leaveUserId, playerId);
                }
            }
        }*/


        if (data[0]["userId"].Equals(DataManager.Instance.playerData._id))
        {
            isAvaliable = true;
        }
        //}
        if (isAvaliable == true)
        {
            if (DataManager.Instance.gameMode == GameType.Ludo)
            {
                if (LudoManager.Instance != null)
                {
                    LudoManager.Instance.isOtherPlayLeft = true;
                    LudoManager.Instance.WinUserShow();
                }
            }
        }
        //SocketGameplay.Instace.ResetGameData();
    }









}
