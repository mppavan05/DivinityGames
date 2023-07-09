using UnityEngine;

public class MaskScriptChildren : MonoBehaviour
{
	public GameObject player;

	private void Start()
	{
		player = GameObject.FindWithTag("Player");
	}

	private void OnTriggerStay2D(Collider2D other)
	{
		if (!other.GetComponent<Collider2D>().isTrigger && other.gameObject.tag != "Player")
		{
			player.GetComponent<trajectoryScript>().collided(base.gameObject);
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (!other.GetComponent<Collider2D>().isTrigger)
		{
			player.GetComponent<trajectoryScript>().uncollided(base.gameObject);
		}
	}
}
