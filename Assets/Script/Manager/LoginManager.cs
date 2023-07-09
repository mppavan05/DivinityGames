using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using SimpleJSON;
using UnityEngine.SceneManagement;
using WebSocketSharp;

public class LoginManager : MonoBehaviour
{
    public static LoginManager Instance;

    public bool testMode;
    //public GameObject ludoSignFirstScreenObj;

    [Header("Mobile Number Screen"), Space(5)]
    //public GameObject mobileLoginScreenObj;
    public InputField mobileInput;
    public string termUrl;
    public Text msgText;

    [Header("OTP Screen"), Space(5)]
    //public GameObject otpScreenObj;
    public InputField otpInput;
    //public Sprite rightOtpBox;
    //public GameObject tickImageObj;
    public Text msgOtpTxt;


    //[Header("About Tell Screen"), Space(5)]
    //public GameObject aboutTellScreenObj;
    //public InputField nameInput;
    //public Dropdown stateDropDown;
    //public Text msgDropDown;

    //[Header("Pin Dialog Details"), Space(5)]
    //public GameObject pinDialog;
    //public Text pinDialogTitle;
    //public Text pinButtonTxt;
    //public InputField pinInputField;
    //public Text msgTextPin;

    //[Header("Referral Screen"), Space(5)]
    //public GameObject refferalDialog;
    //public InputField refferalField;
    //public Text msgTextRefer;

    [Header("Loading Panel"), Space(5)]
    public GameObject loadingPanel;

    public string googleUserName;
    public string googleUserEmail;

    public string googleUserEmailDemo;

    public Text errorMSGTxt;

