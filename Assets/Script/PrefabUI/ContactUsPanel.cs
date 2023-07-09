using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

//Line 127

public class ContactUsPanel : MonoBehaviour
{

    public Dropdown issueType;
    public InputField subIssueType;
    public InputField issueDetails;
    public Text message;

    public bool isPath;
    public string path1 = "";
    public GameObject ticketReceiveDialog;

    bool isSend = false;

    private void Start()
    {
        MainMenuManager.Instance.screenObj.Add(this.gameObject);
        message.gameObject.SetActive(false);

        ticketReceiveDialog.SetActive(false);

    }


    public void RaiseATicketButttonClick()
    {

        SoundManager.Instance.ButtonClick();
        string msg = "";
        if (issueType.captionText.text == "")
        {
            msg = "Please fill the your issue";
        }
        else if (subIssueType.text == "")
        {
            msg = "Please fill the your sub issue";
        }
        else if (issueDetails.text == "")
        {
            msg = "Please fill the issue details";
        }
        else
        {
            print("Raise A Ticket");
            //print(PlayerPrefs.GetString("token"));
            StartCoroutine(SendSupportMSg());
        }

        if (msg != "")
        {

            message.gameObject.SetActive(true);
            message.text = msg;
            print("Message : " + msg);
        }
    }

    IEnumerator SendSupportMSg()
    {
        WWWForm form = new WWWForm();
        form.AddField("subject", issueType.captionText.text);
        form.AddField("description", issueDetails.text);
        if (path1 != null && path1 != "")
        {
            form.AddBinaryData("file", File.ReadAllBytes(path1), DataManager.Instance.playerData._id + "Ticket.jpg");
        }
        //print(form);
        UnityWebRequest request = UnityWebRequest.Post(DataManager.Instance.url + "/api/v1/players/ticket/add", form);
        request.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString("token"));

        yield return request.SendWebRequest();


        if (request.error == null && !request.isNetworkError)
        {

            Logger.log.Log("Support", request.error);
            Logger.log.Log("Support", request.downloadHandler.text);
            JSONNode keys = JSON.Parse(request.downloadHandler.text.ToString());
            Logger.log.Log("Support", keys["success"]);

            if (keys["success"] == true)
            {
                message.gameObject.SetActive(true);
                ticketReceiveDialog.SetActive(true);
                message.text = "Request Send successfull";
                isSend = true;

            }
        }
    }
    public void PlusButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        GetImageButton();
    }

    public void CloseScreenShot()
    {
        path1 = "";
        //screenShotImg.gameObject.SetActive(false);
        //shadowImg.gameObject.SetActive(false);
    }
    public void BackButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        MainMenuManager.Instance.screenObj.Remove(this.gameObject);
        Destroy(this.gameObject);
    }
    #region Image Upload To Server


    public void GetImageButton()
    {
        //path1 = "/Users/greejesh/Desktop/Screenshot 2022-04-30 at 5.19.29 PM.png";
        //NativeGallery.GetImageFromGallery(GetImagePath);
    }


    public void GetImagePath(string path)
    {
        print("String Path : " + path);
        path1 = path;
        //GetImage(screenShotImg);
    }

    public void GetImage(RawImage img)
    {

        //screenShotImg.gameObject.SetActive(true);
        //shadowImg.gameObject.SetActive(true);
        //screenShotImg.texture = NativeGallery.LoadImageAtPath(path1);
    }


    // Call Function

    //UploadFile(path1, m_URL);



    #endregion


}
