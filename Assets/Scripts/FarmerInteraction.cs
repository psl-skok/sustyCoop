using UnityEngine;
using TMPro;
public class FarmerInteraction : MonoBehaviour
{
    public GameObject promptCanvas; // Assign the panel GameObject in the Inspector
    private bool isPlayerNearby = false;
    void Start()
    {
        // Ensure the text panel is hidden initially
        promptCanvas.SetActive(false);
    }
    private void TogglePromptCanvas()
    {
        // Toggle the visibility of the text panel
        promptCanvas.SetActive(true);
    }
    private void OnTriggerEnter(Collider other)
    {
        // Check if the player enters the sign's trigger area
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            // Optionally display a hint like "Press E to interact"
            TogglePromptCanvas();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        // Reset when the player leaves the trigger area
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            promptCanvas.SetActive(false);
        }
    }
}