using Identifiers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Controllers {
	public class LobbyPanelController : MonoBehaviour {

		private MatchController match;

		public GameObject player1Sprite;
		public GameObject player2Sprite;
		public GameObject player3Sprite;
		public GameObject player4Sprite;
	
		protected void Start () {
			match = GameObject.FindWithTag("GameController").GetComponent<MatchController>(); 
		}
	
		protected void Update () {
			player1Sprite.SetActive(true);
			player2Sprite.SetActive(true);
			player3Sprite.SetActive(match.matchType == MatchType.FourPlayers);
			player4Sprite.SetActive(match.matchType == MatchType.FourPlayers);
		}
	}
}
