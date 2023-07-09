using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public class MatchWinLeaderData
{
    public string userId;
    public string userName;
    public string lobbyId;
    public string roomName;
    public float entryAmount;
    public float winAmount;
    public bool isBot;
    public string gameName;

}
[System.Serializable]
public class WinDataSend
{
    public List<MatchWinLeaderData> matchWinLeaderDatas = new List<MatchWinLeaderData>();
}

public class MatchWinManager : MonoBehaviour
{
    public static MatchWinManager Instance;

    //public GameObject matchResultLoading;

    public Text rankTxtMain;
    public Image profileImgMain;
    public Text wonTitleMain;
    public Text scoreTxtMain;
    //public GameObject cup;


    public Text[] rankTxt;
    public Image[] profileImg;
    public Text[] profileNameTxt;
    public Text[] scoreTxt;
    public Text[] winTxt;

    public GameObject[] rowObj;
    public Sprite[] profileSprite;

    public MatchWinLeaderData leaderData1;
    public MatchWinLeaderData leaderData2;

    public WinDataSend winDataSend;

    string roomId = "";
    string gameName = "";
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
        //matchResultLoading.SetActive(true);

        MatchDataGet();
        //MatchDataGet();

    }

    public void MatchDataGet()
    {
        SoundManager.Instance.WinSound();
        winDataSend = new WinDataSend();
        leaderData1 = new MatchWinLeaderData();
        leaderData2 = new MatchWinLeaderData();
        if (SceneManager.GetActiveScene().name == "Carrom")
        {
            gameName = "Carrom";
            //DataSetCarrom();
        }
        else if (SceneManager.GetActiveScene().name == "Archery")
        {
            gameName = "Archery";
            DataSetArchery();
        }
        leaderData1.gameName = gameName;
        leaderData2.gameName = gameName;

        //matchResultLoading.SetActive(false);
    }

    /*#region Ludo Win
    void DataSetLudo()
    {
        if (LudoManager.Instance.isOpenWin == true)
        {
            return;
        }
        LudoManager.Instance.isOpenWin = true;
        if (DataManager.Instance.isTwoPlayer)
        {
            for (int i = 0; i < rowObj.Length; i++)
            {
                if (i <= 1)
                {
                    rowObj[i].SetActive(true);
                }
                else
                {
                    rowObj[i].SetActive(false);
                }
            }
            int playerWin = 0;
            if (LudoManager.Instance.isOtherPlayLeft == false)
            {
                int no1 = LudoManager.Instance.playerScoreCnt1;
                int no2 = LudoManager.Instance.playerScoreCnt3;


                if (no1 == no2)
                {
                    playerWin = 1;
                }
                else if (no1 > no2)
                {
                    playerWin = 2;
                }
                else
                {
                    playerWin = 3;
                }
            }
            else if (LudoManager.Instance.isOtherPlayLeft == true)
            {
                playerWin = 2;
            }




            if (playerWin == 1 || playerWin == 2)
            {
                //rankTxtMain.transform.parent.transform.GetChild(4).GetComponent<Text>().text = "Congratulations";
                winTxtTitle.text = "You have won the game!";

                winImg.sprite = congrulationSprite;

                rankSide1.sprite = rankSideSprite1[0];
                rankSide2.sprite = rankSideSprite2[0];

                if (playerWin == 1)
                {
                    wonTitleMain.text = "YOU WON " + (DataManager.Instance.winAmount).ToString("F2") + " Coin";


                    //float adminCommision = ((DataManager.Instance.tourEntryMoney) * 2) - DataManager.Instance.winAmount;

                    float adminCommision = DataManager.Instance.adminPercentage;


                    // if (DataManager.Instance.tourEntryMoney == 0)
                    // {
                    //     adminCommision = 0;
                    // }


                    DataManager.Instance.AddAmount(((float)DataManager.Instance.winAmount), DataManager.Instance.gameId, "Ludo-Win-" + DataManager.Instance.gameId, "won", adminCommision, 1);

                    DataManager.Instance.SetWonMoneyGame(DataManager.Instance.GetWonMoneyGame() + DataManager.Instance.winAmount);
                }
                else
                {
                    wonTitleMain.text = "YOU WON " + DataManager.Instance.winAmount + " Coin";


                    //float adminCommision = ((DataManager.Instance.tourEntryMoney / 10) * 2) - DataManager.Instance.winAmount;
                    float adminCommision = DataManager.Instance.adminPercentage;
                    bool isTournamentFree = false;
                    if (DataManager.Instance.tourEntryMoney == 0)
                    {
                        isTournamentFree = true;
                    }
                    if (DataManager.Instance.playerData.membership == "free")
                    {
                        if (isTournamentFree)
                        {
                            adminCommision = 0;
                        }
                        DataManager.Instance.AddAmount(DataManager.Instance.winAmount, DataManager.Instance.gameId, "Win " + gameName + "-" + DataManager.Instance.gameId, "won", adminCommision, 2);

                        DataManager.Instance.SetWonMoneyGame(DataManager.Instance.GetWonMoneyGame() + DataManager.Instance.winAmount);
                    }
                    else
                    {

                        adminCommision = 0;
                        float winVIPAmount = 0;
                        if (isTournamentFree)
                        {
                            winVIPAmount = DataManager.Instance.winAmount / 10;
                        }
                        else
                        {
                            winVIPAmount = (DataManager.Instance.tourEntryMoney / 5);
                        }
                        DataManager.Instance.AddAmount(winVIPAmount, DataManager.Instance.gameId, "Ludo-Win-" + DataManager.Instance.gameId, "won", adminCommision, 2);

                        DataManager.Instance.SetWonMoneyGame(DataManager.Instance.GetWonMoneyGame() + DataManager.Instance.winAmount);
                    }
                }
            }
            else
            {
                //wonTitleMain.text = "YOU WON ₹ " + 0;
                winTxtTitle.text = "You have lost the game!";
                winImg.sprite = tryAgainSprite;

                rankSide1.sprite = rankSideSprite1[1];
                rankSide2.sprite = rankSideSprite2[1];
                wonTitleMain.text = "Let's try again";
            }



            if (DataManager.Instance.isTwoPlayer)
            {

                profileNameTxt[0].text = "1st";


                if (LudoManager.Instance.isOtherPlayLeft)
                {
                    profileNameTxt[1].text = "Left-";

                }
                else
                {
                    profileNameTxt[1].text = "2nd";
                }

            }

            if (playerWin == 1 || playerWin == 2)
            {
                int getIndex = 0;
                if (DataManager.Instance.playerNo == 3)
                {
                    getIndex = 1;
                }
                //profileImgMain.sprite = profileSprite[DataManager.Instance.joinPlayerDatas[getIndex].avtar];
                profileNameMain.text = DataManager.Instance.joinPlayerDatas[getIndex].userName;
                //profileImg[0].sprite = profileSprite[DataManager.Instance.joinPlayerDatas[getIndex].avtar];
                profileNameTxt[0].text += UserNameStringManage(DataManager.Instance.joinPlayerDatas[getIndex].userName);
                scoreTxt[0].text = LudoManager.Instance.playerScoreCnt1.ToString();
                winTxt[0].text = DataManager.Instance.winAmount + " Coin";

                int secondIndex = 0;
                if (getIndex == 0)
                {
                    secondIndex = 1;
                }
                else if (getIndex == 1)
                {
                    secondIndex = 0;
                }
                //profileImg[1].sprite = profileSprite[DataManager.Instance.joinPlayerDatas[secondIndex].avtar];
                profileNameTxt[1].text += UserNameStringManage(DataManager.Instance.joinPlayerDatas[secondIndex].userName);

                if (LudoManager.Instance.isOtherPlayLeft)
                {
                    scoreTxt[1].text = "";
                    winTxt[1].text = "Left";
                }
                else
                {
                    scoreTxt[1].text = LudoManager.Instance.playerScoreCnt3.ToString();
                    winTxt[1].text = 0 + " Coin";
                }



            }
            else if (playerWin == 3)
            {
                int getIndex = 0;
                if (DataManager.Instance.playerNo == 3)
                {
                    getIndex = 1;
                }

                //profileImgMain.sprite = profileSprite[DataManager.Instance.joinPlayerDatas[getIndex].avtar];
                profileNameMain.text = DataManager.Instance.joinPlayerDatas[getIndex].userName;

                //profileImg[1].sprite = profileSprite[DataManager.Instance.joinPlayerDatas[getIndex].avtar];
                profileNameTxt[1].text += UserNameStringManage(DataManager.Instance.joinPlayerDatas[getIndex].userName);

                scoreTxt[1].text = LudoManager.Instance.playerScoreCnt1.ToString();
                winTxt[1].text = 0 + " Coin";

                int secondIndex = 0;
                if (getIndex == 0)
                {
                    secondIndex = 1;
                }
                else if (getIndex == 1)
                {
                    secondIndex = 0;
                }
                //profileImg[0].sprite = profileSprite[DataManager.Instance.joinPlayerDatas[secondIndex].avtar];
                profileNameTxt[0].text += UserNameStringManage(DataManager.Instance.joinPlayerDatas[secondIndex].userName);

                scoreTxt[0].text = LudoManager.Instance.playerScoreCnt3.ToString();
                winTxt[0].text = DataManager.Instance.winAmount + " Coin";
            }
        }
        else
        {
            for (int i = 0; i < rowObj.Length; i++)
            {
                if (i <= 1)
                {
                    rowObj[i].SetActive(true);
                }
                else
                {
                    rowObj[i].SetActive(false);
                }
            }
            int playerWin = 0;
            if (LudoManager.Instance.isOtherPlayLeft == false)
            {
                int no1 = LudoManager.Instance.playerScoreCnt1;
                int no2 = LudoManager.Instance.playerScoreCnt2;
                int no3 = LudoManager.Instance.playerScoreCnt3;
                int no4 = LudoManager.Instance.playerScoreCnt4;


                if (no1 == no2)
                {
                    playerWin = 1;
                }
                else if (no1 > no2)
                {
                    playerWin = 2;
                }
                else if(no3 == no4)
                {
                    playerWin = 3;
                }
                else
                {
                    playerWin = 4;
                }
            }
            else if (LudoManager.Instance.isOtherPlayLeft == true)
            {
                playerWin = 2;
            }




            if (playerWin == 1 || playerWin == 2)
            {
                //rankTxtMain.transform.parent.transform.GetChild(4).GetComponent<Text>().text = "Congratulations";
                winTxtTitle.text = "You have won the game!";

                winImg.sprite = congrulationSprite;

                rankSide1.sprite = rankSideSprite1[0];
                rankSide2.sprite = rankSideSprite2[0];

                if (playerWin == 1)
                {
                    wonTitleMain.text = "YOU WON " + (DataManager.Instance.winAmount / 2).ToString("F2") + " Coin";


                    float adminCommision = ((DataManager.Instance.tourEntryMoney) * 2) - DataManager.Instance.winAmount;


                    if (DataManager.Instance.tourEntryMoney == 0)
                    {
                        adminCommision = 0;
                    }


                    DataManager.Instance.AddAmount(((float)DataManager.Instance.winAmount), DataManager.Instance.gameId, "Ludo-Win-" + DataManager.Instance.gameId, "won", adminCommision, 1);

                    DataManager.Instance.SetWonMoneyGame(DataManager.Instance.GetWonMoneyGame() + DataManager.Instance.winAmount);
                }
                else
                {
                    wonTitleMain.text = "YOU WON " + DataManager.Instance.winAmount + " Coin";


                    float adminCommision = ((DataManager.Instance.tourEntryMoney / 10) * 2) - DataManager.Instance.winAmount;
                    bool isTournamentFree = false;
                    if (DataManager.Instance.tourEntryMoney == 0)
                    {
                        isTournamentFree = true;
                    }
                    if (DataManager.Instance.playerData.membership == "free")
                    {
                        if (isTournamentFree)
                        {
                            adminCommision = 0;
                        }
                        DataManager.Instance.AddAmount(DataManager.Instance.winAmount, DataManager.Instance.gameId, "Win " + gameName + "-" + DataManager.Instance.gameId, "won", adminCommision, 2);

                        DataManager.Instance.SetWonMoneyGame(DataManager.Instance.GetWonMoneyGame() + DataManager.Instance.winAmount);
                    }
                    else
                    {

                        adminCommision = 0;
                        float winVIPAmount = 0;
                        if (isTournamentFree)
                        {
                            winVIPAmount = DataManager.Instance.winAmount / 10;
                        }
                        else
                        {
                            winVIPAmount = (DataManager.Instance.tourEntryMoney / 5);
                        }
                        DataManager.Instance.AddAmount(winVIPAmount, DataManager.Instance.gameId, "Ludo-Win-" + DataManager.Instance.gameId, "won", adminCommision, 2);

                        DataManager.Instance.SetWonMoneyGame(DataManager.Instance.GetWonMoneyGame() + DataManager.Instance.winAmount);
                    }
                }
            }
            else
            {
                //wonTitleMain.text = "YOU WON ₹ " + 0;
                winTxtTitle.text = "You have lost the game!";
                winImg.sprite = tryAgainSprite;

                rankSide1.sprite = rankSideSprite1[1];
                rankSide2.sprite = rankSideSprite2[1];
                wonTitleMain.text = "Let's try again";
            }



            if (DataManager.Instance.isTwoPlayer)
            {

                profileNameTxt[0].text = "1st";


                if (LudoManager.Instance.isOtherPlayLeft)
                {
                    profileNameTxt[1].text = "Left-";

                }
                else
                {
                    profileNameTxt[1].text = "2nd";
                }

            }
            else
            {
                profileNameTxt[0].text = "1st";


                if (LudoManager.Instance.isOtherPlayLeft)
                {
                    profileNameTxt[1].text = "Left-";

                }
                else
                {
                    profileNameTxt[1].text = "2nd";
                }
            }

            if (playerWin == 1 || playerWin == 2)
            {
                int getIndex = 0;
                if (DataManager.Instance.playerNo == 3)
                {
                    getIndex = 1;
                }
                //profileImgMain.sprite = profileSprite[DataManager.Instance.joinPlayerDatas[getIndex].avtar];
                profileNameMain.text = DataManager.Instance.joinPlayerDatas[getIndex].userName;
                //profileImg[0].sprite = profileSprite[DataManager.Instance.joinPlayerDatas[getIndex].avtar];
                profileNameTxt[0].text += UserNameStringManage(DataManager.Instance.joinPlayerDatas[getIndex].userName);
                scoreTxt[0].text = LudoManager.Instance.playerScoreCnt1.ToString();
                winTxt[0].text = DataManager.Instance.winAmount + " Coin";

                int secondIndex = 0;
                if (getIndex == 0)
                {
                    secondIndex = 1;
                }
                else if (getIndex == 1)
                {
                    secondIndex = 0;
                }
                //profileImg[1].sprite = profileSprite[DataManager.Instance.joinPlayerDatas[secondIndex].avtar];
                profileNameTxt[1].text += UserNameStringManage(DataManager.Instance.joinPlayerDatas[secondIndex].userName);

                if (LudoManager.Instance.isOtherPlayLeft)
                {
                    scoreTxt[1].text = "";
                    winTxt[1].text = "Left";
                }
                else
                {
                    scoreTxt[1].text = LudoManager.Instance.playerScoreCnt3.ToString();
                    winTxt[1].text = 0 + " Coin";
                }



            }
            else if (playerWin == 3)
            {
                int getIndex = 0;
                if (DataManager.Instance.playerNo == 3)
                {
                    getIndex = 1;
                }

                //profileImgMain.sprite = profileSprite[DataManager.Instance.joinPlayerDatas[getIndex].avtar];
                profileNameMain.text = DataManager.Instance.joinPlayerDatas[getIndex].userName;

                //profileImg[1].sprite = profileSprite[DataManager.Instance.joinPlayerDatas[getIndex].avtar];
                profileNameTxt[1].text += UserNameStringManage(DataManager.Instance.joinPlayerDatas[getIndex].userName);

                scoreTxt[1].text = LudoManager.Instance.playerScoreCnt1.ToString();
                winTxt[1].text = 0 + " Coin";

                int secondIndex = 0;
                if (getIndex == 0)
                {
                    secondIndex = 1;
                }
                else if (getIndex == 1)
                {
                    secondIndex = 0;
                }
                //profileImg[0].sprite = profileSprite[DataManager.Instance.joinPlayerDatas[secondIndex].avtar];
                profileNameTxt[0].text += UserNameStringManage(DataManager.Instance.joinPlayerDatas[secondIndex].userName);

                scoreTxt[0].text = LudoManager.Instance.playerScoreCnt3.ToString();
                winTxt[0].text = DataManager.Instance.winAmount + " Coin";
            }
            else if(playerWin == 4)
            {
                int getIndex = 0;
                if (DataManager.Instance.playerNo == 3)
                {
                    getIndex = 1;
                }

                //profileImgMain.sprite = profileSprite[DataManager.Instance.joinPlayerDatas[getIndex].avtar];
                profileNameMain.text = DataManager.Instance.joinPlayerDatas[getIndex].userName;

                //profileImg[1].sprite = profileSprite[DataManager.Instance.joinPlayerDatas[getIndex].avtar];
                profileNameTxt[1].text += UserNameStringManage(DataManager.Instance.joinPlayerDatas[getIndex].userName);

                scoreTxt[1].text = LudoManager.Instance.playerScoreCnt1.ToString();
                winTxt[1].text = 0 + " Coin";

                int secondIndex = 0;
                if (getIndex == 0)
                {
                    secondIndex = 1;
                }
                else if (getIndex == 1)
                {
                    secondIndex = 0;
                }
                //profileImg[0].sprite = profileSprite[DataManager.Instance.joinPlayerDatas[secondIndex].avtar];
                profileNameTxt[0].text += UserNameStringManage(DataManager.Instance.joinPlayerDatas[secondIndex].userName);

                scoreTxt[0].text = LudoManager.Instance.playerScoreCnt3.ToString();
                winTxt[0].text = DataManager.Instance.winAmount + " Coin";
                
            }
        }

    }
    #endregion*/
    
     #region Archery

    void DataSetArchery()
    {

        ArcheryScroreManager.Instance.isOpenWin = true;
        if (DataManager.Instance.isTwoPlayer)
        {
            for (int i = 0; i < rowObj.Length; i++)
            {
                if (i <= 1)
                {
                    rowObj[i].SetActive(true);
                }
                else
                {
                    rowObj[i].SetActive(false);
                }
            }
            int playerWin = 0;
            if (ArcheryScroreManager.Instance.isOtherPlayLeft == false)
            {
                int no1 = ArcheryScroreManager.Instance.playerScoreCnt1;
                int no2 = ArcheryScroreManager.Instance.playerScoreCnt2;


                if (no1 == no2)
                {
                    playerWin = 1;
                }
                else if (no1 > no2)
                {
                    playerWin = 2;
                }
                else
                {
                    playerWin = 3;
                }
            }
            else if (ArcheryScroreManager.Instance.isOtherPlayLeft == true)
            {
                playerWin = 2;
            }




            float adminCommision = ((DataManager.Instance.tourEntryMoney / 1) * 2) - (DataManager.Instance.winAmount / 1);

            if (playerWin == 1 || playerWin == 2)
            {
                rankTxtMain.text = "Congratulations";


                if (playerWin == 1)
                {
                    wonTitleMain.text = "YOU WON ₹" + (DataManager.Instance.winAmount / 2).ToString("F2");
                    //rankTxtMain.text = "1";
                    //half Admin commssion

                    DataManager.Instance.AddAmount(((float)DataManager.Instance.winAmount / 2) / 1, TestSocketIO.Instace.roomid, "Win " + gameName + "-" + TestSocketIO.Instace.roomid, "won", adminCommision / 2, 0);

                    DataManager.Instance.SetWonMoneyGame(DataManager.Instance.GetWonMoneyGame() + DataManager.Instance.winAmount);
                }
                else
                {
                    wonTitleMain.text = "YOU WON ₹" + DataManager.Instance.winAmount;
                    //rankTxtMain.text = "1";
                    // Full Admin Commision

                    DataManager.Instance.AddAmount(DataManager.Instance.winAmount / 1, TestSocketIO.Instace.roomid, "Win " + gameName + "-" + TestSocketIO.Instace.roomid, "won", adminCommision, 1);

                    DataManager.Instance.SetWonMoneyGame(DataManager.Instance.GetWonMoneyGame() + DataManager.Instance.winAmount);

                }
            }
            else
            {
                //rankTxtMain.text = "2";
                //wonTitleMain.text = "YOU WON ₹ " + 0;
                //cup.gameObject.SetActive(false);
                rankTxtMain.text = "You Lost";
                wonTitleMain.text = "Let's try again";
            }

            //scoreTxtMain.text = ArcheryScroreManager.Instance.playerScoreCnt1.ToString();


            if (DataManager.Instance.isTwoPlayer)
            {

                rankTxt[0].text = "1";

                if (ArcheryScroreManager.Instance.isOtherPlayLeft)
                {
                    rankTxt[1].text = "-";

                }
                else
                {
                    rankTxt[1].text = "2";
                }

            }

            if (playerWin == 1 || playerWin == 2)
            {
                int getIndex = 0;
                if (DataManager.Instance.playerNo == 2)
                {
                    getIndex = 1;
                }
                //profileImgMain.sprite = profileSprite[DataManager.Instance.joinPlayerDatas[getIndex].avtar];

                //profileImg[0].sprite = profileSprite[DataManager.Instance.joinPlayerDatas[getIndex].avtar];



                StartCoroutine(GetImages(DataManager.Instance.joinPlayerDatas[getIndex].avtar, profileImgMain));
                StartCoroutine(GetImages(DataManager.Instance.joinPlayerDatas[getIndex].avtar, profileImg[0]));
                profileNameTxt[0].text = UserNameStringManage(DataManager.Instance.joinPlayerDatas[getIndex].userName);
                scoreTxt[0].text =ArcheryScroreManager.Instance.playerScoreCnt1.ToString();
                winTxt[0].text = "₹" + DataManager.Instance.winAmount;

                leaderData1.userId = DataManager.Instance.joinPlayerDatas[getIndex].userId;
                leaderData1.userName = DataManager.Instance.joinPlayerDatas[getIndex].userName;
                leaderData1.lobbyId = DataManager.Instance.joinPlayerDatas[getIndex].lobbyId;
                leaderData1.roomName = TestSocketIO.Instace.roomid;
                leaderData1.entryAmount = (DataManager.Instance.tourEntryMoney / 1);
                leaderData1.winAmount = (DataManager.Instance.winAmount / 1);
                if (BotManager.Instance.isConnectBot)
                {
                    if (!leaderData1.userId.Equals(DataManager.Instance.playerData._id))
                    {
                        leaderData1.isBot = true;
                    }
                }
                int secondIndex = 0;
                if (getIndex == 0)
                {
                    secondIndex = 1;
                }
                else if (getIndex == 1)
                {
                    secondIndex = 0;
                }
                //profileImg[1].sprite = profileSprite[DataManager.Instance.joinPlayerDatas[secondIndex].avtar];

                StartCoroutine(GetImages(DataManager.Instance.joinPlayerDatas[secondIndex].avtar, profileImg[1]));

                profileNameTxt[1].text = UserNameStringManage(DataManager.Instance.joinPlayerDatas[secondIndex].userName);

                if (ArcheryScroreManager.Instance.isOtherPlayLeft)
                {
                    scoreTxt[1].text = "";
                    winTxt[1].text = "Left";
                }
                else
                {
                    scoreTxt[1].text =ArcheryScroreManager.Instance.playerScoreCnt2.ToString();
                    winTxt[1].text = "₹" + 0;
                }
                leaderData2.userId = DataManager.Instance.joinPlayerDatas[secondIndex].userId;
                leaderData2.userName = DataManager.Instance.joinPlayerDatas[secondIndex].userName;
                leaderData2.lobbyId = DataManager.Instance.joinPlayerDatas[secondIndex].lobbyId;
                leaderData2.roomName = TestSocketIO.Instace.roomid;
                leaderData2.entryAmount = (DataManager.Instance.tourEntryMoney / 1);
                leaderData2.winAmount = 0;
                if (BotManager.Instance.isConnectBot)
                {
                    if (!leaderData2.userId.Equals(DataManager.Instance.playerData._id))
                    {
                        leaderData2.isBot = true;
                    }
                }

                leaderData1.gameName = gameName;
                leaderData2.gameName = gameName;

                winDataSend.matchWinLeaderDatas.Add(leaderData1);
                winDataSend.matchWinLeaderDatas.Add(leaderData2);

                string sendWinJson = JsonUtility.ToJson(winDataSend);

                string tmpStatus = "";
                if (playerWin == 1)
                {
                    tmpStatus = "tie";
                }
                else if (playerWin == 2)
                {
                    tmpStatus = "win";
                }

                DataManager.Instance.SendLeaderBoardData(leaderData1.userId, leaderData1.winAmount, leaderData1.lobbyId, 1, leaderData1.roomName, "Win " + gameName + "-" + TestSocketIO.Instace.roomid, sendWinJson, tmpStatus);
            }
            else if (playerWin == 3)
            {
                int getIndex = 0;
                if (DataManager.Instance.playerNo == 2)
                {
                    getIndex = 1;
                }

                //profileImgMain.sprite = profileSprite[DataManager.Instance.joinPlayerDatas[getIndex].avtar];
                //profileImg[1].sprite = profileSprite[DataManager.Instance.joinPlayerDatas[getIndex].avtar];
                StartCoroutine(GetImages(DataManager.Instance.joinPlayerDatas[getIndex].avtar, profileImgMain));
                StartCoroutine(GetImages(DataManager.Instance.joinPlayerDatas[getIndex].avtar, profileImg[1]));

                profileNameTxt[1].text = UserNameStringManage(DataManager.Instance.joinPlayerDatas[getIndex].userName);

                scoreTxt[1].text = ArcheryScroreManager.Instance.playerScoreCnt1.ToString();
                winTxt[1].text = "₹" + 0;

                leaderData1.userId = DataManager.Instance.joinPlayerDatas[getIndex].userId;
                leaderData1.userName = DataManager.Instance.joinPlayerDatas[getIndex].userName;
                leaderData1.lobbyId = DataManager.Instance.joinPlayerDatas[getIndex].lobbyId;
                leaderData1.roomName = TestSocketIO.Instace.roomid;
                leaderData1.entryAmount = (DataManager.Instance.tourEntryMoney / 1);
                leaderData1.winAmount = 0;
                if (BotManager.Instance.isConnectBot)
                {
                    if (!leaderData1.userId.Equals(DataManager.Instance.playerData._id))
                    {
                        leaderData1.isBot = true;
                    }
                }


                int secondIndex = 0;
                if (getIndex == 0)
                {
                    secondIndex = 1;
                }
                else if (getIndex == 1)
                {
                    secondIndex = 0;
                }
                //profileImg[0].sprite = profileSprite[DataManager.Instance.joinPlayerDatas[secondIndex].avtar];
                StartCoroutine(GetImages(DataManager.Instance.joinPlayerDatas[secondIndex].avtar, profileImg[0]));

                profileNameTxt[0].text = UserNameStringManage(DataManager.Instance.joinPlayerDatas[secondIndex].userName);

                scoreTxt[0].text = ArcheryScroreManager.Instance.playerScoreCnt2.ToString();
                winTxt[0].text = "₹" + DataManager.Instance.winAmount;

                leaderData2.userId = DataManager.Instance.joinPlayerDatas[secondIndex].userId;
                leaderData2.userName = DataManager.Instance.joinPlayerDatas[secondIndex].userName;
                leaderData2.lobbyId = DataManager.Instance.joinPlayerDatas[secondIndex].lobbyId;
                leaderData2.roomName = TestSocketIO.Instace.roomid;
                leaderData2.entryAmount = (DataManager.Instance.tourEntryMoney / 1);
                leaderData2.winAmount = (DataManager.Instance.winAmount / 1);
                if (BotManager.Instance.isConnectBot)
                {
                    if (!leaderData2.userId.Equals(DataManager.Instance.playerData._id))
                    {
                        leaderData2.isBot = true;
                    }
                }

                leaderData1.gameName = gameName;
                leaderData2.gameName = gameName;

                winDataSend.matchWinLeaderDatas.Add(leaderData1);
                winDataSend.matchWinLeaderDatas.Add(leaderData2);

                string sendWinJson = JsonUtility.ToJson(winDataSend);

                DataManager.Instance.SendLeaderBoardData(leaderData2.userId, leaderData2.winAmount, leaderData2.lobbyId, 2, leaderData2.roomName, "Win " + gameName + "-" + TestSocketIO.Instace.roomid, sendWinJson, "loss");
            }

        }

    }

    #endregion
    
    /*#region Carrom

    void DataSetCarrom()
    {

        CarromSocketManager.Instance.isOpenWin = true;
        if (DataManager.Instance.isTwoPlayer)
        {
            for (int i = 0; i < rowObj.Length; i++)
            {
                if (i <= 1)
                {
                    rowObj[i].SetActive(true);
                }
                else
                {
                    rowObj[i].SetActive(false);
                }
            }
            int playerWin = 0;
            if (CarromSocketManager.Instance.isOtherPlayLeft == false)
            {
                int no1 = CarromSocketManager.Instance.playerScoreCnt1;
                int no2 = CarromSocketManager.Instance.playerScoreCnt2;


                if (no1 == no2)
                {
                    playerWin = 1;
                }
                else if (no1 > no2)
                {
                    playerWin = 2;
                }
                else
                {
                    playerWin = 3;
                }
            }
            else if (CarromSocketManager.Instance.isOtherPlayLeft == true)
            {
                playerWin = 2;
            }




            float adminCommision = ((DataManager.Instance.tourEntryMoney / 1) * 2) - (DataManager.Instance.winAmount / 1);

            if (playerWin == 1 || playerWin == 2)
            {
                rankTxtMain.transform.parent.transform.GetChild(4).GetComponent<Text>().text = "Congratulations";


                if (playerWin == 1)
                {
                    wonTitleMain.text = "YOU WON ₹" + (DataManager.Instance.winAmount / 2).ToString("F2");
                    //rankTxtMain.text = "1";
                    //half Admin commssion

                    DataManager.Instance.AddAmount(((float)DataManager.Instance.winAmount / 2) / 1, TestSocketIO.Instace.roomid, "Win " + gameName + "-" + TestSocketIO.Instace.roomid, "won", adminCommision / 2, "tie");

                    DataManager.Instance.SetWonMoneyGame(DataManager.Instance.GetWonMoneyGame() + DataManager.Instance.winAmount);
                }
                else
                {
                    wonTitleMain.text = "YOU WON ₹" + DataManager.Instance.winAmount;
                    //rankTxtMain.text = "1";
                    // Full Admin Commision

                    DataManager.Instance.AddAmount(DataManager.Instance.winAmount / 1, TestSocketIO.Instace.roomid, "Win " + gameName + "-" + TestSocketIO.Instace.roomid, "won", adminCommision, "win");

                    DataManager.Instance.SetWonMoneyGame(DataManager.Instance.GetWonMoneyGame() + DataManager.Instance.winAmount);

                }
            }
            else
            {
                //rankTxtMain.text = "2";
                //wonTitleMain.text = "YOU WON ₹ " + 0;
                cup.gameObject.SetActive(false);
                rankTxtMain.transform.parent.transform.GetChild(4).GetComponent<Text>().text = "You Lost";
                wonTitleMain.text = "Let's try again";
            }

            //scoreTxtMain.text = CarromSocketManager.Instance.playerScoreCnt1.ToString();


            if (DataManager.Instance.isTwoPlayer)
            {

                rankTxt[0].text = "1";

                if (CarromSocketManager.Instance.isOtherPlayLeft)
                {
                    rankTxt[1].text = "-";

                }
                else
                {
                    rankTxt[1].text = "2";
                }

            }

            if (playerWin == 1 || playerWin == 2)
            {
                int getIndex = 0;
                if (DataManager.Instance.playerNo == 2)
                {
                    getIndex = 1;
                }
                //profileImgMain.sprite = profileSprite[DataManager.Instance.joinPlayerDatas[getIndex].avtar];

                //profileImg[0].sprite = profileSprite[DataManager.Instance.joinPlayerDatas[getIndex].avtar];

                //StartCoroutine(GetImages(DataManager.Instance.joinPlayerDatas[getIndex].pPicture, profileImgMain));
                //StartCoroutine(GetImages(DataManager.Instance.joinPlayerDatas[getIndex].pPicture, profileImg[0]));

                profileNameTxt[0].text = UserNameStringManage(DataManager.Instance.joinPlayerDatas[getIndex].userName);
                scoreTxt[0].text ="Points : " +  CarromSocketManager.Instance.playerScoreCnt1.ToString();
                winTxt[0].text = "₹" + DataManager.Instance.winAmount;

                leaderData1.userId = DataManager.Instance.joinPlayerDatas[getIndex].userId;
                leaderData1.userName = DataManager.Instance.joinPlayerDatas[getIndex].userName;
                leaderData1.lobbyId = DataManager.Instance.joinPlayerDatas[getIndex].lobbyId;
                leaderData1.roomName = TestSocketIO.Instace.roomid;
                leaderData1.entryAmount = (DataManager.Instance.tourEntryMoney / 1);
                leaderData1.winAmount = (DataManager.Instance.winAmount / 1);
                if (BotManager.Instance.isConnectBot)
                {
                    if (!leaderData1.userId.Equals(DataManager.Instance.playerData._id))
                    {
                        leaderData1.isBot = true;
                    }
                }

                int secondIndex = 0;
                if (getIndex == 0)
                {
                    secondIndex = 1;
                }
                else if (getIndex == 1)
                {
                    secondIndex = 0;
                }
                //profileImg[1].sprite = profileSprite[DataManager.Instance.joinPlayerDatas[secondIndex].avtar];
                //StartCoroutine(GetImages(DataManager.Instance.joinPlayerDatas[secondIndex].pPicture, profileImg[1]));
                profileNameTxt[1].text = UserNameStringManage(DataManager.Instance.joinPlayerDatas[secondIndex].userName);


                if (CarromSocketManager.Instance.isOtherPlayLeft)
                {
                    scoreTxt[1].text = "";
                    winTxt[1].text = "Left";
                }
                else
                {
                    scoreTxt[1].text ="Points : " +  CarromSocketManager.Instance.playerScoreCnt2.ToString();
                    winTxt[1].text = "₹" + 0;
                }

                leaderData2.userId = DataManager.Instance.joinPlayerDatas[secondIndex].userId;
                leaderData2.userName = DataManager.Instance.joinPlayerDatas[secondIndex].userName;
                leaderData2.lobbyId = DataManager.Instance.joinPlayerDatas[secondIndex].lobbyId;
                leaderData2.roomName = TestSocketIO.Instace.roomid;
                leaderData2.entryAmount = (DataManager.Instance.tourEntryMoney / 1);
                leaderData2.winAmount = 0;
                if (BotManager.Instance.isConnectBot)
                {
                    if (!leaderData2.userId.Equals(DataManager.Instance.playerData._id))
                    {
                        leaderData2.isBot = true;
                    }
                }


                leaderData1.gameName = gameName;
                leaderData2.gameName = gameName;

                winDataSend.matchWinLeaderDatas.Add(leaderData1);
                winDataSend.matchWinLeaderDatas.Add(leaderData2);

                string sendWinJson = JsonUtility.ToJson(winDataSend);
                string tmpStatus = "";
                if (playerWin == 1)
                {
                    tmpStatus = "tie";
                }
                else if (playerWin == 2)
                {
                    tmpStatus = "win";
                }
                DataManager.Instance.SendLeaderBoardData(leaderData1.userId, leaderData1.winAmount, leaderData1.lobbyId, 1, leaderData1.roomName, "Win " + gameName + "-" + TestSocketIO.Instace.roomid, sendWinJson, tmpStatus);
            }
            else if (playerWin == 3)
            {
                int getIndex = 0;
                if (DataManager.Instance.playerNo == 2)
                {
                    getIndex = 1;
                }

                //profileImgMain.sprite = profileSprite[DataManager.Instance.joinPlayerDatas[getIndex].avtar];
                //profileImg[1].sprite = profileSprite[DataManager.Instance.joinPlayerDatas[getIndex].avtar];

                //StartCoroutine(GetImages(DataManager.Instance.joinPlayerDatas[getIndex].pPicture, profileImgMain));
                //StartCoroutine(GetImages(DataManager.Instance.joinPlayerDatas[getIndex].pPicture, profileImg[1]));

                profileNameTxt[1].text = UserNameStringManage(DataManager.Instance.joinPlayerDatas[getIndex].userName);

                scoreTxt[1].text ="Points : " +  CarromSocketManager.Instance.playerScoreCnt1.ToString();
                winTxt[1].text = "₹" + 0;

                leaderData1.userId = DataManager.Instance.joinPlayerDatas[getIndex].userId;
                leaderData1.userName = DataManager.Instance.joinPlayerDatas[getIndex].userName;
                leaderData1.lobbyId = DataManager.Instance.joinPlayerDatas[getIndex].lobbyId;
                leaderData1.roomName = TestSocketIO.Instace.roomid;
                leaderData1.entryAmount = (DataManager.Instance.tourEntryMoney / 1);
                leaderData1.winAmount = 0;
                if (BotManager.Instance.isConnectBot)
                {
                    if (!leaderData1.userId.Equals(DataManager.Instance.playerData._id))
                    {
                        leaderData1.isBot = true;
                    }
                }


                int secondIndex = 0;
                if (getIndex == 0)
                {
                    secondIndex = 1;
                }
                else if (getIndex == 1)
                {
                    secondIndex = 0;
                }
                //profileImg[0].sprite = profileSprite[DataManager.Instance.joinPlayerDatas[secondIndex].avtar];

                //StartCoroutine(GetImages(DataManager.Instance.joinPlayerDatas[secondIndex].pPicture, profileImg[0]));
                profileNameTxt[0].text = UserNameStringManage(DataManager.Instance.joinPlayerDatas[secondIndex].userName);

                scoreTxt[0].text ="Points : " +  CarromSocketManager.Instance.playerScoreCnt2.ToString();
                winTxt[0].text = "₹" + DataManager.Instance.winAmount;

                leaderData2.userId = DataManager.Instance.joinPlayerDatas[secondIndex].userId;
                leaderData2.userName = DataManager.Instance.joinPlayerDatas[secondIndex].userName;
                leaderData2.lobbyId = DataManager.Instance.joinPlayerDatas[secondIndex].lobbyId;
                leaderData2.roomName = TestSocketIO.Instace.roomid;
                leaderData2.entryAmount = (DataManager.Instance.tourEntryMoney / 1);
                leaderData2.winAmount = (DataManager.Instance.winAmount / 1);
                if (BotManager.Instance.isConnectBot)
                {
                    if (!leaderData2.userId.Equals(DataManager.Instance.playerData._id))
                    {
                        leaderData2.isBot = true;
                    }
                }

                leaderData1.gameName = gameName;
                leaderData2.gameName = gameName;

                winDataSend.matchWinLeaderDatas.Add(leaderData1);
                winDataSend.matchWinLeaderDatas.Add(leaderData2);

                string sendWinJson = JsonUtility.ToJson(winDataSend);
                DataManager.Instance.SendLeaderBoardData(leaderData2.userId, leaderData2.winAmount, leaderData2.lobbyId, 2, leaderData2.roomName, "Win " + gameName + "-" + TestSocketIO.Instace.roomid, sendWinJson, "loss");
            }

        }

    }

    #endregion*/
    
    
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
            if (name.Length > 7)
            {
                name = name.Substring(0, 5) + "...";
            }
            else
            {
                name = name;
            }
        }
        return name;
    }
    public void HomeButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        DataReset();
        TestSocketIO.Instace.LeaveRoom();
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        SceneManager.LoadScene("Main");
        SoundManager.Instance.StartBackgroundMusic();
    }

    public void PayAgainButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        DataReset();
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        SceneManager.LoadScene("Main");
    }

    void DataReset()
    {
        DataManager.Instance.tournamentID = "";
        DataManager.Instance.tourEntryMoney = 0;
        DataManager.Instance.tourCommision = 0;
        DataManager.Instance.commisionAmount = 0;
        DataManager.Instance.orgIndexPlayer = 0;
        DataManager.Instance.joinPlayerDatas.Clear();
        TestSocketIO.Instace.roomid = "";
        TestSocketIO.Instace.userdata = "";
        TestSocketIO.Instace.playTime = 0;
        BotManager.Instance.isBotAvalible = false;
        BotManager.Instance.isConnectBot = false;
    }
}
