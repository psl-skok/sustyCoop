using UnityEngine;
using System.Collections;


public class PlayerBucket : MonoBehaviour
{
    public Transform compostBin1; // Drop-off point
    public Transform compostBin2;
    public int wasteCollected = 0;
    private GameObject currentWaste;
    private bool isNearWaste = false;
    private bool isNearTrash = false;
    public Camera playerCamera; // Reference to the player’s camera
    public GameObject successCanvas;

    void Start()
    {
        // Find the gameObjects by their names in the scene
        compostBin1 = GameObject.Find("CompostBin1Location").transform;
        compostBin2 = GameObject.Find("CompostBin2Location").transform;
        successCanvas = GameObject.Find("SuccessCanvas");

        if (compostBin1 == null || compostBin2 == null)
        {
            Debug.LogError("Compost bins not found in the scene. Please ensure the objects are named correctly.");
        }

        // Find the player's camera if not already assigned
        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Waste"))
        {
            isNearWaste = true;
            currentWaste = other.gameObject;
        }
        if (other.CompareTag("TrashBin"))
        {
            isNearTrash = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Waste"))
        {
            isNearWaste = false;
        }
        if (other.CompareTag("TrashBin"))
        {
            isNearTrash = false;
        }
    }

    void Update()
    {
        if(wasteCollected == 1){
            successCanvas.SetActive(true);
            wasteCollected--;
            StartCoroutine(HideSuccessMessageAfterTime());
        }
        // Check if the player presses 'E' to pick up waste
        if (isNearWaste && Input.GetKeyDown(KeyCode.E))
        {
            PickUpWaste();
        }

        // Drop waste at compost bin if bucket is full
        if (Input.GetKeyDown(KeyCode.E) && isNearTrash && currentWaste != null)
        {
            wasteCollected++;
            currentWaste.SetActive(false);
            currentWaste = null;
        }
    }

    void PickUpWaste()
    {
        // Make the current waste a child of the player so it moves with them
        currentWaste.transform.SetParent(transform);

        // Disable physics on the waste object so it stays fixed in front of the player
        currentWaste.GetComponent<Rigidbody>().isKinematic = true;

        // Position the waste object in front of the player's camera
        Vector3 forwardPosition = playerCamera.transform.position + playerCamera.transform.forward * 1.0f + Vector3.up * 0.5f;
        currentWaste.transform.position = forwardPosition;
        currentWaste.transform.rotation = Quaternion.LookRotation(playerCamera.transform.forward);

        // Mark as no longer near waste (prevents picking up again until near new waste)
        isNearWaste = false;
    }

    IEnumerator HideSuccessMessageAfterTime()
    {
        yield return new WaitForSeconds(10f); // Wait for the specified time
        successCanvas.SetActive(false); // Deactivate the canvas from PlayerBucket
    }
}
