using UnityEngine;

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

    private ScrollSpeed waterwheelAnimation; // Reference to the ScrollSpeed script on the waterwheel

    void Start()
    {
        // Get the ScrollSpeed component from the waterwheel
        waterwheelAnimation = waterwheel.GetComponent<ScrollSpeed>();
        if (waterwheelAnimation == null)
        {
            Debug.LogError("ScrollSpeed component not found on the waterwheel.");
        }
        else
        {
            Debug.Log("ScrollSpeed component successfully linked to the waterwheel.");
        }
    }

    void Update()
    {
        // Detect when the "Q" key is pressed
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (isHoldingItem)
            {
                if (IsWagonAndGeneratorInRange())
                {
                    Debug.Log("Wagon and generator are both in range. Dropping the item into the generator.");
                    DropItemIntoGenerator(); // Directly drop the item into the generator
                }
                else if (IsInWagonCollider())
                {
                    Debug.Log("Chicken is in wagon's collider. Placing the item in the wagon.");
                    PlaceItemInWagon(); // Place the item in the wagon if in range
                }
                else
                {
                    Debug.Log("Dropping the item on the ground.");
                    DropItem(); // Drop the item otherwise
                }
            }
            else
            {
                if (IsGeneratorInRange())
                {
                    Debug.Log("Chicken is in range of the generator. Attempting to take an item from the wagon.");
                    TryTakeItemFromWagon(); // Try to take an item from the wagon
                }
                else
                {
                    Debug.Log("Attempting to pick up an item.");
                    TryPickUpItem(); // Attempt to pick up an item
                }
            }
        }

        // Automatically drop the item if it is taken out of range
        if (isHoldingItem && !IsWagonInRange())
        {
            Debug.Log("Chicken moved out of range of the wagon. Dropping the item automatically.");
            DropItem();
        }
    }

    private void TryPickUpItem()
    {
        // Check if the wagon is within range
        if (!IsWagonInRange())
        {
            Debug.Log("Wagon is not in range. Cannot pick up item.");
            return;
        }

        // Find all nearby colliders within the pickup range
        Collider[] colliders = Physics.OverlapSphere(transform.position, pickupRange);

        foreach (Collider collider in colliders)
        {
            // Check if the collider belongs to an item (by tag)
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

        // Attach the item to the chicken's hold position
        item.transform.position = holdPosition.position;
        item.transform.rotation = holdPosition.rotation;
        item.transform.SetParent(holdPosition); // Make the item a child of the hold position
        Debug.Log($"Picked up item: {item.name}");
    }

    private void DropItem()
    {
        if (currentItem != null)
        {
            isHoldingItem = false;

            // Detach the item from the chicken
            currentItem.transform.SetParent(null);

            // Place the item slightly in front of the chicken
            Vector3 dropPosition = transform.position + transform.forward;
            currentItem.transform.position = dropPosition;

            Debug.Log($"Dropped item: {currentItem.name} on the ground.");
            currentItem = null; // Clear the reference to the held item
        }
    }

    private void DropItemIntoGenerator()
    {
        if (currentItem != null)
        {
            Debug.Log($"Dropped item: {currentItem.name} into the generator.");
            Destroy(currentItem); // Make the item disappear
            currentItem = null; // Clear the reference
            isHoldingItem = false; // Update holding state

            // Activate the waterwheel animation
            if (waterwheelAnimation != null)
            {
                waterwheelAnimation.isActive = true;
                Debug.Log("Waterwheel animation activated.");
            }
        }
    }

    private void PlaceItemInWagon()
    {
        if (currentItem != null)
        {
            Debug.Log($"Placing item: {currentItem.name} in the wagon.");
            isHoldingItem = false;

            // Place the item at the wagon's storage point
            currentItem.transform.position = wagonStoragePoint.position;
            currentItem.transform.rotation = wagonStoragePoint.rotation;

            // Make the item a child of the wagon's storage point
            currentItem.transform.SetParent(wagonStoragePoint);

            currentItem = null; // Clear the reference to the held item
        }
    }

    private void TryTakeItemFromWagon()
    {
        if (wagonStoragePoint.childCount > 0)
        {
            // Take the first child from the wagon storage point
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
        // Check if the chicken collides with the generator while holding an item
        if (collision.gameObject == generator.gameObject && isHoldingItem)
        {
            DropItemIntoGenerator();
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize the pickup range, wagon range, and generator range in the editor
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, pickupRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, wagonRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, generatorRange);
    }
}
