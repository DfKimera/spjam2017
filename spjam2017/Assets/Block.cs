using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;

public class Block : MonoBehaviour {

	public BlockID id = BlockID.A;
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
		
	}
}
