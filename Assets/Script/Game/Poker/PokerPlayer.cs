using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;


public class PokerPlayer : MonoBehaviour
{
    public Image avatarImg;
    public Text playerNameTxt;
    public Image cardImg1;
    public Image cardImg2;

    public Text playerBalanceTxt;

    public GameObject[] playerWinObj;
    public Image fillLine;
    public GameObject delearObj;
    public GameObject foldImg;

    public GameObject betObj;
    public float betAmount;
    public float currentBotBetAmount;
    public Text betTxt;


    public int playerNo;

    public string playerId;
    public string lobbyId;


    public int ruleNo;

    public CardSuffle card1;
    public CardSuffle card2;


    public bool isOneTimeEnter = false;
    public bool isTurn = false;
    public bool isFold;
    public bool admin;

    public string avatar;
    public bool isBot;
    public bool isCalled;
    private bool _isFunctionCalled;
    // Start is called before the first frame update
    void Start()
    {
        isCalled = false;
        isBot = false;
        betTxt.text = "0";
        _isFunctionCalled = false;
    }

    public void UpdateAvatar()
    {

        StartCoroutine(DataManager.Instance.GetImages(avatar, avatarImg));
    }
    // Update is called once per frame
    private void FixedUpdate()
    {
        // if(!PokerGameManager.Instance.isBotActivate) return;

        if (!PokerGameManager.Instance.isGameStop) return;
        if (playerWinObj[0].activeSelf == true && PokerGameManager.Instance.isWin == false)
        {
            PokerGameManager.Instance.isWin = true;
        }
        else if (playerWinObj[0].activeSelf == false)
        {
            PokerGameManager.Instance.isWin = false;
        }

        if (isTurn && PokerGameManager.Instance.isWin == false)
        {
            fillLine.fillAmount -= 1.0f / PokerGameManager.Instance.timerSpeed * Time.deltaTime;
            if (fillLine.fillAmount == 0 && isOneTimeEnter == false)
            {
                isOneTimeEnter = true;
                isTurn = false;

                //PokerGameManager.Instance.ChangeCardStatus("PACK", playerNo);
                if (PokerGameManager.Instance.isAdmin == true)
                {
                    admin = true;
                    PokerGameManager.Instance.SendPokerPlayerFold(playerId);
                }
                //}
                PokerGameManager.Instance.ChangePlayerTurn(playerNo);

                //Pack and Change Turn
            }
            if (isCalled) return;
            if(!isTurn || !isBot) return;
            /*if (!TeenPattiManager.Instance.isAdmin) return;
            {
            }*/
            if (!PokerGameManager.Instance.isAdmin) return;
            StartCoroutine(CallBotFunction());
        }
    }
    
    private IEnumerator CallBotFunction()
    {
        StartCoroutine(BotTurn());
        yield return new WaitForFixedUpdate();
        isCalled = true;
    }
    
    public IEnumerator BotTurn()
    {
        yield return new WaitForSeconds(3f);
        if (!PokerGameManager.Instance.isGameStop) yield break;
        isOneTimeEnter = true;
        isTurn = false;
        if (_isFunctionCalled) yield break;
        BotAutoTurn();
        //TeenPattiManager.Instance.BetAnim(this, 0.1f);
    }
    
    public void BotAutoTurn()
    {
        int num = Random.Range(1, 4);
        GetBotBetAmount();
        print(num + "This is the Card Number");
        SendBotBetNo(num, playerNo, currentBotBetAmount);   
        switch (num)
        {
            case 1:
            {
                PokerGameManager.Instance.BetAnim(this, currentBotBetAmount);
                SoundManager.Instance.ThreeBetSound();
                break;
            }
            case 2:
            {
                // if (PokerGameManager.Instance.raisePrice == 100)
                // {
                //     PokerGameManager.Instance.SendPokerBet(playerNo, PokerGameManager.Instance.player1BetAmount, "allin");
                //
                // }
                // else
                // {
                //     PokerGameManager.Instance.SendPokerBet(playerNo, PokerGameManager.Instance.player1BetAmount, "raise");
                // }
                // break;
               // PokerGameManager.Instance.SendPokerBet(playerNo, PokerGameManager.Instance.lastPrice, "call");
               PokerGameManager.Instance.BetAnim(this, currentBotBetAmount);
               SoundManager.Instance.ThreeBetSound();
                break;
            }
            case 3:
            {
                PokerGameManager.Instance.BetAnim(this, currentBotBetAmount);
                SoundManager.Instance.ThreeBetSound();
                break;
            }
            case 4:
            {
                break;
            }
        }
        PokerGameManager.Instance.ChangePlayerTurn(playerNo);
        _isFunctionCalled = true;

       // if (PokerGameManager.Instance.counter == 5)
        //{
       //     PokerGameManager.Instance.WinPoker();
       // }
    }
    
    
    public float GetBotBetAmount()
    {
        float botBetAmount = 0f;

        // Find the maximum bet amount among all players
        float maxBetAmount = PokerGameManager.Instance.playerSquList.Select(player => player.betAmount).Prepend(0f).Max();

        // Calculate the bot's bet amount based on the maximum bet amount and the bot's current bet amount
        if (maxBetAmount > betAmount)
        {
            botBetAmount = maxBetAmount - betAmount;
        }

        if (botBetAmount == 0)
        {
            botBetAmount = 10f;
        }

        currentBotBetAmount = botBetAmount;
        return botBetAmount;
    }
    
    

