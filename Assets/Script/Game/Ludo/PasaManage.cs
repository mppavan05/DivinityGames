using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class PasaManage : MonoBehaviour
{

    public Vector3 firstPosition;
    public int pasaCurrentNo;
    public GameObject pasaParentObj;
    public GameObject pasaObj;
    public int playerNo;
    public int playerSubNo;
    public int orgParentNo;
    public float singlePasaScale;


    public bool isStarted = false;

    public float scale2;
    public float scale3;
    public float scale4;
    public float scale5;
    public float scale6;
    public bool isClick;
    public bool isSafe;
    public bool isStopZoom;

    public bool isPlayer;
    public bool isReadyForClick;
    public int orgNo;

    public bool isOneTimeMove;
    public List<PasaMove> scaleList1 = new List<PasaMove>();

    public bool isPasaWin;
    // Start is called before the first frame update
    void Start()
    {
        firstPosition = transform.position;
        //playerNo = DataManager.Instance.playerNo;

        if (this.GetComponent<Image>().sprite == LudoManager.Instance.blueToken)
        {
            playerNo = 1;
        }
        else if (this.GetComponent<Image>().sprite == LudoManager.Instance.redToken)
        {
            playerNo = 2;
        }
        else if (this.GetComponent<Image>().sprite == LudoManager.Instance.greenToken)
        {
            playerNo = 3;
        }
        else if (this.GetComponent<Image>().sprite == LudoManager.Instance.yellowToken)
        {
            playerNo = 4;
        }


        LudoManager.Instance.pasaSocketList.Add(this);
        if (playerNo == DataManager.Instance.playerNo)
        {
            isPlayer = true;
            LudoManager.Instance.currentPlayerPasaList.Add(this);
            //if (LudoManager.Instance.currentPlayerPasaList.Count == 4)
            //{
            //    if (DataManager.Instance.modeType == 2 || DataManager.Instance.modeType == 3)
            //    {
            //        LudoManager.Instance.TokenAllNumberFirst();
            //    }
            //}
        }


        if (playerNo != DataManager.Instance.playerNo)
        {
            LudoManager.Instance.pasaBotPlayer.Add(this);
        }
    }

    


    int GetOrginialNumber(int cNo, int pNo)
    {
        //print(" Player Number : "+ pNo);
        if (pNo == 1)
        {
            return cNo;
        }
        else if (pNo == 2)
        {
            return LudoManager.Instance.orgListNo2[cNo - 1];
        }
        else if (pNo == 3)
        {
            return LudoManager.Instance.orgListNo3[cNo - 1];
        }
        else if (pNo == 4)
        {
            return LudoManager.Instance.orgListNo4[cNo - 1];
        }
        return 0;
    }
    //public void DecrementThisPasa()
    //{
    //    SoundManager.Instance.TokenKillSound();
    //    Move_Decrement_Steps();
    //}

    #region pasa First

    public void PasaOnFirst()
    {
        pasaCurrentNo = 1;
        orgNo = GetOrginialNumber(pasaCurrentNo, orgParentNo);
        if (!LudoManager.Instance.pasaObjects.Contains(this.gameObject))
        {
            LudoManager.Instance.pasaObjects.Add(this.gameObject);
        }

        List<GameObject> findObj = CheckAvaliableObjectSamePos(1, false);

        if (findObj.Count == 4)
        {
            ScaleManageMent(findObj, LudoManager.Instance.numberObj[0].transform.position);
        }
    }


    #endregion

    #region ScaleManage

    public void ScaleManageMent(List<GameObject> scaleObj, Vector3 pos)
    {

        float posX = pos.x;
        float posY = pos.y;
        float scale = 0;
        float increment = 0;
        if (scaleObj.Count == 1)
        {
            increment = 0f;
            posX = pos.x;
            scale = scale2;
        }
        else if (scaleObj.Count == 2)
        {
            increment = 0.1f;
            posX = pos.x - (increment * 1.5f);
            scale = scale2;
        }
        else if (scaleObj.Count == 3)
        {
            increment = 0.07f;
            posX = pos.x - (increment * 2.1f);
            scale = scale3;
        }
        else if (scaleObj.Count == 4)
        {
            increment = 0.05f;
            posX = pos.x - (increment * 2.5f);
            scale = scale4;
        }
        else if (scaleObj.Count == 5)
        {
            scale = scale5;
            increment = 0.04f;
            posX = pos.x - (increment * 3f);
            scale = scale4;
        }
        else if (scaleObj.Count == 6)
        {
            scale = scale6;
            increment = 0.03f;
            posX = pos.x - (increment * 3.5f);
            scale = scale4;
        }


        for (int i = 0; i < scaleObj.Count; i++)
        {
            posX += increment;
            scaleObj[i].transform.position = new Vector3(posX, posY, pos.z);
            scaleObj[i].transform.localScale = new Vector3(scale, scale, scale);
        }

    }

    #endregion

    #region Increament Pasa

    public void PasaButtonClick()
    {


        if (isClick == false && playerNo == DataManager.Instance.playerNo && !LudoManager.Instance.pasaCollectList.Contains(this) && isReadyForClick)
        {
            isClick = true;
            LudoManager.Instance.isPathClick = false;
            if (pasaCurrentNo == 0)
            {

                LudoManager.Instance.MovePlayer(playerSubNo, 1);
                LudoManager.Instance.PlayerStopDice();
                LudoManager.Instance.RestartTimer();

                pasaCurrentNo = 1;
                isSafe = true;
                List<GameObject> findObj = CheckAvaliableObjectSamePos(1, false);
                if (findObj.Count == 0)
                {
                    Vector3 pos = LudoManager.Instance.numberObj[pasaCurrentNo - 1].transform.position;
                    if (!LudoManager.Instance.pasaObjects.Contains(this.gameObject))
                    {
                        LudoManager.Instance.pasaObjects.Add(this.gameObject);
                    }
                    List<GameObject> createList = new List<GameObject>();
                    createList.Add(pasaObj);
                    pasaObj.transform.DOMove(LudoManager.Instance.numberObj[pasaCurrentNo - 1].transform.position, 0.25f);
                    pasaObj.transform.DOScale(new Vector3(singlePasaScale, singlePasaScale, singlePasaScale), 0.25f).OnComplete(() =>
                    ScaleManageMent(createList, pos));

                }
                else
                {
                    if (findObj.Count == 1)
                    {
                        Vector3 pos = LudoManager.Instance.numberObj[pasaCurrentNo - 1].transform.position;
                        pasaCurrentNo = 1;
                        if (!LudoManager.Instance.pasaObjects.Contains(this.gameObject))
                        {
                            LudoManager.Instance.pasaObjects.Add(this.gameObject);
                        }
                        List<GameObject> createList = new List<GameObject>();
                        createList.Add(findObj[0]);
                        createList.Add(pasaObj);
                        pasaObj.transform.DOMove(new Vector3(pos.x + 0.05f, pos.y, pos.z), 0.25f);
                        pasaObj.transform.DOScale(new Vector3(scale2, scale2, scale2), 0.25f).OnComplete(() =>
                        ScaleManageMent(createList, pos));
                    }
                    if (findObj.Count == 2)
                    {
                        Vector3 pos = LudoManager.Instance.numberObj[pasaCurrentNo - 1].transform.position;
                        pasaCurrentNo = 1;
                        if (!LudoManager.Instance.pasaObjects.Contains(this.gameObject))
                        {
                            LudoManager.Instance.pasaObjects.Add(this.gameObject);
                        }
                        List<GameObject> createList = new List<GameObject>();
                        createList.Add(findObj[0]);
                        createList.Add(findObj[1]);
                        createList.Add(pasaObj);
                        pasaObj.transform.DOMove(new Vector3(pos.x, pos.y, pos.z), 0.25f);
                        pasaObj.transform.DOScale(new Vector3(scale3, scale3, scale3), 0.25f).OnComplete(() =>
                        ScaleManageMent(createList, pos));
                    }
                    if (findObj.Count == 3)
                    {
                        Vector3 pos = LudoManager.Instance.numberObj[pasaCurrentNo - 1].transform.position;
                        pasaCurrentNo = 1;
                        if (!LudoManager.Instance.pasaObjects.Contains(this.gameObject))
                        {
                            LudoManager.Instance.pasaObjects.Add(this.gameObject);
                        }
                        List<GameObject> createList = new List<GameObject>();
                        createList.Add(findObj[0]);
                        createList.Add(findObj[1]);
                        createList.Add(findObj[2]);
                        createList.Add(pasaObj);
                        pasaObj.transform.DOMove(new Vector3(pos.x + 0.027f, pos.y, pos.z), 0.25f);
                        pasaObj.transform.DOScale(new Vector3(scale4, scale4, scale4), 0.25f).OnComplete(() =>
                        ScaleManageMent(createList, pos));

                    }
                    if (findObj.Count == 4)
                    {
                        Vector3 pos = LudoManager.Instance.numberObj[pasaCurrentNo - 1].transform.position;
                        pasaCurrentNo = 1;
                        if (!LudoManager.Instance.pasaObjects.Contains(this.gameObject))
                        {
                            LudoManager.Instance.pasaObjects.Add(this.gameObject);
                        }
                        List<GameObject> createList = new List<GameObject>();
                        createList.Add(findObj[0]);
                        createList.Add(findObj[1]);
                        createList.Add(findObj[2]);
                        createList.Add(findObj[3]);
                        createList.Add(pasaObj);
                        pasaObj.transform.DOMove(new Vector3(pos.x + 0.025f, pos.y - 0.05f, pos.z), 0.25f);
                        pasaObj.transform.DOScale(new Vector3(scale5, scale5, scale5), 0.25f).OnComplete(() =>
                        ScaleManageMent(createList, pos));

                    }
                    if (findObj.Count == 5)
                    {
                        Vector3 pos = LudoManager.Instance.numberObj[pasaCurrentNo - 1].transform.position;
                        pasaCurrentNo = 1;
                        if (!LudoManager.Instance.pasaObjects.Contains(this.gameObject))
                        {
                            LudoManager.Instance.pasaObjects.Add(this.gameObject);
                        }
                        List<GameObject> createList = new List<GameObject>();
                        createList.Add(findObj[0]);
                        createList.Add(findObj[1]);
                        createList.Add(findObj[2]);
                        createList.Add(findObj[3]);
                        createList.Add(findObj[4]);
                        createList.Add(pasaObj);
                        pasaObj.transform.DOMove(new Vector3(pos.x, pos.y - 0.05f, pos.z), 0.25f);
                        pasaObj.transform.DOScale(new Vector3(scale5, scale5, scale5), 0.25f).OnComplete(() =>
                        ScaleManageMent(createList, pos));

                    }
                    if (findObj.Count == 6)
                    {
                        Vector3 pos = LudoManager.Instance.numberObj[pasaCurrentNo - 1].transform.position;
                        pasaCurrentNo = 1;
                        if (!LudoManager.Instance.pasaObjects.Contains(this.gameObject))
                        {
                            LudoManager.Instance.pasaObjects.Add(this.gameObject);
                        }
                        List<GameObject> createList = new List<GameObject>();
                        createList.Add(findObj[0]);
                        createList.Add(findObj[1]);
                        createList.Add(findObj[2]);
                        createList.Add(findObj[3]);
                        createList.Add(findObj[4]);
                        createList.Add(findObj[5]);
                        createList.Add(pasaObj);
                        pasaObj.transform.DOMove(new Vector3(pos.x, pos.y - 0.05f, pos.z), 0.25f);
                        pasaObj.transform.DOScale(new Vector3(scale6, scale6, scale6), 0.25f).OnComplete(() =>
                        ScaleManageMent(createList, pos));

                    }
                    if (findObj.Count == 7)
                    {
                        Vector3 pos = LudoManager.Instance.numberObj[pasaCurrentNo - 1].transform.position;
                        pasaCurrentNo = 1;
                        if (!LudoManager.Instance.pasaObjects.Contains(this.gameObject))
                        {
                            LudoManager.Instance.pasaObjects.Add(this.gameObject);
                        }
                        List<GameObject> createList = new List<GameObject>();
                        createList.Add(findObj[0]);
                        createList.Add(findObj[1]);
                        createList.Add(findObj[2]);
                        createList.Add(findObj[3]);
                        createList.Add(findObj[4]);
                        createList.Add(findObj[5]);
                        createList.Add(findObj[6]);
                        createList.Add(pasaObj);
                        pasaObj.transform.DOMove(new Vector3(pos.x + 0.08f, pos.y - 0.05f, pos.z), 0.25f);
                        pasaObj.transform.DOScale(new Vector3(scale6, scale6, scale6), 0.25f).OnComplete(() =>
                        ScaleManageMent(createList, pos));
                    }
                }

                orgNo = GetOrginialNumber(pasaCurrentNo, orgParentNo);

            }
            for (int i = 0; i < LudoManager.Instance.currentPlayerPasaList.Count; i++)
            {
                LudoManager.Instance.currentPlayerPasaList[i].isReadyForClick = false;
            }

            if (LudoManager.Instance.pasaCurrentNo != 6)
            {
                LudoManager.Instance.PlayerDiceChange();
            }
            else if (LudoManager.Instance.pasaCurrentNo == 6)
            {
                LudoManager.Instance.isClickAvaliableDice = 0;
            }

        }
    }


    int counter;
    public void Move_Increment_Steps(int no)
    {
        if (isPasaWin)
        {
            return;
        }
        counter++;
        //if (LudoManager.Instance.isCheckEnter == true)
        //{
        //    LudoManager.Instance.isCheckEnter = false;
        //}
        LudoManager.Instance.ScoreManage(orgParentNo, 1);
        LudoManager.Instance.TimerStop();
        float moveScale = 0.15f;
        float currentScale = this.gameObject.transform.localScale.x;
        currentScale += moveScale;


        pasaCurrentNo += 1;

        orgNo = GetOrginialNumber(pasaCurrentNo, orgParentNo);
        isOneTimeMove = true;
        SoundManager.Instance.TokenMoveSound();


        if (pasaCurrentNo == 57)
        {

            this.gameObject.transform.DOScale(new Vector3(currentScale, currentScale, currentScale), 0.16f).OnComplete(() =>
            this.gameObject.transform.DOScale(new Vector3(currentScale - moveScale, currentScale - moveScale, currentScale - moveScale), 0.1f));
            this.gameObject.transform.DOMoveY(this.gameObject.transform.position.y + 0.2f, 0.27f).OnComplete(() =>
              Check_Move_Increment_Next(no));
        }
        else
        {
            if (isPasaWin == false)
            {
                this.gameObject.transform.DOScale(new Vector3(currentScale, currentScale, currentScale), 0.16f).OnComplete(() =>
                this.gameObject.transform.DOScale(new Vector3(currentScale - moveScale, currentScale - moveScale, currentScale - moveScale), 0.1f));
                this.gameObject.transform.DOMove(LudoManager.Instance.numberObj[pasaCurrentNo - 1].transform.position, 0.3f).OnComplete(() =>
                Check_Move_Increment_Next(no));
            }
        }
    }




    bool isEnterMove = false;

    void Check_Move_Increment_Next(int no)
    {

        bool isLast = false;
        if (no == counter)
        {
            counter = 0;
            isEnterMove = false;
            isLast = true;
            if (pasaCurrentNo == 1 || pasaCurrentNo == 9 || pasaCurrentNo == 22 || pasaCurrentNo == 35 || pasaCurrentNo == 48 || pasaCurrentNo == 1 || pasaCurrentNo == 14 || pasaCurrentNo == 27 || pasaCurrentNo == 40)
            {
                isSafe = true;
            }
            else
            {
                isSafe = false;
            }
            //if (DataManager.Instance.modeType == 3)
            //{
            //    GameUIManager.Instance.FirstNumberRemove();
            //}

            if (pasaCurrentNo == 57)
            {
                SoundManager.Instance.TokenHomeSound();
                LudoManager.Instance.GeneratePasaFire();
                isClick = false;
                isPasaWin = true;
                isEnterMove = true;
                LudoManager.Instance.ScoreManage(orgParentNo, 56);

                this.gameObject.transform.DOScale(new Vector3(0, 0, 0), 0.16f).OnComplete(() =>
                LudoManager.Instance.pasaObjects.Remove(this.gameObject));
            }

            bool isTurnChange = false;
            if (DataManager.Instance.modeType == 3)
            {
                isTurnChange = true;
            }

            if (LudoManager.Instance.pasaCurrentNo != 6)
            {
                if (isEnterMove == false)
                {
                    isTurnChange = true;
                }
                else
                {
                    LudoManager.Instance.isClickAvaliableDice = 0;
                    LudoManager.Instance.RestartTimer();
                }

            }
            else if (LudoManager.Instance.pasaCurrentNo == 6)
            {
                LudoManager.Instance.isClickAvaliableDice = 0;
                LudoManager.Instance.RestartTimer();
            }

            if (pasaCurrentNo >= 0)
            {
                ScaleManageToNext(false, pasaCurrentNo, true, isTurnChange);
            }
            if (pasaCurrentNo - 1 >= 0)
            {
                ScaleManageToNext(false, pasaCurrentNo - 1, false, false);
            }



            //LudoManager.Instance.isCheckEnter = false;

        }
        else
        {
            if (pasaCurrentNo >= 0)
            {
                ScaleManageToNext(false, pasaCurrentNo, false, false);
            }
            if (pasaCurrentNo - 1 >= 0)
            {
                ScaleManageToNext(false, pasaCurrentNo - 1, false, false);
            }
            Move_Increment_Steps(no);
        }


    }
    #endregion


    #region Move Decerement

    public void Move_Decrement_Steps(int pNo, bool isSocket)
    {
        List<GameObject> nObj = new List<GameObject>();
        if (pNo == 1)
        {
            nObj = LudoManager.Instance.numberObj;
        }
        else if (pNo == 2)
        {
            nObj = LudoManager.Instance.numberObj2;
        }
        else if (pNo == 3)
        {
            if (DataManager.Instance.isTwoPlayer)
            {
                nObj = LudoManager.Instance.numberObj2;
            }
            else
            {
                nObj = LudoManager.Instance.numberObj3;
            }
        }
        else if (pNo == 4)
        {
            nObj = LudoManager.Instance.numberObj4;
        }

        pasaCurrentNo -= 1;

        this.gameObject.transform.DOMove(nObj[pasaCurrentNo].transform.position, 0.08f).OnComplete(() =>
        Check_Move_Decrement_Next(pNo, isSocket, nObj));

    }
    void Check_Move_Decrement_Next(int pNo, bool isSocket, List<GameObject> nObj)
    {
        if (DataManager.Instance.modeType == 1)
        {
            if (pasaCurrentNo == 0)
            {
                this.gameObject.transform.DOMove(firstPosition, 0.08f);
                this.gameObject.transform.DOScale(Vector3.one, 0.08f);
                isClick = false;
                isStarted = false;
                isOneTimeMove = false;
                orgNo = 0;

                LudoManager.Instance.isClickAvaliableDice = 0;
                LudoManager.Instance.RestartTimer();
                if (BotManager.Instance.isConnectBot)
                {
                    if (DataManager.Instance.isDiceClick == false)
                    {
                        LudoManager.Instance.OnceTimeTurnBot();
                    }
                }

                LudoManager.Instance.pasaObjects.Remove(this.gameObject);


            }
            else
            {
                Move_Decrement_Steps(pNo, isSocket);
            }
        }
        else if (DataManager.Instance.modeType == 2 || DataManager.Instance.modeType == 3)
        {
            if (pasaCurrentNo == 1)
            {
                this.gameObject.transform.DOMove(nObj[pasaCurrentNo - 1].transform.position, 0.08f); //.OnComplete(() =>
                isOneTimeMove = false;
                isSafe = true;
                orgNo = 1;

                LudoManager.Instance.isClickAvaliableDice = 0;
                LudoManager.Instance.RestartTimer();
                if (BotManager.Instance.isConnectBot)
                {
                    if (DataManager.Instance.isDiceClick == false)
                    {
                        LudoManager.Instance.OnceTimeTurnBot();
                    }
                }

                if (orgNo >= 0)
                {
                    ScaleManageToNext(false, orgNo, false, false);
                }


                // LudoManager.Instance.pasaObjects.Remove(this.gameObject);
            }
            else
            {
                Move_Decrement_Steps(pNo, isSocket);
            }
        }
    }



    #endregion


    //#region Zoom

    //float getStartScale = 0;
    //public bool isFirstZoom;
    //public void PlayerPasaZoom()
    //{
    //    if (isFirstZoom == false)
    //    {

    //        getStartScale = pasaObj.transform.localScale.x;
    //    }
    //    pasaObj.transform.DOScale(new Vector3(getStartScale - 0.05f, getStartScale - 0.05f, getStartScale - 0.05f), 0.25f).OnComplete(() =>
    //       pasaObj.transform.DOScale(new Vector3(getStartScale + 0.05f, getStartScale + 0.05f, getStartScale + 0.05f), 0.25f).OnComplete(() =>
    //        CheckZoom()
    //    ));
    //}
    //void CheckZoom()
    //{
    //    if(isStopZoom==false)
    //    {
    //        pasaObj.transform.localScale = new Vector3(getStartScale, getStartScale , getStartScale);
    //        PlayerPasaZoom();
    //    }
    //    else
    //    {
    //        pasaObj.transform.localScale = new Vector3(getStartScale, getStartScale, getStartScale);
    //    }
    //}


    //#endregion


    #region Common Method

    List<GameObject> CheckAvaliableObjectSamePos(int no, bool isSocket)
    {

        List<GameObject> checkList = new List<GameObject>();
        for (int i = 0; i < LudoManager.Instance.pasaObjects.Count; i++)
        {
            GameObject pObj = LudoManager.Instance.pasaObjects[i];
            if (pObj.GetComponent<PasaManage>().pasaCurrentNo == no && pObj.GetComponent<PasaManage>().playerNo == DataManager.Instance.playerNo)
            {
                checkList.Add(pObj);
            }
            else if (pObj.GetComponent<PasaManage>().orgNo == no && pObj.GetComponent<PasaManage>().playerNo != DataManager.Instance.playerNo)
            {
                checkList.Add(pObj);
            }
        }
        return checkList;
    }


    public int IsEntryZone(PasaManage p)
    {
        if (p.pasaCurrentNo == 52 || p.pasaCurrentNo == 53 || p.pasaCurrentNo == 54 || p.pasaCurrentNo == 55 || p.pasaCurrentNo == 56)
        {
            //Ek Manage
            return 1;
        }
        else
        {
            return 0;
        }
        return 0;
    }


    public void ScaleManageToNext(bool isSocket, int no, bool isLast, bool isTurn)
    {
        List<GameObject> checkList = new List<GameObject>();
        bool isDiePlayer = false;
        for (int i = 0; i < LudoManager.Instance.pasaObjects.Count; i++)
        {
            GameObject pObj = LudoManager.Instance.pasaObjects[i];
            PasaManage manage = pObj.GetComponent<PasaManage>();
            if (manage.orgNo == no && (IsEntryZone(manage) != 1 || manage.orgParentNo == DataManager.Instance.playerNo))
            {
                if (manage.orgNo > 51)
                {
                    if (manage.playerNo == DataManager.Instance.playerNo)
                    {
                        checkList.Add(pObj);
                    }
                }
                else
                {
                    checkList.Add(pObj);
                }
            }

            //if (pObj.GetComponent<PasaManage>().orgNo == no)
            //{
            //    checkList.Add(pObj);
            //}
        }
        if (isLast)
        {
            int cntOnePos = 0;

            for (int i = 0; i < checkList.Count; i++)
            {
                PasaManage cPasaManage = checkList[i].GetComponent<PasaManage>();
                if (cPasaManage.isSafe == false)
                {
                    cntOnePos++;
                }
            }
            if (cntOnePos == 2)
            {
                for (int i = 0; i < checkList.Count; i++)
                {
                    PasaManage cPasaManage = checkList[i].GetComponent<PasaManage>();

                    if (isSocket)
                    {
                        if (cPasaManage.playerNo == DataManager.Instance.playerNo)
                        {
                            LudoManager.Instance.ScoreManageDecrese(cPasaManage.orgParentNo, cPasaManage.pasaCurrentNo);
                            int sNo = 0;
                            if (cPasaManage.orgParentNo == 1)
                            {
                                sNo = 3;
                            }
                            if (cPasaManage.orgParentNo == 3)
                            {
                                sNo = 1;
                            }
                            LudoManager.Instance.ScoreManage(sNo, cPasaManage.pasaCurrentNo);
                            isDiePlayer = true;
                            cPasaManage.Move_Decrement_Steps(cPasaManage.orgParentNo, isSocket);
                            break;
                        }
                    }
                    else
                    {
                        if (cPasaManage.playerNo != DataManager.Instance.playerNo)
                        {
                            LudoManager.Instance.ScoreManageDecrese(cPasaManage.orgParentNo, cPasaManage.pasaCurrentNo);
                            int sNo = 0;
                            if (cPasaManage.orgParentNo == 1)
                            {
                                sNo = 3;
                            }
                            if (cPasaManage.orgParentNo == 3)
                            {
                                sNo = 1;
                            }
                            LudoManager.Instance.ScoreManage(sNo, cPasaManage.pasaCurrentNo);
                            isDiePlayer = true;
                            cPasaManage.Move_Decrement_Steps(cPasaManage.orgParentNo, isSocket);
                            break;
                        }
                    }
                }
            }
        }

        if (checkList.Count > 0 && isDiePlayer == false)
        {
            Vector3 changePos = Vector3.zero;
            for (int i = 0; i < checkList.Count; i++)
            {
                PasaManage checkPasaManage = checkList[i].GetComponent<PasaManage>();


                if (isSocket)
                {
                    if (checkPasaManage.playerNo != DataManager.Instance.playerNo)
                    {
                        int getNo = 0;

                        if (isSocket)
                        {
                            getNo = checkPasaManage.orgNo - 1;

                        }
                        else
                        {
                            getNo = checkPasaManage.pasaCurrentNo - 1;

                        }
                        if (getNo >= 0)
                        {
                            if (isSocket)
                            {
                                if (getNo >= 51)
                                {

                                    // print("Enter The Socket Condition 1-1: " + getNo);
                                    changePos = LudoManager.Instance.numberObj2[getNo].transform.position;
                                }
                                else
                                {
                                    //print("Enter The Socket Condition 1-2: " + getNo);

                                    changePos = LudoManager.Instance.numberObj[getNo].transform.position;
                                }
                            }
                            else
                            {
                                if (getNo >= 51)
                                {

                                    //print("Enter The Socket Condition 2 - 1: " + getNo);
                                    changePos = LudoManager.Instance.numberObj2[getNo].transform.position;
                                }
                                else
                                {
                                    //print("Enter The Socket Condition 2 - 2: " + getNo);

                                    changePos = LudoManager.Instance.numberObj[getNo].transform.position;
                                }
                                // changePos = LudoManager.Instance.numberObj[getNo].transform.position;
                            }


                            if (no - 1 >= 0 && getNo - 1 >= 0)
                            {
                                List<GameObject> checkList1 = new List<GameObject>();
                                for (int j = 0; j < LudoManager.Instance.pasaObjects.Count; j++)
                                {
                                    GameObject pObj = LudoManager.Instance.pasaObjects[j];

                                    //if (pObj.GetComponent<PasaManage>().orgNo == no-1)
                                    //{
                                    //    checkList1.Add(pObj);
                                    //}
                                    PasaManage manage1 = pObj.GetComponent<PasaManage>();
                                    if (manage1.orgNo == no - 1 && (IsEntryZone(manage1) != 1 || manage1.orgParentNo == DataManager.Instance.playerNo))
                                    {
                                        if (manage1.playerNo != DataManager.Instance.playerNo)
                                        {
                                            checkList.Add(pObj);
                                        }
                                    }


                                }
                                Vector3 changePos1 = Vector3.zero;
                                if (isSocket)
                                {
                                    if (getNo - 1 >= 51)
                                    {

                                        //print("Minus Condition Else 1");
                                        changePos1 = LudoManager.Instance.numberObj2[getNo - 1].transform.position;
                                    }
                                    else
                                    {

                                        changePos1 = LudoManager.Instance.numberObj[getNo - 1].transform.position;
                                    }
                                }
                                else
                                {
                                    changePos1 = LudoManager.Instance.numberObj[getNo - 1].transform.position;
                                }
                                ScaleManageMent(checkList1, changePos1);
                            }
                            // print("Socket Check List No : " + checkList[0].name);
                            ScaleManageMent(checkList, changePos);
                        }
                        break;
                    }
                }
                else
                {
                    if (checkPasaManage.playerNo == DataManager.Instance.playerNo)
                    {
                        int getNo = 0;

                        if (isSocket)
                        {
                            getNo = checkPasaManage.orgNo - 1;

                        }
                        else
                        {
                            getNo = checkPasaManage.pasaCurrentNo - 1;

                        }
                        if (getNo >= 0)
                        {
                            if (isSocket)
                            {

                                if (getNo >= 51)
                                {
                                    // print("Enter The Simple Condition 1-1: " + getNo);
                                    changePos = LudoManager.Instance.numberObj2[getNo].transform.position;
                                }
                                else
                                {
                                    // print("Enter The Simple Condition 1-2: " + getNo);

                                    changePos = LudoManager.Instance.numberObj[getNo].transform.position;
                                }
                            }
                            else
                            {
                                //print("Enter The Simple Condition 2-1: " + getNo);

                                changePos = LudoManager.Instance.numberObj[getNo].transform.position;
                            }

                            if (no - 1 >= 0 && getNo - 1 >= 0)
                            {
                                List<GameObject> checkList1 = new List<GameObject>();
                                for (int j = 0; j < LudoManager.Instance.pasaObjects.Count; j++)
                                {
                                    GameObject pObj = LudoManager.Instance.pasaObjects[j];

                                    //if (pObj.GetComponent<PasaManage>().orgNo == no - 1)
                                    //{
                                    //    checkList1.Add(pObj);
                                    //}

                                    PasaManage manage1 = pObj.GetComponent<PasaManage>();
                                    if (manage1.orgNo == no - 1 && (IsEntryZone(manage1) != 1 || manage1.orgParentNo == DataManager.Instance.playerNo))
                                    {
                                        if (manage1.playerNo == DataManager.Instance.playerNo)
                                        {
                                            checkList1.Add(pObj);
                                        }
                                    }
                                }

                                Vector3 changePos1 = Vector3.zero;
                                if (isSocket)
                                {
                                    if (getNo - 1 >= 51)
                                    {
                                        //print("Minus Condition Else 1");
                                        changePos1 = LudoManager.Instance.numberObj2[getNo - 1].transform.position;
                                    }
                                    else
                                    {
                                        changePos1 = LudoManager.Instance.numberObj[getNo - 1].transform.position;
                                    }
                                }
                                else
                                {
                                    //print("Minus Condition Else 2");
                                    if (checkList1.Count > 0)
                                    {
                                        //print("Simple Check List No : " + checkList1[0].name);
                                    }
                                    changePos1 = LudoManager.Instance.numberObj[getNo - 1].transform.position;
                                }
                                ScaleManageMent(checkList1, changePos1);
                            }
                            //print("Simple Check List No : " + checkList[0].name);
                            ScaleManageMent(checkList, changePos);
                        }
                        break;
                    }
                }
            }
        }



        if (isTurn)
        {
            if (isDiePlayer)
            {
                LudoManager.Instance.isClickAvaliableDice = 0;
                LudoManager.Instance.RestartTimer();
            }
            else
            {
                LudoManager.Instance.isClickAvaliableDice = 0;
                
                if (BotManager.Instance.isConnectBot)
                {
                    bool isSendBot = false;
                    bool isSendPlayer = false;
                    if (DataManager.Instance.isTwoPlayer)
                    {
                        if (DataManager.Instance.isDiceClick)
                        {
                            isSendBot = false;
                            isSendPlayer = true; 
                        }
                        else
                        {
                            isSendBot = true;
                            isSendPlayer = false;
                        }
                    }
                    else
                    {
                        // isSendBot = true;
                        // isSendPlayer = false;
                        
                         print("$$$$$$$$$$$$$$$$$$$$$$ TRIGEREED &&&&&&&&&&&&&&&&&&&&&&&&&&&&&");
                        // isSendBot = false;
                        // isSendPlayer = true;
                        if (LudoManager.Instance.playerRoundChecker == 4)// DataManager.Instance.isDiceClick && 
                        {
                            isSendBot = true;
                            isSendPlayer = false;
                            print("$$$$$$$$$$$$$ Inside the if &&&&&&&&&&&&");
                        }
                        else
                        {
                            isSendBot = false;
                            isSendPlayer = true;
                            print("$$$$$$$$$$$$$ Outside the if &&&&&&&&&&&&");
                        }
                    }
                    LudoManager.Instance.BotChangeTurn(isSendBot, isSendPlayer);
                }
                else
                {
                    LudoManager.Instance.PlayerChangeTurn();
                }
            }
        }
    }
    #endregion


    #region Socket Method

    public void MoveStart(int listNo, int move)
    {
        List<GameObject> numberObj = new List<GameObject>();


        if (listNo == 1)
        {
            numberObj = LudoManager.Instance.numberObj;
        }
        else if (listNo == 2)
        {
            numberObj = LudoManager.Instance.numberObj2;
        }
        else if (listNo == 3)
        {
            if (DataManager.Instance.isTwoPlayer)
            {
                numberObj = LudoManager.Instance.numberObj2;
            }
            else
            {
                numberObj = LudoManager.Instance.numberObj3;
            }
        }
        else if (listNo == 4)
        {
            numberObj = LudoManager.Instance.numberObj4;
        }

        if (pasaCurrentNo == 0 && !isStarted)
        {
            pasaCurrentNo = 1;

            isSafe = true;
            isStarted = true;
            orgNo = GetOrginialNumber(pasaCurrentNo, orgParentNo);
            LudoManager.Instance.RestartTimer();

            List<GameObject> findObj = CheckAvaliableObjectSamePos(orgNo, true);
            if (findObj.Count == 0)
            {
                Vector3 pos = numberObj[pasaCurrentNo - 1].transform.position;
                if (!LudoManager.Instance.pasaObjects.Contains(this.gameObject))
                {
                    LudoManager.Instance.pasaObjects.Add(this.gameObject);
                }
                List<GameObject> createList = new List<GameObject>();
                createList.Add(pasaObj);
                pasaObj.transform.DOMove(numberObj[pasaCurrentNo - 1].transform.position, 0.25f);
                pasaObj.transform.DOScale(new Vector3(singlePasaScale, singlePasaScale, singlePasaScale), 0.25f).OnComplete(() =>
                ScaleManageMent(createList, pos));
            }
            else
            {
                if (findObj.Count == 1)
                {
                    Vector3 pos = numberObj[pasaCurrentNo - 1].transform.position;
                    pasaCurrentNo = 1;
                    if (!LudoManager.Instance.pasaObjects.Contains(this.gameObject))
                    {
                        LudoManager.Instance.pasaObjects.Add(this.gameObject);
                    }
                    List<GameObject> createList = new List<GameObject>();
                    createList.Add(findObj[0]);
                    createList.Add(pasaObj);
                    pasaObj.transform.DOMove(new Vector3(pos.x + 0.05f, pos.y, pos.z), 0.25f);
                    pasaObj.transform.DOScale(new Vector3(scale2, scale2, scale2), 0.25f).OnComplete(() =>
                    ScaleManageMent(createList, pos));
                }
                if (findObj.Count == 2)
                {
                    Vector3 pos = numberObj[pasaCurrentNo - 1].transform.position;
                    pasaCurrentNo = 1;
                    if (!LudoManager.Instance.pasaObjects.Contains(this.gameObject))
                    {
                        LudoManager.Instance.pasaObjects.Add(this.gameObject);
                    }
                    List<GameObject> createList = new List<GameObject>();
                    createList.Add(findObj[0]);
                    createList.Add(findObj[1]);
                    createList.Add(pasaObj);
                    pasaObj.transform.DOMove(new Vector3(pos.x, pos.y, pos.z), 0.25f);
                    pasaObj.transform.DOScale(new Vector3(scale3, scale3, scale3), 0.25f).OnComplete(() =>
                    ScaleManageMent(createList, pos));
                }
                if (findObj.Count == 3)
                {
                    Vector3 pos = numberObj[pasaCurrentNo - 1].transform.position;
                    pasaCurrentNo = 1;
                    if (!LudoManager.Instance.pasaObjects.Contains(this.gameObject))
                    {
                        LudoManager.Instance.pasaObjects.Add(this.gameObject);
                    }
                    List<GameObject> createList = new List<GameObject>();
                    createList.Add(findObj[0]);
                    createList.Add(findObj[1]);
                    createList.Add(findObj[2]);
                    createList.Add(pasaObj);
                    pasaObj.transform.DOMove(new Vector3(pos.x + 0.027f, pos.y, pos.z), 0.25f);
                    pasaObj.transform.DOScale(new Vector3(scale4, scale4, scale4), 0.25f).OnComplete(() =>
                    ScaleManageMent(createList, pos));
                }
                if (findObj.Count == 4)
                {
                    Vector3 pos = numberObj[pasaCurrentNo - 1].transform.position;
                    pasaCurrentNo = 1;
                    if (!LudoManager.Instance.pasaObjects.Contains(this.gameObject))
                    {
                        LudoManager.Instance.pasaObjects.Add(this.gameObject);
                    }
                    List<GameObject> createList = new List<GameObject>();
                    createList.Add(findObj[0]);
                    createList.Add(findObj[1]);
                    createList.Add(findObj[2]);
                    createList.Add(findObj[3]);
                    createList.Add(pasaObj);
                    pasaObj.transform.DOMove(new Vector3(pos.x + 0.025f, pos.y - 0.05f, pos.z), 0.25f);
                    pasaObj.transform.DOScale(new Vector3(scale5, scale5, scale5), 0.25f).OnComplete(() =>
                    ScaleManageMent(createList, pos));
                }
                if (findObj.Count == 5)
                {
                    Vector3 pos = numberObj[pasaCurrentNo - 1].transform.position;
                    pasaCurrentNo = 1;
                    if (!LudoManager.Instance.pasaObjects.Contains(this.gameObject))
                    {
                        LudoManager.Instance.pasaObjects.Add(this.gameObject);
                    }
                    List<GameObject> createList = new List<GameObject>();
                    createList.Add(findObj[0]);
                    createList.Add(findObj[1]);
                    createList.Add(findObj[2]);
                    createList.Add(findObj[3]);
                    createList.Add(findObj[4]);
                    createList.Add(pasaObj);
                    pasaObj.transform.DOMove(new Vector3(pos.x, pos.y - 0.05f, pos.z), 0.25f);
                    pasaObj.transform.DOScale(new Vector3(scale5, scale5, scale5), 0.25f).OnComplete(() =>
                    ScaleManageMent(createList, pos));
                }
                if (findObj.Count == 6)
                {
                    Vector3 pos = numberObj[pasaCurrentNo - 1].transform.position;
                    pasaCurrentNo = 1;
                    if (!LudoManager.Instance.pasaObjects.Contains(this.gameObject))
                    {
                        LudoManager.Instance.pasaObjects.Add(this.gameObject);
                    }
                    List<GameObject> createList = new List<GameObject>();
                    createList.Add(findObj[0]);
                    createList.Add(findObj[1]);
                    createList.Add(findObj[2]);
                    createList.Add(findObj[3]);
                    createList.Add(findObj[4]);
                    createList.Add(findObj[5]);
                    createList.Add(pasaObj);
                    pasaObj.transform.DOMove(new Vector3(pos.x, pos.y - 0.05f, pos.z), 0.25f);
                    pasaObj.transform.DOScale(new Vector3(scale6, scale6, scale6), 0.25f).OnComplete(() =>
                    ScaleManageMent(createList, pos));
                }
                if (findObj.Count == 7)
                {
                    Vector3 pos = numberObj[pasaCurrentNo - 1].transform.position;
                    pasaCurrentNo = 1;
                    if (!LudoManager.Instance.pasaObjects.Contains(this.gameObject))
                    {
                        LudoManager.Instance.pasaObjects.Add(this.gameObject);
                    }
                    List<GameObject> createList = new List<GameObject>();
                    createList.Add(findObj[0]);
                    createList.Add(findObj[1]);
                    createList.Add(findObj[2]);
                    createList.Add(findObj[3]);
                    createList.Add(findObj[4]);
                    createList.Add(findObj[5]);
                    createList.Add(findObj[6]);
                    createList.Add(pasaObj);
                    pasaObj.transform.DOMove(new Vector3(pos.x + 0.08f, pos.y - 0.05f, pos.z), 0.25f);
                    pasaObj.transform.DOScale(new Vector3(scale6, scale6, scale6), 0.25f).OnComplete(() =>
                    ScaleManageMent(createList, pos));
                }
            }
        }
        else
        {
            Move_Increment_Steps1(move, listNo);
        }
    }


    public void Move_Increment_Steps1(int no, int listNo)
    {
        if (isPasaWin == true)
        {
            return;
        }
        List<GameObject> numberObj = new List<GameObject>();
        if (listNo == 1)
        {
            numberObj = LudoManager.Instance.numberObj;
        }
        else if (listNo == 2)
        {
            numberObj = LudoManager.Instance.numberObj2;
        }
        else if (listNo == 3)
        {
            if (DataManager.Instance.isTwoPlayer)
            {
                numberObj = LudoManager.Instance.numberObj2;
            }
            else
            {
                numberObj = LudoManager.Instance.numberObj3;
            }
        }
        else if (listNo == 4)
        {
            numberObj = LudoManager.Instance.numberObj4;
        }

        counter++;
        LudoManager.Instance.TimerStop();
        float moveScale = 0.15f;
        float currentScale = this.gameObject.transform.localScale.x;
        currentScale += moveScale;

        pasaCurrentNo += 1;

        isOneTimeMove = true;
        //print("org Parent No : " + orgParentNo);
        LudoManager.Instance.ScoreManage(orgParentNo, 1);
        orgNo = GetOrginialNumber(pasaCurrentNo, orgParentNo);

        //print("Second OrgNo : " + orgNo);
        SoundManager.Instance.TokenMoveSound();


        if (pasaCurrentNo == 57)
        {

            this.gameObject.transform.DOScale(new Vector3(currentScale, currentScale, currentScale), 0.16f).OnComplete(() =>
            this.gameObject.transform.DOScale(new Vector3(currentScale - moveScale, currentScale - moveScale, currentScale - moveScale), 0.1f));
            this.gameObject.transform.DOMoveY(this.gameObject.transform.position.y - 0.2f, 0.3f).OnComplete(() =>
              Check_Move_Increment_Next1(no, listNo));
        }
        else
        {
            if (isPasaWin == false)
            {
                this.gameObject.transform.DOScale(new Vector3(currentScale, currentScale, currentScale), 0.16f).OnComplete(() =>
                this.gameObject.transform.DOScale(new Vector3(currentScale - moveScale, currentScale - moveScale, currentScale - moveScale), 0.1f));
                this.gameObject.transform.DOMove(numberObj[pasaCurrentNo - 1].transform.position, 0.3f).OnComplete(() =>
                Check_Move_Increment_Next1(no, listNo));
            }

        }

    }
    void Check_Move_Increment_Next1(int no, int lno)
    {
        bool isLast = false;
        bool isEnterWin = false;
        if (no == counter)
        {
            counter = 0;
            if (orgNo == 1 || orgNo == 9 || orgNo == 22 || orgNo == 35 || orgNo == 48 || orgNo == 1 || orgNo == 14 || orgNo == 27 || orgNo == 40)
            {
                isSafe = true;
            }
            else
            {
                isSafe = false;
            }
            isLast = true;


            if (pasaCurrentNo == 57)
            {
                SoundManager.Instance.TokenHomeSound();
                LudoManager.Instance.GeneratePasaFire();
                isClick = false;
                isPasaWin = true;
                isEnterWin = true;
                LudoManager.Instance.ScoreManage(orgParentNo, 56);
                this.gameObject.transform.DOScale(new Vector3(0, 0, 0), 0.16f).OnComplete(() =>
                LudoManager.Instance.pasaObjects.Remove(this.gameObject));
            }

            if (no == 6)//&& DataManager.Instance.modeType == 1)
            {
                LudoManager.Instance.isClickAvaliableDice = 0;
                LudoManager.Instance.RestartTimer();

            }

            if (isEnterWin == true)
            {
                LudoManager.Instance.isClickAvaliableDice = 0;
                LudoManager.Instance.RestartTimer();

            }
            if (orgNo >= 0)
            {
                ScaleManageToNext(true, orgNo, true, false);
            }
            if (orgNo - 1 >= 0)
            {
                ScaleManageToNext(true, orgNo - 1, false, false);
            }
        }
        else
        {
            if (orgNo >= 0)
            {
                ScaleManageToNext(true, orgNo, false, false);
            }
            if (orgNo - 1 >= 0)
            {
                ScaleManageToNext(true, orgNo - 1, false, false);
            }
            Move_Increment_Steps1(no, lno);
        }
        if (isPasaWin == false)
        {
            for (int i = 0; i < LudoManager.Instance.pasaObjects.Count; i++)
            {
                //   LudoManager.Instance.pasaObjects[i].GetComponent<PasaManage>().RescaleEvery(true, isLast);
            }
        }
    }

    #endregion



    #region Bot Manager

    public void MoveStart_Bot(int listNo, int move)
    {
        List<GameObject> numberObj = new List<GameObject>();

        if (listNo == 1)
        {
            numberObj = LudoManager.Instance.numberObj;
        }
        else if (listNo == 2)
        {
            numberObj = DataManager.Instance.isTwoPlayer ? LudoManager.Instance.numberObj2 : LudoManager.Instance.numberObj3;
        }
        else if (listNo == 3)
        {
            numberObj = DataManager.Instance.isTwoPlayer ? LudoManager.Instance.numberObj3 : LudoManager.Instance.numberObj2;
        }
        else if (listNo == 4)
        {
            numberObj = LudoManager.Instance.numberObj4;
        }

        if (pasaCurrentNo == 0 && !isStarted)
        {
            pasaCurrentNo = 1;

            isSafe = true;
            isStarted = true;
            orgNo = GetOrginialNumber(pasaCurrentNo, orgParentNo);
            LudoManager.Instance.RestartTimer();

            List<GameObject> findObj = CheckAvaliableObjectSamePos(orgNo, true);
            if (findObj.Count == 0)
            {
                Vector3 pos = numberObj[pasaCurrentNo - 1].transform.position;
                if (!LudoManager.Instance.pasaObjects.Contains(this.gameObject))
                {
                    LudoManager.Instance.pasaObjects.Add(this.gameObject);
                }
                List<GameObject> createList = new List<GameObject>();
                createList.Add(pasaObj);
                pasaObj.transform.DOMove(numberObj[pasaCurrentNo - 1].transform.position, 0.25f);
                pasaObj.transform.DOScale(new Vector3(singlePasaScale, singlePasaScale, singlePasaScale), 0.25f).OnComplete(() =>
                ScaleManageMent(createList, pos));
            }
            else
            {
                if (findObj.Count == 1)
                {
                    Vector3 pos = numberObj[pasaCurrentNo - 1].transform.position;
                    pasaCurrentNo = 1;
                    if (!LudoManager.Instance.pasaObjects.Contains(this.gameObject))
                    {
                        LudoManager.Instance.pasaObjects.Add(this.gameObject);
                    }
                    List<GameObject> createList = new List<GameObject>();
                    createList.Add(findObj[0]);
                    createList.Add(pasaObj);
                    pasaObj.transform.DOMove(new Vector3(pos.x + 0.05f, pos.y, pos.z), 0.25f);
                    pasaObj.transform.DOScale(new Vector3(scale2, scale2, scale2), 0.25f).OnComplete(() =>
                    ScaleManageMent(createList, pos));
                }
                if (findObj.Count == 2)
                {
                    Vector3 pos = numberObj[pasaCurrentNo - 1].transform.position;
                    pasaCurrentNo = 1;
                    if (!LudoManager.Instance.pasaObjects.Contains(this.gameObject))
                    {
                        LudoManager.Instance.pasaObjects.Add(this.gameObject);
                    }
                    List<GameObject> createList = new List<GameObject>();
                    createList.Add(findObj[0]);
                    createList.Add(findObj[1]);
                    createList.Add(pasaObj);
                    pasaObj.transform.DOMove(new Vector3(pos.x, pos.y, pos.z), 0.25f);
                    pasaObj.transform.DOScale(new Vector3(scale3, scale3, scale3), 0.25f).OnComplete(() =>
                    ScaleManageMent(createList, pos));
                }
                if (findObj.Count == 3)
                {
                    Vector3 pos = numberObj[pasaCurrentNo - 1].transform.position;
                    pasaCurrentNo = 1;
                    if (!LudoManager.Instance.pasaObjects.Contains(this.gameObject))
                    {
                        LudoManager.Instance.pasaObjects.Add(this.gameObject);
                    }
                    List<GameObject> createList = new List<GameObject>();
                    createList.Add(findObj[0]);
                    createList.Add(findObj[1]);
                    createList.Add(findObj[2]);
                    createList.Add(pasaObj);
                    pasaObj.transform.DOMove(new Vector3(pos.x + 0.027f, pos.y, pos.z), 0.25f);
                    pasaObj.transform.DOScale(new Vector3(scale4, scale4, scale4), 0.25f).OnComplete(() =>
                    ScaleManageMent(createList, pos));
                }
                if (findObj.Count == 4)
                {
                    Vector3 pos = numberObj[pasaCurrentNo - 1].transform.position;
                    pasaCurrentNo = 1;
                    if (!LudoManager.Instance.pasaObjects.Contains(this.gameObject))
                    {
                        LudoManager.Instance.pasaObjects.Add(this.gameObject);
                    }
                    List<GameObject> createList = new List<GameObject>();
                    createList.Add(findObj[0]);
                    createList.Add(findObj[1]);
                    createList.Add(findObj[2]);
                    createList.Add(findObj[3]);
                    createList.Add(pasaObj);
                    pasaObj.transform.DOMove(new Vector3(pos.x + 0.025f, pos.y - 0.05f, pos.z), 0.25f);
                    pasaObj.transform.DOScale(new Vector3(scale5, scale5, scale5), 0.25f).OnComplete(() =>
                    ScaleManageMent(createList, pos));
                }
                if (findObj.Count == 5)
                {
                    Vector3 pos = numberObj[pasaCurrentNo - 1].transform.position;
                    pasaCurrentNo = 1;
                    if (!LudoManager.Instance.pasaObjects.Contains(this.gameObject))
                    {
                        LudoManager.Instance.pasaObjects.Add(this.gameObject);
                    }
                    List<GameObject> createList = new List<GameObject>();
                    createList.Add(findObj[0]);
                    createList.Add(findObj[1]);
                    createList.Add(findObj[2]);
                    createList.Add(findObj[3]);
                    createList.Add(findObj[4]);
                    createList.Add(pasaObj);
                    pasaObj.transform.DOMove(new Vector3(pos.x, pos.y - 0.05f, pos.z), 0.25f);
                    pasaObj.transform.DOScale(new Vector3(scale5, scale5, scale5), 0.25f).OnComplete(() =>
                    ScaleManageMent(createList, pos));
                }
                if (findObj.Count == 6)
                {
                    Vector3 pos = numberObj[pasaCurrentNo - 1].transform.position;
                    pasaCurrentNo = 1;
                    if (!LudoManager.Instance.pasaObjects.Contains(this.gameObject))
                    {
                        LudoManager.Instance.pasaObjects.Add(this.gameObject);
                    }
                    List<GameObject> createList = new List<GameObject>();
                    createList.Add(findObj[0]);
                    createList.Add(findObj[1]);
                    createList.Add(findObj[2]);
                    createList.Add(findObj[3]);
                    createList.Add(findObj[4]);
                    createList.Add(findObj[5]);
                    createList.Add(pasaObj);
                    pasaObj.transform.DOMove(new Vector3(pos.x, pos.y - 0.05f, pos.z), 0.25f);
                    pasaObj.transform.DOScale(new Vector3(scale6, scale6, scale6), 0.25f).OnComplete(() =>
                    ScaleManageMent(createList, pos));
                }
                if (findObj.Count == 7)
                {
                    Vector3 pos = numberObj[pasaCurrentNo - 1].transform.position;
                    pasaCurrentNo = 1;
                    if (!LudoManager.Instance.pasaObjects.Contains(this.gameObject))
                    {
                        LudoManager.Instance.pasaObjects.Add(this.gameObject);
                    }
                    List<GameObject> createList = new List<GameObject>();
                    createList.Add(findObj[0]);
                    createList.Add(findObj[1]);
                    createList.Add(findObj[2]);
                    createList.Add(findObj[3]);
                    createList.Add(findObj[4]);
                    createList.Add(findObj[5]);
                    createList.Add(findObj[6]);
                    createList.Add(pasaObj);
                    pasaObj.transform.DOMove(new Vector3(pos.x + 0.08f, pos.y - 0.05f, pos.z), 0.25f);
                    pasaObj.transform.DOScale(new Vector3(scale6, scale6, scale6), 0.25f).OnComplete(() =>
                    ScaleManageMent(createList, pos));
                }
            }
            //LudoManager.Instance.OnceTimeTurnBot();
            Invoke(nameof(WaitAfterSecondTurn), 0.3f);
        }
        else
        {
            print("Enter The Move Bot Player");
            Move_Increment_Steps_Bot(move, listNo);
        }
    }

    void WaitAfterSecondTurn()
    {
        print("Enter The Second Chance");
        LudoManager.Instance.OnceTimeTurnBot();

    }

    public void Move_Increment_Steps_Bot(int no, int listNo)
    {
        if (isPasaWin == true)
        {
            return;
        }
        List<GameObject> numberObj = new List<GameObject>();
        if (listNo == 1)
        {
            numberObj = LudoManager.Instance.numberObj;
        }
        else if (listNo == 2)
        {
            numberObj = DataManager.Instance.isTwoPlayer ? LudoManager.Instance.numberObj2 : LudoManager.Instance.numberObj3;
            //numberObj = LudoManager.Instance.numberObj2;
        }
        else if (listNo == 3)
        {
            numberObj = DataManager.Instance.isTwoPlayer ? LudoManager.Instance.numberObj3 : LudoManager.Instance.numberObj2;
            //numberObj = DataManager.Instance.isTwoPlayer ? LudoManager.Instance.numberObj2 : LudoManager.Instance.numberObj3;
        }
        else if (listNo == 4)
        {
            numberObj = LudoManager.Instance.numberObj4;
        }

        counter++;
        LudoManager.Instance.TimerStop();
        float moveScale = 0.15f;
        float currentScale = this.gameObject.transform.localScale.x;
        currentScale += moveScale;

        pasaCurrentNo += 1;

        isOneTimeMove = true;
        LudoManager.Instance.ScoreManage(orgParentNo, 1);
        orgNo = GetOrginialNumber(pasaCurrentNo, orgParentNo);

        SoundManager.Instance.TokenMoveSound();


        if (pasaCurrentNo == 57)
        {

            this.gameObject.transform.DOScale(new Vector3(currentScale, currentScale, currentScale), 0.16f).OnComplete(() =>
            this.gameObject.transform.DOScale(new Vector3(currentScale - moveScale, currentScale - moveScale, currentScale - moveScale), 0.1f));
            this.gameObject.transform.DOMoveY(this.gameObject.transform.position.y - 0.2f, 0.3f).OnComplete(() =>
              Check_Move_Increment_Next_Bot(no, listNo));
        }
        else
        {
            if (isPasaWin == false)
            {
                this.gameObject.transform.DOScale(new Vector3(currentScale, currentScale, currentScale), 0.16f).OnComplete(() =>
                this.gameObject.transform.DOScale(new Vector3(currentScale - moveScale, currentScale - moveScale, currentScale - moveScale), 0.1f));
                this.gameObject.transform.DOMove(numberObj[pasaCurrentNo - 1].transform.position, 0.3f).OnComplete(() =>
                Check_Move_Increment_Next_Bot(no, listNo));
            }

        }

    }
    void Check_Move_Increment_Next_Bot(int no, int lno)
    {
        bool isLast = false;
        bool isEnterWin = false;
        if (no == counter)
        {
            counter = 0;
            if (orgNo == 1 || orgNo == 9 || orgNo == 22 || orgNo == 35 || orgNo == 48 || orgNo == 1 || orgNo == 14 || orgNo == 27 || orgNo == 40)
            {
                isSafe = true;
            }
            else
            {
                isSafe = false;
            }
            isLast = true;


            if (pasaCurrentNo == 57)
            {
                SoundManager.Instance.TokenHomeSound();
                LudoManager.Instance.GeneratePasaFire();
                isClick = false;
                isPasaWin = true;
                isEnterWin = true;
                LudoManager.Instance.ScoreManage(orgParentNo, 56);
                this.gameObject.transform.DOScale(new Vector3(0, 0, 0), 0.16f).OnComplete(() =>
                LudoManager.Instance.pasaObjects.Remove(this.gameObject));
                LudoManager.Instance.pasaBotPlayer.Remove(this);
            }
            if (DataManager.Instance.modeType == 3 && DataManager.Instance.playerNo == 1)
            {
                //if (GameUIManager.Instance.moveCnt == 24)
                //{
                //    LudoManager.Instance.WinUserShow();
                //}
            }
            bool isTurnChange = true;
            if (no == 6 && DataManager.Instance.modeType != 3)//&& DataManager.Instance.modeType == 1)
            {
                // print("Enter the move COndition");
                LudoManager.Instance.isClickAvaliableDice = 0;
                LudoManager.Instance.RestartTimer();
                LudoManager.Instance.OnceTimeTurnBot();
                isTurnChange = false;
            }

            if (isEnterWin == true)
            {
                LudoManager.Instance.isClickAvaliableDice = 0;
                LudoManager.Instance.RestartTimer();
                LudoManager.Instance.OnceTimeTurnBot();
                isTurnChange = false;

            }
            if (orgNo >= 0)
            {
                //print("isTurnChange Bot: " + isTurnChange);
                ScaleManageToNext(true, orgNo, true, isTurnChange);
            }
            if (orgNo - 1 >= 0)
            {
                ScaleManageToNext(true, orgNo - 1, false, false);
            }
            if (isTurnChange)
            {
                LudoManager.Instance.isCheckEnter = false;
            }
        }
        else
        {
            if (orgNo >= 0)
            {
                ScaleManageToNext(true, orgNo, false, false);
            }
            if (orgNo - 1 >= 0)
            {
                ScaleManageToNext(true, orgNo - 1, false, false);
            }
            Move_Increment_Steps_Bot(no, lno);
        }
        if (isPasaWin == false)
        {
            for (int i = 0; i < LudoManager.Instance.pasaObjects.Count; i++)
            {
                //   LudoManager.Instance.pasaObjects[i].GetComponent<PasaManage>().RescaleEvery(true, isLast);
            }
        }
    }


    #endregion

}

[System.Serializable]
public class PasaMove
{
    public GameObject scaleObj;
    public List<GameObject> numberObj;
}