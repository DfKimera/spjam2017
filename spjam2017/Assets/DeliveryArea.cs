using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryArea : MonoBehaviour {

	public TeamID team = TeamID.Team1;
	
	private List<GameObject> objectsInArea;
	private Dictionary<BlockID, int> objectCounter;
	private MatchController match;
	
	protected void Start () {
		objectsInArea = new List<GameObject>();
		objectCounter = new Dictionary<BlockID, int>();
		
		match = GameObject.FindGameObjectWithTag("GameController").GetComponent<MatchController>();
		
		Debug.Log("DeliveryArea initialized: " + GetTeamID());
	}
	
	protected void Update () {
		
	}

	private string GetTeamID() {
		switch (team) {
			case TeamID.Team1: return "Team_1";
			case TeamID.Team2: return "Team_2";
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

	private void CheckIfScored() {
		objectCounter.Clear();

		bool shouldDestroy = false;
		BlockID destroyWithID = BlockID.A;
		
		objectsInArea.ForEach(o => {
			BlockID id = o.GetComponent<Block>().id;
			
			if (!objectCounter.ContainsKey(id)) {
				objectCounter[id] = 0;
			}

			objectCounter[id]++;

			if (objectCounter[id] >= 3) {
				Debug.Log(GetTeamID() + ": SCORE!");
				match.AwardPoints(team, 10);

				shouldDestroy = true;
				destroyWithID = id;
			}
			
		});


		if (shouldDestroy) {
			DestroyBlocksWithId(destroyWithID);
		}

		foreach (KeyValuePair<BlockID, int> kv in objectCounter) {
			Debug.Log(GetTeamID() + ": Counter -> " + kv.Key + " : " + kv.Value);	
		}
		
	}

	private void DestroyBlocksWithId(BlockID id) {
		
		List<GameObject> objectsToDestroy = new List<GameObject>(); 
		
		objectsInArea.ForEach(o => {
			if (!o.GetComponent<Block>().id.Equals(id)) return;
			
			objectsToDestroy.Add(o);
		});
		
		objectsToDestroy.ForEach(o => {
			Destroy(o);
			objectsInArea.Remove(o);
		});
		
		objectsToDestroy.Clear();
	}
}
