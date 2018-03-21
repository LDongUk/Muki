using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour {

	public float height = 5f;
	public float distance = -50f;
	public GameObject player;

	public float x_leftLimit;
	public float x_rightLimit;
	public float y_upperLimit;
	public float y_lowLimit;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (x_leftLimit < player.transform.position.x && player.transform.position.x < x_rightLimit
		    && y_lowLimit < player.transform.position.y && player.transform.position.y < y_upperLimit) {

			Move (player.transform.position.x, player.transform.position.y + height);
		} 
		//left & low
		else if (player.transform.position.x <= x_leftLimit && player.transform.position.y <= y_lowLimit) {
			Move (x_leftLimit, y_lowLimit);
		}
		//left & upper
		else if (player.transform.position.x <= x_leftLimit && y_upperLimit <= player.transform.position.y) {
			Move (x_leftLimit, y_upperLimit);
		}
		//right & low
		else if (x_rightLimit <= player.transform.position.x && player.transform.position.y <= y_lowLimit) {
			Move (x_rightLimit, y_lowLimit);
		}
		//right & upper
		else if (x_rightLimit <= player.transform.position.x && y_upperLimit <= player.transform.position.y) {
			Move (x_rightLimit, y_upperLimit);
		}
		// left
		else if (player.transform.position.x <= x_leftLimit) {

			Move (x_leftLimit, player.transform.position.y + height);
		} 
		//right
		else if (x_rightLimit <= player.transform.position.x) {

			Move (x_rightLimit, player.transform.position.y + height);
		} 
		//low
		else if (player.transform.position.y <= y_lowLimit) {

			Move (player.transform.position.x, y_lowLimit);
		} 
		//upper
		else if (y_upperLimit <= player.transform.position.y) {

			Move (player.transform.position.x, y_upperLimit);
		}

	
	}




	// ===================================================== [Move logic]

	void Move(float horizontal, float vertical){
		Vector3 camPos = (Vector3.right * horizontal) + (Vector3.up * vertical) + (Vector3.forward * distance);

		transform.position = Vector3.Lerp(transform.position, camPos, 10 * Time.deltaTime);
	}

}
