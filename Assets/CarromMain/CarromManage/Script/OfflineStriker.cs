using System.Collections;
using UnityEngine;

public class OfflineStriker : MonoBehaviour
{
    public Sprite dotSprite;

    public bool changeSpriteAfterStart;

    public float initialDotSize;

    public int numberOfDots;

    public float dotSeparation;

    public float dotShift;

    public float idleTime;

    public GameObject trajectoryDots;

    private GameObject ball;

    private Rigidbody2D ballRB;

    private Vector3 ballPos;

    private Vector3 fingerPos;

    private Vector3 ballFingerDiff;

    private Vector2 shotForce;

    private float x1;

    private float y1;

    private GameObject helpGesture;

    private float idleTimer = 7f;

    private bool ballIsClicked;

    private bool ballIsClicked2;

    private GameObject ballClick;

    public float shootingPowerX;

    public float shootingPowerY;

    public bool usingHelpGesture;

    public bool explodeEnabled;

    public bool grabWhileMoving;

    public GameObject[] dots;

    public bool mask;

    private BoxCollider2D[] dotColliders;

    private GameObject arrow;

    public Color enabledColor;

    public Color disabledColor;

    public Color strikerInHoleColor;

    private OfflineGameManager gameManager;

    private OfflineStrikerAnimator strikerHandler;

    private CircleCollider2D circleCollider2d;

    private SpriteRenderer spriteRender;

    private AudioSource source;

    private bool audioEnabled;

    private Vector3 lastfingurePos;

    public bool canStrike = true;

    float beforeValue = 0;
    private void Start()
    {
        gameManager = Object.FindObjectOfType<OfflineGameManager>();
        strikerHandler = GetComponent<OfflineStrikerAnimator>();
        source = GetComponent<AudioSource>();
        audioEnabled = PlayerPrefs.GetInt("audio", 1) == 1;
        ball = base.gameObject;
        ballClick = base.gameObject.transform.GetChild(0).gameObject;
        arrow = base.gameObject.transform.GetChild(1).gameObject;
        circleCollider2d = ball.GetComponent<CircleCollider2D>();
        spriteRender = ball.GetComponent<SpriteRenderer>();
        bool flag = ballClick == null;
        if (usingHelpGesture)
        {
            helpGesture = GameObject.Find("Help Gesture");
        }
        ballRB = GetComponent<Rigidbody2D>();
        Transform obj = trajectoryDots.transform;
        float x = initialDotSize;
        float y = initialDotSize;
        obj.localScale = new Vector3(x, y, trajectoryDots.transform.localScale.z);
        for (int i = 0; i < numberOfDots; i++)
        {
            dots[i] = trajectoryDots.transform.GetChild(i).gameObject;
            if (dotSprite != null)
            {
                dots[i].GetComponent<SpriteRenderer>().sprite = dotSprite;
            }
        }
        trajectoryDots.SetActive(false);
        EnableOrDisableDots(false);
    }

