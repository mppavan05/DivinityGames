using UnityEngine;

public class BoardEdge : MonoBehaviour
{
	public AudioSource source;

	private bool audioEnabled;

	private void Start()
	{
		audioEnabled = PlayerPrefs.GetInt("audio", 1) == 1;
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (!(collision.gameObject == null) && collision.gameObject.GetComponent<Rigidbody2D>() != null && audioEnabled)
		{
			float a = collision.relativeVelocity.magnitude / 10f;
			source.volume = Mathf.Min(a, 1f);
			source.Play();
		}
	}
}
