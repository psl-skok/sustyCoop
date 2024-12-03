using System.Collections;
using UnityEngine;

public class ButterflyMovement : MonoBehaviour
{
    public float speed = 3f;          // Movement speed
    public float hoverHeight = 1f;   // Height above flowers
    private Transform targetFlower;  // Current target flower
    private bool isHovering = false; // Prevent movement during hovering

    void Start()
    {
        ChooseNewFlower();
    }

    void Update()
    {
        if (targetFlower != null && !isHovering)
        {
            // Move towards the flower
            Vector3 targetPosition = targetFlower.position + Vector3.up * hoverHeight;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

            // Check if we've reached the flower
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                StartCoroutine(HoverAndMove());
            }
        }
    }

    private IEnumerator HoverAndMove()
    {
        isHovering = true; // Set state to hovering
        // Hover at the flower for a few seconds
        yield return new WaitForSeconds(Random.Range(2f, 5f));
        ChooseNewFlower();
        isHovering = false; // Resume movement after choosing new flower
    }

    private void ChooseNewFlower()
    {
        // Find all flowers in the scene
        GameObject[] flowers = GameObject.FindGameObjectsWithTag("Flower");

        if (flowers.Length > 0)
        {
            // Pick a random flower, ensuring it's different from the current target
            Transform newTarget;
            do
            {
                newTarget = flowers[Random.Range(0, flowers.Length)].transform;
            } while (newTarget == targetFlower && flowers.Length > 1);

            targetFlower = newTarget;
        }
    }
}
