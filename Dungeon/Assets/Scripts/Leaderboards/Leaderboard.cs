using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Xml;
using System.Globalization;
using System;
using System.Collections;
using XboxCtrlrInput;
using System.Threading;

public class Leaderboard : MonoBehaviour
{

	private dreamloLeaderBoard leaderboard;
	private bool scoresRetrieved;

	[System.Serializable]
	public struct LeaderboardDetails
	{
		public string bossName;
		public string privateKey;
		public string publicKey;
	}

	public LeaderboardDetails scyllaDetails;
	public LeaderboardDetails spewnicornDetails;
	public LeaderboardDetails abraDetails;
	public LeaderboardDetails velestineDetails;
	public GameObject LeaderBoardPanel;
	public Text lastBossDefeatedOnLB;

	public GameObject onScreenKeyboard;

	private bool LBIsON = false;
	public bool scoreUploaded = false;
	private bool scoreReloaded = false;
	private CanvasGroup CG;

	dreamloLeaderBoard lb;

	private List<LeaderboardScore> scoreList = new List<LeaderboardScore> ();


	// Use this for initialization
	void Start ()
	{
		CG = LeaderBoardPanel.GetComponent<CanvasGroup> ();
		lastBossDefeatedOnLB.text = GameController.gameController.lastBossDefeated;
		lb = dreamloLeaderBoard.GetSceneDreamloLeaderboard ();
		string lastBoss = GameController.gameController.lastBossDefeated;
		leaderboard = dreamloLeaderBoard.GetSceneDreamloLeaderboard ();
        
		switch (lastBoss) {
		case "Scylla":
			lb.publicCode = scyllaDetails.publicKey;
			lb.privateCode = scyllaDetails.privateKey;
			break;
		case "Spewnicorn":
			lb.publicCode = spewnicornDetails.publicKey;
			lb.privateCode = spewnicornDetails.privateKey;
			break;
		case "Abra Cadaver":
			lb.publicCode = abraDetails.publicKey;
			lb.privateCode = abraDetails.privateKey;
			break;
		case "Velestine":
			lb.publicCode = velestineDetails.publicKey;
			lb.privateCode = velestineDetails.privateKey;
			break;

		}

		leaderboard.LoadScores ();

	}

	// Update is called once per frame
	void Update ()
	{
		//			if (LeaderBoardPanel.activeSelf)
		//				LeaderBoardPanel.SetActive(false);
		//			else 
		//				LeaderBoardPanel.SetActive(true);
	
		if (XCI.GetButtonDown (XboxButton.X)) {
			if (LBIsON == false)
				LBIsON = true;
			else
				LBIsON = false;
		}
		if (leaderboard.highScores != "" && !scoresRetrieved) {
			if (scoreUploaded && !scoreReloaded) {
				StartCoroutine ("ReloadScores");
				return;
			}

			scoresRetrieved = true;
			Debug.Log (leaderboard.highScores);
			XmlDocument leaderboardXml = new XmlDocument ();
			leaderboardXml.LoadXml (leaderboard.highScores);

			XmlNodeList scoreNodes = leaderboardXml.SelectNodes ("//entry");
			scoreList.Clear ();
			foreach (XmlNode score in scoreNodes) {
				LeaderboardScore newScore = new LeaderboardScore (score);
				scoreList.Add (newScore);
			}

			LoadLeaderboardPanel ();

			Debug.Log ("Retrieved " + scoreList.Count.ToString () + " scores");
		}
		if (scoreList.Count < 10 && !scoreUploaded) {
			Debug.Log ("AddingScore");
			Debug.Log (scoreList.Count);
			//				
			if (GameController.gameController.playerName == null || GameController.gameController.playerName == "") {
				onScreenKeyboard.SetActive (true);
				CG.alpha = 0.0f;
				GameController.gameController.playerName = onScreenKeyboard.GetComponent<OnScreenKeyboard> ().ReturnUsername ();
			}
			if (!(GameController.gameController.playerName == null) && !(GameController.gameController.playerName == "")) {
				CG.alpha = 1f;
                int timeTaken = 10000 - Mathf.FloorToInt(GameController.gameController.gameTimer);
                leaderboard.AddScore (GameController.gameController.playerName, 0, timeTaken);
				scoresRetrieved = false;
				scoreUploaded = true;
				onScreenKeyboard.SetActive (false);
			}
			//leaderboard.AddScore (GameController.gameController.playerName, 0, Mathf.FloorToInt (GameController.gameController.gameTimer));
			//scoresRetrieved = false;
			//scoreUploaded = true;
		} else {
			bool placedOnLeaderboard = false;
			foreach (LeaderboardScore score in scoreList) {
				if (Mathf.FloorToInt (GameController.gameController.gameTimer) > score.TotalTime) {
					placedOnLeaderboard = true;
				}
			}
			if (placedOnLeaderboard && !scoreUploaded) {
				Debug.Log ("asd");
				if (GameController.gameController.playerName == null || GameController.gameController.playerName == "") {
					CG.alpha = 0f;
					onScreenKeyboard.SetActive (true);
					GameController.gameController.playerName = onScreenKeyboard.GetComponent<OnScreenKeyboard> ().ReturnUsername ();
				}
				if (!(GameController.gameController.playerName == null) && !(GameController.gameController.playerName == "")) {
					CG.alpha = 1f;
                    int timeTaken = 10000 - Mathf.FloorToInt(GameController.gameController.gameTimer);
                    leaderboard.AddScore (GameController.gameController.playerName, 0, timeTaken);
					scoresRetrieved = false;
					scoreUploaded = true;
					onScreenKeyboard.SetActive (false);
				}
			}
		}
	}

	IEnumerator ReloadScores ()
	{

		leaderboard.LoadScores ();

		while (leaderboard.highScores == "") {
			yield return new WaitForEndOfFrame ();
		}



		XmlDocument leaderboardXml = new XmlDocument ();

		leaderboard.highScores = "";

		leaderboard.LoadScores ();

		while (leaderboard.highScores == "") {
			yield return new WaitForEndOfFrame ();
		}

		Debug.Log (leaderboard.highScores);
        
		leaderboardXml.LoadXml (leaderboard.highScores);
		scoresRetrieved = true;

		XmlNodeList scoreNodes = leaderboardXml.SelectNodes ("//entry");
		scoreList.Clear ();
		foreach (XmlNode score in scoreNodes) {
			LeaderboardScore newScore = new LeaderboardScore (score);
			scoreList.Add (newScore);
		}

		LoadLeaderboardPanel ();
		scoreReloaded = true;
	}

	void SetLBAlpha ()
	{
		CanvasGroup CG = LeaderBoardPanel.GetComponent<CanvasGroup> ();
		if (LBIsON == false) {
			
			CG.alpha = 0.1f;
		} else
			CG.alpha = 0.0f;
	}
	void LoadLeaderboardPanel ()
	{
        scoreList.Sort((s1, s2) => s1.TotalTime.CompareTo(s2.TotalTime));

        for (int i = 0; i< scoreList.Count; i++) {
			int lineNum = i + 1;
			GameObject leaderboardLine = GameObject.Find ("Line (" + lineNum.ToString () + ")");
			Text playerNameText = leaderboardLine.transform.FindChild ("NameText").GetComponent<Text> ();
			Text timeText = leaderboardLine.transform.FindChild ("TimeText").GetComponent<Text> ();

			LeaderboardScore currentScore = scoreList [i];
			TimeSpan time = TimeSpan.FromSeconds (currentScore.TotalTime);
			playerNameText.text = currentScore.PlayerName.Replace ("+", " ");
			timeText.text = time.ToString ();
		}
	}
    
}
