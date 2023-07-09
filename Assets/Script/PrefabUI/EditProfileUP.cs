using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using WebSocketSharp;

public class EditProfileUP : MonoBehaviour
{
    // Start is called before the first frame update


    public InputField mobileInput;
    public Text nameTxt;
    public Text emailTxt;
    public Text joinTxt;
    public Image ProfileImage;

    public Text msgTextRefer;

    public InputField refferalField;

    public GameObject add_Profile_1;
    public GameObject add_Profile_2;

    [Header("--- OTP Screen ---")]
    public GameObject otpScreenObj;
    public InputField otpField;
    public GameObject sendBtnObj;
    public Text sendBtnTxt;
    public Text timerTxt;
    public Text errorOTPTxt;
    public float timerValue;
    public float secondsCount;
    bool isOpenOTP;
    public string sendStr;
    public string reSendStr;

    public GameObject ProfilePrefab;
    public GameObject profileParent;


    void Start()
    {
        MainMenuManager.Instance.screenObj.Add(this.gameObject);
        mobileInput.text = DataManager.Instance.playerData.phone;
        if (mobileInput.text != null && mobileInput.text.Length != 0)
        {
            add_Profile_2.SetActive(true);
            add_Profile_1.SetActive(false);
        }
        else
        {
            add_Profile_1.SetActive(true);
            add_Profile_2.SetActive(false);
        }


        nameTxt.text = "Name : " + DataManager.Instance.playerData.firstName;
        emailTxt.text = "Email : " + DataManager.Instance.playerData.email;
        string curDateStr = DateTime.Parse(DataManager.Instance.playerData.createdAt).ToLocalTime().ToString();
        DateTime dateT1 = DateTime.Parse(curDateStr.Split(" ")[0]);
        DateTime dateT2 = DateTime.Parse(curDateStr.Split(" ")[1]);
        joinTxt.text = "Joined : " + dateT1.ToString("dd") + " " + dateT1.ToString("MMM") + " " + dateT1.ToString("yyyy") + "-" + dateT2.ToString("hh:mm tt");

        StartCoroutine(GetImages(PlayerPrefs.GetString("ProfileURL"), ProfileImage));
    }
    IEnumerator GetImages(string URl, Image image)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(URl);
        yield return request.SendWebRequest();

