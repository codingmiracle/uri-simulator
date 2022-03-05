using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugHight : MonoBehaviour
{

    public float jumpheight;
    public bool hightmeasured;
    public float timejumped;

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
            timejumped += Time.deltaTime;
			if(transform.localPosition.y >= jumpheight)
            {
               jumpheight = transform.localPosition.y;
            }
		}
        if(cc.isGrounded && hightmeasured == false)
        {
            //Debug.Log(jumpheight-1.08);
            //Debug.Log(timejumped);
            hightmeasured = true;
            jumpheight = 0;
            timejumped = 0;
        }
    }
}
