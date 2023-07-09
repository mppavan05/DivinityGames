using System.Collections;
using UnityEngine;

public class trajectoryScript : MonoBehaviour
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

    private CarromGameManager gameManager;

    private StrikerAnimator strikerHandler;

    private CircleCollider2D circleCollider2d;

    private SpriteRenderer spriteRender;

    private AudioSource source;

    private bool audioEnabled;

    private Vector3 lastfingurePos;

    private Vector2 velocity;

    private bool canStrike = true;

    private void Start()
    {
        gameManager = Object.FindObjectOfType<CarromGameManager>();
        strikerHandler = GetComponent<StrikerAnimator>();
        source = GetComponent<AudioSource>();
        audioEnabled = PlayerPrefs.GetInt("audio", 1) == 1;
        ball = base.gameObject;
        ballClick = base.gameObject.transform.GetChild(0).gameObject;
        arrow = base.gameObject.transform.GetChild(1).gameObject;
        circleCollider2d = ball.GetComponent<CircleCollider2D>();
        spriteRender = ball.GetComponent<SpriteRenderer>();
        bool flag = ballClick == null;

        trajectoryDots = GameObject.Find("Trajectory Dots(Clone)");

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
        for (int j = numberOfDots; j < 40; j++)
        {
        }
        trajectoryDots.SetActive(false);
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
        if (raycastHit2D.collider != null && !ballIsClicked2 && !gameManager.isGameOver)//&& !gameManager.IsChatPannelOpen())
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
        else
        {
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
        if (Input.GetKey(KeyCode.Mouse0) && ballIsClicked && ((ballRB.velocity.x == 0f && ballRB.velocity.y == 0f) || grabWhileMoving) && canStrike)
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
            if (Mathf.Sqrt(ballFingerDiff.x * ballFingerDiff.x + ballFingerDiff.y * ballFingerDiff.y) > 0.4f)
            {
                arrow.SetActive(true);

            }
            else
            {
                arrow.SetActive(false);
                ResetTrajectoryDotPositions();

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
        if (!Input.GetKeyUp(KeyCode.Mouse0))
        {
            return;
        }
        ballIsClicked2 = false;
        if (trajectoryDots.activeInHierarchy)
        {
            if (explodeEnabled)
            {
                StartCoroutine(explode());
            }
            if (gameManager.isPlayerTurn && canStrike)
            {
                //gameManager.FinishTurn();
                trajectoryDots.SetActive(false);
                arrow.SetActive(false);
                ResetTrajectoryDotPositions();
                bool isKinematic = ballRB.isKinematic;
            }
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

    public void MakeStrikerStrike()
    {
        canStrike = true;
    }


}
