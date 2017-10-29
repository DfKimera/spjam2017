using System;
using Controllers;
using Identifiers;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
	public class ScoreDisplay : MonoBehaviour {

		public TeamID team;
		private MatchController match;

		private Text scoreRenderer;
	
		protected void Start () {
			match = GameObject.FindGameObjectWithTag("GameController").GetComponent<MatchController>();
			scoreRenderer = GetComponent<Text>();
		}
	
		protected void Update () {
			scoreRenderer.text = Convert.ToString(match.GetScore(team));
		}
	}
}
