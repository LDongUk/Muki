using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMonster : MonoBehaviour {

	public GameObject spwanCube;
	public GameObject attackBullet;
	public GameObject player;
	public GameObject damagedEffect;
	public GameObject dieCube;
	public GameObject item;

	private bool isShake = false;
	private bool isPattern = false;
	private bool attackDelay = true;
	private int pattern_num = 0;
	private Rigidbody rigd;

	private PatrolPos pat;

	public int bossHealth = 10;
	private bool unBeatTime = false;

	// Use this for initialization
	void Start () {
		rigd = GetComponent<Rigidbody> ();
		pat = GameObject.Find ("Patrols1_R").GetComponent<PatrolPos> ();

		InvokeRepeating ("Attack", 0, 3.0f);
	}
	
	// Update is called once per frame
	void Update () {

//		//StartCoroutine ("Delay");
//		StartCoroutine ("Delay_and_Pattern");
//		if (attackDelay == true) {
//
//			switch (pattern_num) {
//				
//			case 0:
//				Jump ();
//				break;
//			case 1:
//				SpawnCube ();
//				break;
//
//			}
//
//
//		}


		//Invoke("Delay_and_Pattern", 5.0f);
	}

	void Attack(){
		//Random.InitState ((int)Time.time);

		float dist = 100;

		if(player)
			dist = Vector3.Distance (player.transform.position, transform.position);

		if (dist <= 50)
			isPattern = true;

		if (isPattern) {

			pattern_num = Random.Range (0, 100);

			switch (pattern_num % 3) {

			case 0:
				Jump ();
				break;
			case 1:
				SpawnCube ();
				break;
			case 2:
				Attack_Sphear ();
				break;


			}

		}
	}


//	IEnumerator Delay_and_Pattern(){
//
//		if (attackDelay == false) {
//			pattern_num = Random.Range (0, 2);
//			print (Time.time);
//			for(int i = 0; i < 10; i++)
//				yield return new WaitForSeconds (0.3f);
//
//			print (Time.time);
//			attackDelay = true;
//		}
//
//	}

	void Delay_and_Pattern(){

		if (attackDelay == false) {
			pattern_num = Random.Range (0, 10);

			//Debug.Log ("ASdsa");
			attackDelay = true;
		}

	}

	void Jump(){
		rigd.AddForce (Vector3.up * 250f, ForceMode.Impulse);
		//Camera.main.GetComponent<FollowCam>().ShakeCamera(0.5f);
		attackDelay = false;

		isShake = true;

	}

	void SpawnCube(){

//		for (int i = 0; i < 10; i++) {
//			int rand = Random.Range (26, 47);
//			Instantiate (spwanCube, pat.patrolPostition [rand] + (Vector3.up * 20), rigd.rotation);
//		}
		StartCoroutine("spawnDelay");
		SoundManager.instance.DisappearCube ();
		attackDelay = false;
	}

	void Attack_Sphear(){
		Instantiate (attackBullet, rigd.position + (Vector3.up * 15), rigd.rotation);
		SoundManager.instance.AppearShpear ();
		attackDelay = false;
	}


	void OnCollisionEnter(Collision col){
		if (!unBeatTime) {
			if (col.gameObject.tag.Equals ("Bullet")) {
				bossHealth--;
				SoundManager.instance.Damaged ();
				if (damagedEffect != null)
					Instantiate (damagedEffect, transform.position + (Vector3.up * 7), Quaternion.Euler (Vector3.left * 90));

				if (bossHealth > 0) {
					unBeatTime = true;
					StartCoroutine ("UnBeatTime");
				} else if (bossHealth == 0) {

					//					Destroy (gameObject);

					Time.timeScale = 0.1f;
					BossDie ();
					Invoke ("UnPause", 0.02f);

				}

			}
		}

		if(col.gameObject.tag.Equals ("Land")){
			if (isShake) {
				Camera.main.GetComponent<FollowCam>().ShakeCamera(0.8f);
				SoundManager.instance.EarthQuake ();
				isShake = false;
			}
		}
	}

	void BossDie(){
		
		for (int i = 0; i <= 30; i++) {
			Vector3 rand = Random.onUnitSphere * 5.0f;
			Instantiate (dieCube, transform.position + (Vector3.up * 10) + rand, rigd.rotation);
		}

		Instantiate (item, pat.patrolPostition[50] + (Vector3.up * 5), Quaternion.identity);
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

	IEnumerator spawnDelay(){
		for (int i = 0; i < 20; i++) {
			int rand = Random.Range (26, 47);
			Instantiate (spwanCube, pat.patrolPostition [rand] + (Vector3.up * 20), rigd.rotation);
			yield return new WaitForSeconds (0.1f);
		}
	}

	void UnPause(){
		Time.timeScale = 1;
		SoundManager.instance.BossDie ();
		Destroy (gameObject);
	}

}
