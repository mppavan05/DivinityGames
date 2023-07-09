using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;

public class SendTypeMessageManager : MonoBehaviour
{

    public InputField messageInput;

    public Text leftWords;
    string playerName = "";
    public string gameName;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < DataManager.Instance.joinPlayerDatas.Count; i++)
        {
            if (DataManager.Instance.joinPlayerDatas[i].userId.Equals(DataManager.Instance.playerData._id))
            {
                playerName = DataManager.Instance.joinPlayerDatas[i].userName;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SendMainMessage(string msg)
    {
        print(msg);
        SendMessgaeSocket(playerName + ": " + msg);
    }

    public void MessageInputCheck(string value)
    {
        messageInput.text = value.TrimStart();
        leftWords.text = (50 - messageInput.text.Length).ToString() + " Left";
    }

    public void SendMessageBtn()
    {
        if (!IsNullOrEmpty(messageInput.text.Trim()))
        {
            SoundManager.Instance.ButtonClick();
            SendMessgaeSocket(playerName + ": " + messageInput.text.TrimEnd());

        }
    }

    public void CloseButtonClick()
    {
        this.gameObject.SetActive(false);
    }



    public bool IsNullOrEmpty(string value)
    {
        return value == null || value.Length == 0;
    }


    public void SendMessgaeSocket(string msg)
    {
        JSONObject obj = new JSONObject();
        obj.AddField("PlayerID", DataManager.Instance.playerData._id);
        obj.AddField("RoomId", TestSocketIO.Instace.roomid);
        obj.AddField("Message", msg);
        obj.AddField("gameName", gameName);
        TestSocketIO.Instace.Senddata("SendChatMessage", obj);
    }
}
