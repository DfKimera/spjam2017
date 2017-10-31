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

		public int footstepsCooldown = 0;
		public int footstepsDelay = 8;

		public AudioClip[] sfxFootsteps;
		public AudioClip sfxAttack;
		public AudioClip sfxStunned;
		public AudioClip sfxPickBlock;
		public AudioClip sfxDropBlock;
	
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
			HandleFootsteps();
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

			inputX = (Input.GetAxis(GetPlayerPrefix() + "_Move_Horizontal") + Input.GetAxis(GetPlayerPrefix() + "_Move_KBHorizontal")) / 2;
			inputY = (Input.GetAxis(GetPlayerPrefix() + "_Move_Vertical") + Input.GetAxis(GetPlayerPrefix() + "_Move_KBVertical")) / 2;

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
			body.AddForce(new Vector3(inputX * speed * Time.fixedDeltaTime * 100, 0, -inputY * speed * Time.fixedDeltaTime * 100), ForceMode.Force);
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

		private void HandleFootsteps() {
			if (body.velocity.x > animationDeadzone 
			    || body.velocity.x < -animationDeadzone 
			    || body.velocity.z > animationDeadzone 
			    || body.velocity.z < -animationDeadzone) {
				footstepsCooldown--;
			}

			bool isTryingToMove = inputX != 0 || inputY != 0;

			if (isTryingToMove && footstepsCooldown <= 0) {
				AudioClip footstepClip = sfxFootsteps[Random.Range(0, sfxFootsteps.Length - 1)];
				
				PlaySFX(footstepClip, 0.3f);
				
				footstepsCooldown = footstepsDelay;
			}
		}

		private void HandleAnimations() {

			animationName = "idle";

			bool isAttacking = (attackCooldown > (attackDelay - 15));

			if (isAttacking) animationName = "attack_down";
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

		public void PlaySFX(AudioClip clip, float volume = 1.0f) {
			AudioSource.PlayClipAtPoint(clip, transform.position, volume);
		}
		
		public void Attack() {

			if (attackCooldown > 0) return;
			
			attackCooldown = attackDelay;

			PlaySFX(sfxAttack);
			
			if (!targetPlayer) return;

			targetPlayer.GetComponent<Player>().Stun(this);
			
		}

		public void Stun(Player attacker) {
			Debug.Log(GetPlayerPrefix() + " -> STUNNED!");
			
			isStunned = true;
			stunCooldown = stunDelay;
			
			body.AddForce((transform.position - attacker.transform.position).normalized * attackForce, ForceMode.Impulse);
			
			PlaySFX(sfxStunned);
		}
		
	
		private void GrabBlock(GameObject obj) {
			dragJoint = gameObject.AddComponent<FixedJoint>() as FixedJoint;
			dragJoint.connectedBody = objectBeingDragged.GetComponent<Rigidbody>();
			dragJoint.connectedBody.transform.Translate(Vector3.up * 0.1f);
			
			PlaySFX(sfxPickBlock);
		
			Debug.Log(GetPlayerPrefix() + " [release grab] Created joint: " + gameObject.GetHashCode());
		}

		private void ReleaseBlock() {
		
			Debug.Log(GetPlayerPrefix() + " [release hold] Found joint, destroying: " + gameObject.GetHashCode());

			if (dragJoint == null) return;
		
			if (dragJoint.connectedBody != null) {
				dragJoint.connectedBody.transform.Translate(Vector3.down * 0.1f);
			}
			
			PlaySFX(sfxDropBlock);
	
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
