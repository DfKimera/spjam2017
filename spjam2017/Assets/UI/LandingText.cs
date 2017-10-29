using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Controllers;
using UnityEngine;
using UnityEngine.UI;
using Entities;

public class LandingText : MonoBehaviour
{
	public float timer = 4.00f;
	public Text renderText;
	public MatchController match;
	public bool isFinish = false;
    public Player player1;
    public Player player2;
    public Player player3;
    public Player player4;
    public Timer timeRender;


    private float initTime;
	private bool active = false;
	
	// Use this for initialization
	void Start ()
	{
		initTime = timer;
		timer += 2.4f;
	}


	

	// Update is called once per frame
	void Update () {

        timer -= Time.deltaTime;

        if (timer > initTime + 2.0f) return;

        if (timer > initTime + 1.00f)
        {
            renderText.enabled = true;
            return;
        }

        if (timer > 1.5f)
        {
            renderText.text = (timer - 1.00f).ToString("0");
            return;
        }

        if (timer > 0)
        {
            renderText.text = "GO!";
            player1.inputOn = player2.inputOn = player3.inputOn = player4.inputOn = timeRender.TimerOn =  true;
            return;
        }
        
        renderText.enabled = false;
        active = isFinish = false;
        
    }
}
