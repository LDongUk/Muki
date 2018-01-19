using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveAndLoad : MonoBehaviour {

	public static PlayerData LocalCopyOfData;


	//Save data at Saves/save.sav file
	public static void Save(){

		if (!Directory.Exists ("Saves"))
			Directory.CreateDirectory ("Saves");

		BinaryFormatter formatter = new BinaryFormatter ();
		FileStream saveFile = File.Create ("Saves/save.sav");

		LocalCopyOfData = PlayerCtrl.playerData;

		formatter.Serialize (saveFile, LocalCopyOfData);

		saveFile.Close ();
	}

	//Load data at Saves/save.sav file
	public static void Load(){

		BinaryFormatter formatter = new BinaryFormatter ();
		FileStream saveFile = File.Open ("Saves/save.sav", FileMode.Open);

		LocalCopyOfData = (PlayerData)formatter.Deserialize (saveFile);
		PlayerCtrl.playerData = LocalCopyOfData;
		saveFile.Close ();
	
	}
}
	