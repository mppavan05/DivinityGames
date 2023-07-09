//using Photon.Pun;
using UnityEngine;

public class PuckAnimator : MonoBehaviour//Pun
{
    private Vector3 end;

    private Vector3 queenend;

    private CircleCollider2D collider2d;

    private AudioSource source;

    private Vector3 defaultScale;

    private SpriteRenderer spriterender;

    private bool audioEnabled;

    private bool animateStriker;

    private float animateStrikerDuration;

    private float animateStrikerEndDuration = 1f;

    private bool animatePuck;

    private float animatePuckDuration;

    private float animatePuckEndDuration = 1f;

    private bool animateQueenPuck;

    private float animateQueenPuckDuration;

    private float animateQueenPuckEndDuration = 1f;

    private void Start()
    {
        collider2d = GetComponent<CircleCollider2D>();
        source = GetComponent<AudioSource>();
        spriterender = GetComponent<SpriteRenderer>();
        audioEnabled = PlayerPrefs.GetInt("audio", 1) == 1;
    }

    private void Update()
    {
        if (animateStrikerDuration > animateStrikerEndDuration && animateStriker)
        {
            collider2d.isTrigger = false;
            collider2d.enabled = true;
            animateStriker = false;
        }
        if (animatePuckDuration > animatePuckEndDuration && animatePuck)
        {
            animatePuck = false;
            Object.Destroy(base.gameObject);
        }
        if (animateQueenPuckDuration > animateQueenPuckEndDuration && animateQueenPuck)
        {
            animateQueenPuck = false;
        }
        if (animateStriker)
        {
            animateStrikerDuration += Time.deltaTime;
            float t = animateStrikerDuration / animateStrikerEndDuration;
            base.transform.position = Vector3.Lerp(base.transform.position, end, t);
        }
        if (animatePuck)
        {
            animatePuckDuration += Time.deltaTime;
            float t2 = animatePuckDuration / animatePuckEndDuration;
            base.transform.position = Vector3.Lerp(base.transform.position, end, t2);
        }
        if (animateQueenPuck)
        {
            animateQueenPuckDuration += Time.deltaTime;
            float t3 = animateQueenPuckDuration / animateQueenPuckEndDuration;
            base.transform.position = Vector3.Lerp(base.transform.position, queenend, t3);
        }
    }

    //[PunRPC]
    public void AnimatePuck(Vector3 end)
    {
        if (AudioManager.getInstance() != null)
        {
            AudioManager.getInstance().PlaySound(AudioManager.PLAY_STRIKER_DRAG);
        }
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        base.transform.localScale = new Vector3(0.45f, 0.45f, 0.45f);
        GetComponent<SpriteRenderer>().color = Color.white;
        this.end = end;
        end.z = 0f;
        animateStrikerDuration = 0f;
        animateStriker = true;
    }

    public void AnimatePuckToScore(Vector3 end)
    {
        base.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
        GetComponent<SpriteRenderer>().color = Color.white;
        this.end = end;
        end.z = 0f;
        animatePuckDuration = 0f;
        animatePuck = true;
    }

    public void AnimateQueen(Vector3 end)
    {
        queenend = end;
        queenend.z = 0f;
        animateQueenPuckDuration = 0f;
        animateQueenPuck = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!(collision.gameObject == null) && collision.gameObject.GetComponent<Rigidbody2D>() != null && source != null && audioEnabled)
        {
            float a = collision.relativeVelocity.magnitude / 10f;
            source.volume = Mathf.Min(a, 1f);
            source.Play();
        }
    }
}
