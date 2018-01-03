using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Del_Effect : MonoBehaviour {

	public float timeForDel;
	public float timeForDelLim;
	
	// Update is called once per frame
	void Update () {

		timeForDel += Time.deltaTime;

		if (timeForDel > timeForDelLim)
			Destroy (gameObject);
	}
}
