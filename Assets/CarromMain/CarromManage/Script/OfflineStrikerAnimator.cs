using UnityEngine;

public class OfflineStrikerAnimator : MonoBehaviour
{
    protected Rigidbody2D ballRB;

    protected CircleCollider2D circlecollider;

    protected SpriteRenderer ballRenderer;

    private OfflineGameManager gamemanager;

    protected bool isMoving;

    private Transform restPositionTop;

    private Transform restPositionBottom;

    private AudioSource source;

    public bool isMasterPlayer;

    private bool animateStriker;

    private float animateStrikerDuration;

    private float animateStrikerEndDuration = 1f;

    private bool animateStrikerOut;

    private float animateStrikerOutDuration;

    private float animateStrikerOutEndDuration = 1f;

    private bool animateStrikerIn;

    private float animateStrikerInDuration;

    private float animateStrikerInEndDuration = 1f;

    private void Start()
    {
        circlecollider = GetComponent<CircleCollider2D>();
        ballRB = GetComponent<Rigidbody2D>();
        ballRenderer = GetComponent<SpriteRenderer>();
        gamemanager = Object.FindObjectOfType<OfflineGameManager>();
        restPositionTop = GameObject.Find("RestPositionTop").transform;
        restPositionBottom = GameObject.Find("RestPositionBottom").transform;
        source = GetComponent<AudioSource>();
    }

    protected void StrikerStoppedMoving()
    {
        if (animateStrikerDuration > animateStrikerEndDuration && animateStriker)
        {
            animateStriker = false;
            circlecollider.enabled = true;
            gamemanager.isPlayerTurn = true;
        }
    }

    protected void UpdateStrikerPosition()
    {
        if (animateStriker)
        {
            animateStrikerDuration += Time.deltaTime;
            float t = animateStrikerDuration / animateStrikerEndDuration;
            ballRB.transform.position = Vector3.Lerp(ballRB.transform.position, (!isMasterPlayer) ? new Vector3(0f, 2.15f, 0f) : new Vector3(0f, -2.15f, 0f), t);
        }
    }

    private void Update()
    {
        StrikerStoppedMoving();
        if (animateStrikerOutDuration > animateStrikerOutEndDuration && animateStrikerOut)
        {
            animateStrikerOut = false;
            animateStriker = false;
            circlecollider.enabled = true;
        }
        if (animateStrikerInDuration > animateStrikerInEndDuration && animateStrikerIn)
        {
            animateStrikerIn = false;
            gamemanager.isPlayerTurn = true;
        }
        UpdateStrikerPosition();
        if (animateStrikerOut)
        {
            animateStrikerOutDuration += Time.deltaTime;
            float t = animateStrikerOutDuration / animateStrikerOutEndDuration;
            ballRB.transform.position = Vector3.Lerp(ballRB.transform.position, (!isMasterPlayer) ? restPositionTop.position : restPositionBottom.position, t);
        }
        if (animateStrikerIn)
        {
            animateStrikerInDuration += Time.deltaTime;
            float t2 = animateStrikerInDuration / animateStrikerInEndDuration;
            ballRB.transform.position = Vector3.Lerp(ballRB.transform.position, (!isMasterPlayer) ? new Vector3(0f, 2.15f, 0f) : new Vector3(0f, -2.15f, 0f), t2);
        }
        if (isMoving && AllStoppedMoving())
        {
            isMoving = false;
            if (!circlecollider.enabled)
            {
                Debug.Log("Ball stopped inside hole");
                Invoke("HandleRoundComplete", 1f);
            }
            else
            {
                Debug.Log("Ball stopped");
                circlecollider.isTrigger = true;
                gamemanager.RoundComplete(2f);
            }
        }
        if (ballRB.velocity.magnitude <= 0f && source.isPlaying)
        {
            source.Stop();
        }
    }

    private void HandleRoundComplete()
    {
        ResetStrikerColor();
        MoveStrikerOut();
        gamemanager.RoundComplete(2f);
    }

    protected void ResetStrikerColor()
    {
        ballRB.constraints = RigidbodyConstraints2D.None;
        ballRenderer.color = Color.white;
        base.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
    }

    private bool AllStoppedMoving()
    {
        if (OfflineGameManager.Instance != null)
        {
            foreach (Puck puck in OfflineGameManager.Instance.pucks)
            {
                if (puck.rigidbody2D.velocity.magnitude > 0f)
                {
                    return false;
                }
            }
        }
        if (ballRB.velocity.magnitude > 0f)
        {
            return false;
        }
        return true;
    }

    public void MoveBackStriker()
    {
        ballRenderer.color = Color.white;
        ballRB.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
        ballRB.velocity = Vector2.zero;
        circlecollider.isTrigger = true;
        AnimateStriker();
    }

    public void EnableOrDisableCollider(bool state)
    {
        if (circlecollider != null)
        {
            circlecollider.isTrigger = state;
        }
    }

    public void SetStrikerMoving()
    {
        isMoving = true;
    }

    private void AnimateStriker()
    {
        animateStrikerDuration = 0f;
        animateStriker = true;
    }

    public void MoveStrikerOut()
    {
        animateStrikerOut = true;
        animateStrikerOutDuration = 0f;
        EnableOrDisableCollider(true);
        Debug.Log("Greejesh Change Turn User");
    }

    public void MoveStrikerIn()
    {
        animateStrikerIn = true;
        animateStrikerInDuration = 0f;
        EnableOrDisableCollider(true);
    }
}
