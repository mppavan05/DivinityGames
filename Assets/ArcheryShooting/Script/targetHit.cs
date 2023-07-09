using System.Collections;
using UnityEngine;

public class targetHit : MonoBehaviour
{

    public int scorePoint;
    private void Start()
    {
    }

    private void Update()
    {
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name == "Arrow")
        {
            arrowScript arrowScript1 = other.gameObject.GetComponent<arrowScript>();
            arrowScript1.ScoreIncrement(scorePoint);

            print("Score Point : " + scorePoint);
            //shooot.Instance.arrowObjList[shooot.Instance.arrowObjList.Count - 1].GetComponent<BoxCollider>().isTrigger = true;
            // StartCoroutine(waitAndGo(0.1f, other.gameObject));
        }
    }

    private IEnumerator waitAndGo(float waitTime, GameObject other)
    {
        yield return new WaitForSeconds(waitTime);
        if (GetComponent<Rigidbody>() == null)
        {
            Object.Destroy(other.gameObject.GetComponent<Rigidbody>());
            base.transform.tag = "Ground";
            if (base.gameObject.layer != 9)
            {
                GetComponent<MeshCollider>().convex = true;
                base.gameObject.AddComponent<Rigidbody>();
                GetComponent<Rigidbody>().AddForce(new Vector3(1f, 0.5f, 0f) * 2f, ForceMode.Impulse);
            }
            //if (GetComponent<MoverRotator>() != null)
            //{
            //	Object.Destroy(GetComponent<MoverRotator>());
            //}
            if (GetComponent<Animator>() != null)
            {
                GetComponent<Animator>().enabled = false;
            }
            if (base.gameObject.layer != 9)
            {
                DestroySelf();
            }
        }
    }

    public void DestroySelf()
    {
        Object.Destroy(base.gameObject, 2f);
    }
}
