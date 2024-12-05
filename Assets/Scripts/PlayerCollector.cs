using UnityEngine;
using TMPro;
using System.Collections;

public class PlayerCollector : MonoBehaviour
{
    public Transform compostBin1;
    public Transform compostBin2;
    public int wasteCollected = 0;
    private GameObject currentObject; // Currently carried object (waste or watering can)
    private bool isNearWaste = false;
    private bool isNearTrash = false;
    private bool isNearWell = false;
    private bool isNearPlant = false;
    public Camera playerCamera; 
    private GameObject successCanvas;
    private bool isCarryingObject = false;
    
    public WellInteraction wellInteraction; 
    private Plant currentPlant;

    public static event System.Action<PlayerCollector> OnPlayerBucketCreated;

    void Awake()
    {
        OnPlayerBucketCreated?.Invoke(this);
    }

    void OnEnable()
    {
        successCanvas = GameObject.Find("SuccessCanvas");
        if (successCanvas != null)
        {
            successCanvas.SetActive(false); // Hide the success canvas initially
        }
        else
        {
            Debug.LogError("SuccessCanvas not found in the scene.");
        }
    }

    void Start()
    {
        compostBin1 = GameObject.Find("CompostBin1Location").transform;
        compostBin2 = GameObject.Find("CompostBin2Location").transform;

        if (compostBin1 == null || compostBin2 == null)
        {
            Debug.LogError("Compost bins not found in the scene. Please ensure the objects are named correctly.");
        }

        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }

        if (wellInteraction == null)
        {
            wellInteraction = FindObjectOfType<WellInteraction>(); // Make sure the well interaction script is found
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Waste"))
        {
            isNearWaste = true;
            currentObject = other.gameObject;
        }
        else if (other.CompareTag("TrashBin"))
        {
            isNearTrash = true;
        }
        else if (other.CompareTag("WateringCan"))
        {
            isNearWaste = true; // Treat watering can like waste for now
            currentObject = other.gameObject;
        }
        else if (other.CompareTag("Well"))
        {
            isNearWell = true;
        }
        else if (other.CompareTag("Flower"))
        {
            isNearPlant = true;
            currentPlant = other.GetComponent<Plant>(); // Get the plant script reference
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Waste") || other.CompareTag("WateringCan"))
        {
            isNearWaste = false;
        }
        else if (other.CompareTag("TrashBin"))
        {
            isNearTrash = false;
        }
        else if (other.CompareTag("Well"))
        {
            isNearWell = false;
        }
        else if (other.CompareTag("Flower"))
        {
            isNearPlant = false;
            currentPlant = null;
        }
    }

    void Update()
    {
        if (wasteCollected == 1)
        {
            if (successCanvas != null)
            {
                successCanvas.SetActive(true);
            }
            
            if (wellInteraction != null)
            {
                wellInteraction.CleanWell(); // Change well to cleaned state
            }

            StartCoroutine(HideSuccessMessageAfterTime());
        }

        if (isNearWaste && Input.GetKeyDown(KeyCode.E) && !isCarryingObject)
        {
            PickUpObject();
        }

        if (Input.GetKeyDown(KeyCode.E) && isNearTrash && currentObject != null && currentObject.CompareTag("Waste"))
        {
            DepositWaste();
        }

        if (Input.GetKeyDown(KeyCode.E) && isNearWell && currentObject != null && currentObject.CompareTag("WateringCan"))
        {
            FillWateringCan();
        }

        if (isNearPlant && Input.GetKeyDown(KeyCode.E) && currentObject != null && currentObject.CompareTag("WateringCan"))
        {
            if (currentObject.GetComponent<WateringCan>().isFilled)
            {
                WaterPlant();
            }
        }
    }

    void PickUpObject()
    {
        if (isCarryingObject)
        {
            Debug.LogWarning("You are already carrying an object! Deposit it before picking up another.");
            return;
        }

        isCarryingObject = true;
        currentObject.transform.SetParent(transform);
        currentObject.GetComponent<Rigidbody>().isKinematic = true;
        Vector3 forwardPosition;

        // Adjust the position based on the object's tag
        if (currentObject.CompareTag("Waste"))
        {
            forwardPosition = playerCamera.transform.position + playerCamera.transform.forward * 1.0f + Vector3.up * 0.5f;
        }
        else if (currentObject.CompareTag("WateringCan"))
        {
            forwardPosition = playerCamera.transform.position + playerCamera.transform.forward * 0.5f + Vector3.down * 0.7f;

            // Set local rotation for watering can
            Quaternion targetRotation = Quaternion.Euler(50f, 0f, 0f); // Rotate by 20 degrees on X axis
            currentObject.transform.localRotation = targetRotation;

            Debug.Log("Watering Can Rotation: " + currentObject.transform.localRotation.eulerAngles);

        }
        else
        {
            Debug.LogWarning("Unhandled object tag: " + currentObject.tag);
            return;
        }

        // Apply the calculated position and rotation
        currentObject.transform.position = forwardPosition;
        currentObject.transform.rotation = Quaternion.LookRotation(playerCamera.transform.forward);

        // Reset interaction flags
        isNearWaste = false;
        isNearTrash = false;
    }

    void DepositWaste()
    {
        wasteCollected++;
        currentObject.SetActive(false);
        currentObject = null;
        isCarryingObject = false;
    }

    void FillWateringCan()
    {
        WateringCan canScript = currentObject.GetComponent<WateringCan>();
        if (canScript != null && !canScript.isFilled)
        {
            canScript.FillWithWater();
            Debug.Log("Watering can filled with water!");
        }
    }

    void WaterPlant()
    {
        Debug.Log("WaterPlant method called");
        if (currentPlant != null)
        {
            currentPlant.WaterPlant(); // Call WaterPlant method to bloom the plant
        }
    }

    IEnumerator HideSuccessMessageAfterTime()
    {
        yield return new WaitForSeconds(5f); // Adjust duration as needed
        if (successCanvas != null)
        {
            successCanvas.SetActive(false);
        }
    }
}
