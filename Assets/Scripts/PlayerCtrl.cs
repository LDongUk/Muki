using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerCtrl : MonoBehaviour {

	public float moveSpeed = 5f;
	public float jumpPow = 5f;

	Rigidbody2D rigid;
	SpriteRenderer render;
	Animator animator;

	Vector3 moveDir;
	bool isJumping = false;
	int maxJump = 1;
	int jumpCount = 0;


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
		} else if (Input.GetAxisRaw ("Horizontal") > 0) {
			animator.SetInteger ("Direction", 1);
			animator.SetBool ("isMoving", true);
		}


		//jump
		if (Input.GetButtonDown("Jump"))
			isJumping = true;

		//print (rigid.velocity);
	}

	void FixedUpdate(){
		
		Move ();
		Jump ();
	}

	//-------------------------------------------------------[Movement Function]
	
	void Move(){

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

	void Jump(){

		if (!isJumping || jumpCount >= maxJump)
			return;
		

		rigid.velocity = Vector2.zero;

		Vector2 jumpVelocity = new Vector2 (0, jumpPow);
		rigid.AddForce (jumpVelocity, ForceMode2D.Impulse);
		jumpCount++;
	}

	//-------------------------------------------------------[Collision Function]

	//Attach Event
	void OnTriggerEnter2D (Collider2D other){

		if (other.gameObject.tag == "Floor") {
			jumpCount = 0;
			isJumping = false;
		}

	}
}
