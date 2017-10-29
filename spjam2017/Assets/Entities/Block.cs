using Identifiers;
using UnityEditor;
using UnityEngine;

namespace Entities {
	public class Block : MonoBehaviour {

		public BlockType type = BlockType.Crawfish;
		public bool isAttracting = false;

		public int attractRange = 3;
		public int attractForce = 40;
	
		private MeshRenderer mesh;
		private Rigidbody body;

		protected void Start () {
			mesh = GetComponent<MeshRenderer>();
			body = GetComponent<Rigidbody>();

			string materialName = "block_a";
		
			switch (type) {
				case BlockType.Crawfish: materialName = "crawfish_block"; break;
				case BlockType.Larvae: materialName = "larvae_block"; break;
				case BlockType.Worm: materialName = "worm_block"; break;
					
			}

			body.mass = GetBlockMass();
			mesh.material = AssetDatabase.LoadAssetAtPath<Material>("Assets/Blocks/" + materialName + ".mat");
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
