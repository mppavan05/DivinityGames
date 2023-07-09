using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class BotPlayerManager : MonoBehaviour
{
    public static BotPlayerManager Instance;
    
    
    [Header("--- PlayersList ---")]
    public BotPlayers player1;
    public BotPlayers player2;
    public BotPlayers player3;
    public BotPlayers player4;
    public BotPlayers player5;
    public BotPlayers player6;

    public List<BotPlayers> andarBaharBotPlayer = new List<BotPlayers>();
    public List<BotPlayersData> botplayerData = new List<BotPlayersData>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        GetBotPlayer();
        SetBotPlayer();
    }

    public void OpenBotPlayer()
    {
        if (DataManager.Instance.joinPlayerDatas.Count == 1)
        {
            foreach (var t in andarBaharBotPlayer)
            {
                t.gameObject.SetActive(true);
            }
        }
        else 
        {
            // for (int i = 0; i < DataManager.Instance.joinPlayerDatas.Count - 1 ; i++)
            // {
            //     andarBaharBotPlayer[i].gameObject.SetActive(false);
            // }
            for (int i = 0; i < andarBaharBotPlayer.Count; i++)
            {
                andarBaharBotPlayer[i].gameObject.SetActive(i >= (DataManager.Instance.joinPlayerDatas.Count - 1));
            }

            return;
        }
        GetBotPlayer();
        SetBotPlayer();
    }


    private void GetBotPlayer()
    {
        int[] avatars = Enumerable.Range(0, BotManager.Instance.botUser_Profile_URL.Count).ToArray();
        avatars.Shuffle();
        int[] randomAvatars = avatars.Take(andarBaharBotPlayer.Count).ToArray();
        
        int[] names = Enumerable.Range(0, BotManager.Instance.botUserName.Count).ToArray();
        names.Shuffle();
        int[] randomNames = names.Take(andarBaharBotPlayer.Count).ToArray();
        
        for (int i = 0; i < andarBaharBotPlayer.Count; i++)
        {
            BotPlayersData t = new BotPlayersData();
            t.avatar = BotManager.Instance.botUser_Profile_URL[randomAvatars[i]];
            t.name =  BotManager.Instance.botUserName[randomNames[i]];
            //t.andarBalance = Random.Range(1, 11) * 10;
            //t.baharBalance = Random.Range(1, 11) * 10;
            botplayerData.Add(t);
        }
    }
    

    public void UpdateBalance()
    {
        for (int i = 0; i < andarBaharBotPlayer.Count; i++)
        {
            andarBaharBotPlayer[i].andarBalance = Random.Range(1, 11) * 10;
            andarBaharBotPlayer[i].andarPriceTxt.text = andarBaharBotPlayer[i].andarBalance.ToString(CultureInfo.InvariantCulture);
            andarBaharBotPlayer[i].baharBalance = Random.Range(1, 11) * 10;
            andarBaharBotPlayer[i].baharPriceTxt.text = andarBaharBotPlayer[i].baharBalance.ToString(CultureInfo.InvariantCulture);
            
        }
        
    }
    
    public void TurnOffBotBet()
    {
        foreach (var t in andarBaharBotPlayer)
        {
            t.andar.SetActive(false);
            t.bahar.SetActive(false);
        }
    }

    private void SetBotPlayer()
    {
        for (int i = 0; i < andarBaharBotPlayer.Count; i++)
        {
            andarBaharBotPlayer[i].playerNameTxt.text = botplayerData[i].name;
            andarBaharBotPlayer[i].avatar = botplayerData[i].avatar;
            andarBaharBotPlayer[i].SetProfileImage();
            
        }
    }

    public void StartBotBetting()
    {
        StartCoroutine(ActivateRandomGameObject());
    }
    
    IEnumerator ActivateRandomGameObject()
    {
        float time = 6f; // 10 seconds
        while (time > 0)
        {
            // randomly activate one gameobject and deactivate the other for each player
            foreach (var t in andarBaharBotPlayer)
            {
                int randomIndex = Random.Range(0, 2);
                switch (randomIndex)
                {
                    case 0:
                        t.andar.SetActive(true);
                        t.bahar.SetActive(false);
                        break;
                    case 1:
                        t.bahar.SetActive(true);
                        t.andar.SetActive(false);
                        break;
                }
            }
            yield return new WaitForSeconds(2f); // wait for 1 second
            time--;
        }
    }

    public void UpdateBotBetAmount()
    {
        for (int i = 0; i < andarBaharBotPlayer.Count; i++)
        {
            andarBaharBotPlayer[i].andarPriceTxt.text = botplayerData[i].andarBalance.ToString(CultureInfo.InvariantCulture);
            andarBaharBotPlayer[i].baharPriceTxt.text = botplayerData[i].baharBalance.ToString(CultureInfo.InvariantCulture);
        }
    }
    
   

   
    
    
    
    [System.Serializable]
    public class BotPlayersData
    {
        public string name;
        public string avatar;
        public float andarBalance;
        public float baharBalance;
    }
}
