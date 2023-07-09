using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;

public class TransactionHistoryPanel : MonoBehaviour
{
    public static TransactionHistoryPanel Instance;

    public GameObject pleaseWaitScreen;
    public Text waitTxt;
    public GameObject scorllParent;
    public GameObject tranPrefab;

    public Color greenColor;
    public Color redColor;

    public List<Transaction> transactions = new List<Transaction>();

    // Start is called before the first frame update
    void OnEnable()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        GetTransaction();
        MainMenuManager.Instance.screenObj.Add(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void BackButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        MainMenuManager.Instance.screenObj.Remove(this.gameObject);
        Destroy(this.gameObject);
    }
    #region Transaction
    public void GetTransaction()
    {
        pleaseWaitScreen.SetActive(true);
        waitTxt.text = "Please Wait...";
        StartCoroutine(GetTransactions());
    }
    IEnumerator GetTransactions()
    {
        UnityWebRequest request = UnityWebRequest.Get(DataManager.Instance.url + "/api/v1/transactions/player");

        request.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString("token"));
        yield return request.SendWebRequest();

        if (request.error == null && !request.isNetworkError)
        {
            print("tran Data : " + request.downloadHandler.text.ToString());
            JSONNode keys = JSON.Parse(request.downloadHandler.text.ToString());
            JSONNode data = JSON.Parse(keys["data"].ToString());
            if (data.Count == 0)
            {
                waitTxt.text = "No History...";
            }
            else
            {
                pleaseWaitScreen.SetActive(false);
                for (int i = 0; i < data.Count; i++)
                {
                    Transaction t = new Transaction();
                    t.paymentStatus = data[i]["paymentStatus"];
                    t.logType = data[i]["logType"];
                    t._id = data[i]["_id"];
                    t.amount = data[i]["amount"];
                    t.transactionType = data[i]["transactionType"];
                    t.note = data[i]["note"];
                    t.createdAt = data[i]["createdAt"];
                    transactions.Add(t);

                    GameObject tObj = Instantiate(tranPrefab, scorllParent.transform);
                    Text t1 = tObj.transform.GetChild(0).GetComponent<Text>();
                    Text t2 = tObj.transform.GetChild(1).GetComponent<Text>();
                    Text t3 = tObj.transform.GetChild(2).GetComponent<Text>();

                    string curDateStr = DateTime.Parse(t.createdAt).ToLocalTime().ToString();
                    DateTime dateT1 = DateTime.Parse(curDateStr.Split(" ")[0]);
                    DateTime dateT2 = DateTime.Parse(curDateStr.Split(" ")[1]);
                    //t1.text = "Joined : " + dateT1.ToString("dd") + " " + dateT1.ToString("MMM") + " " + dateT1.ToString("yyyy") + "-" + dateT2.ToString("hh:mm tt");
                    t1.text = dateT1.ToString("dd") + " " + dateT1.ToString("MMM") + ", " + dateT2.ToString("hh:mm tt");
                    t2.text = t.note;
                    if (t.transactionType == "debit")
                    {
                        t3.text = "-" + (t.amount).ToString("F2");
                        t1.color = redColor;
                        t2.color = redColor;
                        t3.color = redColor;
                    }
                    else if (t.transactionType == "credit")
                    {
                        t3.text = "+" + (t.amount).ToString("F2");
                        t1.color = greenColor;
                        t2.color = greenColor;
                        t3.color = greenColor;
                    }


                }
            }
        }
    }
    #endregion

}

[System.Serializable]
public class Transaction
{
    public string paymentStatus;
    public string logType;
    public string _id;
    public float amount;
    public string transactionType;
    public string note;
    public string createdAt;
}

