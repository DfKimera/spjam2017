using Identifiers;
using UnityEngine;

namespace Entities {
	public class Block : MonoBehaviour {

		public BlockType type = BlockType.Crawfish;
		public bool isAttracting = false;

		public int attractRange = 3;
		public int attractForce = 40;
	
		private MeshRenderer mesh;
		private Rigidbody body;

		public Material materialCrawfish;
		public Material materialLarvae;
		public Material materialWorm;

		protected void Start () {
			mesh = GetComponent<MeshRenderer>();
			body = GetComponent<Rigidbody>();
		
			switch (type) {
				case BlockType.Crawfish: mesh.material = materialCrawfish; break;
				case BlockType.Larvae: mesh.material = materialLarvae; break;
				case BlockType.Worm: mesh.material = materialWorm; break;
			}

			body.mass = GetBlockMass();
		}
	
		protected void Update () {
			HandleClusterAttraction();
		}

		public float GetBlockMass() {
			switch (type) {
				case BlockType.Crawfish: return 40;
				case BlockType.Larvae: return 25;
				case BlockType.Worm: return 10;
			}

			return 10;
		}

		private void HandleClusterAttraction() {

			if (!isAttracting) return;
		
			foreach (Collider c in Physics.OverlapSphere(transform.position, attractRange)) {

				Block block = c.GetComponent<Block>();

				if (block == null) continue;
				if (block.type != type) continue;
				if (!block.isAttracting) continue;
			
				Vector3 forceDirection = transform.position - c.transform.position;
				c.GetComponent<Rigidbody>().AddForce(forceDirection.normalized * attractForce);
			}
		
		}
	
	}
}
