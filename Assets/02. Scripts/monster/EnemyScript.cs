using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour {

	bool unBeatTime = false;
	private Rigidbody rd;
	public int monsterHealth = 2;

	// Use this for initialization
	void Start () {
		rd = GetComponent<Rigidbody> ();	
	}


	void OnCollisionEnter(Collision col){
		rd.velocity = Vector3.zero;
		if (!unBeatTime) {
			if (col.gameObject.tag.Equals ("Bullet")) {
				monsterHealth--;

				SoundManager.instance.Damaged ();
				Camera.main.GetComponent<FollowCam>().ShakeCamera(0.1f);


				if (monsterHealth > 0) {
					unBeatTime = true;
					StartCoroutine ("UnBeatTime");
				} else if (monsterHealth == 0) {

					//					Destroy (gameObject);

					Time.timeScale = 0.1f;
					Invoke ("UnPause", 0.02f);

				}

			}
		}
	}

	//무적시간
	IEnumerator UnBeatTime()
	{
		int time = 0;

		while (time < 5) {

			yield return new WaitForSeconds (0.05f);
			time++;
		}


		unBeatTime = false;
	}

	void UnPause(){
		Time.timeScale = 1;
		Destroy (gameObject);
	}
}
