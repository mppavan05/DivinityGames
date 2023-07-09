//using ExitGames.Client.Photon;
//using Photon.Pun;
//using Photon.Realtime;
using UnityEngine;

public class StrikerMover : MonoBehaviour
{
    public Transform leftEnd;

    public Transform rightEnd;

    public Transform strikerPosition;

    private Vector3 end;

    private AudioSource source;

    private CircleCollider2D strikerCollider;

    private CircleCollider2D scollider;

    public static byte MOVE_STRIKER = 101;

    [SerializeField]
    private GameObject striker;

    public StrikerMover oppStrikeMover;


    private bool isDragging;

    private Vector3 lastMovement;

    private bool animateStriker;

    private float animateStrikerDuration;

    private float animateStrikerEndDuration = 1f;

    Vector3 beforeVector;
    private void Start()
    {
        end = base.transform.position;
        end.x = 0f;
        source = GetComponent<AudioSource>();
    }

    public void SetStriker(GameObject striker, int strikerIndex)
    {
        this.striker = striker;
        strikerCollider = striker.transform.GetChild(0).GetComponent<CircleCollider2D>();
        scollider = striker.GetComponent<CircleCollider2D>();
        //gamePhotonView = striker.GetComponent<PhotonView>();
        GetComponent<SpriteRenderer>().sprite = ((strikerIndex >= LevelManager.instance.strikers.Length) ? LevelManager.instance.strikers[0] : LevelManager.instance.strikers[strikerIndex]);
    }

    private void Update()
    {
        if (animateStrikerDuration > animateStrikerEndDuration && animateStriker)
        {
            animateStriker = false;
        }
        if (animateStriker)
        {
            animateStrikerDuration += Time.deltaTime;
            float t = animateStrikerDuration / animateStrikerEndDuration;
            base.transform.position = Vector3.Lerp(base.transform.position, end, t);
        }
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D raycastHit2D = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (raycastHit2D.collider != null)
            {
                if (raycastHit2D.collider.gameObject.name == base.gameObject.name)
                {
                    isDragging = true;
                    strikerCollider.enabled = false;
                }
                else
                {
                    isDragging = false;
                }
            }
            else
            {
                isDragging = false;
            }
        }
        if (Input.GetMouseButton(0) && isDragging)
        {
            int num;
            if (!(CarromGameManager.Instance != null))
            {
                num = (((!(OfflineGameManager.Instance != null)) ? MasterStrikeManager.Instance.canMoveStriker : OfflineGameManager.Instance.isPlayerTurn) ? 1 : 0);
            }
            else
            {
                if (!CarromGameManager.Instance.isPlayerTurn)
                {
                    goto IL_02aa;
                }
                num = (scollider.isTrigger ? 1 : 0);
            }
            if (num != 0)
            {
                Vector3 vector = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                vector.y = leftEnd.position.y;
                vector.z = 0f;
                float x = vector.x;
                float x2 = leftEnd.position.x;
                vector.x = Mathf.Clamp(x, x2, rightEnd.position.x);
                base.transform.position = vector;
                if (beforeVector != vector)
                {
                    beforeVector = vector;
                    CarromSocketManager.Instance.Strike_Slider_Send(vector.x);
                    print("Greejesh Strik Slider");
                }
                vector.y = strikerPosition.position.y;
                if (striker != null)
                {
                    striker.transform.position = vector;

                }
                if (!source.isPlaying && (vector - lastMovement).magnitude > 0f)
                {
                    source.Play();
                }
                lastMovement = vector;
            }
        }
        goto IL_02aa;
    IL_02aa:
        if (Input.GetMouseButtonUp(0) && strikerCollider != null)
        {
            isDragging = false;
            strikerCollider.enabled = true;
            if (source.isPlaying)
            {
                source.Stop();
            }
        }
    }


    public void Socket_Get_Pos(float posX)
    {
        float newPos = -posX;
        striker.transform.position = new Vector2(newPos, striker.transform.position.y);
    }


    public void AnimateStrikerHandlerToCenter()
    {
        animateStrikerDuration = 0f;
        animateStriker = true;
    }
}
