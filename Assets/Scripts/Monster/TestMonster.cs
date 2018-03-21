using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMonster : MonoBehaviour {

	public int health = 10;
	Rigidbody2D rigd;


	// Use this for initialization
	void Start () {
		rigd = gameObject.GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () {


		if (health <= 0) {
			Destroy (gameObject);
		}
	}

	void OnTriggerEnter2D(Collider2D other){

		if (other.gameObject.tag == "Attack Damage 1") {

			if (other.gameObject.transform.position.x < transform.position.x) {
				rigd.AddForce (Vector2.right * 800f + Vector2.up * 300f, ForceMode2D.Impulse);
			} 

			else {
				rigd.AddForce (Vector2.left * 800f  + Vector2.up * 300f, ForceMode2D.Impulse);
			}


		}
	}
}
