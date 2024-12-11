using UnityEngine;

public class StableInteraction : MonoBehaviour
{
    [SerializeField] private CharacterSwap characterSwap; // Reference to the CharacterSwap script
    [SerializeField] private GameObject chickenFirstPrompt; // First prompt for the chicken
    [SerializeField] private GameObject chickenSecondPrompt; // Second prompt for the chicken
    [SerializeField] private GameObject cowFirstPrompt; // First prompt for the cow
    [SerializeField] private GameObject cowSecondPrompt; // Second prompt for the cow
    [SerializeField] private float interactionRange = 5f; // Range within which interaction is possible

    private GameObject activeCharacter; // Reference to the currently active character
    private GameObject lastActiveCharacter; // Reference to the previously active character
    private bool isFirstPromptActive = false; // Track if the first prompt is active
    private bool isSecondPromptShown = false; // Track if the second prompt has been shown

    void Start()
    {
        // Ensure all prompts are hidden initially
        chickenFirstPrompt.SetActive(false);
        chickenSecondPrompt.SetActive(false);
        cowFirstPrompt.SetActive(false);
        cowSecondPrompt.SetActive(false);

        // Initialize active and last active characters
        activeCharacter = characterSwap.GetActiveCharacter();
        lastActiveCharacter = activeCharacter;
    }

    void Update()
    {
        activeCharacter = characterSwap.GetActiveCharacter();

        // If the active character has changed, reset all prompts
        if (activeCharacter != lastActiveCharacter)
        {
            ResetPrompts();
            lastActiveCharacter = activeCharacter; // Update the last active character
        }

        // Check if the active character is within range
        if (IsCharacterInRange())
        {
            if (!isFirstPromptActive)
            {
                ShowFirstPrompt();
            }

            // Check if E is pressed and the second prompt hasn't been shown
            if (Input.GetKeyDown(KeyCode.E) && isFirstPromptActive && !isSecondPromptShown)
            {
                ShowSecondPrompt();
            }
        }
        else
        {
            // Reset prompts if the character leaves the range
            ResetPrompts();
        }
    }

    private bool IsCharacterInRange()
    {
        // Calculate distance between the active character and the stable
        return Vector3.Distance(activeCharacter.transform.position, transform.position) <= interactionRange;
    }

    private void ShowFirstPrompt()
    {
        if (activeCharacter == characterSwap.character1) // Chicken
        {
            chickenFirstPrompt.SetActive(true);
        }
        else if (activeCharacter == characterSwap.character2) // Cow
        {
            cowFirstPrompt.SetActive(true);
        }

        isFirstPromptActive = true;
    }

    private void ShowSecondPrompt()
    {
        if (activeCharacter == characterSwap.character1) // Chicken
        {
            chickenFirstPrompt.SetActive(false);
            chickenSecondPrompt.SetActive(true);
        }
        else if (activeCharacter == characterSwap.character2) // Cow
        {
            cowFirstPrompt.SetActive(false);
            cowSecondPrompt.SetActive(true);
        }

        isSecondPromptShown = true;
    }

    private void ResetPrompts()
    {
        // Hide all prompts and reset states
        chickenFirstPrompt.SetActive(false);
        chickenSecondPrompt.SetActive(false);
        cowFirstPrompt.SetActive(false);
        cowSecondPrompt.SetActive(false);

        isFirstPromptActive = false;
        isSecondPromptShown = false;
    }
}