    public void CardGenerate()
    {

        int startIndex = (playerNo - 1) * 2;
        if (startIndex >= 0)
        {
            card1 = new CardSuffle();
            card2 = new CardSuffle();
            //print("Start Index : " + startIndex);
            card1 = PokerGameManager.Instance.cardSuffles[PokerGameManager.Instance.mainList[startIndex] - 1];
            card2 = PokerGameManager.Instance.cardSuffles[PokerGameManager.Instance.mainList[startIndex + 1] - 1];


            /*if (playerNo == 1)
            {
                cardImg1.sprite = card1.cardSprite;
                cardImg2.sprite = card2.cardSprite;
            }
            else
            {
                cardImg1.sprite = PokerGameManager.Instance.simpleCardSprite;
                cardImg2.sprite = PokerGameManager.Instance.simpleCardSprite;
            }*/
            cardImg1.sprite = PokerGameManager.Instance.simpleCardSprite;
            cardImg2.sprite = PokerGameManager.Instance.simpleCardSprite;

        }


        int startIndex5 = (DataManager.Instance.joinPlayerDatas.Count * 2);
        PokerGameManager.Instance.card1 = PokerGameManager.Instance.cardSuffles[PokerGameManager.Instance.mainList[startIndex5] - 1];
        PokerGameManager.Instance.card2 = PokerGameManager.Instance.cardSuffles[PokerGameManager.Instance.mainList[startIndex5 + 1] - 1];
        PokerGameManager.Instance.card3 = PokerGameManager.Instance.cardSuffles[PokerGameManager.Instance.mainList[startIndex5 + 2] - 1];
        PokerGameManager.Instance.card4 = PokerGameManager.Instance.cardSuffles[PokerGameManager.Instance.mainList[startIndex5 + 3] - 1];
        PokerGameManager.Instance.card5 = PokerGameManager.Instance.cardSuffles[PokerGameManager.Instance.mainList[startIndex5 + 4] - 1];

    }

    public void RestartFillLine()
    {
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
    
    public void DisplayPlayerCard()
    {
        cardImg1.sprite = card1.cardSprite;
        cardImg2.sprite = card2.cardSprite;
    }
    
    public PokerWinDataMaintain CardDisplay()
    {
        cardImg1.sprite = card1.cardSprite;
        cardImg2.sprite = card2.cardSprite;

        return PokerGameManager.Instance.MatchResult(card1, card2, PokerGameManager.Instance.card1, PokerGameManager.Instance.card2, PokerGameManager.Instance.card3, PokerGameManager.Instance.card4, PokerGameManager.Instance.card5);
    }

    public void PlayerSetBet(float amount, string betType)
    {
        PokerGameManager.Instance.BetAnim(this, amount);
    }

    public void PlayerSetSocketBet(float amount, string betType)
    {
        PokerGameManager.Instance.BetAnim(this, amount);
    }
    
    
    public void SendBotBetNo(int no, int botPlayerNo, float botBetAmount)
    {
        JSONObject obj = new JSONObject();
        obj.AddField("TournamentID", DataManager.Instance.tournamentID);
        obj.AddField("RoomId", TestSocketIO.Instace.roomid);
        obj.AddField("BetAmount", botBetAmount);
        obj.AddField("BotPlayerNo", botPlayerNo);
        obj.AddField("BotNo", no);
        obj.AddField("Action", "BotBetData");
        TestSocketIO.Instace.Senddata("PokerBotBetNo", obj);
    }
}
