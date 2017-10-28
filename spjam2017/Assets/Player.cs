using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour {

	private Rigidbody body;
	private SphereCollider radius;

	public GameObject objectBeingDragged = null;
	public FixedJoint dragJoint = null;
	
	public PlayerID id;

	public float inputX = 0;
	public float inputY = 0;
	public bool tryingToHold = false;
	public bool tryingToAttack = false;
	public float speed = 150.0f;
	
	protected void Start () {
		body = GetComponent<Rigidbody>();
	}
	
	protected void Update () {
		CheckInputs();
		HandleMovement();
		HandleGrabbing();
	}
	
	private string GetPlayerPrefix() {
		switch (id) {
			default: case PlayerID.Player1: return "P1";
			case PlayerID.Player2: return "P2";
			case PlayerID.Player3: return "P3";
			case PlayerID.Player4: return "P4";
		}
	}

	private void CheckInputs() {

		inputX = Input.GetAxis(GetPlayerPrefix() + "_Move_Horizontal");
		inputY = Input.GetAxis(GetPlayerPrefix() + "_Move_Vertical");

		tryingToHold = Input.GetButton(GetPlayerPrefix() + "_Hold");
		tryingToAttack = Input.GetButtonDown(GetPlayerPrefix() + "_Attack");
		
	}

	private void HandleMovement() {
		body.AddForce(new Vector3(inputX * speed, 0, -inputY * speed), ForceMode.Force);
	}

	private void HandleGrabbing() {
		if (!objectBeingDragged) return; // No objects in radius
		if (!tryingToHold) return; // Not dragging

		if (dragJoint) return; // Already has drag joint
		
		dragJoint = gameObject.AddComponent<FixedJoint>() as FixedJoint;
		dragJoint.connectedBody = objectBeingDragged.GetComponent<Rigidbody>();
		dragJoint.connectedBody.transform.Translate(Vector3.up * 0.1f);
		
		Debug.Log(GetPlayerPrefix() + " [release grab] Created joint: " + gameObject.GetHashCode());
	}

	private void OnTriggerStay(Collider other) {
		if (!other.CompareTag("CanBeGrabbed")) return;
		
		if (tryingToHold) {
			objectBeingDragged = other.gameObject;
			return;
		}
		
		objectBeingDragged = null;

		if (dragJoint) {
			Debug.Log(GetPlayerPrefix() + " [release hold] Found joint, destroying: " + gameObject.GetHashCode());
			dragJoint.connectedBody.transform.Translate(Vector3.down * 0.1f);
			Destroy(dragJoint);
			dragJoint = null;
		}
	}
}
