using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class NumberHandle : MonoBehaviour
{
    public GameObject colliderObj;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (RouletteManager.Instance.isGameRouletteStart)
        {
            colliderObj.SetActive(true);

            RouletteManager.Instance.isRoundOn = true;
            RouletteManager.Instance.greenLineBoard[RouletteManager.Instance.findNo].DOColor(RouletteManager.Instance.colorOnGreen, 0.1f);
            RouletteManager.Instance.isGameRouletteStart = false;
            RouletteeBoardDataManage.Instance.OpenWinBox();

        }
        //green True
    }
}
