using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ArcheryScroreManager : MonoBehaviour
{
    public static bool isCompleted;

    public static bool isFailed;

    public static bool isGameOver;

    public static bool isHumanHit;

    public static bool isPaused;

    public static ArcheryScroreManager Instance;
    public Sprite[] profileAvatar;

    public Text playerNameTxt1;
    public Text playerNameTxt2;

    public Image playerProfile1;
    public Image playerProfile2;

    public Text playerScoreTxt1;
    public Text playerScoreTxt2;

    public Text timerTxt;

    public float secondsCount;
    public int flag = 0;
    
    [Header("--- Open Message Screen ---")]
    public GameObject soundOn;
    public GameObject soundOff;
    public GameObject vibOn;
    public GameObject vibOff;
    public GameObject sfxOn;
    public GameObject sfxOff;
    public GameObject shopPrefab;
    public GameObject shopPrefabParent;
    public GameObject ruleScreenObj;
    public GameObject menuScreenObj;
    public GameObject settingsScreenObj;
    





    [Header("---Match Win Manage---")]

    public GameObject winScreenObj;
    public bool isOpenWin;
    public bool isOtherPlayLeft;
    public int playerScoreCnt1;
    public int playerScoreCnt2;



    public bool isStop;

    float botRepeatTimeSet = 0;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        secondsCount = (TestSocketIO.Instace.playTime * 60);
        playerScoreTxt1.text = playerScoreCnt1.ToString();
        playerScoreTxt2.text = playerScoreCnt2.ToString();






    }


    private void Start()
    {
        SoundManager.Instance.StopBackgroundMusic();
        PlayerNameManage();

        if (DataManager.Instance.playerNo == 1)
        {
            DataManager.Instance.isDiceClick = true;
        }
        else
        {
            DataManager.Instance.isDiceClick = false;
        }

    }



    private void Update()
    {

        Timer();
    }

    public void PlayerNameManage()
    {
        if (DataManager.Instance.isTwoPlayer == true)
        {
            int index1 = 0;
            int index2 = 1;
            if (DataManager.Instance.playerNo == 2)
            {
                index1 = 1;
                index2 = 0;
            }
            playerNameTxt1.text = UserNameStringManage(DataManager.Instance.joinPlayerDatas[index1].userName);
            playerNameTxt2.text = UserNameStringManage(DataManager.Instance.joinPlayerDatas[index2].userName);


            //playerProfile1.sprite = profileAvatar[DataManager.Instance.joinPlayerDatas[index1].avtar];
            //playerProfile2.sprite = profileAvatar[DataManager.Instance.joinPlayerDatas[index2].avtar];

            StartCoroutine(GetImages(DataManager.Instance.joinPlayerDatas[index1].avtar, playerProfile1));
            StartCoroutine(GetImages(DataManager.Instance.joinPlayerDatas[index2].avtar, playerProfile2));

        }
    }

    IEnumerator GetImages(string URl, Image image)
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
        //print("Second Count : " + secondsCount);
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
        if (isStop == false)
        {
            isStop = true;
            isGameOver = true;
            winScreenObj.SetActive(true);
        }
    }
    public void CurrentScoreManage(int increment)
    {
        playerScoreCnt1 += increment;
        playerScoreTxt1.text = playerScoreCnt1.ToString();

        if (BotManager.Instance.isConnectBot)
        {
            if (BotManager.Instance.botType == BotType.Easy)
            {
                BotEasy(increment);
            }
            else if (BotManager.Instance.botType == BotType.Medium)
            {
                BotMedium(increment);
            }
            else if (BotManager.Instance.botType == BotType.Hard)
            {
                BotHard(increment);
            }
        }
        else
        {
            ArcherySendData(playerScoreCnt1);
        }

    }

    #region Ui Interaction

    public void MenuSubButtonClick(int no)
    {
        SoundManager.Instance.ButtonClick();
        if (no == 1)
        {
            Time.timeScale = 1;
            TestSocketIO.Instace.LeaveRoom();
            SoundManager.Instance.StartBackgroundMusic();
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
    
    void OpenRuleScreen()
    {
        ruleScreenObj.SetActive(true);
    }
    
    public void SettingsButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        OpenSettingsScreen();
    }
    
    public void SettingsCloseButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        CloseSettingsScreen();
    }

    public void MenuButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        OpenMenuScreen();
    }
    
    public void MenuCloseButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        CloseMenuScreen();
    }
    
    void OpenMenuScreen()
    {
        menuScreenObj.SetActive(true);
    }
    
    void CloseMenuScreen()
    {
        menuScreenObj.SetActive(false);
    }
    
    void OpenSettingsScreen()
    {
        settingsScreenObj.SetActive(true);
    }
    
    void CloseSettingsScreen()
    {
        settingsScreenObj.SetActive(false);
    }


    public void CloseRuleButton()
    {
        SoundManager.Instance.ButtonClick();
        ruleScreenObj.SetActive(false);
    }
    public void SoundButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        if (soundOn.activeSelf)
        {
            DataManager.Instance.SetSound(1);
            SoundManager.Instance.StopBackgroundMusic();
            soundOn.SetActive(false);
            soundOff.SetActive(true);
        }
        else
        {
            DataManager.Instance.SetSound(0);
            soundOff.SetActive(false);
            soundOn.SetActive(true);
            SoundManager.Instance.StartBackgroundMusic();
        }
    }
    
    public void VibrationButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        if (vibOn.activeSelf)
        {
            DataManager.Instance.SetVibration(1);
            //SoundManager.Instance.StopBackgroundMusic();
            vibOn.SetActive(false);
            vibOff.SetActive(true);
        }
        else
        {
            DataManager.Instance.SetVibration(0);
            vibOff.SetActive(false);
            vibOn.SetActive(true);
            //SoundManager.Instance.StartBackgroundMusic();
        }
    }
    
    public void SfxButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        if (sfxOn.activeSelf)
        {
            sfxOn.SetActive(false);
            sfxOff.SetActive(true);
        }
        else
        {
            sfxOn.SetActive(true);
            sfxOff.SetActive(false);
        }
    }

    private void ManageSoundButtons()
    {
        if (DataManager.Instance.GetSound() == 0)
        {
            soundOn.SetActive(true);
            soundOff.SetActive(false);
        }
        else
        {
            soundOn.SetActive(false);
            soundOff.SetActive(true);
        }

        if (DataManager.Instance.GetVibration() == 0)
        {
            vibOn.SetActive(true);
            vibOff.SetActive(false);
        }
        else
        {
            vibOn.SetActive(false);
            vibOff.SetActive(true);
        }
    }

    

    #endregion




    #region Archery Game Bot Manage

    void BotEasy(int ino)
    {
        playerScoreCnt2 = playerScoreCnt1 - Random.Range(2, 5);
        playerScoreTxt2.text = playerScoreCnt2.ToString();

    }

    void BotMedium(int ino)
    {
        int rno = Random.Range(0, 4);
        if (rno == 0)
        {
            BotEasy(ino);
        }
        else
        {
            BotHard(ino);
        }
    }

    void BotHard(int ino)
    {
        if (ino == 10)
        {
            playerScoreCnt2 = playerScoreCnt2 + ino;
        }
        else if (ino == 9)
        {
            playerScoreCnt2 = playerScoreCnt2 + 10;
        }
        else if (ino == 8)
        {
            playerScoreCnt2 = playerScoreCnt2 + Random.Range(9, 11);
        }
        else if (ino == 7)
        {
            playerScoreCnt2 = playerScoreCnt2 + Random.Range(8, 11);
        }
        else if (ino == 6)
        {
            playerScoreCnt2 = playerScoreCnt2 + Random.Range(7, 11);
        }
        else if (ino == 5)
        {
            playerScoreCnt2 = playerScoreCnt2 + Random.Range(6, 11);
        }
        else if (ino == 4)
        {
            playerScoreCnt2 = playerScoreCnt2 + Random.Range(5, 11);
        }
        else if (ino == 3)
        {
            playerScoreCnt2 = playerScoreCnt2 + Random.Range(4, 11);
        }
        else if (ino == 2)
        {
            playerScoreCnt2 = playerScoreCnt2 + Random.Range(4, 11);
        }
        else if (ino == 1)
        {
            playerScoreCnt2 = playerScoreCnt2 + Random.Range(4, 11);
        }

        playerScoreTxt2.text = playerScoreCnt2.ToString();

    }

    #endregion

    #region Socket Send

    public void ArcherySendData(int score)
    {
        if (BotManager.Instance.isConnectBot == false)
        {
            JSONObject obj = new JSONObject();
            obj.AddField("PlayerID", DataManager.Instance.playerData._id);
            obj.AddField("TournamentID", DataManager.Instance.tournamentID);
            int noSend = 0;
            noSend = DataManager.Instance.playerNo;

            if (DataManager.Instance.playerNo == 1)
            {

                noSend = 2;

            }
            else if (DataManager.Instance.playerNo == 2)
            {
                noSend = 1;
            }

            obj.AddField("PlayerNo", noSend);
            obj.AddField("Score", score);
            obj.AddField("RoomId", TestSocketIO.Instace.roomid);
            TestSocketIO.Instace.Senddata("ArcheryChangeData", obj);
        }


    }




    #endregion

    #region Socker Receive
    public void ArcherySendDataReceiveData(int no1)
    {
        playerScoreCnt2 = no1;
        playerScoreTxt2.text = playerScoreCnt2.ToString();

    }

    #endregion


    

   
}
