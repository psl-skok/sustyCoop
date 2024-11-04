using UnityEngine;

public class CharacterSwap : MonoBehaviour
{
    public GameObject character1;      // Reference to the first character
    public GameObject character2;      // Reference to the second character
    public Camera camera1;             // Camera attached to character1
    public Camera camera2;             // Camera attached to character2

    private GameObject activeCharacter;  // The currently active character
    private Camera activeCamera;         // The currently active camera
    
    private IPlayerMovement character1Movement;  // Reference to character1's movement script
    private IPlayerMovement character2Movement;  // Reference to character2's movement script

    void Start()
    {
        character1Movement = character1.GetComponent<IPlayerMovement>();
        character2Movement = character2.GetComponent<IPlayerMovement>();

        character1Movement.canMove = true;
        character2Movement.canMove = false;

        // Set initial active character and camera
        activeCharacter = character1;
        activeCamera = camera1;

        camera1.enabled = true;
        camera2.enabled = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SwapCharacter();
        }
    }

    void SwapCharacter()
    {
        if (character1Movement.canMove)
        {
            activeCharacter = character2;
            activeCamera = camera2;

            camera1.enabled = false;
            camera2.enabled = true;

            character1Movement.canMove = false;
            character2Movement.canMove = true;
        }
        else
        {
            activeCharacter = character1;
            activeCamera = camera1;

            camera1.enabled = true;
            camera2.enabled = false;

            character1Movement.canMove = true;
            character2Movement.canMove = false;
        }

        Debug.Log("Switched to: " + activeCharacter.name);
    }

    public GameObject GetActiveCharacter()
    {
        return activeCharacter;  // Return the currently active character
    }
}
