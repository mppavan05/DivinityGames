using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class NotificationListPanel : MonoBehaviour
{

    public static NotificationListPanel Instance;
    public Text noNotificationText;
    public GameObject scrollObj;

    public GameObject scrollParent;
    public GameObject clearBtn;
    public GameObject notificationbar;

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

        MainMenuManager.Instance.screenObj.Add(this.gameObject);
        noNotificationText.text = "Please Wait...";
        scrollObj.SetActive(false);
        clearBtn.SetActive(false);
        Getnotification();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void BackButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        MainMenuManager.Instance.screenObj.Remove(this.gameObject);
        MainMenuManager.Instance.UserUpdateDisplayData();
        this.gameObject.SetActive(false);
    }


    #region Notification

    public void Getnotification()
    {
        StartCoroutine(GetNotifications());
    }

    IEnumerator GetNotifications()
    {
        UnityWebRequest request = UnityWebRequest.Get(DataManager.Instance.url + "/api/v1/notifications/player");
        request.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString("token"));
        yield return request.SendWebRequest();

        //print("Notification Button Click : " + request.downloadHandler.text);
        if (request.error == null && !request.isNetworkError)
        {
            JSONNode value = JSON.Parse(request.downloadHandler.text.ToString());
            if (value.Count > 0)
            {
                noNotificationText.text = "";
                scrollObj.SetActive(true);
                MainMenuManager.Instance.notificationRedDot.SetActive(true);
                clearBtn.SetActive(true);
            }
            else
            {
                noNotificationText.text = "No Notifications avaliable";
                MainMenuManager.Instance.notificationRedDot.SetActive(false);

                scrollObj.SetActive(false);
            }

            MainMenuManager.Instance.notiBarManages.Clear();

            print(request.downloadHandler.text);
            for (int i = 0; i < value.Count; i++)
            {
                GameObject temp = Instantiate(notificationbar, scrollParent.transform);
                MainMenuManager.Instance.notiBarManages.Add(temp.GetComponent<NotiBarManage>());
                byte[] st = Encoding.ASCII.GetBytes(value[i]["title"].ToString().Trim('"'));
                //byte[] url = Encoding.ASCII.GetBytes(value[i]["url"].ToString().Trim('"'));
                string data = Encoding.UTF8.GetString(st);

                //string url = "https://google.com";// value[i]["url"];
                string url = value[i]["url"];

                string imageId = value[i]["imageUrl"]; //imageId
                string createdAt = value[i]["createdAt"]; //imageId

                NotiBarManage notiBarManage = temp.GetComponent<NotiBarManage>();

                temp.GetComponent<Button>().onClick.AddListener(() => NotificationsButtonClick(url.ToString()));
                string title = RemoveQuotes(value[i]["title"].ToString());
                string subTitle = RemoveQuotes(value[i]["message"].ToString());
                if (title.Length > 65)
                {
                    title = title.Substring(0, 65) + "...";
                }
                if (subTitle.Length > 110)
                {
                    subTitle = subTitle.Substring(0, 110) + "...";
                }

                notiBarManage.titleTxt.text = title;
                notiBarManage.middleTitleTxt.text = subTitle;

                string curDateStr = DateTime.Parse(createdAt).ToLocalTime().ToString();
                DateTime dateT1 = DateTime.Parse(curDateStr.Split(" ")[0]);
                DateTime dateT2 = DateTime.Parse(curDateStr.Split(" ")[1]);
                notiBarManage.dateTxt.text = dateT1.ToString("yyyy") + "-" + dateT1.ToString("MM") + "-" + dateT1.ToString("dd") + " " + dateT2.ToString("hh:mm:ss");

                StartCoroutine(GetImages(DataManager.Instance.url + "/api/v1/files/" + imageId, temp.transform.GetChild(0).GetComponent<Image>()));

                //temp.transform.GetChild(2).GetComponent<Text>().text = value[i]["message"].ToString().Trim('"');
            }

        }
    }

    string RemoveQuotes(string s)
    {
        string str = s;
        string newstr = str.Replace("\"", "");
        return newstr;
    }
    IEnumerator GetImages(string URl, Image image)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(URl);
        yield return request.SendWebRequest();

        if (request.error == null)
        {
            var texture = DownloadHandlerTexture.GetContent(request);
            Rect rect = new Rect(0, 0, texture.width, texture.height);
            image.sprite = Sprite.Create(texture, rect, new Vector2(0, 0));
        }
    }

    public void NotificationsButtonClick(string s)
    {
        SoundManager.Instance.ButtonClick();
        if (s != "" && s.Length != 0)
        {
            print("Enter The First Condition");
            Application.OpenURL(s);
        }

    }

    public void Cleat_Notification_Button()
    {
        print("Clear Button Click");
        SoundManager.Instance.ButtonClick();
        StartCoroutine(ClearNoti());
    }

    IEnumerator ClearNoti()
    {
        WWWForm form = new WWWForm();
        UnityWebRequest request = UnityWebRequest.Post(DataManager.Instance.url + "/api/v1/players/notification/clearall", form);
        request.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString("token"));
        yield return request.SendWebRequest();
        if (request.error == null && !request.isNetworkError)
        {
            print("Notification Clear Data : " + request.downloadHandler.text);
            JSONNode value = JSON.Parse(request.downloadHandler.text.ToString());
            if (value["success"] == true)
            {
                foreach (Transform t in scrollParent.transform)
                {
                    Destroy(t.gameObject);
                }
                Getnotification();
            }

        }
        else
        {
            print("Notification Clear Error : " + request.error);
        }
    }


    #endregion
}
