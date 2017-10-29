using System.Collections.Generic;
using Controllers;
using Identifiers;
using UnityEngine;

namespace Entities {
	public class DeliveryArea : MonoBehaviour {

		public TeamID team = TeamID.TeamA;
	
		private List<GameObject> objectsInArea;
		private Dictionary<BlockType, int> objectCounter;
		private MatchController match;
	
		protected void Start () {
			objectsInArea = new List<GameObject>();
			objectCounter = new Dictionary<BlockType, int>();
		
			match = GameObject.FindGameObjectWithTag("GameController").GetComponent<MatchController>();
		
			Debug.Log("DeliveryArea initialized: " + GetTeamID());
		}
	
		protected void Update () {
		
		}

		public int GetBlockCount(BlockType type) {
			if (!this.objectCounter.ContainsKey(type)) return 0;
			return this.objectCounter[type];
		}

		private string GetTeamID() {
			switch (team) {
				case TeamID.TeamA: return "Team_1";
				case TeamID.TeamB: return "Team_2";
				default: return "Unknown";
			}
		}

		private void OnTriggerEnter(Collider other) {
		
			Debug.Log(GetTeamID() + ": Block entered: " + other.gameObject.tag + " - " + other.gameObject.GetHashCode());
		
			if (!other.gameObject.CompareTag("CanBeGrabbed")) return;
			if (objectsInArea.Contains(other.gameObject)) return;

			objectsInArea.Add(other.gameObject);
			other.gameObject.GetComponent<Block>().isAttracting = true;

			CheckIfScored();
		}

		private void OnTriggerExit(Collider other) {
		
			Debug.Log(GetTeamID() + ": Block left: " + other.gameObject.tag + " - " + other.gameObject.GetHashCode());
		
			if (!other.gameObject.CompareTag("CanBeGrabbed")) return;
			if (!objectsInArea.Contains(other.gameObject)) return;
		
			other.gameObject.GetComponent<Block>().isAttracting = false;

			objectsInArea.Remove(other.gameObject);

		}
		
		public int GetNumPoints(BlockType type) {
			switch (type) {
				case BlockType.Crawfish: return 30;
				case BlockType.Larvae: return 20;
				case BlockType.Worm: return 10;
			}

			return 10;
		}

		private void CheckIfScored() {
			objectCounter.Clear();

			bool shouldDestroy = false;
			BlockType destroyWithType = BlockType.Crawfish;
		
			objectsInArea.ForEach(o => {
				if (o == null || !o.activeSelf) return;
				
				BlockType type = o.GetComponent<Block>().type;
			
				if (!objectCounter.ContainsKey(type)) {
					objectCounter[type] = 0;
				}

				objectCounter[type]++;

				if (objectCounter[type] >= 3) {
					Debug.Log(GetTeamID() + ": SCORE!");
					match.AwardPoints(team, GetNumPoints(type));

					shouldDestroy = true;
					destroyWithType = type;

					objectCounter[type] = 0;
				}
			
			});


			if (shouldDestroy) {
				DestroyBlocksWithId(destroyWithType);
			}

			foreach (KeyValuePair<BlockType, int> kv in objectCounter) {
				Debug.Log(GetTeamID() + ": Counter -> " + kv.Key + " : " + kv.Value);	
			}
		
		}

		private void DestroyBlocksWithId(BlockType type) {
		
			List<GameObject> objectsToDestroy = new List<GameObject>(); 
		
			objectsInArea.ForEach(o => {
				if (o == null || !o.activeSelf) return;
				if (!o.GetComponent<Block>().type.Equals(type)) return;
			
				objectsToDestroy.Add(o);
			});
		
			objectsToDestroy.ForEach(o => {
				objectsInArea.Remove(o);
				Destroy(o);
			});
		
			objectsToDestroy.Clear();
		}
	}
}
