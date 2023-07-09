using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingDialog : MonoBehaviour
{

    public static SettingDialog Instance;
    public Image soundImg;
    public Image vibrationImg;
    public Image notificationImg;
    public Image friendRequestImg;
    public Sprite onSprite;
    public Sprite offSprite;

    [Header("Buttons")]
    public GameObject SupportPrefab;
    public GameObject SupportParentOBJ;
    public GameObject PrivacyPrefagb;
    public GameObject PrivacyParentOBJ;
    public GameObject RefundPrefab;
    public GameObject RefundParentOBJ;
    public GameObject TermsPrefab;
    public GameObject TermsParentOBJ;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        MainMenuManager.Instance.screenObj.Add(this.gameObject);

        if (DataManager.Instance.GetSound() == 0)
        {
            soundImg.sprite = onSprite;
        }
        else
        {
            soundImg.sprite = offSprite;
        }

        if (DataManager.Instance.GetVibration() == 0)
        {
            vibrationImg.sprite = onSprite;
        }
        else
        {
            vibrationImg.sprite = offSprite;
        }

        if (DataManager.Instance.GetNotification() == 0)
        {
            notificationImg.sprite = onSprite;
        }
        else
        {
            notificationImg.sprite = offSprite;
        }

        if (DataManager.Instance.GetFriendRequest() == 0)
        {
            friendRequestImg.sprite = onSprite;
        }
        else
        {
            friendRequestImg.sprite = offSprite;
        }

    }

    public void CloseSetting()
    {
        MainMenuManager.Instance.screenObj.Remove(this.gameObject);
        Destroy(this.gameObject);
    }

    public void CloseSettingButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        CloseSetting();
    }



    public void SoundButtonClick()
    {
        if (soundImg.sprite == onSprite)
        {
            DataManager.Instance.SetSound(1);
            SoundManager.Instance.StopBackgroundMusic();
            soundImg.sprite = offSprite;
        }
        else if (soundImg.sprite == offSprite)
        {
            DataManager.Instance.SetSound(0);
            soundImg.sprite = onSprite;
            SoundManager.Instance.StartBackgroundMusic();
            SoundManager.Instance.ButtonClick();
        }
    }

    public void NotificationButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        if (notificationImg.sprite == onSprite)
        {
            DataManager.Instance.SetNotification(1);
            notificationImg.sprite = offSprite;
        }
        else if (notificationImg.sprite == offSprite)
        {
            DataManager.Instance.SetNotification(0);
            notificationImg.sprite = onSprite;
        }
    }

    public void VibrationButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        if (vibrationImg.sprite == onSprite)
        {
            DataManager.Instance.SetVibration(1);
            vibrationImg.sprite = offSprite;
        }
        else if (vibrationImg.sprite == offSprite)
        {
            DataManager.Instance.SetVibration(0);
            vibrationImg.sprite = onSprite;
        }
    }

    public void FriendRequestButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        if (friendRequestImg.sprite == onSprite)
        {
            DataManager.Instance.SetFriendRequest(1);
            friendRequestImg.sprite = offSprite;
        }
        else if (friendRequestImg.sprite == offSprite)
        {
            DataManager.Instance.SetFriendRequest(0);
            friendRequestImg.sprite = onSprite;
        }
    }


    public void BottomButtonClick(int no)
    {
        SoundManager.Instance.ButtonClick();

        if (no == 1)
        {
            Instantiate(RefundPrefab, RefundParentOBJ.transform);
        }
        else if (no == 2)
        {
            Instantiate(TermsPrefab, TermsParentOBJ.transform);
        }
        else if (no == 3)
        {
            Instantiate(SupportPrefab, SupportParentOBJ.transform);
        }
        else if (no == 4)
        {
            Instantiate(PrivacyPrefagb, PrivacyParentOBJ.transform);
        }
       
    }


}
