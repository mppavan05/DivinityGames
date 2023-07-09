using UnityEngine;

public class GoaledPuck
{
	public PuckColor.Color color;

	public GameObject gameObject;

	public GoaledPuck(GameObject gameObject, PuckColor.Color suit)
	{
		this.gameObject = gameObject;
		color = suit;
	}
}
