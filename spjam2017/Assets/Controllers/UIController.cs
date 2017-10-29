using UnityEngine;

namespace Controllers {
	public class UIController : MonoBehaviour {

		private MatchController match;

		public GameObject gameHUD;
		public GameObject matchTypePanel;
		public GameObject lobbyPanel;
		public GameObject gameOverPanel;
	
		protected void Start () {
			match = GameObject.FindWithTag("GameController").GetComponent<MatchController>(); 		
		}
	
		protected void Update () {
			CheckPanelVisibility();
		}

		private void CheckPanelVisibility() {
			gameHUD.SetActive(match.hasMatchStarted);
			matchTypePanel.SetActive(!match.hasMatchStarted && !match.hasGameOver && !match.hasSelectedMatchType);
			lobbyPanel.SetActive(!match.hasMatchStarted && !match.hasGameOver && match.hasSelectedMatchType);
			gameOverPanel.SetActive(match.hasGameOver);
		
		}
	}
}
