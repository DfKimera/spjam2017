using UnityEngine;
using UnityEngine.EventSystems;

namespace Controllers {
	public class TitleScreenController : MonoBehaviour {

		private EventSystem uiEvents;
		public bool hasPreselectedButton = false;

		protected void Start () {
			uiEvents = GameObject.FindWithTag("UIEventSystem").GetComponent<EventSystem>();
		}
	
		protected void Update () {
			if (!hasPreselectedButton) {
				uiEvents.SetSelectedGameObject(GameObject.Find("BtnTwoPlayers"));
				hasPreselectedButton = true;
			}
		}
	}
}
