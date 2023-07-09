using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TournamentPanel : MonoBehaviour
{
    public static TournamentPanel Instance;
    public GameObject scrollParent;
    public GameObject tourPrefab;
    public GameType gameType;

    public string tourmentId;

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
        MainMenuManager.Instance.screenObj.Add(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GenerateTournament()
    {
        List<TournamentData> tournaments = new List<TournamentData>();
        for (int i = 0; i < DataManager.Instance.tournamentData.Count; i++)
        {
            if (DataManager.Instance.tournamentData[i].modeType == gameType)
            {
                tournaments.Add(DataManager.Instance.tournamentData[i]);
            }
        }

        GenerateTypeTournament(tournaments);


    }

    void GenerateTypeTournament(List<TournamentData> tournaments)
    {
        if (tournaments.Count == 0) return;
        switch (gameType)
        {
            case GameType.Teen_Patti:
            {
                foreach (var t in tournaments)
                {
                    TeenPattiTourBox teenPattiTourBox = Instantiate(tourPrefab, scrollParent.transform).GetComponent<TeenPattiTourBox>();

                    teenPattiTourBox.tournamentID = t._id.ToString();
                    teenPattiTourBox.bootTxt.text = t.betAmount.ToString();
                    teenPattiTourBox.minBuyInTxt.text = t.betAmount.ToString();
                    teenPattiTourBox.chaalLimitTxt.text = t.challLimit.ToString();
                    teenPattiTourBox.potLimitTxt.text = t.potLimit.ToString();
                    teenPattiTourBox.joinPlayerTxt.text = t.betAmount.ToString();

                    teenPattiTourBox.chaalLimit = t.challLimit;
                    teenPattiTourBox.potLimit = t.potLimit;
                }

                break;
            }
            case GameType.Ludo:
            {
                for (int i = 0; i < tournaments.Count; i++)
                {
                   if (tournaments[i].players == 2 && LudoSelector.Instance.isTwoPlayerSelected)
                    {
                        // DataManager.Instance.isTwoPlayer = true;
                        LudoTourBox ludoTourBox =
                            Instantiate(tourPrefab, scrollParent.transform).GetComponent<LudoTourBox>();
                        int index = i;

                        ludoTourBox.tourData = tournaments[i];
                        ludoTourBox.tournamentID = tournaments[i]._id.ToString();
                        ludoTourBox.time = tournaments[i].time;
                        ludoTourBox.createDate = tournaments[i].createdAt;
                        ludoTourBox.interval = tournaments[i].interval;
                        ludoTourBox.betAmount = tournaments[i].betAmount;
                        ludoTourBox.winAmount = tournaments[i].totalWinAmount * 10;
                        ludoTourBox.isBotAvliablity = tournaments[i].bot;

                        ludoTourBox.UpdateDisplay();
                    }
                    else if (tournaments[i].players == 4 && LudoSelector.Instance.isFourPlayerSelected)
                    {
                        //DataManager.Instance.isFourPlayer = true;
                        LudoTourBox ludoTourBox =
                            Instantiate(tourPrefab, scrollParent.transform).GetComponent<LudoTourBox>();
                        int index = i;

                        ludoTourBox.tourData = tournaments[i];
                        ludoTourBox.tournamentID = tournaments[i]._id.ToString();
                        ludoTourBox.time = tournaments[i].time;
                        ludoTourBox.createDate = tournaments[i].createdAt;
                        ludoTourBox.interval = tournaments[i].interval;
                        ludoTourBox.betAmount = tournaments[i].betAmount;
                        ludoTourBox.winAmount = tournaments[i].totalWinAmount * 10;
                        ludoTourBox.isBotAvliablity = tournaments[i].bot;

                        ludoTourBox.UpdateDisplay();
                    }
                }

                break;
            }
        }
    }




    public void BackButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        MainMenuManager.Instance.screenObj.Remove(this.gameObject);
        Destroy(this.gameObject);
    }


}
