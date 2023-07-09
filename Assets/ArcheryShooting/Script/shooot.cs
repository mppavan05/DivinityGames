
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class shooot : MonoBehaviour
{
    //public enum GameStates
    //{
    //    menu = 0,
    //    game = 1,
    //    gameOver = 2
    //}

    public static shooot Instance;
    public GameObject arrow;

    public Camera aimCam;

    public Camera arrowCam;

    public GameObject crossHair;

    private GameObject arrowGO;

    public bool isShooting;

    public bool isDrawingBow;

    public AudioClip drawBow;

    public AudioClip releaseBow;

    public AudioClip fanfare;

    private float fovMax = 60f;

    private float fovMin = 50f;

    private float fovAkt;

    private float windSpeed;

    private int arrowCount;

    private int level;

    private int distance;

    public int NoOfMoves;


    public static int AvlblMoves;

    public static int hittedTargets;

    //public static int TragetNo;

    private bool isDownReady;

    private bool isUpReady;

    //public GameStates gameState;

    private float startTime;

    private float endTime;

    private Animator BowAnimator;
    public List<GameObject> arrowObjList;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void Shooting(bool Shooting)
    {
        isShooting = Shooting;
    }

    private void Start()
    {
        //NoOfTarget = GameObject.FindGameObjectsWithTag("Target").Length;
        ArcheryScroreManager.isGameOver = false;
        ArcheryScroreManager.isPaused = false;
        isShooting = false;
        isDrawingBow = false;
        fovAkt = fovMax;
        arrowCam.enabled = false;
        aimCam.enabled = false;
        crossHair.SetActive(false);
        startTime = (endTime = 0f);
        setLevel(1);
        //TragetNo = NoOfTarget;
        hittedTargets = 0;
        AvlblMoves = NoOfMoves;
        isDownReady = true;
        isUpReady = false;
        BowSelector(PlayerPrefs.GetInt("Bow"));
        //MonoBehaviour.print(PlayerPrefs.GetInt("Bow"));


    }



    public void setLevel(int _level)
    {
        level = _level;
        //gameState = GameStates.game;
        aimCam.enabled = true;
    }

    private void Update()
    {
        ////GameStates gameStates = gameState;
        //if (gameStates != GameStates.game)
        //{
        //    return;
        //}


        if (ArcheryScroreManager.isGameOver || ArcheryScroreManager.isPaused)
        {
            return;
        }

        if (Input.GetButtonUp("Fire1") && !isShooting && !ArcheryScroreManager.isGameOver && !ArcheryScroreManager.isPaused)
        //if (Input.GetButtonUp("Fire1") && !ArcheryScroreManager.isGameOver && !ArcheryScroreManager.isPaused)
        {
            endTime = Time.time;
            isDrawingBow = false;
            if (endTime - startTime > 1f)
            {
                //BowAnimator.SetTrigger("Release");
                if (DataManager.Instance.GetSound() == 0)
                {
                    GetComponent<AudioSource>().PlayOneShot(releaseBow);
                }
                arrowGO = Object.Instantiate(arrow, aimCam.transform.position, Quaternion.Euler(new Vector3(0f, aimCam.transform.localEulerAngles.y - 90f, 360f - aimCam.transform.localEulerAngles.x)));
                arrowGO.name = "Arrow";
                arrowGO.GetComponent<arrowScript>().shootArrow(aimCam.transform.localEulerAngles);
                arrowGO.GetComponent<arrowScript>().setCam(arrowCam, true);

                arrowObjList.Add(arrowGO);
                arrowGO.GetComponent<arrowScript>().setMainCam(aimCam);
                // arrowGO.GetComponent<arrowScript>().setWindSpeed(windSpeed);
                arrowGO.GetComponent<arrowScript>().setGameManager(base.gameObject);


                isShooting = true;
                fovAkt = fovMax;
                aimCam.fieldOfView = fovAkt;
                //MonoBehaviour.print("<color=red>Up After 5 Sec</color>");
            }
            else
            {
                //MonoBehaviour.print("<color=yellow>Up Before 5 Sec</color>");
                //BowAnimator.SetTrigger("StrToRI");
            }
            BowOscilationDisable(PlayerPrefs.GetInt("Bow"));
            crossHair.SetActive(false);
        }
        if (!Input.GetButtonDown("Fire1") || isDrawingBow || !isShooting)
        {
        }
        if (!Input.GetButton("Fire1") || ArcheryScroreManager.isGameOver || ArcheryScroreManager.isPaused)
        {
            return;
        }
        fovAkt = (fovAkt -= 0.5f);
        fovAkt = Mathf.Clamp(fovAkt, fovMin, fovMax);
        aimCam.fieldOfView = fovAkt;
        BowOscilationEnable(PlayerPrefs.GetInt("Bow"));
        if (!isDrawingBow && !isShooting)
        {
            if (DataManager.Instance.GetSound() == 0)
            {
                GetComponent<AudioSource>().PlayOneShot(drawBow);
            }
            isDrawingBow = true;
            startTime = Time.time;
            crossHair.SetActive(true);
            // BowAnimator.SetTrigger("String");
            //MonoBehaviour.print("<color=blue>Inside Down</color>");
        }
    }

    public void BowSelector(int _Value)
    {
        aimCam.transform.GetChild(_Value).gameObject.SetActive(true);
        BowAnimator = aimCam.transform.GetChild(_Value).GetComponent<Animator>();

        BowAnimator.SetTrigger("String");//Greejesh Bow
    }

    public void BowOscilationEnable(int bowNum)
    {
        switch (bowNum)
        {
            case 0:
                //if (!aimCam.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Moving3"))
                //{
                //    aimCam.GetComponent<Animator>().SetTrigger("Moving3");
                //}
                break;
            case 1:
                //if (!aimCam.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Moving2"))
                //{
                //    aimCam.GetComponent<Animator>().SetTrigger("Moving2");
                //}
                break;
            case 2:
                //if (!aimCam.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Moving"))
                //{
                //    aimCam.GetComponent<Animator>().SetTrigger("Moving");
                //}
                break;
        }
    }

    public void BowOscilationDisable(int bowNum)
    {
        switch (bowNum)
        {
            case 0:
                //aimCam.GetComponent<Animator>().SetTrigger("Idle3");
                break;
            case 1:
                //aimCam.GetComponent<Animator>().SetTrigger("Idle2");
                break;
            case 2:
                //aimCam.GetComponent<Animator>().SetTrigger("Idle");
                break;
        }
    }
}
