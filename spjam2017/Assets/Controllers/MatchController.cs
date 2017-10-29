using System.Collections.Generic;
using Identifiers;
using Structs;
using UnityEngine;

namespace Controllers {
	public class MatchController : MonoBehaviour {

		public bool hasMatchStarted = false;
		public bool hasSelectedMatchType = false;
		public bool hasGameOver = false;

		public TeamID winningTeam;
		public MatchType matchType = MatchType.TwoPlayers;

		public GameObject player1;
		public GameObject player2;
		public GameObject player3;
		public GameObject player4;

		public int scoreToVictory = 60;
		
		public Vector3 player1StartingPos = new Vector3(-2.0f, 0.3f, 0.5f);
		public Vector3 player2StartingPos = new Vector3(2.0f, 0.3f, 0.5f);
		public Vector3 player3StartingPos = new Vector3(-2.0f, 0.3f, -1.5f);
		public Vector3 player4StartingPos = new Vector3(2.0f, 0.3f, -1.5f);
	
		private Dictionary<TeamID, int> score = new Dictionary<TeamID, int>();
		private Dictionary<PlayerID, PlayerSettings> players = new Dictionary<PlayerID, PlayerSettings>();
	
		private RandomBlockSpawner spawner;
	
	
		protected void Start () {
			ResetScore();

			ResetPlayerSettings();
			ResetPlayerPositions();

			spawner = GameObject.FindGameObjectWithTag("BlockSpawner").GetComponent<RandomBlockSpawner>();
		}
	
		protected void Update () {
		
		}

		private void ResetScore() {
			score[TeamID.TeamA] = 0;
			score[TeamID.TeamB] = 0;
		}

		private void ResetPlayerSettings() {
			players[PlayerID.Amber] = new PlayerSettings(PlayerID.Amber, false, TeamID.TeamA);
			players[PlayerID.Redmayne] = new PlayerSettings(PlayerID.Redmayne, false, TeamID.TeamB);
			players[PlayerID.Pinkerton] = new PlayerSettings(PlayerID.Pinkerton, false, TeamID.TeamA);
			players[PlayerID.Dorange] = new PlayerSettings(PlayerID.Dorange, false, TeamID.TeamB);
		}

		private void ResetPlayerEntities() {
			player1.SetActive(players[PlayerID.Amber].isPlaying);
			player2.SetActive(players[PlayerID.Redmayne].isPlaying);
			player3.SetActive(players[PlayerID.Pinkerton].isPlaying);
			player4.SetActive(players[PlayerID.Dorange].isPlaying);
		}
		
		private void ResetPlayerPositions() {
			player1.transform.SetPositionAndRotation(player1StartingPos, player1.transform.rotation);
			player2.transform.SetPositionAndRotation(player2StartingPos, player2.transform.rotation);
			player3.transform.SetPositionAndRotation(player3StartingPos, player3.transform.rotation);
			player4.transform.SetPositionAndRotation(player4StartingPos, player4.transform.rotation);
		}

		public PlayerSettings GetPlayerSettings(PlayerID id) {
			return this.players[id];
		}

		public void StartMatch() {
			ResetScore();

			spawner.ClearAllBlocks();
			spawner.SpawnInitialBlocks();
			spawner.StartSpawnTimer();
		
			hasMatchStarted = true;
			hasSelectedMatchType = false;
		}

		public int GetScore(TeamID team) {
			if (!score.ContainsKey(team)) return 0;
			return score[team];
		}

		public void AwardPoints(TeamID team, int numPoints) {
			if (!score.ContainsKey(team)) return;
			
			score[team] += numPoints;
		
			CheckIfGameOver();
		}

		public void TakePoints(TeamID team, int numPoints) {
			if (!score.ContainsKey(team)) return;
			
			score[team] -= numPoints;

			if (score[team] < 0) score[team] = 0;
		}

		private void CheckIfGameOver() {
			if (score[TeamID.TeamA] >= scoreToVictory) {
				HandleGameOver(TeamID.TeamA);
			}
		
			if (score[TeamID.TeamB] >= scoreToVictory) {
				HandleGameOver(TeamID.TeamB);
			}
		}

		private void HandleGameOver(TeamID whoWon) {
			winningTeam = whoWon;
		
			hasGameOver = true;
			hasMatchStarted = false;
			hasSelectedMatchType = false;
		
			Invoke("ShowTitleScreen", 5.0f);
		}

		public string GetWinnerName() {
			switch (winningTeam) {
				case TeamID.TeamA: return "Team A";
				case TeamID.TeamB: return "Team B";
				default: return "Unknown team";
			}
		}

		private void ShowTitleScreen() {
			hasGameOver = false;
			hasMatchStarted = false;
			hasSelectedMatchType = false;
		}

		public void Select2v2() {
			hasSelectedMatchType = true;
			matchType = MatchType.FourPlayers;

			players[PlayerID.Amber].isPlaying = true;
			players[PlayerID.Redmayne].isPlaying = true;
			players[PlayerID.Pinkerton].isPlaying = true;
			players[PlayerID.Dorange].isPlaying = true;

			ResetPlayerEntities();
			ResetPlayerPositions();
		}

		public void Select1v1() {
			hasSelectedMatchType = true;
			matchType = MatchType.TwoPlayers;
		
			players[PlayerID.Amber].isPlaying = true;
			players[PlayerID.Redmayne].isPlaying = true;
			players[PlayerID.Pinkerton].isPlaying = false;
			players[PlayerID.Dorange].isPlaying = false;

			ResetPlayerEntities();
			ResetPlayerPositions();
		}
	
	}
}
