using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class NewSelectCtrl : MonoBehaviour
{

	public List<GameObject> buttons;
	public GameObject Menu, text_muki;
	public GameObject SaveSelect;
	int current = 0;


	// Use this for initialization
	void Start ()
	{
		for (int i = 1; i < buttons.Count; i++)
			buttons [i].SetActive (false);
	}
	
	// Update is called once per frame
	void Update ()
	{
		
		if(Input.GetButtonDown("Up"))
			KeyUp();

		if (Input.GetButtonDown ("Down"))
			KeyDown ();

		//Select the menu
		if (Input.GetKeyDown (KeyCode.Return)) {
			switch (current) {
			case 0:
			case 1:
			case 2:
			case 3:

//				if (File.Exists ("Saves/save" + (current + 1).ToString () + ".sav")) {
//					/*
//					  Ask really?
//					*/
//				} else {
//					PlayerData playerdata = new PlayerData ();
//					playerdata.saveNumber = current + 1;
//					print (current + 1);
//					SaveAndLoad.Save (current + 1);
//					SceneManager.LoadScene (1);
//				}

				SaveAndLoad.Save (current + 1);
				SceneManager.LoadScene (1);

				break;
			case 4:
				//Back to the main title
				text_muki.SetActive (true);
				Menu.SetActive (true);
				SaveSelect.SetActive (false);
				ButtonReset ();
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

	void ButtonReset(){

		GameObject.Find ("Start").GetComponent<ButtonSelect> ().isBlink = false;
		GameObject.Find ("Start").GetComponent<ButtonSelect> ().increase = false;
		GameObject.Find ("Start").GetComponent<ButtonSelect> ().alpha = 250;

		GameObject.Find ("New").GetComponent<ButtonSelect> ().isBlink = false;
		GameObject.Find ("New").GetComponent<ButtonSelect> ().increase = false;
		GameObject.Find ("New").GetComponent<ButtonSelect> ().alpha = 250;

		GameObject.Find ("Load").GetComponent<ButtonSelect> ().isBlink = false;
		GameObject.Find ("Load").GetComponent<ButtonSelect> ().increase = false;
		GameObject.Find ("Load").GetComponent<ButtonSelect> ().alpha = 250;

		GameObject.Find ("Option").GetComponent<ButtonSelect> ().isBlink = false;
		GameObject.Find ("Option").GetComponent<ButtonSelect> ().increase = false;
		GameObject.Find ("Option").GetComponent<ButtonSelect> ().alpha = 250;

		GameObject.Find ("Exit").GetComponent<ButtonSelect> ().isBlink = false;
		GameObject.Find ("Exit").GetComponent<ButtonSelect> ().increase = false;
		GameObject.Find ("Exit").GetComponent<ButtonSelect> ().alpha = 250;

	}
}

