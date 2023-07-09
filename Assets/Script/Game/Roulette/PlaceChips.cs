using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PlaceChips : MonoBehaviour
{
    public GameObject placeObjPrefab;
    public string placeStr;
    public float scaleSpeed;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    StartCoroutine(GeneratePlaceCoinTxt());
        //}
    }


    public void GeneratePlaceCoinTxt()
    {
        GameObject genObj = Instantiate(placeObjPrefab, this.transform);
        genObj.transform.GetChild(0).GetComponent<Text>().text = placeStr;
        genObj.transform.position = new Vector3(genObj.transform.position.x, genObj.transform.position.y + 0.25f, 0);
        genObj.transform.DOMove(new Vector3(genObj.transform.position.x, genObj.transform.position.y - 0.25f, 0), 0.5f).OnComplete(() =>
            genObj.transform.DOScale(Vector3.zero, scaleSpeed).OnComplete(() =>
            {
                Destroy(genObj);
            }
            )
            );
    }
}
