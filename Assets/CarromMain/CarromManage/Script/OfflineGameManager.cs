using System.Collections.Generic;
//using Photon.Pun.UtilityScripts;
//using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OfflineGameManager : MonoBehaviour//, IPunTurnManagerCallbacks
{
    public class OfflinePlayer
    {
        public PuckColor.Color color;

        public int score;

        public bool playerPutQueen;

        public OfflinePlayer(PuckColor.Color color)
        {
            this.color = color;
        }
    }
    
    public GameObject soundOn;
    public GameObject soundOff;
    public GameObject vibOn;
    public GameObject vibOff;
    public GameObject sfxOn;
    public GameObject sfxOff;
    public GameObject menuScreenObj;
    public GameObject settingsScreenObj;

    private const int FIRST_PLAYER = 0;

    private const int SECOND_PLAYER = 1;

    public Color disabledColor;

    public Color enabledColor;

    public Sprite whiteCoin;

    public Sprite blackCoin;

    public GameObject playerStriker;

    public GameObject opponentStriker;

    public GameObject blackPuck;

    public GameObject whitePuck;

    public GameObject redPuck;

    public GameObject redPuckOffline;

    public GameObject strikerMoverUp;

    public GameObject strikerMoverDown;

    private SpriteRenderer strikerMoverUpRender;

    private SpriteRenderer strikerMoverDownRender;

    public SpriteRenderer strikerStripUpRender;

    public SpriteRenderer strikerStripDownRender;

    private Collider2D strikerMoverUpCollider;

    private Collider2D strikerMoverDownCollider;

    public Text leftScoreView;

    public Text rightScoreView;

    public Image playerCoin;

    public Image opponentCoin;

    public Image playerImageView;

    public Image opponentImageView;

    public Text playerNameView;

    public Text opponentNameView;

    public Text foulDetail;

    private Text playerScoreView;

    private Text opponentScoreView;

    public static OfflineGameManager Instance;

    public GameObject movementPadBottom;

    public GameObject movementPadTop;

    public GameObject gameOverPanel;

    public GameObject gameOverPartilce;

    public GameObject quitPanel;

    public GameObject leftAvatar;

    public GameObject rightAvatar;

    public Animator foulAnimator;

    public Text queenRecoverLeft;

    public Text queenRecoverRight;

    public Transform restPositionTop;

    public Transform restPositionBottom;

    public CarromCoin[] puckPositions;
    public CarromCoin[] puckPositionsGreeSave;

    public List<Puck> pucks = new List<Puck>();

    private OfflinePlayer player;

    private OfflinePlayer opponentPlayer;

    private OfflinePlayer playingPlayer;

    private OfflineStrikerAnimator playerStrikerAnimator;

    private OfflineStrikerAnimator opponentStrikerAnimator;

    public bool isPlayerTurn = true;

    private bool strikedAnyPucks;

    //private PunTurnManager turnManager;

    private float TURN_DELAY = 30f;

    private Image currentLoader;

    public Image playerLoader;

    public Image opponentLoader;

    public Animator queenPuckAnimator;

    public Animator queenPuckAnimatorRight;

    private List<GoaledPuck> goaledColors;

    private StrikerMover downStrikerMover;

    private StrikerMover upStrikerMover;

    public Button GOShareButton;

    private float fillAmount;

    private float startTime;

    private bool IsPlayerWon;

    private bool isGameOver;

    private int winscore;

    private bool queenAcquired;

    public Transform redCoinLeft;

    public Transform redCoinRight;

    private GameObject queenPuck;

    private bool timesUp;

    public GameObject home;

    public GameObject share;

    private void Start()
    {
        downStrikerMover = strikerMoverDown.GetComponent<StrikerMover>();
        upStrikerMover = strikerMoverUp.GetComponent<StrikerMover>();
        strikerMoverUpRender = strikerMoverUp.GetComponent<SpriteRenderer>();
        strikerMoverDownRender = strikerMoverDown.GetComponent<SpriteRenderer>();
        strikerMoverUpCollider = strikerMoverUp.GetComponent<CircleCollider2D>();
        strikerMoverDownCollider = strikerMoverDown.GetComponent<CircleCollider2D>();
        SetWinScore((puckPositions.Length - 1) / 2);
        Instance = this;
        goaledColors = new List<GoaledPuck>();
        StartGame();
    }

    private void Update()
    {
        fillAmount = 1f - Mathf.Abs(startTime - Time.fixedTime) / LevelManager.getInstance().turnTime;
        if (!timesUp && currentLoader != null)
        {
            if (fillAmount < 0f)
            {
                OnTurnTimeEnds(0);
            }
            if (fillAmount > 0f)
            {
                currentLoader.fillAmount = fillAmount;
            }
        }
    }

    private void AddTunrManager()
    {
        //turnManager = base.gameObject.AddComponent<PunTurnManager>();
        //turnManager.TurnManagerListener = this;
        TURN_DELAY = LevelManager.getInstance().turnTime;
        //turnManager.TurnDuration = TURN_DELAY;
    }

    public void SetStrikerAnimator(OfflineStrikerAnimator strikerAnimator)
    {
        playerStrikerAnimator = strikerAnimator;
    }

    public void SetOpponentStrikerAnimator(OfflineStrikerAnimator strikerAnimator)
    {
        opponentStrikerAnimator = strikerAnimator;
    }

    public void StartGame()
    {
        InstantiatePucks();
        InstantiateOpponetStriker();
        BeginGame(0);
        PlayerTurn();
        AudioManager.getInstance().PlaySound(AudioManager.PLAY_PLAYER_TURN);
        ManageSoundButtons();
    }


    public List<GameObject> allPuck = new List<GameObject>();
    private void InstantiatePucks()
    {
        CarromCoin[] array = puckPositions;
        GameObject gameObject;
        foreach (CarromCoin carromCoin in array)
        {
            gameObject = Object.Instantiate(carromCoin.prefab, carromCoin.prefab.transform.position, Quaternion.identity);
            gameObject.name = gameObject.name + CarromSocketManager.Instance.posListOrg.Count;
            PuckColor component = gameObject.GetComponent<PuckColor>();
            allPuck.Add(gameObject);

            //if (DataManager.Instance.playerNo == 2)
            //{
            //    PuckColor puckColor = gameObject.GetComponent<PuckColor>();
            //    if (puckColor.suit == PuckColor.Color.BLACK)
            //    {
            //        gameObject.GetComponent<SpriteRenderer>().sprite = whiteCoin;
            //        puckColor.suit = PuckColor.Color.WHITE;
            //    }
            //    else if (puckColor.suit == PuckColor.Color.WHITE)
            //    {
            //        gameObject.GetComponent<SpriteRenderer>().sprite = blackCoin;
            //        puckColor.suit = PuckColor.Color.BLACK;
            //    }
            //}
            pucks.Add(new Puck(gameObject.GetComponent<Rigidbody2D>(), component));
        }
        gameObject = playerStriker;
        ChangePos();
        playerStriker.GetComponent<SpriteRenderer>().sprite = LevelManager.instance.strikers[PlayerPrefs.GetInt("striker", 0)];
        playerStrikerAnimator = gameObject.GetComponent<OfflineStrikerAnimator>();
        SetStrikerAnimator(playerStrikerAnimator);
        gameObject.GetComponent<CircleCollider2D>().enabled = true;
        downStrikerMover.SetStriker(gameObject, PlayerPrefs.GetInt("striker"));
    }

    void ChangePos()
    {
        if (DataManager.Instance.playerNo == 2)
        {

            foreach (GameObject gameObject in allPuck)
            {
                print("gameObject Name : " + gameObject.name);
                float posY = gameObject.transform.position.y;
                float posX = gameObject.transform.position.x;

                //CarromSocketManager.Instance.posListOrg.Add(posY);
                if (posY < 0)
                {
                    posY = Mathf.Abs(posY);

                    print("posX : " + posY);
                }
                else
                {
                    posY = -posY;
                    print("posX : " + posY);
                }

                if (posX < 0)
                {
                    posX = Mathf.Abs(posX);

                    print("posX : " + posX);
                }
                else
                {
                    posX = -posX;
                    print("posX : " + posX);
                }

                //CarromSocketManager.Instance.posList.Add(posY);
                Vector3 updatePos = new Vector3(posX, posY, gameObject.transform.position.z);
                gameObject.transform.position = updatePos;
            }
        }
    }


    private void InstantiateOpponetStriker()
    {
        GameObject gameObject = opponentStriker;
        opponentStriker.GetComponent<SpriteRenderer>().sprite = LevelManager.instance.strikers[PlayerPrefs.GetInt("striker", 0)];
        gameObject.GetComponent<CircleCollider2D>().enabled = true;
        opponentStrikerAnimator = gameObject.GetComponent<OfflineStrikerAnimator>();
        SetOpponentStrikerAnimator(opponentStrikerAnimator);
        upStrikerMover.SetStriker(gameObject, PlayerPrefs.GetInt("striker"));
    }

    private void BeginGame(int playetToPlay)
    {
        if (playetToPlay == 0)
        {
            PuckColor.Color color = ((playetToPlay == 0) ? PuckColor.Color.WHITE : PuckColor.Color.BLACK);
            currentLoader = ((playetToPlay != 0) ? opponentLoader : playerLoader);
            playerCoin.sprite = ((color != PuckColor.Color.WHITE) ? blackCoin : whiteCoin);
            opponentCoin.sprite = ((color != PuckColor.Color.WHITE) ? whiteCoin : blackCoin);
            PuckColor.Color color2 = ((color != PuckColor.Color.WHITE) ? PuckColor.Color.WHITE : PuckColor.Color.BLACK);
            player = new OfflinePlayer(color);
            opponentPlayer = new OfflinePlayer(color2);

            if (DataManager.Instance.playerNo == 1)
            {
                playingPlayer = player;
            }
            else
            {
                playingPlayer = opponentPlayer;//Greejesh Main
            }
            playerScoreView = leftScoreView;
            opponentScoreView = rightScoreView;
            movementPadBottom.SetActive(true);
        }
        playerCoin.enabled = true;
        opponentCoin.enabled = true;
        playerNameView.text = "Player 1";
        opponentNameView.text = "Player 2";
    }

    public void PlayerTurn()
    {
        EnableStrikerHandler();

        //if (CarromSocketManager.Instance.greeNo == 1)
        //{
        //    playerStrikerAnimator.MoveStrikerIn();
        //}
        //else
        //{
        //    opponentStrikerAnimator.MoveStrikerIn();


        //}
        if (player == playingPlayer)
        {
            playerStrikerAnimator.MoveStrikerIn();
        }
        else
        {
            opponentStrikerAnimator.MoveStrikerIn();
        }
        AudioManager.getInstance().PlaySound(AudioManager.PLAY_PLAYER_TURN);
        RoundBegin();
    }

    public void RoundBegin()
    {
        strikedAnyPucks = false;
        //if (turnManager != null)
        //{
        //    turnManager.BeginTurn();
        //}
        startTime = (int)Time.time;
        timesUp = false;
    }


    private void HandleOwnerShip()
    {
        playingPlayer = ((playingPlayer != player) ? player : opponentPlayer);
    }

    public void OpponentScoredPoint(OfflinePlayer opponnentId, int score)
    {
        AudioManager.getInstance().PlaySound(AudioManager.PLAY_POINT_SCORED);
        ((opponnentId == player) ? playerScoreView : opponentScoreView).text = score.ToString();
    }

    public void PlayerScoredPoint(OfflinePlayer playerId, int score)
    {
        AudioManager.getInstance().PlaySound(AudioManager.PLAY_POINT_SCORED);
        ((playerId != player) ? opponentScoreView : playerScoreView).text = score.ToString();
    }

    public void GameOver(OfflinePlayer playerId)
    {
        currentLoader.fillAmount = 1f;
        currentLoader = null;
        AudioManager.getInstance().PlaySound(AudioManager.GAME_OVER);
        gameOverPanel.SetActive(true);
        gameOverPartilce.SetActive(true);
        isGameOver = true;
        if (playerId == player)
        {
            //leftAvatar.GetComponent<GameOverAnimator>().AnimateWinner();
            //rightAvatar.GetComponent<GameOverAnimator>().AnimateLooser();
        }
        else
        {
            //rightAvatar.GetComponent<GameOverAnimator>().AnimateWinner();
            //leftAvatar.GetComponent<GameOverAnimator>().AnimateLooser();
        }
    }

    public void PlayerLostPoint(OfflinePlayer playerId, int score)
    {
        ((playerId != player) ? opponentScoreView : playerScoreView).text = score.ToString();
    }

    public void AnimatePuck(PuckAnimator animator)
    {
    }

    private void MoveStrikerHandlerToCenter()
    {
        if (playingPlayer == player)
        {
            downStrikerMover.AnimateStrikerHandlerToCenter();
        }
        else
        {
            upStrikerMover.AnimateStrikerHandlerToCenter();
        }
    }

    private Foul IsFoul()
    {
        int num = 0;
        int num2 = 0;
        foreach (Puck puck in pucks)
        {
            if (puck.puckcolor.suit != PuckColor.Color.RED)
            {
                if (puck.puckcolor.suit == playingPlayer.color)
                {
                    num++;
                }
                else
                {
                    num2++;
                }
            }
        }
        foulDetail.text = string.Empty;
        foreach (GoaledPuck goaledColor in goaledColors)
        {
            if (goaledColor.color == PuckColor.Color.STRIKER_COLOR)
            {
                foulDetail.text = "You cannot put the Striker";
                return new Foul(num, num2, true, "Foul");
            }
        }
        if (num == 1 || num2 == 1)
        {
            int num3 = 0;
            int num4 = 0;
            foreach (GoaledPuck goaledColor2 in goaledColors)
            {
                if (goaledColor2.color != PuckColor.Color.RED)
                {
                    if (goaledColor2.color == playingPlayer.color)
                    {
                        num3++;
                    }
                    else
                    {
                        num4++;
                    }
                }
            }
            if ((!queenAcquired || IsQueenGoaled()) && num2 == 1 && num4 == 1)
            {
                foulDetail.text = "Putting opponent's last puck when\n Red is not recovered is Foul";
                return new Foul(num, num2, true, "Plotting opponent's last puck when\n Red is not recovered is Foul");
            }
            if (IsQueenPresent() && !IsQueenGoaled() && num == 1 && num3 == 1)
            {
                foulDetail.text = "First Put Red Puck";
                return new Foul(num, num2, true, "First Put Queen(Red) Puck");
            }
            if (IsQueenPresent() && !IsQueenGoaled() && num2 == 1 && num4 == 1)
            {
                foulDetail.text = "First Put Red Puck";
                return new Foul(num, num2, true, "First Put Queen(Red) Puck");
            }
        }
        return new Foul(num, num2, false, string.Empty);
    }

    private bool IsQueenPresent()
    {
        foreach (Puck puck in pucks)
        {
            if (puck.puckcolor.suit == PuckColor.Color.RED)
            {
                return true;
            }
        }
        return false;
    }

    private bool IsQueenGoaled()
    {
        foreach (GoaledPuck goaledColor in goaledColors)
        {
            if (goaledColor.color == PuckColor.Color.RED)
            {
                return true;
            }
        }
        return false;
    }

    private bool HasStrikedAnyPucksToHole()
    {
        return goaledColors.Count > 0;
    }

    public void SetWinScore(int score)
    {
        winscore = score;
    }

    private bool IsGameOver()
    {
        if (player.score >= winscore)
        {
            GameOver(player);
            return true;
        }
        if (opponentPlayer.score >= winscore)
        {
            GameOver(opponentPlayer);
            return true;
        }
        return false;
    }

    public void RoundComplete(float delay)
    {
        Foul foul = IsFoul();
        if (foul.isFoul)
        {
            ShowFoulMessage();
            if (playingPlayer.playerPutQueen)
            {
                RespawnQueen();
            }
            foreach (GoaledPuck goaledColor in goaledColors)
            {
                if (goaledColor.color != PuckColor.Color.STRIKER_COLOR)
                {
                    if (goaledColor.color == playingPlayer.color || goaledColor.color == PuckColor.Color.RED)
                    {
                        goaledColor.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
                        goaledColor.gameObject.GetComponent<PuckAnimator>().AnimatePuck(Vector3.zero);
                        continue;
                    }
                    if (foul.opponentCoinsCount == 1 && IsQueenPresent())
                    {
                        goaledColor.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
                        goaledColor.gameObject.GetComponent<PuckAnimator>().AnimatePuck(Vector3.zero);
                        continue;
                    }
                    DestroyPuck(goaledColor.gameObject);
                    OfflinePlayer offlinePlayer = ((playingPlayer != player) ? player : opponentPlayer);
                    offlinePlayer.score++;
                    OpponentScoredPoint(offlinePlayer, offlinePlayer.score);
                }
            }
            if (playingPlayer.score > 0)
            {
                GameObject gameObject = ((playingPlayer.color != 0) ? Object.Instantiate(whitePuck, (playingPlayer != player) ? strikerMoverUp.transform.position : strikerMoverDown.transform.position, Quaternion.identity) : Object.Instantiate(blackPuck, (playingPlayer != player) ? strikerMoverUp.transform.position : strikerMoverDown.transform.position, Quaternion.identity));
                gameObject.GetComponent<PuckAnimator>().AnimatePuck(Vector3.zero);
                pucks.Add(new Puck(gameObject.GetComponent<Rigidbody2D>(), gameObject.GetComponent<PuckColor>()));
                playingPlayer.score--;
                PlayerLostPoint(playingPlayer, playingPlayer.score);
            }
            if (!IsGameOver())
            {
                goaledColors.Clear();
                Invoke("NextPlayerTurn", 2f);
            }
            return;
        }
        strikedAnyPucks = HasStrikedAnyPucksToHole();
        if (strikedAnyPucks)
        {
            bool flag = false;
            bool flag2 = false;
            foreach (GoaledPuck goaledColor2 in goaledColors)
            {
                if (goaledColor2.color == playingPlayer.color)
                {
                    flag = true;
                    playingPlayer.score++;
                    PlayerScoredPoint(playingPlayer, playingPlayer.score);
                    DestroyPuck(goaledColor2.gameObject);
                }
                else if (goaledColor2.color == PuckColor.Color.RED)
                {
                    playingPlayer.playerPutQueen = true;
                    flag2 = true;
                    DestroyPuck(goaledColor2.gameObject);
                }
                else
                {
                    DestroyPuck(goaledColor2.gameObject);
                    OfflinePlayer offlinePlayer2 = ((playingPlayer != player) ? player : opponentPlayer);
                    offlinePlayer2.score++;
                    OpponentScoredPoint(offlinePlayer2, offlinePlayer2.score);
                }
            }
            MoveStrikerHandlerToCenter();
            if (flag && playingPlayer.playerPutQueen)
            {
                playingPlayer.playerPutQueen = false;
                RoundBegin();
                if (player == playingPlayer)
                {
                    playerStrikerAnimator.MoveBackStriker();
                }
                else
                {
                    opponentStrikerAnimator.MoveBackStriker();
                }
                if (queenPuck != null)
                {
                    queenAcquired = true;
                    queenPuck.GetComponent<SpriteRenderer>().color = Color.white;
                    if (player == playingPlayer)
                    {
                        queenRecoverLeft.text = "Red recovered";
                        queenPuckAnimator.gameObject.SetActive(true);
                        queenPuckAnimator.SetTrigger("show");
                    }
                    else
                    {
                        queenRecoverRight.text = "Red recovered";
                        queenPuckAnimatorRight.gameObject.SetActive(true);
                        queenPuckAnimatorRight.SetTrigger("show");
                    }
                }
            }
            else if (flag || flag2)
            {
                RoundBegin();
                if (player == playingPlayer)
                {
                    playerStrikerAnimator.MoveBackStriker();
                }
                else
                {
                    opponentStrikerAnimator.MoveBackStriker();
                }
            }
            else
            {
                if (playingPlayer.playerPutQueen)
                {
                    RespawnQueen();
                }
                Invoke("NextPlayerTurn", 2f);
            }
            if (IsGameOver())
            {
                return;
            }
        }
        else if (playingPlayer.playerPutQueen)
        {
            RespawnQueen();
            Invoke("NextPlayerTurn", 2f);
        }
        else
        {
            Invoke("NextPlayerTurn", delay);
        }
        goaledColors.Clear();
    }

    public void ShowFoulMessage()
    {
        AudioManager.getInstance().PlaySound(AudioManager.PLAY_FOUL);
        foulAnimator.SetTrigger("foul");
    }

    private void RespawnQueen()
    {
        playingPlayer.playerPutQueen = false;
        GameObject gameObject = Object.Instantiate(redPuck, (playingPlayer != player) ? strikerMoverUp.transform.position : strikerMoverDown.transform.position, Quaternion.identity);
        gameObject.GetComponent<PuckAnimator>().AnimatePuck(Vector3.zero);
        pucks.Add(new Puck(gameObject.GetComponent<Rigidbody2D>(), gameObject.GetComponent<PuckColor>()));
        if (queenPuck != null)
        {
            if (player == playingPlayer)
            {
                queenRecoverLeft.text = "Red not recovered";
                queenPuckAnimator.gameObject.SetActive(true);
                queenPuckAnimator.SetTrigger("show");
            }
            else
            {
                queenRecoverRight.text = "Red not recovered";
                queenPuckAnimatorRight.gameObject.SetActive(true);
                queenPuckAnimatorRight.SetTrigger("show");
            }
            Object.Destroy(queenPuck);
        }
    }

    public void DestroyPuck(GameObject puck)
    {
        PuckColor component = puck.GetComponent<PuckColor>();
        foreach (Puck puck2 in pucks)
        {
            if (puck2.puckcolor == component)
            {
                pucks.Remove(puck2);
                break;
            }
        }
        if (component.suit == PuckColor.Color.RED)
        {
            queenPuck = Object.Instantiate(redPuckOffline, puck.transform.position, Quaternion.identity);
            if (player == playingPlayer)
            {
                queenPuck.GetComponent<PuckAnimator>().AnimateQueen(Camera.main.ScreenToWorldPoint(redCoinLeft.position));
            }
            else
            {
                queenPuck.GetComponent<PuckAnimator>().AnimateQueen(Camera.main.ScreenToWorldPoint(redCoinRight.position));
            }
            Object.Destroy(puck);
        }
        else if (playingPlayer.color == component.suit)
        {
            if (playingPlayer == player)
            {
                puck.GetComponent<PuckAnimator>().AnimatePuckToScore(Camera.main.ScreenToWorldPoint(playerCoin.transform.position));
            }
            else
            {
                puck.GetComponent<PuckAnimator>().AnimatePuckToScore(Camera.main.ScreenToWorldPoint(opponentCoin.transform.position));
            }
        }
        else if (playingPlayer == player)
        {
            puck.GetComponent<PuckAnimator>().AnimatePuckToScore(Camera.main.ScreenToWorldPoint(opponentCoin.transform.position));
        }
        else
        {
            puck.GetComponent<PuckAnimator>().AnimatePuckToScore(Camera.main.ScreenToWorldPoint(playerCoin.transform.position));
        }
    }

    private void NextPlayerTurn()
    {
        disableStrikerMover();
        strikedAnyPucks = false;
        if (playingPlayer == player)
        {
            playerStrikerAnimator.MoveStrikerOut();
        }
        else
        {
            opponentStrikerAnimator.MoveStrikerOut();
        }
        MoveStrikerHandlerToCenter();
        HandleOwnerShip();
        PlayerTurn();
        UpdateLoader();
    }

    public void UpdateLoader()
    {
        if (!(currentLoader == null))
        {
            currentLoader.fillAmount = 1f;
            if (playingPlayer != player)
            {
                currentLoader = opponentLoader;
            }
            else
            {
                currentLoader = playerLoader;
            }
        }
    }

    private void disableStrikerMover()
    {
        if (playingPlayer == player)
        {
            strikerMoverDownRender.color = disabledColor;
            strikerMoverDownCollider.enabled = false;
            strikerStripDownRender.color = disabledColor;
        }
        else
        {
            strikerMoverUpRender.color = disabledColor;
            strikerMoverUpCollider.enabled = false;
            strikerStripUpRender.color = disabledColor;
        }
    }

    private void EnableStrikerHandler()
    {
        if (playingPlayer == player)
        {
            strikerMoverDownRender.color = enabledColor;
            strikerMoverDownCollider.enabled = true;
            strikerStripDownRender.color = enabledColor;

            strikerMoverUpRender.color = disabledColor;
            strikerMoverUpCollider.enabled = false;
            strikerStripUpRender.color = disabledColor;
        }
        else
        {
            strikerMoverUpRender.color = enabledColor;
            strikerMoverUpCollider.enabled = true;
            strikerStripUpRender.color = enabledColor;

            strikerMoverDownRender.color = disabledColor;
            strikerMoverDownCollider.enabled = false;
            strikerStripDownRender.color = disabledColor;
        }
    }

    public void SetScorePoint()
    {
    }



    public void GoaledColor(GoaledPuck puck)
    {
        goaledColors.Add(puck);
    }

    public void OnTurnBegins(int turn)
    {
        Debug.LogError("OnTurnBegins");
    }

    public void OnTurnCompleted(int turn)
    {
    }

    //public void OnPlayerMove(Player player, int turn, object move)
    //{
    //}

    //public void OnPlayerFinished(Player player, int turn, object move)
    //{
    //}

    public void OnTurnTimeEnds(int turn)
    {
        if (!isGameOver)
        {
            timesUp = true;
            RoundComplete(0f);
        }
    }

    public void FinishTurn()
    {
        timesUp = true;
    }

    public void OnQuitClicked()
    {
        AudioManager.getInstance().PlaySound(AudioManager.PLAY_CLICK);
        quitPanel.SetActive(true);
    }

    public void OnCancelQuitClicked()
    {
        AudioManager.getInstance().PlaySound(AudioManager.PLAY_CLICK);
        quitPanel.SetActive(false);
    }

    public void LeaveAndGoHome()
    {
        AudioManager.getInstance().PlaySound(AudioManager.PLAY_CLICK);
        AudioManager.getInstance().StopSound(AudioManager.GAME_OVER);
        GoBackToMenu();
    }

    private void GoBackToMenu()
    {
        AudioManager.getInstance().StopSound(AudioManager.PLAY_TICK);
        SceneManager.LoadScene("Main");
    }

    public void ShareScore()
    {
        AudioManager.getInstance().PlaySound(AudioManager.PLAY_CLICK);
        //ShareManager.insatnce.ShareRoomId("I'm playing Carrom Master.\nDownload today and join me!\n" + FacebookLogin.SHARE_URL);
    }

    public void CurtainClosed()
    {
        home.SetActive(true);
        share.SetActive(true);
    }

    #region Button Functions
    
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
    
    void OpenSettingsScreen()
    {
        settingsScreenObj.SetActive(true);
    }
    
    void CloseSettingsScreen()
    {
        settingsScreenObj.SetActive(false);
    }
    
    void OpenMenuScreen()
    {
        menuScreenObj.SetActive(true);
    }
    
    void CloseMenuScreen()
    {
        menuScreenObj.SetActive(false);
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
    
    public void MenuSubButtonClick(int no)
    {
        SoundManager.Instance.ButtonClick();
        if (no == 1)
        {
            Time.timeScale = 1;
            AudioManager.getInstance().PlaySound(AudioManager.PLAY_CLICK);
            AudioManager.getInstance().StopSound(AudioManager.GAME_OVER);
            AudioManager.getInstance().StopSound(AudioManager.PLAY_TICK);
            TestSocketIO.Instace.LeaveRoom();
            SoundManager.Instance.StartBackgroundMusic();
            SceneManager.LoadScene("Main");
        }
        else if (no == 2)
        {
            //OpenRuleScreen();
        }
        else if (no == 3)
        {
            //Shop
            //Instantiate(shopPrefab, shopPrefabParent.transform);
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
}
