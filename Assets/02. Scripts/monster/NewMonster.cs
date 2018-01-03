using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewMonster : MonoBehaviour {

	public int pathNum = 1;
	public int left_node, right_node;
	public float speed;

	public GameObject item;

	PatrolPos pat;
	Rigidbody rd;
	bool unBeatTime = false;
	public int monsterHealth = 3;

	public bool dirRight = true;
	int current_node;
	public bool isJumpingMonster;
	bool isJumping = false;
	int jumpCount = 0;


	// Use this for initialization
	void Start () {

		switch (pathNum) {
		case 1:
			pat = GameObject.Find ("Patrols1_R").GetComponent<PatrolPos> ();
			break;
		case 2:
			pat = GameObject.Find ("Patrols2_R").GetComponent<PatrolPos> ();
			break;
		case 3:
			pat = GameObject.Find ("Patrols2_L").GetComponent<PatrolPos> ();
			break;
		default:
			pat = GameObject.Find ("Patrols3_R").GetComponent<PatrolPos> ();
			break;
		}

		current_node = left_node;
		rd = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {




		if (dirRight) {
			Vector3 dir = Vector3.right * pat.patrolPostition [current_node + 1].x + Vector3.up * transform.position.y + Vector3.forward * pat.patrolPostition [current_node + 1].z;
			transform.position = Vector3.MoveTowards (transform.position, dir, speed * Time.deltaTime);

			if (Vector3.Distance (Vector3.right * transform.position.x, Vector3.right * pat.patrolPostition [current_node + 1].x) <= 0.1f &&
			    Vector3.Distance (Vector3.forward * transform.position.z, Vector3.forward * pat.patrolPostition [current_node + 1].z) <= 0.1f) {

				current_node++;

				if (current_node >= right_node) {
					dirRight = false;
				}

			}
		
		}

		else {
			Vector3 dir = Vector3.right * pat.patrolPostition [current_node - 1].x + Vector3.up * transform.position.y + Vector3.forward * pat.patrolPostition [current_node - 1].z;
			transform.position = Vector3.MoveTowards (transform.position, dir, speed * Time.deltaTime);

			if (Vector3.Distance (Vector3.right * transform.position.x, Vector3.right * pat.patrolPostition [current_node - 1].x) <= 0.1f &&
				Vector3.Distance (Vector3.forward * transform.position.z, Vector3.forward * pat.patrolPostition [current_node - 1].z) <= 0.1f) {

				current_node--;

				if (current_node <= left_node) {
					dirRight = true;
				}

			}
		}



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
				} else if (monsterHealth <= 0) {
					Time.timeScale = 0.1f;
					Invoke ("UnPause", 0.02f);

				}

			}
		}
		if (isJumpingMonster) {
			//땅인지 체크
			if (col.gameObject.tag.Equals ("Land")) {

				if (isJumping == false && jumpCount < 1) {
					jumpCount++;
					rd.AddForce (Vector3.up * 15, ForceMode.Impulse);
					StartCoroutine ("Delay");
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

	IEnumerator Delay(){
		yield return new WaitForSeconds (0.2f);
		isJumping = false;
		jumpCount = 0;
	}

	void UnPause(){
		Time.timeScale = 1;
		if(isJumpingMonster)
			Instantiate (item, rd.position + (Vector3.up * 3), Quaternion.identity);

		Destroy (gameObject);
	}


}
