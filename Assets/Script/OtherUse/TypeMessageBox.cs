using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TypeMessageBox : MonoBehaviour
{

    public Text messageTxt;

    public void Update_Message_Box(string msg)
    {
        messageTxt.text = msg;
        Canvas.ForceUpdateCanvases();

    }

}
