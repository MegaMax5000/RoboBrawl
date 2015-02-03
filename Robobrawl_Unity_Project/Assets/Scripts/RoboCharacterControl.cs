using UnityEngine;
using System.Collections;

public class RoboCharacterControl : MonoBehaviour 
{
	
	CharacterController cc;

	Animator anim;

	public bool isGrounded;
	public float upDownRange = 40.0f;
	public float mouseSpeed = 2.5f;
	public float slowMovementSpeed = 12;
	public float jumpPower = 1f;
	float movementSpeed;
	float currForward = 0.0f; 
	float currSide = 0.0f;
	float forwardSpeed = 0.0f;
	float sideSpeed = 0.0f;
	float vertRot = 0;
	float vertVelocity = 0;

	bool jumpPressed = false;
	bool canMove = true;

	// Use this for initialization
	void Start () 
	{
		anim = GetComponent<Animator> ();
		cc = GetComponent<CharacterController> ();

		movementSpeed = slowMovementSpeed;
		isGrounded = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(canMove)
		{
			if (cc.isGrounded)
				isGrounded = true;
			else
				isGrounded = false;

			anim.SetBool ("GROUNDED", isGrounded);

			// Rotation

			float rotLR = Input.GetAxis ("Mouse X") * mouseSpeed;
			transform.Rotate (0, rotLR, 0);
			
			vertRot -= Input.GetAxis ("Mouse Y") * mouseSpeed * 0.7f;
			vertRot = Mathf.Clamp (vertRot, -upDownRange, upDownRange);
			Camera.main.transform.localRotation = Quaternion.Euler (vertRot, 0, 0);

			// Movement
			
			currForward = Input.GetAxis ("Vertical");
			currSide = Input.GetAxis ("Horizontal");
			
			if (Input.GetAxis("Vertical") > 0.1f || Input.GetAxis("Vertical") < -0.1f
			    || Input.GetAxis ("Horizontal") > 0.1f || Input.GetAxis ("Horizontal") < -0.1f) 
			{
				anim.SetBool ("RUN", true);
			} 
			else
			{
				anim.SetBool ("RUN", false);
			}
			
			forwardSpeed = currForward * movementSpeed * Time.deltaTime; 
			sideSpeed = currSide * movementSpeed * Time.deltaTime;

			if(isGrounded)
			{
				vertVelocity = -.1f;
				if(Input.GetKeyDown("space") && !jumpPressed)
				{
					jumpPressed = true;
					StartCoroutine(Jump ());
				}
			}
			else
			{
				jumpPressed = false;
				anim.SetBool ("JUMP", false);
				vertVelocity += Physics.gravity.y * Time.deltaTime;
			}
			Vector3 speed = new Vector3 (sideSpeed, vertVelocity, forwardSpeed);
			speed = transform.rotation * speed;
			cc.Move(speed);
		}
	}

	IEnumerator Jump()
	{
		currForward = Input.GetAxis ("Vertical");
		currSide = Input.GetAxis ("Horizontal");
		forwardSpeed = currForward * movementSpeed * Time.deltaTime; 
		sideSpeed = currSide * movementSpeed * Time.deltaTime;

		anim.SetBool ("JUMP", true);
		canMove = false;
		yield return new WaitForSeconds (.35f);
		canMove = true;
		vertVelocity = jumpPower;
		Vector3 speed = new Vector3 (sideSpeed, vertVelocity, forwardSpeed);
		speed = transform.rotation * speed;
		cc.Move(speed);
	}
}











