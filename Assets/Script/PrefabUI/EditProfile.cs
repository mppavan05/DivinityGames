using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using WebSocketSharp;

public class EditProfile : MonoBehaviour
{

    public string geneder;

    //public Image[] allAvatarImg;
    //public Color avatarOn;
    //public Color avatarOff;

    public Dropdown genderDrop;

    public InputField firstNameInput;
    //public InputField lastNameInput;
    public InputField emailInput;
    public InputField panInput;
    public InputField aadharInput;
    public InputField phoneInput;
    public Dropdown stateInput;

    //public GameObject vipLabel;

    public Text idTxt;
    public Text timeTxt;
    public Text kycStatusTxt;
    public GameObject kycGreenObj;

    public Text msg;

    public Text birthDateTxt;
    public Color birthDateColorOff;
    public Color birthDateColorOn;

    public GameObject birthDialogObj;
    public Text birthText;
    public Text birthMessageTxt;
    public DatePickerControl datePic;
    int ano = 0;

    void OnEnable()
    {
        MainMenuManager.Instance.screenObj.Add(this.gameObject);
        msg.text = "";
        StartDataSet();
        //Getdata();
    }


    void StartDataSet()
    {
        ano = DataManager.Instance.GetAvatarValue();
        //if (DataManager.Instance.playerData.membership == "free")
        //{
        //    vipLabel.gameObject.SetActive(false);
        //}
        //else
        //{
        //    vipLabel.gameObject.SetActive(true);
        //}

        print("State  : " + DataManager.Instance.playerData.state);
        for (int i = 0; i < stateInput.options.Count; i++)
        {
            if (DataManager.Instance.playerData.state.Equals(stateInput.options[i].text))
            {
                print("State Inpur Set: " + i);
                stateInput.value = i;
            }
        }
        //for (int i = 0; i < allAvatarImg.Length; i++)
        //{
        //    if (i == ano)
        //    {
        //        allAvatarImg[i].transform.GetChild(0).gameObject.SetActive(true);
        //        DataManager.Instance.SetAvatarValue(i);
        //    }
        //    else
        //    {
        //        allAvatarImg[i].transform.GetChild(0).gameObject.SetActive(false);
        //    }
        //}

        if (DataManager.Instance.playerData.firstName.IsNullOrEmpty())
        {
            firstNameInput.text = DataManager.Instance.GetDefaultPlayerName();
        }
        else
        {
            firstNameInput.text = DataManager.Instance.playerData.firstName;
        }
        //lastNameInput.text = DataManager.Instance.playerData.lastName;
        geneder = DataManager.Instance.playerData.gender;
        panInput.text = DataManager.Instance.playerData.panNumber;
        emailInput.text = DataManager.Instance.playerData.email;
        aadharInput.text = DataManager.Instance.playerData.aadharNumber;

        phoneInput.interactable = false;
        phoneInput.text = DataManager.Instance.playerData.phone;
        idTxt.text = "ID: " + DataManager.Instance.playerData._id.Substring(0, 15);


        string curDateStr = DateTime.Parse(DataManager.Instance.playerData.createdAt).ToLocalTime().ToString();
        DateTime dateT1 = DateTime.Parse(curDateStr.Split(" ")[0]);
        DateTime dateT2 = DateTime.Parse(curDateStr.Split(" ")[1]);
        timeTxt.text = "Joined : " + dateT1.ToString("dd") + " " + dateT1.ToString("MMM") + " " + dateT1.ToString("yyyy") + "-" + dateT2.ToString("hh:mm tt");

        if (DataManager.Instance.playerData.kycStatus == "notverified")
        {
            kycStatusTxt.text = "KYC Status: Not Verified";
            kycGreenObj.SetActive(false);

        }
        else if (DataManager.Instance.playerData.kycStatus == "verified")
        {
            kycStatusTxt.text = "KYC Status: Verified";
            kycGreenObj.SetActive(true);

        }
        if (geneder == "male")
        {
            genderDrop.value = 0;
        }
        else if (geneder == "female")
        {
            genderDrop.value = 1;
        }
        else if (geneder == "other")
        {
            genderDrop.value = 2;
        }
        else
        {
            genderDrop.value = 0;
        }
        if (DataManager.Instance.playerData.dob == "none")
        {
            birthDateTxt.text = "Select a Birthdate";
            birthDateTxt.color = birthDateColorOff;
        }
        else
        {
            birthDateTxt.text = DataManager.Instance.playerData.dob;
            birthDateTxt.color = birthDateColorOn;
            //print("Error Date Text : "+birthText.text);
            //int dayNo = int.Parse(birthText.text.Split("-")[0]);
            //int monthNo = int.Parse(birthText.text.Split("-")[1]);
            //int yearNo = int.Parse(birthText.text.Split("-")[2]);

            //DateTime bDay = new DateTime(yearNo, monthNo, dayNo);

            //    birthDateTxt.text = DataManager.Instance.DateConvertter(bDay);

        }
    }

