using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logger : MonoBehaviour
{
    public static Logger log;

    private void Awake()
    {
        log = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public void Log(string msg,string data)
    {
        Debug.Log(msg + "===:::" + data);  
    }

    public void Log(string data)
    {
        Debug.Log("===:::" + data);
    }
}
