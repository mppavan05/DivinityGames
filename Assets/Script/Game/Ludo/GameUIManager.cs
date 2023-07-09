using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameUIManager : MonoBehaviour
{
    public static GameUIManager Instance;
    [Header("---Setting Screen---")]
    public GameObject settingScreenObj;
    public Image settingSoundImg;
    public Image settingVibrationImg;
    public Sprite settingSoundOn;
    public Sprite settingSoundOff;
    public Sprite settingVibrationOn;
    public Sprite settingVibrationOff;

    [Header("---Leave Game Screen---")]
    public GameObject leaveGameScreenObj;

    [Header("--- Rule Game Screen---")]
    public GameObject ruleScreenObj;
    public GameObject[] ruleSubScreenObj;
    public Button ruleLeftBtn;
    public Button ruleRightBtn;
    public int ruleScreenNo;

    [Header("--- Turn Screen---")]
    public GameObject turnSkipScreenObj;
    public Text turnSkipTitleTxt;
    public Text turnSkipSubtitleTxt;

    [Header("---Others--- ")]
    public GameObject potObj;
    public Text potTxt;


    public bool potObjectAvoid;


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

        //if (DataManager.Instance.playerData.firstName == "" || DataManager.Instance.playerData.firstName == null)
        //{
        //    player1Txt.text = UserNameStringManage(DataManager.Instance.GetDefaultPlayerName().ToString().Trim('"'));
        //}
        //else
        //{
        //    player1Txt.text = UserNameStringManage(DataManager.Instance.playerData.firstName.ToString().Trim('"'));
        //}

        /*if (potObjectAvoid == false)
        {
            if (DataManager.Instance.modeType == 3)
            {
                potTxt.text = 25.ToString();
            }
            else
            {
                potObj.SetActive(false);
            }
        }*/
    }





    // Update is called once per frame
    void Update()
    {

    }

    #region Play Screen

    public void HomeButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        OpenLeaveScreen();
    }

    public void SettingButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        OpenSettingScreen();
    }

    public void ListButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        OpenRuleScreen();
    }

    public void ScoreButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        OpenTurnSkip();

    }

    #endregion

    #region Setting Screen

    void OpenSettingScreen()
    {
        settingScreenObj.SetActive(true);
        if (DataManager.Instance.GetSound() == 0)
        {
            settingSoundImg.sprite = settingSoundOn;
        }
        else
        {
            settingSoundImg.sprite = settingSoundOff;
            settingSoundImg.sprite = settingSoundOff;
        }

        if (DataManager.Instance.GetVibration() == 0)
        {
            settingVibrationImg.sprite = settingVibrationOn;
        }
        else
        {
            settingVibrationImg.sprite = settingVibrationOff;
        }
    }

    public void Setting_Sound_ButtonClick()
    {
        if (settingSoundImg.sprite == settingSoundOn)
        {
            settingSoundImg.sprite = settingSoundOff;
            DataManager.Instance.SetSound(1);
        }
        else if (settingSoundImg.sprite == settingSoundOff)
        {

            SoundManager.Instance.ButtonClick();
            settingSoundImg.sprite = settingSoundOn;
            DataManager.Instance.SetSound(0);
        }
    }

    public void Setting_Vibration_ButtonClick()
    {
        SoundManager.Instance.ButtonClick();

        if (settingVibrationImg.sprite == settingVibrationOn)
        {
            settingVibrationImg.sprite = settingVibrationOff;
            DataManager.Instance.SetVibration(1);
        }
        else if (settingVibrationImg.sprite == settingVibrationOff)
        {
            settingVibrationImg.sprite = settingVibrationOn;
            DataManager.Instance.SetVibration(0);
        }
    }

    public void Setting_LeaveMatch_ButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        DataManager.Instance.isTwoPlayer = false;
        DataManager.Instance.isFourPlayer = false;
        TestSocketIO.Instace.LeaveRoom();
        //BotManager.Instance.isBotAvalible = false;
        //BotManager.Instance.isConnectBot = false;
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        SceneManager.LoadScene("Main");
        SoundManager.Instance.StartBackgroundMusic();
        print("Leave Match Button Click");
    }

    public void Setting_Close_ButtonClick()
    {
        SoundManager.Instance.ButtonClick();

        settingScreenObj.SetActive(false);
    }

    #endregion

    #region Leave Game

    public void OpenLeaveScreen()
    {
        leaveGameScreenObj.SetActive(true);
    }

    public void Leave_LeaveGame_ButtonClick()
    {
        SoundManager.Instance.ButtonClick();

        TestSocketIO.Instace.LeaveRoom();
        BotManager.Instance.isBotAvalible = false;
        BotManager.Instance.isConnectBot = false;
        SceneManager.LoadScene("Main");
        print("Leave a Game");
    }

    public void Leave_KeepPlaying_ButtonClick()
    {
        SoundManager.Instance.ButtonClick();

        leaveGameScreenObj.SetActive(false);
    }


    #endregion

    #region Rule Screen

    void OpenRuleScreen()
    {
        ruleScreenObj.SetActive(true);
        ruleScreenNo = 0;
        RuleSubScreenSet();
    }

    void RuleSubScreenSet()
    {
        for (int i = 0; i < ruleSubScreenObj.Length; i++)
        {
            if (i == ruleScreenNo)
            {
                ruleSubScreenObj[i].SetActive(true);
            }
            else
            {
                ruleSubScreenObj[i].SetActive(false);
            }
        }

        if (ruleScreenNo == 0)
        {
            ruleLeftBtn.interactable = false;
            ruleRightBtn.interactable = true;
        }
        else if (ruleScreenNo == ruleSubScreenObj.Length - 1)
        {
            ruleRightBtn.interactable = false;
            ruleLeftBtn.interactable = true;
        }
        else
        {
            ruleLeftBtn.interactable = true;
            ruleRightBtn.interactable = true;
        }
    }

    public void Rule_Left_ButtonClick()
    {
        SoundManager.Instance.ButtonClick();

        ruleScreenNo--;
        RuleSubScreenSet();
    }
    public void Rule_Right_ButtonClick()
    {
        SoundManager.Instance.ButtonClick();

        ruleScreenNo++;
        RuleSubScreenSet();
    }

    public void Rule_Close_ButtonClick()
    {
        SoundManager.Instance.ButtonClick();

        ruleScreenObj.SetActive(false);
    }

    #endregion

    #region Turn Skip

    void OpenTurnSkip()
    {
        turnSkipScreenObj.SetActive(true);
        turnSkipTitleTxt.text = "Turn Skipped";
        turnSkipSubtitleTxt.text = "Third 6 in row, can't be played.\nNext players turn.";
        Invoke(nameof(CloseTurnSkip), 1.75f);
    }

    void CloseTurnSkip()
    {
        SoundManager.Instance.ButtonClick();

        turnSkipScreenObj.SetActive(false);
    }
    #endregion
}
