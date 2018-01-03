using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Del_spawnCube : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter(Collision col){

		//땅인지 체크
		if (col.gameObject.tag.Equals ("Land") || col.gameObject.tag.Equals("Player")) {
			Destroy (gameObject);

		}
	}
}
