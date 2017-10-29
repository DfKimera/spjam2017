using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;

public class Block : MonoBehaviour {

	public BlockType Type = BlockType.Crawfish;
	public bool isAttracting = false;

	public int attractRange = 2;
	public int attractForce = 10;
	
	private MeshRenderer mesh;

	protected void Start () {
		mesh = GetComponent<MeshRenderer>();

		string materialName = "block_a";
		
		switch (Type) {
			case BlockType.Crawfish: materialName = "crawfish_block"; break;
			case BlockType.Larvae: materialName = "larvae_block"; break;
			case BlockType.Worm: materialName = "worm_block"; break;
					
		}

		mesh.material = AssetDatabase.LoadAssetAtPath<Material>("Assets/Blocks/" + materialName + ".mat");
	}
	
	protected void Update () {
		HandleClusterAttraction();
	}

	private void HandleClusterAttraction() {

		if (!isAttracting) return;
		
		foreach (Collider c in Physics.OverlapSphere(transform.position, attractRange)) {

			Block block = c.GetComponent<Block>();

			if (block == null) continue;
			if (block.Type != Type) continue;
			if (!block.isAttracting) continue;
			
			Vector3 forceDirection = transform.position - c.transform.position;
			c.GetComponent<Rigidbody>().AddForce(forceDirection.normalized * attractForce * Time.fixedDeltaTime);
		}
		
	}
	
}
