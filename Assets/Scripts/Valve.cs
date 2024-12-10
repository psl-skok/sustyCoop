using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Valve : MonoBehaviour
{
    public GameObject valve;
    public GameObject valveSlot;
    public GameObject waterStream;
    public GameObject river;
    private bool isInSlot = false;
    private bool isNearPlayer = false;

    void Start()
    {
        waterStream.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player")){
            isNearPlayer = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player")){
            isNearPlayer = false;
        }
    }

    void Update()
    {
        if (isNearPlayer && Input.GetKeyDown(KeyCode.E) && isInSlot == true)
        {
            SpinValve();
        }
    }

    public void SpinValve()
    {
        Animator valveAnimator = valve.GetComponent<Animator>();
        if (valveAnimator != null)
        {
            valveAnimator.SetTrigger("Spin"); 
        }

        Animator riverAnimator = river.GetComponent<Animator>();
        if (riverAnimator != null)
        {
            riverAnimator.SetTrigger("Rise"); 
        }
        waterStream.SetActive(true);
    }

    public void InsertValve()
    {
        // Position the valve at the slot and lock it
        valve.transform.position = valveSlot.transform.position + Vector3.left * 0.1f; // Assuming slotPosition is predefined
        valve.transform.rotation = valveSlot.transform.rotation;
        valve.transform.SetParent(valveSlot.transform);

        // Set it as "in slot"
        isInSlot = true;

        // Disable Rigidbody to prevent physics interactions
        GetComponent<Rigidbody>().isKinematic = true;
    }

    
}
