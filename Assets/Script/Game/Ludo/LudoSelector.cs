using System;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class LudoSelector : MonoBehaviour
{
    public static LudoSelector Instance;
    public TournamentData tourData;
    public Image twoPlayerBtn;
    public Image fourPlayerBtn;
    //public Image twoPlayerLImg;
    //public Image fourPlayerLImg;
    public Text errorTxt;
    public int numberOfPlayers;

    //public Sprite onBtn;
    //public Sprite offBtn;

    public Sprite twoLImgOn;
    public Sprite twoLImgOff;
    public Sprite fourLImgOn;
    public Sprite fourLImgOff;

    public bool isTwoPlayerSelected;
    public bool isFourPlayerSelected;

    public Text joined;
    
    public Text priceTxt;
    public float tableMaxLimit;
    public float tableMinLimit;
    public float incrementNo;
    public Text betPriceTxt;
    public Button betPlusButton;
    public Button betMinusButton;
    private int[] numbers = { 5, 10, 50, 100, 250, 500, 1000 };
    private int currentIndex = 0;
    
    [Header("--- WaitingScene ---")]
    public Text timeTxt;
    public float secondsCount;
    public bool isTourEnter;
    public bool isClicked;

    public GameObject waitingScene;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        MainMenuManager.Instance.screenObj.Add(this.gameObject);
        waitingScene.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        //incrementNo = tableMaxLimit / tableMinLimit;
        priceTxt.text = "5";

        betPriceTxt.text = tableMinLimit.ToString();
        betPlusButton.interactable = true;
        betMinusButton.interactable = false;
    }

    // Update is called once per frame
    void Update()
    {
        ButtonSpriteChanger();

        if (isClicked)
        {
            Timer();
        }
    }

    private void Timer()
    {
        if (secondsCount > 0)
        {
            secondsCount -= Time.deltaTime;
            float seconds = secondsCount % 60;
            if (seconds.ToString("0").Length == 1)
            {
                timeTxt.text = "Starting....." + "0" + seconds.ToString("0");
            }
            else
            {
                timeTxt.text = "Starting....." + seconds.ToString("0");
            }
        }
        else if (isTourEnter == false)
        {
            isTourEnter = true;
            OpenAPlayMode();
        }

        // if (DataManager.Instance.isTwoPlayer && DataManager.Instance.joinPlayerDatas.Count == 2)
        // {
        //     OpenAPlayMode();
        // }
        // else if(DataManager.Instance.isFourPlayer && DataManager.Instance.joinPlayerDatas.Count == 4)
        // {
        //     OpenAPlayMode();
        // }
    }
    
    public void CloseSetting()
    {
        MainMenuManager.Instance.screenObj.Remove(this.gameObject);
        Destroy(this.gameObject);
    }

    public void CloseSettingButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        CloseSetting();
    }

    private void ButtonSpriteChanger()
    {
        if (!isTwoPlayerSelected)
        {
            //twoPlayerBtn.sprite = offBtn;
            twoPlayerBtn.sprite = twoLImgOff;
        }
        else
        {
            //twoPlayerBtn.sprite = onBtn;
            twoPlayerBtn.sprite = twoLImgOn;
        }
        if (!isFourPlayerSelected)
        {
            //fourPlayerBtn.sprite = offBtn;
            fourPlayerBtn.sprite = fourLImgOff;
        }
        else
        {
            //fourPlayerBtn.sprite = onBtn;
            fourPlayerBtn.sprite = fourLImgOn;
        }
    }
    
    public void Ludo_Game_Selector(int no)
    {
        SoundManager.Instance.ButtonClick();
        switch (no)
        {
            case 1:
                //TwoPlayer
                DataManager.Instance.modeType = 1;
                numberOfPlayers = 2;
                isTwoPlayerSelected = true;
                DataManager.Instance.isTwoPlayer = true;
                isFourPlayerSelected = false;
                DataManager.Instance.isFourPlayer = false;
               // PlayButtonClicked();
                
                break;

            case 2:
                //FourPlayer
                DataManager.Instance.modeType = 1;
                numberOfPlayers = 4;
                isFourPlayerSelected = true;
                DataManager.Instance.isFourPlayer = true;
                isTwoPlayerSelected = false;
                DataManager.Instance.isTwoPlayer = false;
                //PlayButtonClicked();
                
                break;
        }
    }
    

    public void PlayButtonClicked()
    {
        float price = float.Parse(priceTxt.text);
        DataManager.Instance.betPrice = price;
        SoundManager.Instance.ButtonClick();
        if (!isTwoPlayerSelected && !isFourPlayerSelected)
        {
            errorTxt.text = "Please Select Game Mode";
            Invoke(nameof(ErrorMsg), 1f);
            print("Please select game");
        }
        else
        {
            if (CheckMoney(price) == false)
            {
                errorTxt.text = "Dont have Enough balance";
                Invoke(nameof(ErrorMsg), 1f);
                return;
            }
            string getTour = IsAvaliableSingleTournament(GameType.Ludo);
            DataManager.Instance.gameMode = GameType.Ludo;
            //Instantiate(MainMenuManager.Instance.tournamentLudoPrefab, MainMenuManager.Instance.prefabParent.transform);
            //TournamentPanel.Instance.gameType = GameType.Ludo;
            //TournamentPanel.Instance.GenerateTournament();
            if (getTour != null && getTour.Length != 0)
            {
                DataManager.Instance.tournamentID = getTour;
                JoinTheGame();
                //MainMenuManager.Instance.OpenTournamentLoadScreen(joined);
                //StartCoroutine(LoadScene());
                waitingScene.gameObject.SetActive(true);
                isClicked = true;
            }
            else
            {
                MainMenuManager.Instance.GenerateTournamentError();
            }
            MainMenuManager.Instance.screenObj.Remove(this.gameObject);
        }
        
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

    public void OpenAPlayMode()
    {
        MainMenuManager.Instance.OpenTournamentLoadScreen(joined);
        TestSocketIO.Instace.SetGameId(DataManager.Instance.tournamentID);
    }
    
    
    string IsAvaliableSingleTournament(GameType modeType)
    {
         List<TournamentData> tournaments = new List<TournamentData>();

        for (int i = 0; i < DataManager.Instance.tournamentData.Count; i++)
        {
            if (DataManager.Instance.tournamentData[i].modeType != modeType) continue;
            switch (DataManager.Instance.tournamentData[i].players)
            {
                case 2 when isTwoPlayerSelected && DataManager.Instance.tournamentData[i].betAmount == DataManager.Instance.betPrice:
                    tournaments.Add(DataManager.Instance.tournamentData[i]);
                    tourData = DataManager.Instance.tournamentData[i];
                    return DataManager.Instance.tournamentData[i]._id;
                case 4 when isFourPlayerSelected && DataManager.Instance.tournamentData[i].betAmount == DataManager.Instance.betPrice:
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
        
        float betAmount = float.Parse(priceTxt.text);
        if (isTwoPlayerSelected)
        {
            /*playerValue = betAmount * 2;
            float winAmount = playerValue;
            float adminCommssion = (DataManager.Instance.adminPercentage / 100);
            float playerWinAmount = winAmount - (winAmount * adminCommssion);
            DataManager.Instance.winAmount = playerWinAmount;*/
            
            playerValue = betAmount * 2;
            float winReward = playerValue - betAmount;
            float adminCommission = DataManager.Instance.adminPercentage / 100;
            float winAmount = winReward - (winReward * adminCommission);
            float playerWinAmount = betAmount + winAmount;
            DataManager.Instance.winAmount = playerWinAmount;

        }
        else
        {
            /*playerValue = betAmount * 4;
            float winAmount = playerValue;
            float adminCommssion = (DataManager.Instance.adminPercentage / 100);
            float playerWinAmount = winAmount - (winAmount * adminCommssion);
            DataManager.Instance.winAmount = playerWinAmount;*/
            
            playerValue = betAmount * 4;
            float winReward = playerValue - betAmount;
            float adminCommission = DataManager.Instance.adminPercentage / 100;
            float winAmount = winReward - (winReward * adminCommission);
            float playerWinAmount = betAmount + winAmount;
            DataManager.Instance.winAmount = playerWinAmount;
        }

        //DataManager.Instance.winAmount = tourData.totalWinAmount * 10;
        
        DataManager.Instance.tournamentID = tourData._id;
        DataManager.Instance.tourEntryMoney = tourData.betAmount * 10;

        BotManager.Instance.isBotAvalible = tourData.bot;
        print(tourData.bot);
        int complex = tourData.complexity;
        //print("Cnt Move : " + index + " Complex : " + complex);

        if (complex == 1)
        {

            BotManager.Instance.botType = BotType.Easy;
        }
        else if (complex == 2)
        {
            BotManager.Instance.botType = BotType.Medium;
        }
        else if (complex == 3)
        {
            BotManager.Instance.botType = BotType.Hard;
        }

        TestSocketIO.Instace.LudoJoinroom();
        //TestSocketIO.Instace.SetGameId(DataManager.Instance.tournamentID);
        //SoundManager.Instance.ButtonClick();
    }
    
    public void Plus_ButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        float price = float.Parse(priceTxt.text);
        if (currentIndex < numbers.Length - 1)
        {
            currentIndex++;
            priceTxt.text = numbers[currentIndex].ToString();
            if (currentIndex == numbers.Length - 1)
            {
                betPlusButton.interactable = false;
                betMinusButton.interactable = true;
            }
            else if (currentIndex == 0)
            {
                betPlusButton.interactable = true;
                betMinusButton.interactable = false;
            }
            else
            {
                betPlusButton.interactable = true;
                betMinusButton.interactable = true;
            }
        }
    }

    public void Minus_ButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        float price = float.Parse(priceTxt.text);
        if (currentIndex > 0)
        {
            currentIndex--;
            priceTxt.text = numbers[currentIndex].ToString();
            if (currentIndex == numbers.Length - 1)
            {
                betPlusButton.interactable = false;
                betMinusButton.interactable = true;
            }
            else if (currentIndex == 0)
            {
                betPlusButton.interactable = true;
                betMinusButton.interactable = false;
            }
            else
            {
                betPlusButton.interactable = true;
                betMinusButton.interactable = true;
            }
        }
    }
    


    private void ErrorMsg()
    {
        errorTxt.text = "";
    }
    
}
