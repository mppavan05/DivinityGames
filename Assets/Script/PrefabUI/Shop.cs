using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddCashButton(int no)
    {
        SoundManager.Instance.ButtonClick();
        print(no);
        StartCoroutine(CashFreeManage.Instance.getToken((int)(no / 10), CashFreeManage.Instance.couponId));
    }

    public void ShopCloseButton()
    {
        SoundManager.Instance.ButtonClick();
        Destroy(this.gameObject);
    }
}
