using UnityEngine;
using UnityEngine.EventSystems;

namespace Controllers {
	public class UIController : MonoBehaviour {

		private MatchController match;
		private EventSystem uiEvents;

		public GameObject gameHUD;
		public GameObject matchTypePanel;
		public GameObject lobbyPanel;
		public GameObject gameOverPanel;
		public GameObject creditsPanel;
		
		public AudioClip sfxMenuMove;
		public AudioClip sfxMenuSelect;
	
		protected void Start () {
			match = GameObject.FindWithTag("GameController").GetComponent<MatchController>(); 		
			//uiEvents = GameObject.FindWithTag("UIEventSystem").GetComponent<EventSystem>(); 		
		}
	
		protected void Update () {
			CheckPanelVisibility();
		}

		public void PlayMenuMoveSFX() {
			//AudioSource.PlayClipAtPoint(sfxMenuMove, Camera.main.transform.position);
		}

		public void PlayMenuSelectSFX() {
			//AudioSource.PlayClipAtPoint(sfxMenuSelect, Camera.main.transform.position);
		}

		private void CheckPanelVisibility() {
			gameHUD.SetActive(match.hasMatchStarted || match.hasGameOver);
			matchTypePanel.SetActive(!match.hasMatchStarted && !match.hasGameOver && !match.showCredits && !match.hasSelectedMatchType);
			lobbyPanel.SetActive(!match.hasMatchStarted && !match.hasGameOver && !match.showCredits && match.hasSelectedMatchType);
			gameOverPanel.SetActive(match.hasGameOver);
			creditsPanel.SetActive(match.showCredits);
		}
	}
}
