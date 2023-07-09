using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Profile : MonoBehaviour
{
    public GameObject KycPrefab;
    public GameObject ParentPrefab;

    public void OpenKyC()
    {
        Instantiate(KycPrefab, ParentPrefab.transform);
    }
    public void CLoseProfile()
    {
        this.gameObject.SetActive(false);
    }
}
