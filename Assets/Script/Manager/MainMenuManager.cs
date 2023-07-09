using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;
using UnityEngine.Networking;
using WebSocketSharp;
using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;


public class MainMenuManager : MonoBehaviour
{

    public static MainMenuManager Instance;

    [Header("--- Home ---")]
    public Image avatarImg;
    public Text userNameTxt;
    public Text userIdTxt;
    public Text coinTxt;
    public Text secondCoinText;
    public Text diamondTxt;
    public TournamentData tourData;

    [Header("--- Prefab ---")]
    public GameObject prefabParent;
    public GameObject editProfilePrefab;
    public GameObject settingPrefab;
    public GameObject withdrawPrefab;
    public GameObject shopScreenPrefab;
    public GameObject contactUsPrefab;
    public GameObject tourErrorPrefab;
    public GameObject ludoLoadingPrefab;
    public GameObject tranHistoryPrefab;
    public GameObject refferalPrefab;
    public GameObject withdrawErrorPrefab;


    [Header("--- Tournament ---")]
    public GameObject tournamentTeenPattiPrefab;
    public GameObject tournamentLudoPrefab;
    //public bool isPressJoin = false;
    public GameObject ludoSelectorPrefab;
    public int minPlayerRequired = 5;
    public GameObject tournamentPanel;
    public Text tournamentTitle;
    public GameObject tournamentSecondScrollParentObj;
    public GameObject tourSecondPrefab;
    public GameObject lobbyObj;
    public int totalTournament;
    public Button freeButton;
    public Button paidButton;
    public Sprite onImage;
    public Sprite offImage;
    public Color onColor;
    public Color offColor;
    public GameObject waitingPanel;
    public GameObject playerNotFound;

    [Header("--- LeavePanel ---")] 
    
    public GameObject leaveMainPanel;

    //public GameObject settingUpdateObj;

    [Header("---Screen---")]
    public List<GameObject> screenObj = new List<GameObject>();

    [Header("---Notification List Panel---")]
    public GameObject notificationListPanel;
    public GameObject notificationRedDot;

    [Header("---Withdraw---")]
    public bool isWithdraw;

    public List<NotiBarManage> notiBarManages = new List<NotiBarManage>();
    public int botPlayers;
    bool isPressJoin;

    [Header("Buttons")]
    [SerializeField] private GameObject m_AllOn;
    [SerializeField] private GameObject m_AllOff;
    [SerializeField] private GameObject m_MultiplayerOn;
    [SerializeField] private GameObject m_MultiplayerOff;
    [SerializeField] private GameObject m_SkillsOn;
    [SerializeField] private GameObject m_SkillsOff;
    [SerializeField] private GameObject m_SlotsOn;
    [SerializeField] private GameObject m_SlottsOff;
    [SerializeField] private GameObject m_CardsOn;
    [SerializeField] private GameObject m_CardsOff;

