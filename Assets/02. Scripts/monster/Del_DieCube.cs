using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Del_DieCube : MonoBehaviour {

	Rigidbody rigd;

	// Use this for initialization
	void Start () {
		rigd = GetComponent<Rigidbody> ();

		rigd.AddForce (Vector3.up * 5, ForceMode.Impulse);
	}


	void OnCollisionEnter(Collision col){

		//땅인지 체크
		if (col.gameObject.tag.Equals ("Land") || col.gameObject.tag.Equals("Player")) {
			Destroy (gameObject);

		}
	}

}
