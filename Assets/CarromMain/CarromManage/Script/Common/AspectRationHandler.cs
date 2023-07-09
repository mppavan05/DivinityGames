using UnityEngine;

public class AspectRationHandler : MonoBehaviour
{
    private float width;

    private void Start()
    {
        Debug.Log("Aspect:" + Camera.main.aspect);
        if ((double)Camera.main.aspect < 0.5)
        {
            Camera.main.orthographicSize = 6f;
        }
        else if ((double)Camera.main.aspect > 0.5)
        {
            Camera.main.orthographicSize = 5f;
        }
        else
        {
            Camera.main.orthographicSize = 5.6f;
        }
    }

    private void Update()
    {
    }
}
