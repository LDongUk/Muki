using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScrollViewCtrl : MonoBehaviour {

	int current = 0;
	bool isButtonDown = false;
	bool oneButton = false;

	float timeSpan = 0f;
	float checkTime = 0.4f;


	public List<GameObject> buttons;
	public GameObject newMenu;
	public GameObject text_muki, Menu;

	// Use this for initialization
	void Start () {
		//buttons = new List<GameObject> ();
		for (int i = 1; i < buttons.Count; i++) {
			buttons [i].SetActive (false);
		}

	}
	
	// Update is called once per frame
	void Update () {


		//Keep moving select button while button pushing
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


		}


		//Stop moving select when finish pushing button
		if (Input.GetButtonUp ("Up") || Input.GetButtonUp("Down")) {
			timeSpan = 0f;
			oneButton = false;
		}

		//Select menu
		if (Input.GetKeyDown (KeyCode.Return)) {

			switch (current) {
			case 0:
				SceneManager.LoadScene (1);
				break;

			case 1:
				newMenu.SetActive (true);
				text_muki.SetActive (false);
				Menu.SetActive (false);
				break;
			case 2:
				break;
			case 3:
				break;
			case 4:
				Application.Quit ();
				break;
			}

		}

	}

	//When push 'up' button
	void KeyUp(){

		buttons [current].SetActive (false);

		if (current == 0)
			current = buttons.Count - 1;
		else
			current--;

		buttons [current].SetActive (true);

	}

	//When push 'down' button
	void KeyDown(){

		buttons [current].SetActive (false);

		if (current == buttons.Count - 1)
			current = 0;
		else
			current++;

		buttons [current].SetActive (true);
	}


	//Delay for select moving during pushing button
	IEnumerator ButtonDown(){

		yield return new WaitForSeconds (0.2f);

		isButtonDown = false;
	}

}
