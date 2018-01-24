using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class SetNewMenu : MonoBehaviour
{
	public List<GameObject> newGames;
	public static PlayerData playerData;

	// Use this for initialization
	void Start ()
	{


		for (int i = 1; i < 5; i++) {
			if (File.Exists ("Saves/save" + i.ToString () + ".sav")) {
				SaveAndLoad.SetTitleMenu (i);

				Text nodata = GameObject.Find ("NoData" + i.ToString ()).GetComponent<Text> ();
				nodata.text = "Save Number : " + playerData.saveNumber.ToString ();

			}
		}

	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
}

