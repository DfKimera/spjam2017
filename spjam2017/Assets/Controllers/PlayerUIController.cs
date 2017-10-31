using Entities;
using Identifiers;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers {
	public class PlayerUIController : MonoBehaviour {

		private Player player;
		private GameObject stunnedLabel;
		private GameObject attackCooldownMeter;
		private GameObject teamALabel;
		private GameObject teamBLabel;
		
		protected void Start () {
			player = GetComponentInParent<Player>();
			stunnedLabel = GetComponentInChildren<Text>().gameObject;
			attackCooldownMeter = GetComponentInChildren<Scrollbar>().gameObject;
			teamALabel = transform.Find("TeamALabel").gameObject;
			teamBLabel = transform.Find("TeamBLabel").gameObject;
		}
	
		protected void Update () {
			bool isCoolingDown = player.attackCooldown > 0;
			
			stunnedLabel.SetActive(player.isStunned);
			attackCooldownMeter.SetActive(isCoolingDown);
			
			teamALabel.SetActive(player.team == TeamID.TeamA);
			teamBLabel.SetActive(player.team == TeamID.TeamB);

			if (!isCoolingDown) return;

			attackCooldownMeter.GetComponent<Scrollbar>().size = ((float) player.attackCooldown / (float) player.attackDelay);
		}
	}
}
