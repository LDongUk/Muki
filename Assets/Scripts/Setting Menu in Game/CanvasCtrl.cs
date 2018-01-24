using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasCtrl : MonoBehaviour {

	public bool isActive = false;

	// Use this for initialization
	void Start () {
		gameObject.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			
			if(isActive) {
				isActive = false;
				Time.timeScale = 1f;
				setDisactive ();
			}
		}
	}


	//Exit menu
	public void setDisactive(){
		GameObject.Find ("Select").GetComponent<SettingMenuCtrl> ().isActive = false;
		GameObject.Find ("Select").GetComponent<SettingMenuCtrl> ().current = 0;
		GameObject.Find ("Select").GetComponent<SettingMenuCtrl> ().Unvisualize ();
		//GameObject.Find ("Select").transform.position = new Vector2 (GameObject.Find ("Select").transform.position.x, GameObject.Find ("Continue").transform.position.y);
		gameObject.SetActive (false);
	}
}
