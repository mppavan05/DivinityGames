using System.Collections;

using UnityEngine;

public class HoleHandler : MonoBehaviour
{
    public GameObject shadowLayer;

    public Color delected;

    private AudioSource source;

    private Vector3 center;

    private void Start()
    {
        center = base.transform.GetChild(1).position;
        source = GetComponent<AudioSource>();
    }

    public IEnumerator DestroyPuck(GameObject puck)
    {
        //Debug.LogError("DestroyPuck");
        yield return new WaitForSeconds(0.5f);

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
    }

    private void HideShadowLayer()
    {
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!(collision.tag == "Player") && !(collision.tag == "Puck"))
        {
            return;
        }
        CircleCollider2D component = collision.gameObject.GetComponent<CircleCollider2D>();
        if (component.isTrigger)
        {
            return;
        }
        source.Play();
        component.isTrigger = true;
        PuckColor component2 = collision.gameObject.GetComponent<PuckColor>();

        collision.gameObject.GetComponent<SpriteRenderer>().color = delected;
        Vector2 vector2 = collision.gameObject.transform.localScale;
        vector2.x -= 0.05f;
        vector2.y -= 0.05f;
        collision.gameObject.transform.localScale = vector2;
        Rigidbody2D component6 = collision.gameObject.GetComponent<Rigidbody2D>();
        component6.velocity = center - collision.gameObject.transform.position;
        component6.velocity = Vector2.zero;
        component6.constraints = RigidbodyConstraints2D.FreezePosition;
        collision.gameObject.transform.position = center;
        component.isTrigger = true;
        component.enabled = false;

        if (collision.tag == "Puck")
        {
            //if (CarromGameManager.Instance != null)
            //{
            //    if (CarromGameManager.Instance.isPlayerTurn)
            //    {
            //        //CarromGameManager.Instance.GoaledColor(new GoaledPuck(collision.gameObject, component2.suit));
            //    }
            //}
            if (OfflineGameManager.Instance != null)
            {
                OfflineGameManager.Instance.GoaledColor(new GoaledPuck(collision.gameObject, component2.suit));
            }
            else
            {
                MasterStrikeManager.Instance.GoaledPuck(collision.gameObject);
            }
        }
        else
        {
            if (!(collision.tag == "Player"))
            {
                return;
            }
            //if (CarromGameManager.Instance != null)
            //{
            //    if (CarromGameManager.Instance.isPlayerTurn)
            //    {
            //        //CarromGameManager.Instance.GoaledColor(new GoaledPuck(collision.gameObject, PuckColor.Color.STRIKER_COLOR));
            //    }
            //}
            if (OfflineGameManager.Instance != null)
            {
                OfflineGameManager.Instance.GoaledColor(new GoaledPuck(collision.gameObject, PuckColor.Color.STRIKER_COLOR));
            }
        }
    }
}
