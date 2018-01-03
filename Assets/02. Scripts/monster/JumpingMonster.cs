using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingMonster : MonoBehaviour {

	private bool isJumping = false;
	public GameObject player;
	private Rigidbody rigd;
	private int jumpCount = 0;
	public int Health = 2;
	private bool unBeatTime = false;


	// Use this for initialization
	void Start () {
		rigd = GetComponent<Rigidbody> ();

	}
	
	// Update is called once per frame
	void Update () {

		Vector3 dir = Vector3.right * 8 * Time.deltaTime;

		if (player.transform.position.x < transform.position.x)
			dir *= -1;

		if (Vector3.Distance (player.transform.position, transform.position) < 25) {

			rigd.MovePosition (transform.position + dir);

		}


	}


	void OnCollisionEnter (Collision col)
	{

		//땅인지 체크
		if (col.gameObject.tag.Equals ("Land")) {
			
			if (isJumping == false && jumpCount < 1) {
				jumpCount++;
				rigd.AddForce (Vector3.up * 15, ForceMode.Impulse);
				StartCoroutine("Delay");
			}

		}

		if (col.gameObject.tag.Equals ("Bullet")) {

			if (!unBeatTime) {
				Health--;
				SoundManager.instance.Damaged ();
				Camera.main.GetComponent<FollowCam1> ().ShakeCamera (0.1f);

				if (Health <= 0)
					Destroy (gameObject);
				
				if (Health > 0) {
					unBeatTime = true;
					StartCoroutine ("UnBeatTime");
				}
			}

		}
	}

	IEnumerator Delay(){
		yield return new WaitForSeconds (0.2f);
		isJumping = false;
		jumpCount = 0;
	}


	IEnumerator UnBeatTime()
	{
		int time = 0;

		while (time < 5) {

			yield return new WaitForSeconds (0.05f);
			time++;
		}


		unBeatTime = false;
	}
}
