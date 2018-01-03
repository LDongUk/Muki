using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour {

	PlayerCtrl2 plct;
	// Use this for initialization
	void Start () {
		plct = GameObject.Find ("Muki").GetComponent<PlayerCtrl2> ();
	}

	// Update is called once per frame
	void Update () {

		float distance = Vector3.Distance (plct.transform.position, transform.position);

		if (distance <= 2.5) {
			Destroy (gameObject);
			SoundManager.instance.Item ();

			plct.isPowerUp = true;
		}
	}
}
