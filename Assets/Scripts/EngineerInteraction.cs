using UnityEngine;

public class EngineerInteraction : MonoBehaviour
{
    public GameObject promptCanvas; // Assign the panel GameObject in the Inspector
    public GameObject infoCanvas;
    public GameObject infoCanvas2;
    public GameObject infoCanvas3; // The new canvas
    private bool isPlayerNearby = false;
    private bool isInfoCanvasActive = false;
    private bool isInfoCanvas2Active = false;
    private bool isInfoCanvas3Active = false;
    private bool hasCompletedAllInteractions = false; // Flag to track if all messages have been shown
    public AudioSource audioSource;

    void Start()
    {
        // Ensure all canvases are hidden initially
        promptCanvas.SetActive(false);
        infoCanvas.SetActive(false);
        infoCanvas2.SetActive(false);
        infoCanvas3.SetActive(false);
    }

    void Update()
    {
        // Check if the player is nearby and presses 'E'
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            if (!hasCompletedAllInteractions)
            {
                HandleCanvasInteractions();
                PlaySound();
            }
        }
    }

    private void HandleCanvasInteractions()
    {
        if (!isInfoCanvasActive && !isInfoCanvas2Active && !isInfoCanvas3Active)
        {
            // Show the first info canvas
            promptCanvas.SetActive(false);
            infoCanvas.SetActive(true);
            isInfoCanvasActive = true;
        }
        else if (isInfoCanvasActive && !isInfoCanvas2Active && !isInfoCanvas3Active)
        {
            // Transition to the second info canvas
            infoCanvas.SetActive(false);
            infoCanvas2.SetActive(true);
            isInfoCanvas2Active = true;
        }
        else if (isInfoCanvasActive && isInfoCanvas2Active && !isInfoCanvas3Active)
        {
            // Transition to the third info canvas
            infoCanvas2.SetActive(false);
            infoCanvas3.SetActive(true);
            isInfoCanvas3Active = true;
        }
        else if (isInfoCanvasActive && isInfoCanvas2Active && isInfoCanvas3Active)
        {
            // All messages have been interacted with
            infoCanvas3.SetActive(false);
            hasCompletedAllInteractions = true;
            Debug.Log("All messages have been shown. Interaction completed.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player enters the trigger area
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            if (!hasCompletedAllInteractions && !isInfoCanvasActive && !isInfoCanvas2Active && !isInfoCanvas3Active)
            {
                promptCanvas.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Reset when the player leaves the trigger area
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;

            // Hide prompt but do not reset completed interactions
            promptCanvas.SetActive(false);

            if (!hasCompletedAllInteractions)
            {
                infoCanvas.SetActive(false);
                infoCanvas2.SetActive(false);
                infoCanvas3.SetActive(false);
                isInfoCanvasActive = false;
                isInfoCanvas2Active = false;
                isInfoCanvas3Active = false;
            }
        }
    }

    public bool HasCompletedAllInteractions()
    {
        return hasCompletedAllInteractions;
    }

    void PlaySound()
    {
        if (audioSource != null)
        {
            audioSource.Play(); //Plays audio sound
        }
    }
}
