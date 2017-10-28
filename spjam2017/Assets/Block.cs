using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;

public class Block : MonoBehaviour {

	public BlockID id = BlockID.A;
	public bool isAttracting = false;

	public int attractRange = 2;
	public int attractForce = 10;
	
	private MeshRenderer mesh;

	protected void Start () {
		mesh = GetComponent<MeshRenderer>();

		string materialName = "block_a";
		
		switch (id) {
			case BlockID.A: materialName = "block_a"; break;
			case BlockID.B: materialName = "block_b"; break;
			case BlockID.C: materialName = "block_c"; break;
					
		}

		mesh.material = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/" + materialName + ".mat");
	}
	
	protected void Update () {
		HandleClusterAttraction();
	}

	private void HandleClusterAttraction() {

		if (!isAttracting) return;
		
		foreach (Collider c in Physics.OverlapSphere(transform.position, attractRange)) {

			Block block = c.GetComponent<Block>();

			if (block == null) continue;
			if (block.id != id) continue;
			if (!block.isAttracting) continue;
			
			Vector3 forceDirection = transform.position - c.transform.position;
			c.GetComponent<Rigidbody>().AddForce(forceDirection.normalized * attractForce * Time.fixedDeltaTime);
		}
		
	}
	
}
