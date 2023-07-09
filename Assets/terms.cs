using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class terms : MonoBehaviour
{
    public void CloseTerms()
    {
        this.gameObject.SetActive(false);
        Destroy(this.gameObject);
    }
}
