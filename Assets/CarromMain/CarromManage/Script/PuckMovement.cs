//using Photon.Pun;
using UnityEngine;

public class PuckMovement : MonoBehaviour//Pun, IPunInstantiateMagicCallback
{
    public Color disabled;

    private Vector2 position;

    private Rigidbody2D rb;

    private CircleCollider2D circleCollider;

    private SpriteRenderer spriterenderer;

    private Vector2 networkPosition;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();
        spriterenderer = GetComponent<SpriteRenderer>();
    }

    //public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    //{
    //    //if (stream.IsWriting)
    //    //{
    //    //    Debug.LogError("IsWriting " + base.gameObject.name);
    //    //    if (!(spriterenderer == null))
    //    //    {
    //    //        stream.SendNext((spriterenderer.color == Color.white) ? 1 : 0);
    //    //        stream.SendNext(circleCollider.isTrigger);
    //    //    }
    //    //    return;
    //    //}
    //    //Debug.LogError("Reading " + base.gameObject.name);
    //    //if (!(spriterenderer == null))
    //    //{
    //    //    int num = (int)stream.ReceiveNext();
    //    //    spriterenderer.color = ((num != 1) ? disabled : Color.white);
    //    //    circleCollider.isTrigger = (bool)stream.ReceiveNext();
    //    //}
    //}

    public void FixedUpdate()
    {
        //bool isMine = base.photonView.IsMine;
    }

    //public void OnPhotonInstantiate(PhotonMessageInfo info)
    //{
    //    PuckColor component = info.photonView.GetComponent<PuckColor>();
    //    if (component != null)
    //    {
    //        Puck item = new Puck(info.photonView, info.photonView.GetComponent<Rigidbody2D>(), component);
    //        CarromGameManager.Instance.pucks.Add(item);
    //    }
    //}

    public void OnEnable()
    {
        //PhotonNetwork.AddCallbackTarget(this);
    }

    public void OnDisable()
    {
        //PhotonNetwork.RemoveCallbackTarget(this);
    }

    //[PunRPC]
    public void StopPuck(Vector3 position)
    {
        rb.velocity = Vector2.zero;
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        base.transform.position = position;
        spriterenderer.color = disabled;
        circleCollider.isTrigger = true;
        circleCollider.enabled = false;
    }
}
