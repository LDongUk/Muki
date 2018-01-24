using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class PlayerCtrl : MonoBehaviour {


	Rigidbody2D rigid;
	SpriteRenderer render;
	Animator animator;


	//Move
	public float moveSpeed = 15f;
	Vector3 moveDir = Vector3.right;


	//Jump
	public float jumpPow = 40f;
	bool isJumping = false;
	int maxJump = 1;
	int jumpCount = 0;

	//Dash
	public float dashSpeed;
	bool isDash = false;
	bool isDashDelay = false;
	public float leftEnd;
	public float rightEnd;

	//Damaged
	bool isUnBeatTime = false;

	//Player Data
	public static PlayerData playerData;

	//Option Menu
	public GameObject canvas;

	//Animation
	float anim_timer = 0f;
	float standing_time_delay = 0.1f;

	//-------------------------------------------------------[Event Fuctions]


	// Use this for initialization
	void Start () {
		
		rigid = gameObject.GetComponent<Rigidbody2D> ();
		render = gameObject.GetComponent<SpriteRenderer> ();
		animator = gameObject.GetComponentInChildren<Animator> ();
		playerData = new PlayerData ();
		render.flipX = true;

//		if (File.Exists ("Saves/save.sav")) {
//		
//			SaveAndLoad.Load ();
//			transform.position = new Vector2 (playerData.positionX, playerData.positionY);
//		}
	}
	
	// Update is called once per frame
	void Update () {


		//Jump
		if (Input.GetAxisRaw("Jump") > 0)
			isJumping = true;

		if (Input.GetButtonUp ("Jump")) {
			if(rigid.velocity.y >= 0)
				rigid.velocity = Vector3.zero;
		}
		//Dash
		if (Input.GetButtonDown ("Dash")) {
			if(!isDashDelay)
				isDash = true;
		}


		//Save (Test)
		if (Input.GetButtonDown ("Save")) {
			
			playerData.positionX = transform.position.x;
			playerData.positionY = transform.position.y;
			playerData.SceneID = SceneManager.GetActiveScene ().buildIndex;
			SaveAndLoad.Save (playerData.saveNumber);
		}

		//Esc (Call the menu)
		if (Input.GetKeyDown (KeyCode.Escape)) {
			canvas.SetActive (true);
			//GameObject.Find ("Select").transform.position = new Vector2 (GameObject.Find ("Select").transform.position.x, GameObject.Find ("Continue").transform.position.y);
			GameObject.Find ("Canvas").GetComponent<CanvasCtrl> ().isActive = true;
			Time.timeScale = 0f;

		}

	}

	void FixedUpdate(){
		
		Move ();
		Jump ();
		Dash ();
	}



	//-------------------------------------------------------[Movement Function]

	//Move logic
	void Move(){

		if (isDash)
			return;


		if (Input.GetAxisRaw ("Horizontal") == 0) {
			anim_timer += Time.deltaTime;

			if (anim_timer > standing_time_delay) {
				animator.SetBool ("isMoving", false);
				return;
			}
		}
		else{
			animator.SetBool ("isMoving", true);
			anim_timer = 0f;
			if (Input.GetAxisRaw ("Horizontal") < 0) {
				animator.SetInteger ("Direction", -1);
				moveDir = Vector2.left;
				render.flipX = false;
			} else if (Input.GetAxisRaw ("Horizontal") > 0) {
				animator.SetInteger ("Direction", 1);
				moveDir = Vector2.right;
				render.flipX = true;
			}
		}
		transform.position += moveDir * moveSpeed * Time.deltaTime;

	}

	//Jump logic
	void Jump(){

		if (!isJumping || jumpCount >= maxJump)
			return;
		

		rigid.velocity = Vector2.zero;

		Vector2 jumpVelocity = new Vector2 (0, jumpPow);
		rigid.AddForce (jumpVelocity, ForceMode2D.Impulse);
		jumpCount++;
	}


	//-------------------------------------------------------[Skill Function]

	void Dash(){

		if (!isDash || isDashDelay)
			return;

		StartCoroutine("DashCoroutine");
		isDash = false;

		isDashDelay = true;
		StartCoroutine ("DashDelay");
	}


	//-------------------------------------------------------[Collision Function]

	//Attach Event when isTrigger is true
	void OnTriggerStay2D (Collider2D other){

		//Jump count reset
		if (other.gameObject.tag == "Floor") {
			jumpCount = 0;
			isJumping = false;
		}

	
	}


	//Attach Event when isTrigger is false
	void OnCollisionStay2D(Collision2D other){

		//Damaged
		if (other.gameObject.tag == "Obstacle" && !isUnBeatTime) {

			Vector2 attackedVelocity = Vector2.zero;

			if (transform.position.x <= other.gameObject.transform.position.x)
				attackedVelocity = new Vector2 (-10f, 35f);
			else
				attackedVelocity = new Vector2 (10f, 35f);

			rigid.AddForce (attackedVelocity, ForceMode2D.Impulse);

			isUnBeatTime = true;
			StartCoroutine ("UnBeatTime");
		}
	}

	//-------------------------------------------------------[Coroutine Function]
			//-------------------------------------------------------[Dash]

	//Dash logic
	IEnumerator DashCoroutine(){
		
		int dashLoop = 8;

		rigid.velocity = Vector3.zero;
		rigid.gravityScale = 0f;

		for (int i = 0; i < dashLoop; i++) {
			if (leftEnd < transform.position.x && transform.position.x < rightEnd)
				transform.position += moveDir * dashSpeed * Time.deltaTime;
			yield return new WaitForSeconds (0.015f);
			
		}
		rigid.gravityScale = 1f;
		rigid.velocity = Vector3.zero;

		yield return null;
	}


	//Dash delay 0.5 seconds.
	IEnumerator DashDelay(){

		int delayTime = 5;

		for (int i = 0; i < delayTime; i++)
			yield return new WaitForSeconds (0.1f);

		isDashDelay = false;

		yield return null;
	}
			//-------------------------------------------------------[Damaged]

	//No damaged while this coroutine operating
	IEnumerator UnBeatTime(){
		int countTime = 0;

		//Color flicker
		while (countTime < 10) {

			if (countTime % 2 == 0)
				render.color = new Color32 (255, 255, 255, 90);
			else
				render.color = new Color32 (255, 255, 255, 180);

			yield return new WaitForSeconds (0.2f);

			countTime++;
		}

		//Return to original color
		render.color = new Color32 (255, 255, 255, 255);

		isUnBeatTime = false;

		yield return null;
	}

}
 