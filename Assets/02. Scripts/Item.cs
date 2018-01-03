using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {

	PlayerCtrl2 plct;
	// Use this for initialization
	void Start () {
		plct = GameObject.Find ("Muki").GetComponent<PlayerCtrl2> ();
	}
	
	// Update is called once per frame
	void Update () {

		float distance = Vector3.Distance (plct.transform.position, transform.position);

		if (distance <= 3.5) {
			Destroy (gameObject);
			SoundManager.instance.Item ();

			if(plct.playerHealth < 5)
				plct.hp [plct.playerHealth++].SetActive (true);
		}
	}


}
