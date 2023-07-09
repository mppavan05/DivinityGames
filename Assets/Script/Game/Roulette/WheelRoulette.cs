using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelRoulette : MonoBehaviour
{
    public float speed = 1.1f;
    private float position = 0.0f;
    private float initradius = 4.75f;
    private float minradius = 236.54f;// = 3.28f;
    public CircleCollider2D collider;



    public void UpdateWheel()
    {
        position = 0;
        transform.rotation = Quaternion.identity;
    }

    //168.5
    //236.54
    // Update is called once per frame
    void FixedUpdate()
    {
        if (RouletteManager.Instance.isGameRouletteStart == true)
        {
            if (RouletteManager.Instance.isRoundOn == false)
            {
                speed *= 0.995f;
                if (speed < 7)
                {
                    if (collider.radius > minradius)
                        collider.radius *= 0.995f;
                    else
                        collider.radius = minradius;
                }

                if (speed < 2f)
                {
                    RouletteManager.Instance.ObjectAvaliable();
                }
                position -= speed;
                position = position % 360;
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, position), 0.5f);

            }
        }
    }
}