    private void Update()
    {
        if (numberOfDots > 40)
        {
            numberOfDots = 40;
        }
        if (usingHelpGesture)
        {
            helpGesture.transform.position = new Vector3(ballPos.x, ballPos.y, ballPos.z);
        }
        RaycastHit2D raycastHit2D = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (raycastHit2D.collider != null && !ballIsClicked2)
        {
            if (raycastHit2D.collider.gameObject.name == ballClick.gameObject.name)
            {
                ballIsClicked = true;
            }
            else
            {
                ballIsClicked = false;
            }
        }
        else
        {
            ballIsClicked = false;
        }
        if (ballIsClicked2)
        {
            ballIsClicked = true;
        }
        float num = ballRB.velocity.x * ballRB.velocity.x;
        float y = ballRB.velocity.y;
        if (num + y * ballRB.velocity.y <= 0.0085f)
        {
            ballRB.velocity = new Vector2(0f, 0f);
            idleTimer -= Time.deltaTime;
        }
        else if (trajectoryDots.activeSelf)
        {
            trajectoryDots.SetActive(false);
            arrow.SetActive(false);
            ResetTrajectoryDotPositions();
        }
        if (usingHelpGesture && idleTimer <= 0f)
        {
            helpGesture.GetComponent<Animator>().SetBool("Inactive", true);
        }
        ballPos = ball.transform.position;
        if (changeSpriteAfterStart)
        {
            for (int i = 0; i < numberOfDots; i++)
            {
                if (dotSprite != null)
                {
                    dots[i].GetComponent<SpriteRenderer>().sprite = dotSprite;
                }
            }
        }
        if (Input.GetKey(KeyCode.Mouse0) && ballIsClicked && ((ballRB.velocity.x == 0f && ballRB.velocity.y == 0f) || grabWhileMoving) && canStrike && gameManager.isPlayerTurn)
        {
            ballIsClicked2 = true;
            if (usingHelpGesture)
            {
                idleTimer = idleTime;
                helpGesture.GetComponent<Animator>().SetBool("Inactive", false);
            }
            fingerPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            fingerPos.z = 0f;
            if (grabWhileMoving)
            {
                ballRB.velocity = new Vector2(0f, 0f);
                ballRB.isKinematic = true;
            }
            ballFingerDiff = ballPos - fingerPos;
            ballFingerDiff = Vector3.ClampMagnitude(ballFingerDiff, 1.5f);
            shotForce = new Vector2(ballFingerDiff.x * shootingPowerX, ballFingerDiff.y * shootingPowerY);
            float num2 = Mathf.Min(1f + ballFingerDiff.magnitude * 4f, 7f);
            arrow.transform.localScale = new Vector3(num2, num2, 1f);

            float checkValue = Mathf.Sqrt(ballFingerDiff.x * ballFingerDiff.x + ballFingerDiff.y * ballFingerDiff.y);

            CarromSocketManager.Instance.Strike_Direction_Send(ballFingerDiff);
            if (checkValue > 0.4f)
            {
                if (beforeValue != checkValue)
                {
                    beforeValue = checkValue;

                    print("Ball Finger Diff : " + ballFingerDiff);
                    //print("Greejesh Angel Get");
                }
                trajectoryDots.SetActive(true);
                arrow.SetActive(true);
                EnableOrDisableDots(true);
            }
            else
            {
                trajectoryDots.SetActive(false);
                arrow.SetActive(false);
                ResetTrajectoryDotPositions();
                EnableOrDisableDots(false);
                if (ballRB.isKinematic)
                {
                    ballRB.isKinematic = false;
                }
            }
            for (int j = 0; j < numberOfDots; j++)
            {
                x1 = ballPos.x + shotForce.x * Time.fixedDeltaTime * (dotSeparation * (float)j + dotShift);
                y1 = ballPos.y + shotForce.y * Time.fixedDeltaTime * (dotSeparation * (float)j + dotShift);
                Transform obj = dots[j].transform;
                float x = x1;
                float y2 = y1;
                obj.position = new Vector3(x, y2, dots[j].transform.position.z);
            }
        }
        if (!Input.GetKeyUp(KeyCode.Mouse0) || !ballIsClicked)
        {
            return;
        }
        ballIsClicked2 = false;
        if (!trajectoryDots.activeInHierarchy)
        {
            return;
        }
        if (explodeEnabled)
        {
            StartCoroutine(explode());
        }
        if (canStrike && gameManager.isPlayerTurn)
        {


            gameManager.isPlayerTurn = false;
            gameManager.FinishTurn();
            trajectoryDots.SetActive(false);
            arrow.SetActive(false);
            ResetTrajectoryDotPositions();
            EnableOrDisableDots(false);
            strikerHandler.SetStrikerMoving();
            DisableBallTrigger();
            Vector2 forceVelo = new Vector2(shotForce.x, shotForce.y);
            ballRB.velocity = forceVelo;

            if (forceVelo.x < 0)
            {
                forceVelo.x = Mathf.Abs(forceVelo.x);
            }
            else
            {
                forceVelo.x = -forceVelo.x;
            }

            if (forceVelo.y < 0)
            {
                forceVelo.y = Mathf.Abs(forceVelo.x);
            }
            else
            {
                forceVelo.x = -forceVelo.x;
            }
            print("Send Force : " + forceVelo);
            print("Sedn Shot Force : " + shotForce);
            CarromSocketManager.Instance.Strike_Fire_Send(forceVelo);
            print("Fire Velocity");
            if (ballRB.isKinematic)
            {
                ballRB.isKinematic = false;
            }
            ballIsClicked2 = false;
        }
    }

    #region Socket MainTain

    public void Strike_Fire(Vector2 vector)
    {
        trajectoryDots.SetActive(false);
        arrow.SetActive(false);
        ResetTrajectoryDotPositions();
        EnableOrDisableDots(false);
        strikerHandler.SetStrikerMoving();
        DisableBallTrigger();

        print("Vector Force : " + vector);
        print("Shot Force : " + shotForce);
        ballRB.velocity = new Vector2(vector.x, vector.y);
        if (ballRB.isKinematic)
        {
            ballRB.isKinematic = false;
        }
    }

