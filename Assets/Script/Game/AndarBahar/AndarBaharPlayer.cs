using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class AndarBaharPlayer : MonoBehaviour
{
    public GameObject winParticleObj;
    public Image avatarImg;
    public Text playerNameTxt;
    public Text andarPriceTxt;
    public Text baharPriceTxt;

    public string avatar;

    public int playerNo;
    public string playerId;
    public string tournamentId;


    public GameObject andarObj;
    public GameObject baharObj;

    public GameObject andarParentPos;
    public GameObject baharParentPos;


    public Text andarSubPriceTxt;
    public Text baharSubPriceTxt;
    private void OnEnable()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        StartPrice();
    }


    public void StartPrice()
    {
        andarSubPriceTxt = andarObj.transform.GetChild(1).GetComponent<Text>();
        baharSubPriceTxt = baharObj.transform.GetChild(1).GetComponent<Text>();
        andarSubPriceTxt.text = "0";
        baharSubPriceTxt.text = "0";
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateAvatar()
    {

        StartCoroutine(DataManager.Instance.GetImages(avatar, avatarImg));
    }

    public void GetSocketBet(bool isAndar, bool isBahar, float value)
    {
        if (isAndar)
        {
            andarObj.SetActive(true);

            float value1 = float.Parse(andarSubPriceTxt.text);
            andarSubPriceTxt.text = (value1 + value).ToString();


            AndarBaharManager.Instance.PlaceBet(true, false, value, playerId);
        }
        else if (isBahar)
        {
            baharObj.SetActive(true);

            float value1 = float.Parse(baharSubPriceTxt.text);
            baharSubPriceTxt.text = (value1 + value).ToString();
            AndarBaharManager.Instance.PlaceBet(false, true, value, playerId);


        }
        Canvas.ForceUpdateCanvases();
    }

    //AA

    public void PlayerGenerateBet(bool isAndar, bool isBahar)
    {

        if (isAndar)
        {
            GameObject coinObj = Instantiate(AndarBaharManager.Instance.andarCPrefab, AndarBaharManager.Instance.cardGenObj.transform);
            coinObj.transform.position = andarParentPos.transform.position;
            coinObj.transform.localScale = Vector3.zero;
            float andarPrice = float.Parse(andarPriceTxt.text);
            //PlayerSetNo(5, true, false, andarPrice);
            coinObj.transform.DOScale(Vector3.one, 0.25f);
            coinObj.transform.DOMove(andarObj.transform.GetChild(0).transform.position, 0.25f).OnComplete(() =>
            {

                andarObj.SetActive(true);
                AndarBaharManager.Instance.PlaceBet(true, false, andarPrice, playerId);
                //andarSubPriceTxt.text = (andarPrice).ToString();

                float value1 = float.Parse(andarSubPriceTxt.text);
                andarSubPriceTxt.text = (value1 + andarPrice).ToString();
                AndarBaharManager.Instance.SendBet(true, false, andarPrice);

                Canvas.ForceUpdateCanvases();

                Destroy(coinObj);
            });
        }
        else if (isBahar)
        {
            GameObject coinObj = Instantiate(AndarBaharManager.Instance.baharCPrefab, AndarBaharManager.Instance.cardGenObj.transform);
            coinObj.transform.position = baharParentPos.transform.position;
            coinObj.transform.localScale = Vector3.zero;
            float baharPrice = float.Parse(baharPriceTxt.text);
            //PlayerSetNo(5, true, false, andarPrice);
            coinObj.transform.DOScale(Vector3.one, 0.25f);
            coinObj.transform.DOMove(baharObj.transform.GetChild(0).transform.position, 0.25f).OnComplete(() =>
            {
                baharObj.SetActive(true);
                print("bahar Price : " + baharPrice);

                AndarBaharManager.Instance.PlaceBet(false, true, baharPrice, playerId);
                //baharSubPriceTxt.text = (baharPrice).ToString();

                float value1 = float.Parse(baharSubPriceTxt.text);
                baharSubPriceTxt.text = (value1 + baharPrice).ToString();

                AndarBaharManager.Instance.SendBet(false, true, baharPrice);
                Canvas.ForceUpdateCanvases();

                Destroy(coinObj);
            });
        }
    }





}
