using UnityEngine;

public class MusicObj : MonoBehaviour
{
    public static MusicObj Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    private void Start()
    {
    }

    private void Update()
    {
        if (DataManager.Instance.GetSound() == 0)
        {
            GetComponent<AudioSource>().mute = true;
        }
        else
        {
            GetComponent<AudioSource>().mute = false;
        }
    }
}
