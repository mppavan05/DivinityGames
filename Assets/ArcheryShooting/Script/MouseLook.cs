using UnityEngine;

[AddComponentMenu("Camera-Control/Mouse Look")]
public class MouseLook : MonoBehaviour
{
    public enum RotationAxes
    {
        MouseXAndY = 0,
        MouseX = 1,
        MouseY = 2
    }

    public RotationAxes axes;

    public float sensitivityX = 15f;

    public float sensitivityY = 15f;

    public float minimumX = -360f;

    public float maximumX = 360f;

    public float minimumY = -60f;

    public float maximumY = 60f;

    private float rotationY;

    private Vector3 initAngle;

    private void Update()
    {
        float value = base.transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;
        value = Mathf.Clamp(value, minimumX, maximumX);
        rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
        rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);
        if (!ArcheryScroreManager.isGameOver && !ArcheryScroreManager.isPaused)
        {
            base.transform.localEulerAngles = new Vector3(0f - rotationY, value, 0f);
        }
    }

    private void Start()
    {
        if ((bool)GetComponent<Rigidbody>())
        {
            GetComponent<Rigidbody>().freezeRotation = true;
        }
        initAngle = base.transform.localEulerAngles;
        minimumX = initAngle.y - 50f;
        maximumX = initAngle.y + 50f;
        minimumY = initAngle.x - 5f;
        maximumY = initAngle.x + 20f;
    }
}