    public void BackButtonClick()
    {

        SoundManager.Instance.ButtonClick();
        MainMenuManager.Instance.screenObj.Remove(this.gameObject);
        //MainMenuManager.Instance.TopBarDataSet();
        MainMenuManager.Instance.Getdata();
        this.gameObject.SetActive(false);
        Destroy(this.gameObject);

    }

    public void DropDownCheckButton(GameObject obj)
    {

        SoundManager.Instance.ButtonClick();
        GameObject lastObj = obj.transform.GetChild(obj.transform.childCount - 1).gameObject;
        if (lastObj.name == "Dropdown List")
        {
            lastObj.GetComponent<RectTransform>().sizeDelta = new Vector2(lastObj.GetComponent<RectTransform>().sizeDelta.x, 286f);
        }
    }
    public void DropDownGetValue(int no)
    {

        SoundManager.Instance.ButtonClick();
        if (no == 0)
        {
            geneder = "male";
        }
        else if (no == 1)
        {
            geneder = "female";
        }
        else if (no == 2)
        {
            geneder = "other";
        }
    }


    public void OpenBirthDateButton()
    {
        SoundManager.Instance.ButtonClick();
        OpenBirthDialog();
    }



    //public void AvatarButtonClick(int no)
    //{
    //    SoundManager.Instance.ButtonClick();
    //    for (int i = 0; i < allAvatarImg.Length; i++)
    //    {
    //        if (i == no)
    //        {
    //            allAvatarImg[i].transform.GetChild(0).gameObject.SetActive(true);
    //            ano = i;
    //        }
    //        else
    //        {
    //            allAvatarImg[i].transform.GetChild(0).gameObject.SetActive(false);
    //        }
    //    }
    //}

