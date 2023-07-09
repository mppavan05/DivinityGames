using UnityEngine;

public class MasterStrikeStrikerAnimator : OfflineStrikerAnimator
{
	public MasterStrikeManager manager;

	private void Update()
	{
		StrikerStoppedMoving();
		UpdateStrikerPosition();
		if (isMoving && AllStoppedMoving())
		{
			isMoving = false;
			if (!circlecollider.enabled)
			{
				Debug.Log("Ball stopped inside hole");
				Invoke("MoveBackStrikerFromHole", 1f);
			}
			else
			{
				Debug.Log("Ball stopped");
				circlecollider.isTrigger = true;
				MoveBackStriker();
			}
			manager.SetPlayerReward();
			manager.FinishedChance();
			manager.IsMasterStrikeOver();
		}
	}

	private void MoveBackStrikerFromHole()
	{
		ResetStrikerColor();
		MoveBackStriker();
	}

	protected bool AllStoppedMoving()
	{
		if (!manager.AllPucksStopped())
		{
			return false;
		}
		if (ballRB.velocity.magnitude > 0f)
		{
			return false;
		}
		return true;
	}
}
