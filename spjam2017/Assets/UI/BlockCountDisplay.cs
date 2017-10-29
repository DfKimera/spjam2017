using System.Collections.Generic;
using Entities;
using Identifiers;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
	public class BlockCountDisplay : MonoBehaviour {

		public TeamID team;
		public string alignment = "left";
		public GameObject iconPrefab;
		public DeliveryArea deliveryArea;

		public int maxIcons = 3;
		public int iconSize = 80;
		public List<GameObject> icons = new List<GameObject>();
		public Dictionary<BlockType, int> count = new Dictionary<BlockType, int>();
		
		protected void Start () {
			count[BlockType.Crawfish] = 0;
			count[BlockType.Larvae] = 0;
			count[BlockType.Worm] = 0;
		}
	
		protected void Update () {
			if (deliveryArea.GetBlockCount(BlockType.Crawfish) != count[BlockType.Crawfish]) {
				count[BlockType.Crawfish] = deliveryArea.GetBlockCount(BlockType.Crawfish);
				RefreshIcons();
			}
			
			if (deliveryArea.GetBlockCount(BlockType.Larvae) != count[BlockType.Larvae]) {
				count[BlockType.Larvae] = deliveryArea.GetBlockCount(BlockType.Larvae);
				RefreshIcons();
			}
			
			if (deliveryArea.GetBlockCount(BlockType.Worm) != count[BlockType.Worm]) {
				count[BlockType.Worm] = deliveryArea.GetBlockCount(BlockType.Worm);
				RefreshIcons();
			}
		}

		private void RefreshIcons() {
			
			icons.ForEach(o => {
				Destroy(o);
			});
			
			icons.Clear();

			for (int i = 0; i < count[BlockType.Crawfish]; i++) {
				SpawnIcon(GetXOffset(i), 0, BlockType.Crawfish);
			}

			for (int i = 0; i < count[BlockType.Larvae]; i++) {
				SpawnIcon(GetXOffset(i), iconSize, BlockType.Larvae);
			}

			for (int i = 0; i < count[BlockType.Worm]; i++) {
				SpawnIcon(GetXOffset(i), iconSize * 2, BlockType.Worm);
			}
			
		}

		private int GetXOffset(int index) {
			if (alignment == "left") {
				return index * iconSize;
			}

			return (maxIcons * iconSize) - (index * iconSize);
		}

		private void SpawnIcon(int x, int y, BlockType type) {
			GameObject obj = Instantiate(iconPrefab, transform);
			obj.GetComponent<RectTransform>().localPosition = new Vector3(x, y, 0);
			//obj.GetComponent<RectTransform>().anchoredPosition.Set(x, y);
			obj.GetComponent<Image>().sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Blocks/" + GetSpritePathByBlock(type));
			icons.Add(obj);
		}

		private string GetSpritePathByBlock(BlockType type) {
			switch (type) {
				case BlockType.Crawfish: return "crawfish_symbol.png";
				case BlockType.Larvae: return "larvae_symbol.png";
				case BlockType.Worm: return "worm_symbol.png";
			}

			return "crawfish_symbol.png";
		}
	}
}
