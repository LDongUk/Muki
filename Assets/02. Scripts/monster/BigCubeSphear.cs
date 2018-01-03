using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigCubeSphear : MonoBehaviour {


	Transform playerPos;

	// Use this for initialization
	void Start () {
		playerPos = GameObject.Find ("Muki").GetComponent<Transform> ();
	}
	
	// Update is called once per frame
	void Update () {

		if(playerPos)
			transform.position = Vector3.MoveTowards (transform.position,
				playerPos.position, 10 * Time.deltaTime);


//		if (Vector3.Distance (transform.position, playerPos.position) <= 7) {
//			Destroy (gameObject);
//
//		}
	}

	void OnCollisionEnter(Collision col){

		if (col.gameObject.tag.Equals ("Boss") || col.gameObject.tag.Equals ("Bullet")) {
		}


		else{
			Destroy (gameObject);
			//StartCoroutine ("destroyDelay");
		}
	}

	IEnumerator destroyDelay(){
		yield return new WaitForSeconds (0.1f);
		Destroy (gameObject);
	}

}
