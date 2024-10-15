using UnityEngine;
using UnityEngine.UI;  // Add this to use the UI elements
using TMPro;

public class SignController : MonoBehaviour
{
    public float AnimationSpeed = 1;
    public float openDistance = 0.5f;

    private CharacterSwap characterSwap;  // Reference to the CharacterSwap script
    private Transform player;  // Reference to the active player's transform
    private bool isPlayerNear = false;

    // Reference to the UI Text for door interaction
    public TextMeshProUGUI interactionText;

    // Reference to the UI Image that will show the sign
    public Image signImage;

    private bool isSignActive = false;  // Tracks if the sign is currently being displayed

    void Start()
    {
        // Find the CharacterSwap script in the scene
        characterSwap = FindObjectOfType<CharacterSwap>();

        // Hide the interaction text and sign image at the start
        if (interactionText != null)
        {
            interactionText.gameObject.SetActive(false);
        }
        if (signImage != null)
        {
            signImage.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        // Get the current active player from CharacterSwap
        if (characterSwap != null)
        {
            player = characterSwap.GetActiveCharacter().transform;
        }

        if (player != null)
        {
            CheckPlayerDistance();

            if (isPlayerNear && Input.GetKeyDown(KeyCode.E))
            {
                ToggleSign();
            }
        }
    }

    void CheckPlayerDistance()
    {
        float distance = Vector3.Distance(player.position, transform.position);

        if (distance <= openDistance)
        {
            isPlayerNear = true;

            // Show the interaction text
            if (interactionText != null)
            {
                interactionText.gameObject.SetActive(true);
            }
        }
        else
        {
            isPlayerNear = false;

            // Hide the interaction text when the player is out of range
            if (interactionText != null)
            {
                interactionText.gameObject.SetActive(false);
            }

            // If the player moves away while the sign is active, exit the sign
            if (isSignActive)
            {
                ExitSign();
            }
        }
    }

    public void ToggleSign()
    {
        if (isSignActive)
            ExitSign();
        else
            EnterSign();
    }

    private void EnterSign()
    {
        // Display the sign image
        if (signImage != null)
        {
            signImage.gameObject.SetActive(true);
        }

        // Disable player movement
        if (characterSwap != null)
        {
            var activeCharacter = characterSwap.GetActiveCharacter();
            if (activeCharacter != null)
            {
                var movement = activeCharacter.GetComponent<IPlayerMovement>();
                if (movement != null)
                {
                    movement.canMove = false; // Lock movement
                }
            }
        }

        isSignActive = true;
    }

    private void ExitSign()
    {
        // Hide the sign image
        if (signImage != null)
        {
            signImage.gameObject.SetActive(false);
        }

        // Re-enable player movement
        if (characterSwap != null)
        {
            var activeCharacter = characterSwap.GetActiveCharacter();
            if (activeCharacter != null)
            {
                var movement = activeCharacter.GetComponent<IPlayerMovement>();
                if (movement != null)
                {
                    movement.canMove = true; // Unlock movement
                }
            }
        }

        isSignActive = false;
    }
}
