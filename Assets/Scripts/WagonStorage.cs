using UnityEngine;
using System; // For Action delegate

public class WagonStorage : MonoBehaviour
{
    public Transform holdPosition; // Position where the item will be held
    public Transform wagonStoragePoint; // Position in the wagon where items are stored
    public Transform generator; // Reference to the generator object
    public GameObject waterwheel; // Reference to the waterwheel object with the ScrollSpeed component
    private GameObject currentItem; // Currently held item
    private bool isHoldingItem = false; // State for whether the chicken is holding an item
    public float pickupRange = 2f; // Distance within which items can be picked up
    public float wagonRange = 5f; // Distance within which the wagon must be for item pickup
    public float generatorRange = 5f; // Distance within which the generator must be for item removal
    public GameObject wagon; // Reference to the wagon object
    public bool generatorBoolean = false;

    private ScrollSpeed waterwheelAnimation; // Reference to the ScrollSpeed script on the waterwheel

    [SerializeField] private GameObject itemInRangeCanvas; // Canvas to display when the chicken is near an item
    [SerializeField] private EngineerInteraction engineerInteraction; // Reference to the EngineerInteraction script

    // Event to notify generator state change
    public Action<bool> OnGeneratorStateChanged;

    void Start()
    {
        waterwheelAnimation = waterwheel.GetComponent<ScrollSpeed>();
        if (waterwheelAnimation == null)
        {
            Debug.LogError("ScrollSpeed component not found on the waterwheel.");
        }

        // Ensure the canvas is hidden initially
        if (itemInRangeCanvas != null)
        {
            itemInRangeCanvas.SetActive(false);
        }
        else
        {
            Debug.LogError("ItemInRangeCanvas reference is not assigned in the Inspector.");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (isHoldingItem)
            {
                if (IsWagonAndGeneratorInRange())
                {
                    DropItemIntoGenerator();
                }
                else if (IsInWagonCollider())
                {
                    PlaceItemInWagon();
                }
                else
                {
                    DropItem();
                }
            }
            else
            {
                if (IsGeneratorInRange())
                {
                    TryTakeItemFromWagon();
                }
                else
                {
                    TryPickUpItem();
                }
            }
        }

        if (isHoldingItem && !IsWagonInRange())
        {
            DropItem();
        }

        HandleItemInRangeCanvas(); // Check and handle canvas visibility
    }

    private void HandleItemInRangeCanvas()
    {
        // Check if the player has completed all engineer interactions
        bool hasCompletedInteractions = engineerInteraction != null && engineerInteraction.HasCompletedAllInteractions();

        if (!hasCompletedInteractions)
        {
            // Hide the canvas if interactions are incomplete
            if (itemInRangeCanvas != null)
            {
                itemInRangeCanvas.SetActive(false);
            }
            return;
        }

        // Check if an item is within range
        bool isItemInRange = false;
        Collider[] colliders = Physics.OverlapSphere(transform.position, pickupRange);

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Item"))
            {
                isItemInRange = true;
                break;
            }
        }

        // Toggle the canvas based on proximity to an item
        if (itemInRangeCanvas != null)
        {
            itemInRangeCanvas.SetActive(isItemInRange);
        }
    }

    private void TryPickUpItem()
    {
        // Check if the player has completed all interactions with the engineer
        if (engineerInteraction != null && !engineerInteraction.HasCompletedAllInteractions())
        {
            Debug.Log("You must complete the engineer interaction before picking up items.");
            return;
        }

        if (!IsWagonInRange())
        {
            Debug.Log("Wagon is not in range. Cannot pick up item.");
            return;
        }

        Collider[] colliders = Physics.OverlapSphere(transform.position, pickupRange);

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Item"))
            {
                currentItem = collider.gameObject;
                PickUpItem(currentItem);
                return;
            }
        }

        Debug.Log("No valid item found to pick up.");
    }

    private bool IsWagonInRange()
    {
        if (wagon == null)
        {
            Debug.LogError("Wagon reference is not assigned in the Inspector.");
            return false;
        }

        float distanceToWagon = Vector3.Distance(transform.position, wagon.transform.position);
        return distanceToWagon <= wagonRange;
    }

    private bool IsWagonAndGeneratorInRange()
    {
        return IsWagonInRange() && IsGeneratorInRange();
    }

    private bool IsInWagonCollider()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, pickupRange);

        foreach (Collider collider in colliders)
        {
            if (collider.gameObject == wagon)
            {
                return true;
            }
        }

        return false;
    }

    private bool IsGeneratorInRange()
    {
        if (generator == null)
        {
            Debug.LogError("Generator reference is not assigned in the Inspector.");
            return false;
        }

        float distanceToGenerator = Vector3.Distance(transform.position, generator.position);
        return distanceToGenerator <= generatorRange;
    }

    private void PickUpItem(GameObject item)
    {
        isHoldingItem = true;

        item.transform.position = holdPosition.position;
        item.transform.rotation = holdPosition.rotation;
        item.transform.SetParent(holdPosition);
        Debug.Log($"Picked up item: {item.name}");
    }

    private void DropItem()
    {
        if (currentItem != null)
        {
            isHoldingItem = false;

            currentItem.transform.SetParent(null);

            Vector3 dropPosition = transform.position + transform.forward;
            currentItem.transform.position = dropPosition;

            Debug.Log($"Dropped item: {currentItem.name} on the ground.");
            currentItem = null;
        }
    }

    private void DropItemIntoGenerator()
    {
        if (currentItem != null)
        {
            Debug.Log($"Dropped item: {currentItem.name} into the generator.");
            Destroy(currentItem);
            currentItem = null;
            isHoldingItem = false;

            if (waterwheelAnimation != null)
            {
                waterwheelAnimation.isActive = true;
                generatorBoolean = true;
                Debug.Log("Generator activated. Notifying listeners.");
                OnGeneratorStateChanged?.Invoke(generatorBoolean);
            }
        }
    }

    private void PlaceItemInWagon()
    {
        if (currentItem != null)
        {
            Debug.Log($"Placing item: {currentItem.name} in the wagon.");
            isHoldingItem = false;

            currentItem.transform.position = wagonStoragePoint.position;
            currentItem.transform.rotation = wagonStoragePoint.rotation;
            currentItem.transform.SetParent(wagonStoragePoint);

            currentItem = null;
        }
    }

    private void TryTakeItemFromWagon()
    {
        if (wagonStoragePoint.childCount > 0)
        {
            currentItem = wagonStoragePoint.GetChild(0).gameObject;

            Debug.Log($"Taking item: {currentItem.name} from the wagon.");
            PickUpItem(currentItem);
        }
        else
        {
            Debug.Log("No items in the wagon to take.");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == generator.gameObject && isHoldingItem)
        {
            DropItemIntoGenerator();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, pickupRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, wagonRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, generatorRange);
    }
}
