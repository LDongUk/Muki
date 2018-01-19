using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollViewCtrl : MonoBehaviour {

	RectTransform[] childList;

	// Use this for initialization
	void Start () {
		childList = gameObject.GetComponentsInChildren<RectTransform> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetAxisRaw("Horizontal") < 0) {

			//Vector2.MoveTowards (transform.localPosition, new Vector2 (167, 0), Time.deltaTime);
			//transform.localPosition = new Vector2 (167, 0);

			childList [1].SetSiblingIndex (3);

		}
	}
}
