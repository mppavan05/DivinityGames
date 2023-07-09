//using Photon.Pun;
using UnityEngine;

public class StrikerAnimator : MonoBehaviour
{
    private Rigidbody2D ballRB;

    private CircleCollider2D circlecollider;

    private SpriteRenderer ballRenderer;

    private CarromGameManager gamemanager;

    private bool isMoving;

    private Transform restPositionTop;

    private Transform restPositionBottom;

    private AudioSource source;

    private bool audioEnabled;

    private bool animateStriker;

    private float animateStrikerDuration;

    private float animateStrikerEndDuration = 1f;

    private Vector3 position;

    private bool animateStrikerOut;

    private float animateStrikerOutDuration;

    private float animateStrikerOutEndDuration = 1f;

    private Vector3 outPosition;

    private bool animateStrikerIn;

    private float animateStrikerInDuration;

    private float animateStrikerInEndDuration = 1f;

    private Vector3 inPosition;

    private void Start()
    {
        circlecollider = GetComponent<CircleCollider2D>();
        ballRB = GetComponent<Rigidbody2D>();
        ballRenderer = GetComponent<SpriteRenderer>();
        gamemanager = FindObjectOfType<CarromGameManager>();
        restPositionTop = GameObject.Find("RestPositionTop").transform;
        restPositionBottom = GameObject.Find("RestPositionBottom").transform;
        source = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (animateStrikerDuration > animateStrikerEndDuration && animateStriker)
        {
            animateStriker = false;
            circlecollider.enabled = true;
        }
        if (animateStrikerOutDuration > animateStrikerOutEndDuration && animateStrikerOut)
        {
            animateStrikerOut = false;
            animateStriker = false;
            circlecollider.enabled = true;
        }
        if (animateStrikerInDuration > animateStrikerInEndDuration && animateStrikerIn)
        {
            animateStrikerIn = false;
        }
        if (animateStriker)
        {
            animateStrikerDuration += Time.deltaTime;
            float t = animateStrikerDuration / animateStrikerEndDuration;
            ballRB.transform.position = Vector3.Lerp(ballRB.transform.position, position, t);
        }
        if (animateStrikerOut)
        {
            animateStrikerOutDuration += Time.deltaTime;
            float t2 = animateStrikerOutDuration / animateStrikerOutEndDuration;
            ballRB.transform.position = Vector3.Lerp(ballRB.transform.position, outPosition, t2);
        }
        if (animateStrikerIn)
        {
            animateStrikerInDuration += Time.deltaTime;
            float t3 = animateStrikerInDuration / animateStrikerInEndDuration;
            ballRB.transform.position = Vector3.Lerp(ballRB.transform.position, inPosition, t3);
        }
        if (isMoving && AllStoppedMoving())//&& !base.photonView.IsMine && PhotonNetwork.IsConnected)
        {
            isMoving = false;
            //gamemanager.BeginWaitForOpponent();
            //base.photonView.RPC("AllStopped", RpcTarget.Others, null);
        }
        if (ballRB.velocity.magnitude <= 0f && source.isPlaying)
        {
            source.Stop();
            //base.photonView.RPC("StopAudio", RpcTarget.Others, null);
        }
    }

    //[PunRPC]
    public void AllStopped()
    {
        isMoving = false;
        if (!circlecollider.enabled)
        {
            Debug.LogError("Ball stopped inside hole");
            HandleRoundComplete();
        }
        else
        {
            //base.photonView.RPC("EnableBallTrigger", RpcTarget.All, null);
            //gamemanager.RoundComplete();
        }
    }

    private void HandleRoundComplete()
    {
        //base.photonView.RPC("ReEnableStriker", RpcTarget.All, null);
        //gamemanager.RoundComplete();
    }

    //[PunRPC]
    public void ReEnableStriker()
    {
        ballRB.constraints = RigidbodyConstraints2D.None;
        ballRB.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    private bool AllStoppedMoving()
    {
        //foreach (Puck puck in CarromGameManager.Instance.pucks)
        //{
        //    //if (puck.photonView != null && puck.rigidbody2D.velocity.magnitude > 0f)
        //    //{
        //    //    return false;
        //    //}
        //}
        if (Mathf.Abs(ballRB.velocity.magnitude) > 0f)
        {
            return false;
        }
        return true;
    }

    //[PunRPC]
    public void MoveBackStriker(Vector3 position)
    {
        AudioManager.getInstance().PlaySound(AudioManager.PLAY_STRIKER_DRAG);
        ballRenderer.color = Color.white;
        ballRB.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
        ballRB.velocity = Vector2.zero;
        //base.photonView.RPC("EnableOrDisableCollider", RpcTarget.Others, true);
        circlecollider.isTrigger = true;
        AnimateStriker(position);
    }

    //[PunRPC]
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

    private void AnimateStriker(Vector3 position)
    {
        this.position = position;
        animateStrikerDuration = 0f;
        animateStriker = true;
    }

    //[PunRPC]
    public void MoveStrikerOut(Vector3 position)
    {
        ballRenderer.color = Color.white;
        base.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
        outPosition = position;
        animateStrikerOut = true;
        animateStrikerOutDuration = 0f;
        //base.photonView.RPC("EnableOrDisableCollider", RpcTarget.All, true);
    }

    //[PunRPC]
    public void MoveStrikerIn(Vector3 position)
    {
        AudioManager.getInstance().PlaySound(AudioManager.PLAY_STRIKER_DRAG);
        inPosition = position;
        animateStrikerIn = true;
        animateStrikerInDuration = 0f;
        //base.photonView.RPC("EnableOrDisableCollider", RpcTarget.All, true);
    }
}
