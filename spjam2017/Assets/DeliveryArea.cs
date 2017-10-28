using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryArea : MonoBehaviour {

	public TeamID team = TeamID.Team1;
	
	private SphereCollider area;
	private List<GameObject> objectsInArea;
	private Dictionary<BlockID, int> objectCounter;
	
	protected void Start () {
		area = GetComponent<SphereCollider>();
		objectsInArea = new List<GameObject>();
		objectCounter = new Dictionary<BlockID, int>();
		
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

		CheckIfScored();
	}

	private void OnTriggerExit(Collider other) {
		
		Debug.Log(GetTeamID() + ": Block left: " + other.gameObject.tag + " - " + other.gameObject.GetHashCode());
		
		if (!other.gameObject.CompareTag("CanBeGrabbed")) return;
		if (!objectsInArea.Contains(other.gameObject)) return;

		objectsInArea.Remove(other.gameObject);

	}

	private void CheckIfScored() {
		objectCounter.Clear();
		
		objectsInArea.ForEach(o => {
			BlockID id = o.GetComponent<Block>().id;
			
			if (!objectCounter.ContainsKey(id)) {
				objectCounter[id] = 0;
			}

			objectCounter[id]++;

			if (objectCounter[id] >= 3) {
				Debug.Log(GetTeamID() + ": SCORE!");
			}
			
		});


		foreach (KeyValuePair<BlockID, int> kv in objectCounter) {
			Debug.Log(GetTeamID() + ": Counter -> " + kv.Key + " : " + kv.Value);	
		}
		
	}
}
