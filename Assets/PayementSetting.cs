using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PayementSetting : MonoBehaviour
{
    public GameObject Bank;
    public GameObject BankType;

    public void OpenBank()
    {
        Bank.SetActive(true);
        BankType.SetActive(false);
    }
    public void OpenBankType()
    {
        BankType.SetActive(true);
        Bank.SetActive(false);
    }
    public void ClosePaymentSetting()
    {
        this.gameObject.SetActive(false);
    }
}
