using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementSystem : MonoBehaviour {

	public float moveSpeed = 5f;
	public float jumpHeight = 1f;	// jumpHieght indicates, how many Units high the player can cofortably jump.
	public float jumpBoost;
	public float verticalAxis;
	public float horizontalAxis;
	public float upwardVelocity;
	public float gravity;
	public float currentspeedhorizontal;
	public float currentspeedvertical;
	public float speedsmoothvelocity1;
	public float speedsmoothvelocity2;

	public bool jumping;
	
	private CharacterController cc;

	// Use this for initialization
	void Start () 
	{	
		cc = GetComponent<CharacterController>();	
		gravity = -1f;
		jumping = false;
		jumpHeight = 1f;	// jumpHieght indicates, how many Units high the player can cofortably jump.
		InitJumpBoost();
	}
	
	// Update is called once per frame
	void Update ()
	 {
		verticalAxis = Input.GetAxisRaw("Vertical");
		horizontalAxis = Input.GetAxisRaw("Horizontal");
		if (Input.GetKeyDown(KeyCode.Space) && cc.isGrounded)
		{
			jumping = true;
		}
			

		if (Input.GetKeyDown(KeyCode.LeftShift))
            StartSprint();
        if (Input.GetKeyUp(KeyCode.LeftShift))
            StopSprint();
	}

	private void FixedUpdate() // Es ist besser alle Bewegungen im FixedUpdate zu machen, da es nicht jeden Frame passiert => mehr Zeit
    {
		InitJumpBoost();
		
			currentspeedhorizontal = Mathf.SmoothDamp (currentspeedhorizontal , horizontalAxis, ref speedsmoothvelocity1, 0.1f);
			currentspeedvertical = Mathf.SmoothDamp (currentspeedvertical , verticalAxis, ref speedsmoothvelocity2, 0.1f);
			currentspeedvertical = Mathf.Round(currentspeedvertical*1000)/1000;
			currentspeedhorizontal = Mathf.Round(currentspeedhorizontal*1000)/1000;
		
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
			upwardVelocity += jumpBoost;
			jumping = false;
		}

		
		cc.Move((transform.forward * currentspeedvertical + transform.right * currentspeedhorizontal) * moveSpeed * Time.deltaTime + transform.up * upwardVelocity);
		
    }

	private void StartSprint()
    {
        moveSpeed = moveSpeed * 2f;
    }

    private void StopSprint()
    {
        moveSpeed = moveSpeed / 2f;
    }

	private void InitJumpBoost()
	{
		jumpBoost = Mathf.Sqrt(((jumpHeight+0.7f) * -0.1f * gravity)/2);	//Wierd Math, but it works. Trust me
	}


}
