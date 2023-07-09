using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class BotPlayers : MonoBehaviour
{
    
    public Image avatarImg;
    public Text playerNameTxt;
    public Text andarPriceTxt;
    public Text baharPriceTxt;
    public GameObject andar;
    public GameObject bahar;

    public string avatar;

    public float andarBalance;
    public float baharBalance;

    public GameObject andarAnimation;
    public GameObject baharAnimation;
    // Start is called before the first frame update
    void Start()
    {
        
        andarPriceTxt.text = andarBalance.ToString(CultureInfo.InvariantCulture);
        baharPriceTxt.text = baharBalance.ToString(CultureInfo.InvariantCulture);
        SetProfileImage();
    }

    public void SetProfileImage()
    {
        for (int i = 0; i < BotPlayerManager.Instance.andarBaharBotPlayer.Count; i++)
        {
            StartCoroutine(DataManager.Instance.GetImages(avatar, avatarImg));
        }
        
    }
}
