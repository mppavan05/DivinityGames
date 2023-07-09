using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TourData : MonoBehaviour
{
    public string id;
    public float betAmount;

    public Text titleTxt;
    //   public Button joinBtn;
    //public GameObject prize1Obj;
    //public GameObject prize2Obj;
    public Text entryAmount;
    //public Text prizeTxt2;
    public Text timeTxt;
    public Text joiningPlayers;
    public Text people;
    public Text peopleLimite;
    public Text winningAmount;
    public int tourIndex;
    int flag = 0;
    public int prizePool;
    
    public Button joinBtn;

    public Sprite freeEntry;
    public Sprite normalEntry;

    public Sprite joinSprite;
    public string createDate;
    public int interval = 0;
    public float secondsCount;
    public float playTime;
    public float totalWinAmount = 0;

    public int playerCnt;

    public int complexity;
    public bool isBot;
    public float bonusAmountDeduction;
    private void Start()
    {
        //GetDiffMinute();
    }

    //public void FreeButtonShow()
    //{

    //    joinBtn.GetComponent<Image>().sprite = freeEntry;
    //    joinBtn.transform.GetChild(0).GetComponent<Text>().text = "FREE";

    //}
    //public void JoinedButtonShow()
    //{

    //    joinBtn.GetComponent<Image>().sprite = joinSprite;

    //}

    public void FreeButtonShow()
    {
        entryAmount.text = "FREE";
    }

    public void JoinButton()
    {
        entryAmount.text = "JOINED";
    }

    public void SimpleButton()
    {
        entryAmount.text = "Rs." + betAmount;
    }


    public void ButtonReperesent()
    {
        if (DataManager.Instance.tournamentID == id)
        {
            JoinButton();
        }
        else if (betAmount == 0)
        {
            FreeButtonShow();
        }
        else
        {
            SimpleButton();
        }
    }

    public void PlayerIncrease()
    {
        if (playerCnt >= 0 && playerCnt < 10)
        {
            //print("Enter the conditon");
            playerCnt++;
            joiningPlayers.text = playerCnt + "";
            Invoke(nameof(PlayerIncrease), UnityEngine.Random.Range(0.5f, 1f));
        }
        else if (playerCnt >= 10 && playerCnt < 30)
        {
            playerCnt += UnityEngine.Random.Range(2, 6);
            joiningPlayers.text = playerCnt + "";
            Invoke(nameof(PlayerIncrease), UnityEngine.Random.Range(1f, 2f));
        }
        else if (playerCnt >= 30 && playerCnt < 60)
        {
            playerCnt += UnityEngine.Random.Range(4, 9);
            joiningPlayers.text = playerCnt + "";
            Invoke(nameof(PlayerIncrease), UnityEngine.Random.Range(1.5f, 3f));
        }
        else if (playerCnt >= 60 && playerCnt < 100)
        {
            playerCnt += UnityEngine.Random.Range(8, 14);
            joiningPlayers.text = playerCnt + playerCnt + "";
            Invoke(nameof(PlayerIncrease), UnityEngine.Random.Range(2f, 4f));
        }
        else if (playerCnt >= 100 && playerCnt < 200)
        {
            playerCnt += UnityEngine.Random.Range(10, 20);
            joiningPlayers.text = playerCnt + "";
            Invoke(nameof(PlayerIncrease), UnityEngine.Random.Range(2.5f, 4.5f));
        }
        else if (playerCnt >= 200 && playerCnt < 350)
        {
            playerCnt += UnityEngine.Random.Range(15, 25);
            joiningPlayers.text = playerCnt + "";
            Invoke(nameof(PlayerIncrease), UnityEngine.Random.Range(3f, 5f));
        }
        else if (playerCnt >= 350)
        {
            playerCnt += UnityEngine.Random.Range(15, 25);
            joiningPlayers.text = playerCnt + "";
            Invoke(nameof(PlayerIncrease), UnityEngine.Random.Range(3.5f, 6f));
        }
    }
    private void FixedUpdate()
    {
        //Timer();
    }

    private void GetDiffMinute()
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
        long diff1 = 0;
        if (diffInSeconds != 0)
        {
            //print("Diff In Second : " + diffInSeconds);
            //print("Interveal : " + interval);
            diff1 = diffInSeconds % (interval * 60);
        }
        //secondsCount = ((interval * 60) - (int)diff1);

        //print("Main : " + );
        secondsCount = Mathf.Abs((int)diff1 - (interval * 60));

        //print("Main : " + );
        //print("Date Diff Second : " + diffInSeconds);


        //print("Main : " + );
        //print("Date Diff Second : " + diffInSeconds);


    }

    void Timer()
    {
        if (flag == 0)
        {
            secondsCount -= Time.deltaTime;
            if (secondsCount < 0) { return; }
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

                if (DataManager.Instance.tournamentID == id)
                {
                    //MainMenuManager.Instance.OpenTournamentLoadScreen();
                }
                //if (joinBtn.transform.GetChild(0).GetComponent<Text>().text == "JOINED")
                //{
                //    //MainMenuManager.Instance.OpenTournamentLoadScreen(joinBtn.transform.GetChild(0).GetComponent<Text>());
                //}
                timeTxt.text = "00 min 00s";
                flag = 1;
            }

            if (flag == 1) return;
            timeTxt.text = timeValue;
        }
        else
        {
            playerCnt = 0;
            GetDiffMinute();
        }
    }

    #region  Application Pause
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




    void GetDiffPause()
    {

        //string getDate = PlayerPrefs.GetString("PlayTimeDate", "none");
        //if (getDate == "none")
        //{
        //    return;
        //}

        //int createHour = int.Parse(getDate.Split(" ")[1].Split(":")[0]);
        ////int currHour = ;
        //int createMinute = int.Parse(getDate.Split(" ")[1].Split(":")[1]);
        //int createSecond = int.Parse(getDate.Split(" ")[1].Split(":")[2]);

        //DateTime date = DateTime.Now;
        //string curDate = date.ToString();
        //int currHour = int.Parse(curDate.Split(" ")[1].Split(":")[0]);

        //int currMinute = int.Parse(curDate.Split(" ")[1].Split(":")[1]);
        //int currSecond = int.Parse(curDate.Split(" ")[1].Split(":")[2]);



        //DateTime dateTime1 = DateTime.Parse(createHour + ":" + createMinute + ":" + createSecond);
        //DateTime dateTime2 = DateTime.Parse(currHour + ":" + currMinute + ":" + currSecond);

        //var diff = (dateTime2 - dateTime1).TotalSeconds;

        //string changeString = diff.ToString();

        //long diffInSeconds = long.Parse(changeString);

        GetDiffMinute();

    }



    #endregion
}
