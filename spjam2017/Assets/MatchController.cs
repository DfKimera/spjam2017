using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchController : MonoBehaviour {

	private Dictionary<TeamID, int> score = new Dictionary<TeamID, int>();
	
	protected void Start () {
		score[TeamID.Team1] = 0;
		score[TeamID.Team2] = 0;
	}
	
	protected void Update () {
		
	}

	public int GetScore(TeamID team) {
		return score[team];
	}

	public void AwardPoints(TeamID team, int numPoints) {
		score[team] += numPoints;
	}

	public void TakePoints(TeamID team, int numPoints) {
		score[team] -= numPoints;

		if (score[team] < 0) score[team] = 0;
	}
}
