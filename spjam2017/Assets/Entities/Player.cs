using Controllers;
using Identifiers;
using Structs;
using UnityEngine;

namespace Entities {
	public class Player : MonoBehaviour {

		private Rigidbody body;
		private SphereCollider radius;
		private Animator animator;
		private MatchController match;

		public GameObject objectBeingDragged = null;
		public FixedJoint dragJoint = null;
	
		public PlayerID id = PlayerID.Amber;
		public TeamID team = TeamID.TeamA;

		public string animationName = "player_idle";

		public float inputX = 0;
		public float inputY = 0;
		public bool tryingToHold = false;
		public bool tryingToAttack = false;
		public bool isStunned = false;
		public float speed = 150.0f;
		public float animationDeadzone = 0.05f;
        public bool inputOn = false;
	
		protected void Start () {
			body = GetComponent<Rigidbody>();
			animator = GetComponentInChildren<Animator>();
			match = GameObject.FindWithTag("GameController").GetComponent<MatchController>();
		}
	
		protected void Update () {
			if (!IsActive()) return;
		    if(inputOn) CheckInputs();
			HandleMovement();
			HandleGrabbing();
			HandleAnimations();
		}

		private bool IsActive() {
			return match.hasMatchStarted && GetSettings().isPlaying;
		}

		private PlayerSettings GetSettings() {
			return match.GetPlayerSettings(this.id);
		}
	
		private string GetPlayerPrefix() {
			switch (id) {
				default: case PlayerID.Amber: return "yellow";
				case PlayerID.Redmayne: return "red";
				case PlayerID.Pinkerton: return "pink";
				case PlayerID.Dorange: return "orange";
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
			if (!objectBeingDragged) return;

			if (tryingToHold && !dragJoint) {
				GrabBlock(objectBeingDragged);
			}
		
			if (!tryingToHold && dragJoint) {
				ReleaseBlock();
			}
		}

		private void HandleAnimations() {

			animationName = "idle";

			if (body.velocity.x > animationDeadzone) animationName = "walk_right";
			if (body.velocity.x < -animationDeadzone) animationName = "walk_left";
			if (body.velocity.z < -animationDeadzone && body.velocity.z < body.velocity.x) animationName = "walk_down";
			if (body.velocity.z > animationDeadzone && body.velocity.z > body.velocity.x) animationName = "walk_up";
			if (isStunned) animationName = "stun";

			animator.Play(GetPlayerPrefix() + "_" + animationName);

		}
	
		private void GrabBlock(GameObject obj) {
			dragJoint = gameObject.AddComponent<FixedJoint>() as FixedJoint;
			dragJoint.connectedBody = objectBeingDragged.GetComponent<Rigidbody>();
			dragJoint.connectedBody.transform.Translate(Vector3.up * 0.1f);
		
			Debug.Log(GetPlayerPrefix() + " [release grab] Created joint: " + gameObject.GetHashCode());
		}

		private void ReleaseBlock() {
		
			Debug.Log(GetPlayerPrefix() + " [release hold] Found joint, destroying: " + gameObject.GetHashCode());

			if (dragJoint == null) return;
		
			if (dragJoint.connectedBody != null) {
				dragJoint.connectedBody.transform.Translate(Vector3.down * 0.1f);
			}
	
			Destroy(dragJoint);
			dragJoint = null;
		
		}

		private void OnTriggerStay(Collider other) {
			if (!IsActive()) return;
		
			if (!other.CompareTag("CanBeGrabbed")) return;
		
			if (tryingToHold) {
				objectBeingDragged = other.gameObject;
				return;
			}
		
			objectBeingDragged = null;
		}
	}
}
