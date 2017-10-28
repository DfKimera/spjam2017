using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
