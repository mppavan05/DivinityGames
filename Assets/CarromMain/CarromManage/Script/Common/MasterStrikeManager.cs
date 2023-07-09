using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MasterStrikeManager : MonoBehaviour
{
    private class Reward
    {
        public RewardType type;

        public RewardMode mode;

        public int amount;

        public Reward()
        {
            amount = 0;
            type = RewardType.NONE;
        }
    }

    private enum RewardType
    {
        NONE = 0,
        COINS = 1,
        GEMS = 2
    }

    private enum RewardMode
    {
        INITIAL = 0,
        BUY = 1,
        EARLY = 2
    }

    public const int BOUGHT_MASTER_STRIKER = 3;

    public int STRIKE_CHANCE = 2;

    public int STRIKE_CHANCE_EARLY = 5;

    public const int MAX_COINS = 7500;

    public const int MAX_GEMS = 10;

    public const int MIN_COINS = 500;

    public const string CHANCE = "chance";

    public static string KEY_TIME = "master_strike_time";

    public Transform[] firstPucks;

    public Transform[] secondPucks;

    public Transform[] thirdPucks;

    public List<SpriteRenderer> chanceSprites;

    public List<SpriteRenderer> chanceExtras;

    public Color chanceAvaibale;

    public Color chanceUnAvaibale;

    public GameObject strikerMoverDown;

    public GameObject playerStriker;

    public GameObject buyChancePanel;

    public GameObject masterPanelRetry;

    public GameObject helpPanel;

    public GameObject timerPanel;

    public GameObject earlyAccessPanel;

    public GameObject rewardedPanel;

    public GameObject buyEarlyAcessButton;

    public GameObject buyChanceButton;

    public GameObject overflowInfoPanel;

    public GameObject puck;

    public Text chanceMessage;

    public Text helpMessage;

    public Text timer;

    public Text earlyBuyMessage;

    public Text playText;

    public Text earlyAccessPriceText;

    public Text chancePriceText;

    public Text maxCoinsText;

    public Text maxGemText;

    public Text minCoinsText;

    private StrikerMover downStrikerMover;

    private List<Rigidbody2D> pucks;

    public static MasterStrikeManager Instance;

    private int chance;

    private int goaled;

    private int minCoins = 500;

    private int maxCoins = 7500;

    private int maxGems = 25;

    public OfflineStriker striker;

    private Reward reward;

    private long lasttime;

    public Text initialMaxCoinsText;

    public Text initialMaxGemText;

    public Text initialMinCoinsText;

    public Text buyMaxCoinsText;

    public Text buyMaxGemText;

    public Text buyMinCoinsText;

    public Text earlyMaxCoinsText;

    public Text earlyMaxGemText;

    public Text earlyMinCoinsText;

    public Rigidbody2D ballRB;

    private bool isShownRewarded;

    public Text rewardMessage;

    private DateTime tomorrow;

    private bool showHelpTimerPanel = true;

    public long waitMiliSeconds = 86400000L;

    public Text overFlowMaxCoinsText;

    public Text overFlowMaxGemText;

    public Text overFlowMinCoinsText;

    public bool canMoveStriker = true;

    private void Start()
    {
        Instance = this;
        STRIKE_CHANCE = LevelManager.getInstance().initialChance;
        STRIKE_CHANCE_EARLY = LevelManager.getInstance().earlyAccessChance;
        reward = new Reward();
        lasttime = long.Parse(PlayerPrefs.GetString(KEY_TIME, "636913312439118356"));
        pucks = new List<Rigidbody2D>();
        setStrikerSprites();
        pucks.Add(UnityEngine.Object.Instantiate(puck, firstPucks[UnityEngine.Random.Range(0, firstPucks.Length)].position, Quaternion.identity).GetComponent<Rigidbody2D>());
        pucks.Add(UnityEngine.Object.Instantiate(puck, secondPucks[UnityEngine.Random.Range(0, secondPucks.Length)].position, Quaternion.identity).GetComponent<Rigidbody2D>());
        pucks.Add(UnityEngine.Object.Instantiate(puck, thirdPucks[UnityEngine.Random.Range(0, thirdPucks.Length)].position, Quaternion.identity).GetComponent<Rigidbody2D>());
        chance = pucks.Count;
        goaled = 0;
        striker.canStrike = false;
        //chanceMessage.text = GetChancePrice();
        //earlyBuyMessage.text = GetEarlyAccessPrice();
        helpMessage.text = "Plot all coins using " + chance + " strike";
        if (isMasterStrikeReady())
        {
            timerPanel.SetActive(false);
            helpPanel.SetActive(true);
            SetInitialRewards();
        }

    }

    private void setStrikerSprites()
    {
        downStrikerMover = strikerMoverDown.GetComponent<StrikerMover>();
        int @int = PlayerPrefs.GetInt("striker", 0);
        downStrikerMover.SetStriker(playerStriker, @int);
        Sprite sprite = ((@int >= LevelManager.instance.strikers.Length) ? LevelManager.instance.strikers[0] : LevelManager.instance.strikers[@int]);
        playerStriker.GetComponent<SpriteRenderer>().sprite = sprite;
        foreach (SpriteRenderer chanceSprite in chanceSprites)
        {
            chanceSprite.sprite = sprite;
        }
        foreach (SpriteRenderer chanceExtra in chanceExtras)
        {
            chanceExtra.sprite = sprite;
        }
    }

    private void SetInitialRewards()
    {
        reward.mode = RewardMode.INITIAL;
        initialMaxCoinsText.text = LevelManager.instance.initialMaxCoins.ToString();
        initialMaxGemText.text = LevelManager.instance.initilaMaxGems.ToString();
        initialMinCoinsText.text = LevelManager.instance.initialMinCoins.ToString();
        SetOverFlowInfo(LevelManager.instance.initialMaxCoins, LevelManager.instance.initilaMaxGems, LevelManager.instance.initialMinCoins);
    }

    private void SetBuyRewards()
    {
        reward.mode = RewardMode.BUY;
        buyMaxCoinsText.text = LevelManager.instance.buyMaxCoins.ToString();
        buyMaxGemText.text = LevelManager.instance.buyMaxGems.ToString();
        buyMinCoinsText.text = LevelManager.instance.buyMinCoins.ToString();
        SetOverFlowInfo(LevelManager.instance.buyMaxCoins, LevelManager.instance.buyMaxGems, LevelManager.instance.buyMinCoins);
    }

    private void SetEarlyAccessRewards()
    {
        reward.mode = RewardMode.EARLY;
        earlyMaxCoinsText.text = LevelManager.instance.earlyMaxCoins.ToString();
        earlyMaxGemText.text = LevelManager.instance.earlyMaxGems.ToString();
        earlyMinCoinsText.text = LevelManager.instance.earlyMinCoins.ToString();
        SetOverFlowInfo(LevelManager.instance.earlyMaxCoins, LevelManager.instance.earlyMaxGems, LevelManager.instance.earlyMinCoins);
    }

    public bool AllPucksStopped()
    {
        foreach (Rigidbody2D puck in pucks)
        {
            if (puck != null && puck.velocity.magnitude > 0f)
            {
                return canMoveStriker = false;
            }
        }
        if (ballRB.velocity.magnitude > 0f)
        {
            return canMoveStriker = false;
        }
        downStrikerMover.AnimateStrikerHandlerToCenter();
        return canMoveStriker = true;
    }

    public void GoaledPuck(GameObject puck)
    {
        goaled++;
        UnityEngine.Object.Destroy(puck, 1f);
    }

    public void FinishedChance()
    {
        chance--;
        if (chance < chanceSprites.Count)
        {
            chanceSprites[chance].color = chanceUnAvaibale;
        }
        PlayerPrefs.SetInt("chance", chance);
    }

    public void IsMasterStrikeOver()
    {
        if (chance != 0 && goaled != pucks.Count)
        {
            return;
        }
        striker.canStrike = false;
        HideOverFlowInfoPanel();
        if (goaled == pucks.Count)
        {
            Debug.Log("Master Striker " + goaled);
            ShowUserRewards();
            return;
        }
        Debug.Log("Non Master Striker" + goaled);
        PlayerPrefs.GetInt("master_stiker", 0);
        if (reward.mode == RewardMode.EARLY)
        {
            SetEarlyAccessRewards();
            earlyAccessPanel.SetActive(true);
        }
        else
        {
            SetBuyRewards();
            //chanceMessage.text = GetChancePrice();
            buyChancePanel.SetActive(true);
        }
    }

    public void GoToHome()
    {
        AudioManager.getInstance().PlaySound(AudioManager.PLAY_CLICK);
        SceneManager.LoadScene("menu");
    }

    public void CloseFinishedPanel()
    {
        reward.mode = RewardMode.INITIAL;
        ShowUserRewards();
        buyChancePanel.SetActive(false);
        //chancePriceText.text = GetChancePrice();
        buyChanceButton.SetActive(true);
        AudioManager.getInstance().PlaySound(AudioManager.PLAY_CLICK);
    }

    public void OpenChancePanel()
    {
        AudioManager.getInstance().PlaySound(AudioManager.PLAY_CLICK);
        //chanceMessage.text = GetChancePrice();
        buyChancePanel.SetActive(true);
        buyChanceButton.SetActive(false);
    }

    private void ShowUserRewards()
    {
        if (goaled != 0 && reward.amount != 0 && !isShownRewarded)
        {
            isShownRewarded = true;
            switch (reward.type)
            {
                case RewardType.COINS:
                    rewardMessage.text = getCoinsMessage(reward.amount);
                    break;
                case RewardType.GEMS:
                    rewardMessage.text = getGemsMessage(reward.amount);
                    break;
            }
            if (goaled == 3)
            {
                AudioManager.getInstance().PlaySound(AudioManager.GAME_OVER);
            }
            else
            {
                AudioManager.getInstance().PlaySound(AudioManager.PLAY_WIN);
            }
            rewardedPanel.SetActive(true);
        }
    }

    public void SetPlayerReward()
    {
        switch (reward.mode)
        {
            case RewardMode.INITIAL:
                SetPlayerInitialReward();
                break;
            case RewardMode.BUY:
                SetPlayerBoughtReward();
                break;
            case RewardMode.EARLY:
                SetPlayerEarlyReward();
                break;
        }
    }

    private void SetPlayerInitialReward()
    {
        switch (goaled)
        {
            case 1:
                reward.amount = LevelManager.instance.initialMinCoins;
                reward.type = RewardType.COINS;
                break;
            case 2:
                reward.amount = LevelManager.instance.initilaMaxGems;
                reward.type = RewardType.GEMS;
                break;
            case 3:
                reward.amount = LevelManager.instance.initialMaxCoins;
                reward.type = RewardType.COINS;
                break;
        }
    }

    private void SetPlayerBoughtReward()
    {
        switch (goaled)
        {
            case 1:
                reward.amount = LevelManager.instance.buyMinCoins;
                reward.type = RewardType.COINS;
                break;
            case 2:
                reward.amount = LevelManager.instance.buyMaxGems;
                reward.type = RewardType.GEMS;
                break;
            case 3:
                reward.amount = LevelManager.instance.buyMaxCoins;
                reward.type = RewardType.COINS;
                break;
        }
    }

    private void SetPlayerEarlyReward()
    {
        switch (goaled)
        {
            case 1:
                reward.amount = LevelManager.instance.earlyMinCoins;
                reward.type = RewardType.COINS;
                break;
            case 2:
                reward.amount = LevelManager.instance.earlyMaxGems;
                reward.type = RewardType.GEMS;
                break;
            case 3:
                reward.amount = LevelManager.instance.earlyMaxCoins;
                reward.type = RewardType.COINS;
                break;
        }
    }

    private string getCoinsMessage(int coins)
    {
        return "You have earned " + coins + " coins";
    }

    private string getGemsMessage(int gems)
    {
        return "You have earned " + gems + " gems";
    }

    public void BuyMoreRounds()
    {
        AudioManager.getInstance().PlaySound(AudioManager.PLAY_CLICK);
        //MonetizeManager.Instance.BuyMoreMasterStrike();
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnPurchasedMasterStriker()
    {
        Debug.Log("OnPurchasedMasterStriker");
        isShownRewarded = false;
        chance = STRIKE_CHANCE;
        reward.mode = RewardMode.BUY;
        int num = 0;
        foreach (SpriteRenderer chanceSprite in chanceSprites)
        {
            if (num < chance)
            {
                chanceSprite.color = chanceAvaibale;
            }
            num++;
        }
        striker.canStrike = true;
        buyChancePanel.SetActive(false);
        ShowOverFlowInfoPanel();
    }

    public void OnPurchasedMasterStrikerEarly()
    {
        isShownRewarded = false;
        Debug.Log("OnPurchasedMasterStrikerEarly");
        showHelpTimerPanel = false;
        chance = STRIKE_CHANCE_EARLY;
        if (chanceSprites.Count <= 3)
        {
            chanceSprites.Insert(0, chanceExtras[0]);
            chanceSprites.Insert(chanceSprites.Count, chanceExtras[1]);
        }
        int num = 0;
        foreach (SpriteRenderer chanceSprite in chanceSprites)
        {
            if (num < chance)
            {
                chanceSprite.color = chanceAvaibale;
                chanceSprite.gameObject.SetActive(true);
            }
            num++;
        }
        earlyAccessPanel.SetActive(false);
        buyChancePanel.SetActive(false);
        timerPanel.SetActive(false);
        playText.text = "Play";
        AllowContinuePlay();
        ShowOverFlowInfoPanel();
    }

    private void AddTimer()
    {
        tomorrow = DateTime.Now.Date.AddDays(1.0);
    }

    private void Update()
    {
        if (showHelpTimerPanel && !isMasterStrikeReady())
        {
            if (!timerPanel.activeSelf)
            {
                SetEarlyAccessRewards();
                earlyAccessPanel.SetActive(true);
                timerPanel.SetActive(true);
                helpPanel.SetActive(false);
            }
            showTimer();
        }
    }

    private bool isMasterStrikeReady()
    {
        long ticks = DateTime.Now.Ticks;
        if (waitMiliSeconds - (ticks - lasttime) / 10000 < 0)
        {
            if (timerPanel.activeSelf)
            {
                timerPanel.SetActive(false);
                earlyAccessPanel.SetActive(false);
                showHelpPanel();
            }
            return true;
        }
        return false;
    }

    public void PlayFree()
    {
        AudioManager.getInstance().PlaySound(AudioManager.PLAY_CLICK);
        AllowContinuePlay();
        helpPanel.SetActive(false);
        ShowOverFlowInfoPanel();
    }

    private void AllowContinuePlay()
    {
        striker.canStrike = true;
        PlayerPrefs.SetString(KEY_TIME, DateTime.Now.Ticks.ToString());
        showHelpTimerPanel = false;
    }

    private void showHelpPanel()
    {
        helpPanel.SetActive(true);
        helpMessage.text = "Plot all coins using " + chance + " strike";
        maxCoinsText.text = maxCoins.ToString();
        maxGemText.text = maxGems.ToString();
        minCoinsText.text = minCoins.ToString();
    }

    private void showTimer()
    {
        long ticks = DateTime.Now.Ticks;
        long num = waitMiliSeconds - (ticks - lasttime) / 10000;
        string text = num / 3600000 + "h ";
        num -= num / 3600000 * 3600000;
        text = text + (num / 60000).ToString("00") + "m ";
        text = text + (num % 60000 / 1000).ToString("00") + "s";
        timer.text = text;
    }

    public void BuyPaidShots()
    {
        AudioManager.getInstance().PlaySound(AudioManager.PLAY_CLICK);
        //MonetizeManager.Instance.BuyMasterStrikeEarly();
    }

    public void CloseEarlyAccessPanel()
    {
        AudioManager.getInstance().PlaySound(AudioManager.PLAY_CLICK);
        earlyAccessPanel.SetActive(false);
        buyEarlyAcessButton.SetActive(true);
        ShowUserRewards();
        //earlyAccessPriceText.text = GetEarlyAccessPrice();
    }

    //private string GetEarlyAccessPrice()
    //{
    //    return STRIKE_CHANCE_EARLY + " strike for " + MonetizeManager.Instance.earlyPrice;
    //}

    //private string GetChancePrice()
    //{
    //    return STRIKE_CHANCE + " strike for " + MonetizeManager.Instance.chance;
    //}

    public void OpenEarlyAccessPanel()
    {
        AudioManager.getInstance().PlaySound(AudioManager.PLAY_CLICK);
        buyEarlyAcessButton.SetActive(false);
        earlyAccessPanel.SetActive(true);
    }

    public void CollectReward()
    {
        rewardedPanel.SetActive(false);
        AudioManager.getInstance().PlaySound(AudioManager.PLAY_COIN_COLLECT);
        if (reward.type == RewardType.COINS)
        {
            //int @int = PlayerPrefs.GetInt("coins", FacebookLogin.DEFAULT_COINS);
            //@int += reward.amount;
            //PlayerPrefs.SetInt("coins", @int);
        }
        else if (reward.type == RewardType.GEMS)
        {
            //int int2 = PlayerPrefs.GetInt("gems", FacebookLogin.DEFAULT_GEMS);
            //int2 += reward.amount;
            //PlayerPrefs.SetInt("gems", int2);
        }
        //LeaderBoardManager instance = LeaderBoardManager.getInstance();
        //if (instance != null)
        //{
        //    Debug.Log("UpdatePlayerDetails");
        //    instance.UpdatePlayerDetails();
        //}

    }

    private void ShowOverFlowInfoPanel()
    {
        overflowInfoPanel.SetActive(true);
    }

    private void HideOverFlowInfoPanel()
    {
        overflowInfoPanel.SetActive(false);
    }

    private void SetOverFlowInfo(int maxcoin, int gems, int mincoin)
    {
        overFlowMaxCoinsText.text = maxcoin.ToString();
        overFlowMaxGemText.text = gems.ToString();
        overFlowMinCoinsText.text = mincoin.ToString();
    }

    public bool CanMoveStriker()
    {
        return canMoveStriker;
    }
}
