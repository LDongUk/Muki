using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerCtrl : MonoBehaviour {


	Rigidbody2D rigid;
	SpriteRenderer render;
	Animator animator;


	//move
	public float moveSpeed = 15f;
	Vector3 moveDir = Vector3.right;

	//jump
	public float jumpPow = 40f;
	bool isJumping = false;
	int maxJump = 1;
	int jumpCount = 0;

	//dash
	public float dashSpeed;
	bool isDash = false;
	bool isDashDelay = false;


	//-------------------------------------------------------[Event Fuctions]


	// Use this for initialization
	void Start () {

		rigid = gameObject.GetComponent<Rigidbody2D> ();
		render = gameObject.GetComponent<SpriteRenderer> ();
		animator = gameObject.GetComponentInChildren<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {

		//move
		if (Input.GetAxisRaw ("Horizontal") == 0) {
			animator.SetBool ("isMoving", false);

		}
		else if (Input.GetAxisRaw ("Horizontal") < 0) {
			animator.SetInteger ("Direction", -1);
			animator.SetBool ("isMoving", true);
			moveDir = Vector3.left;
		} else if (Input.GetAxisRaw ("Horizontal") > 0) {
			animator.SetInteger ("Direction", 1);
			animator.SetBool ("isMoving", true);
			moveDir = Vector3.right;
		}


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

		Vector3 moveVelocity = Vector3.zero;

		if (Input.GetAxisRaw ("Horizontal") == 0) {
			animator.SetBool ("isMoving", false);
			return;
		}
		else{
			animator.SetBool ("isMoving", true);
			if (Input.GetAxisRaw ("Horizontal") < 0) {
				moveVelocity = Vector3.left;
				render.flipX = true;
			} else if (Input.GetAxisRaw ("Horizontal") > 0) {
				moveVelocity = Vector3.right;
				render.flipX = false;
			}
		}
		transform.position += moveVelocity * moveSpeed * Time.deltaTime;
	
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

	//Attach Event
	void OnTriggerEnter2D (Collider2D other){

		//jump count reset
		if (other.gameObject.tag == "Floor") {
			jumpCount = 0;
			isJumping = false;
		}

	}



	//-------------------------------------------------------[Coroutine Function]
			//-------------------------------------------------------[Dash]

	//Dash logic
	IEnumerator DashCoroutine(){
		
		int dashLoop = 8;

		rigid.velocity = Vector3.zero;
		rigid.gravityScale = 0f;

		Vector3 dashDir = moveDir;
		for (int i = 0; i < dashLoop; i++) {
			transform.position += dashDir * dashSpeed * Time.deltaTime;
			yield return new WaitForSeconds(0.015f);
		}
		rigid.gravityScale = 1f;
		rigid.velocity = Vector3.zero;

	}


	//Dash delay 0.5 seconds.
	IEnumerator DashDelay(){

		int delayTime = 5;

		for (int i = 0; i < delayTime; i++)
			yield return new WaitForSeconds (0.1f);

		isDashDelay = false;
	}
			//-------------------------------------------------------[]


}
 