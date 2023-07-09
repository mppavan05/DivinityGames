using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InternetManager : MonoBehaviour
{
    public static InternetManager Instance;
    public GameObject internetObj;
    public GameObject updateObj;

    public bool isCheckUpdate;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

        }

        print("Applicationn Version : " + Application.version);
    }
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating(nameof(CheckInterenet), 0, 3);


    }

    public void CheckUpdate()
    {
        if (Application.version != DataManager.Instance.appVersion)
        {
            if (MainMenuManager.Instance != null)
            {
                //MainMenuManager.Instance.settingUpdateObj.SetActive(true);
            }
            Instantiate(updateObj, this.transform);
        }
        else
        {
            if (MainMenuManager.Instance != null)
            {
                //MainMenuManager.Instance.settingUpdateObj.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void CheckInterenet()
    {
        if (InternetPanel.Instance == null && Application.internetReachability == NetworkReachability.NotReachable)
        {
            Instantiate(internetObj, this.transform);
        }
    }
}
