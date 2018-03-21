using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonSelect : MonoBehaviour
{
	
	public Image myImage;
	public int alpha;

	public bool increase = false;
	public bool isBlink = false;

	// Use this for initialization
	void Start ()
	{
		myImage = GetComponent<Image> ();
		alpha = 250;
	}
	
	// Update is called once per frame
	void Update ()
	{
		
		if(!isBlink)
			StartCoroutine ("Blink");


	}


	//-------------------------------------------------------[Coroutine Function]

	//Blink the button color
	IEnumerator Blink(){

		isBlink = true;
		int time = 50;



		if (increase) {
			for (int i = 0; i < time; i++) {
				alpha += 5;
				myImage.color = new Color32 (150, 150, 200, (byte) alpha);
				yield return new WaitForSeconds (0.01f);
			}

			increase = false;

		} 

		else {
			for(int i = 0; i < time; i++){
				alpha -= 5;
				myImage.color = new Color32 (150, 150, 200, (byte) alpha);
				yield return new WaitForSeconds (0.01f);
			}

			increase = true;
		}
		isBlink = false;
	}

}

