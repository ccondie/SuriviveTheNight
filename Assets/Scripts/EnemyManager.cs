using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {

	public GameObject enemy;

	public float MinX = 0;
	public float MaxX = 1;
	public float MinY = 0;
	public float MaxY = 1;

	// Use this for initialization
	void Start () {
		for (int i = 0; i < 5; i++) {
			Spawn ();
		}
	}

	void Spawn() {
		float x = Random.Range(MinX,MaxX);
		float y = Random.Range(MinY,MaxY);
		float z = 0;
		Instantiate (enemy, new Vector3(x,y,z), Quaternion.identity);
	}


	
	// Update is called once per frame
	void Update () {
		
	}
}
