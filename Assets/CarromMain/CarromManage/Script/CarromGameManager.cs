using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CarromGameManager : MonoBehaviour
{
    public Color disabledColor;

    public Color enabledColor;

    public Sprite whiteCoin;

    public Sprite blackCoin;

    public GameObject strikerMoverUp;

    public GameObject strikerMoverDown;

    public Text leftScoreView;

    public Text rightScoreView;

    public Image playerCoin;

    public Image opponentCoin;

    public Image playerImageView;

    public Image opponentImageView;

    public Transform redCoinLeft;

    public Transform redCoinRight;

    public Transform gemLeft;

    public Transform gemRight;

    public GameObject gem;

    public GameObject gemAnimator;

    public GameObject backgroundPattern;

    public GameObject gemParticleObject;

    public Text playerNameView;

    public Text opponentNameView;

    public Text foulDetail;

    public Sprite[] avatars;



    public Text dealMessage;

    public Text winnercoins;

    public Text snackbarMessage;

    public Text playerPuckMessage;

    public Text leftChatText;

    public Text rightChatText;

    public Text queenRecoverLeft;

    public Text queenRecoverRight;

    public static CarromGameManager Instance;

    public GameObject movementPadBottom;

    public GameObject movementPadTop;

    public GameObject gameOverPanel;

    public GameObject searchingPanel;

    public GameObject boardShadow;

    public GameObject gameOverParticle;

    public GameObject leftAvatar;

    public GameObject rightAvatar;

    public GameObject adRewardedPanel;

    public GameObject doubleCoins;

    public GameObject home;

    public GameObject share;

    public GameObject chatMessagePanel;

    public GameObject chatButton;

    public GameObject quitPanel;

    public GameObject blackPuck;

    public GameObject whitePuck;

    public GameObject redPuck;

    public Transform restPositionTop;

    public Transform restPositionBottom;

    public Animator dealAnimator;

    public Animator snackBarAnimator;

    public Animator foulAnimator;

    public Animator cameraAnimator;

    public Animator playerPuckAnimator;

    public Animator queenPuckAnimator;

    public Animator queenPuckAnimatorRight;

    public Animator chatAnimatorLeft;

    public Animator chatAnimatorRight;

    public CarromCoin[] puckPositions;

    public List<Puck> pucks = new List<Puck>();

    public bool isPlayerTurn = true;

    private Image currentLoader;

    public Image playerLoader;

    public Image opponentLoader;

    public PuckColor.Color playerColor;

    public PuckColor.Color opponentColor;


    public trajectoryScript strikerScript;

    public Button ShareButton;

    public Button GOShareButton;

    public float OPPONENT_WAITING_TIME = 5f;


    public bool timesup;

    public bool isGameOver;





    public Text rewardMessage;

    public bool showCustomMessages;

    public InputField messageBox;

    public Button sendMessageButton;



    public Text quitMessage;

    public Text quitSubMessage;

    public Text quitButton;

    private void Start()
    {
        Instance = this;
    }

}
