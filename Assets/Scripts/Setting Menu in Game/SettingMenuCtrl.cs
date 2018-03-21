using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SettingMenuCtrl : MonoBehaviour {

	GameObject[] buttons;
	public int current = 0;
	public bool isActive = false;

	SpriteRenderer rend;

	// Use this for initialization
	void Start () {
		buttons = new GameObject[4];
		buttons [0] = GameObject.Find ("Continue");
		buttons [1] = GameObject.Find ("Option");
		buttons [2] = GameObject.Find ("Title");
		buttons [3] = GameObject.Find ("Exit");

		rend = GetComponent<SpriteRenderer> ();
		Unvisualize ();

		//transform.position = new Vector2 (transform.position.x, buttons [0].transform.position.y);
	}



	// Update is called once per frame
	void Update () {

		if (!isActive) {
			transform.position = new Vector2 (transform.position.x, buttons [0].transform.position.y);
			Visualize ();
			isActive = true;
		}

		//Menu is Activate
		if (GameObject.Find("Canvas").GetComponent<CanvasCtrl> ().isActive) {


			if (Input.GetButtonDown ("Up")) {
			
				if (current == 0)
					current = 3;
				else
					current--;

				transform.position = new Vector2 (transform.position.x, buttons [current].transform.position.y);
			}

			if (Input.GetButtonDown ("Down")) {

				if (current == 3)
					current = 0;
				else
					current++;

				transform.position = new Vector2 (transform.position.x, buttons [current].transform.position.y);
			}


			//Select menu
			if (Input.GetKey (KeyCode.Return)) {

				switch (current) {
				case 0:
					Time.timeScale = 1f;
					GameObject.Find ("Canvas").GetComponent<CanvasCtrl> ().setDisactive ();
					break;
				
				case 1:
					break;
				case 2:
					break;
				case 3:
					Application.Quit ();
					break;
				}
					
			}

		}//end if GetComponent

	}//end Update


	public void Unvisualize(){
		rend.color = new Color32 (255, 255, 255, 0);
	}

	public void Visualize(){
		rend.color = new Color32 (255, 255, 255, 255);
	}
}
