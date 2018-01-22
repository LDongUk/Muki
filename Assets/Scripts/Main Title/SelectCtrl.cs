using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectCtrl : MonoBehaviour {

	GameObject[] buttons;
	int current = 0;
	bool isButtonDown = false;
	bool oneButton = false;

	float timeSpan;
	float checkTime;

	// Use this for initialization
	void Start () {
		buttons = new GameObject[4];
		buttons [0] = GameObject.Find ("Start");
		buttons [1] = GameObject.Find ("Load");
		buttons [2] = GameObject.Find ("Option");
		buttons [3] = GameObject.Find ("Exit");
	
		timeSpan = 0f;
		checkTime = 0.5f;
	}
	
	// Update is called once per frame
	void Update () {
	
		if (!isButtonDown) {

			if (Input.GetButton("Up")) {

				timeSpan += Time.deltaTime;

				if (timeSpan < checkTime && !oneButton) {
					KeyUp ();
					oneButton = true;
				
				}

				else if (timeSpan >= checkTime) {
					KeyUp ();
					isButtonDown = true;
					StartCoroutine ("ButtonDown");
				}

			}
			else if (Input.GetButton("Down")) {

				timeSpan += Time.deltaTime;

				if (timeSpan < checkTime && !oneButton) {
					KeyDown ();
					oneButton = true;

				}

				else if (timeSpan >= checkTime) {
					KeyDown ();
					isButtonDown = true;
					StartCoroutine ("ButtonDown");
				}
			}
		

//			if (isButtonDown)
//				StartCoroutine ("ButtonDown");
		}

		if (Input.GetButtonUp ("Up") || Input.GetButtonUp("Down")) {
			timeSpan = 0f;
			oneButton = false;
		}

		if (Input.GetKey (KeyCode.Return)) {

			switch (current) {
			case 0:
				SceneManager.LoadScene (1);
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

	}

	void KeyUp(){
		
		if (current == 0)
			current = 3;
		else
			current--;

		transform.position = Vector2.right * transform.position.x + Vector2.up * buttons [current].transform.position.y;

	}

	void KeyDown(){
		if (current == 3)
			current = 0;
		else
			current++;

		transform.position = Vector2.right * transform.position.x + Vector2.up * buttons [current].transform.position.y;

	}



	IEnumerator ButtonDown(){

		yield return new WaitForSeconds (0.2f);

		isButtonDown = false;
	}

}
