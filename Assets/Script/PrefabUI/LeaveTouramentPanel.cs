using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaveTouramentPanel : MonoBehaviour
{
    void Start()
    {
        MainMenuManager.Instance.screenObj.Add(this.gameObject);
    }

    public void BackButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        MainMenuManager.Instance.screenObj.Remove(this.gameObject);
        //MainMenuManager.Instance.Tournament_Back_Main_Leave();
        //MainMenuManager.Instance.OpenHome();

        Destroy(this.gameObject);
    }

    public void LeaveButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        MainMenuManager.Instance.screenObj.Remove(this.gameObject);
        if (TestSocketIO.Instace.roomid != null && TestSocketIO.Instace.roomid.Length != 0)
        {
            MainMenuManager.Instance.UserRefund();
            TestSocketIO.Instace.LeaveRoom();
        }
        //MainMenuManager.Instance.Tournament_Back_Main_Leave();
        //MainMenuManager.Instance.OpenHome();
        Destroy(this.gameObject);

    }
}
