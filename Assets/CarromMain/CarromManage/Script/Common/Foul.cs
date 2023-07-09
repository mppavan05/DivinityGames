public class Foul
{
	public int playerCoinsCount;

	public int opponentCoinsCount;

	public bool isFoul;

	public string message;

	public Foul(int playerCoinsCount, int opponentCoinsCount, bool isFoul, string message)
	{
		this.playerCoinsCount = playerCoinsCount;
		this.opponentCoinsCount = opponentCoinsCount;
		this.isFoul = isFoul;
		this.message = message;
	}
}
