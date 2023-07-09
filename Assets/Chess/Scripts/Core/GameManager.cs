﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Chess.Game {
	public class GameManager : MonoBehaviour {

		public enum Result { Playing, WhiteIsMated, BlackIsMated, Stalemate, Repetition, FiftyMoveRule, InsufficientMaterial }

		public event System.Action onPositionLoaded;
		public event System.Action<Move> onMoveMade;

		public enum PlayerType { Human, AI }

		public bool loadCustomPosition;
		public string customPosition = "1rbq1r1k/2pp2pp/p1n3p1/2b1p3/R3P3/1BP2N2/1P3PPP/1NBQ1RK1 w - - 0 1";

		public PlayerType whitePlayerType;
		public PlayerType blackPlayerType;
		public AISettings aiSettings;
		public Color[] colors;

		public bool useClocks;
		public Clock whiteClock;
		public Clock blackClock;
		public TMPro.TMP_Text aiDiagnosticsUI;
		public TMPro.TMP_Text resultUI;
		
		public int playerScoreCnt1;
		public int playerScoreCnt2;
		
		public Text playerNameTxt1;
		public Text playerNameTxt2;

		public Image playerProfile1;
		public Image playerProfile2;

		public Text playerScoreTxt1;
		public Text playerScoreTxt2;
		
		public GameObject soundOn;
		public GameObject soundOff;
		public GameObject vibOn;
		public GameObject vibOff;
		public GameObject sfxOn;
		public GameObject sfxOff;
		public GameObject menuScreenObj;
		public GameObject settingsScreenObj;
		public GameObject pauseScreen;

		Result gameResult;

		Player whitePlayer;
		Player blackPlayer;
		Player playerToMove;
		List<Move> gameMoves;
		BoardUI boardUI;

		public ulong zobristDebug;
		public Board board { get; private set; }
		Board searchBoard; // Duplicate version of board used for ai search

		void Start () {
			//Application.targetFrameRate = 60;

			if (useClocks) {
				whiteClock.isTurnToMove = false;
				blackClock.isTurnToMove = false;
			}

			boardUI = FindObjectOfType<BoardUI> ();
			gameMoves = new List<Move> ();
			board = new Board ();
			searchBoard = new Board ();
			aiSettings.diagnostics = new Search.SearchDiagnostics ();

			NewGame (whitePlayerType, blackPlayerType);
			PlayerNameManage();
			ManageSoundButtons();

		}

		void Update () {
			zobristDebug = board.ZobristKey;

			if (gameResult == Result.Playing) {
				LogAIDiagnostics ();

				playerToMove.Update ();

				if (useClocks) {
					whiteClock.isTurnToMove = board.WhiteToMove;
					blackClock.isTurnToMove = !board.WhiteToMove;
				}
			}

			if (Input.GetKeyDown (KeyCode.E)) {
				ExportGame ();
			}

		}

		void OnMoveChosen (Move move) {
			bool animateMove = playerToMove is AIPlayer;
			board.MakeMove (move);
			searchBoard.MakeMove (move);

			print("---------------------------------" + move.Name + "-------------------------------------");
			gameMoves.Add (move);
			onMoveMade?.Invoke (move);
			boardUI.OnMoveMade (board, move, animateMove);

			NotifyPlayerToMove ();
		}

		public void NewGame (bool humanPlaysWhite) {
			boardUI.SetPerspective (humanPlaysWhite);
			NewGame ((humanPlaysWhite) ? PlayerType.Human : PlayerType.AI, (humanPlaysWhite) ? PlayerType.AI : PlayerType.Human);
		}

		public void NewComputerVersusComputerGame () {
			boardUI.SetPerspective (true);
			NewGame (PlayerType.AI, PlayerType.AI);
		}

		void NewGame (PlayerType whitePlayerType, PlayerType blackPlayerType) {
			gameMoves.Clear ();
			if (loadCustomPosition) {
				board.LoadPosition (customPosition);
				searchBoard.LoadPosition (customPosition);
			} else {
				board.LoadStartPosition ();
				searchBoard.LoadStartPosition ();
			}
			onPositionLoaded?.Invoke ();
			boardUI.UpdatePosition (board);
			boardUI.ResetSquareColours ();

			CreatePlayer (ref whitePlayer, whitePlayerType);
			CreatePlayer (ref blackPlayer, blackPlayerType);

			gameResult = Result.Playing;
			PrintGameResult (gameResult);

			NotifyPlayerToMove ();

		}

		void LogAIDiagnostics () {
			string text = "";
			var d = aiSettings.diagnostics;
			//text += "AI Diagnostics";
			text += $"<color=#{ColorUtility.ToHtmlStringRGB(colors[3])}>Version 1.0\n";
			text += $"<color=#{ColorUtility.ToHtmlStringRGB(colors[0])}>Depth Searched: {d.lastCompletedDepth}";
			//text += $"\nPositions evaluated: {d.numPositionsEvaluated}";

			string evalString = "";
			if (d.isBook) {
				evalString = "Book";
			} else {
				float displayEval = d.eval / 100f;
				if (playerToMove is AIPlayer && !board.WhiteToMove) {
					displayEval = -displayEval;
				}
				evalString = ($"{displayEval:00.00}").Replace (",", ".");
				if (Search.IsMateScore (d.eval)) {
					evalString = $"mate in {Search.NumPlyToMateFromScore(d.eval)} ply";
				}
			}
			text += $"\n<color=#{ColorUtility.ToHtmlStringRGB(colors[1])}>Eval: {evalString}";
			text += $"\n<color=#{ColorUtility.ToHtmlStringRGB(colors[2])}>Move: {d.moveVal}";

			aiDiagnosticsUI.text = text;
		}

		public void ExportGame () {
			string pgn = PGNCreator.CreatePGN (gameMoves.ToArray ());
			string baseUrl = "https://www.lichess.org/paste?pgn=";
			string escapedPGN = UnityEngine.Networking.UnityWebRequest.EscapeURL (pgn);
			string url = baseUrl + escapedPGN;

			Application.OpenURL (url);
			TextEditor t = new TextEditor ();
			t.text = pgn;
			t.SelectAll ();
			t.Copy ();
		}

		public void QuitGame () {
			Application.Quit ();
		}

		void NotifyPlayerToMove () {
			gameResult = GetGameState ();
			PrintGameResult (gameResult);

			if (gameResult == Result.Playing) {
				playerToMove = (board.WhiteToMove) ? whitePlayer : blackPlayer;
				playerToMove.NotifyTurnToMove ();

			} else {
				Debug.Log ("Game Over");
			}
		}

		void PrintGameResult (Result result) {
			float subtitleSize = resultUI.fontSize * 0.75f;
			string subtitleSettings = $"<color=#787878> <size={subtitleSize}>";

			if (result == Result.Playing) {
				resultUI.text = "";
			} else if (result == Result.WhiteIsMated || result == Result.BlackIsMated) {
				resultUI.text = "Checkmate!";
			} else if (result == Result.FiftyMoveRule) {
				resultUI.text = "Draw";
				resultUI.text += subtitleSettings + "\n(50 move rule)";
			} else if (result == Result.Repetition) {
				resultUI.text = "Draw";
				resultUI.text += subtitleSettings + "\n(3-fold repetition)";
			} else if (result == Result.Stalemate) {
				resultUI.text = "Draw";
				resultUI.text += subtitleSettings + "\n(Stalemate)";
			} else if (result == Result.InsufficientMaterial) {
				resultUI.text = "Draw";
				resultUI.text += subtitleSettings + "\n(Insufficient material)";
			}
		}

		Result GetGameState () {
			MoveGenerator moveGenerator = new MoveGenerator ();
			var moves = moveGenerator.GenerateMoves (board);

			// Look for mate/stalemate
			if (moves.Count == 0) {
				if (moveGenerator.InCheck ()) {
					return (board.WhiteToMove) ? Result.WhiteIsMated : Result.BlackIsMated;
				}
				return Result.Stalemate;
			}

			// Fifty move rule
			if (board.fiftyMoveCounter >= 100) {
				return Result.FiftyMoveRule;
			}

			// Threefold repetition
			int repCount = board.RepetitionPositionHistory.Count ((x => x == board.ZobristKey));
			if (repCount == 3) {
				return Result.Repetition;
			}

			// Look for insufficient material (not all cases implemented yet)
			int numPawns = board.pawns[Board.WhiteIndex].Count + board.pawns[Board.BlackIndex].Count;
			int numRooks = board.rooks[Board.WhiteIndex].Count + board.rooks[Board.BlackIndex].Count;
			int numQueens = board.queens[Board.WhiteIndex].Count + board.queens[Board.BlackIndex].Count;
			int numKnights = board.knights[Board.WhiteIndex].Count + board.knights[Board.BlackIndex].Count;
			int numBishops = board.bishops[Board.WhiteIndex].Count + board.bishops[Board.BlackIndex].Count;

			if (numPawns + numRooks + numQueens == 0) {
				if (numKnights == 1 || numBishops == 1) {
					return Result.InsufficientMaterial;
				}
			}

			return Result.Playing;
		}

		void CreatePlayer (ref Player player, PlayerType playerType) {
			if (player != null) {
				player.onMoveChosen -= OnMoveChosen;
			}

			if (playerType == PlayerType.Human) {
				player = new HumanPlayer (board);
			} else {
				player = new AIPlayer (searchBoard, aiSettings);
			}
			player.onMoveChosen += OnMoveChosen;
		}

		#region Button Functions

		public void SettingsButtonClick()
		{
			SoundManager.Instance.ButtonClick();
			OpenSettingsScreen();
		}

		public void SettingsCloseButtonClick()
		{
			SoundManager.Instance.ButtonClick();
			CloseSettingsScreen();
		}

		public void MenuButtonClick()
		{
			SoundManager.Instance.ButtonClick();
			OpenMenuScreen();
		}

		public void MenuCloseButtonClick()
		{
			SoundManager.Instance.ButtonClick();
			CloseMenuScreen();
		}

		private void OpenSettingsScreen()
		{
			settingsScreenObj.SetActive(true);
			pauseScreen.SetActive(true);
		}

		private void CloseSettingsScreen()
		{
			settingsScreenObj.SetActive(false);
			pauseScreen.SetActive(false);
		}

		private void OpenMenuScreen()
		{
			menuScreenObj.SetActive(true);
			pauseScreen.SetActive(true);
		}

		private void CloseMenuScreen()
		{
			menuScreenObj.SetActive(false);
			pauseScreen.SetActive(false);
		}


		public void SoundButtonClick()
		{
			SoundManager.Instance.ButtonClick();
			if (soundOn.activeSelf)
			{
				DataManager.Instance.SetSound(1);
				SoundManager.Instance.StopBackgroundMusic();
				soundOn.SetActive(false);
				soundOff.SetActive(true);
			}
			else
			{
				DataManager.Instance.SetSound(0);
				soundOff.SetActive(false);
				soundOn.SetActive(true);
				SoundManager.Instance.StartBackgroundMusic();
			}
		}

		public void VibrationButtonClick()
		{
			SoundManager.Instance.ButtonClick();
			if (vibOn.activeSelf)
			{
				DataManager.Instance.SetVibration(1);
				//SoundManager.Instance.StopBackgroundMusic();
				vibOn.SetActive(false);
				vibOff.SetActive(true);
			}
			else
			{
				DataManager.Instance.SetVibration(0);
				vibOff.SetActive(false);
				vibOn.SetActive(true);
				//SoundManager.Instance.StartBackgroundMusic();
			}
		}

		public void SfxButtonClick()
		{
			SoundManager.Instance.ButtonClick();
			if (sfxOn.activeSelf)
			{
				sfxOn.SetActive(false);
				sfxOff.SetActive(true);
			}
			else
			{
				sfxOn.SetActive(true);
				sfxOff.SetActive(false);
			}
		}

		public void MenuSubButtonClick(int no)
		{
			SoundManager.Instance.ButtonClick();
			if (no == 1)
			{
				Time.timeScale = 1;
				TestSocketIO.Instace.LeaveRoom();
				SoundManager.Instance.StartBackgroundMusic();
				SceneManager.LoadScene("Main");
			}
			else if (no == 2)
			{
				//OpenRuleScreen();
			}
			else if (no == 3)
			{
				//Shop
				//Instantiate(shopPrefab, shopPrefabParent.transform);
			}
		}


		private void ManageSoundButtons()
		{
			if (DataManager.Instance.GetSound() == 0)
			{
				soundOn.SetActive(true);
				soundOff.SetActive(false);
			}
			else
			{
				soundOn.SetActive(false);
				soundOff.SetActive(true);
			}

			if (DataManager.Instance.GetVibration() == 0)
			{
				vibOn.SetActive(true);
				vibOff.SetActive(false);
			}
			else
			{
				vibOn.SetActive(false);
				vibOff.SetActive(true);
			}
		}

		#endregion

		#region ProfileData

		
		public void PlayerNameManage()
		{
			if (DataManager.Instance.isTwoPlayer == true)
			{
				int index1 = 0;
				int index2 = 1;
				if (DataManager.Instance.playerNo == 2)
				{
					index1 = 1;
					index2 = 0;
				}
				playerNameTxt1.text = UserNameStringManage(DataManager.Instance.joinPlayerDatas[index1].userName);
				playerNameTxt2.text = UserNameStringManage(DataManager.Instance.joinPlayerDatas[index2].userName);


				//playerProfile1.sprite = profileAvatar[DataManager.Instance.joinPlayerDatas[index1].avtar];
				//playerProfile2.sprite = profileAvatar[DataManager.Instance.joinPlayerDatas[index2].avtar];

				StartCoroutine(GetImages(DataManager.Instance.joinPlayerDatas[index1].avtar, playerProfile1));
				StartCoroutine(GetImages(DataManager.Instance.joinPlayerDatas[index2].avtar, playerProfile2));

			}
			
			playerScoreTxt1.text = playerScoreCnt1.ToString();
			playerScoreTxt2.text = playerScoreCnt2.ToString();
		}
		
		IEnumerator GetImages(string URl, Image image)
		{
			UnityWebRequest request = UnityWebRequestTexture.GetTexture(URl);
			yield return request.SendWebRequest();

			if (request.error == null)
			{
				if (image != null)
				{
					var texture = DownloadHandlerTexture.GetContent(request);
					Rect rect = new Rect(0, 0, texture.width, texture.height);
					image.sprite = Sprite.Create(texture, rect, new Vector2(0, 0));
					image.color = new Color(255, 255, 255, 255);
				}
			}
		}
		public string UserNameStringManage(string name)
		{
			if (name != null && name != "")
			{
				if (name.Length > 13)
				{
					name = name.Substring(0, 10) + "...";
				}
				else
				{
					name = name;
				}
			}
			return name;
		}


		#endregion
	}
}