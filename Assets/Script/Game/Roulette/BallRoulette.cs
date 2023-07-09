using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallRoulette : MonoBehaviour
{
    public GameObject center;
    public Rigidbody2D body;
    void OnEnable()
    {

        Vector3 vector = (transform.position - center.transform.position).normalized;
        body.AddForce((new Vector2(vector.y, (-1 * vector.x))) * -800);

    }

    public void UpdateBallSecond()
    {
        body.position = new Vector2(3.390648f, 1.105111f);
        body.rotation = 0;
        body.velocity = Vector2.zero;
        body.angularVelocity = 0;
        body.inertia = 0;
        body.centerOfMass = Vector2.zero;
        Vector3 vector = (transform.position - center.transform.position).normalized;
        body.AddForce((new Vector2(vector.y, (-1 * vector.x))) * -1300);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (RouletteManager.Instance.isGameRouletteStart)
        {
            Vector3 vector = (transform.position - center.transform.position).normalized;
            body.AddForce(new Vector2(body.velocity.x * -0.2f, body.velocity.y * -0.2f));
        }
    }
}
