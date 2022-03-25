using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementSystem : MonoBehaviour {

	
	public float jumpHeight;	
	public float upwardVelocity;
	public float gravity;

	private float speedWalk = 5f;
	private float speedSneak = 2.5f;
	private float speedSprint = 7.5f;

	public bool walking;
	public bool sneaking;
	public bool sprinting;

	public float wantedHeight;
	public float currentHeight;

	public float moveSpeed = 5f;
	public float rampuptime = 0.1f;
	public float sneakdowntime = 0.15f;
	public float verticalAxis;
	public float horizontalAxis;
	public float currentspeedhorizontal;
	public float currentspeedvertical;
	public float speedsmoothvelocity1;	//wird gebraucht um die smoothness der Bewegung zu speichern, agiert wie eine Art laufvariable für die Funktion Mathf.SmoothDamp
	public float speedsmoothvelocity2;
	public float speedsmoothvelocity3;

	public GameObject cameraHolder;
	public float campos;
	
	private CharacterController cc;

	void Start () 
	{	
		cc = GetComponent<CharacterController>();	
		jumpHeight = 1f;
		gravity = -30f;	
		walking = true;
		wantedHeight = 2;
		currentHeight = 2;
		campos = cameraHolder.transform.position.y;
	}
	
	// Update is called once per frame
	void Update ()
	 {
		verticalAxis = Input.GetAxisRaw("Vertical");
		horizontalAxis = Input.GetAxisRaw("Horizontal");
		// Jump
		if (Input.GetKeyDown(KeyCode.Space))
			Jump();
		// Sprint
		if (Input.GetKeyDown(KeyCode.LeftShift) && !sneaking)
			StartSprint();   
        if (Input.GetKeyUp(KeyCode.LeftShift) && sprinting)
            StartWalk();
		// Sneak
		if (Input.GetKeyDown(KeyCode.LeftControl) && !sprinting)
 			StartSneak();
        if (Input.GetKeyUp(KeyCode.LeftControl) && sneaking)
			StartWalk();
            
	}

	private void FixedUpdate() // Es ist besser alle Bewegungen im FixedUpdate zu machen, da es nicht jeden Frame passiert => mehr Zeit
    {
		//erhöht die geschwindigkeit langsam, was der Bewegung ein smootheres gefühl gibt
		currentspeedhorizontal = Mathf.SmoothDamp (currentspeedhorizontal , horizontalAxis*moveSpeed, ref speedsmoothvelocity1, rampuptime);
		currentspeedvertical = Mathf.SmoothDamp (currentspeedvertical , verticalAxis*moveSpeed, ref speedsmoothvelocity2, rampuptime);	
		//float currentspeedverticalrounded = Mathf.Round(currentspeedvertical*1000)/1000;
		//float currentspeedhorizontalrounded = Mathf.Round(currentspeedhorizontal*1000)/1000;

		if (cc.height != wantedHeight)
		{
			ChangeHeight(wantedHeight);
		}
			

		upwardVelocity += Time.deltaTime * gravity;
		
		cc.Move((transform.forward * currentspeedvertical + transform.right * currentspeedhorizontal + transform.up * upwardVelocity) * Time.deltaTime);
		// ein problem/feature ist, dass wenn man diagonal geht, schneller ist alls wenn man einfach gerade aus geht. Das ist weil kein .normalised da ist. vielleicht wird das noch geändert.
		if (cc.isGrounded) 
			upwardVelocity = 0;	
    }

	void StartSprint()
    {
		walking = false;
		sprinting = true;
        moveSpeed = speedSprint;
    }

    void StartWalk()
    {
		walking = true;
		sneaking = false;
		sprinting = false;
		wantedHeight = 2;
        moveSpeed = speedWalk;
    }

	void StartSneak()
    {
		walking = false;
		sneaking = true;
		wantedHeight = 1;
        moveSpeed = speedSneak;
    }

	void ChangeHeight(float newheight)
	{
		float center = 1 - newheight / 2;
		Vector3 deltaheight = cc.center;
		if (cc.height < wantedHeight)
		{
			transform.position = transform.position + (cc.center - Vector3.Lerp(cc.center, new Vector3(0,center,0),sneakdowntime))*2;
		}
		cc.height = Mathf.Lerp(cc.height, newheight, sneakdowntime);
		cc.center = Vector3.Lerp(cc.center, new Vector3(0,center,0),sneakdowntime);
		deltaheight -= cc.center;
		
		//cameraHolder.transform.position = new Vector3(transform.position.x,transform.position.y + (campos-1) + cc.center.y*2 ,transform.position.z);
		//cameraHolder.transform.position = Vector3.Lerp(cameraHolder.transform.position, new Vector3(cameraHolder.transform.position.x,campos+center,cameraHolder.transform.position.z),0.1f);

		//transform.position = new Vector3(transform.position.x, 3, transform.position.z);
		//transform.position = Vector3.Lerp(transform.position, new Vector3(0,center,0),0.1f);
		//transform.center = Vector3.Lerp(transform.center, new Vector3(0,center,0),0.1f);
		
		
			currentHeight = cc.height;
		
	}

	void Jump()
	{
		if (cc.isGrounded)
		{
			float jumpp = Mathf.Sqrt(-2 * gravity * jumpHeight);
			upwardVelocity = jumpp;
		}
	}

}