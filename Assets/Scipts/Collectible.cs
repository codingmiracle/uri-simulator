using UnityEngine;
using System.Collections;
using UnityEngine.UI; 

public class Collectible : MonoBehaviour
{

    public GameObject Default;
    public GameObject Hand;

    void Start ()
    {
        Hand.SetActive(false);
        Default.SetActive(true);
    }


    // See Order of Execution for Event Functions for information on FixedUpdate() and Update() related to physics queries
    void FixedUpdate()
    {
        RaycastHit hit;

        Debug.DrawRay(transform.position , transform.forward * 4.5f, Color.blue);

        if (Physics.Raycast(transform.position,  transform.forward, out hit, 4.5f) && (hit.collider.CompareTag("Collectible")))
        {
            //print("Found an object - distance: " + hit.distance);
            Default.SetActive(false);
            Hand.SetActive(true);
        
        if(Input.GetMouseButtonDown(0))
        {
            MeshCollider bc = hit.collider as MeshCollider;    //Das kann fix besser gelöst werden
            if(bc != null)                                    // Mit anderem Collider sollte es auch gehen aber egal
            {                                                //  Nicht ganz safe 
                Destroy(bc.gameObject);
            }
        }

        }
        else
        {
            Hand.SetActive(false);
            Default.SetActive(true);
        }

    }
}