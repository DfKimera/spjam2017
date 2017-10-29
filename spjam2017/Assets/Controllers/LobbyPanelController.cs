using Identifiers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Controllers {
	public class LobbyPanelController : MonoBehaviour {

		private MatchController match;
		private EventSystem uiEvents;

		public GameObject player1Sprite;
		public GameObject player2Sprite;
		public GameObject player3Sprite;
		public GameObject player4Sprite;
	
		protected void Start () {
			match = GameObject.FindWithTag("GameController").GetComponent<MatchController>();
			uiEvents = GameObject.FindWithTag("UIEventSystem").GetComponent<EventSystem>(); 
		}
	
		protected void Update () {
			if (match.hasSelectedMatchType) {
				uiEvents.SetSelectedGameObject(GameObject.Find("BtnStart"));
			}
			
			player1Sprite.SetActive(true);
			player2Sprite.SetActive(true);
			player3Sprite.SetActive(match.matchType.Equals(MatchType.FourPlayers));
			player4Sprite.SetActive(match.matchType.Equals(MatchType.FourPlayers));
		}
	}
}
