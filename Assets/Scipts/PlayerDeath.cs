using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    public bool deadFlag;
    // Start is called before the first frame update
    void Start () 
    {
		deadFlag = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.localPosition.y <= -10)
        {
            deadFlag = true;
        }
    }

    private void FixedUpdate()
    {
        if(deadFlag == true)
        {
            Debug.Log("player fell off");
            transform.position = new Vector3(0,2,0);       // Sets Player position to x=0, y=2, z=0
            deadFlag = false;
        }
    }
}
