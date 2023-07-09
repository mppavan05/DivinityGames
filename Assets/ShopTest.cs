using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class ShopTest : MonoBehaviour
{


    public GameObject CoinsDialog;
    public GameObject GullakDialog;
    public GameObject VipDialog;
    public void FirstDialogBtn(int no)
    {

        SoundManager.Instance.ButtonClick();
        switch (no)
        {
            case 1:
                GullakDialog.SetActive(true);
                CoinsDialog.SetActive(false);
                VipDialog.SetActive(false);
                break;
            case 2:
                GullakDialog.SetActive(false);
                CoinsDialog.SetActive(false);
                VipDialog.SetActive(true);
                break;
            case 3:
                VipDialog.SetActive(false);
                GullakDialog.SetActive(false);
                CoinsDialog.SetActive(true);
                break;

        }
    }
    public void CloseSHop()
    {
        this.gameObject.SetActive(false);
        Destroy(this.gameObject);
    }
}

           

