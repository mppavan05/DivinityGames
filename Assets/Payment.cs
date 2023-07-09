using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Payment : MonoBehaviour
{
    public GameObject PaymentSettings;
    public GameObject UPiPanel;
    public GameObject ParentOBJ;
    public GameObject WithdrawError;
    public GameObject WinningBalance;

    public void OpenPaymentSettings()
    {
        Instantiate(PaymentSettings, ParentOBJ.transform);
    }
    public void OpenUPI()
    {
        Instantiate(UPiPanel, ParentOBJ.transform);
    }
    public void CloseWithdrawPanel()
    {
        this.gameObject.SetActive(false);
    }
    public void OpenError()
    {
        WithdrawError.SetActive(true);
        WinningBalance.SetActive(false);
    }
    public void GoBack()
    {
        WinningBalance.SetActive(true );
        WithdrawError.SetActive(false);
    }

}
