using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;
using System.Linq;
using Unity.VisualScripting;

public class RouletteButtonData : MonoBehaviour
{

    public int chipNo;
    public List<int> btnAvaliableNo;

    public GameObject btnParent;
    public bool isMultipleSelectNo;

    Vector3 animPos;
    // Start is called before the first frame update
    void Start()
    {
        animPos = RouletteManager.Instance.chipAnimParent.transform.position;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SimpleRebetGenerate(RouleteeBetClass passBetClass)
    {
        bool isMoneyAv = RouletteManager.Instance.CheckMoney(RouletteManager.Instance.GetPrice(passBetClass.betImgNo));
        if (isMoneyAv == false)
        {
            RouletteManager.Instance.OpenErrorScreen();
            return;
        }

        RouleteeBetClass existRouleteeBet = null;

        GameObject chipGenObj = Instantiate(RouletteManager.Instance.chipPrefab, btnParent.transform);
        SendRouletteBet(chipNo);


        RouletteManager.Instance.totalCurrentInvest += RouletteManager.Instance.GetPrice(RouletteManager.Instance.betChipNo);
        //DataManager.Instance.DebitAmount((RouletteManager.Instance.GetPrice(passBetClass.betImgNo)).ToString(), TestSocketIO.Instace.roomid, "Roulette-Bet-" + TestSocketIO.Instace.roomid, "game");

        //Minus Balance
        RouleteeBetClass betClass = new RouleteeBetClass();
        int betChipNo = passBetClass.betImgNo;
        betClass.placeNo = chipNo;
        betClass.betImgNo = betChipNo;
        betClass.betTotalAmount = RouletteManager.Instance.chipsValue[betChipNo];
        betClass.chipObj = chipGenObj;
        betClass.rouletteButtonData = this;
        existRouleteeBet = betClass;
        RouletteManager.Instance.rouleteeBets.Add(betClass);


        chipGenObj.transform.GetComponent<Image>().sprite = RouletteManager.Instance.chipSprite[RouletteManager.Instance.betChipNo];
        Vector3 orgPos = chipGenObj.transform.position;
        chipGenObj.transform.position = animPos;
        chipGenObj.transform.DOMove(orgPos, 0.2f).OnComplete(() =>
        {
            if (btnAvaliableNo.Count != 2 && btnAvaliableNo.Count != 3 && btnAvaliableNo.Count != 4 && btnAvaliableNo.Count != 6)
            {
                GameObject bObj = RouletteManager.Instance.blackPanelObj[chipNo].gameObject;
                bObj.gameObject.SetActive(true);

                if (!existRouleteeBet.blackPanelList.Contains(bObj))
                {
                    existRouleteeBet.blackPanelList.Add(bObj);
                }

            }
            if (btnAvaliableNo.Count == 2 || btnAvaliableNo.Count == 4)
            {
                for (int i = 0; i < btnAvaliableNo.Count; i++)
                {
                    GameObject bObj = RouletteManager.Instance.blackPanelObj[btnAvaliableNo[i]].gameObject;
                    bObj.SetActive(true);
                    if (!existRouleteeBet.blackPanelList.Contains(bObj))
                    {
                        existRouleteeBet.blackPanelList.Add(bObj);
                    }
                }
            }

        });
    }




    public void ButtonClick()
    {

        if (RouletteManager.Instance.isStopBet)
        {
            return;
        }
        if (RouletteManager.Instance.undoBlockList.Contains(chipNo))
        {
            RouletteManager.Instance.ClearSpecificNo(chipNo);
        }


        if (RouletteManager.Instance.undoBlockList.Count == 0)
        {
            bool isExist = false;
            bool isAdd = false;
            //print("Chip No :" + chipNo);

            RouleteeBetClass existRouleteeBet = null;
            for (int i = 0; i < RouletteManager.Instance.rouleteeBets.Count; i++)
            {
                int no = RouletteManager.Instance.rouleteeBets[i].placeNo;
                if (no == chipNo)
                {
                    if (RouletteManager.Instance.rouleteeBets[i].betTotalAmount > 0)
                    {

                        isExist = true;
                        existRouleteeBet = RouletteManager.Instance.rouleteeBets[i];
                    }
                }

            }
            bool isMoneyAv = RouletteManager.Instance.CheckMoney(RouletteManager.Instance.GetPrice(RouletteManager.Instance.betChipNo));
            if (isMoneyAv == false)
            {
                RouletteManager.Instance.OpenErrorScreen();
                return;
            }
            //print("bool isExists : " + isExist);

            SoundManager.Instance.ThreeBetSound();
            GameObject chipGenObj = Instantiate(RouletteManager.Instance.chipPrefab, btnParent.transform);
            SendRouletteBet(chipNo);

            RouletteManager.Instance.totalCurrentInvest += RouletteManager.Instance.GetPrice(RouletteManager.Instance.betChipNo);

            //DataManager.Instance.DebitAmount((RouletteManager.Instance.GetPrice(RouletteManager.Instance.betChipNo)).ToString(), TestSocketIO.Instace.roomid, "Roulette-Bet-" + TestSocketIO.Instace.roomid, "game");


            //Minus Balance
            if (isExist == false)
            {
                RouleteeBetClass betClass = new RouleteeBetClass();
                int betChipNo = RouletteManager.Instance.betChipNo;
                betClass.placeNo = chipNo;
                betClass.betImgNo = betChipNo;
                betClass.betTotalAmount = RouletteManager.Instance.chipsValue[betChipNo];
                betClass.chipObj = chipGenObj;
                betClass.rouletteButtonData = this;
                existRouleteeBet = betClass;
                RouletteManager.Instance.rouleteeBets.Add(betClass);
                isAdd = true;
            }


            chipGenObj.transform.GetComponent<Image>().sprite = RouletteManager.Instance.chipSprite[RouletteManager.Instance.betChipNo];
            Vector3 orgPos = chipGenObj.transform.position;
            chipGenObj.transform.position = animPos;
            chipGenObj.transform.DOMove(orgPos, 0.2f).OnComplete(() =>
            {
                if (btnAvaliableNo.Count != 2 && btnAvaliableNo.Count != 3 && btnAvaliableNo.Count != 4 && btnAvaliableNo.Count != 6)
                {
                    GameObject bObj = RouletteManager.Instance.blackPanelObj[chipNo].gameObject;
                    bObj.gameObject.SetActive(true);

                    if (!existRouleteeBet.blackPanelList.Contains(bObj))
                    {
                        existRouleteeBet.blackPanelList.Add(bObj);
                    }
                }

                if (btnAvaliableNo.Count == 2 || btnAvaliableNo.Count == 4)
                {


                    for (int i = 0; i < btnAvaliableNo.Count; i++)
                    {
                        GameObject bObj = RouletteManager.Instance.blackPanelObj[btnAvaliableNo[i]].gameObject;
                        bObj.SetActive(true);
                        if (!existRouleteeBet.blackPanelList.Contains(bObj))
                        {
                            existRouleteeBet.blackPanelList.Add(bObj);
                        }
                    }
                }

                if (isAdd == false)
                {

                    if (existRouleteeBet != null)
                    {
                        Destroy(chipGenObj);
                        int betChipNo = RouletteManager.Instance.betChipNo;
                        existRouleteeBet.betImgNo = betChipNo;
                        existRouleteeBet.betTotalAmount += RouletteManager.Instance.chipsValue[betChipNo];
                        existRouleteeBet.chipObj.GetComponent<Image>().sprite = RouletteManager.Instance.chipSprite[RouletteManager.Instance.betChipNo];
                        PlaceChips placeChips = existRouleteeBet.chipObj.GetComponent<PlaceChips>();
                        placeChips.placeStr = existRouleteeBet.betTotalAmount.ToString();
                        placeChips.GeneratePlaceCoinTxt();
                    }
                }

            });
        }

    }


    public void Add_Socket_Chip(Sprite blankS)
    {
        GameObject chipGenObj = Instantiate(RouletteManager.Instance.chipPrefab, btnParent.transform);

        chipGenObj.transform.GetComponent<Image>().sprite = blankS;
        Vector3 orgPos = chipGenObj.transform.position;
        chipGenObj.transform.position = animPos;
        chipGenObj.transform.DOMove(orgPos, 0.2f).OnComplete(() =>
        {

            chipGenObj.transform.DOScale(new Vector3(0.9f, 0.9f, 0.9f), 0.1f).OnComplete(() =>
            {
                chipGenObj.transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.1f).OnComplete(() =>
                {
                    chipGenObj.transform.DOScale(Vector3.zero, 0.05f).OnComplete(() =>
                     {
                         Destroy(chipGenObj);
                     });
                });
            });
        });

    }

    public void SendRouletteBet(int chipNo)
    {
        JSONObject obj = new JSONObject();
        obj.AddField("PlayerID", DataManager.Instance.playerData._id);
        obj.AddField("TournamentID", DataManager.Instance.tournamentID);
        obj.AddField("RoomId", TestSocketIO.Instace.roomid);
        obj.AddField("chipNo", chipNo);
        TestSocketIO.Instace.Senddata("RouletteSendBetData", obj);
    }


}
