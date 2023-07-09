using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InternetPanel : MonoBehaviour
{
    public static InternetPanel Instance;

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
        if (MainMenuManager.Instance != null)
        {
            MainMenuManager.Instance.screenObj.Add(this.gameObject);
        }

    }

    public void BackButtonClick()
    {
        SoundManager.Instance.ButtonClick();

        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Debug.Log("Error. Check internet connection!");
        }
        else
        {
            if (MainMenuManager.Instance != null)
            {
                MainMenuManager.Instance.screenObj.Remove(this.gameObject);
            }

            Destroy(this.gameObject);
        }

    }

    public void UpdateButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        if (MainMenuManager.Instance != null)
        {
            MainMenuManager.Instance.screenObj.Remove(this.gameObject);
        }

        Application.OpenURL(DataManager.Instance.appUrl);
        //Destroy(this.gameObject);

    }

    public void CloseButtonClick()
    {
        SoundManager.Instance.ButtonClick();

        if (MainMenuManager.Instance != null)
        {
            MainMenuManager.Instance.screenObj.Remove(this.gameObject);
        }

        Destroy(this.gameObject);
    }
}
