using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Refund : MonoBehaviour
{
    public void CloseRefund()
    {
        this.gameObject.SetActive(false);
        //Destroy(this.gameObject);
    }
}
