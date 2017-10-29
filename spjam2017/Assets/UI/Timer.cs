using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{

	public float time = 120.00f;
	public Text textRender;
	public Text finishRender;
	public GameObject timerObject;
    public bool TimerOn;

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        if (TimerOn) time -= Time.deltaTime;

        if (time > 10)
        {                
            textRender.text = time.ToString("0");
            return;
        }
        if(time > 0 && time < 10)
        {
            textRender.text = time.ToString("0.00");
            return;
        }

        textRender.text = "0.00";
        finishRender.enabled = true;
    }
	
	
}
