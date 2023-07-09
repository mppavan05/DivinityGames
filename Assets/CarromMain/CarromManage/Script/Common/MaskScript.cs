using UnityEngine;

public class MaskScript : MonoBehaviour
{
	public static void collided(GameObject dot)
	{
		for (int i = 0; i < 40; i++)
		{
			if (dot.name == "Dot (" + i + ")")
			{
				Debug.Log(string.Empty + dot.name);
			}
		}
	}
}
