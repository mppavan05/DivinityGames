using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class GiftBox
{
    public string giftName;
    public int giftNo;
    public string giftId;
    public float price;
    public Sprite giftSprite;
}

public class GiftSendManager : MonoBehaviour
{

    public static GiftSendManager Instance;

    public List<GiftBox> giftBoxes = new List<GiftBox>();
    public GameObject giftBox;
    public GameObject giftBoxParent;

    public string gameName;

    public TeenPattiPlayer teenPattiOtherPlayer;
    public AndarBaharPlayer andarBaharOtherPlayer;
    public PokerPlayer pokerOtherPlayer;
    // Start is called before the first frame update


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    void Start()
    {
        for (int i = 0; i < giftBoxes.Count; i++)
        {
            GameObject giftGenObj = Instantiate(giftBox, giftBoxParent.transform);
            int no = i;
            giftGenObj.transform.GetChild(1).GetComponent<Text>().text = giftBoxes[no].price.ToString();
            giftGenObj.transform.GetChild(2).GetComponent<Image>().sprite = giftBoxes[no].giftSprite;

            giftGenObj.transform.GetComponent<Button>().onClick.AddListener(() => GiftBtnClick(no));
        }
    }



    // Update is called once per frame
    void Update()
    {

    }

    public void GiftBtnClick(int no)
    {

        SoundManager.Instance.ButtonClick();
        if (gameName == "TeenPatti")
        {
            //TeenPattiManager.Instance.GetGift(DataManager.Instance.playerData._id, teenPattiOtherPlayer.playerId, no);
            SendMessgaeSocket(no, teenPattiOtherPlayer.playerId);
            this.gameObject.SetActive(false);

        }
        else if (gameName == "AndarBahar")
        {
            //AndarBaharManager.Instance.GetGift(DataManager.Instance.playerData._id, teenPattiOtherPlayer.playerId, no);

            SendMessgaeSocket(no, andarBaharOtherPlayer.playerId);
            this.gameObject.SetActive(false);

        }
        else if (gameName == "Poker")
        {
            //AndarBaharManager.Instance.GetGift(DataManager.Instance.playerData._id, teenPattiOtherPlayer.playerId, no);

            SendMessgaeSocket(no, pokerOtherPlayer.playerId);
            this.gameObject.SetActive(false);

        }
    }


    public void SendMessgaeSocket(int giftNo, string otherPlayerId)
    {
        JSONObject obj = new JSONObject();
        obj.AddField("SendPlayerID", DataManager.Instance.playerData._id);
        obj.AddField("ReceivePlayerID", otherPlayerId);
        obj.AddField("RoomId", TestSocketIO.Instace.roomid);
        obj.AddField("GiftNo", giftNo);
        obj.AddField("gameName", gameName);
        TestSocketIO.Instace.Senddata("SendGiftMessage", obj);
    }
    public void CloseButtonClick()
    {
        this.gameObject.SetActive(false);
    }
}