    [Header("-- Test --")]
    public string testEmail;
    public string testUserName;
    public string testImageUrl;
    public string testFirebaseToken;
    public string testDeviceToken;


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
        if (DataManager.Instance.GetLoginValue() == "Y")
        {
            //OpenPinDialog(2);
            //LudoSignFirstScreen();

            PlayerPrefs.SetInt("OpenReffer", 1);
            LoadSceneMainMenu();
            //ludoSignFirstScreenObj.SetActive(false);
            //mobileLoginScreenObj.SetActive(false);
            //ludoSignFirstScreenObj.SetActive(false);

        }
        else
        {
            //LudoSignFirstScreen();

        }
    }

    // Update is called once per frame
    void Update()
    {
    }


    IEnumerator MessageTextClenar(Text t)
    {
        yield return new WaitForSeconds(3f);
        t.text = "";
    }

    void SendCustomMessage(Text t, string msg, bool isOff)
    {
        t.text = msg;
        if (isOff)
        {
            StartCoroutine(MessageTextClenar(t));
        }
    }




    public void email_Main(string firebaseToken, string firstName, string picture)
    {

        if (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.WindowsEditor)
        {

            print("Enter The Windows Condition");
            googleUserEmail = testEmail;

            GetUserEmail(googleUserEmail, testDeviceToken, firebaseToken, firstName, picture);
        }
        else
        {

            GetUserEmail(googleUserEmail, "123452@#$%", firebaseToken, firstName, picture);
        }


    }

    void GetUserEmail(string email, string deviceToken, string firebaseToken, string firstName, string picture)
    {
        Root_PlayerRegisterEmail instance = new Root_PlayerRegisterEmail();
        instance.email = googleUserEmail;

        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            instance.deviceToken = deviceToken;
        }
        else
        {
            instance.deviceToken = SystemInfo.deviceUniqueIdentifier;
        }
        instance.countryCode = "91";
        instance.firebaseToken = firebaseToken;
        instance.firstName = firstName;
        instance.picture = picture;


        string create_Json = JsonUtility.ToJson(instance);
        Debug.Log(create_Json);
        StartCoroutine(RigisterNewUserEmail(DataManager.Instance.url + "/api/v1/auth/player/registeremail", create_Json));
        //StartCoroutine(CheckUserListEmail(email, deviceToken, firebaseToken));
    }

    IEnumerator CheckUserListEmail(string email, string deviceToken, string firebaseToken)
    {
        string url = DataManager.Instance.url + "/api/v1/auth/getbyemail?email=" + email + "&deviceToken=" + deviceToken;
        print("user Url : " + url);
        UnityWebRequest request = UnityWebRequest.Get(url);
        //request.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString("token"));
        yield return request.SendWebRequest();
        JSONNode values = JSON.Parse(request.downloadHandler.text.ToString());
        print("User request.downloadHandler.text : " + request.downloadHandler.text);
        if (values["error"].Equals("Player not found"))
        {
            Root_PlayerRegisterEmail instance = new Root_PlayerRegisterEmail();
            instance.email = googleUserEmail;
            if (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.WindowsEditor)
            {
                instance.deviceToken = testDeviceToken;
            }
            else
            {
                instance.deviceToken = SystemInfo.deviceUniqueIdentifier;
            }
            instance.countryCode = "91";
            instance.firebaseToken = firebaseToken;

            string create_Json = JsonUtility.ToJson(instance);
            Debug.Log(create_Json);
            StartCoroutine(RigisterNewUserEmail(DataManager.Instance.url + "/api/v1/auth/player/registeremail", create_Json));
        }
        else
        {
            //print("Enter The New Player 1");

            PlayerPrefs.SetString("playerId", values["data"]["_id"].Value);

            //ludoSignFirstScreenObj.SetActive(false);
            //mobileLoginScreenObj.SetActive(false);
            //OpenPinDialog(2);
            //Fetch Data Direct On Pin
        }
        //print("User Data : " + request.downloadHandler.text.ToString());
    }


    IEnumerator RigisterNewUserEmail(string url, string bodyJsonString)
    {

        UnityWebRequest request = UnityWebRequest.Post(url, bodyJsonString);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();
        Debug.Log(url);
        Debug.Log("Register Json: " + request.downloadHandler.text);
        if (!request.isNetworkError && !request.isHttpError)// && request.responseCode == 200) // 200, 404 or 500. (In Unity Only)
        {
            loadingPanel.SetActive(false);
            Debug.Log("Login API working  - /api/v1/auth/player/registeremail");
            Debug.Log("Register Json: " + request.downloadHandler.text);
            JSONNode jsonNode = SimpleJSON.JSON.Parse(request.downloadHandler.text);
            if (jsonNode["success"] == true)
            {


                PlayerPrefs.SetString("playerId", jsonNode["playerId"].Value);
                //print("Token Check TIme : " + jsonNode["token"].Value);
                PlayerPrefs.SetString("token", jsonNode["token"].Value);

                //DataManager.Instance.SetDefaultPlayerName(googleUserName);
                //ludoSignFirstScreenObj.SetActive(false);

                DataManager.Instance.SetLoginEmail(1);
                //OpenTellAboutScreen();

                PlayerPrefs.SetInt("OpenReffer", 0);
                LoadSceneMainMenu();
                //nameInput.text = googleUserName;
                //StartCoroutine(Profiledatasave(nameInput.text, stateDropDown.captionText.text, true));

                //OpenPinDialog(1);

                SendCustomMessage(msgText, "You will recieve OTP code shortly.", false);
            }
            else if (jsonNode["success"] == false)
            {
                string error = jsonNode["error"];
                errorMSGTxt.text = error;
                Invoke(nameof(MsgOff), 5f);
            }
        }
        else
        {
            Debug.Log("Erro: " + request.error);
            Debug.Log("Error in - " + url);
            loadingPanel.SetActive(false);
        }
    }

    void MsgOff()
    {
        errorMSGTxt.text = "";

    }


    /*
      #region LudoSignFirst Screen 

    void LudoSignFirstScreen()
    {
        ludoSignFirstScreenObj.SetActive(true);
    }
    public void SignUpButtonClick()
    {
        print("Sign Up Button Click");
    }



    public void LoginButtonClick()
    {

        SoundManager.Instance.ButtonClick();
        ludoSignFirstScreenObj.SetActive(false);

        //mobileLoginScreenObj.SetActive(true);
        //OpenMobileLoginScreen();
    }

    #endregion
    #region Mobile Number Login Screen

    bool isMobileTick = false;
    void OpenMobileLoginScreen()
    {
        mobileLoginScreenObj.SetActive(true);
    }

    public void MobileTickButtonClick(GameObject tickObj)
    {
        SoundManager.Instance.ButtonClick();
        if (tickObj.activeSelf == false)
        {
            isMobileTick = true;
            tickObj.SetActive(true);
        }
        else
        {
            isMobileTick = false;
            tickObj.SetActive(false);
        }
    }

    public void TermsServiceButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        Application.OpenURL(termUrl);
    }

    public void Mobile_Number_Next_Button()
    {
        if (isMobileTick == false)
        {
            SendCustomMessage(msgText, "Please agree terms", true);
        }
        else if (mobileInput.text.Length < 10)
        {
            SendCustomMessage(msgText, "Invalid mobile number", true);
            return;
        }
        else if (isMobileTick == true)
        {
            SoundManager.Instance.ButtonClick();
            RegisterWithNewMobileNumber();
        }
    }

    #region Check User
    void GetUserPhone(string mobile, string deviceToken)
    {
        StartCoroutine(CheckUserListPhone(mobile, deviceToken));
    }

    IEnumerator CheckUserListPhone(string mobile, string deviceToken)
    {
        string url = DataManager.Instance.url + "/api/v1/auth/getbyphone?phone=" + mobile + "&deviceToken=" + deviceToken;

        UnityWebRequest request = UnityWebRequest.Get(url);
        //request.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString("token"));
        yield return request.SendWebRequest();
        JSONNode values = JSON.Parse(request.downloadHandler.text.ToString());
        print("User Main : " + request.downloadHandler.text.ToString());
        if (values["error"].Equals("Player not found"))
        {
            Root_PlayerRegister instance = new Root_PlayerRegister();
            instance.phone = mobile;
            instance.deviceToken = deviceToken;
            instance.countryCode = "91";
            instance.firebaseToken = "jhsjhjd";


            string create_Json = JsonUtility.ToJson(instance);
            Debug.Log(create_Json);

            StartCoroutine(RigisterNewUser(DataManager.Instance.url + "/api/v1/auth/player/register", create_Json));
            //New Player 
        }
        else
        {
            //print("Enter The New Player 1");

            PlayerPrefs.SetString("playerId", values["data"]["_id"].Value);
            ludoSignFirstScreenObj.SetActive(false);
            mobileLoginScreenObj.SetActive(false);
            OpenPinDialog(2);

            //Fetch Data Direct On Pin
        }
        //print("User Data : " + request.downloadHandler.text.ToString());
    }

    void GetUserEmail(string email, string deviceToken, string firebaseToken, string firstName, string picture)
    {
        Root_PlayerRegisterEmail instance = new Root_PlayerRegisterEmail();
        instance.email = googleUserEmail;

        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            instance.deviceToken = deviceToken;
        }
        else
        {
            instance.deviceToken = SystemInfo.deviceUniqueIdentifier;
        }
        instance.countryCode = "91";
        instance.firebaseToken = firebaseToken;
        instance.firstName = firstName;
        instance.picture = picture;


        string create_Json = JsonUtility.ToJson(instance);
        Debug.Log(create_Json);
        StartCoroutine(RigisterNewUserEmail(DataManager.Instance.url + "/api/v1/auth/player/registeremail", create_Json));
        //StartCoroutine(CheckUserListEmail(email, deviceToken, firebaseToken));
    }

    IEnumerator CheckUserListEmail(string email, string deviceToken, string firebaseToken)
    {
        string url = DataManager.Instance.url + "/api/v1/auth/getbyemail?email=" + email + "&deviceToken=" + deviceToken;
        print("user Url : " + url);
        UnityWebRequest request = UnityWebRequest.Get(url);
        //request.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString("token"));
        yield return request.SendWebRequest();
        JSONNode values = JSON.Parse(request.downloadHandler.text.ToString());
        print("User request.downloadHandler.text : " + request.downloadHandler.text);
        if (values["error"].Equals("Player not found"))
        {
            Root_PlayerRegisterEmail instance = new Root_PlayerRegisterEmail();
            instance.email = googleUserEmail;
            instance.deviceToken = SystemInfo.deviceUniqueIdentifier;
            instance.countryCode = "91";
            instance.firebaseToken = firebaseToken;

            string create_Json = JsonUtility.ToJson(instance);
            Debug.Log(create_Json);
            StartCoroutine(RigisterNewUserEmail(DataManager.Instance.url + "/api/v1/auth/player/registeremail", create_Json));
        }
        else
        {
            //print("Enter The New Player 1");

            PlayerPrefs.SetString("playerId", values["data"]["_id"].Value);

            ludoSignFirstScreenObj.SetActive(false);
            mobileLoginScreenObj.SetActive(false);
            OpenPinDialog(2);
            //Fetch Data Direct On Pin
        }
        //print("User Data : " + request.downloadHandler.text.ToString());
    }

    #endregion


    public void RegisterWithNewMobileNumber()
    {
        if (mobileInput.text == "")
        {
            SendCustomMessage(msgText, "Enter mobile number", true);
            return;
        }
        if (mobileInput.text.Length < 10)
        {
            SendCustomMessage(msgText, "Invalid mobile number", true);
            return;
        }


        GetUserPhone("91" + mobileInput.text, SystemInfo.deviceUniqueIdentifier);
        //loadingPanel.SetActive(true);

        //a

        //Root_PlayerRegister instance = new Root_PlayerRegister();
        //instance.phone = "91" + mobileInput.text;
        //instance.deviceToken = SystemInfo.deviceUniqueIdentifier;
        //instance.countryCode = "91";
        //instance.firebaseToken = "jhsjhjd";


        //string create_Json = JsonUtility.ToJson(instance);
        //Debug.Log(create_Json);

        //StartCoroutine(RigisterNewUser(DataManager.Instance.url + "/api/v1/auth/player/register", create_Json));
    }

    IEnumerator RigisterNewUser(string url, string bodyJsonString)
    {

        UnityWebRequest request = UnityWebRequest.Post(url, bodyJsonString);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();
        Debug.Log(url);
        Debug.Log("Register Json: " + request.downloadHandler.text);
        if (!request.isNetworkError && !request.isHttpError)// && request.responseCode == 200) // 200, 404 or 500. (In Unity Only)
        {
            loadingPanel.SetActive(false);
            Debug.Log("Login API working  - /api/v1/auth/player/register");
            Debug.Log("Register Json: " + request.downloadHandler.text);
            JSONNode jsonNode = SimpleJSON.JSON.Parse(request.downloadHandler.text);
            if (jsonNode["success"] == true)
            {

                OpenOTPScreen();
                SendCustomMessage(msgText, "You will recieve OTP code shortly.", false);
            }
        }
        else
        {
            Debug.Log("Erro: " + request.error);
            Debug.Log("Error in - " + url);
            loadingPanel.SetActive(false);
        }
    }

  




    IEnumerator RigisterNewUserEmail(string url, string bodyJsonString)
    {

        UnityWebRequest request = UnityWebRequest.Post(url, bodyJsonString);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();
        Debug.Log(url);
        Debug.Log("Register Json: " + request.downloadHandler.text);
        if (!request.isNetworkError && !request.isHttpError)// && request.responseCode == 200) // 200, 404 or 500. (In Unity Only)
        {
            loadingPanel.SetActive(false);
            Debug.Log("Login API working  - /api/v1/auth/player/registeremail");
            Debug.Log("Register Json: " + request.downloadHandler.text);
            JSONNode jsonNode = SimpleJSON.JSON.Parse(request.downloadHandler.text);
            if (jsonNode["success"] == true)
            {


                PlayerPrefs.SetString("playerId", jsonNode["playerId"].Value);
                //print("Token Check TIme : " + jsonNode["token"].Value);
                PlayerPrefs.SetString("token", jsonNode["token"].Value);

                //DataManager.Instance.SetDefaultPlayerName(googleUserName);
                ludoSignFirstScreenObj.SetActive(false);

                DataManager.Instance.SetLoginEmail(1);
                //OpenTellAboutScreen();

                PlayerPrefs.SetInt("OpenReffer", 0);
                LoadSceneMainMenu();
                //nameInput.text = googleUserName;
                //StartCoroutine(Profiledatasave(nameInput.text, stateDropDown.captionText.text, true));

                //OpenPinDialog(1);

                SendCustomMessage(msgText, "You will recieve OTP code shortly.", false);
            }
            else if (jsonNode["success"] == false)
            {
                string error = jsonNode["error"];
                errorMSGTxt.text = error;
                Invoke(nameof(MsgOff), 5f);
            }
        }
        else
        {
            Debug.Log("Erro: " + request.error);
            Debug.Log("Error in - " + url);
            loadingPanel.SetActive(false);
        }
    }


    void MsgOff()
    {
        errorMSGTxt.text = "";

    }


    public void CloseMobileNumberButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        mobileLoginScreenObj.SetActive(false);
        mobileInput.text = "";
        msgText.text = "";
        LudoSignFirstScreen();
    }

    #endregion

    #region OTP


    void OpenOTPScreen()
    {
        mobileLoginScreenObj.SetActive(false);
        otpScreenObj.SetActive(true);
        tickImageObj.SetActive(false);
        otpBoxInput.text = "";

    }

    public void OTPInputCustomManage(string s)
    {
        if (otpBoxInput.text.Length == 4)
        {
            OTPVerify();
        }
    }


    public void OTPNextButtonClick()
    {
        if (tickImageObj.activeSelf == false)
        {
            if (otpBoxInput.text == null || otpBoxInput.text.Length != 4 || otpBoxInput.text == "")
            {
                SendCustomMessage(msgOtpTxt, "Please enter OTP", true);
            }
            else if (otpBoxInput.text.Length == 4)
            {
                SendCustomMessage(msgOtpTxt, "Please wait otp checking...", true);
            }
        }
        else
        {
            OpenTellAboutScreen();
        }

        //tickImageObj.SetActive(true);
    }


    public void OTPVerify()
    {

        SoundManager.Instance.ButtonClick();
        if (testMode)
        {
            LoadSceneMainMenu();
        }
        else
        {

            Root_PlayerOTPVerify instance = new Root_PlayerOTPVerify();
            instance.phone = "91" + mobileInput.text;
            instance.code = otpBoxInput.text;
            instance.deviceType = "android";
            instance.deviceToken = SystemInfo.deviceUniqueIdentifier;
            instance.gamecode = "TeenPatti";

            string create_Json = JsonUtility.ToJson(instance);
            StartCoroutine(CheckOTPCode(DataManager.Instance.url + "/api/v1/auth/player/verify", create_Json));
        }
    }
    IEnumerator CheckOTPCode(string url, string bodyJsonString)
    {
        //loadingPanel.SetActive(true);
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();
        Debug.Log("CheckOTPCode: " + request.downloadHandler.text);

        if (!request.isNetworkError && !request.isHttpError)// && request.responseCode == 200) // 200, 404 or 500. (In Unity Only)
        {
            JSONNode jsonNode = SimpleJSON.JSON.Parse(request.downloadHandler.text);
            if (jsonNode["success"] == false)
            {
                //loadingPanel.SetActive(false);
                SendCustomMessage(msgOtpTxt, jsonNode["error"].Value, true);
            }
            else
            {
                //loadingPanel.SetActive(false);
                PlayerPrefs.SetString("mobilenumber", mobileInput.text);
                SendCustomMessage(msgOtpTxt, " ", true);
                tickImageObj.SetActive(true);


                //msgText.text = "Mobile Verified";

                PlayerPrefs.SetString("playerId", jsonNode["playerId"].Value);
                //print("Token Check TIme : " + jsonNode["token"].Value);

                PlayerPrefs.SetString("token", jsonNode["token"].Value);

                yield return new WaitForSeconds(1.0f);

                mobileLoginScreenObj.SetActive(false);

            }
        }
        else
        {
            Debug.Log("Erro: " + request.error);
            Debug.Log("Error in - " + url);
            loadingPanel.SetActive(false);
        }
    }

    #endregion

    #region Tell about

    void OpenTellAboutScreen()
    {
        otpScreenObj.SetActive(false);
        aboutTellScreenObj.SetActive(true);
        nameInput.text = googleUserName;
    }

    public void About_NextButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        if (nameInput.text == "")
        {
            print("Name : " + nameInput.text);
            msgDropDown.text = "Please enter your name";
            Invoke(nameof(MessageOff), 2.5f);

        }
        else
        {
            StartCoroutine(Profiledatasave(nameInput.text, stateDropDown.captionText.text, true));
        }
        //
        print("StateTxt : " + stateDropDown.captionText.text);
    }

    public void About_State_Click()
    {

    }

    void MessageOff()
    {
        msgDropDown.text = "";
    }

    #endregion


    

    #region SetData


    IEnumerator Profiledatasave(string name, string picture, bool isName)
    {
        WWWForm form = new WWWForm();

        form.AddField("firstName", name);
        if (DataManager.Instance.GetLoginEmail() == 1)
        {
            form.AddField("email", googleUserEmail);
        }
        form.AddField("picture", picture);

        //print("Url : " + DataManager.Instance.url + "/api/v1/players/profile");
        UnityWebRequest request = UnityWebRequest.Post(DataManager.Instance.url + "/api/v1/players/profile", form);
        request.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString("token"));
        //Datamanger.Intance.Avtar = Avtarint;
        yield return request.SendWebRequest();

        if (request.error == null && !request.isNetworkError)
        {

            JSONNode values = JSON.Parse(request.downloadHandler.text.ToString());
            //print("Satte Data : " + request.downloadHandler.text);
            Logger.log.Log("Save Data", values.ToString());

            JSONNode datas = JSON.Parse(values["data"].ToString());
            DataManager.Instance.playerData.firstName = datas["firstName"];


            //OpenPinDialog(1);
            //Debug.Log("User Data===:::" + datas.ToString());

        }
        else
        {
            Logger.log.Log(request.error.ToString());
        }

    }

    #endregion
    #region Pin Dialog
    //OpenPinDialog(1);
    void OpenPinDialog(int no)
    {
        //no=1 Set Pin
        //no=2 Enter Pin
        aboutTellScreenObj.SetActive(false);
        pinDialog.SetActive(true);
        if (no == 1)
        {
            pinDialogTitle.text = "SET" + " PASSCODE";
            pinButtonTxt.text = "VERIFY";

        }
        else if (no == 2)
        {
            pinDialogTitle.text = "ENTER" + " PASSCODE";
            pinButtonTxt.text = "PROCEED";
        }
    }

    public void PinDialogButtonClick(Text btnTxt)
    {
        SoundManager.Instance.ButtonClick();
        if (btnTxt.text == "VERIFY")
        {
            print("Click On Set");
            SetPin();
        }
        else if (btnTxt.text == "PROCEED")
        {
            print("Click On Login");
            CheckPin();
        }
    }

    void SetPin()
    {
        if (testMode)
        {
            LoadSceneMainMenu();
        }
        else
        {
            if (pinInputField.text.Length < 4)
            {
                msgTextPin.text = "Invalid Pin Number";
                return;
            }
            Root_SetPlayerPIN instance = new Root_SetPlayerPIN();
            instance.pin = pinInputField.text;
            string create_Json = JsonUtility.ToJson(instance);
            StartCoroutine(PinSet(DataManager.Instance.url + "/api/v1/players/pin", create_Json));
        }
    }
    IEnumerator PinSet(string url, string bodyJsonString)
    {
        yield return new WaitForSeconds(0.0f);
        loadingPanel.SetActive(true);
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString("token"));
        yield return request.SendWebRequest();
        Debug.Log("PinSet " + request.downloadHandler.text);
        if (!request.isNetworkError && !request.isHttpError)// && request.responseCode == 200) // 200, 404 or 500. (In Unity Only)
        {
            JSONNode jsonNode = SimpleJSON.JSON.Parse(request.downloadHandler.text);
            if (jsonNode["success"] == false)
            {
                loadingPanel.SetActive(false);
                msgTextPin.text = jsonNode["error"].Value;
            }
            else
            {
                loadingPanel.SetActive(false);
                msgTextPin.text = "Set Pin Successfully";

                yield return new WaitForSeconds(1.0f);
                pinDialog.SetActive(false);


                //OpenRefferalDialog();
                LoadSceneMainMenu();
            }
        }
        else
        {
            Debug.Log("Erro: " + request.error);
            Debug.Log("Error in - " + url);
            loadingPanel.SetActive(false);
        }
    }

    void CheckPin()
    {
        if (testMode)
        {
            LoadSceneMainMenu();
        }
        else
        {
            if (pinInputField.text.Length < 4)
            {
                msgTextPin.text = "Invalid Pin Number";
                return;
            }
            Root_CheckPlayerPIN instance = new Root_CheckPlayerPIN();
            instance.pin = pinInputField.text;
            instance.playerId = PlayerPrefs.GetString("playerId");
            string create_Json = JsonUtility.ToJson(instance);
            StartCoroutine(CheckPinNumber(DataManager.Instance.url + "/api/v1/players/checkpin", create_Json));
        }
    }

    IEnumerator CheckPinNumber(string url, string bodyJsonString)
    {
        yield return new WaitForSeconds(0.0f);
        loadingPanel.SetActive(true);
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();
        Debug.Log("CheckPinNumber " + request.downloadHandler.text);
        if (!request.isNetworkError && !request.isHttpError)// && request.responseCode == 200) // 200, 404 or 500. (In Unity Only)
        {
            JSONNode jsonNode = SimpleJSON.JSON.Parse(request.downloadHandler.text);
            if (jsonNode["success"] == false)
            {
                loadingPanel.SetActive(false);
                msgTextPin.text = jsonNode["error"].Value;
            }
            else
            {
                loadingPanel.SetActive(false);
                msgTextPin.text = "Login Successfully";
                Debug.Log("User Token:::::::::: " + jsonNode["token"].Value);
                PlayerPrefs.SetString("playerId", jsonNode["playerId"].Value);
                print("Token Check TIme : " + jsonNode["token"].Value);
                PlayerPrefs.SetString("token", jsonNode["token"].Value);
                yield return new WaitForSeconds(1.0f);

                LoadSceneMainMenu();
            }
        }
        else
        {
            Debug.Log("Erro: " + request.error);
            Debug.Log("Error in - " + url);
            loadingPanel.SetActive(false);
        }
    }

    public void ClosePinDialogClick()
    {
        pinDialog.SetActive(false);

        LudoSignFirstScreen();
    }


    #endregion

    #region Refferal Dialog

    public void OpenRefferalDialog()
    {
        refferalDialog.SetActive(true);
    }

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


    public void SkipButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        LoadSceneMainMenu();
    }

    public void RefferSendServer(string refferValue)
    {
        StartCoroutine(SendRefferal(refferValue));
    }

    IEnumerator SendRefferal(string refferValue)
    {
        WWWForm form = new WWWForm();
        form.AddField("referId", refferValue.ToString());
        UnityWebRequest request = UnityWebRequest.Post(DataManager.Instance.url + "/api/v1/players/refer", form);
        request.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString("token"));

        yield return request.SendWebRequest();
        if (request.error == null)
        {
            JSONNode values = JSON.Parse(request.downloadHandler.text.ToString());
            if (values["success"] == true)
            {

                LoadSceneMainMenu();
                //DataManager.Instance.playerData.balance = data["balance"].ToString().Trim('"');
                //Balance_Txt.text = Datamanger.Intance.balance.ToString().Trim('"');
            }
            else
            {
                msgTextRefer.text = "Invalid Refer Code";
                Invoke(nameof(ReferMessageTxtNull), 3f);
            }
        }
        else
        {
            msgTextRefer.text = "Invalid Refer Code";
            Invoke(nameof(ReferMessageTxtNull), 3f);
            //Reffer code is invalid.
        }
    }
    void ReferMessageTxtNull()
    {
        msgTextRefer.text = "";
    }


    #endregion


    */
    
    
    public void OTPButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        RegisterWithNewMobileNumber();
    }
    
    public void RegisterWithNewMobileNumber()
    {
        if (mobileInput.text == "")
        {
            msgText.gameObject.SetActive(true);
            msgText.text = "Enter mobile number";
            Invoke(nameof(MessageOffDialog), 2.5f);
            return;
        }
        if (mobileInput.text.Length < 10)
        {
            msgText.gameObject.SetActive(true);
            msgText.text = "Invalid mobile number";
            Invoke(nameof(MessageOffDialog), 2.5f);
            return;
        }

        GetUserPhone("91" + mobileInput.text, SystemInfo.deviceUniqueIdentifier);
        //loadingPanel.SetActive(true);

        /*Root_PlayerRegister instance = new Root_PlayerRegister();
        instance.phone = "91" + mobileInput.text;
        instance.deviceToken = SystemInfo.deviceUniqueIdentifier;
        instance.countryCode = "91";
        instance.firebaseToken = "jhsjhjd";

        string create_Json = JsonUtility.ToJson(instance);
        Debug.Log(create_Json);

        StartCoroutine(RigisterNewUser(DataManager.Instance.url + "/api/v1/auth/player/register", create_Json));*/
    }

    IEnumerator RigisterNewUser(string url, string bodyJsonString)
    {
        UnityWebRequest request = UnityWebRequest.Post(url, bodyJsonString);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();
        Debug.Log(url);
        Debug.Log("Register Json: " + request.downloadHandler.text);
        if (!request.isNetworkError && !request.isHttpError)// && request.responseCode == 200) // 200, 404 or 500. (In Unity Only)
        {
            loadingPanel.SetActive(false);
            Debug.Log("Login API working  - /api/v1/auth/player/register");
            Debug.Log("Register Json: " + request.downloadHandler.text);
            JSONNode jsonNode = SimpleJSON.JSON.Parse(request.downloadHandler.text);
            if (jsonNode["success"] == true)
            {
                msgText.text = "You will recieve OTP code shortly.";
            }
        }
        else
        {
            Debug.Log("Erro: " + request.error);
            Debug.Log("Error in - " + url);
            loadingPanel.SetActive(false);
        }
    }
    
    
    void GetUserPhone(string mobile, string deviceToken)
    {
        StartCoroutine(CheckUserListPhone(mobile, deviceToken));
    }

    IEnumerator CheckUserListPhone(string mobile, string deviceToken)
    {
        string url = DataManager.Instance.url + "/api/v1/auth/getbyphone?phone=" + mobile + "&deviceToken=" + deviceToken;

        UnityWebRequest request = UnityWebRequest.Get(url);
        //request.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString("token"));
        yield return request.SendWebRequest();
        JSONNode values = JSON.Parse(request.downloadHandler.text.ToString());
        if (values["error"].Equals("Player not found"))
        {
            Root_PlayerRegister instance = new Root_PlayerRegister();
            instance.phone = "91" + mobileInput.text;
            instance.deviceToken = SystemInfo.deviceUniqueIdentifier;
            instance.countryCode = "91";
            instance.firebaseToken = "jhsjhjd";

            string create_Json = JsonUtility.ToJson(instance);
            Debug.Log(create_Json);
            print("Regester new player");

            StartCoroutine(RigisterNewUser(DataManager.Instance.url + "/api/v1/auth/player/register", create_Json));
            //New Player 
        }
        else
        {
            print("Player Already Regestered");
            PlayerPrefs.SetString("playerId", values["data"]["_id"].Value);

            /*ludoSignFirstScreenObj.SetActive(false);
            mobileLoginScreenObj.SetActive(false);
            OpenPinDialog(2);*/

            //Fetch Data Direct On Pin
        }
        //print("User Data : " + request.downloadHandler.text.ToString());
    }
    
    public void LoginMobileNumberButtonClick()
    {

        SoundManager.Instance.ButtonClick();
        if (testMode)
        {
            LoadSceneMainMenu();
        }
        else
        {
            if (mobileInput.text.Length < 10)
            {
                msgText.text = "Invalid mobile number";
                return;
            }
            if (otpInput.text.Length < 4)
            {
                msgText.text = "Invalid otp code";
                return;
            }

            Root_PlayerOTPVerify instance = new Root_PlayerOTPVerify();
            instance.phone = "91" + mobileInput.text;
            instance.code = otpInput.text;
            instance.deviceType = "android";
            instance.deviceToken = SystemInfo.deviceUniqueIdentifier;
            instance.gamecode = "TeenPatti";

            string create_Json = JsonUtility.ToJson(instance);
            StartCoroutine(CheckOTPCode(DataManager.Instance.url + "/api/v1/auth/player/verify", create_Json));
        }
    }
    IEnumerator CheckOTPCode(string url, string bodyJsonString)
    {
        loadingPanel.SetActive(true);
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();
        Debug.Log("CheckOTPCode: " + request.downloadHandler.text);

        if (!request.isNetworkError && !request.isHttpError)// && request.responseCode == 200) // 200, 404 or 500. (In Unity Only)
        {
            JSONNode jsonNode = SimpleJSON.JSON.Parse(request.downloadHandler.text);
            if (jsonNode["success"] == false)
            {
                loadingPanel.SetActive(false);
                msgText.text = jsonNode["error"].Value;
            }
            else
            {
                loadingPanel.SetActive(false);
                PlayerPrefs.SetString("mobilenumber", mobileInput.text);
                msgText.text = "Mobile Verified";

                PlayerPrefs.SetString("playerId", jsonNode["playerId"].Value);
                print("Token Check TIme : " + jsonNode["token"].Value);

                PlayerPrefs.SetString("token", jsonNode["token"].Value);

                //yield return new WaitForSeconds(1.0f);
                
                PlayerPrefs.SetInt("OpenReffer", 0);
                LoadSceneMainMenu();
                //OpenPinDialog(1);
                //mobileLoginScreenObj.SetActive(false);

            }
        }
        else
        {
            Debug.Log("Erro: " + request.error);
            Debug.Log("Error in - " + url);
            loadingPanel.SetActive(false);
        }
    }
    
    void MessageOffDialog()
    {
        msgText.gameObject.SetActive(false);
    }

    void LoadSceneMainMenu()
    {
        DataManager.Instance.SetLoginValue("Y");
        loadingPanel.SetActive(true);
        //panelMobileLogin.SetActive(false);
        SceneManager.LoadScene("Main");
    }




}
[Serializable]
public class Root_PlayerRegister
{
    public string phone;
    public string deviceToken;
    public string countryCode;
    public string firebaseToken;
}

[Serializable]
public class Root_PlayerRegisterEmail
{
    public string email;
    public string deviceToken;
    public string countryCode;
    public string firebaseToken;
    public string firstName;
    public string picture;
}
[Serializable]
public class Root_PlayerOTPVerify
{
    public string phone;
    public string code;
    public string deviceType;
    public string deviceToken;
    public string gamecode;
}
[Serializable]
public class Root_SetPlayerPIN
{
    public string pin;
    //public string token;
    //public string playerId;
}
[Serializable]
public class Root_CheckPlayerPIN
{
    public string pin;
    public string token;
    public string playerId;
}