using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugHight : MonoBehaviour
{

    public float jumpheight;
    public bool hightmeasured;

    private CharacterController cc;
    void Start () 
	{	
		cc = GetComponent<CharacterController>();
        hightmeasured = true;
	}


    // Update is called once per frame
    private void FixedUpdate()
    {
        if (!cc.isGrounded) 
		{
            hightmeasured = false;
			if(transform.localPosition.y >= jumpheight)
            {
               jumpheight = transform.localPosition.y;
            }
		}
        if(cc.isGrounded && hightmeasured == false)
        {
            Debug.Log(jumpheight-1.08);
            hightmeasured = true;
            jumpheight = 0;
        }
    }
}
