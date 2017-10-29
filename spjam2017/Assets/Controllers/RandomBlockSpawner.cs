using Entities;
using Identifiers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Controllers {
	public class RandomBlockSpawner : MonoBehaviour {

		public GameObject blockPrefab;
		public MatchController match;
	
		public int numBlocksToStartWith = 8;
		public int maxSpawnedObjects = 32;
	
		public float minX = -3.5f;
		public float maxX = 3.5f;
	
		public float minY = -2;
		public float maxY = 2;

		public float initialBlockHeight = 0.6f;
	
		public float minBlockDistance = 0.4f;
		public float initialSpawnDelay = 5.0f;
		public float spawnInterval = 4.0f;
		
		public float spawnIntervalFourPlayers = 2.3f;
		public float spawnIntervalTwoPlayers = 4.0f;
	
		protected void Start () {
			match = GameObject.FindWithTag("GameController").GetComponent<MatchController>();
		}
	
		protected void Update () {
		
		}

		public void SpawnInitialBlocks() {
			for (int i = 0; i < numBlocksToStartWith; i++) {
				SpawnRandomBlock();
			}
		}

		public void StartSpawnTimer() {
			spawnInterval = match.matchType == MatchType.FourPlayers ? spawnIntervalFourPlayers : spawnIntervalTwoPlayers;
			InvokeRepeating("SpawnRandomBlock", initialSpawnDelay, spawnInterval);
		}

		public void ClearAllBlocks() {
			foreach(GameObject obj in GameObject.FindGameObjectsWithTag("CanBeGrabbed")) {
				Destroy(obj);
			}
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

			if (random < 45) return BlockType.Worm;
			if (random < 85) return BlockType.Larvae;

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
			obj.GetComponent<Block>().type = type;
		}
	}
}
