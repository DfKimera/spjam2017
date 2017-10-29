using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomBlockSpawner : MonoBehaviour {

	public GameObject blockPrefab;
	
	public int numBlocksToStartWith = 8;
	public float minX = -3.5f;
	public float maxX = 3.5f;
	
	public float minY = -2;
	public float maxY = 2;
	
	
	protected void Start () {

		for (int i = 0; i < numBlocksToStartWith; i++) {
			SpawnRandomBlock();
		}
		
		InvokeRepeating("SpawnRandomBlock", 5.0f, 5.0f);
		
	}
	
	protected void Update () {
		
	}

	private void SpawnRandomBlock() {
		SpawnBlock( GenerateRandomBlockID(), GenerateRandomPosition() );
	}

	private BlockID GenerateRandomBlockID() {
		Array values = Enum.GetValues(typeof(BlockID));
		System.Random random = new System.Random();
		return (BlockID) values.GetValue(random.Next(values.Length));
	}

	private Vector3 GenerateRandomPosition() {
		Vector3 pos = new Vector3();

		pos.x = Random.Range(minX, maxX);
		pos.z = Random.Range(minY, maxY);
		pos.y = 0;

		return pos;
	}

	private void SpawnBlock(BlockID id, Vector3 position) {
		GameObject obj = Instantiate<GameObject>(blockPrefab, position, Quaternion.identity);
		obj.GetComponent<Block>().id = id;
	}
}
