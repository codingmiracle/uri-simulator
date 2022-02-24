using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementSystem : MonoBehaviour {

	public float moveSpeed;
	public float jumpHeight;
	public float verticalAxis;
	public float horizontalAxis;
	public float upwardVelocity;
	public float gravity;

	public bool jumping;
	
	private CharacterController cc;

	// Use this for initialization
	void Start () 
	{	
		cc = GetComponent<CharacterController>();
		moveSpeed = 10f;
		jumpHeight = 1f;	// jumpHieght 1 und gravity -1 passen am besten
		gravity = -1f;
		jumping = false;
	}
	
	// Update is called once per frame
	void Update () {
		
		verticalAxis = Input.GetAxisRaw("Vertical");
		horizontalAxis = Input.GetAxisRaw("Horizontal");
		if (Input.GetKeyDown(KeyCode.Space) && cc.isGrounded)
		{
			jumping = true;
		}
	}

	private void FixedUpdate() // Es ist besser alle Bewegungen im FixedUpdate zu machen, da es nicht jeden Frame passiert => mehr Zeit
    {

		if (cc.isGrounded) 
		{
			upwardVelocity = -0.1f;	// Damit der Spieler am Boden bleibt, wenn er am boden ist
		}
		else
		{
			upwardVelocity += Time.deltaTime * gravity;
		}

		if (jumping == true) 
		{
			upwardVelocity += jumpHeight * -0.3f * gravity;
			jumping = false;
		}

		cc.Move((transform.forward * verticalAxis + transform.right * horizontalAxis).normalized * moveSpeed * Time.deltaTime);
		cc.Move(Vector3.up * upwardVelocity);
    }
}
