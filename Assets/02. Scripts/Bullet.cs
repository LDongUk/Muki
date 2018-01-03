using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	public GameObject damagedEffect;

	private float timeForDel;
	private float timeForDelLim = 0.5f;
	private Rigidbody rg;
	Vector3 dir;
	PlayerCtrl2 player;

	// Use this for initialization
	void Start () {
		rg = GetComponent<Rigidbody> ();
		SoundManager.instance.PlayerAttackSound ();
		player = GameObject.Find ("Muki").GetComponent<PlayerCtrl2> ();
		dir = player.attackDir;
	}

	// Update is called once per frame
	void Update () {
		
		rg.MovePosition (transform.position + dir);
		timeForDel += Time.deltaTime;

		if (timeForDel > timeForDelLim)
			Destroy (gameObject);

	}

	void OnCollisionEnter(Collision col){
		if (col.gameObject.tag.Equals ("Player")) {
		}
		else if (col.gameObject.tag.Equals ("Land")) {
			StartCoroutine ("destroyDelay");
		}
		else if (col.gameObject.tag.Equals ("Enemy")) {
			StartCoroutine ("Hitted");
		}
		else{
			//Destroy (gameObject);
			StartCoroutine ("destroyDelay");
		}
	}

	IEnumerator destroyDelay(){
		yield return new WaitForSeconds (0.1f);
		Destroy (gameObject);
	}

	IEnumerator Hitted(){
		yield return new WaitForSeconds (0.1f);
		Instantiate (damagedEffect, transform.position + (Vector3.up * 3), Quaternion.Euler (Vector3.left * 90));
		Destroy (gameObject);
	}

}
