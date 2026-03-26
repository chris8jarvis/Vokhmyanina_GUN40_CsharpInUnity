namespace Controllers
{
	public sealed class GameStateSystem
	{
		public Player currentPlayer = Player.White;
		public GameState currentState = GameState.SelectUnit;
	}
}
