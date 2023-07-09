using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WithdrawErrorPanel : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        MainMenuManager.Instance.screenObj.Add(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void BackButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        MainMenuManager.Instance.screenObj.Remove(this.gameObject);
        Destroy(this.gameObject);
    }


}
