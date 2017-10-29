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
		private ParticleSystem particles;

		public GameObject targetPlayer = null;
		public GameObject objectBeingDragged = null;
		public FixedJoint dragJoint = null;
	
		public PlayerID id = PlayerID.Amber;
		public TeamID team = TeamID.TeamA;

		public int attackCooldown = 0;
		public int stunCooldown = 0;

		public float attackForce = 85.0f;
		public int attackDelay = 200;
		public int stunDelay = 150;

		public string animationName = "player_idle";

		public float inputX = 0;
		public float inputY = 0;
		public bool tryingToHold = false;
		public bool tryingToAttack = false;
		public bool isStunned = false;
		public float speed = 150.0f;
		public float animationDeadzone = 0.05f;
	
		protected void Start () {
			body = GetComponent<Rigidbody>();
			//particles = GetComponent<ParticleSystem>();
			animator = GetComponentInChildren<Animator>();
			match = GameObject.FindWithTag("GameController").GetComponent<MatchController>();
		}
	
		protected void Update () {
			if (!IsActive()) return;
		
			CheckInputs();

			if (tryingToAttack) {
				Attack();
			}
			
			HandleStun();
			HandleMovement();
			HandleGrabbing();
			HandleAnimations();
			
			TickCooldowns();
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

		private void HandleStun() {
			if (!isStunned) return;
			
			inputX = 0;
			inputY = 0;
			tryingToHold = false;
			tryingToAttack = false;
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

			bool isAttacking = (attackCooldown > (attackDelay - 8));

			if (body.velocity.x > animationDeadzone) animationName = ((isAttacking) ? "attack" : "walk") + "_right";
			if (body.velocity.x < -animationDeadzone) animationName = ((isAttacking) ? "attack" : "walk") + "_left";
			if (body.velocity.z < -animationDeadzone && body.velocity.z < body.velocity.x) animationName = ((isAttacking) ? "attack" : "walk") + "_down";
			if (body.velocity.z > animationDeadzone && body.velocity.z > body.velocity.x) animationName = ((isAttacking) ? "attack" : "walk") + "_up";
			if (isStunned) animationName = "stun";

			animator.Play(GetPlayerPrefix() + "_" + animationName);

		}

		private void TickCooldowns() {
			if(attackCooldown > 0) attackCooldown--;
			if(stunCooldown > 0) stunCooldown--;

			if (stunCooldown == 0) isStunned = false;
		}
		
		public void Attack() {

			if (attackCooldown > 0) return;
			
			attackCooldown = attackDelay;
			//particles.Emit(64);
			
			if (!targetPlayer) {
				// TODO: emit 'ugh' sound
				return;
			}

			targetPlayer.GetComponent<Player>().Stun(this);
			
		}

		public void Stun(Player attacker) {
			Debug.Log(GetPlayerPrefix() + " -> STUNNED!");
			
			isStunned = true;
			stunCooldown = stunDelay;
			
			body.AddForce((transform.position - attacker.transform.position).normalized * attackForce, ForceMode.Impulse);
			
			// TODO: emit 'ouch' sound
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

		private void OnTriggerEnter(Collider other) {
			if (!other.CompareTag("Player")) return;
			//if (other.GetComponent<Player>().team == team) return; // Disable friendly fire
			if (targetPlayer) return;

			targetPlayer = other.gameObject;
		}

		private void OnTriggerExit(Collider other) {
			if (!other.CompareTag("Player")) return;
			if (!targetPlayer) return;

			if (targetPlayer.Equals(other.gameObject)) {
				targetPlayer = null;
			}
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