        if (request.error == null)
        {
            if (image != null)
            {
                var texture = DownloadHandlerTexture.GetContent(request);
                Rect rect = new Rect(0, 0, texture.width, texture.height);
                image.sprite = Sprite.Create(texture, rect, new Vector2(0, 0));
                image.color = new Color(255, 255, 255, 255);
            }
        }
    }

    public void BackButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        MainMenuManager.Instance.screenObj.Remove(this.gameObject);

        Destroy(this.gameObject);

    }

    #region Refferal Dialog


    public void RefferButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        if (refferalField.text == null)
        {
            msgTextRefer.text = "Please Enter Refer Code";

            Invoke(nameof(ReferMessageTxtNull), 3f);

        }
        else
        {
            RefferSendServer(refferalField.text);

        }
    }




    public void RefferSendServer(string refferValue)
    {
        StartCoroutine(SendRefferal(refferValue));
    }

    IEnumerator SendRefferal(string refferValue)
    {
        WWWForm form = new WWWForm();
        form.AddField("referId", refferValue.ToString());
        //WaitPanelManager.Instance.OpenPanel();

        UnityWebRequest request = UnityWebRequest.Post(DataManager.Instance.url + "/api/v1/players/refer", form);
        request.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString("token"));

        yield return request.SendWebRequest();
        if (request.error == null)
        {
            JSONNode values = JSON.Parse(request.downloadHandler.text.ToString());
            if (values["success"] == true)
            {
                //WaitPanelManager.Instance.ClosePanel();

                //DataManager.Instance.playerData.balance = data["balance"].ToString().Trim('"');
                //Balance_Txt.text = Datamanger.Intance.balance.ToString().Trim('"');
            }
            else
            {
                //WaitPanelManager.Instance.ClosePanel();
                msgTextRefer.text = "Invalid Refer Code";
                Invoke(nameof(ReferMessageTxtNull), 3f);
            }
        }
        else
        {
            //WaitPanelManager.Instance.ClosePanel();
            msgTextRefer.text = "Invalid Refer Code";
            Invoke(nameof(ReferMessageTxtNull), 3f);
            //Reffer code is invalid.
        }
    }


    #endregion

    void ReferMessageTxtNull()
    {
        msgTextRefer.text = "";
    }

    #region Save Profile Button

    public void SaveButtonClick()
    {
        if (mobileInput.text.IsNullOrEmpty() || mobileInput.text.Length < 10)
        {
            msgTextRefer.text = "Please Enter Mobile No";
            Invoke(nameof(ReferMessageTxtNull), 3f);
        }
        else if (mobileInput.text.Length == 10)
        {
            //StartCoroutine(Profiledatasave());

            OpenOTPScreen(mobileInput.text);
        }
    }

    IEnumerator Profiledatasave()
    {
        WWWForm form = new WWWForm();
        form.AddField("phone", mobileInput.text);
        //WaitPanelManager.Instance.OpenPanel();

        UnityWebRequest request = UnityWebRequest.Post(DataManager.Instance.url + "/api/v1/players/profile", form);
        request.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString("token"));
        //Datamanger.Intance.Avtar = Avtarint;
        yield return request.SendWebRequest();

        if (request.error == null && !request.isNetworkError)
        {
            //WaitPanelManager.Instance.ClosePanel();

            JSONNode values = JSON.Parse(request.downloadHandler.text.ToString());
            print(request.downloadHandler.text);
            Logger.log.Log("Save Data", values.ToString());



            JSONNode datas = JSON.Parse(values["data"].ToString());
            //Debug.Log("User Data===:::" + datas.ToString());

            if (datas != null)
            {
                MainMenuManager.Instance.Setplayerdata(datas, false);
                MainMenuManager.Instance.screenObj.Remove(this.gameObject);
                Destroy(this.gameObject);
            }
            else
            {
                msgTextRefer.text = (values["error"]);
                Invoke(nameof(ReferMessageTxtNull), 3f);
            }
        }
        else
        {
            Logger.log.Log(request.error.ToString());
        }

    }


    #endregion

    #region OTP Maintain

    string mobileNo = "";
    public void OpenOTPScreen(string phone)
    {
        otpScreenObj.SetActive(true);
        sendBtnTxt.text = sendStr;
        sendBtnObj.SetActive(true);
        mobileNo = phone;

    }

    private void Update()
    {
        if (isOpenOTP)
        {
            if (secondsCount > 0)
            {
                secondsCount -= Time.deltaTime;
                float seconds = secondsCount % 60;


                string Sec = Mathf.RoundToInt(seconds).ToString();
                timerTxt.text = Sec + "S";
            }
            else
            {
                timerTxt.text = "";
                sendBtnObj.SetActive(true);
                sendBtnTxt.text = reSendStr;
                isOpenOTP = false;
            }

        }
    }

    public void SendButtonClick(Text btnTxt)
    {
        SoundManager.Instance.ButtonClick();
        if (btnTxt.text.Equals(sendStr))
        {
            sendBtnObj.SetActive(false);
        }
        else
        {
            sendBtnObj.SetActive(false);
        }
        isOpenOTP = true;
        secondsCount = timerValue;
        StartCoroutine(sendOTP("91" + mobileNo));

    }

    public void OTPVerifyButtonClick()
    {
        if (otpField.text.Length == 4)
        {
            StartCoroutine(PhoneVerify("91" + mobileNo, otpField.text));
        }
        else
        {
            errorOTPTxt.text = "Please Enter OTP";
            Invoke(nameof(OTPMessage), 4f);

        }
    }

    void OTPMessage()
    {
        errorOTPTxt.text = "";
    }


    IEnumerator sendOTP(string phoneNo)
    {
        WWWForm form = new WWWForm();
        form.AddField("phone", phoneNo); ;

        print("Send OTP phoneNo : " + phoneNo);
        UnityWebRequest request = UnityWebRequest.Post(DataManager.Instance.url + "/api/v1/players/sendotp", form);
        //request.SetRequestHeader("Authorization", "Bearer " + phoneNo);
        request.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString("token"));
        //Datamanger.Intance.Avtar = Avtarint;
        yield return request.SendWebRequest();

        if (request.error == null && !request.isNetworkError)
        {
            JSONNode values = JSON.Parse(request.downloadHandler.text.ToString());
            print("Send OTP : " + request.downloadHandler.text);
            Logger.log.Log("Save Data", values.ToString());
            if (values["success"] == true)
            {
                print("Enter The First Condition");

            }
            else
            {
                print("Enter The Second Condition");

                errorOTPTxt.text = values["error"];
                Invoke(nameof(OTPMessage), 4f);
            }

            //Debug.Log("User Data===:::" + datas.ToString());
            //Setplayerdata(datas, false);
        }
        else
        {

            errorOTPTxt.text = request.error.ToString();
            Invoke(nameof(OTPMessage), 4f);
        }

    }


    IEnumerator PhoneVerify(string phoneNo, string code)
    {

        //WaitPanelManager.Instance.OpenPanel();
        WWWForm form = new WWWForm();
        form.AddField("phone", phoneNo);
        form.AddField("code", code);


        UnityWebRequest request = UnityWebRequest.Post(DataManager.Instance.url + "/api/v1/auth/player/verify", form);
        request.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString("token"));
        //Datamanger.Intance.Avtar = Avtarint;
        yield return request.SendWebRequest();

        if (request.error == null && !request.isNetworkError)
        {
            JSONNode values = JSON.Parse(request.downloadHandler.text.ToString());
            print("Verify Message : " + values.ToString());
            //WaitPanelManager.Instance.ClosePanel();

            if (values["success"] == true)
            {
                otpScreenObj.SetActive(false);

                add_Profile_1.SetActive(false);
                add_Profile_2.SetActive(true);

            }
            else
            {
                errorOTPTxt.text = values["error"];
                Invoke(nameof(OTPMessage), 4f);
            }

            //Debug.Log("User Data===:::" + datas.ToString());
            //Setplayerdata(datas, false);
        }
        else
        {
            //WaitPanelManager.Instance.ClosePanel();
            errorOTPTxt.text = request.error.ToString();
            Invoke(nameof(OTPMessage), 4f);
        }

    }
    #endregion

    public void OpenProfileWindow()
    {
        Instantiate(ProfilePrefab, profileParent.transform);
    }
}
