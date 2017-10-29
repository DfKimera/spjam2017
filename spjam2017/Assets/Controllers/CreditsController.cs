using UnityEngine;
using UnityEngine.EventSystems;

namespace Controllers {
	public class CreditsController : MonoBehaviour {

		private MatchController match;
		private EventSystem uiEvents;
		
		protected void Start () {
			match = GameObject.FindWithTag("GameController").GetComponent<MatchController>();
			uiEvents = GameObject.FindWithTag("UIEventSystem").GetComponent<EventSystem>(); 
		}
	
		protected void Update () {
			if (match.showCredits) {
				uiEvents.SetSelectedGameObject(GameObject.Find("BtnBackToTitle"));
			}
		
		}
	}
}
