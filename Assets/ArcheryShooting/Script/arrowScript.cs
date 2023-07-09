using System.Collections;
using UnityEngine;

public class arrowScript : MonoBehaviour
{
    public float velo;

    public Vector3 InitialVelo;

    private float windSpeed;

    public bool isStuck;

    public bool isCollisionOccured;

    public AudioClip swooshArrow;

    public AudioClip arrowHit;

    public AudioClip fruitHit;

    public AudioClip humanHit;

    private Camera arrowCam;

    private Camera mainCam;

    public GameObject gameManager;

    public GameObject HitPart;

    public GameObject BloodAnim;

    public bool isTargetHit;
    private void Start()
    {
        isCollisionOccured = false;

        BoxCollider col = GetComponent<BoxCollider>();
        for (int i = 0; i < shooot.Instance.arrowObjList.Count; i++)
        {
            BoxCollider otherCol = shooot.Instance.arrowObjList[i].GetComponent<BoxCollider>();
            Physics.IgnoreCollision(otherCol, col);
        }
    }

    public void shootArrow(Vector3 eulerangles)
    {
        if (DataManager.Instance.GetSound() == 0)
        {
            GetComponent<AudioSource>().PlayOneShot(swooshArrow);
        }
        isStuck = false;
        GetComponent<Rigidbody>().AddForce(Quaternion.Euler(new Vector3(0f, eulerangles.y - 90f, 360f - eulerangles.x)) * InitialVelo, ForceMode.Impulse);
    }

    public void setCam(Camera cam, bool active)
    {
        arrowCam = cam;
        arrowCam.enabled = active;
    }

    public void ScoreIncrement(int scorePoint)
    {
        if (!isTargetHit)
        {
            isTargetHit = true;
            ArcheryScroreManager.Instance.CurrentScoreManage(scorePoint);
        }

    }

    public void setMainCam(Camera cam)
    {
        mainCam = cam;
    }

    public void setGameManager(GameObject gm)
    {
        gameManager = gm;
    }

    public void setWindSpeed(float wind)
    {
        windSpeed = wind;
    }

    private void Update()
    {
        if (isStuck)
        {
            if (GetComponent<Rigidbody>() != null && !GetComponent<Rigidbody>().isKinematic)
            {
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            }
            return;
        }
        if (GetComponent<Rigidbody>() != null && GetComponent<Rigidbody>().velocity != Vector3.zero)
        {
            Vector3 velocity = GetComponent<Rigidbody>().velocity;
            float z = Mathf.Atan2(velocity.y, velocity.x) * 57.29578f;
            float num = Mathf.Atan2(velocity.z, velocity.x) * 57.29578f;
            //base.transform.eulerAngles = new Vector3(0f, 0f - num, z);//Greejesh Off
        }
        if (arrowCam != null)
        {
            //arrowCam.transform.rotation = base.transform.rotation * Quaternion.Euler(new Vector3(10f, 90f, 0f));//Greejesh Off


            //arrowCam.transform.position = base.transform.position + new Vector3(-2.5f, 1.3f, 0f);
            //arrowCam.transform.position = base.transform.position + new Vector3(-2.5f, 1.3f, 0f);

            //float rotOff = 1.3f;
            float rotOff = 1f;

            Vector3 oldPos = base.transform.position + new Vector3(0, rotOff, 0f);
            arrowCam.transform.position = Vector3.Lerp(oldPos, base.transform.position + new Vector3(-2.5f, rotOff, 0f), Time.deltaTime * 100f);
        }
    }

    private void OnCollisionEnter(Collision otherObject)
    {
        if (isCollisionOccured)
        {
            return;
        }
        //shooot.GameStates gameState = gameManager.GetComponent<shooot>().gameState;
        //if (gameState != shooot.GameStates.game)
        //{
        //    return;
        //}
        if (ArcheryScroreManager.isGameOver || ArcheryScroreManager.isPaused)
        {
            return;
        }
        if (otherObject.transform.tag == "Target")
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            isStuck = true;
            isCollisionOccured = true;
            shooot.hittedTargets++;
            shooot.AvlblMoves--;
            GameObject gameObject = Object.Instantiate(HitPart, otherObject.contacts[0].point, Quaternion.identity);
            StartCoroutine("waitThree");
            if (otherObject.gameObject.layer == 9)
            {
                otherObject.gameObject.GetComponent<MeshRenderer>().enabled = false;
                otherObject.gameObject.GetComponent<Collider>().enabled = false;
                base.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
                otherObject.transform.GetChild(0).gameObject.SetActive(false);
                if (DataManager.Instance.GetSound() == 0)
                {
                    GetComponent<AudioSource>().PlayOneShot(fruitHit);
                }
            }
            else
            {
                base.transform.parent = otherObject.transform;
                if (DataManager.Instance.GetSound() == 0)
                {
                    GetComponent<AudioSource>().PlayOneShot(arrowHit);
                }
            }
        }
        if (otherObject.transform.name == "Terrain" || otherObject.transform.tag == "Ground")
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().isKinematic = true;
            base.transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
            isStuck = true;
            shooot.AvlblMoves--;
            base.transform.parent = otherObject.transform;
            isCollisionOccured = true;
            StartCoroutine("waitThree");
            if (otherObject.gameObject.layer == 9)
            {
                if (otherObject.transform.root.GetChild(0).GetComponent<Animator>() != null)
                {
                    otherObject.transform.root.GetChild(0).GetComponent<Animator>().enabled = false;
                }
                if (otherObject.transform.root.GetComponent<Animator>() != null)
                {
                    otherObject.transform.root.GetComponent<Animator>().SetTrigger("Falling");
                }
                if (DataManager.Instance.GetSound() == 0)
                {
                    GetComponent<AudioSource>().PlayOneShot(humanHit);
                }
                GameObject gameObject2 = Object.Instantiate(BloodAnim, otherObject.contacts[0].point, Quaternion.Euler(new Vector3(0f, 90f, 0f)));
                gameObject2.transform.parent = otherObject.transform;
                ArcheryScroreManager.isHumanHit = true;
                //if (otherObject.transform.root.GetComponent<MoverRotator>() != null)
                //{
                //    otherObject.transform.root.GetComponent<MoverRotator>().enabled = false;
                //}
            }
        }
        if (otherObject.transform.name == "cart")
        {
            GetComponent<AudioSource>().PlayOneShot(arrowHit);
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            base.transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
            isCollisionOccured = true;
            StartCoroutine("waitThree");
        }
    }

    private IEnumerator setLevel(int level)
    {
        yield return new WaitForSeconds(1f);
        mainCam.fieldOfView = 60f;
        mainCam.transform.localEulerAngles = new Vector3(0f, 90f, 0f);
        mainCam.enabled = true;
        arrowCam.enabled = false;
        gameManager.GetComponent<shooot>().Shooting(false);
        gameManager.GetComponent<shooot>().setLevel(level);
        StopCoroutine("setLevel");
    }

    private IEnumerator chooseAgain()
    {
        yield return new WaitForSeconds(3f);
        gameManager.GetComponent<shooot>().Shooting(false);
        StopCoroutine("chooseAgain");
    }

    private IEnumerator waitThree()
    {
        yield return new WaitForSeconds(2f);
        mainCam.fieldOfView = 60f;
        mainCam.transform.localEulerAngles = new Vector3(0f, 90f, 0f);
        mainCam.enabled = true;
        arrowCam.enabled = false;
        gameManager.GetComponent<shooot>().Shooting(false);
        StopCoroutine("waitThree");
    }
}
