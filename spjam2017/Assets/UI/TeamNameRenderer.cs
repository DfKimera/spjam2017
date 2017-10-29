using Controllers;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
	public class TeamNameRenderer : MonoBehaviour {

		private MatchController match;
		private Text nameDisplay;
	
		protected void Start () {
			nameDisplay = GetComponent<Text>();
			match = GameObject.FindWithTag("GameController").GetComponent<MatchController>();
		}
	
		protected void Update () {
			nameDisplay.text = match.GetWinnerName();
		}
	}
}
