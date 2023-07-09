using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class privacy : MonoBehaviour
{
   public void ClosePrivacy()
    {
        this.gameObject.SetActive(false);
        Destroy(this.gameObject);
    }
}
