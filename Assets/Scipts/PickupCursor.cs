using UnityEngine;
using System.Collections;
using UnityEngine.UI; 

public class PickupCursor : MonoBehaviour
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

        Debug.DrawRay(transform.position , transform.forward * 3.5f, Color.blue);

        if (Physics.Raycast(transform.position,  transform.forward, out hit, 3.5f) && (hit.collider.CompareTag("Pickup")))
        {
            print("Found an object - distance: " + hit.distance);
            Default.SetActive(false);
            Hand.SetActive(true);
        }
        else
        {
            Hand.SetActive(false);
            Default.SetActive(true);
        }

    }
}