using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementSystem : MonoBehaviour 
{
	
	private float jumpHeight = 1f;	
	private float upwardVelocity;
	private float gravity = -30f;

	//different movement speeds
	private float speedWalk = 5f;
	private float speedSneak = 2.5f;
	private float speedSprint = 7.5f;

	public bool walking;
	public bool sneaking;
	public bool sprinting;

	private float wantedHeight;
	private float currentHeight;

	private float moveSpeed = 5f;
	private float rampuptime = 0.1f;
	private float sneakdowntime = 0.15f;
	private float verticalAxis;
	private float horizontalAxis;
	private float currentspeedhorizontal;
	private float currentspeedvertical;
	private float speedsmoothvelocity1;	//wird gebraucht um die smoothness der Bewegung zu speichern, agiert wie eine Art laufvariable für die Funktion Mathf.SmoothDamp
	private float speedsmoothvelocity2;
	
	private CharacterController cc;
	private Keybinds k;

	void Start () 
	{	
		cc = GetComponent<CharacterController>();
		k = GameObject.FindObjectOfType<Keybinds> ();	
		walking = true;
		wantedHeight = 2;
		currentHeight = 2;
	}
	
	// Update is called once per frame
	void Update ()
	 {
		verticalAxis = Input.GetAxisRaw("Vertical");
		horizontalAxis = Input.GetAxisRaw("Horizontal");
		// Jump
		if (Input.GetKeyDown(k.jumpKey))
			Jump();
		// Sprint
		if (Input.GetKeyDown(k.sprintKey) && !sneaking)
			StartSprint();   
        /*if (Input.GetKeyUp(KeyCode.LeftShift) && sprinting)
            StartWalk();*/
		// Sneak
		if (Input.GetKeyDown(k.sneakKey) && !sprinting)
 			StartSneak();
		if (!Input.GetKey(k.sprintKey) && !Input.GetKey(k.sneakKey) && (sneaking || sprinting))
			StartWalk();
        /*if (Input.GetKeyUp(KeyCode.LeftControl) && sneaking)
			StartWalk();*/
			
		//Debug.DrawRay(transform.position+(Vector3.up*0.2f) , Vector3.up * 1.6f, Color.blue);
		//transform.position + (Vector3.up*0.1f),  Vector3.up, 1.4f)
		//Physics.Raycast(transform.position + (Vector3.up*0.1f),  (Vector3.up*0.1f))
            
	}

	private void FixedUpdate() // Es ist besser alle Bewegungen im FixedUpdate zu machen, da es nicht jeden Frame passiert => mehr Zeit
    {
		//erhöht die geschwindigkeit langsam, was der Bewegung ein smootheres gefühl gibt
		currentspeedhorizontal = Mathf.SmoothDamp (currentspeedhorizontal , horizontalAxis*moveSpeed, ref speedsmoothvelocity1, rampuptime);
		currentspeedvertical = Mathf.SmoothDamp (currentspeedvertical , verticalAxis*moveSpeed, ref speedsmoothvelocity2, rampuptime);	
		
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
		wantedHeight = 1.8f;
        moveSpeed = speedSprint;
    }

    void StartWalk()
    {
		if(sneaking && Physics.Raycast(transform.position + (Vector3.up*0.2f),  Vector3.up, 1.6f))
			return;

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
		float center = 1 - newheight / 2;	//center wird nach oben geshiftet, damit springen und crouchen einen höheren sprung ermöglichen.
		if (cc.height < wantedHeight)		//wenn vergrößerung, wird höhe angepasst, dammit man nicht in den Boden glitcht, und die Bewegung smooth ist
		{
			transform.position = transform.position + (cc.center - Vector3.Lerp(cc.center, new Vector3(0,center,0),sneakdowntime))*2;
		}
		cc.height = Mathf.Lerp(cc.height, newheight, sneakdowntime);
		cc.center = Vector3.Lerp(cc.center, new Vector3(0,center,0),sneakdowntime);
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

