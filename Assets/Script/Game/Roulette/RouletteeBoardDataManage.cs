using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class RouletteeBoardDataManage : MonoBehaviour
{

    public static RouletteeBoardDataManage Instance;

    public Text winAnimationTxt;

    [Header("--- Win Screen ---")]
    public GameObject winScreenObj;
    public Text winAmountTxt;
    public Text winNoTxt;
    public GameObject winLastObj;
    public GameObject winFirstObj;
    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void OnEnable()
    {
        winScreenObj.transform.DOMove(winFirstObj.transform.position, 0f);
        winScreenObj.transform.localScale = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {

    }

    #region Win Screen

    public void OpenWinBox()
    {
        winScreenObj.transform.DOMove(winFirstObj.transform.position, 0f);
        winScreenObj.transform.localScale = Vector3.zero;
        winScreenObj.SetActive(true);


        //Greejesh Count Admin Commision

        float adminPercentage = DataManager.Instance.adminPercentage;

        float winAmount = RouletteManager.Instance.WinManager();
        float adminCommssion = (winAmount / adminPercentage);
        float playerWinAmount = winAmount - adminCommssion;

        if (playerWinAmount != 0)
        {
            SoundManager.Instance.CasinoWinSound();
            winAnimationTxt.gameObject.SetActive(true);
            winAnimationTxt.text = "+" + playerWinAmount;
            Invoke(nameof(WinAmountTextOff), 0.52f);
            //DataManager.Instance.AddAmount((float)(playerWinAmount), TestSocketIO.Instace.roomid, "Roulette-Win-" + TestSocketIO.Instace.roomid, "won", (float)(adminCommssion));
        }
        float otherAmount = RouletteManager.Instance.totalCurrentInvest - playerWinAmount;
        if (otherAmount != 0)
        {
            //WinProfite
        }
        winAmountTxt.text = "₹" + playerWinAmount.ToString("F2");
        winNoTxt.text = RouletteManager.Instance.noGen.ToString();

        print("Admin Commsion : " + adminCommssion);

        winScreenObj.transform.DOMove(winLastObj.transform.position, 0.35f);
        winScreenObj.transform.DOScale(Vector3.one, 0.35f);
        Invoke(nameof(RouletteBoardOff), 2.5f);
    }

    public void WinAmountTextOff()
    {
        winAnimationTxt.gameObject.SetActive(false);
    }
    void RouletteBoardOff()
    {
        RouletteManager.Instance.GiveUserData();
        this.gameObject.SetActive(false);
    }

    #endregion

}
