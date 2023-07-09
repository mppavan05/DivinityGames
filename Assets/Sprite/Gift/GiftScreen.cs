using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using SimpleJSON;

public class GiftScreen : MonoBehaviour
{

    public static GiftScreen Instance;
    public List<GiftCard> giftCards;

    public GameObject giftScreenObj;
    public GameObject giftCardPrefab;
    public GameObject giftCardParent;

    public Image player1GiftImg;
    public Image player2GiftImg;
    public GameObject player1GiftObj;
    public GameObject player2GiftObj;

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
        GenerateGift();
        player1GiftObj.SetActive(false);
        player2GiftObj.SetActive(false);
    }



    // Update is called once per frame
    void Update()
    {

    }

    public void OpenMyGift(string id)
    {
        OffPlayer1();

        Sprite getSprite = null;

        for (int i = 0; i < giftCards.Count; i++)
        {
            if (giftCards[i].giftId.Equals(id))
            {
                getSprite = giftCards[i].giftSprite;
            }
        }

        //player1GiftObj.GetComponent<Animator>().enabled = false;
        player1GiftObj.transform.localScale = Vector3.zero;
        player1GiftImg.sprite = getSprite;
        player1GiftObj.SetActive(true);
        //player1GiftObj.GetComponent<Animator>().enabled = true;
        player1GiftObj.transform.DOScale(Vector3.one, 0.75f);
        Invoke(nameof(OffPlayer1), 2f);
    }
    void OffPlayer1()
    {
        player1GiftObj.SetActive(false);
    }


    public void OpenOppGift(string id)
    {
        OffPlayer2();

        Sprite getSprite = null;

        for (int i = 0; i < giftCards.Count; i++)
        {
            if (giftCards[i].giftId.Equals(id))
            {
                getSprite = giftCards[i].giftSprite;
            }
        }

        player2GiftObj.transform.localScale = Vector3.zero;
        //player2GiftObj.GetComponent<Animator>().enabled = false;
        player2GiftImg.sprite = getSprite;
        player2GiftObj.SetActive(true);
        //player2GiftObj.GetComponent<Animator>().enabled = true;
        player2GiftObj.transform.DOScale(Vector3.one, 0.75f);
        Invoke(nameof(OffPlayer2), 2f);
    }
    void OffPlayer2()
    {
        player2GiftObj.SetActive(false);
    }

    //public void OpenGiftScreenObj

    void GenerateGift()
    {
        for (int i = 0; i < giftCards.Count; i++)
        {
            GameObject giftCard = Instantiate(giftCardPrefab, giftCardParent.transform);
            string id = giftCards[i].giftId;
            Sprite giftSprite = giftCards[i].giftSprite;

            Image cImg = giftCard.GetComponent<Image>();
            Button cBtn = giftCard.GetComponent<Button>();
            cImg.sprite = giftSprite;
            cBtn.onClick.AddListener(() => GiftButtonClick(id));
        }
    }

    void GiftButtonClick(string id)
    {
        SoundManager.Instance.ButtonClick();
        print("Gift Id : " + id);
        giftScreenObj.SetActive(false);
        OpenMyGift(id);
        SendGift(id);
    }

    public void GiftButtonClose()
    {
        SoundManager.Instance.ButtonClick();
        giftScreenObj.SetActive(false);
    }

    public void GiftButtonHomeClick()
    {
        giftScreenObj.SetActive(true);
    }


    #region Socket 
    public void SendGift(string id)
    {
        JSONObject obj = new JSONObject();
        obj.AddField("PlayerID", DataManager.Instance.playerData._id);
        obj.AddField("TournamentID", DataManager.Instance.tournamentID);
        obj.AddField("RoomId", TestSocketIO.Instace.roomid);
        obj.AddField("GiftId", id);
        TestSocketIO.Instace.Senddata("SendGift", obj);
    }

    public void ReceiveGift(string id)
    {
        OpenOppGift(id);
    }

    #endregion

}

[System.Serializable]
public class GiftCard
{
    public string giftId;
    public Sprite giftSprite;
}
