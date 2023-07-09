using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitPanelManager : MonoBehaviour
{
   public static WaitPanelManager Instance;
       public GameObject panelObj;
   
       private void Awake()
       {
           if (Instance == null)
           {
               Instance = this;
               DontDestroyOnLoad(this.gameObject);
           }
           else
           {
               DestroyImmediate(this.gameObject);
           }
       }
   
   
       public void OpenPanel()
       {
           panelObj.SetActive(true);
       }
   
       public void ClosePanel()
       {
           panelObj.SetActive(false);
       }
}
