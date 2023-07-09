using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Networking;

public class CheckMaintainServer : MonoBehaviour
{
    public GameObject serverMaintain;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        GetMaintanance();
    }

    public void GetMaintanance()
    {
        StartCoroutine(GetMaintanances());
    }

    IEnumerator GetMaintanances()
    {
        UnityWebRequest request = UnityWebRequest.Get(DataManager.Instance.url + "/api/v1/auth/maintanance");
        yield return request.SendWebRequest();

        if (request.error == null && !request.isNetworkError)
        {
            JSONNode value = JSON.Parse(request.downloadHandler.text);
            print("Maintain  :" + request.downloadHandler.text);
            JSONNode botPr = JSON.Parse(value["data"]["bot_profile"].ToString());

            BotManager.Instance.botUser_Profile_URL.Clear();
            for (int i = 0; i < botPr.Count; i++)
            {
                BotManager.Instance.botUser_Profile_URL.Add(RemoveQuotes(botPr[i].ToString()));
            }

            serverMaintain.SetActive(false);
            Time.timeScale = 1;

        }
        else
        {
            Debug.Log("request.error : " + request.error);
            serverMaintain.SetActive(true);
            Time.timeScale = 0;
        }

    }
    public string RemoveQuotes(string s)
    {
        string str = s;
        string newstr = str.Replace("\"", "");
        return newstr;
    }
}
