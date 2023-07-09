using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TeenPattiTourBox : MonoBehaviour
{

    public GameType gameType;
    public Text bootTxt;
    public Text minBuyInTxt;
    public Text chaalLimitTxt;
    public Text potLimitTxt;
    public Text joinPlayerTxt;
    public string tournamentID;

    public float chaalLimit;
    public float potLimit;

    // Start is called before the first frame update
    void Start()
    {
        TestSocketIO.Instace.SetLobbyCount(tournamentID);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayNowButtonClick()
    {

        DataManager.Instance.gameMode = gameType;
        DataManager.Instance.chaalLimit = chaalLimit;
        DataManager.Instance.potLimit = potLimit;
        DataManager.Instance.tournamentID = tournamentID;
        TestSocketIO.Instace.TeenPattiJoinroom();
        //
    }




}