    public void Strike_Dire_Rec(Vector3 posSend)
    {
        print("pos Send : " + posSend);
        //ballIsClicked2 = true;
        if (usingHelpGesture)
        {
            idleTimer = idleTime;
            helpGesture.GetComponent<Animator>().SetBool("Inactive", false);
        }

        posSend.z = 0f;
        if (grabWhileMoving)
        {
            ballRB.velocity = new Vector2(0f, 0f);
            ballRB.isKinematic = true;
        }
        //ballFingerDiff = ballPos - posSend;
        ballFingerDiff = posSend;
        // ballFingerDiff = Vector3.ClampMagnitude(ballFingerDiff, 1.5f);
        shotForce = new Vector2(ballFingerDiff.x * shootingPowerX, ballFingerDiff.y * shootingPowerY);
        float num2 = Mathf.Min(1f + ballFingerDiff.magnitude * 4f, 7f);
        arrow.transform.localScale = new Vector3(num2, num2, 1f);

        float checkValue = Mathf.Sqrt(ballFingerDiff.x * ballFingerDiff.x + ballFingerDiff.y * ballFingerDiff.y);

        if (checkValue > 0.4f)
        {
            if (beforeValue != checkValue)
            {
                beforeValue = checkValue;

                print("Greejesh Angel Get");
            }
            trajectoryDots.SetActive(true);
            arrow.SetActive(true);
            EnableOrDisableDots(true);
        }
        else
        {
            trajectoryDots.SetActive(false);
            arrow.SetActive(false);
            ResetTrajectoryDotPositions();
            EnableOrDisableDots(false);
            if (ballRB.isKinematic)
            {
                ballRB.isKinematic = false;
            }
        }
        for (int j = 0; j < numberOfDots; j++)
        {
            x1 = ballPos.x + shotForce.x * Time.fixedDeltaTime * (dotSeparation * (float)j + dotShift);
            y1 = ballPos.y + shotForce.y * Time.fixedDeltaTime * (dotSeparation * (float)j + dotShift);
            Transform obj = dots[j].transform;
            float x = x1;
            float y2 = y1;
            obj.position = new Vector3(x, y2, dots[j].transform.position.z);
        }
    }


    #endregion

    public void DisableBallTrigger()
    {
        circleCollider2d.isTrigger = false;
        if (audioEnabled)
        {
            source.Play();
        }
    }

    public void StopAudio()
    {
        if (audioEnabled && source.isPlaying)
        {
            source.Play();
        }
    }

    public void EnableBallTrigger()
    {
        circleCollider2d.isTrigger = true;
    }

    private void HideTrajectory()
    {
        for (int i = 0; i < numberOfDots; i++)
        {
            dots[i].transform.gameObject.SetActive(false);
        }
    }

    private void showTrajectory()
    {
        for (int i = 0; i < numberOfDots; i++)
        {
            dots[i].transform.gameObject.SetActive(true);
        }
    }

    public IEnumerator explode()
    {
        yield return new WaitForSeconds(Time.fixedDeltaTime * (dotSeparation * ((float)numberOfDots - 1f)));
        Debug.Log("exploded");
    }

    public void collided(GameObject dot)
    {
        Debug.LogError("******************");
        for (int i = 0; i < numberOfDots; i++)
        {
            if (dot.name == "Dot (" + i + ")")
            {
                for (int j = i + 1; j < numberOfDots; j++)
                {
                    dots[j].gameObject.GetComponent<SpriteRenderer>().enabled = false;
                }
            }
        }
    }

    public void uncollided(GameObject dot)
    {
        for (int i = 0; i < numberOfDots; i++)
        {
            if (!(dot.name == "Dot (" + i + ")"))
            {
                continue;
            }
            for (int num = i - 1; num > 0; num--)
            {
                if (!dots[num].gameObject.GetComponent<SpriteRenderer>().enabled)
                {
                    Debug.Log("nigggssss");
                    return;
                }
            }
            if (!dots[i].gameObject.GetComponent<SpriteRenderer>().enabled)
            {
                for (int num2 = i; num2 > 0; num2--)
                {
                    dots[num2].gameObject.GetComponent<SpriteRenderer>().enabled = true;
                }
            }
        }
    }

    private void ResetTrajectoryDotPositions()
    {
        for (int i = 0; i < numberOfDots; i++)
        {
            dots[i].transform.position = Vector3.zero;
        }
    }

    public void EnableOrDisableDots(bool state)
    {
        if (trajectoryDots != null)
        {
            trajectoryDots.SetActive(state);
        }
    }

    public void ChangeStrikerColor(int color)
    {
        if (color == 0)
        {
            spriteRender.color = strikerInHoleColor;
        }
        else
        {
            spriteRender.color = Color.white;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Puck")
        {
            canStrike = false;
            spriteRender.color = disabledColor;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Puck")
        {
            canStrike = true;
            spriteRender.color = enabledColor;
        }
    }
}
