using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomBlockSpawner : MonoBehaviour {

	public GameObject blockPrefab;
	
	public int numBlocksToStartWith = 8;
	public int maxSpawnedObjects = 32;
	
	public float minX = -3.5f;
	public float maxX = 3.5f;
	
	public float minY = -2;
	public float maxY = 2;

	public float initialBlockHeight = 0.6f;
	
	public float minBlockDistance = 0.4f;
	public float initialSpawnDelay = 5.0f;
	public float spawnInterval = 5.0f;
	
	protected void Start () {

		for (int i = 0; i < numBlocksToStartWith; i++) {
			
		}
		
		InvokeRepeating("SpawnRandomBlock", initialSpawnDelay, spawnInterval);
		
	}
	
	protected void Update () {
		
	}

	private void SpawnRandomBlock() {

		// Ensures blocks are not spammed
		if (GameObject.FindGameObjectsWithTag("CanBeGrabbed").Length >= maxSpawnedObjects) return;

		Vector3 position = GenerateRandomPosition();
		int maxIterations = 32;

		// Ensures blocks don't overlap with players or other blocks
		while (Physics.CheckSphere(position, minBlockDistance)) {
			if (maxIterations-- <= 0) return;
			position = GenerateRandomPosition();
		}
		
		SpawnBlock( GenerateRandomBlockID(), position );
	}

	private BlockType GenerateRandomBlockID() {
		int random = (int) (Random.value * 100);

		if (random > 45) return BlockType.Worm;
		if (random < 80) return BlockType.Larvae;

		return BlockType.Crawfish;
	}

	private Vector3 GenerateRandomPosition() {
		Vector3 pos = new Vector3();

		pos.x = Random.Range(minX, maxX);
		pos.z = Random.Range(minY, maxY);
		pos.y = initialBlockHeight;

		return pos;
	}

	private void SpawnBlock(BlockType type, Vector3 position) {
		GameObject obj = Instantiate<GameObject>(blockPrefab, position, Quaternion.identity);
		obj.transform.parent = gameObject.transform;
		obj.GetComponent<Block>().Type = type;
	}
}
