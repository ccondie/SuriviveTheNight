using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour {

	public Text timeUI = null;
	public static int frame_count = 0;


	// Use this for initialization
	void Awake()
	{

	}

	// Update is called once per frame
	void Update()
	{
		frame_count += 1;
		timeUI.text = "[" + frame_count.ToString("D8") + "]";
	}
}
