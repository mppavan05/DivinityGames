using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarromSocketManager : MonoBehaviour
{

    public static CarromSocketManager Instance;

    public int greeNo;
    public GameObject winScreenObj;
    public bool isOpenWin;
    public bool isOtherPlayLeft;
    public int playerScoreCnt1;
    public int playerScoreCnt2;

    public bool isAvaliableStrike;

    public StrikerMover strikeCurrent;
    public OfflineStriker strikeOpp;

    public List<float> posList = new List<float>();
    public List<float> posListOrg = new List<float>();
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }


    }




    public void WinUserShow()
    {
        winScreenObj.SetActive(true);
    }

    void Start()
    {
        if (greeNo == 1)
        {
            isAvaliableStrike = true;
        }
        else
        {
            isAvaliableStrike = false;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    #region Send Socket

    public void Strike_Slider_Send(float posX)
    {
        JSONObject obj = new JSONObject();
        obj.AddField("PlayerID", DataManager.Instance.playerData._id);
        obj.AddField("TournamentID", DataManager.Instance.tournamentID);
        obj.AddField("PlayerNo", DataManager.Instance.playerNo);
        obj.AddField("SliderPos", posX);
        obj.AddField("RoomId", TestSocketIO.Instace.roomid);
        TestSocketIO.Instace.Senddata("CarromSlideData", obj);
    }

    public void Strike_Slider_Receive(float pos)
    {
        //print("Player Pos = : " + pos);
        if (pos < 0)
        {
            pos = Mathf.Abs(pos);
        }
        else
        {
            pos = -pos;
        }
        strikeCurrent.oppStrikeMover.Socket_Get_Pos(pos);
    }

    public void Strike_Fire_Send(Vector2 force)
    {
        JSONObject obj = new JSONObject();
        obj.AddField("PlayerID", DataManager.Instance.playerData._id);
        obj.AddField("TournamentID", DataManager.Instance.tournamentID);
        obj.AddField("PlayerNo", DataManager.Instance.playerNo);
        obj.AddField("ForceX", force.x);
        obj.AddField("ForceY", force.y);
        obj.AddField("RoomId", TestSocketIO.Instace.roomid);
        TestSocketIO.Instace.Senddata("CarromStrikeFireData", obj);
    }

    public void Strike_Fire_Receive(float posX, float posY)
    {

        float posX1 = posX;
        if (posX1 < 0)
        {
            posX1 = Mathf.Abs(posX1);
        }
        else
        {
            posX1 = -posX1;
        }

        float posY1 = posY;
        if (posY1 < 0)
        {
            posY1 = Mathf.Abs(posY1);
        }
        else
        {
            posY1 = -posY1;
        }

        //print("Receive Force : " + new Vector2(-posX, -posY));
        strikeOpp.Strike_Fire(new Vector2(posX1, posY1));
    }


    public void Strike_Direction_Send(Vector3 fingerPos)
    {
        JSONObject obj = new JSONObject();
        obj.AddField("PlayerID", DataManager.Instance.playerData._id);
        obj.AddField("TournamentID", DataManager.Instance.tournamentID);
        obj.AddField("PlayerNo", DataManager.Instance.playerNo);
        obj.AddField("DirX", fingerPos.x);
        obj.AddField("DirY", fingerPos.y);
        obj.AddField("DirZ", fingerPos.z);
        obj.AddField("RoomId", TestSocketIO.Instace.roomid);
        TestSocketIO.Instace.Senddata("CarromStrikeDirData", obj);
    }

    public void Strike_Direction_Receive(float posX, float posY, float posZ)
    {

        float posX1 = posX;
        if (posX1 < 0)
        {
            posX1 = Mathf.Abs(posX1);
        }
        else
        {
            posX1 = -posX1;
        }

        float posY1 = posY;
        if (posY1 < 0)
        {
            posY1 = Mathf.Abs(posY1);
        }
        else
        {
            posY1 = -posY1;
        }


        float posZ1 = posZ;
        if (posZ1 < 0)
        {
            posZ1 = Mathf.Abs(posZ1);
        }
        else
        {
            posZ1 = -posZ1;
        }

        //print("Receive Force : " + new Vector2(-posX, -posY));
        strikeOpp.Strike_Dire_Rec(new Vector3(posX1, posY1, posZ1));
    }

    #endregion
}
