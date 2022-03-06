using UnityEngine;
using System.Collections;
using UnityEngine.UI; 

public class Collectible : MonoBehaviour
{

    public GameObject Default;
    public GameObject Hand;

    private Items items;

    void Start ()
    {
        Hand.SetActive(false);
        Default.SetActive(true);
        items = GameObject.FindObjectOfType<Items> ();
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
                    if(bc.name == "IC")
                    {
                        Debug.Log("IC has been collected");
                        items.hasIC = true;
                    }
                    Destroy(bc.gameObject);
                }
            }
        }
        else if(Physics.Raycast(transform.position,  transform.forward, out hit, 4.5f) && (hit.collider.CompareTag("Interactable")))
        {
            Default.SetActive(false);
            Hand.SetActive(true);
        
            if(Input.GetMouseButtonDown(0))
            {
                BoxCollider bc = hit.collider as BoxCollider;      //Das kann fix besser gelöst werden
                if(bc != null)                                    // Mit anderem Collider sollte es auch gehen aber egal
                {                                                //  Nicht ganz safe                                    
                    if(bc.name == "solderingstation")
                    {
                        if(items.hasIC == true)
                        {
                            Debug.Log("IC has been soldered");
                            items.icParts++;
                            items.hasIC = false;
                        }
                        else
                        {
                            Debug.Log("no IC");
                        }
                    }
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