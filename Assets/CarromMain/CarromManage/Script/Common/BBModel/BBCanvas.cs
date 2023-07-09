using UnityEngine;
using UnityEngine.UI;

namespace BBModel
{
	public class BBCanvas : MonoBehaviour
	{
		private Animator animator;

		private CanvasScaler scaller;

		[SerializeField]
		private string trigger;

		private void Awake()
		{
			animator = GetComponent<Animator>();
			scaller = GetComponent<CanvasScaler>();
			if ((bool)animator)
			{
				animator.enabled = false;
				if (0 == 1)
				{
					scaller.matchWidthOrHeight = 1f;
				}
				if (0 == 2)
				{
					animator.SetTrigger(trigger + "Tall");
					return;
				}
				int num = Screen.height / Screen.width;
				int num2 = 2;
			}
		}
	}
}
