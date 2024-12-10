using UnityEngine;

public class WagonInteraction : MonoBehaviour
{
    public GameObject promptCanvasPrefab; // UI prompt prefab
    private GameObject promptCanvasInstance; // Instance of the prompt canvas
    private Transform cowTransform; // Reference to the "CowBIW" transform
    private bool isCowNearby = false; // Tracks if the "CowBIW" is near the wagon
    private bool isWagonConnected = false; // Tracks if the wagon is connected to the "CowBIW"

    public string playerTag = "Player"; // Tag for player objects
    public string cowName = "CowBIW"; // Specific name for the cow

    void Start()
    {
        if (promptCanvasPrefab != null)
        {
            promptCanvasInstance = Instantiate(promptCanvasPrefab, transform);
            promptCanvasInstance.transform.localPosition = new Vector3(0, 2, 0); // Position it above the wagon
            promptCanvasInstance.SetActive(false); // Ensure canvas starts hidden
        }
    }

    void Update()
    {
        if (isCowNearby && cowTransform != null && cowTransform.gameObject.name == "CowBIW" && Input.GetKeyDown(KeyCode.E))
        {
            if (isWagonConnected)
            {
                DisconnectWagon();
            }
            else
            {
                ConnectWagon();
            }
        }
    }

    private void TogglePromptCanvas(bool state)
    {
        if (promptCanvasInstance != null)
        {
            promptCanvasInstance.SetActive(state);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(playerTag) && other.gameObject.name == cowName)
        {
            cowTransform = other.transform;
            isCowNearby = true;

            if (!isWagonConnected)
            {
                TogglePromptCanvas(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(playerTag) && other.gameObject.name == cowName)
        {
            cowTransform = null;
            isCowNearby = false;
            TogglePromptCanvas(false);
        }
    }

    private void ConnectWagon()
    {
        transform.SetParent(cowTransform);
        transform.localPosition = new Vector3(0, 0.5f, -2);
        transform.localRotation = Quaternion.Euler(0, -90, 0);
        isWagonConnected = true;
        TogglePromptCanvas(false);
    }

    private void DisconnectWagon()
    {
        transform.SetParent(null);
        isWagonConnected = false;
    }
}
