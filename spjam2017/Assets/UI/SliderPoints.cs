using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Controllers;
using Identifiers;

public class SliderPoints : MonoBehaviour
{	
	public Slider sliderRendererB;

	private MatchController match;
	private Slider sliderRendererA;
	private bool active = false;
	
	protected void Start () {
		match = GameObject.FindGameObjectWithTag("GameController").GetComponent<MatchController>();
		sliderRendererA = GetComponent<Slider>();
	}
	
	protected void Update () {
		
		var scoreTeam1 = match.GetScore(TeamID.TeamA);
		var scoreTeam2 = match.GetScore(TeamID.TeamB);
		
		if (scoreTeam1 == scoreTeam2)
		{
			sliderRendererA.value = (float)0.5;
			sliderRendererB.value = (float)0.5;
			return;
		}
		
		var teamACent = (float)scoreTeam1 + (float)scoreTeam2;
		teamACent = 100 / teamACent;
		teamACent = teamACent * scoreTeam1;
		teamACent = teamACent / 100;
		sliderRendererA.value = (float)teamACent;
		sliderRendererB.value = 1 - (float)teamACent;
	}

	public void SetActive(bool _active = true) {
		Start();
		active = _active;
	}
}
