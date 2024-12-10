using UnityEngine;
using TMPro;
using System.Collections;

public class PlayerCollector : MonoBehaviour
{
    //general Global Variables
    private GameObject currentObject; // Currently carried object 
    public Camera playerCamera; 
    private bool isCarryingObject = false;

    //Global Variables for PigPen
    public Transform compostBin1;
    public Transform compostBin2;
    public int wasteCollected = 0; 
    private bool isNearWaste = false;
    private bool isNearTrash = false;
    public GameObject pigSuccessCanvas;
    public WellInteraction wellInteraction; 
    private Plant currentPlant;
    private WateringCan wateringCan;

    //Global Variables for Garden
    private bool isNearWell = false;
    private bool isNearPlant = false;
    private bool isNearWaterCanSpawn = false;
    private bool allFlowersWatered = false;
    public GameObject flowerWinCanvas;

    //Global Variables for WaterTower
    private bool isNearValve = false;
    private bool isNearValveSlot = false;
    private Valve valve;

    public static event System.Action<PlayerCollector> OnPlayerCollectorCreated;

    void Awake()
    {
        OnPlayerCollectorCreated?.Invoke(this);
    }

    void OnEnable()
    {
        pigSuccessCanvas = GameObject.Find("PigSuccessCanvas");
        flowerWinCanvas = GameObject.Find("FlowerWinCanvas");
        if (pigSuccessCanvas != null && flowerWinCanvas != null)
        {
            pigSuccessCanvas.SetActive(false); // Hide the success canvas initially
            flowerWinCanvas.SetActive(false);
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
            isNearWaste = true; 
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
        else if (other.CompareTag("WaterCanSpawn"))
        {
            isNearWaterCanSpawn = true;
        }
        else if (other.CompareTag("Valve"))
        {
            isNearValve = true;
            currentObject = other.gameObject;
        }
        else if (other.CompareTag("ValveSlot"))
        {
            isNearValveSlot = true;
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
        else if (other.CompareTag("WaterCanSpawn"))
        {
            isNearWaterCanSpawn = false;;
        }
        else if (other.CompareTag("Valve"))
        {
            isNearValve = false;
        }
        else if (other.CompareTag("ValveSlot"))
        {
            isNearValveSlot = false;
        }
    }

    void Update()
    {
        if (wasteCollected == 9)
        {
            if (pigSuccessCanvas != null)
            {
                pigSuccessCanvas.SetActive(true);
            }
            
            if (wellInteraction != null)
            {
                wellInteraction.CleanWell(); // Change well to cleaned state
            }

        }
        if ((isNearWaste || isNearValve) && Input.GetKeyDown(KeyCode.E) && !isCarryingObject)
        {
            PickUpObject();
        }
        if(isNearWaterCanSpawn && Input.GetKeyDown(KeyCode.E) && currentObject.CompareTag("WateringCan") && allFlowersWatered == true){
            Debug.Log("Attempting to return");
            ReturnWaterCan();
        }

        if (Input.GetKeyDown(KeyCode.E) && isNearTrash && currentObject != null && currentObject.CompareTag("Waste"))
        {
            DepositWaste();
        }

        if (Input.GetKeyDown(KeyCode.E) && isNearWell && currentObject != null && currentObject.CompareTag("WateringCan"))
        {
            FillWateringCan();
        }

        if (Input.GetKeyDown(KeyCode.E) && currentObject != null && currentObject.CompareTag("WateringCan"))
        {
            PourCan();
        }

        if (isNearPlant && Input.GetKeyDown(KeyCode.E) && currentObject != null && currentObject.CompareTag("WateringCan"))
        {
            if (currentObject.GetComponent<WateringCan>().isFilled)
            {
                WaterPlant();
            }
        }

        if (isNearValveSlot && Input.GetKeyDown(KeyCode.E) && currentObject != null && currentObject.CompareTag("Valve"))
        {
            InsertValve();
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
            forwardPosition = playerCamera.transform.position + playerCamera.transform.forward * 1.0f + Vector3.up * 0.2f;
            isNearWaste = false;
        }
        else if (currentObject.CompareTag("WateringCan"))
        {
            wateringCan = currentObject.GetComponent<WateringCan>();
            forwardPosition = playerCamera.transform.position + playerCamera.transform.forward * 0.5f + Vector3.down * 0.7f;

            // Set local rotation for watering can
            Quaternion targetRotation = Quaternion.Euler(50f, 0f, 0f); // Rotate by 20 degrees on X axis
            currentObject.transform.localRotation = targetRotation;
        }
        else if (currentObject.CompareTag("Valve"))
        {
            valve = currentObject.GetComponent<Valve>();
            forwardPosition = playerCamera.transform.position + playerCamera.transform.forward * 1.0f + Vector3.up * 0.1f;
            isNearValve = false;
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
        isNearValve = false;
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
        }
    }

    void WaterPlant()
    {
        int numWateredPlants = 0;
        if (currentPlant != null)
        {
            currentPlant.WaterPlant(); // Call WaterPlant method to bloom the plant
            numWateredPlants++;
        }
        if(numWateredPlants == 12){
            allFlowersWatered = true;
            flowerWinCanvas.SetActive(true);
            Debug.Log(allFlowersWatered);
        }
    }

    void PourCan()
    {
        wateringCan.PourCan(); // Call PourCan method to trigger animation
    }

    void ReturnWaterCan()
    {
        currentObject.SetActive(false);

    }

    void InsertValve()
    {
        if (valve != null)
        {
            valve.InsertValve();
            isCarryingObject = false; // Player is no longer carrying the valve
            currentObject = null;
        }
    }
}
