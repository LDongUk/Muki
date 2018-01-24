using UnityEngine;
using System.Collections;
using System.IO;

public class SetLoadMenu : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
			if (File.Exists ("Saves/save1.sav")) {
				//SaveAndLoad.Load ();
				
			}
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}

