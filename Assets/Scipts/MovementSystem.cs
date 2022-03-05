using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementSystem : MonoBehaviour {

	
	public float jumpHeight;	
	public float upwardVelocity;
	public float gravity;

	public float moveSpeed = 5f;
	public float rampuptime = 0.1f;
	public float verticalAxis;
	public float horizontalAxis;
	public float currentspeedhorizontal;
	public float currentspeedvertical;
	public float speedsmoothvelocity1;	//wird gebraucht um die smoothness der Bewegung zu speichern, agiert wie eine Art laufvariable für die Funktion Mathf.SmoothDamp
	public float speedsmoothvelocity2;
	
	private CharacterController cc;

	void Start () 
	{	
		cc = GetComponent<CharacterController>();	
		jumpHeight = 1f;
		gravity = -30f;	
	}
	
	// Update is called once per frame
	void Update ()
	 {
		verticalAxis = Input.GetAxisRaw("Vertical");
		horizontalAxis = Input.GetAxisRaw("Horizontal");

		if (Input.GetKeyDown(KeyCode.Space))
			jump();
		if (Input.GetKeyDown(KeyCode.LeftShift))
            StartSprint();
        if (Input.GetKeyUp(KeyCode.LeftShift))
            StopSprint();
	}

	private void FixedUpdate() // Es ist besser alle Bewegungen im FixedUpdate zu machen, da es nicht jeden Frame passiert => mehr Zeit
    {
		//erhöht die geschwindigkeit langsam, was der Bewegung ein smootheres gefühl gibt
		currentspeedhorizontal = Mathf.SmoothDamp (currentspeedhorizontal , horizontalAxis*moveSpeed, ref speedsmoothvelocity1, rampuptime);
		currentspeedvertical = Mathf.SmoothDamp (currentspeedvertical , verticalAxis*moveSpeed, ref speedsmoothvelocity2, rampuptime);	
		//float currentspeedverticalrounded = Mathf.Round(currentspeedvertical*1000)/1000;
		//float currentspeedhorizontalrounded = Mathf.Round(currentspeedhorizontal*1000)/1000;

		upwardVelocity += Time.deltaTime * gravity;
		
		cc.Move((transform.forward * currentspeedvertical + transform.right * currentspeedhorizontal + transform.up * upwardVelocity) * Time.deltaTime);
		// ein problem/feature ist, dass wenn man diagonal geht, schneller ist alls wenn man einfach gerade aus geht. Das ist weil kein .normalised da ist. vielleicht wird das noch geändert.
		if (cc.isGrounded) 
			upwardVelocity = 0;	
    }

	void StartSprint()
    {
        moveSpeed = moveSpeed * 1.5f;
    }

    void StopSprint()
    {
        moveSpeed = moveSpeed / 1.5f;
    }

	void jump()
	{
		if (cc.isGrounded)
		{
			float jumpp = Mathf.Sqrt(-2 * gravity * jumpHeight);
			upwardVelocity = jumpp;
		}
	}

}