    [Header("Games")]
    [SerializeField] private GameObject g_Slots;
    [SerializeField] private GameObject g_Poker;
    [SerializeField] private GameObject g_Rummy;
    [SerializeField] private GameObject g_Chess;
    [SerializeField] private GameObject g_Ludo;
    [SerializeField] private GameObject g_Carrom;
    [SerializeField] private GameObject g_Archery;
    [SerializeField] private GameObject g_8BallPool;
    [SerializeField] private GameObject g_TeenPatti;
    public GameObject loadingScene;
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
        //GetVersionUpdate();
        UpdateAllData();
        //GetTournament();
        //GetTran();
        DataManager.Instance.GetTournament();
    }


    public void UpdateAllData()
    {
        Getdata();
        //Getnotification();
    }
    // Update is called once per frame
    void Update()
    {
        if (DataManager.Instance.tournamentData != null || DataManager.Instance.tournamentData.Count != 0) return;
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("Splash");
    }

    #region Home

    public void ProfileButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        GenerateEditProfile();
    }

    public void SettingButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        GenerateSetting();
    }

    public void CustomerButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        Application.OpenURL("mailto: " + "support@starxplayzone.com" + " ? subject = " + "subject" + " & body = " + "body");
        //GenerateContactUs();
    }

    public void MailButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        GenerateNotification();
    }

    public void VIPButtonClick()
    {

    }

    public void LoadTournamentData()
    {
        if (DataManager.Instance.gameMode == GameType.Teen_Patti)
        {
            string Tour = IsAvaliableSingleTournament(GameType.Teen_Patti);
            if (!string.IsNullOrEmpty(Tour))
            {
                DataManager.Instance.tournamentID = Tour;
                TestSocketIO.Instace.TeenPattiJoinroom();
            }
            else
            {
                GenerateTournamentError();
            }
        }
        
    }


    /*public void GameButtonClick(int no)
    {
        SoundManager.Instance.ButtonClick();
        switch (no)
        {
            case 1:
                // Teen Patti

                DataManager.Instance.gameMode = GameType.Teen_Patti;
                tournamentScene.SetActive(true);
                
                break;
            case 2:
                // Dragon Tiger
                /*DataManager.Instance.gameMode = GameType.Dragon_Tiger;
            string getTour = IsAvaliableSingleTournament(GameType.Dragon_Tiger);
            if (getTour != null && getTour.Length != 0)
            {
                DataManager.Instance.tournamentID = getTour;
                TestSocketIO.Instace.RouletteJoinroom();
            }
            else
            {
                GenerateTournamentError();
            }#1#
                break;
            case 3:
                /#1#/ Roulette
            DataManager.Instance.gameMode = GameType.Roulette;
            string getTour = IsAvaliableSingleTournament(GameType.Roulette);
            if (getTour != null && getTour.Length != 0)
            {
                DataManager.Instance.tournamentID = getTour;
                TestSocketIO.Instace.RouletteJoinroom();
            }
            else
            {
                GenerateTournamentError();
            }#1#
                break;
            case 4:
            {
                // Poker
                DataManager.Instance.gameMode = GameType.Poker;
                //SceneManager.LoadScene(DataManager.Instance.GetModeToSceneName(DataManager.Instance.gameMode));
                string getTour = IsAvaliableSingleTournament(GameType.Poker);
                if (!string.IsNullOrEmpty(getTour))
                {
                    DataManager.Instance.tournamentID = getTour;
                    TestSocketIO.Instace.TeenPattiJoinroom();
                }
                else
                {
                    GenerateTournamentError();
                }

                break;
            }
            case 5:
                /*DataManager.Instance.gameMode = GameType.Andar_Bahar;
            // Andar Bahar
            string getTour = IsAvaliableSingleTournament(GameType.Andar_Bahar);
            if (getTour != null && getTour.Length != 0)
            {
                DataManager.Instance.tournamentID = getTour;
                TestSocketIO.Instace.AndarBaharJoinroom();
            }

            //SceneManager.LoadScene(DataManager.Instance.GetModeToSceneName(DataManager.Instance.gameMode));
            //string getTour = IsAvaliableSingleTournament(GameType.Roulette);
            //if (getTour != null && getTour.Length != 0)
            //{
            //    DataManager.Instance.tournamentID = getTour;
            //    TestSocketIO.Instace.RouletteJoinroom();
            //}
            //else
            //{
            //    GenerateTournamentError();
            //}#1#
                break;
            case 6:
                //Ludo
                /*DataManager.Instance.gameMode = GameType.Ludo;

                Instantiate(tournamentLudoPrefab, prefabParent.transform);
                TournamentPanel.Instance.gameType = GameType.Ludo;
                TournamentPanel.Instance.GenerateTournament();#1#
                Instantiate(ludoSelectorPrefab, prefabParent.transform);
                
                //string getTour = IsAvaliableSingleTournament(GameType.Ludo);
                //if (getTour != null && getTour.Length != 0)
                //{
                //    DataManager.Instance.tournamentID = getTour;//AA

                //    Screen.orientation = ScreenOrientation.Portrait;
                //    TestSocketIO.Instace.playTime = 2f;
                //    DataManager.Instance.playerNo = 0;
                //    DataManager.Instance.diceManageCnt = 0;
                //    DataManager.Instance.tourEntryMoney = 0;
                //    DataManager.Instance.winAmount = 0;
                //    //DataManager.Instance.tourBon
                //    TestSocketIO.Instace.LudoJoinroom();
                //}
                break;
            case 7:
                DataManager.Instance.gameMode = GameType.Archery;
                //lno = 8;
                DataManager.Instance.isTwoPlayer = true;
                string data = CheckTournament(GameType.Archery);
                if (!string.IsNullOrEmpty(data))
                {
                    DataManager.Instance.tournamentID = data;
                    //TestSocketIO.Instace.TeenPattiJoinroom();
                }
                else
                {
                    GenerateTournamentError();
                    break;
                }
                JoinTheGame();
                break;
            case 8:
                DataManager.Instance.gameMode = GameType.Carrom;
                //lno = 8;
                DataManager.Instance.isTwoPlayer = true;
                string gamedata = CheckTournament(GameType.Carrom);
                if (!string.IsNullOrEmpty(gamedata))
                {
                    DataManager.Instance.tournamentID = gamedata;
                    //TestSocketIO.Instace.TeenPattiJoinroom();
                }
                else
                {
                    GenerateTournamentError();
                    break;
                }
                JoinTheGame();
                break;
            case 9:
                DataManager.Instance.gameMode = GameType.Chess;
                //lno = 8;
                DataManager.Instance.isTwoPlayer = true;
                string chessdata = CheckTournament(GameType.Chess);
                if (!string.IsNullOrEmpty(chessdata))
                {
                    DataManager.Instance.tournamentID = chessdata;
                    //TestSocketIO.Instace.TeenPattiJoinroom();
                }
                else
                {
                    GenerateTournamentError();
                    break;
                }
                JoinTheGame();
                break;
            case 10:
                DataManager.Instance.gameMode = GameType.Slot;
                //lno = 8;
                DataManager.Instance.isTwoPlayer = true;
                string slotData = CheckTournament(GameType.Slot);
                if (!string.IsNullOrEmpty(slotData))
                {
                    DataManager.Instance.tournamentID = slotData;
                    //TestSocketIO.Instace.TeenPattiJoinroom();
                }
                else
                {
                    GenerateTournamentError();
                    break;
                }
                JoinTheGame();
                
                break;
        }
    }
    */
    
    public void GameButtonClick(int gameNo)
    {
        SoundManager.Instance.ButtonClick();
        GameType gameName;

        switch (gameNo)
        {
            case 1:
                gameName = GameType.Slot;
                break;
            case 2:
                gameName = GameType.Poker;
                break;
            case 3:
                gameName = GameType.Rummy;
                break;
            case 4:
                gameName = GameType.Teen_Patti;
                break;
            case 5:
                gameName = GameType.Carrom;
                break;
            case 6:
                gameName = GameType.Ludo;
                break;
            case 7:
                gameName = GameType.Archery;
                break;
            case 8:
                gameName = GameType.Ball_Pool;
                break;
            case 9:
                gameName = GameType.Chess;
                break;
            default:
                // Invalid game number
                return;
        }

        OpenSecondTournament(gameName);
    }
    
    #region Tournament
    
    
    
    /*int lno = 0;

    int cntMove = 0;
    List<TourData> tour_Datas = new List<TourData>();
    private GameType currentSelectedGame;
    private bool isFreeSelected = false;
    private bool isPaidSelected = false;
    
    
     void OpenSecondTournament(GameType selectedGame)
    {
        tournamentPanel.SetActive(true);
        cntMove = 0;
        if (selectedGame == GameType.Slot)
        {
            tournamentTitle.text = "Slot";
            DataManager.Instance.isTwoPlayer = true;
            DataManager.Instance.gameMode = GameType.Slot;
        }
        else if (selectedGame == GameType.Poker)
        {
            tournamentTitle.text = "Poker";
            //DataManager.Instance.isTwoPlayer = true;
            DataManager.Instance.gameMode = GameType.Poker;
        }
        else if (selectedGame == GameType.Rummy)
        {
            tournamentTitle.text = "Rummy";
            //DataManager.Instance.isTwoPlayer = true;
            DataManager.Instance.gameMode = GameType.Rummy;
        }
        else if (selectedGame == GameType.Teen_Patti)
        {
            tournamentTitle.text = "TeenPatti";
            //DataManager.Instance.isTwoPlayer = true;
            DataManager.Instance.gameMode = GameType.Teen_Patti;
        }
        else if (selectedGame == GameType.Carrom)
        {
            tournamentTitle.text = "Carrom";
            //DataManager.Instance.isTwoPlayer = true;
            DataManager.Instance.gameMode = GameType.Carrom;
        }
        else if (selectedGame == GameType.Ludo)
        {
            tournamentTitle.text = "Ludo";
            //DataManager.Instance.isTwoPlayer = true;
            DataManager.Instance.gameMode = GameType.Ludo;
        }
        else if (selectedGame == GameType.Archery)
        {
            tournamentTitle.text = "Archery";
            //DataManager.Instance.isTwoPlayer = true;
            DataManager.Instance.gameMode = GameType.Archery;
        }
        else if (selectedGame == GameType.Ball_Pool)
        {
            tournamentTitle.text = "BallPool";
            //DataManager.Instance.isTwoPlayer = true;
            DataManager.Instance.gameMode = GameType.Ball_Pool;
        }
        else if (selectedGame == GameType.Chess)
        {
            tournamentTitle.text = "Chess";
            //DataManager.Instance.isTwoPlayer = true;
            DataManager.Instance.gameMode = GameType.Chess;
        }

        currentSelectedGame = selectedGame;
        FreeButtonClick();
    }

     public void FreeButtonClick()
     {
         freeButton.image.sprite = onImage;
         freeButton.gameObject.transform.GetChild(0).GetComponent<Text>().color = onColor;
         paidButton.image.sprite = offImage;
         paidButton.gameObject.transform.GetChild(0).GetComponent<Text>().color = offColor;
         isFreeSelected = true;
         isPaidSelected = false;
         GenerateTournamentSecond();
     }

     public void PaidButtonClick()
     {
         freeButton.image.sprite = offImage;
         freeButton.gameObject.transform.GetChild(0).GetComponent<Text>().color = offColor;
         paidButton.image.sprite = onImage;
         paidButton.gameObject.transform.GetChild(0).GetComponent<Text>().color = onColor;
         isPaidSelected = true;
         isFreeSelected = false;
         GenerateTournamentSecond();
     }

    void GenerateTournamentSecond()
    {
        int indexNo = 0;
        for (int j = 0; j < tournamentSecondScrollParentObj.transform.childCount; j++)
        {
            Destroy(tournamentSecondScrollParentObj.transform.GetChild(j).gameObject);
        }
        bool isFirstTour = false;
        if (DataManager.Instance.tournamentData.Count == 0)
        {
            lobbyObj.GetComponent<Text>().text = "No Tournament Available...";
        }
        else
        {
            totalTournament = 0;
            tour_Datas.Clear();
            //lobbyObj.SetActive(false);
            //float betAmountFirst = -10;
            for (int i = 0; i < DataManager.Instance.tournamentData.Count; i++)
            {
                //print("Gene For");
                if (DataManager.Instance.tournamentData[i].modeType != currentSelectedGame ||
                    !DataManager.Instance.tournamentData[i].active) continue;
                if (DataManager.Instance.tournamentData[i].betAmount == 0 && isFreeSelected)
                {
                    lobbyObj.GetComponent<Text>().text = "";
                    totalTournament++;
                    GameObject tourObj = Instantiate(tourSecondPrefab, tournamentSecondScrollParentObj.transform);
                    TourData data = tourObj.GetComponent<TourData>();

                    data.id = DataManager.Instance.tournamentData[i]._id;
                    //data.titleTxt.text = DataManager.Instance.tournamentData[i].name;
                    data.betAmount = DataManager.Instance.tournamentData[i].betAmount * 1;
                    data.totalWinAmount = DataManager.Instance.tournamentData[i].totalWinAmount;
                    data.winningAmount.text = "Rs " + DataManager.Instance.tournamentData[i].winnerRow[0];
                    int index = i;
                    var no1 = indexNo;
                    data.joinBtn.onClick.AddListener(() => Tour_Second_PlayButtonClick(no1));
                    indexNo++;
                    data.PlayerIncrease();
                    data.createDate = DataManager.Instance.tournamentData[i].createdAt;

                    data.interval = DataManager.Instance.tournamentData[i].interval;
                    data.playTime = DataManager.Instance.tournamentData[i].time;
                    data.complexity = DataManager.Instance.tournamentData[i].complexity;
                    data.isBot = DataManager.Instance.tournamentData[i].bot;
                    data.bonusAmountDeduction = DataManager.Instance.tournamentData[i].bonusAmountDeduction;
                    print("Tour Bet Amount : " + DataManager.Instance.tournamentData[i].betAmount);
                    if (currentSelectedGame is GameType.Archery or GameType.Carrom or GameType.Chess)
                    {
                        data.people.text = "2P";
                        data.peopleLimite.text = "1P-2P";
                    }
                    else if (currentSelectedGame == GameType.Slot)
                    {
                        data.people.text = "1P";
                        data.peopleLimite.text = "1P";
                    }
                    else if (currentSelectedGame is GameType.Teen_Patti or GameType.Poker or GameType.Rummy)
                    {
                        data.people.text = "5P";
                        data.peopleLimite.text = "3P-5P";
                    }
                    if (isFirstTour == false)
                    {
                        isFirstTour = true;
                        
                    }
                    print("Bet Amount : " + DataManager.Instance.tournamentData[i].betAmount);
                    if (DataManager.Instance.tournamentID == data.id)
                    {
                        data.JoinButton();
                    }
                    else if (DataManager.Instance.tournamentData[i].betAmount == 0)
                    {
                        data.FreeButtonShow();
                    }
                    else
                    {
                        data.SimpleButton();
                    }
                    tour_Datas.Add(data);
                }
                else if (DataManager.Instance.tournamentData[i].betAmount > 0 && isPaidSelected)
                {
                    lobbyObj.GetComponent<Text>().text = "";
                    totalTournament++;
                    GameObject tourObj = Instantiate(tourSecondPrefab, tournamentSecondScrollParentObj.transform);
                    TourData data = tourObj.GetComponent<TourData>();

                    data.id = DataManager.Instance.tournamentData[i]._id;
                    //data.titleTxt.text = DataManager.Instance.tournamentData[i].name;
                    data.betAmount = DataManager.Instance.tournamentData[i].betAmount * 1;
                    data.totalWinAmount = DataManager.Instance.tournamentData[i].totalWinAmount;
                    data.winningAmount.text = "Rs " + DataManager.Instance.tournamentData[i].winnerRow[0];
                    int index = i;
                    var no1 = indexNo;
                    data.joinBtn.onClick.AddListener(() => Tour_Second_PlayButtonClick(no1));
                    indexNo++;
                    data.PlayerIncrease();
                    data.createDate = DataManager.Instance.tournamentData[i].createdAt;

                    data.interval = DataManager.Instance.tournamentData[i].interval;
                    data.playTime = DataManager.Instance.tournamentData[i].time;
                    data.complexity = DataManager.Instance.tournamentData[i].complexity;
                    data.isBot = DataManager.Instance.tournamentData[i].bot;
                    data.bonusAmountDeduction = DataManager.Instance.tournamentData[i].bonusAmountDeduction;
                    print("Tour Bet Amount : " + DataManager.Instance.tournamentData[i].betAmount);
                    if (currentSelectedGame is GameType.Archery or GameType.Carrom or GameType.Chess)
                    {
                        data.people.text = "2P";
                        data.peopleLimite.text = "1P-2P";
                    }
                    else if (currentSelectedGame == GameType.Slot)
                    {
                        data.people.text = "1P";
                        data.peopleLimite.text = "1P";
                    }
                    else if (currentSelectedGame is GameType.Teen_Patti or GameType.Poker or GameType.Rummy)
                    {
                        data.people.text = "5P";
                        data.peopleLimite.text = "3P-5P";
                    }
                    if (isFirstTour == false)
                    {
                        isFirstTour = true;
                        
                    }
                    print("Bet Amount : " + DataManager.Instance.tournamentData[i].betAmount);
                    if (DataManager.Instance.tournamentID == data.id)
                    {
                        data.JoinButton();
                    }
                    else if (DataManager.Instance.tournamentData[i].betAmount == 0)
                    {
                        data.FreeButtonShow();
                    }
                    else
                    {
                        data.SimpleButton();
                    }
                    tour_Datas.Add(data);
                }
            }

            if (totalTournament == 0)
            {
                lobbyObj.GetComponent<Text>().text = "No Tournament Available...";
            }
        }
    }
    */


    private int totalSeletedTournament;
    private readonly List<TourData> tour_Datas = new();
    private GameType currentSelectedGame;
    private bool isFreeSelected;
    private bool isPaidSelected;

    public void OpenSecondTournament(GameType selectedGame)
    {
        tournamentPanel.SetActive(true);
        totalSeletedTournament = 0;
        tour_Datas.Clear();
        currentSelectedGame = selectedGame;
        isFreeSelected = true;
        isPaidSelected = false;
        UpdateFilterButtons();

        switch (selectedGame)
        {
            case GameType.Slot:
                tournamentTitle.text = "Slot";
                DataManager.Instance.isTwoPlayer = true;
                DataManager.Instance.gameMode = GameType.Slot;
                break;
            case GameType.Poker:
                tournamentTitle.text = "Poker";
                DataManager.Instance.gameMode = GameType.Poker;
                break;
            case GameType.Rummy:
                tournamentTitle.text = "Rummy";
                DataManager.Instance.gameMode = GameType.Rummy;
                break;
            case GameType.Teen_Patti:
                tournamentTitle.text = "TeenPatti";
                DataManager.Instance.gameMode = GameType.Teen_Patti;
                break;
            case GameType.Carrom:
                tournamentTitle.text = "Carrom";
                DataManager.Instance.gameMode = GameType.Carrom;
                break;
            case GameType.Ludo:
                tournamentTitle.text = "Ludo";
                DataManager.Instance.gameMode = GameType.Ludo;
                break;
            case GameType.Archery:
                tournamentTitle.text = "Archery";
                DataManager.Instance.gameMode = GameType.Archery;
                break;
            case GameType.Ball_Pool:
                tournamentTitle.text = "BallPool";
                DataManager.Instance.gameMode = GameType.Ball_Pool;
                break;
            case GameType.Chess:
                tournamentTitle.text = "Chess";
                DataManager.Instance.gameMode = GameType.Chess;
                break;
        }

        GenerateTournamentSecond();
    }

    public void FreeButtonClick()
    {
        isFreeSelected = true;
        isPaidSelected = false;
        UpdateFilterButtons();
        GenerateTournamentSecond();
    }

    public void PaidButtonClick()
    {
        isFreeSelected = false;
        isPaidSelected = true;
        UpdateFilterButtons();
        GenerateTournamentSecond();
    }

    private void UpdateFilterButtons()
    {
        freeButton.image.sprite = isFreeSelected ? onImage : offImage;
        freeButton.gameObject.transform.GetChild(0).GetComponent<Text>().color = isFreeSelected ? onColor : offColor;
        paidButton.image.sprite = isPaidSelected ? onImage : offImage;
        paidButton.gameObject.transform.GetChild(0).GetComponent<Text>().color = isPaidSelected ? onColor : offColor;
    }

    private void GenerateTournamentSecond()
    {
        totalTournament = 0;
        tour_Datas.Clear();
        for (var j = 0; j < tournamentSecondScrollParentObj.transform.childCount; j++)
            Destroy(tournamentSecondScrollParentObj.transform.GetChild(j).gameObject);

        var isFirstTour = true;
        bool hasTournaments = false; // Flag to track if there are any tournaments
        int indexNo = 0; // Declare and initialize indexNo to zero

        foreach (var tournamentData in DataManager.Instance.tournamentData)
        {
            if (tournamentData.modeType != currentSelectedGame || !tournamentData.active)
                continue;

            var isFree = tournamentData.betAmount == 0;

            if ((isFree && !isFreeSelected) || (!isFree && !isPaidSelected))
                continue;
            hasTournaments = true; // Set the flag to true

            totalTournament++;
            var tourObj = Instantiate(tourSecondPrefab, tournamentSecondScrollParentObj.transform);
            var data = tourObj.GetComponent<TourData>();

            data.id = tournamentData._id;
            data.betAmount = tournamentData.betAmount * 1;
            data.totalWinAmount = tournamentData.totalWinAmount;
            data.winningAmount.text = "Rs " + tournamentData.winnerRow[0];
            //data.joinBtn.onClick.AddListener(() => Tour_Second_PlayButtonClick(indexNo++));
            int capturedIndex = indexNo;  // Capture the value of indexNo
            data.joinBtn.onClick.AddListener(() => Tour_Second_PlayButtonClick(capturedIndex));
            indexNo++; // Increment indexNo
            data.PlayerIncrease();
            data.createDate = tournamentData.createdAt;
            data.interval = tournamentData.interval;
            data.playTime = tournamentData.time;
            data.complexity = tournamentData.complexity;
            data.isBot = tournamentData.bot;
            data.bonusAmountDeduction = tournamentData.bonusAmountDeduction;

            if (currentSelectedGame is GameType.Archery or GameType.Carrom or GameType.Chess)
            {
                data.people.text = "2P";
                data.peopleLimite.text = "1P-2P";
            }
            else if (currentSelectedGame == GameType.Slot)
            {
                data.people.text = "1P";
                data.peopleLimite.text = "1P";
            }
            else if (currentSelectedGame is GameType.Teen_Patti or GameType.Poker or GameType.Rummy)
            {
                data.people.text = "5P";
                data.peopleLimite.text = "3P-5P";
            }

            if (isFirstTour)
            {
                isFirstTour = false;
                lobbyObj.GetComponent<Text>().text = string.Empty;
            }

            if (DataManager.Instance.tournamentID == data.id)
                data.JoinButton();
            else if (isFree)
                data.FreeButtonShow();
            else
                data.SimpleButton();

            tour_Datas.Add(data);
        }

        if (!hasTournaments)
        {
            lobbyObj.GetComponent<Text>().text = "No Tournament Available...";
        }
    }


    private void Tour_Second_PlayButtonClick(int index)
    {
        SoundManager.Instance.ButtonClick();

        if (string.IsNullOrEmpty(DataManager.Instance.tournamentID))
        {
            float playerDeposit = float.Parse(DataManager.Instance.playerData.deposit);
            float betAmount = tour_Datas[index].betAmount;

            if (betAmount > playerDeposit)
            {
                GenerateTournamentError();
                return;
            }

            TestSocketIO.Instace.playTime = tour_Datas[index].playTime;
            DataManager.Instance.playerNo = 0;
            DataManager.Instance.diceManageCnt = 0;
            DataManager.Instance.tournamentID = tour_Datas[index].id;
            DataManager.Instance.tourEntryMoney = betAmount;
            DataManager.Instance.winAmount = tour_Datas[index].totalWinAmount * 1;
            DataManager.Instance.tourBonuseCutAmount = tour_Datas[index].bonusAmountDeduction;

            BotManager.Instance.isBotAvalible = tour_Datas[index].isBot;
            int complexity = tour_Datas[index].complexity;

            BotManager.Instance.botType = complexity switch
            {
                1 => BotType.Easy,
                2 => BotType.Medium,
                3 => BotType.Hard,
                _ => BotManager.Instance.botType
            };
            
            StartCoroutine(WaitingForPlayersToJoin());
        }

        tour_Datas[index].ButtonReperesent();
    }

    private IEnumerator WaitingForPlayersToJoin()
    {
        if (DataManager.Instance.gameMode == GameType.Slot)
        {
            TestSocketIO.Instace.SlotJoinRoom();
            yield break;
        }
        waitingPanel.SetActive(true);
        yield return new WaitForSeconds(5f);
        waitingPanel.SetActive(false);
        CheckRequiredPlayers();
        
        
    }

    private void CheckRequiredPlayers()
    {
        if (DataManager.Instance.gameMode is GameType.Archery or GameType.Carrom or GameType.Chess)
        {
            if (DataManager.Instance.joinPlayerDatas.Count is 0 or 1 or 2 && BotManager.Instance.isBotAvalible)
            {
                TestSocketIO.Instace.ArcheryJoinRoom();
            }
            else
            {
                ClearData();
                StartCoroutine(ShowPopup(playerNotFound, 1.5f));
            }
            
        }
        else if (DataManager.Instance.gameMode is GameType.Poker or GameType.Teen_Patti)
        {
            if (DataManager.Instance.joinPlayerDatas.Count is 0 or 1 or 2 or 3 or 4 or 5 && BotManager.Instance.isBotAvalible)
            {
                TestSocketIO.Instace.TeenPattiJoinroom(); 
            }
            else
            {
                ClearData();
                StartCoroutine(ShowPopup(playerNotFound, 1.5f));
            }
        }
        else if (DataManager.Instance.gameMode == GameType.Ludo)
        {
            if (DataManager.Instance.joinPlayerDatas.Count is 0 or 1 or 2 && BotManager.Instance.isBotAvalible)
            {
                TestSocketIO.Instace.LudoJoinroom();
                
            }
            else
            {
                ClearData();
                StartCoroutine(ShowPopup(playerNotFound, 1.5f));
            }
        }
    }

    public void ClearData()
    {
        DataManager.Instance.tournamentID = "";
        DataManager.Instance.tourEntryMoney = 0;
        DataManager.Instance.tourCommision = 0;
        DataManager.Instance.commisionAmount = 0;
        DataManager.Instance.orgIndexPlayer = 0;
        DataManager.Instance.joinPlayerDatas.Clear();
        isPressJoin = false;
        TestSocketIO.Instace.roomid = "";
        TestSocketIO.Instace.userdata = "";
        TestSocketIO.Instace.playTime = 0;
        tournamentPanel.SetActive(false);
    }
    
    public IEnumerator ShowPopup(GameObject gameObject, float duration)
    {
        gameObject.SetActive(true);
        yield return new WaitForSeconds(duration);
        gameObject.SetActive(false);
    }
    
    public void TournamentBackButtonClick()
    {
        SoundManager.Instance.ButtonClick();

        if (DataManager.Instance.joinPlayerDatas.Count == 0)
        {
            Tournament_Back_Main_Leave();
        }
        else
        {
            GenerateLeaveTournament();
        }
        //OpenHome();
    }

    private void Tournament_Back_Main_Leave()
    {
        tournamentPanel.SetActive(false);
    }

    private void GenerateLeaveTournament()
    {
        Instantiate(leaveMainPanel, prefabParent.transform);
    }
    
    public void MainMenuMoneyCut()
    {
        string gameNameSS = DataManager.Instance.gameMode switch
        {
            //cut amount
            GameType.Slot => "Slot",
            GameType.Poker => "Poker",
            GameType.Rummy => "Rummy",
            GameType.Teen_Patti => "Teenpatti",
            GameType.Carrom => "Carrom",
            GameType.Ludo => "Ludo",
            GameType.Archery => "Archery",
            GameType.Ball_Pool => "BallPool",
            GameType.Chess => "Chess",
            _ => ""
        };
        DataManager.Instance.JoinMoneyCut(gameNameSS);
    }
    
    
    public void UserRefund()
    {
        string gameName = DataManager.Instance.gameMode switch
        {
            //cut amount
            GameType.Slot => "Slot",
            GameType.Poker => "Poker",
            GameType.Rummy => "Rummy",
            GameType.Teen_Patti => "Teenpatti",
            GameType.Carrom => "Carrom",
            GameType.Ludo => "Ludo",
            GameType.Archery => "Archery",
            GameType.Ball_Pool => "BallPool",
            GameType.Chess => "Chess",
            _ => ""
        };
        DataManager.Instance.UserRefund(gameName);
    }


    
    #endregion
    
    

    public void LoadNewGame(string gameName)
    {
        StartCoroutine(LoadSceneAsyncCoroutine(gameName));
        TestSocketIO.Instace.SetGameId(DataManager.Instance.tournamentID);
    }
    
    private IEnumerator LoadSceneAsyncCoroutine(string sceneName)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        
        loadingScene.SetActive(true);

        // Allow the scene to load in the background while the game continues to run
        asyncOperation.allowSceneActivation = false;

        // Wait until the scene finishes loading
        while (!asyncOperation.isDone)
        {
            float progress = Mathf.Clamp01(asyncOperation.progress / 0.9f); // Normalize the progress value
            Debug.Log("Loading progress: " + progress * 100f + "%");

            // Check if the loading progress is almost complete
            if (asyncOperation.progress >= 0.9f)
            {
                // Scene is almost loaded, allow activation to complete loading
                asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }
        loadingScene.SetActive(false);
    }
    
    string CheckTournament(GameType modeType)
    {
        List<TournamentData> tournaments = new List<TournamentData>();

        for (int i = 0; i < DataManager.Instance.tournamentData.Count; i++)
        {
            if (DataManager.Instance.tournamentData[i].modeType != modeType) continue;
        
            if (DataManager.Instance.tournamentData[i].players == 2 && 
                DataManager.Instance.tournamentData[i].betAmount == DataManager.Instance.betPrice)
            {
                tournaments.Add(DataManager.Instance.tournamentData[i]);
                tourData = DataManager.Instance.tournamentData[i];
                return DataManager.Instance.tournamentData[i]._id;
            }
        }

        return null;
    }
    
    public void JoinTheGame()
    {
        TestSocketIO.Instace.playTime = tourData.time;
        DataManager.Instance.playerNo = 0;
        DataManager.Instance.diceManageCnt = 0;
        float playerValue = 0;
        DataManager.Instance.tournamentID = tourData._id;
        DataManager.Instance.tourEntryMoney = tourData.betAmount;

        BotManager.Instance.isBotAvalible = tourData.bot;
        print(tourData.bot);
        //print("Cnt Move : " + index + " Complex : " + complex);
        

        switch (DataManager.Instance.gameMode)
        {
            case GameType.Archery:
            case GameType.Carrom:
            case GameType.Chess:
                TestSocketIO.Instace.ArcheryJoinRoom();
                break;
            case GameType.Slot:
                TestSocketIO.Instace.SlotJoinRoom();
                break;
        }

    }


    string IsAvaliableSingleTournament(GameType modeType)
    {
        for (int i = 0; i < DataManager.Instance.tournamentData.Count; i++)
        {
            if (DataManager.Instance.tournamentData[i].modeType == modeType)
            {
                return DataManager.Instance.tournamentData[i]._id;
            }
        }
        return null;
    }


    public void ShopButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        GenerateShop();
    }

    public void SafeButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        print("Balance : " + float.Parse(DataManager.Instance.playerData.balance));
        if (float.Parse(DataManager.Instance.playerData.balance) < 100)
        {
            GenerateWithdrawErrorPanel();
        }
        else
        {
            GenerateWithdraw();
        }
    }

    public void RankButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        GenerateHistroy();
    }

    public void ShareButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        //string shareTxt = "Download Latest StarX apk from Link Here : \n\n" + DataManager.Instance.appUrl + " \n\nUse this referral code :" + DataManager.Instance.playerData.refer_code;

        //new NativeShare().Share(shareTxt);
        GenerateRefferalPanel();
    }

    public void NoticeButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        GenerateNotification();
    }

    public void CoinButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        GenerateShop();
    }




    #endregion

    #region Prefab

    public void GenerateSetting()
    {
        Instantiate(settingPrefab, prefabParent.transform);
    }

    public void GenerateWithdraw()
    {
        Instantiate(withdrawPrefab, prefabParent.transform);
    }

    public void GenerateEditProfile()
    {
        Instantiate(editProfilePrefab, prefabParent.transform);
    }
    public void GenerateShop()
    {
        Instantiate(shopScreenPrefab, prefabParent.transform);
    }

    public void GenerateNotification()
    {
        Instantiate(notificationListPanel, prefabParent.transform);
    }

    public void GenerateContactUs()
    {
        Instantiate(contactUsPrefab, prefabParent.transform);
    }

    public void GenerateTournamentError()
    {
        Instantiate(tourErrorPrefab, prefabParent.transform);
    }
    public void GenerateHistroy()
    {
        Instantiate(tranHistoryPrefab, prefabParent.transform);
    }

    public void GenerateLoadingPanel()
    {
        Instantiate(ludoLoadingPrefab, prefabParent.transform);

    }

    public void GenerateRefferalPanel()
    {
        Instantiate(refferalPrefab, prefabParent.transform);

    }

    public void GenerateWithdrawErrorPanel()
    {
        Instantiate(withdrawErrorPrefab, prefabParent.transform);

    }
    #endregion

    #region API Calling

    #region Profile Data
    public void Getdata()
    {
        StartCoroutine(GetPlayerdata());
    }


    IEnumerator GetPlayerdata()
    {
        UnityWebRequest request = UnityWebRequest.Get(DataManager.Instance.url + "/api/v1/players/profile");
        request.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString("token"));
        yield return request.SendWebRequest();

        if (request.error == null && !request.isNetworkError)
        {
            print("Data:" + request.downloadHandler.text);
            JSONNode values = JSON.Parse(request.downloadHandler.text.ToString());
            JSONNode data = JSON.Parse(values["data"].ToString());
            if (values["success"] == false)
            {
                DataManager.Instance.SetLoginValue("N");
                SceneManager.LoadScene("Splash");
                yield break;
            }
            Setplayerdata(data, true);
        }
        else
        {
            Logger.log.Log(request.error.ToString());
        }

    }

    public void SavePlayerProfile()
    {
        StartCoroutine(Profiledatasave());
    }

    IEnumerator Profiledatasave()
    {
        WWWForm form = new WWWForm();
        form.AddField("firstName", DataManager.Instance.playerData.firstName);
        form.AddField("lastName", DataManager.Instance.playerData.lastName);
        form.AddField("gender", DataManager.Instance.playerData.gender);
        form.AddField("email", DataManager.Instance.playerData.email);
        form.AddField("state", DataManager.Instance.playerData.state);
        form.AddField("panNumber", DataManager.Instance.playerData.panNumber);
        form.AddField("aadharNumber", DataManager.Instance.playerData.aadharNumber);
        form.AddField("dob", DataManager.Instance.playerData.dob);
        form.AddField("country", DataManager.Instance.playerData.country);
        UnityWebRequest request = UnityWebRequest.Post(DataManager.Instance.url + "/api/v1/players/profile", form);
        request.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString("token"));
        //Datamanger.Intance.Avtar = Avtarint;
        yield return request.SendWebRequest();

        if (request.error == null && !request.isNetworkError)
        {
            JSONNode values = JSON.Parse(request.downloadHandler.text.ToString());
            print(request.downloadHandler.text);
            Logger.log.Log("Save Data", values.ToString());

            JSONNode datas = JSON.Parse(values["data"].ToString());
            //Debug.Log("User Data===:::" + datas.ToString());
            Setplayerdata(datas, false);
        }
        else
        {
            Logger.log.Log(request.error.ToString());
        }

    }

    public void Setplayerdata(JSONNode data, bool isGet)
    {
        Debug.Log("User Data===:::" + data.ToString());

        if (isGet)
        {
            if (data[nameof(DataManager.Instance.playerData.balance)] == "")
            {
                data[nameof(DataManager.Instance.playerData.balance)] = "";
            }
            DataManager.Instance.playerData.balance = ((float)data[nameof(DataManager.Instance.playerData.balance)]).ToString("F2");
            DataManager.Instance.playerData.kycStatus = data[nameof(DataManager.Instance.playerData.kycStatus)];
            if (data[nameof(DataManager.Instance.playerData.wonCount)] == "")
            {
                data[nameof(DataManager.Instance.playerData.wonCount)] = "";
            }
            DataManager.Instance.playerData.wonCount = data[nameof(DataManager.Instance.playerData.wonCount)];
            if (data[nameof(DataManager.Instance.playerData.joinCount)] == "")
            {
                data[nameof(DataManager.Instance.playerData.joinCount)] = "";
            }
            DataManager.Instance.playerData.joinCount = data[nameof(DataManager.Instance.playerData.joinCount)];
            DataManager.Instance.playerData.deposit = (data[nameof(DataManager.Instance.playerData.deposit)] * 10).ToString();
            DataManager.Instance.playerData.winings = (data[nameof(DataManager.Instance.playerData.winings)] * 10).ToString();
            DataManager.Instance.playerData.bonus = (data[nameof(DataManager.Instance.playerData.bonus)] * 10).ToString();
            DataManager.Instance.playerData._id = data[nameof(DataManager.Instance.playerData._id)];
            DataManager.Instance.playerData.phone = data[nameof(DataManager.Instance.playerData.phone)];
            DataManager.Instance.playerData.aadharNumber = data[nameof(DataManager.Instance.playerData.aadharNumber)];
            DataManager.Instance.playerData.refer_code = data[nameof(DataManager.Instance.playerData.refer_code)];
            DataManager.Instance.playerData.email = data[nameof(DataManager.Instance.playerData.email)];
            DataManager.Instance.playerData.firstName = data[nameof(DataManager.Instance.playerData.firstName)];
            DataManager.Instance.playerData.lastName = data[nameof(DataManager.Instance.playerData.lastName)];
            DataManager.Instance.playerData.gender = data[nameof(DataManager.Instance.playerData.gender)];
            DataManager.Instance.playerData.state = data[nameof(DataManager.Instance.playerData.state)];
            DataManager.Instance.playerData.createdAt = RemoveQuotes(data[nameof(DataManager.Instance.playerData.createdAt)].ToString());
            DataManager.Instance.playerData.countryCode = data[nameof(DataManager.Instance.playerData.countryCode)];

            string getName = data[nameof(DataManager.Instance.playerData.dob)];
            if (getName == "" || getName == null)
            {
                DataManager.Instance.playerData.dob = "none";
            }
            else
            {
                DataManager.Instance.playerData.dob = RemoveQuotes(data[nameof(DataManager.Instance.playerData.dob)]);
            }
            DataManager.Instance.playerData.panNumber = data[nameof(DataManager.Instance.playerData.panNumber)];
            DataManager.Instance.playerData.membership = "free";
            //DataManager.Instance.playerData.membership = data[nameof(DataManager.Instance.playerData.membership)];
            DataManager.Instance.playerData.avatar = DataManager.Instance.GetAvatarValue();
            DataManager.Instance.playerData.refer_count = data[nameof(DataManager.Instance.playerData.refer_count)];
            DataManager.Instance.playerData.refrer_level = data[nameof(DataManager.Instance.playerData.refrer_level)];
            DataManager.Instance.playerData.refrer_amount_total = data[nameof(DataManager.Instance.playerData.refrer_amount_total)];

            DataManager.Instance.playerData.refer_lvl1_count = data[nameof(DataManager.Instance.playerData.refer_lvl1_count)];
            DataManager.Instance.playerData.refer_vip_count = data[nameof(DataManager.Instance.playerData.refer_vip_count)];
            DataManager.Instance.playerData.refer_deposit_count = data[nameof(DataManager.Instance.playerData.refer_deposit_count)];
        }
        else
        {

            DataManager.Instance.playerData.email = data[nameof(DataManager.Instance.playerData.email)];
            DataManager.Instance.playerData.firstName = data[nameof(DataManager.Instance.playerData.firstName)];
            DataManager.Instance.playerData.lastName = data[nameof(DataManager.Instance.playerData.lastName)];
            DataManager.Instance.playerData.gender = data[nameof(DataManager.Instance.playerData.gender)];
            DataManager.Instance.playerData.panNumber = data[nameof(DataManager.Instance.playerData.panNumber)];
            DataManager.Instance.playerData.state = data[nameof(DataManager.Instance.playerData.state)];

            DataManager.Instance.playerData.aadharNumber = data[nameof(DataManager.Instance.playerData.aadharNumber)];
            DataManager.Instance.playerData.country = data[nameof(DataManager.Instance.playerData.country)];
            DataManager.Instance.playerData.dob = data[nameof(DataManager.Instance.playerData.dob)];
            DataManager.Instance.playerData.avatar = DataManager.Instance.GetAvatarValue();

            Getdata();

        }
        //print("Default Name : " + DataManager.Instance.GetDefaultPlayerName().Length);
        //print("Player Name : " + DataManager.Instance.playerData.firstName);
        coinTxt.text = DataManager.Instance.playerData.balance.ToString();
        secondCoinText.text = coinTxt.text;
        if (DataManager.Instance.GetDefaultPlayerName().IsNullOrEmpty() && DataManager.Instance.playerData.firstName.IsNullOrEmpty())
        {
            print("Sub String : ");
            DataManager.Instance.SetDefaultPlayerName(DataManager.Instance.playerData.phone.Substring(0, 5));
            DataManager.Instance.playerData.firstName = DataManager.Instance.GetDefaultPlayerName();
        }
        else if (DataManager.Instance.playerData.firstName.IsNullOrEmpty())
        {
            DataManager.Instance.playerData.firstName = DataManager.Instance.GetDefaultPlayerName();
        }
        UserUpdateDisplayData();
        //TopBarDataSet();

    }

    #endregion

    /*#region Notification
    public void Getnotification()
    {
        StartCoroutine(GetNotifications());
    }

    IEnumerator GetNotifications()
    {
        UnityWebRequest request = UnityWebRequest.Get(DataManager.Instance.url + "/api/v1/notifications/player");
        request.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString("token"));
        yield return request.SendWebRequest();

        if (request.error == null && !request.isNetworkError)
        {
            JSONNode value = JSON.Parse(request.downloadHandler.text.ToString());
            //print("Update Data : " + value.ToString())
            if (value.Count > 0)
            {
                notificationRedDot.SetActive(true);
            }
            else
            {
                notificationRedDot.SetActive(false);
            }
        }

    }

    #endregion*/




    #region Transaction
    public void GetTran()
    {
        StartCoroutine(GetTrans());

    }

    IEnumerator GetTrans()
    {
        UnityWebRequest request = UnityWebRequest.Get(DataManager.Instance.url + "/api/v1/transactions/player");
        request.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString("token"));
        yield return request.SendWebRequest();
        if (request.error == null && !request.isNetworkError)
        {
            JSONNode keys = JSON.Parse(request.downloadHandler.text.ToString());
            JSONNode data = JSON.Parse(keys["data"].ToString());

            JSONNode value = JSON.Parse(request.downloadHandler.text.ToString());
            if (value.Count > 0)
            {
                //isWithdraw = true;
            }
            else
            {
                isWithdraw = false;
            }

            for (int i = 0; i < value["data"].Count; i++)
            {

                //byte[] st = Encoding.ASCII.GetBytes(value["data"][i]["title"].ToString().Trim('"'));
                //string data = Encoding.UTF8.GetString(st);



                string paymentStatus = value["data"][i]["paymentStatus"];
                string logType = value["data"][i]["logType"];
                //string _id = value["data"][i]["_id"];
                //string amount = value["data"][i]["amount"];
                //string transactionType = value["data"][i]["transactionType"];
                //string note = value["data"][i]["note"];
                //string createdAt = value["data"][i]["createdAt"];



                if (logType == "withdraw" && paymentStatus == "PROCESSING")
                {
                    isWithdraw = true;
                }

            }
        }

        #endregion
    }

    #endregion

    #region Common

    public void UserUpdateDisplayData()
    {

        if (DataManager.Instance.playerData.firstName.IsNullOrEmpty())
        {
            userNameTxt.text = DataManager.Instance.GetDefaultPlayerName();
        }
        else
        {
            userNameTxt.text = DataManager.Instance.playerData.firstName;
        }
        if (DataManager.Instance.playerData.email.Length > 14)
        {
            userIdTxt.text = DataManager.Instance.playerData.email.Substring(0, 14) + "...";
        }
        else
        {
            userIdTxt.text = DataManager.Instance.playerData.email;
        }

        StartCoroutine(DataManager.Instance.GetImages(PlayerPrefs.GetString("ProfileURL"), avatarImg));
    }

    public string RemoveQuotes(string s)
    {
        string str = s;
        string newstr = str.Replace("\"", "");
        return newstr;
    }
    #endregion

    #region Archery bot

    public void OpenLoadScreen()
    {
        botPlayers = 2 - DataManager.Instance.joinPlayerDatas.Count;
        if (DataManager.Instance.joinPlayerDatas.Count <= minPlayerRequired)
        {
            int[] avatars = Enumerable.Range(0, BotManager.Instance.botUser_Profile_URL.Count).ToArray();
            avatars.Shuffle();
            int[] randomAvatars = avatars.Take(botPlayers).ToArray();
        
            int[] names = Enumerable.Range(0, BotManager.Instance.botUserName.Count).ToArray();
            names.Shuffle();
            int[] randomNames = names.Take(botPlayers).ToArray();
            
            for (int i = 0; i < botPlayers; i++)
            {
                string avatar =
                    BotManager.Instance.botUser_Profile_URL[randomAvatars[i]];
                string botUserName =
                    BotManager.Instance.botUserName[randomNames[i]];
                string userId = DataManager.Instance.joinPlayerDatas[i].userId
                    .Substring(0, DataManager.Instance.joinPlayerDatas[i].userId.Length - 1) + "Archery";
                DataManager.Instance.AddRoomUser(userId, botUserName,
                    DataManager.Instance.joinPlayerDatas[i].lobbyId,
                    10.ToString(), i , avatar);
            }
        }

        BotManager.Instance.isConnectBot = true;
    }

    #endregion
    
    #region Load Bot

    public void CheckPlayers()
    {
        botPlayers = minPlayerRequired - DataManager.Instance.joinPlayerDatas.Count;
        if (DataManager.Instance.joinPlayerDatas.Count <= minPlayerRequired)
        {
            int[] avatars = Enumerable.Range(0, BotManager.Instance.botUser_Profile_URL.Count).ToArray();
            avatars.Shuffle();
            int[] randomAvatars = avatars.Take(botPlayers).ToArray();
        
            int[] names = Enumerable.Range(0, BotManager.Instance.botUserName.Count).ToArray();
            names.Shuffle();
            int[] randomNames = names.Take(botPlayers).ToArray();
            
            for (int i = 0; i < botPlayers; i++)
            {
                string avatar =
                    BotManager.Instance.botUser_Profile_URL[randomAvatars[i]];
                string botUserName =
                    BotManager.Instance.botUserName[randomNames[i]];
                string userId = DataManager.Instance.joinPlayerDatas[i].userId
                    .Substring(0, DataManager.Instance.joinPlayerDatas[i].userId.Length - 1) + "TeenPatti";
                DataManager.Instance.AddRoomUser(userId, botUserName,
                    DataManager.Instance.joinPlayerDatas[i].lobbyId,
                    10.ToString(), i , avatar);
            }
        }
    }
    

    #endregion
    

    #region Ludo Other Maintain


    public void OpenTournamentLoadScreen(Text t)
    {
       DataManager.Instance.SetPlayedGame(DataManager.Instance.GetPlayedGame() + 1);
        if (DataManager.Instance.isTwoPlayer)
        {
            print("Data Manager Join Player Count : " + DataManager.Instance.joinPlayerDatas.Count);
            if (DataManager.Instance.joinPlayerDatas.Count == 2)
            {
                StartCoroutine(LoadScene());
                //GenerateLoadingPanel();
            }
            else if (DataManager.Instance.joinPlayerDatas.Count == 1 &&
                     BotManager.Instance.isBotAvalible) // && DataManager.Instance.gameType!="Game")
            {
                //print("Enter The Bot Connect");
                //print("Enter The Condition");
                int playerNo = 3;


                string avatar =
                    BotManager.Instance.botUser_Profile_URL[
                        UnityEngine.Random.Range(0, BotManager.Instance.botUser_Profile_URL.Count)];
                string botUserName =
                    BotManager.Instance.botUserName[UnityEngine.Random.Range(0, BotManager.Instance.botUserName.Count)];
                string userId = DataManager.Instance.joinPlayerDatas[0].userId
                    .Substring(0, DataManager.Instance.joinPlayerDatas[0].userId.Length - 1) + "Ludo";
                DataManager.Instance.AddRoomUser(userId, botUserName, DataManager.Instance.joinPlayerDatas[0].lobbyId,
                    10.ToString(), playerNo, avatar);


                BotManager.Instance.isConnectBot = true;
                int rnoInd = UnityEngine.Random.Range(0, 2);
                //int rnoInd = 0;
                print("rnoInd : " + rnoInd);
                if (rnoInd == 0)
                {
                    DataManager.Instance.playerNo = 3;

                    JoinPlayerData joinplayerData1 = DataManager.Instance.joinPlayerDatas[0];
                    JoinPlayerData joinplayerData2 = DataManager.Instance.joinPlayerDatas[1];


                    string userId1 = joinplayerData1.userId;
                    string userName1 = joinplayerData1.userName;
                    string balance1 = joinplayerData1.balance;
                    string avtar1 = joinplayerData1.avtar;
                    string pPic = joinplayerData1.pPicture;
                    //print("Join Player Data 1 : " + joinplayerData1.userName);
                    //print("Join Player Data 2 : " + joinplayerData2.userName);
                    DataManager.Instance.joinPlayerDatas[0].userId = joinplayerData2.userId;
                    DataManager.Instance.joinPlayerDatas[0].userName = joinplayerData2.userName;
                    DataManager.Instance.joinPlayerDatas[0].balance = joinplayerData2.balance;
                    DataManager.Instance.joinPlayerDatas[0].playerNo = 1;
                    DataManager.Instance.joinPlayerDatas[0].avtar = joinplayerData2.avtar;
                    DataManager.Instance.joinPlayerDatas[0].pPicture = joinplayerData2.pPicture;

                    DataManager.Instance.joinPlayerDatas[1].userId = userId1;
                    DataManager.Instance.joinPlayerDatas[1].userName = userName1;
                    DataManager.Instance.joinPlayerDatas[1].balance = balance1;
                    DataManager.Instance.joinPlayerDatas[1].playerNo = 3;
                    DataManager.Instance.joinPlayerDatas[1].avtar = avtar1;
                    DataManager.Instance.joinPlayerDatas[1].pPicture = pPic;
                    BotManager.Instance.isConnectBot = true;
                    StartCoroutine(LoadScene());
                    //GenerateLoadingPanel();
                }
                else
                {
                    BotManager.Instance.isConnectBot = true;
                    //GenerateLoadingPanel();
                    StartCoroutine(LoadScene());
                }
            }
            else if (DataManager.Instance.joinPlayerDatas.Count == 1)
            {
                DataManager.Instance.tournamentID = "";
                DataManager.Instance.tourEntryMoney = 0;
                DataManager.Instance.tourCommision = 0;
                DataManager.Instance.commisionAmount = 0;
                DataManager.Instance.orgIndexPlayer = 0;
                DataManager.Instance.joinPlayerDatas.Clear();
                isPressJoin = false;

                t.text = "JOIN";
                TestSocketIO.Instace.roomid = "";
                TestSocketIO.Instace.userdata = "";
                TestSocketIO.Instace.playTime = 0;
                TestSocketIO.Instace.LeaveRoom();
                GenerateTournamentError();
            }

            //if (DataManager.Instance.joinPlayerDatas.Count == 2)
            //{
            //    GenerateLoadingPanel();
            //}
            //else if(DataManager.Instance.joinPlayerDatas.Count==1)
            //{
            //    DataManager.Instance.tournamentID = "";
            //    DataManager.Instance.tourEntryMoney = 0;
            //    DataManager.Instance.tourCommision = 0;
            //    DataManager.Instance.commisionAmount = 0;
            //    DataManager.Instance.orgIndexPlayer = 0;
            //    DataManager.Instance.joinPlayerDatas.Clear();
            //    t.text = "JOIN";
            //    isPressJoin = false;
            //    TestSocketIO.Instace.roomid = "";
            //    TestSocketIO.Instace.userdata = "";
            //    TestSocketIO.Instace.playTime = 0;
            //    Instantiate(tournamentErrorObj, parentObj.transform);

            //}
            //else if(DataManager.Instance.joinPlayerDatas.Count == 1)
            //{

            //}
        }
        else if (DataManager.Instance.isFourPlayer)
        {
            int maxPlayer = 4;
            int playerRequired = maxPlayer - DataManager.Instance.joinPlayerDatas.Count;
            print("Data Manager Join Player Count : " + DataManager.Instance.joinPlayerDatas.Count);
            
            if (DataManager.Instance.joinPlayerDatas.Count == 4)
            { 
                StartCoroutine(LoadScene());
               //GenerateLoadingPanel();
            }
            else if (DataManager.Instance.joinPlayerDatas.Count is 1 or 2 or 3 &&
                     BotManager.Instance.isBotAvalible) // && DataManager.Instance.gameType!="Game")
            {
                //print("Enter The Bot Connect");
                //print("Enter The Condition");
                // for assigning player number
                DataManager.Instance.playerNo = DataManager.Instance.joinPlayerDatas.Count switch
                {
                    2 => 3,
                    3 => 2,
                    1 => 1,
                    _ => DataManager.Instance.playerNo
                };

                for (int i = 0; i < playerRequired; i++)
                {
                    int playerNo = i + 2;
                    string avatar =
                        BotManager.Instance.botUser_Profile_URL[
                            UnityEngine.Random.Range(0, BotManager.Instance.botUser_Profile_URL.Count)];
                    string botUserName =
                        BotManager.Instance.botUserName[
                            UnityEngine.Random.Range(0, BotManager.Instance.botUserName.Count)];
                    string userId = DataManager.Instance.joinPlayerDatas[i].userId
                        .Substring(0, DataManager.Instance.joinPlayerDatas[i].userId.Length - 1) + "Ludo";
                    DataManager.Instance.AddRoomUser(userId, botUserName,
                        DataManager.Instance.joinPlayerDatas[i].lobbyId,
                        10.ToString(), playerNo, avatar);
                
                }
                

                BotManager.Instance.isConnectBot = true;
                //int rnoInd = UnityEngine.Random.Range(0, 2);
                int rnoInd = 0;
                print("rnoInd : " + rnoInd);
                if (rnoInd == 0)
                {
                    print("This is the assigned player number -> " + DataManager.Instance.playerNo);

                    JoinPlayerData joinplayerData1 = DataManager.Instance.joinPlayerDatas[0];
                    JoinPlayerData joinplayerData2 = DataManager.Instance.joinPlayerDatas[1];
                    JoinPlayerData joinplayerData3 = DataManager.Instance.joinPlayerDatas[2];
                    JoinPlayerData joinplayerData4 = DataManager.Instance.joinPlayerDatas[3];

                    string userId1 = joinplayerData1.userId;
                    string userName1 = joinplayerData1.userName;
                    string balance1 = joinplayerData1.balance;
                    string avtar1 = joinplayerData1.avtar;
                    //print("Join Player Data 1 : " + joinplayerData1.userName);
                    //print("Join Player Data 2 : " + joinplayerData2.userName);
                    DataManager.Instance.joinPlayerDatas[0].userId = userId1;
                    DataManager.Instance.joinPlayerDatas[0].userName = userName1;
                    DataManager.Instance.joinPlayerDatas[0].balance = balance1;
                    DataManager.Instance.joinPlayerDatas[0].playerNo = 1;
                    DataManager.Instance.joinPlayerDatas[0].avtar = avtar1;

                    DataManager.Instance.joinPlayerDatas[1].userId = joinplayerData2.userId;
                    DataManager.Instance.joinPlayerDatas[1].userName = joinplayerData2.userName;
                    DataManager.Instance.joinPlayerDatas[1].balance = joinplayerData2.balance;
                    DataManager.Instance.joinPlayerDatas[1].playerNo = 2;
                    DataManager.Instance.joinPlayerDatas[1].avtar = joinplayerData2.avtar;

                    DataManager.Instance.joinPlayerDatas[2].userId = joinplayerData3.userId;
                    DataManager.Instance.joinPlayerDatas[2].userName = joinplayerData3.userName;
                    DataManager.Instance.joinPlayerDatas[2].balance = joinplayerData3.balance;
                    DataManager.Instance.joinPlayerDatas[2].playerNo = 3;
                    DataManager.Instance.joinPlayerDatas[2].avtar = joinplayerData3.avtar;

                    DataManager.Instance.joinPlayerDatas[3].userId = joinplayerData4.userId;
                    DataManager.Instance.joinPlayerDatas[3].userName = joinplayerData4.userName;
                    DataManager.Instance.joinPlayerDatas[3].balance = joinplayerData4.balance;
                    DataManager.Instance.joinPlayerDatas[3].playerNo = 4;
                    DataManager.Instance.joinPlayerDatas[3].avtar = joinplayerData4.avtar;
                    BotManager.Instance.isConnectBot = true;
                    StartCoroutine(LoadScene());
                    //GenerateLoadingPanel();
                }
                else
                {
                    BotManager.Instance.isConnectBot = true;
                    StartCoroutine(LoadScene());
                    //GenerateLoadingPanel();
                }
            }
            else if (DataManager.Instance.joinPlayerDatas.Count == 1)
            {
                DataManager.Instance.tournamentID = "";
                DataManager.Instance.tourEntryMoney = 0;
                DataManager.Instance.tourCommision = 0;
                DataManager.Instance.commisionAmount = 0;
                DataManager.Instance.orgIndexPlayer = 0;
                DataManager.Instance.joinPlayerDatas.Clear();
                isPressJoin = false;

                t.text = "JOIN";
                TestSocketIO.Instace.roomid = "";
                TestSocketIO.Instace.userdata = "";
                TestSocketIO.Instace.playTime = 0;
                TestSocketIO.Instace.LeaveRoom();
                GenerateTournamentError();
            }
        }
    }

    public IEnumerator LoadScene()
    {

        yield return null;

        //Begin to load the Scene you specify
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("Ludo");

        asyncOperation.allowSceneActivation = false;
        while (!asyncOperation.isDone)
        {
            if (asyncOperation.progress >= 0.9f)
            {
                //Destroy(obj);
                Screen.orientation = ScreenOrientation.Portrait;
                asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }
    }



    #endregion

    #region Filter

    public void FilterOn(int no)
    {

        SoundManager.Instance.ButtonClick();
        switch (no)
        {
            case 1:
                m_AllOn.SetActive(true);
                m_AllOff.SetActive(false);
                m_MultiplayerOff.SetActive(true);
                m_MultiplayerOn.SetActive(false);
                m_SkillsOn.SetActive(false);
                m_SkillsOff.SetActive(true);
                m_SlotsOn.SetActive(false);
                m_SlottsOff.SetActive(true);
                m_CardsOn.SetActive(false);
                m_CardsOff.SetActive(true);
                g_8BallPool.SetActive(true);
                g_Archery.SetActive(true);
                g_Carrom.SetActive(true);
                g_Chess.SetActive(true);
                g_Ludo.SetActive(true);
                g_Poker.SetActive(true);
                g_Rummy.SetActive(true);
                g_Slots.SetActive(true);
                g_TeenPatti.SetActive(true);
                break;
            case 2:
                m_AllOn.SetActive(false);
                m_AllOff.SetActive(true);
                m_MultiplayerOff.SetActive(false);
                m_MultiplayerOn.SetActive(true);
                m_SkillsOn.SetActive(false);
                m_SkillsOff.SetActive(true);
                m_SlotsOn.SetActive(false);
                m_SlottsOff.SetActive(true);
                m_CardsOn.SetActive(false);
                m_CardsOff.SetActive(true);
                g_8BallPool.SetActive(true);
                g_Archery.SetActive(false);
                g_Carrom.SetActive(true);
                g_Chess.SetActive(true);
                g_Ludo.SetActive(true);
                g_Poker.SetActive(true);
                g_Rummy.SetActive(true);
                g_Slots.SetActive(true);
                g_TeenPatti.SetActive(true);
                break;
            case 3:
                m_AllOn.SetActive(false);
                m_AllOff.SetActive(true);
                m_MultiplayerOff.SetActive(true);
                m_MultiplayerOn.SetActive(false);
                m_SkillsOn.SetActive(true);
                m_SkillsOff.SetActive(false);
                m_SlotsOn.SetActive(false);
                m_SlottsOff.SetActive(true);
                m_CardsOn.SetActive(false);
                m_CardsOff.SetActive(true);
                g_8BallPool.SetActive(true);
                g_Archery.SetActive(true);
                g_Carrom.SetActive(true);
                g_Chess.SetActive(true);
                g_Ludo.SetActive(true);
                g_Poker.SetActive(false);
                g_Rummy.SetActive(false);
                g_Slots.SetActive(true);
                g_TeenPatti.SetActive(false);
                break;
            case 4:
                m_AllOn.SetActive(false);
                m_AllOff.SetActive(true);
                m_MultiplayerOff.SetActive(true);
                m_MultiplayerOn.SetActive(false);
                m_SkillsOn.SetActive(false);
                m_SkillsOff.SetActive(true);
                m_SlotsOn.SetActive(true);
                m_SlottsOff.SetActive(false);
                m_CardsOn.SetActive(false);
                m_CardsOff.SetActive(true);
                g_8BallPool.SetActive(false);
                g_Archery.SetActive(false);
                g_Carrom.SetActive(false);
                g_Chess.SetActive(false);
                g_Ludo.SetActive(false);
                g_Poker.SetActive(false);
                g_Rummy.SetActive(false);
                g_Slots.SetActive(true);
                g_TeenPatti.SetActive(false);
                break;
            case 5:
                m_AllOn.SetActive(false);
                m_AllOff.SetActive(true);
                m_MultiplayerOff.SetActive(true);
                m_MultiplayerOn.SetActive(false);
                m_SkillsOn.SetActive(false);
                m_SkillsOff.SetActive(true);
                m_SlotsOn.SetActive(false);
                m_SlottsOff.SetActive(true);
                m_CardsOn.SetActive(true);
                m_CardsOff.SetActive(false);
                g_8BallPool.SetActive(false);
                g_Archery.SetActive(false);
                g_Carrom.SetActive(false);
                g_Chess.SetActive(false);
                g_Ludo.SetActive(false);
                g_Poker.SetActive(true);
                g_Rummy.SetActive(true);
                g_Slots.SetActive(false);
                g_TeenPatti.SetActive(true);
                break;
                
        }
    }

    #endregion

}
public enum TournamentType
{
    Free,
    Paid
}
[System.Serializable]
public class TournamentData
{
    public bool bot;
    public float bonusAmountDeduction;
    public bool active;
    public string _id;
    public string name;
    public GameType modeType;
    public float betAmount;
    public float minBet;
    public float maxBet;
    public float maxPayout;
    public float challLimit;
    public float potLimit;
    public int players;
    public int winner;
    public List<string> winnerRow = new List<string>();
    public float totalWinAmount;
    public float time;
    public int complexity;
    public int interval;
    public string _v;
    public string createdAt;
    public string updatedAt;
}