    public void SaveProfileButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        if (firstNameInput.text == "")
        {
            MsgPush("Please Fill First Name Field");
        }
        //else if (lastNameInput.text == "")
        //{
        //    MsgPush("Please Fill Last Name Field");
        //}
        else if (emailInput.text == "")
        {
            MsgPush("Please Fill Email Field");
        }
        else if (panInput.text == "")
        {
            MsgPush("Please Fill Pan Number Field");
        }
        else if (aadharInput.text == "")
        {
            MsgPush("Please Fill Aadhar Field");
        }
        else
        {
            if (genderDrop.value == 0)
            {
                geneder = "male";
            }
            else if (genderDrop.value == 1)
            {
                geneder = "female";
            }
            else if (genderDrop.value == 2)
            {
                geneder = "other";
            }
            DataManager.Instance.playerData.firstName = firstNameInput.text;
            //DataManager.Instance.playerData.lastName = lastNameInput.text;
            DataManager.Instance.playerData.gender = geneder;
            DataManager.Instance.playerData.panNumber = panInput.text;
            DataManager.Instance.playerData.email = emailInput.text;
            DataManager.Instance.playerData.aadharNumber = aadharInput.text;
            DataManager.Instance.playerData.state = stateInput.captionText.text;
            DataManager.Instance.playerData.country = "91";
            if (birthDateTxt.text == "Select a Birthdate")
            {
                DataManager.Instance.playerData.dob = "none";
            }
            else
            {
                DataManager.Instance.playerData.dob = birthDateTxt.text;
            }



            DataManager.Instance.SetAvatarValue(ano);
            SavePlayerProfile();

        }
    }
    void MsgPush(string message)
    {
        msg.text = message;
        Invoke(nameof(MsgOff), 2.5f);
    }

    void MsgOff()
    {
        msg.text = "";
    }


    #region Birth Dialog

    public void OpenBirthDialog()
    {

        datePic.fechaHoy();
        birthMessageTxt.text = "";

        birthDialogObj.SetActive(true);
    }


    public void CloseBirthDialog()
    {
        SoundManager.Instance.ButtonClick();
        birthDialogObj.SetActive(false);
    }

    public void SelectBirthDialog()
    {
        string Split = "/";
        int indNo1 = 1;
        int indNo2 = 0;
        if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor)
        {
            Split = "-";
            indNo1 = 0;
            indNo2 = 1;
        }
        int dayNo = int.Parse(birthText.text.Split(Split)[0]);
        int monthNo = int.Parse(birthText.text.Split(Split)[1]);
        int yearNo = int.Parse(birthText.text.Split(Split)[2]);

        print("Birth Text : " + birthText.text);
        DateTime date = DateTime.Now;
        string curDate = date.ToString();
        print("Current Date Text : " + curDate);

        int cur_Date = int.Parse(curDate.Split(" ")[0].Split(Split)[indNo1]);
        //int currHour = ;
        int cur_month = int.Parse(curDate.Split(" ")[0].Split(Split)[indNo2]);
        int cur_year = int.Parse(curDate.Split(" ")[0].Split(Split)[2]);

        DateTime bDay = new DateTime(yearNo, monthNo, dayNo);
        DateTime now = new DateTime(cur_year, cur_month, cur_Date);
        if (Age(bDay, now) >= 18)
        {
            CloseBirthDialog();
            birthDateTxt.text = birthText.text;
            birthDateTxt.color = birthDateColorOn;


        }
        else
        {
            MessageBirth();
        }


    }

    public int Age(DateTime birthDate, DateTime laterDate)
    {
        int age;
        age = laterDate.Year - birthDate.Year;

        if (age > 0)
        {
            age -= Convert.ToInt32(laterDate.Date < birthDate.Date.AddYears(age));
        }
        else
        {
            age = 0;
        }

        return age;
    }



    void MessageBirth()
    {
        birthMessageTxt.text = "Please Select a Valid Date";
        Invoke(nameof(birthMessageTxt), 3f);
    }

    void TextClose()
    {
        birthMessageTxt.text = "";
    }

    #endregion


    public void SavePlayerProfile()
    {
        StartCoroutine(Profiledatasave());
    }
    IEnumerator Profiledatasave()
    {
        WWWForm form = new WWWForm();
        form.AddField("firstName", DataManager.Instance.playerData.firstName);
        form.AddField("gender", DataManager.Instance.playerData.gender);
        form.AddField("email", DataManager.Instance.playerData.email);
        form.AddField("state", DataManager.Instance.playerData.state);
        form.AddField("panNumber", DataManager.Instance.playerData.panNumber);
        form.AddField("aadharNumber", DataManager.Instance.playerData.aadharNumber);
        if (DataManager.Instance.playerData.dob != "none")
        {
            form.AddField("dob", DataManager.Instance.playerData.dob);
        }
        form.AddField("country", DataManager.Instance.playerData.country);
        UnityWebRequest request = UnityWebRequest.Post(DataManager.Instance.url + "/api/v1/players/profile", form);
        request.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString("token"));
        //Datamanger.Intance.Avtar = Avtarint;
        yield return request.SendWebRequest();

        if (request.error == null && !request.isNetworkError)
        {
            JSONNode values = JSON.Parse(request.downloadHandler.text.ToString());
            print(request.downloadHandler.text);
            Logger.log.Log("Save Data", values.ToString());

            if (values["success"] == false)
            {
                MsgPush(values["error"]);
            }
            else
            {

                JSONNode datas = JSON.Parse(values["data"].ToString());
                //Debug.Log("User Data===:::" + datas.ToString());
                MainMenuManager.Instance.Setplayerdata(datas, false);

                MainMenuManager.Instance.screenObj.Remove(this.gameObject);
                MainMenuManager.Instance.Getdata();
                Destroy(this.gameObject);
            }
        }
        else
        {
            Logger.log.Log(request.error.ToString());
        }

    }


}
