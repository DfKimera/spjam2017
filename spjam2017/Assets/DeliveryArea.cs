using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryArea : MonoBehaviour {

	public TeamID team = TeamID.Team1;
	
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
		BlockType destroyWithType = BlockType.Crawfish;
		
		objectsInArea.ForEach(o => {
			BlockType type = o.GetComponent<Block>().Type;
			
			if (!objectCounter.ContainsKey(type)) {
				objectCounter[type] = 0;
			}

			objectCounter[type]++;

			if (objectCounter[type] >= 3) {
				Debug.Log(GetTeamID() + ": SCORE!");
				match.AwardPoints(team, 10);

				shouldDestroy = true;
				destroyWithType = type;
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
			if (!o.GetComponent<Block>().Type.Equals(type)) return;
			
			objectsToDestroy.Add(o);
		});
		
		objectsToDestroy.ForEach(o => {
			Destroy(o);
			objectsInArea.Remove(o);
		});
		
		objectsToDestroy.Clear();
	}
}
