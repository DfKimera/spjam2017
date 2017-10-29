using Entities;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers {
	public class PlayerUIController : MonoBehaviour {

		private Player player;
		private GameObject stunnedLabel;
		private GameObject attackCooldownMeter;
		
		protected void Start () {
			player = GetComponentInParent<Player>();
			stunnedLabel = GetComponentInChildren<Text>().gameObject;
			attackCooldownMeter = GetComponentInChildren<Scrollbar>().gameObject;
		}
	
		protected void Update () {
			bool isCoolingDown = player.attackCooldown > 0;
			
			stunnedLabel.SetActive(player.isStunned);
			attackCooldownMeter.SetActive(isCoolingDown);

			if (!isCoolingDown) return;

			attackCooldownMeter.GetComponent<Scrollbar>().size = ((float) player.attackCooldown / (float) player.attackDelay);
		}
	}
}
