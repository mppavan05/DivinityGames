using UnityEngine;

public class AnimAutoDestroyer : MonoBehaviour
{
	private void Start()
	{
		float t = GetComponent<Animator>().GetCurrentAnimatorClipInfo(0).Length;
		Object.Destroy(base.gameObject, t);
	}

	private void Update()
	{
	}
}
