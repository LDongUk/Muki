using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveAndLoad : MonoBehaviour {

	public static PlayerData LocalCopyOfData;


	//Save data at Saves/save.sav file
	public static void Save(int saveNum){

		if (!Directory.Exists ("Saves"))
			Directory.CreateDirectory ("Saves");

		BinaryFormatter formatter = new BinaryFormatter ();
		FileStream saveFile;
		if (File.Exists ("Saves/save" + saveNum.ToString () + ".sav"))
			saveFile = File.Open ("Saves/save" + saveNum.ToString () + ".sav", FileMode.Open);
		else
			saveFile = File.Create ("Saves/save" + saveNum.ToString() + ".sav");

		PlayerData playerdata = new PlayerData ();
		LocalCopyOfData = playerdata;
		LocalCopyOfData.saveNumber = saveNum;

		formatter.Serialize (saveFile, LocalCopyOfData);

		saveFile.Close ();
	}

	//Load data at Saves/save.sav file
	public static void Load(int saveNum){

		BinaryFormatter formatter = new BinaryFormatter ();
		FileStream saveFile = File.Open ("Saves/save" + saveNum.ToString() + ".sav", FileMode.Open);

		LocalCopyOfData = (PlayerData)formatter.Deserialize (saveFile);
		PlayerCtrl.playerData = LocalCopyOfData;
		saveFile.Close ();
	
	}

	public static void SetTitleMenu(int saveNum){
		BinaryFormatter formatter = new BinaryFormatter ();
		FileStream saveFile = File.Open ("Saves/save" + saveNum.ToString() + ".sav", FileMode.Open);

		LocalCopyOfData = (PlayerData)formatter.Deserialize (saveFile);
		SetNewMenu.playerData = LocalCopyOfData;

		saveFile.Close ();
	}
}
	