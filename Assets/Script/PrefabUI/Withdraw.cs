using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using SimpleJSON;

public class Withdraw : MonoBehaviour
{
    public static Withdraw instace;



    public GameObject basicDialog;
    public GameObject upiDialog;
    public GameObject walletDialog;
    public GameObject bankDialog;

    [Header("---UPI---")]
    public InputField upiamount;
    public InputField address;
    public Text UPImsg;
    public Text upiName;
    //public GameObject verifyObj;
    //public GameObject verifiedObj;

    [Header("---Wallet---")]
    public InputField walletamount;
    public InputField walletname;
    public InputField walletnumber;
    public Text wallletMSG;

    [Header("---Bank---")]
    public InputField bankamount;
    public InputField accountname;
    public InputField accoutnumber;
    public InputField ifsc;
    public InputField bankname;
    public Text Bankmsg;


    private void Start()
    {
        instace = this;
        MainMenuManager.Instance.screenObj.Add(this.gameObject);
        OpenDkC();
    }

    void OpenDkC()
    {
        //dkcObj.SetActive(false);
        //cashObj.SetActive(true);
        //switchBtnTxt.text = "Switch To DKC";
    }





    public void WithdrawStatusButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        //Instantiate(MainMenuManager.Instance.withdrawListPanel, MainMenuManager.Instance.parentObj.transform);
    }

    public void BackButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        MainMenuManager.Instance.screenObj.Remove(this.gameObject);
        MainMenuManager.Instance.UpdateAllData();
        this.gameObject.SetActive(false);
        Destroy(this.gameObject);

    }

    void OpenCash()
    {
        basicDialog.SetActive(true);
        upiDialog.SetActive(false);
        walletDialog.SetActive(false);
        bankDialog.SetActive(false);

    }

    //public void Close_basic_Dialog()
    //{
    //    SoundManager.Instance.ButtonClick();
    //    MainMenuManager.Instance.screenObj.Remove(this.gameObject);
    //    Destroy(this.gameObject);

    //}

    public void Close_UPI_Dialog()
    {
        SoundManager.Instance.ButtonClick();
        basicDialog.SetActive(true);
        upiDialog.SetActive(false);
    }

    public void Close_Wallet_Dialog()
    {
        SoundManager.Instance.ButtonClick();
        basicDialog.SetActive(true);
        walletDialog.SetActive(false);
    }

    public void Close_Bankt_Dialog()
    {
        SoundManager.Instance.ButtonClick();
        basicDialog.SetActive(true);
        bankDialog.SetActive(false);
    }

    public void FirstDialogBtn(int no)
    {

        SoundManager.Instance.ButtonClick();
        switch (no)
        {
            case 1:
                basicDialog.SetActive(false);
                upiDialog.SetActive(true);
                address.interactable = true;
                upiName.text = "";
                upiamount.text = "";
                address.text = "";
                UPImsg.text = "";
                //verifyObj.SetActive(true);
                //verifiedObj.SetActive(false);
                break;
            case 2:
                basicDialog.SetActive(false);
                walletDialog.SetActive(true);
                break;
            case 3:
                basicDialog.SetActive(false);
                bankDialog.SetActive(true);
                break;

        }
    }


    public void requesttype(int i)
    {

        SoundManager.Instance.ButtonClick();


        switch (i)
        {
            case 0:
                if (upiamount.text == "" || (int.Parse(upiamount.text.Trim('"')) < 1))
                {
                    UPImsg.text = "Enter amount";
                }
                else if (float.Parse(DataManager.Instance.playerData.balance.Trim('"')) <= 0)
                {
                    UPImsg.text = "Your Balance is too low";
                }
                else if ((float.Parse(upiamount.text.ToString()) < 100) || (float.Parse(upiamount.text.ToString()) > 500))
                {
                    UPImsg.text = "Enter amount between 100-500";
                }
                else if (MainMenuManager.Instance.isWithdraw == true)
                {
                    UPImsg.text = "Already withdraw request is pending";
                }
                else
                {
                    StartCoroutine(sendwithrequest(0));
                }
                break;
            case 1:
                if (MainMenuManager.Instance.isWithdraw == true)
                {
                    wallletMSG.text = "Already withdraw request is pending";
                }
                else
                {
                    StartCoroutine(Savewalletdetails());
                }
                break;
            case 2:
                if (MainMenuManager.Instance.isWithdraw == true)
                {
                    Bankmsg.text = "Already withdraw request is pending";
                }
                else
                {
                    StartCoroutine(savebankdetails());
                }
                break;
        }

    }

    IEnumerator savebankdetails()
    {
        WWWForm form = new WWWForm();

        if (bankname.text == "")
        {
            Bankmsg.text = "Please Add Bank Name";
        }
        else if (ifsc.text == "")
        {
            Bankmsg.text = "Please Add IFSC Code";
        }
        else if (accoutnumber.text == "")
        {
            Bankmsg.text = "Please Add Account Number";
        }
        else if (accountname.text == "")
        {
            Bankmsg.text = "Please Add Account Name";
        }
        else if (float.Parse(DataManager.Instance.playerData.balance.Trim('"')) <= 0)
        {
            Bankmsg.text = "Your Balance is too low";
        }
        else if (bankamount.text == "" || (int.Parse(bankamount.text.Trim('"')) < 1))
        {
            Bankmsg.text = "Enter amount";
        }
        else if ((float.Parse(bankamount.text.ToString()) < 100) || (float.Parse(bankamount.text.ToString()) > 500))
        {
            Bankmsg.text = "Enter amount between 100-500";
        }
        else
        {
            form.AddField("bankAddress", " ");
            form.AddField("bankIfc", ifsc.text);
            form.AddField("bankName", bankname.text);
            form.AddField("bankAccount", accoutnumber.text);
            form.AddField("bankAccountHolder", accountname.text);
            //WaitPanelManager.Instance.OpenPanel();

            UnityWebRequest request = UnityWebRequest.Post(DataManager.Instance.url + "/api/v1/players/bank", form);
            request.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString("token"));
            yield return request.SendWebRequest();

            if (request.error == null && !request.isNetworkError)
            {
                JSONNode keys = JSON.Parse(request.downloadHandler.text.ToString());

                if (keys["success"])
                {
                    //WaitPanelManager.Instance.ClosePanel();

                    DataManager.Instance.SetBankValue(1);
                    StartCoroutine(sendwithrequest(2));

                }
                else
                {
                    DataManager.Instance.SetBankValue(0);
                }
            }
            else
            {
                Debug.Log("Bank Details===:::" + request.error);
            }
        }
    }

    IEnumerator Savewalletdetails()
    {
        if (walletname.text == "")
        {
            wallletMSG.text = "Enter Wallet Name";
        }
        else if (walletamount.text == "")
        {
            wallletMSG.text = "Enter Amount";
        }
        else if (walletnumber.text == "")
        {
            wallletMSG.text = "Enter wallet number";
        }
        else if (float.Parse(DataManager.Instance.playerData.balance.Trim('"')) <= 0)
        {
            wallletMSG.text = "Your Balance is too low";
        }
        else if (walletamount.text == "" || (int.Parse(walletamount.text.Trim('"')) < 1))
        {
            wallletMSG.text = "Enter amount between 100-500";
        }

        else if ((float.Parse(walletamount.text.ToString()) < 100) || (float.Parse(walletamount.text.ToString()) > 500))
        {
            wallletMSG.text = "Enter amount between 100-500";
        }
        else
        {
            WWWForm form = new WWWForm();
            form.AddField("walletName", walletname.text);
            form.AddField("walletAddress", walletnumber.text);
            //WaitPanelManager.Instance.OpenPanel();

            UnityWebRequest request = UnityWebRequest.Post(DataManager.Instance.url + "/api/v1/players/wallet", form);
            request.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString("token"));
            yield return request.SendWebRequest();
            //Debug.Log("Wallet request===:::" + request.error);
            //Debug.Log("Wallet request===:::" + request.downloadHandler.text);
            if (request.error == null && !request.isNetworkError)
            {
                //WaitPanelManager.Instance.ClosePanel();
                StartCoroutine(sendwithrequest(1));
            }

        }
    }

    public void CheckUPI()
    {
        SoundManager.Instance.ButtonClick();
        StartCoroutine(SaveUPI());
    }

    IEnumerator SaveUPI()
    {

        if (address.text == "")
        {
            UPImsg.text = "Enter your UPI";
        }

        else
        {
            //print("Enter UPI Missing : " + float.Parse(upiamount.text.ToString()));
            //WWWForm form = new WWWForm();
            //form.AddField("upiId", address.text);
            //WaitPanelManager.Instance.OpenPanel();

            //UnityWebRequest request = UnityWebRequest.Post(DataManager.Instance.url + "/api/v1/players/upi", form);
            //request.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString("token"));


            //yield return request.SendWebRequest();




            //if (request.error == null && !request.isNetworkError)
            //{
            //    JSONNode node = JSON.Parse(request.downloadHandler.text);
            //    //print("UPI request.downloadHandler.text : " + request.downloadHandler.text);
            //    if (node["success"] == false)
            //    {
            //        WaitPanelManager.Instance.ClosePanel();
            //        print("Node Errpr : " + node["error"]);
            //        UPImsg.text = node["error"].ToString();
            //    }
            //    else if (node["success"] == true)
            //    {
            //        WaitPanelManager.Instance.ClosePanel();

            //        WWWForm form1 = new WWWForm();
            //        form1.AddField("upiId", address.text);
            //        WaitPanelManager.Instance.OpenPanel();




            //    }
            //}

            WWWForm form1 = new WWWForm();
            form1.AddField("upiId", address.text);
            //WaitPanelManager.Instance.OpenPanel();
            UnityWebRequest request1 = UnityWebRequest.Post(DataManager.Instance.url + "/api/v1/players/checkupi", form1);
            request1.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString("token"));
            yield return request1.SendWebRequest();


            if (request1.error == null && !request1.isNetworkError)
            {
                print("Second  : " + request1.downloadHandler.text);

                //WaitPanelManager.Instance.ClosePanel();
                JSONNode node1 = JSON.Parse(request1.downloadHandler.text);
                if (node1["success"] == false)
                {
                    //WaitPanelManager.Instance.ClosePanel();
                    UPImsg.text = node1["error"].ToString();
                }
                else if (node1["success"] == true)
                {
                    //print("Second UPI : " + request1.downloadHandler.text);
                    address.interactable = false;
                    //verifyObj.SetActive(false);
                    //verifiedObj.SetActive(true);
                    upiName.text = "UPI Name : " + RemoveQuotes(node1["data"]["data"]["nameAtBank"].ToString());
                    //;
                }
            }


        }
    }

    public string RemoveQuotes(string s)
    {
        string str = s;
        string newstr = str.Replace("\"", "");
        return newstr;
    }
    IEnumerator sendwithrequest(int i)
    {
        WWWForm form = new WWWForm();
        Debug.Log("value of i==:::" + i.ToString());
        //Debug.Log("value of upi amount ==:::" + float.Parse(upiamount.text.ToString()));
        bool isEnter = false;
        if (i == 0)
        {

            if ((float.Parse(upiamount.text.ToString()) < 1000 && (float.Parse(upiamount.text.ToString()) > 5000)) && i == 0)
            {
                UPImsg.text = "Enter amount between 1000-5000";
                yield return null;
                Debug.Log("UPI request===:::");
                isEnter = true;
            }
        }
        else if (i == 1)
        {
            if ((walletamount.text == "" && i == 1) || ((float.Parse(walletamount.text.ToString()) < 1000 && (float.Parse(walletamount.text.ToString()) > 5000)) && i == 1))
            {
                wallletMSG.text = "Enter amount between 1000-5000";
                yield return null;
                isEnter = true;
            }
        }
        else if (i == 2)
        {
            if (bankamount.text == "" && i == 2 || (float.Parse(bankamount.text.ToString()) < 1000 && float.Parse(bankamount.text.ToString()) > 5000) && i == 2)
            {
                Bankmsg.text = "Enter amount between 1000-5000";
                yield return null;
                isEnter = true;
                Debug.Log("bank request===:::");

            }
        }
        if (isEnter == false)
        {
            switch (i)
            {
                case 0:
                    float amount = (float)(float.Parse(upiamount.text) / 10);
                    form.AddField("amount", amount.ToString());
                    form.AddField("note", "UPI withdraw");
                    form.AddField("to", "upi");
                    form.AddField("upi", address.text);
                    print("Upi Withdreaw : " + address.text);
                    break;
                case 1:
                    float amount1 = (float)(float.Parse(walletamount.text) / 10);
                    form.AddField("amount", amount1.ToString());
                    form.AddField("note", "wallet withdraw");
                    form.AddField("to", "wallet");
                    break;
                case 2:
                    float amount2 = (float)(float.Parse(bankamount.text) / 10);
                    form.AddField("amount", amount2.ToString());
                    form.AddField("note", "Bank withdraw request");
                    form.AddField("to", "bank");
                    break;
            }

            //WaitPanelManager.Instance.OpenPanel();

            UnityWebRequest request = UnityWebRequest.Post(DataManager.Instance.url + "/api/v1/players/withdraw/request", form);
            request.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString("token"));
            yield return request.SendWebRequest();
            if (request.error == null && !request.isNetworkError)
            {
                JSONNode keys = JSON.Parse(request.downloadHandler.text.ToString());
                Debug.Log("Request Withdraw===:::" + keys.ToString());
                if (keys["success"])
                {
                    //WaitPanelManager.Instance.ClosePanel();

                    switch (i)
                    {
                        case 0:
                            UPImsg.text = "Request send successfull";
                            MainMenuManager.Instance.screenObj.Remove(this.gameObject);
                            MainMenuManager.Instance.UpdateAllData();
                            this.gameObject.SetActive(false);
                            MainMenuManager.Instance.GetTran();
                            MainMenuManager.Instance.Getdata();

                            Destroy(this.gameObject);
                            break;
                        case 1:
                            wallletMSG.text = "Request send successfull";
                            MainMenuManager.Instance.screenObj.Remove(this.gameObject);
                            MainMenuManager.Instance.UpdateAllData();
                            this.gameObject.SetActive(false);
                            MainMenuManager.Instance.GetTran();
                            MainMenuManager.Instance.Getdata();

                            Destroy(this.gameObject);
                            break;
                        case 2:
                            Bankmsg.text = "Request send successfull";
                            MainMenuManager.Instance.screenObj.Remove(this.gameObject);
                            MainMenuManager.Instance.UpdateAllData();
                            this.gameObject.SetActive(false);
                            MainMenuManager.Instance.GetTran();
                            MainMenuManager.Instance.Getdata();
                            Destroy(this.gameObject);
                            break;
                    }
                    MainMenuManager.Instance.Getdata();
                }
                else
                {
                    //WaitPanelManager.Instance.ClosePanel();
                    if (i == 0)
                    {
                        UPImsg.text = keys["error"];
                    }
                    else if (i == 1)
                    {
                        wallletMSG.text = keys["error"];
                    }
                    else if (i == 2)
                    {
                        Bankmsg.text = keys["error"];
                    }

                }
            }
            else
            {
                if (i == 0)
                {
                    UPImsg.text = request.error;
                }
                else if (i == 1)
                {
                    wallletMSG.text = request.error;
                }
                else if (i == 2)
                {
                    Bankmsg.text = request.error;
                }
                print("0---::" + request.error);
            }


        }
    }
}
