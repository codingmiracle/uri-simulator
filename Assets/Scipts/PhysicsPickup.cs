using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsPickup : MonoBehaviour
{

         public LayerMask PickupMask;
         public Camera PlayerCamera;
         public Transform PickupTarget;
         public float PickupRange;
         private Rigidbody CurrentObject;
         private RaycastHit hit;
         public Vector3 DirectionToPoint;

         public bool pictrue;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
         
        if(Input.GetMouseButtonDown(0))
        {
            pictrue = true;
        }
        else
        {
            pictrue = false;
        }
    }

    private void FixedUpdate()
    {
        if(pictrue == true)
        {

            if(CurrentObject)
            {
                CurrentObject.useGravity = true;
                CurrentObject = null;
                return;
            }

            Ray CameraRay = PlayerCamera.ViewportPointToRay(new Vector3(2f, 2f, 2f));
            if (Physics.Raycast(transform.position,  transform.forward, out hit, 5f) && (hit.collider.CompareTag("Pickup")))
            {
                CurrentObject = hit.rigidbody;
                CurrentObject.useGravity = false;
            }
        }
        if(CurrentObject != null)
        {
            DirectionToPoint = PickupTarget.position - CurrentObject.position;
            float DistanceToPoint = DirectionToPoint.magnitude;
            CurrentObject.velocity = DirectionToPoint * 12f * DistanceToPoint;
        }
        else
        {
            DirectionToPoint = Vector3.zero;
        }
        
        
       
    }


}