using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LudoTourBox : MonoBehaviour
{
    public GameType gameType;
    public TournamentData tourData;
    public Text betAmountTxt;
    public Text winAmountTxt;
    public Text timeTxt;
    public Text joinPlayerTxt;
    public Button joinBtn;
    public float time;
    public string tournamentID;
    public int flag = 0;
    public string createDate;
    public int interval = 0;
    public float secondsCount;
    public float betAmount;
    public float winAmount;

    public bool isBotAvliablity;
    // Start is called before the first frame update
    void Start()
    {
        //TestSocketIO.Instace.SetLobbyCount(tournamentID);
        GetDiffMinute();
    }

    // Update is called once per frame
    void Update()
    {
        Timer();
    }

    void JoinButtonClick()
    {
        //print("Join Button Click : " + isPressJoin);
        //print("Join Button Tournament Id : " + tournamentData[index]._id);
        //print("Join Button Index : " + index);




    }

    public void UpdateDisplay()
    {
        betAmountTxt.text = betAmount.ToString();
        winAmountTxt.text = winAmount.ToString();
    }

    public void PlayNowButtonClick()
    {

        //DataManager.Instance.gameMode = gameType;
        //DataManager.Instance.chaalLimit = chaalLimit;
        //DataManager.Instance.potLimit = potLimit;
        DataManager.Instance.tournamentID = tournamentID;
        //TestSocketIO.Instace.LudoJoinroom();


        float betAmount = tourData.betAmount;


        Text t = joinBtn.transform.GetChild(0).GetComponent<Text>();
        if (t.text == "JOINED")
        {
            return;
        }
        joinBtn.transform.GetChild(0).GetComponent<Text>().text = "JOINED";
        TestSocketIO.Instace.playTime = tourData.time;
        DataManager.Instance.playerNo = 0;
        DataManager.Instance.diceManageCnt = 0;
        DataManager.Instance.winAmount = tourData.totalWinAmount * 10;
        DataManager.Instance.tournamentID = tourData._id;
        DataManager.Instance.tourEntryMoney = tourData.betAmount * 10;

        BotManager.Instance.isBotAvalible = tourData.bot;
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
        SoundManager.Instance.ButtonClick();



        //
    }


    void Timer()
    {
        if (flag == 0)
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

            string timeValue = Min + " min " + Sec + "s";
            if (timeValue.Equals("00 min 00s"))
            {
                print("Time Over");
                if (joinBtn.transform.GetChild(0).GetComponent<Text>().text == "JOINED")
                {
                    MainMenuManager.Instance.OpenTournamentLoadScreen(joinBtn.transform.GetChild(0).GetComponent<Text>());
                    // MainMenuManager.Instance.OpenTournamentLoadScreen();
                }
                timeTxt.text = "00 min 00s";
                flag = 1;
            }
            if (flag != 1)
            {
                timeTxt.text = timeValue;
            }
        }
        else
        {
            GetDiffMinute();
        }
    }

    public void GetDiffMinute()
    {
        flag = 0;
        //int createHour = int.Parse(createDate.Split("T")[1].Split(":")[0]);
        int createHour = 0;
        int createMinute = int.Parse(createDate.Split("T")[1].Split(":")[1]);
        int createSecond = int.Parse(createDate.Split("T")[1].Split(":")[2].Split(".")[0]);

        DateTime date = DateTime.Now;
        string curDate = date.ToString();
        int currHour = int.Parse(curDate.Split(" ")[1].Split(":")[0]);
        int currMinute = int.Parse(curDate.Split(" ")[1].Split(":")[1]);
        int currSecond = int.Parse(curDate.Split(" ")[1].Split(":")[2]);

        //print("Current Hour : " + currHour);
        //print("Current Minute : " + currMinute);
        //print("Current Second : " + currSecond);

        DateTime dateTime1 = DateTime.Parse(createHour + ":" + createMinute + ":" + createSecond);
        DateTime dateTime2 = DateTime.Parse(currHour + ":" + currMinute + ":" + currSecond);

        var diff = (dateTime2 - dateTime1).TotalSeconds;
        //print("Before Diff : " + diff);
        string changeString = diff.ToString();
        char[] ch = changeString.ToCharArray();
        if (ch[0] == '-')
        {
            changeString = changeString.Substring(1, changeString.Length - 1);
        }
        long diffInSeconds = long.Parse(changeString);
        long diff1 = diffInSeconds % (interval * 30);

        //secondsCount = ((interval * 60) - (int)diff1);

        //print("Main : " + );
        secondsCount = Mathf.Abs((int)diff1 - (interval * 30));

        //print("Main : " + );
        //print("Date Diff Second : " + diffInSeconds);


    }

    private void OnApplicationPause(bool pause)
    {
        //print("Pause : " + pause);
        if (pause)
        {
            //Check
            //DateTime date = DateTime.Now;

            //PlayerPrefs.SetString("PlayTimeDate", date.ToString());
        }
        else
        {

            //gettime to diff
            // GetDiffPause();
            GetDiffMinute();

        }
    }
}