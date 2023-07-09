using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class TeenPattiPlayer : MonoBehaviour
{
    public Image avatarImg;
    public Text playerNameTxt;
    public Image cardImg1;
    public Image cardImg2;
    public Image cardImg3;
    public GameObject delearObj;

    public GameObject[] seeObj;
    public GameObject[] playerWinObj;
    public Image fillLine;

    public GameObject seenImg;
    public GameObject packImg;

    public int playerNo;
    public GameObject sendBetObj;

    public int sumOfCards = 0;

    public bool isTurn;
    public bool isOneTimeEnter;

    public bool isPack;
    public bool isSeen;
    public bool isBlind;

    public string playerId;
    public string lobbyId;

    public CardSuffle card1;
    public CardSuffle card2;
    public CardSuffle card3;

    public int ruleNo;
    public string avatar;

    public bool isBot;
    public bool isCalled;
    private bool _isFunctionCalled;
    public int userTurnCount;
    public GameObject[] boxArray;
    public int inactiveCount = 0;


    // Start is called before the first frame update
    void Start()
    {
        isCalled = false;
        isBot = false;
        _isFunctionCalled = false;
        userTurnCount = 0;
    }

    public void UpdateAvatar()
    {

        StartCoroutine(DataManager.Instance.GetImages(avatar, avatarImg));
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        //if (TeenPattiManager.Instance.player1 == this)
        //{
        //    TeenPattiManager.Instance. = 
        //}
        if(!TeenPattiManager.Instance.isBotActivate) return;
        
        if (playerWinObj[0].activeSelf == true && TeenPattiManager.Instance.isWin == false)
        {
            TeenPattiManager.Instance.isWin = true;
        }
        else if (playerWinObj[0].activeSelf == false)
        {
            TeenPattiManager.Instance.isWin = false;
        }
        
        //if(isPack) return;
        if (isTurn && TeenPattiManager.Instance.isWin == false)
        {
            fillLine.fillAmount -= 1.0f / TeenPattiManager.Instance.timerSpeed * Time.deltaTime;
            if (fillLine.fillAmount == 0 && isOneTimeEnter == false)
            {
                isOneTimeEnter = true;
                isTurn = false;
                //if (playerId.Equals(DataManager.Instance.playerData._id))
                //{
                //    isPack = true;
                //    isBlind = false;
                //    isSeen = false;
                //    for (int j = 0; j < seeObj.Length; j++)
                //    {
                //        seeObj[j].SetActive(false);
                //    }
                //    packImg.SetActive(true);
                //    TeenPattiManager.Instance.CheckWin();
                //}
                //else
                //{
                userTurnCount++;
                if (userTurnCount >= 4)
                { 
                    TeenPattiManager.Instance.ChangeCardStatus("PACK", playerNo);
                }
                CheckLife();
                //}
                TeenPattiManager.Instance.ChangePlayerTurn(playerNo);
                //if (this == TeenPattiManager.Instance.player1)
                //{
                //}
                //Pack and Change Turn
            }

            if (isCalled) return;
            if (!isTurn || !isBot) return;
            if (!TeenPattiManager.Instance.isAdmin) return;
            StartCoroutine(CallBotFunction());
            print("---------------------------Bot is called-----------------------------------");
        }
    }

    private void CheckLife()
    {
        if (inactiveCount >= boxArray.Length)
        {
            // All game objects are already inactive, do nothing
            return;
        }
    
        boxArray[boxArray.Length - 1 - inactiveCount].SetActive(false);
        inactiveCount++;
    }


    public void SetActiveTrue()
    {
        foreach (var t in boxArray)
        {
            t.gameObject.SetActive(true);
        }
    }
    
    
    
    private IEnumerator CallBotFunction()
    {
        StartCoroutine(BotTurn());
        yield return new WaitForFixedUpdate();
        isCalled = true;
    }


    private IEnumerator BotTurn()
    {
        yield return new WaitForSeconds(3f);
        isOneTimeEnter = true;
        isTurn = false;
        if (_isFunctionCalled) yield break;
        BotAutoBet();
        //TeenPattiManager.Instance.BetAnim(this, 0.1f);
    }

    private void BotAutoBet()
    {
        int num = Random.Range(1, 6);
        
        SendBotBetNo(num, playerNo);

        switch (TeenPattiManager.Instance.roundCounter)
        {
            //after 1st round
            case <= 1:
                switch (num)
                {
                    case 1:
                    {
                        if(isPack) return;
                        if (!isSeen)
                        {
                            TeenPattiManager.Instance.ChangeCardStatus("SEEN", playerNo);
                        }
                        TeenPattiManager.Instance.BetAnim(this, TeenPattiManager.Instance.currentPriceValue, TeenPattiManager.Instance.currentPriceIndex);
                        SoundManager.Instance.ThreeBetSound();
                        break;
                    }
                    case 2:
                    {
                        if(isPack) return;
                        if (!isSeen)
                        {
                            TeenPattiManager.Instance.ChangeCardStatus("SEEN", playerNo);
                        }
                        TeenPattiManager.Instance.BetAnim(this, TeenPattiManager.Instance.currentPriceValue, TeenPattiManager.Instance.currentPriceIndex);
                        SoundManager.Instance.ThreeBetSound();
                        break;
                    }
                    case 3:
                    {
                        if(isPack) return;
                        if (!isSeen)
                        {
                            TeenPattiManager.Instance.ChangeCardStatus("SEEN", playerNo);
                        }
                        TeenPattiManager.Instance.BetAnim(this, TeenPattiManager.Instance.currentPriceValue, TeenPattiManager.Instance.currentPriceIndex);
                        SoundManager.Instance.ThreeBetSound();
                        break;
                    }
                    case 4:
                    {
                        if(isPack) return;
                        TeenPattiManager.Instance.BetAnim(this, TeenPattiManager.Instance.currentPriceValue, TeenPattiManager.Instance.currentPriceIndex);
                        SoundManager.Instance.ThreeBetSound();
                        break;
                    }
                    case 5:
                    {
                        if(isPack) return;
                        if (!isSeen)
                        {
                            TeenPattiManager.Instance.ChangeCardStatus("SEEN", playerNo);
                        }
                        TeenPattiManager.Instance.BetAnim(this, TeenPattiManager.Instance.currentPriceValue, TeenPattiManager.Instance.currentPriceIndex);
                        SoundManager.Instance.ThreeBetSound();
                        break;
                    }
                }

                break;
            case 2:
                switch (num)
                {
                    case 1:
                    {
                        if(isPack) return;
                        if (!isSeen)
                        {
                            TeenPattiManager.Instance.ChangeCardStatus("SEEN", playerNo);
                        }
                        TeenPattiManager.Instance.BetAnim(this, TeenPattiManager.Instance.currentPriceValue, TeenPattiManager.Instance.currentPriceIndex);
                        SoundManager.Instance.ThreeBetSound();
                        break;
                    }
                    case 2:
                    {
                        if(isPack) return;
                        if (!isSeen)
                        {
                            TeenPattiManager.Instance.ChangeCardStatus("SEEN", playerNo);
                        }
                        TeenPattiManager.Instance.BetAnim(this, TeenPattiManager.Instance.currentPriceValue, TeenPattiManager.Instance.currentPriceIndex);
                        SoundManager.Instance.ThreeBetSound();
                        break;
                    }
                    case 3:
                    {
                        if(isPack) return;
                        if (!isSeen)
                        {
                            TeenPattiManager.Instance.ChangeCardStatus("SEEN", playerNo);
                        }
                        TeenPattiManager.Instance.BetAnim(this, TeenPattiManager.Instance.currentPriceValue, TeenPattiManager.Instance.currentPriceIndex);
                        SoundManager.Instance.ThreeBetSound();
                        break;
                    }
                    case 4:
                    {
                        if(isPack) return;
                        if (!isSeen)
                        {
                            TeenPattiManager.Instance.ChangeCardStatus("SEEN", playerNo);
                        }
                        TeenPattiManager.Instance.BetAnim(this, TeenPattiManager.Instance.currentPriceValue, TeenPattiManager.Instance.currentPriceIndex);
                        SoundManager.Instance.ThreeBetSound();
                        break;
                    }
                    case 5:
                    {
                        if(isPack) return;
                        TeenPattiManager.Instance.ChangeCardStatus("PACK", playerNo);
                        //TeenPattiManager.Instance.BetAnim(this, 5f);
                        break;
                    }
                }

                break;
            
            case 3:
                switch (num)
                {
                    case 1:
                    {
                        if(isPack) return;
                        TeenPattiManager.Instance.ChangeCardStatus("PACK", playerNo);
                        break;
                    }
                    case 2:
                    {
                        if(isPack) return;
                        if (!isSeen)
                        {
                            TeenPattiManager.Instance.ChangeCardStatus("SEEN", playerNo);
                        }
                        TeenPattiManager.Instance.BetAnim(this, TeenPattiManager.Instance.currentPriceValue, TeenPattiManager.Instance.currentPriceIndex);
                        SoundManager.Instance.ThreeBetSound();
                        break;
                    }
                    case 3:
                    {
                        if(isPack) return;
                        if (!isSeen)
                        {
                            TeenPattiManager.Instance.ChangeCardStatus("SEEN", playerNo);
                        }
                        TeenPattiManager.Instance.BetAnim(this, TeenPattiManager.Instance.currentPriceValue, TeenPattiManager.Instance.currentPriceIndex);
                        SoundManager.Instance.ThreeBetSound();
                        break;
                    }
                    case 4:
                    {
                        if(isPack) return;
                        if (!isSeen)
                        {
                            TeenPattiManager.Instance.ChangeCardStatus("SEEN", playerNo);
                        }
                        TeenPattiManager.Instance.BetAnim(this, TeenPattiManager.Instance.currentPriceValue, TeenPattiManager.Instance.currentPriceIndex);
                        SoundManager.Instance.ThreeBetSound();
                        break;
                    }
                    case 5:
                    {
                        if(isPack) return;
                        TeenPattiManager.Instance.ChangeCardStatus("PACK", playerNo);
                        //TeenPattiManager.Instance.BetAnim(this, 5f);
                        break;
                    }
                }

                break;
            case >= 4:
                switch (num)
                {
                    case 1:
                    {
                        if(isPack) return;
                        TeenPattiManager.Instance.BetAnim(this, TeenPattiManager.Instance.currentPriceValue, TeenPattiManager.Instance.currentPriceIndex);
                        SoundManager.Instance.ThreeBetSound();
                        break;
                    }
                    case 2:
                    {
                        if(isPack) return;
                        TeenPattiManager.Instance.BetAnim(this, TeenPattiManager.Instance.currentPriceValue, TeenPattiManager.Instance.currentPriceIndex);
                        SoundManager.Instance.ThreeBetSound();
                        break;
                    }
                    case 3:
                    {
                        if(isPack) return;
                        TeenPattiManager.Instance.ChangeCardStatus("PACK", playerNo);
                        break;
                    }
                    case 4:
                    {
                        if(isPack) return;
                        TeenPattiManager.Instance.BetAnim(this, TeenPattiManager.Instance.currentPriceValue, TeenPattiManager.Instance.currentPriceIndex);
                        SoundManager.Instance.ThreeBetSound();
                        break;
                    }
                    case 5:
                    {
                        if(isPack) return;
                        TeenPattiManager.Instance.BetAnim(this, TeenPattiManager.Instance.currentPriceValue, TeenPattiManager.Instance.currentPriceIndex);
                        SoundManager.Instance.ThreeBetSound();
                        break;
                    }
                }

                break;
        }
        TeenPattiManager.Instance.ChangePlayerTurn(playerNo);

        _isFunctionCalled = true;
    }

    public void SumOfPlayerCards()
    {
        sumOfCards = card1.cardNo + card2.cardNo + card3.cardNo;
    }

   

    public void CardGenerate()
    {
        cardImg1.sprite = TeenPattiManager.Instance.simpleCardSprite;
        cardImg2.sprite = TeenPattiManager.Instance.simpleCardSprite;
        cardImg3.sprite = TeenPattiManager.Instance.simpleCardSprite;
        int startIndex = (playerNo - 1) * 3;
        if (startIndex >= 0)
        {
            card1 = new CardSuffle();
            card2 = new CardSuffle();
            card3 = new CardSuffle();
            print("Start Index : " + startIndex);
            card1 = TeenPattiManager.Instance.cardSuffles[TeenPattiManager.Instance.mainList[startIndex] - 1];
            card2 = TeenPattiManager.Instance.cardSuffles[TeenPattiManager.Instance.mainList[startIndex + 1] - 1];
            card3 = TeenPattiManager.Instance.cardSuffles[TeenPattiManager.Instance.mainList[startIndex + 2] - 1];
            
            print("This is card1 no  -> " + (TeenPattiManager.Instance.mainList[startIndex] - 1));
            print("This is card2 no  -> " + (TeenPattiManager.Instance.mainList[startIndex + 1] - 1));
            print("This is card3 no  -> " + (TeenPattiManager.Instance.mainList[startIndex + 2] - 1));

            TeenPattiWinMaintain winMaintain = TeenPattiManager.Instance.MatchResult(card1, card2, card3);
            ruleNo = winMaintain.ruleNo;
            if (winMaintain.ruleNo == 1 || winMaintain.ruleNo == 5)
            {
                card1 = winMaintain.winList[0];
                card2 = winMaintain.winList[1];
                card3 = winMaintain.winList[2];
            }
            if (winMaintain.ruleNo == 2 || winMaintain.ruleNo == 3 || winMaintain.ruleNo == 4 || winMaintain.ruleNo == 6)
            {
                card1 = winMaintain.winList[0];
                card2 = winMaintain.winList[1];
                card3 = winMaintain.winList[2];
            }
        }

    }


    public void CardDisplay()
    {
        cardImg1.sprite = card1.cardSprite;
        cardImg2.sprite = card2.cardSprite;
        cardImg3.sprite = card3.cardSprite;
    }
    
    
    public void CardPackDisplay()
    {
        card1.cardSprite = TeenPattiManager.Instance.packCardSprite;
        card2.cardSprite = TeenPattiManager.Instance.packCardSprite;
        card3.cardSprite = TeenPattiManager.Instance.packCardSprite;
    }

    public void SendBotBetNo(int no, int botPlayerNo)
    {
        JSONObject obj = new JSONObject();
        obj.AddField("PlayerID", DataManager.Instance.playerData._id);
        obj.AddField("TournamentID", DataManager.Instance.tournamentID);
        obj.AddField("RoomId", DataManager.Instance.gameId);
        obj.AddField("BotPlayerNo", botPlayerNo);
        obj.AddField("BotNo", no);
        obj.AddField("Action", "BotBetData");
        TestSocketIO.Instace.Senddata("TeenPattiBotBetNo", obj);
    }




    public void RestartFillLine()
    {
        //TeenPattiManager.Instance.ShowTextChange();
        fillLine.fillAmount = 1;
        isOneTimeEnter = false;
        isTurn = true;
        isCalled = false;
        _isFunctionCalled = false;
        //if (this == TeenPattiManager.Instance.player1)
        //{
        //    TeenPattiManager.Instance.bottomBox.SetActive(true);
        //}

    }
    public void NotATurn()
    {
        isOneTimeEnter = false;
        isTurn = false;
        _isFunctionCalled = false;
        fillLine.fillAmount = 1;
    }
}
