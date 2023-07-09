using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TournamentLoadPanel : MonoBehaviour
{
    public static TournamentLoadPanel Instance;
    public Text winAmount;



    [Header("---Player 2---")]
    public GameObject player2Obj;
    public Text player_2_amount1;
    public Text player_2_amount2;

    public Image player_2_profile1;
    public Image player_2_profile2;

    public Text player_2_name1;
    public Text player_2_name2;
    
    [Header("---Player 4---")]
    public GameObject player4Obj;
    public Text player_4_amount1;
    public Text player_4_amount2;
    public Text player_4_amount3;
    public Text player_4_amount4;

    public Image player_4_profile1;
    public Image player_4_profile2;
    public Image player_4_profile3;
    public Image player_4_profile4;

    public Text player_4_name1;
    public Text player_4_name2;
    public Text player_4_name3;
    public Text player_4_name4;
    public Sprite[] playerImg;

    public Text timeTxt;

    public float secondsCount;
    bool isTourEnter;

    // Start is called before the first frame update
    void Start()
    {

        if (Instance == null)
        {
            Instance = this;
        }
        MainMenuManager.Instance.screenObj.Add(this.gameObject);
        DataManager.Instance.SetPlayedGame(DataManager.Instance.GetPlayedGame() + 1);
        OpenTournamentLoad();

        //DebitAmount(string amount, string roomId, string note, string logType)


        if (float.Parse(DataManager.Instance.playerData.bonus) <= 0)
        {
            DataManager.Instance.DebitAmount((DataManager.Instance.tourEntryMoney).ToString(), TestSocketIO.Instace.roomid, "Ludo-Bet-" + TestSocketIO.Instace.roomid, "game", 0);
            //print("Enter the Total Balance 1  : " + DataManager.Instance.tourEntryMoney);
        }
        else
        {
            float bonus = float.Parse(DataManager.Instance.playerData.bonus);
            float cutMoney = (float)((DataManager.Instance.tourEntryMoney) / 100);
            if ((bonus - cutMoney) < 0)
            {
                print("Enter the Total Balance 2 : " + DataManager.Instance.tourEntryMoney);

                DataManager.Instance.DebitAmount((DataManager.Instance.tourEntryMoney).ToString(), TestSocketIO.Instace.roomid, "Ludo-Bet-" + TestSocketIO.Instace.roomid, "game", 0);
            }
            else
            {
                print("Enter the Cut Money Else : " + cutMoney);
                print("Total Balanace Else : " + (DataManager.Instance.tourEntryMoney - cutMoney));

                DataManager.Instance.DebitAmount((DataManager.Instance.tourEntryMoney - cutMoney).ToString(), TestSocketIO.Instace.roomid, "Ludo-Bet-" + TestSocketIO.Instace.roomid, "game",0);
            }

        }
    }

    // Update is called once per frame
    void Update()
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
    }

    public void BackButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        MainMenuManager.Instance.screenObj.Remove(this.gameObject);
        //MainMenuManager.Instance.TopBarDataSet();
        this.gameObject.SetActive(false);
        Destroy(this.gameObject);
    }

    void OpenTournamentLoad()
    {
        //        GetCommision();
        winAmount.text = DataManager.Instance.winAmount + " Coin";
        if (DataManager.Instance.isTwoPlayer)
        {

           // player2Obj.SetActive(true);

            int indexNo1 = 0;
            int indexNo2 = 0;

            //if(DataManager.Instance.CheckRoomUserNumber(DataManager.Instance.playerNo) == 1)
            {
                if (DataManager.Instance.playerNo == 3)
                {
                    indexNo1 = 1;
                    indexNo2 = 0;
                }
                else
                {
                    indexNo1 = 0;
                    indexNo2 = 1;
                }
            }
            //print("Index 1 : " + indexNo1);
            //print("Index 2 : " + indexNo2);

            DataManager.Instance.orgIndexPlayer = indexNo1;
            player_2_amount1.text = DataManager.Instance.tourEntryMoney.ToString();
            player_2_amount2.text = DataManager.Instance.tourEntryMoney.ToString();

            //print("Index no 1: " + indexNo1);
            //print("Index no 2: " + indexNo2);
            //UnityEditor.EditorApplication.isPaused = true;
            //player_2_profile1.sprite = playerImg[DataManager.Instance.joinPlayerDatas[indexNo1].avtar];
            //player_2_profile2.sprite = playerImg[DataManager.Instance.joinPlayerDatas[indexNo2].avtar];

            StartCoroutine(DataManager.Instance.GetImages(DataManager.Instance.joinPlayerDatas[indexNo1].avtar, player_2_profile1));

            StartCoroutine(DataManager.Instance.GetImages(DataManager.Instance.joinPlayerDatas[indexNo2].avtar, player_2_profile2));

            player_2_name1.text = UserNameStringManage(DataManager.Instance.joinPlayerDatas[indexNo1].userName);
            player_2_name2.text = UserNameStringManage(DataManager.Instance.joinPlayerDatas[indexNo2].userName);


        }
        else if (DataManager.Instance.isFourPlayer)
        {

            //player4Obj.SetActive(true);
            //player2Obj.SetActive(false);

            int indexNo1 = 0;
            int indexNo2 = 1;
            int indexNo3 = 2;
            int indexNo4 = 3;

           

            DataManager.Instance.orgIndexPlayer = indexNo1;
            player_4_amount1.text = DataManager.Instance.tourEntryMoney.ToString();
            player_4_amount2.text = DataManager.Instance.tourEntryMoney.ToString();
            player_4_amount3.text = DataManager.Instance.tourEntryMoney.ToString();
            player_4_amount4.text = DataManager.Instance.tourEntryMoney.ToString();

            //print("Index no 1: " + indexNo1);
            //print("Index no 2: " + indexNo2);
            //UnityEditor.EditorApplication.isPaused = true;
            //player_2_profile1.sprite = playerImg[DataManager.Instance.joinPlayerDatas[indexNo1].avtar];
            //player_2_profile2.sprite = playerImg[DataManager.Instance.joinPlayerDatas[indexNo2].avtar];

            StartCoroutine(DataManager.Instance.GetImages(DataManager.Instance.joinPlayerDatas[indexNo1].avtar, player_4_profile1));
            StartCoroutine(DataManager.Instance.GetImages(DataManager.Instance.joinPlayerDatas[indexNo2].avtar, player_4_profile2));
            StartCoroutine(DataManager.Instance.GetImages(DataManager.Instance.joinPlayerDatas[indexNo3].avtar, player_4_profile3));
            StartCoroutine(DataManager.Instance.GetImages(DataManager.Instance.joinPlayerDatas[indexNo4].avtar, player_4_profile4));

            player_4_name1.text = UserNameStringManage(DataManager.Instance.joinPlayerDatas[indexNo1].userName);
            player_4_name2.text = UserNameStringManage(DataManager.Instance.joinPlayerDatas[indexNo2].userName);
            player_4_name3.text = UserNameStringManage(DataManager.Instance.joinPlayerDatas[indexNo3].userName);
            player_4_name4.text = UserNameStringManage(DataManager.Instance.joinPlayerDatas[indexNo4].userName);


        }
        else if (!DataManager.Instance.isFourPlayer)
        {
            player4Obj.SetActive(false);
        }
        else if (!DataManager.Instance.isTwoPlayer)
        {
            player2Obj.SetActive(false);
        }

        //Invoke(nameof(OpenAPlayMode), 5f);
    }

    void OpenAPlayMode()
    {
        StartCoroutine(MainMenuManager.Instance.LoadScene());
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
    //void GetCommision()
    //{
    //    float money = DataManager.Instance.tourEntryMoney;
    //    int commision = DataManager.Instance.tourCommision;
    //    if (DataManager.Instance.isTwoPlayer)
    //    {
    //        print("Win Amount : " + ((float)(money + money) - ((float)((money + money)) * commision / 100)));
    //        DataManager.Instance.winAmount = (float)(money + money) - ((float)((money + money)) * commision / 100);
    //    }
    //    else
    //    {
    //        DataManager.Instance.winAmount = (float)(money + money + money + money) - ((float)((money + money + money + money)) * commision / 100);
    //    }

    //    DataManager.Instance.commisionAmount = Mathf.Abs(DataManager.Instance.winAmount - (float)(money + money + money + money));
    //}
}
