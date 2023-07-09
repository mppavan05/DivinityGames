using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RummyTest : MonoBehaviour
{
    [SerializeField] private GameObject m_PoolPanel;
    [SerializeField] private GameObject m_PointPanel;
    [SerializeField] private GameObject m_DealPanel;

    [SerializeField] private GameObject m_101Panel;
    [SerializeField] private GameObject m_201Panel;

    public void TabDialogBtn(int no)
    {

        SoundManager.Instance.ButtonClick();
        switch (no)
        {
            case 1:
                m_PointPanel.SetActive(true);
                m_PoolPanel.SetActive(false);
                m_DealPanel.SetActive(false);

                break;
            case 2:
                m_PointPanel.SetActive(false);
                m_PoolPanel.SetActive(true);
                m_DealPanel.SetActive(false);

                break;
            case 3:
                m_PointPanel.SetActive(false);
                m_PoolPanel.SetActive(false);
                m_DealPanel.SetActive(true);
                break;


        }
    }

    public void UppaerDialogBtn(int no)
    {

        SoundManager.Instance.ButtonClick();
        switch (no)
        {
            case 1:
                m_101Panel.SetActive(true);
                m_201Panel.SetActive(false);
                break;


            case 2:
                m_101Panel.SetActive(false);
                m_201Panel.SetActive(true);
                break;



        }
    }
}
